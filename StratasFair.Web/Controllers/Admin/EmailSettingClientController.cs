using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers.Admin
{
    [ClientSessionExpire]
    [StrataBoardAdmin]
    public class EmailSettingClientController : Controller
    {

        // GET: Settings/EmailSettings
        [HttpGet]
        public ActionResult Index()
        {
            EmailServerClientHelper emailServerClientHelper = new EmailServerClientHelper();
            EmailServerSettingModel ObjModel = emailServerClientHelper.GetEmailServerClientDetails();
            return View(ObjModel);
        }

        // POST: Settings/EmailSettings
        [HttpPost]
        public ActionResult Index(EmailServerSettingModel ObjModel)
        {
            ModelState.Remove("EmailForTest");
            if (ModelState.IsValid)
            {
                EmailServerClientHelper emailServerClientHelper = new EmailServerClientHelper();
                int Result = emailServerClientHelper.AddUpdateEmailServerSettingsClient(ObjModel);
                if (Result == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Email Server settings created sucessfully");
                }
                else if (Result == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Email Server settings updated sucessfully");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Email Server settings updation failed");
                }
            }
            return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/emailsettingclient"));
        }

        [HttpPost]
        public ActionResult CheckEmailSettings(EmailServerSettingModel emailServerSettingModel)
        {
            if (ModelState.IsValid)
            {
                string mailSuccess = EmailSender.SendTestMailFromAdmin(emailServerSettingModel.EmailForTest, "Test Mail", "Test Mail from StratasFair Admin");
                if (mailSuccess == "success")
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Email is tested successfully.");
                  
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, mailSuccess);
                 
                }
            }
            return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/emailsettingclient"));
        }
    }
}