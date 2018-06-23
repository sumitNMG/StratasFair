using StratasFair.Business;
using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using StratasFair.Common;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers
{
    [ClientSessionExpire]
    public class SettingsController : Controller
    {
        ClientLoginHelper clientLoginHelper = new ClientLoginHelper();
        ClientLogOnModel model = new ClientLogOnModel();
        int MaxProfileImageLength = 5; // in MB
        int MaxProfileImageWidth = 100;
        int MaxProfileImageHeight = 100;
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        // GET: Settings/MyProfile
        [StrataBoardAdmin]
        public ActionResult MyProfile()
        {
            if (ClientSessionData.UserClientId != 0)
            {
                model = clientLoginHelper.GetById(ClientSessionData.UserClientId);
            }
            return View(model);
        }

        //// POST: Settings/MyProfile
        [HttpPost]
        [StrataBoardAdmin]
        public ActionResult MyProfile(ClientLogOnModel model, HttpPostedFileBase file)
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


                    if (ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName") && ModelState.IsValidField("ImageType"))
                    {
                        int result = 0;
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
                            result = clientLoginHelper.UpdateProfile(model, false);
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

                                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Record updated successfully.");
                            }
                        }
                        else
                        {
                            result = clientLoginHelper.UpdateProfile(model, false);
                        }

                        if (result == 1)
                        {
                            TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Profile updated successfully!");
                        }
                        else
                        {
                            TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Profile updation failed due to Session Expired!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    new AppError().LogMe(ex);
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Profile Updation Failed");
                    return View(model);
                }
                return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/settings/myprofile"));
            }
            else
            {
                return View(model);
            }
        }


        // GET: Settings/OwnerMyProfile
        [StrataBoardOwner]
        public ActionResult OwnerMyProfile()
        {
            if (ClientSessionData.UserClientId != 0)
            {
                model = clientLoginHelper.GetById(ClientSessionData.UserClientId);
            }
            return View(model);
        }

        //// POST: Settings/OwnerMyProfile
        [HttpPost]
        [StrataBoardOwner]
        public ActionResult OwnerMyProfile(ClientLogOnModel model,  HttpPostedFileBase file)
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

                    ModelState.Remove("Password");
                    if (ModelState.IsValid)
                    {
                        int result = 0;
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
                            result = clientLoginHelper.UpdateProfile(model, true);
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

                                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Record updated successfully.");
                            }
                        }
                        else
                        {
                            result = clientLoginHelper.UpdateProfile(model, true);
                        }

                        if (result == 1)
                        {
                            TempData["CommonMessage"] = AppLogic.setFrontendMessage(0,  "Profile updated successfully!");
                        }
                        else
                        {
                            TempData["CommonMessage"] = AppLogic.setFrontendMessage(2,  "Profile updation failed due to Session Expired!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    new AppError().LogMe(ex);
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Profile Updation Failed");
                    return View(model);
                }
                return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/settings/ownermyprofile"));
            }
            else
            {
                return View(model);
            }
        }





        // GET: Settings/UniqueURLRequest
        [HttpGet]
        [StrataBoardAdmin]
        public ActionResult UniqueURLRequest()
        {
            RequestPortalLinkModel objModel = new RequestPortalLinkModel();
            if (ClientSessionData.UserClientId != 0)
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
                objModel = strataBoardHelper.GetStratasBoardPortalLinkDetails(ClientSessionData.ClientStrataBoardId);
            }
            return View(objModel);
        }

        // POST: Settings/UniqueURLRequest
        [HttpPost]
        [StrataBoardAdmin]
        public ActionResult UniqueURLRequest(RequestPortalLinkModel objModel)
        {
            int result = 0;
            if (ClientSessionData.UserClientId != 0)
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();

                result = strataBoardHelper.UpdateURLRequestPortalLink(objModel);
                if (result == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Request has been sent sucessfully");
                }
                else if(result == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "The Url have already once before");
                }

            }
            return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/settings/UniqueURLRequest"));
        }

        // GET: Settings/ResetPassword
        [HttpGet]
        public ActionResult ResetPassword()
        {
            ClientChangePassword ObjModel = new ClientChangePassword();
            return View(ObjModel);
        }

        // POST: Settings/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ClientChangePassword ObjModel)
        {
            //   if (ModelState.IsValid)
            {
                ClientLoginHelper clientLoginHelper = new ClientLoginHelper();
                int Result = clientLoginHelper.ResetChangePassword(ObjModel);
                if (Result == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Password changed sucessfully");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Old Password is incorrect. Please try again");
                }
            }
            return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/settings/resetpassword"));
        }

        //// GET: Settings/EnableDisableAdminNotification
        [HttpGet]
        public ActionResult EnableDisableAdminNotification()
        {
            if (ClientSessionData.UserClientId != 0)
            {
                model.IsEmailNotification = ClientSessionData.ClientIsEmailNotification;
            }
            return View(model);
        }

        // POST: Settings/UpdateAdminNotificationSetting  
        [HttpPost]
        public JsonResult UpdateAdminNotificationSetting(bool IsEnable, int Type)
        {
            int result = 0;
            string strMsg = "";
            if (ClientSessionData.UserClientId != 0)
            {
                result = clientLoginHelper.UpdateEnableDisableNotification(IsEnable, Type, true);
                if (result == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Notification Settings updated sucessfully");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Notification Settings updation failed");
                }
            }
            return Json(new { msg = strMsg }, JsonRequestBehavior.AllowGet);
        }

        //// GET: Settings/EnableDisableAdminNotification
        [HttpGet]
        public ActionResult EnableDisableOwnerNotification()
        {
            if (ClientSessionData.UserClientId != 0)
            {
                model.IsEmailNotification = ClientSessionData.ClientIsEmailNotification;
                model.IsSMSNotification = ClientSessionData.ClientIsSMSNotification;
            }
            return View(model);
        }

        // POST: Settings/UpdateOwnerNotificationSetting 
         [HttpPost]
        public JsonResult UpdateOwnerNotificationSetting(bool IsEnable, int Type)
        {
            int result = 0;
            string strMsg = "";
            if (ClientSessionData.UserClientId != 0)
            {
                result = clientLoginHelper.UpdateEnableDisableNotification(IsEnable, Type, false);
                if (result == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Notification Settings updated sucessfully");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Notification Settings updation failed");
                }
            }
            return Json(new { msg = strMsg }, JsonRequestBehavior.AllowGet);
        }

        
    }
}

