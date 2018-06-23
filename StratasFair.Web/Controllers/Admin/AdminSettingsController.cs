using StratasFair.Business;
using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using StratasFair.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers
{
    public class AdminSettingsController : Controller
    {
        ClientLoginHelper clientLoginHelper = new ClientLoginHelper();
        ClientLogOnModel model = new ClientLogOnModel();
        int MaxProfileImageLength = 5; // in MB
        int MaxProfileImageWidth = 300;
        int MaxProfileImageHeight = 250;
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        // GET: Settings/MyProfile
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
        public ActionResult MyProfile(ClientLogOnModel model)
        {
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
                        else if (imageTypes.Contains(model.ImageType.ContentType) && model.ImageType.ContentLength <= _maxLength)
                        {
                            System.Drawing.Image img = System.Drawing.Image.FromStream(model.ImageType.InputStream);
                            int height = img.Height;
                            int width = img.Width;

                            if (width > MaxProfileImageWidth || height > MaxProfileImageHeight)
                            {
                                ModelState.AddModelError("ImageType", "Maximum allowed file dimension is " + MaxProfileImageWidth + "*" + MaxProfileImageHeight);
                            }
                        }
                    }


                    if (ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName"))
                    {
                        int result = 0;
                        if (model.ImageType != null)
                        {
                            string ext = System.IO.Path.GetExtension(model.ImageType.FileName);

                            model.ProfilePicture = Guid.NewGuid() + ext;

                            string path = "~/Content/Resources/Stratabaord/" + ClientSessionData.ClientStrataBoardId + "/profileimages/";
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
                                int fileMapped = AwsS3Bucket.CreateFile("resources/stratabaord/" + ClientSessionData.ClientStrataBoardId + "/profileimages/" + model.ProfilePicture, Mappedpath);
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
                                AwsS3Bucket.DeleteObject("resources/stratabaord/" + ClientSessionData.ClientStrataBoardId + "/profileimages/" + model.OldProfilePicture);

                                TempData["CommonMessage"] = AppLogic.setMessage(result, "Record updated successfully.");
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            result = clientLoginHelper.UpdateProfile(model, false);
                        }

                        if (result == 1)
                        {
                            model.Message = "Profile updated successfully!";
                        }
                        else
                        {
                            model.Message = "Profile updation failed due to Session Expired!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    new AppError().LogMe(ex);
                    model.Message = "Profile Updation Failed";
                    return View(model);
                }
                return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/settings/myprofile"));
            }
            else
            {
                return View(model);
            }
        }

        // GET: Settings/UniqueURLRequest
        [HttpGet]
        public ActionResult UniqueURLRequest()
        {
            RequestPortalLinkModel model = new RequestPortalLinkModel();
            if (ClientSessionData.UserClientId != 0)
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
                model = strataBoardHelper.GetStratasBoardPortalLinkDetails(ClientSessionData.ClientStrataBoardId);
            }
            return View(model);
        }

        // POST: Settings/UniqueURLRequest
        [HttpPost]
        public ActionResult UniqueURLRequest(RequestPortalLinkModel model)
        {
            int result = 0;
            if (ClientSessionData.UserClientId != 0)
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
                result = strataBoardHelper.UpdateURLRequestPortalLink(model);
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
                    TempData["CommonMessage"] = AppLogic.setMessage(0, "Password changed sucessfully");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Please check password you have entered and Try again");
                }
            }
            return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/settings/resetpassword"));
        }

        //// GET: Settings/EnableDisableNotification
        [HttpGet]
        public ActionResult EnableDisableNotification()
        {
            if (ClientSessionData.UserClientId != 0)
            {
                model.IsEmailNotification = ClientSessionData.ClientIsEmailNotification;
            }
            return View(model);
        }

        // GET: Settings/UpdateNotificationSetting       
        public JsonResult UpdateNotificationSetting(bool IsEnable, int Type)
        {
            int result = 0;
            string strMsg = "";
            if (ClientSessionData.UserClientId != 0)
            {
                result = clientLoginHelper.UpdateEnableDisableNotification(IsEnable, Type, true);
                if (result == 1)
                {
                    strMsg = "OK";
                }
            }
            return Json(new { msg = strMsg }, JsonRequestBehavior.AllowGet);
        }
    }
}

