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
    public class EmailNotificationController : Controller
    {
        // GET: Administrator/EmailNotification
        [HttpGet]
        public ActionResult Index()
        {
            EmailNotificationModel emailNotificationModel = new EmailNotificationModel();
            EmailNotificationHelper _Helper = new EmailNotificationHelper();
            emailNotificationModel = _Helper.GetEmailNotificationDetails(emailNotificationModel);
            return View(emailNotificationModel);
        }


        [HttpPost]
        public ActionResult Index(EmailNotificationModel emailNotificationModel)
        {
            if (ModelState.IsValid)
            {
                EmailNotificationHelper _Helper = new EmailNotificationHelper();
                int result = _Helper.UpdateEmailNotificationSetting(emailNotificationModel);
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
            return View(emailNotificationModel);
        }
    }
}