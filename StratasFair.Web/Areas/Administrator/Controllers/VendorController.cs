using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Common;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    [AdminSessionExpire]
    public class VendorController : Controller
    {
        int MaxLength = 5; // in MB
        int MaxImageWidth = 100;
        int MaxImageHeight = 100;


        int MaxFileLength = 50; // in MB

        // GET: Administrator/Vendor
        public ActionResult Index()
        {
            VendorSearchModel model = new VendorSearchModel();
            return BindList(model);
        }

        List<VendorSearchModel> ActiveList = new List<VendorSearchModel>();
        List<VendorSearchModel> InActiveList = new List<VendorSearchModel>();

        private ActionResult BindList(VendorSearchModel model)
        {
            VendorHelper _Helper = new VendorHelper();
            var List = _Helper.GetAll(model);
            ActiveList = List.Where(x => x.Status == 1).OrderBy(x => x.Status).ThenBy(a=>a.AdminApproval).ThenByDescending(o=>o.CreatedOn).ToList();
            InActiveList = List.Where(x => x.Status == 0).OrderBy(x => x.Status).ThenBy(a => a.AdminApproval).ThenByDescending(o => o.CreatedOn).ToList();
            return View(Tuple.Create(ActiveList, InActiveList, model));
        }

        [HttpPost]
        public ActionResult Index(VendorSearchModel model)
        {
            return BindList(model);
        }



        [HttpGet]
        public ActionResult Edit(long Id)
        {
            VendorHelper _Helper = new VendorHelper();
            VendorModel _model = new VendorModel();
            _model = _Helper.GetByID(Id);

            ViewBag.DisciplineList = CommonData.GetDisciplineWithoutOtherList();
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(_model.Status));

         
            return View(_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(VendorModel _model)
        {
            try
            {

                // create a bucket if not exists
                string createBucket = AwsS3Bucket.CreateABucket();


                if (ModelState.ContainsKey("ImageType")) ModelState["ImageType"].Errors.Clear();
                var imageTypes = new string[]{    
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };

                int _maxLength = MaxLength * 1024 * 1024;

                if (_model.ImageType != null)
                {
                    if (!imageTypes.Contains(_model.ImageType.ContentType))
                    {
                        ModelState.AddModelError("ImageType", "Please choose either a GIF, JPG or PNG image.");
                    }
                    else if (_model.ImageType.ContentLength > _maxLength)
                    {
                        ModelState.AddModelError("ImageType", "Maximum allowed file size is " + MaxLength + " MB.");
                    }
                    //else if (imageTypes.Contains(_model.ImageType.ContentType) && _model.ImageType.ContentLength <= _maxLength)
                    //{
                    //    System.Drawing.Image img = System.Drawing.Image.FromStream(_model.ImageType.InputStream);
                    //    int height = img.Height;
                    //    int width = img.Width;

                    //    if (width > MaxImageWidth || height > MaxImageHeight)
                    //    {
                    //        ModelState.AddModelError("ImageType", "Maximum allowed file dimension is " + MaxImageWidth + "*" + MaxImageHeight);
                    //    }
                    //}
                }

                if (ModelState.ContainsKey("TradeAndBusinessFileType")) ModelState["TradeAndBusinessFileType"].Errors.Clear();
                int _maxFileLength = MaxFileLength * 1024 * 1024;
                if (_model.TradeAndBusinessFileType != null)
                {

                    string AlllowedExtension = ".pdf";
                    List<string> extList = AlllowedExtension.Split(',').ToList<string>();
                    if (!extList.Contains(System.IO.Path.GetExtension(_model.TradeAndBusinessFileType.FileName)))
                    {
                        ModelState.AddModelError("TradeAndBusinessFileType", "Please choose only " + AlllowedExtension + " file.");
                    }
                    else if (_model.TradeAndBusinessFileType.ContentLength > _maxFileLength)
                    {
                        ModelState.AddModelError("TradeAndBusinessFileType", "Maximum allowed file size is " + MaxFileLength + " MB.");
                    }

                }


                if (ModelState.IsValid)
                {
                    VendorHelper _Helper = new VendorHelper();
                    long _vendorId = 0;
                    int? _adminApproval = 0;
                    string uniqueId = Guid.NewGuid().ToString();
                    string ext = string.Empty;
                    string extFile = string.Empty;

                    if (_model.TradeAndBusinessFileType != null)
                    {
                        extFile = System.IO.Path.GetExtension(_model.TradeAndBusinessFileType.FileName);
                        _model.TradeAndBusinessFile = uniqueId + extFile;
                        _model.ActualTradeAndBusinessFile = _model.TradeAndBusinessFileType.FileName;
                    }
                    if (_model.ImageType != null)
                    {
                        ext = System.IO.Path.GetExtension(_model.ImageType.FileName);
                        _model.ImageFile = uniqueId + "_img" + ext;
                        _model.ActualImageFile = _model.ImageType.FileName;
                    }

                    _adminApproval = _model.AdminApprovalPrev;
                    _vendorId = _Helper.AddUpdate(_model);

                    if (_vendorId > 0)
                    {
                        string initialPath = "resources/vendor/" + _vendorId;

                        // Add/Delete the new trade and business file and image details                   
                        string path = string.Empty;
                        string oldfilePath = string.Empty;

                        int fileMapped = -1;
                        if (_model.ImageType != null)
                        {
                            // save the file locally
                            if (!Directory.Exists(Server.MapPath("~/Content/" + initialPath + "/ProfilePicture/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Content/" + initialPath + "/ProfilePicture/"));
                            }
                            // save the file locally
                            path = Server.MapPath(Path.Combine("~/Content/" + initialPath + "/ProfilePicture/" + _model.ImageFile));
                            _model.ImageType.SaveAs(path);

                            // save the file on s3
                            fileMapped = AwsS3Bucket.CreateFile(initialPath + "/ProfilePicture/" + _model.ImageFile, path);

                            // delete the file locally
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            /*******************************************************************************/

                            // delete the old file locally
                            oldfilePath = Server.MapPath("~/Content/Resources/Vendor/" + _vendorId + "/ProfilePicture/" + _model.OldImageFile);
                            if (System.IO.File.Exists(oldfilePath))
                            {
                                System.IO.File.Delete(oldfilePath);
                            }
                            // delete the old file from s3
                            AwsS3Bucket.DeleteObject(initialPath + "/ProfilePicture/" + _model.OldImageFile);

                            /*******************************************************************************/

                        }
                        if (_model.TradeAndBusinessFileType != null)
                        {
                            // save the file locally
                            if (!Directory.Exists(Server.MapPath("~/Content/" + initialPath + "/TradeFile/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Content/" + initialPath + "/TradeFile/"));
                            }
                            path = Server.MapPath("~/Content/Resources/Vendor/" + _vendorId + "/TradeFile/" + _model.TradeAndBusinessFile);
                            _model.TradeAndBusinessFileType.SaveAs(path);

                            // save the file on s3
                            fileMapped = AwsS3Bucket.CreateFile(initialPath + "/TradeFile/" + _model.TradeAndBusinessFile, path);

                            // delete the file locally
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }



                            // delete the old file locally
                            oldfilePath = Server.MapPath("~/Content/Resources/Vendor/" + _vendorId + "/TradeFile/" + _model.OldTradeAndBusinessFile);
                            if (System.IO.File.Exists(oldfilePath))
                            {
                                System.IO.File.Delete(oldfilePath);
                            }
                            // delete the old file from s3
                            AwsS3Bucket.DeleteObject(initialPath + "/TradeFile/" + _model.OldTradeAndBusinessFile);
                        }

                        if (_model.AdminApproval == 2 && _adminApproval != 2)
                        {
                            SendVendorRejectedMail(_vendorId);
                        }

                        if (_model.AdminApproval == 1 && _adminApproval != 1)
                        {
                            SendVendorApprovedMail(_vendorId);
                        }
                        TempData["CommonMessage"] = AppLogic.setMessage(0, "Record updated successfully.");
                        return RedirectToAction("Index");
                    }
                    else if (_vendorId == -1)
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(1, "Record already exists.");
                    }
                    else
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(2, "Error, Please try again.");
                    }
                }
            }
            catch(Exception ex)
            {

            }
            ViewBag.DisciplineList = CommonData.GetDisciplineWithoutOtherList();
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(_model.Status));
            return View(_model);

        }


        public void SendVendorApprovedMail(long vendorId)
        {
            string result = "";
            try
            {
                VendorHelper _Helper = new VendorHelper();
                VendorModel _model = new VendorModel();
                _model = _Helper.GetByID(vendorId);
                if (_model.EmailId != "")
                {
                    result = EmailSender.FncSendVendorApprovedMailFromAdmin(_model.EmailId, _model.VendorName, _model.Password);
                    if (result == "success")
                    {
                        // mail is sent successfully to the vendor for approval.
                    }
                }

            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                // Exception occured
            }
        }

        public void SendVendorRejectedMail(long vendorId)
        {
            string result = "";
            try
            {
                VendorHelper _Helper = new VendorHelper();
                VendorModel _model = new VendorModel();
                _model = _Helper.GetByID(vendorId);
                if (_model.EmailId != "")
                {
                    result = EmailSender.FncSendVendorRejectedMailFromAdmin(_model.EmailId, _model.VendorName, _model.Remark);
                    if (result == "success")
                    {
                        // mail is sent successfully to the vendor for rejection.
                    }
                }

            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                // Exception occured
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download(long id)
        {
            //return File(Path.Combine(@"c:\path", fileFromDB.FileNameOnDisk), MimeMapping.GetMimeMapping(fileFromDB.FileName), fileFromDB.FileName);
            VendorHelper _Helper = new VendorHelper();
            VendorModel _model = new VendorModel();
            _model = _Helper.GetByID(id);
            //string ext = System.IO.Path.GetExtension(Server.MapPath("~/Content/Resources/vendor/" + _model.VendorId + "/" + _model.TradeAndBusinessFile + ""));
            //return File(Path.Combine(Server.MapPath("~/Content/Resources/vendor/" + _model.VendorId + "/" + _model.TradeAndBusinessFile + "")), MimeMapping.GetMimeMapping(_model.TradeAndBusinessFile), _model.ActualTradeAndBusinessFile);

            using (var client = new WebClient())
            {
                try
                {
                    var content = client.DownloadData(AwsS3Bucket.AccessPath() + "/resources/vendor/" + _model.VendorId + "/TradeFile/" + _model.TradeAndBusinessFile);
                    using (var stream = new MemoryStream(content))
                    {
                        byte[] buff = stream.ToArray();
                        return File(buff, MimeMapping.GetMimeMapping(_model.TradeAndBusinessFile), _model.ActualTradeAndBusinessFile);
                    }
                }
                catch (Exception ex)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error! try again later.");
                    return RedirectToAction("Index");
                }
            }
        }


        public ActionResult Active(long Id)
        {
            VendorHelper _helper = new VendorHelper();
            VendorModel _model = new VendorModel();
            _model = _helper.GetByID(Id);
            _model.Status = 1;
            int count = _helper.ActDeact(_model);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record activated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record activation failed.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Deactive(long Id)
        {
            VendorHelper _helper = new VendorHelper();
            VendorModel _model = new VendorModel();
            _model = _helper.GetByID(Id);
            _model.Status = 0;
            int count = _helper.ActDeact(_model);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deactivated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deactivation failed.");
            }
            return RedirectToAction("Index");

        }

        public ActionResult Delete(long Id)
        {
            VendorHelper _helper = new VendorHelper();
            VendorModel _model = new VendorModel();
            _model = _helper.GetByID(Id);
            int count = _helper.Delete(_model);
            if (count == 0)
            {
                string initialPath = "resources/vendor/" + _model.VendorId;


                // for profile image
                string path = Server.MapPath("~/Content/Resources/Vendor/" + _model.VendorId + "/" + _model.ImageFile);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                // delete the file from s3
                AwsS3Bucket.DeleteObject(initialPath + "/" + _model.ImageFile);

                // for Trade and business file
                path = Server.MapPath("~/Content/Resources/vendor/" + _model.VendorId + "/" + _model.TradeAndBusinessFile);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                // delete the file from s3
                AwsS3Bucket.DeleteObject(initialPath + "/" + _model.TradeAndBusinessFile);


                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deleted successfully.");
            }
            else if (count == -2)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deletion failed. Please delete all the related records first.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deletion failed.");
            }
            return RedirectToAction("Index");
        }

    }
}