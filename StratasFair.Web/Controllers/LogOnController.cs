using System;
using System.Web;
using System.Web.Mvc;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Front;
using StratasFair.Business.Front;
using System.Data;
using StratasFair.BusinessEntity;
using StratasFair.Business;
using System.Configuration;
using StratasFair.Common;
using System.Linq;
using System.IO;
using StratasFair.Web.App_Start;
using StratasFair.Data;

namespace StratasFair.Web.Controllers
{

    public class LogOnController : Controller
    {

        StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
        ClientLoginHelper clientLoginHelper = new ClientLoginHelper();
        StratasFairDBEntities _context = new StratasFairDBEntities();

        int MaxProfileImageLength = 5; // in MB
        int MaxProfileImageWidth = 100;
        int MaxProfileImageHeight = 100;

        // GET: Client/LogOn
        [HttpGet]
        public ActionResult Index()
        {
            if (ClientSessionData.UserClientId != 0)
            {
                StrataBoardModel strataBoardModel = new StrataBoardModel();
                strataBoardModel = strataBoardHelper.GetStratasBoardByID(ClientSessionData.ClientStrataBoardId);
                if (ClientSessionData.ClientRoleName.ToLower() == "owner")
                {
                    if(!ClientSessionData.ClientIsProfileCompleted)
                    {
                        return Redirect(Url.Content("~/" + strataBoardModel.PortalLink + "/logon/completeprofile"));
                    }
                    else
                    {
                        return Redirect(Url.Content("~/" + strataBoardModel.PortalLink + "/dashboard/dashboardowner"));                       
                    }
                    
                }
                else
                {
                    return Redirect(Url.Content("~/" + strataBoardModel.PortalLink + "/dashboard"));
                }                
            }
            else
            {
                string PortalUrl = clientLoginHelper.GetPortalUrlFromCurrentUrl();
                var IsExistPortalUrl = _context.tblStratasBoards.Where(x => x.PortalLink == PortalUrl && x.Status == 0);
                if (IsExistPortalUrl.Count() > 0)
                {
                    return Redirect(Url.Content("~/" + PortalUrl + "/logon/invalidportallink"));
                }
                else
                {
                    return View();
                }
                
            }
        }

        //[ClientSessionExpire]
        // POST: Client/LogOn
        [HttpPost]
        public ActionResult Index(ClientLogOnModel model)
        {
            try
            {
                string pwd = model.Password;

                if (ModelState.IsValidField("Email") && ModelState.IsValidField("Password"))
                {
                    string message = string.Empty;
                    int result = -1;
                    DataTable dt = clientLoginHelper.AuthenticateClientUser(model, out result);
                    if (result == 1)
                    {
                        string PortalUrl = clientLoginHelper.GetPortalUrlFromCurrentUrl();
                        ClientSessionData.ClientPortalLink = dt.Rows[0]["PortalLink"].ToString();
                        if (PortalUrl.ToLower() != ClientSessionData.ClientPortalLink.ToLower())
                        {
                            TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "User Email does not belongs to this Portal Link");
                        }
                        else
                        {
                            ClientSessionData.ClientUserName = model.Email;
                            ClientSessionData.ClientName = dt.Rows[0]["Name"].ToString();
                            ClientSessionData.UserClientId = Convert.ToInt32(dt.Rows[0]["UserClientId"].ToString());
                            ClientSessionData.ClientCreatedOn = dt.Rows[0]["CreatedOn"].ToString();
                            ClientSessionData.ClientLastLoginOn = dt.Rows[0]["LastLogin"].ToString();
                            ClientSessionData.ClientRoleName = dt.Rows[0]["RoleName"].ToString();
                            model.IsProfileComplete = Convert.ToBoolean(dt.Rows[0]["IsProfileComplete"]);
                            ClientSessionData.ClientStrataBoardId = Convert.ToInt32(dt.Rows[0]["StratasBoardId"]);
                            ClientSessionData.ClientIsEmailNotification = Convert.ToBoolean(dt.Rows[0]["IsEmailNotification"]);
                            ClientSessionData.ClientIsSMSNotification = Convert.ToBoolean(dt.Rows[0]["IsSMSNotification"]);
                            ClientSessionData.ClientProfilePicture = dt.Rows[0]["ProfilePicture"].ToString();
                            ClientSessionData.ClientIsProfileCompleted = Convert.ToBoolean(dt.Rows[0]["IsProfileComplete"]);

                            if (model.RememberMe)
                            {
                                HttpCookie cookie = new HttpCookie("ClientLogin");

                                cookie.Values.Add("LoginName", model.Email.Trim());
                                cookie.Values.Add("Password", pwd.Trim());
                                cookie.Values.Add("DtExp", DateTime.Now.AddDays(30).ToString());
                                cookie.Expires = DateTime.Now.AddDays(30);
                                Response.Cookies.Add(cookie);
                            }
                            else
                            {
                                if (Request.Cookies["ClientLogin"] != null)
                                {
                                    HttpCookie objCookie = Request.Cookies["ClientLogin"];
                                    objCookie.Values.Add("DtExp", DateTime.Now.AddDays(-5).ToString());
                                    objCookie.Expires = DateTime.Now.AddDays(-5);
                                    Response.Cookies.Add(objCookie);
                                }
                            }
                           if (ClientSessionData.ClientRoleName.ToLower() == "owner")
                            {
                                if(!model.IsProfileComplete)
                                {
                                    return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/Logon/CompleteProfile"));
                                }
                                else
                                {
                                    return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/dashboard/ownerdashboard"));                       
                                }
                            }
                            else
                            {
                                return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/dashboard"));
                            }
                        }
                    }

                    else if (result == 0)
                    {
                        TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "The user is Deactivated or Deleted by StrataFair Admin");
                    }
                    else if (result == -2)
                    {
                        TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Your password is invalid.");
                    }
                    else if (result == -3)
                    {
                        TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Your Email Address is invalid.");
                    }
                    else
                    {
                        TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Invalid Email Address or Password");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Invalid Username or Password");
                return View(model);
            }
        }

        // GET: Client/CompleteProfile
        [StrataBoardOwner]
        [HttpGet]
        public ActionResult CompleteProfile()
        {
            ClientLogOnModel model = new ClientLogOnModel();
            if (ClientSessionData.UserClientId != 0)
            {
                model = clientLoginHelper.GetById(ClientSessionData.UserClientId);
            }
            return View(model);
        }

         //// POST: Client/CompleteProfile
        [StrataBoardOwner]
        [HttpPost]
        public ActionResult CompleteProfile(ClientLogOnModel model, HttpPostedFileBase file)
        {
            model.ImageType = file;
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    var imageTypes = new string[]{    
                     "image/png",
                     "image/jpeg",
                    "image/pjpeg"
                };

                    int _maxLength = MaxProfileImageLength * 1024 * 1024;

                    ModelState.Remove("ImageType");
                    if (model.ImageType != null)
                    {
                        if (!imageTypes.Contains(model.ImageType.ContentType))
                        {
                            ModelState.AddModelError("ImageType", "Please choose either a JPG or PNG image.");
                        }
                        else if (model.ImageType.ContentLength > _maxLength)
                        {
                            ModelState.AddModelError("ImageType", "Maximum allowed file size is " + MaxProfileImageLength + " MB.");
                        }
                        //else if (imageTypes.Contains(model.ImageType.ContentType) && model.ImageType.ContentLength <= _maxLength)
                        //{
                        //    System.Drawing.Image img = System.Drawing.Image.FromStream(model.ImageType.InputStream);
                        //    int height = img.Height;
                        //    int width = img.Width;

                        //    if (width > MaxProfileImageWidth || height > MaxProfileImageHeight)
                        //    {
                        //        ModelState.AddModelError("ImageType", "Maximum allowed file dimension is " + MaxProfileImageWidth + "*" + MaxProfileImageHeight);
                        //    }
                        //}
                    }

                    ModelState.Remove("Email");
                    ModelState.Remove("Password");
                    int result = 0;
                    if (ModelState.IsValid)
                    {
                        if (model.ImageType != null)
                        {
                            string ext = System.IO.Path.GetExtension(model.ImageType.FileName);

                            model.ProfilePicture = Guid.NewGuid() + ext;

                            string path = "~/Content/Resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/profileimages/";
                            if (!Directory.Exists(Server.MapPath(path)))
                            {
                                Directory.CreateDirectory(Server.MapPath(path));
                            }
                            string Mappedpath = Server.MapPath(path + model.ProfilePicture);
                            result = clientLoginHelper.CompleteProfile(model);
                            if (result == 1)
                            {
                                // save the file locally
                                model.ImageType.SaveAs(Mappedpath);
                                // save the file on s3
                                int fileMapped = AwsS3Bucket.CreateFile("resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/profileimages/" + model.ProfilePicture, Mappedpath);
                                // delete the file locally
                                if (System.IO.File.Exists(Mappedpath))
                                {
                                    System.IO.File.Delete(Mappedpath);
                                }

                                string OldProfilePath = Server.MapPath(path + model.OldProfilePicture);
                                if (System.IO.File.Exists(OldProfilePath))
                                {
                                    System.IO.File.Delete(OldProfilePath);
                                }
                                // delete the old file from s3
                                AwsS3Bucket.DeleteObject("resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/profileimages/" + model.OldProfilePicture);
                            }
                        }
                        else
                        {
                            result = clientLoginHelper.CompleteProfile(model);
                        }

                        if (result == 1)
                        {
                            ClientSessionData.ClientIsProfileCompleted = true;
                            return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/dashboard"));
                        }
                        else
                        {
                            TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Profile Completion Failed.");
                            return View(model);
                        }
                    }
                    else
                    {
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    new AppError().LogMe(ex);
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Something wrong! Profile Completion Failed.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        // GET: Client/ForgotPassword
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Client/ForgotPassword
        [HttpPost]
        public ActionResult ForgotPassword(ClientForgetPasswordModel objClientForgetPasswordModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    objClientForgetPasswordModel.Flag = 4;
                    ClientLoginHelper ClientLoginHelper = new ClientLoginHelper();
                    int StratasBoardId = ClientLoginHelper.ClientPasswordReminder(objClientForgetPasswordModel);
                    if (StratasBoardId > 0)
                    {
                        string result = EmailSender.FncSend_StratasBoard_ForgotPassword_ToClient(StratasBoardId);
                        if (result == "success")
                        {
                            ViewBag.Style = "color:Green !important";
                            TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Your password has been sent. Please check your inbox. It usually takes a few minutes but when we're busy.it may take longer.");
                        }
                        else
                        {
                            ViewBag.Message = "color:Red !important";
                            TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Please try again.");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "color:Red !important";
                        TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Please enter correct email as Email does not exits in our database.");
                    }

                }
                return View(objClientForgetPasswordModel);
            }
            catch
            {
                ViewBag.Message = "color:Red !important";
                TempData["CommonMessage"] = "Email address does not exists. Please try again.";
                return View(objClientForgetPasswordModel);
            }
        }

        // GET: LogOn/ResetPassword
        [HttpGet]
        public ActionResult ResetPassword()
        {
            ClientChangePassword ObjModel = new ClientChangePassword();
            return View(ObjModel);
        }

        // POST: LogOn/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string EncryptUserClientId, ClientChangePassword ObjModel)
        {
            if (ModelState.IsValidField("NewPassword") && ModelState.IsValidField("ConfirmPassword"))
            {
                EncryptUserClientId = EncryptUserClientId.Replace("$", "+");
                EncryptUserClientId = EncryptUserClientId.Replace("/", "!");
                Encrypt64 encrypt = new Encrypt64();
                string UserClientId = encrypt.Decrypt(EncryptUserClientId, ConfigurationManager.AppSettings["SecureKey"].ToString());
                ObjModel.UserClientId = Convert.ToInt32(UserClientId);
                ClientLoginHelper clientLoginHelper = new ClientLoginHelper();
                ClientLogOnModel clientLogOnModel = new ClientLogOnModel();
                clientLogOnModel = clientLoginHelper.GetById(ObjModel.UserClientId);
                int Result = clientLoginHelper.ForgotChangePassword(ObjModel);
                if (Result == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(0, "Password changed sucessfully");
                    return Redirect(Url.Content("~/" + clientLogOnModel.StrataPortalLink + "/Login"));
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Please check password you have entered and Try again");
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        // GET: LogOn/ResetPassword
        [HttpGet]
        public string TestPass(int id)
        {
            string password = clientLoginHelper.TestPassword(id);

            string url = "http://demo2projects.com/stratasfair/sumitest/Login";

            var urlBuilder = new System.UriBuilder(url);
            string PortalUrl = string.Empty;
            if (urlBuilder.Uri.Authority.Contains("demo2projects"))
            {
                PortalUrl = urlBuilder.Uri.AbsolutePath.Replace("/stratasfair/", "").ToString();
            }
            else
            {
                PortalUrl = urlBuilder.Uri.AbsolutePath.TrimStart('/').ToString();
            }
            int endIndex = PortalUrl.IndexOf('/');
            PortalUrl = PortalUrl.Substring(0, endIndex);

            return password;

            //string result = clientLoginHelper.TestPass();


        }

        public ActionResult TestPage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InvalidPortalLink()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotBelongUser()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.AppendHeader("Cache-Control", "no-store");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return View();
        }


    }
}