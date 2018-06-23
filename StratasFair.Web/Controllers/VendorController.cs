using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Front;
using StratasFair.Business.Front;
using StratasFair.Common;
using StratasFair.BusinessEntity;
using StratasFair.Web.App_Start;
using System.Net;
using System.IO;
namespace StratasFair.Web.Controllers
{
    public class VendorController : Controller
    {
        int MaxLength = 5; // in MB
        int MaxFileLength = 50; // in MB
        Encrypt64 enc = new Encrypt64();


        #region //  Vendor Code Starts  //

        // GET: VendorLogin
        public ActionResult Login()
        {
            VendorLoginModel _model = new VendorLoginModel();
            if (Request.Cookies["VendorCookie"] != null)
            {
                var authCookie = Request.Cookies["VendorCookie"];
                _model.EmailId = enc.Decrypt(authCookie.Values["VendorEmail"]);
                _model.Password = enc.Decrypt(authCookie.Values["VendorPassword"]);
                _model.RememberMe = true;
            }
            else
            {
                _model.RememberMe = false;
            }
            return View(_model);
        }

        [HttpPost]
        public ActionResult Login(VendorLoginModel _model)
        {
            var vendor = VendorHelper.Instance.GetVendorByEmail(_model.EmailId);
            if (vendor == null)
            {
                ModelState.AddModelError("EmailId", "Invalid Email Id");
            }
            else if (vendor.AdminApproval == 0)
            {
                TempData["Message"] = AppLogic.setFrontendMessage(1, "Your account is pending for approval. Please contact strataboard admin!");
            }
            else if (vendor.AdminApproval == 2)
            {
                TempData["Message"] = AppLogic.setFrontendMessage(1, "Your account has been rejected. Please contact strataboard admin!");
            }
            else if (vendor.Status != 1)
            {
                TempData["Message"] = AppLogic.setFrontendMessage(1, "Your account is deactive. Please contact strataboard admin!");
            }
            else
            {
                Encrypt64 enc = new Encrypt64();
                var pass = enc.Encrypt(_model.Password);
                var pass2 = enc.Decrypt(vendor.Password);
                if (vendor.Password == pass)
                {
                    Session["VendorId"] = vendor.VendorId;
                    Session["VendorEmailId"] = vendor.EmailId;
                    Session["VendorName"] = vendor.VendorName;
                    Session["VendorMobile"] = vendor.MobileNumber;
                    Session["VendorProfilePicture"] = vendor.ImageFile;
                    Session["VendorCreatedOn"] = vendor.CreatedOn;

                    HttpCookie cookie = new HttpCookie("VendorCookie");

                    if (_model.RememberMe)
                    {
                        cookie.Values.Add("VendorEmail", enc.Encrypt(vendor.EmailId));
                        cookie.Values.Add("VendorPassword", vendor.Password);
                        cookie.Expires = DateTime.Now.AddDays(365);
                    }
                    else
                    {
                        cookie.Expires = DateTime.Now.AddDays(-1d);
                    }
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid Password");
                }
            }
            return View(_model);
        }

        public ViewResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Registration(VendorModel _model, HttpPostedFileBase Image, HttpPostedFileBase CopyofTrade)
        {
            if (!CommonData.IsVendorEmailExists(_model.EmailId))
            {
                int _maxLength = MaxLength * 1024 * 1024;
                int _maxFileLength = MaxFileLength * 1024 * 1024;
                if (ModelState.ContainsKey("ImageFile")) ModelState["ImageFile"].Errors.Clear();
                var imageTypes = new string[] { "image/gif", "image/jpeg", "image/pjpeg", "image/png" };

                if (Image != null)
                {
                    if (!imageTypes.Contains(Image.ContentType))
                    {
                        ModelState.AddModelError("ImageFile", "Please choose either a GIF, JPG or PNG image.");
                    }
                    else if (Image.ContentLength > _maxLength)
                    {
                        ModelState.AddModelError("ImageFile", "Maximum allowed file size is " + MaxLength + " MB.");
                    }
                }

                if (ModelState.ContainsKey("TradeAndBusinessFile")) ModelState["TradeAndBusinessFile"].Errors.Clear();
                if (CopyofTrade != null)
                {
                    string AlllowedExtension = ".pdf";
                    List<string> extList = AlllowedExtension.Split(',').ToList<string>();
                    if (!extList.Contains(System.IO.Path.GetExtension(CopyofTrade.FileName)))
                    {
                        ModelState.AddModelError("TradeAndBusinessFile", "Please choose only " + AlllowedExtension + " file.");
                    }
                    else if (CopyofTrade.ContentLength > _maxFileLength)
                    {
                        ModelState.AddModelError("TradeAndBusinessFile", "Maximum allowed file size is " + MaxFileLength + " MB.");
                    }
                }


                //Check if other discipline is selected
                if (CommonData.GetDisciplineList().Where(x => x.Value == _model.DisciplineId.ToString()).FirstOrDefault().Text.ToLower() == "others")
                {
                    if (!string.IsNullOrEmpty(_model.OtherDisciplineName))
                    {
                        if (VendorHelper.Instance.IsDisciplineExists(_model.OtherDisciplineName))
                        {
                            ModelState.AddModelError("OtherDisciplineName", "Discipline already exists");
                        }
                        else
                        {
                            _model.DisciplineId = VendorHelper.Instance.AddOtherDiscipline(_model.OtherDisciplineName);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("OtherDisciplineName", "Other discipline name is required");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(_model.OtherDisciplineName))
                    {
                        _model.OtherDisciplineName = null;
                    }
                }

                long result = VendorHelper.Instance.AddNewVendor(_model, Image, CopyofTrade);


                if (result > 0)
                {
                    EmailSender.FncSend_StratasBoard_RegistrationMail_ToVendor(_model.VendorName, _model.EmailId);
                    TempData["Message"] = AppLogic.setFrontendMessage(0, "You have successfully registered on the StratasFair portal. Your details will be verified by our expert team and you will be notified with your login credentials shortly.");
                    return RedirectToAction("registration");
                }
                else if (result == -3)
                    TempData["Message"] = AppLogic.setFrontendMessage(1, "Email ID already exists. Registration Failed");
                else
                    TempData["Message"] = AppLogic.setFrontendMessage(-1, "Registration Failed");

            }
            else
            {
                ModelState.AddModelError("EmailId", "Email already exists.");
            }
            return View(_model);
        }

        public ViewResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(VendorModel _model)
        {
            var User = VendorHelper.Instance.GetVendorByEmail(_model.EmailId);
            if (User == null)
            {
                ModelState.AddModelError("EmailId", "Email Id does not exists. Please use valid Email Id");
            }
            else if (User.AdminApproval == 0)
            {
                ModelState.AddModelError("EmailId", "Your account is pending for approval. Please contact Stratasfair admin.");
            }
            else if (User.AdminApproval == 2)
            {
                ModelState.AddModelError("EmailId", "Your account has been rejected. Please contact Stratasfair admin.");
            }
            else if (User.Status != 1)
            {
                TempData["Message"] = AppLogic.setFrontendMessage(1, "Account Deactivated. Please contact Stratasfair admin.");
            }
            else
            {
                TempData["Message"] = AppLogic.setFrontendMessage(0, "Success: A password reset link has been sent to your registered email address!");
                VendorHelper.Instance.AddPasswordRequestDate(User.EmailId);
                EmailSender.FncSendPasswordResetMailToVendor(User.EmailId, User.VendorId, User.VendorName);
                return RedirectToAction("login");
            }
            return View(_model);
        }

        public ActionResult ResetPassword(string email)
        {
            try
            {
                bool IsExpired = false;
                var Vendor = VendorHelper.Instance.GetVendorByEmail(email);
                if (Vendor.PasswordResetStatus != 1)
                {
                    if (Vendor.PasswordResetDate.Value.AddHours(24) > DateTime.UtcNow)
                        IsExpired = false;
                    else
                        IsExpired = true;
                }
                else
                    IsExpired = true;

                if (IsExpired)
                {
                    TempData["Message"] = AppLogic.setFrontendMessage(-1, "Password reset link has expired");
                    return RedirectToAction("login");
                }
                ResetPasswordModel model = new ResetPasswordModel() { EmailId = Vendor.EmailId };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = AppLogic.setFrontendMessage(-1, ex.Message + "! String was:" + email);
                return RedirectToAction("login");
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (VendorHelper.Instance.ResetVendorPassword(model.EmailId, enc.Encrypt(model.NewPassword)) > 0)
                {
                    TempData["Message"] = AppLogic.setFrontendMessage(0, "Password reset successfully");
                    return RedirectToAction("login");
                }
                else
                {
                    TempData["Message"] = AppLogic.setFrontendMessage(1, "Password reset failed");
                    return View(new ResetPasswordModel() { EmailId = model.EmailId });
                }
            }
            catch
            {
                TempData["Message"] = AppLogic.setFrontendMessage(1, "Error: Password reset failed");
                return RedirectToAction("login");
            }
        }

        [VendorSessionExpire]
        public ActionResult Dashboard()
        {
            ViewBag.Quotations = VendorQuotationHelper.Instance.GetAllQuotations().Where(x => x.VendorId == VendorSessionData.Instance.VendorId).OrderByDescending(m => m.CreatedOn).ToList();
            return View();
        }

        [VendorSessionExpire]
        public ActionResult PMB(long Id)
        {
            ViewBag.PMBList = PMBHelper.Instance.GetPMB(Id);
            return View(new PMBModel() { QuotatoinId = Id });
        }

        [VendorSessionExpire]
        //Save Vendor PMB details
        //++++++++++++++++++++++++++++++++//
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PMB(PMBModel _model)
        {
            bool hasError = false;
            if (_model.Attachment != null)
            {
                List<string> fileTypes = new List<string> { ".jpg", ".png", ".jpeg", ".pdf", ".ppt", ".xls",".xlsx", ".doc", ".docx",".txt" };
                if (!fileTypes.Contains(Path.GetExtension(_model.Attachment.FileName)))
                {
                    ModelState.AddModelError("Message", "Invalid File only " + String.Join(",", fileTypes) + " are allowed");
                    hasError = true;
                }
                if (_model.Attachment.ContentLength > (5 * 1024 * 1024))
                {
                    ModelState.AddModelError("Message", "Max file size 5 MB");
                    hasError = true;
                }
                if (string.IsNullOrEmpty(_model.Message))
                {
                    ModelState.AddModelError("Message", "Message is required");
                    hasError = true;
                }
                if (hasError)
                    return View(_model);
            }
            _model.CreatedByUserType = "V";
            _model.CreatedBy = VendorSessionData.Instance.VendorId;

            if (PMBHelper.Instance.AddNewPMB(_model) > 0)
            {
                return RedirectToAction("PMB");
            }
            return View(_model);
        }
        //Download PMB attachment
        //++++++++++++++++++++++//

        [AcceptVerbs(HttpVerbs.Get)]
        public FileResult Download(long quotId, string fileName, string actualFileName)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var content = client.DownloadData(AwsS3Bucket.AccessPath() + "/resources/vendor-owner-pmb/" + quotId + "/pmbFiles/" + fileName);
                    using (var stream = new MemoryStream(content))
                    {
                        byte[] buff = stream.ToArray();
                        return File(buff, MimeMapping.GetMimeMapping(fileName), actualFileName);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = AppLogic.setMessage(-1, "Error! " + ex.Message);
                }
            }
            return null;
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();
            return RedirectToAction("login");
        }

        #endregion

        #region //  Owner Code Starts  //

        [ClientSessionExpire]
        public ActionResult Quotation()
        {
            VendorQuotationModel _model = new VendorQuotationModel();
            _model.RequesterEmailId = ClientSessionData.ClientUserName;
            _model.RequesterName = ClientSessionData.ClientName;
            return View(_model);
        }

        [ClientSessionExpire]
        [HttpPost]
        public ActionResult Quotation(VendorQuotationModel _model)
        {
            if (VendorQuotationHelper.Instance.AddNewQuotation(_model) > 0)
            {
                return Json(new { status = "ok", data = "Quotation added successfully" });
            }
            else
            {
                return Json(new { status = "notok", data = "Failed to add quotation" });
            }
        }

        [ClientSessionExpire]
        public ActionResult OwnerPMB(long Id)
        {
            List<PMBModel> pmbList = PMBHelper.Instance.GetPMB(Id);
            ViewBag.PMBList = pmbList;
            return View(new PMBModel() { QuotatoinId = Id });
        }

        //Save Owner PMB details
        //++++++++++++++++++++++++++++++++//
        [ClientSessionExpire]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OwnerPMB(PMBModel _model)
        {
            bool hasError = false;
            if (_model.Attachment != null)
            {
                List<string> fileTypes = new List<string> { ".jpg", ".png", ".jpeg", ".pdf", ".ppt", ".xls", ".xlsx", ".doc", ".docx",".txt" };
                if (!fileTypes.Contains(Path.GetExtension(_model.Attachment.FileName)))
                {
                    ModelState.AddModelError("Message", "Invalid File only " + String.Join(",", fileTypes) + " file types are allowed.");
                    hasError = true;
                }
                if (_model.Attachment.ContentLength > (5 * 1024 * 1024))
                {
                    ModelState.AddModelError("Message", "Max file size 5 MB");
                    hasError = true;
                }
                if(string.IsNullOrEmpty(_model.Message))
                {
                    ModelState.AddModelError("Message", "Message is required");
                    hasError = true;
                }
                if (hasError)
                    return View(_model);
            }
            _model.CreatedByUserType = "O";
            _model.CreatedBy = ClientSessionData.UserClientId;
            if (PMBHelper.Instance.AddNewPMB(_model) > 0)
            {
                return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/pmb/" + _model.QuotatoinId));
            }
            return View(_model);
        }

        [ClientSessionExpire]
        public JsonResult GetVendorsList(long Id)
        {
            return Json(VendorHelper.Instance.GetVendorsByDiscipline(Id).Select(x => new
            {
                VendorName = x.VendorName,
                VendorId = x.VendorId
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [ClientSessionExpire]
        public ActionResult Directory()
        {
            ViewBag.VendorList = VendorHelper.Instance.SearchVendors(null, null, 1);
            return View();
        }

        [ClientSessionExpire]
        [HttpPost]
        public ActionResult Directory(VendorSearchModel _model)
        {
            ViewBag.VendorList = VendorHelper.Instance.SearchVendors(_model.keyword, _model.disciplineid, 1);
            return View(_model);
        }


        [ClientSessionExpire]
        public ActionResult Search(string keyword, int? disciplineid, int? page)
        {
            try
            {
                var result = VendorHelper.Instance.SearchVendors(keyword, disciplineid, page);
                if (result.Count > 0)
                    return PartialView("_vendorList", result);
                else
                    return Json("-1", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}