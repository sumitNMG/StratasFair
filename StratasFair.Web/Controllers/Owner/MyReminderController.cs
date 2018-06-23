using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers.Owner
{
    [ClientSessionExpire]
    [StrataBoardOwner]
    public class MyReminderController : Controller
    {
        MyReminderHelper myReminderHelper = new MyReminderHelper();

        // GET: MyReminder
        public ActionResult Index()
        {
            int BlockSize = 12;
            List<ReminderModel> reminderModelList = myReminderHelper.GetAllReminders(1, BlockSize);
            return View(reminderModelList);
        }

        [ChildActionOnly]
        public ActionResult ListMyReminder(List<ReminderModel> model)
        {
            return PartialView(model);
        }

        // GET: AddOwnerMyRequest
        [HttpGet]
        public ActionResult AddMyReminder()
        {
            ReminderModel model = new ReminderModel();
            return PartialView(model);
        }

        // POST: AddOwnerMyRequest
        [HttpPost]
        public ActionResult AddMyReminder(ReminderModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = myReminderHelper.AddMyReminder(model);
                if (result == -1)
                {
                    strMsg = "Reminder already exists to this User with given subject & dates";
                }
                else if (result == 1)
                {
                    strMsg = "Reminder created successfully.";
                }
                else
                {
                    strMsg = "Reminder creation failed.";
                }
            }
            else
            {
                strMsg = "Please select Start Time - Hour & Minute.";
            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: AddOwnerMyRequest
        [HttpGet]
        public ActionResult EditMyReminder(int ReminderId)
        {
            ReminderModel model = myReminderHelper.GetReminderDetail(ReminderId);
            return PartialView("EditMyReminder", model);
        }

        // POST: AddOwnerMyRequest
        [HttpPost]
        public ActionResult EditMyReminder(ReminderModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = myReminderHelper.AddMyReminder(model);
                if (result == 1)
                {
                    strMsg = "Reminder updated successfully.";
                }
                else
                {
                    strMsg = "Reminder updation failed.";
                }
            }
            else
            {
                strMsg = "Please select Start Time - Hour & Minute.";
            }

            return Json(new { result = result, Msg = strMsg });

        }

        private string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult =
                ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext
                (ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        public ActionResult InfinateScroll(int BlockNumber)
        {
            int BlockSize = 12;
            var reminderModel = myReminderHelper.GetAllReminders(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = reminderModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListMyReminder", reminderModel);
            return Json(jsonModel);
        }

        // POST: DeleteOwnerMyRequest
        [HttpPost]
        public JsonResult DeleteMyReminder(int ReminderId)
        {
            int result = myReminderHelper.DeleteMyReminder(ReminderId);
            if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Reminder has been deleted successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Reminder deletion failed.");
            }
            return Json(new { success = true });
        }
    }
}