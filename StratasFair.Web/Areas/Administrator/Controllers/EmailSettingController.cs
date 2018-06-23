using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    [AdminSessionExpire]
    public class EmailSettingController : Controller
    {
        // GET: Administrator/EmailSetting
        [HttpGet]
        public ActionResult Index()
        {
            EmailServerModel emailServerModel = new EmailServerModel();
            EmailServerHelper emailServerHelper = new EmailServerHelper();
            emailServerModel = emailServerHelper.GetEmailServerDetails(emailServerModel);
            return View(emailServerModel);
        }


        [HttpPost]
        public ActionResult Index(EmailServerModel emailServerModel)
        {
            ModelState.Remove("Email");
            if (ModelState.IsValid)
            {
                EmailServerHelper emailServerHelper = new EmailServerHelper();
                int result = emailServerHelper.UpdateEmailServerSetting(emailServerModel);
                if (result >= 1)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(0, "Record updated successfully.");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(result, "Error: Please try again.");
                    return RedirectToAction("Index");
                }
            }
            return View(emailServerModel);
        }


        [HttpPost]
        public ActionResult Check(EmailServerModel emailServerModel)
        {
            if (ModelState.IsValid)
            {
                string mailSuccess = EmailSender.SendTestMailFromAdmin(emailServerModel.Email, "Test Mail", "Test Mail from StratasFair Admin");
                if (mailSuccess == "success")
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(0, "Email is tested successfully.");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(-1, mailSuccess);
                    return RedirectToAction("Index");
                }
            }
            return View(emailServerModel);
        }
    }
}