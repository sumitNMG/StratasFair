using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers.Admin
{
    [ClientSessionExpire]
    public class ManageSubAdminController : Controller
    {
        SubAdminHelper subAdminHelper = new SubAdminHelper();

        // GET: Index
        [HttpGet]
        public ActionResult Index()
        {
            int BlockSize = 12;
            List<SubAdminModel> subAdminModelList = subAdminHelper.GetAllSubAdminDetails(1, BlockSize);
            return View(subAdminModelList);

        }

        [ChildActionOnly]
        public ActionResult ListSubAdmin(List<SubAdminModel> model)
        {
            return PartialView(model);
        }

        // GET: AddSubAdmin
        [HttpGet]
        public ActionResult AddSubAdmin()
        {
            SubAdminModel subAdminModel = new SubAdminModel();
            subAdminModel.UserClientPrivilege = subAdminHelper.GetUserClientPrivileges();
            return PartialView("AddSubAdmin", subAdminModel);
        }

        // GET: AddSubAdmin
        [HttpPost]
        public ActionResult AddSubAdmin(SubAdminModel model)
        {
            int result = 0;
            string strMsg = "";

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.SelectedUserClientPrivilege))
                {
                    result = subAdminHelper.AddUpdate(model);
                    if (result == 0)
                    {
                        strMsg = "Strata Board member has been created successfully.";
                    }
                    else if (result == -1)
                    {
                        strMsg = "Email already exits. Please use another Email";
                    }
                    else if (result == -3)
                    {
                        strMsg = "No. of Subscription of User of this StrataBoard Exceeds";
                    }
                    else
                    {
                        strMsg = "Strata Board member creation failed.";
                    }
                }

                else
                {
                    result = -5;
                    strMsg = "Please select at least one user access privilege.";
                }
            }
            // return RedirectToAction("index");
            return Json(new { result = result, Msg = strMsg });
        }

        // GET: ViewUserPrivileges
        // [HttpGet]
        public PartialViewResult ViewUserPrivileges(int SubAdminId)
        {
            SubAdminModel subAdminModel = subAdminHelper.GetSubAdminDetails(SubAdminId);
            return PartialView("ViewUserPrivileges", subAdminModel);
        }

        //// GET: EditSubAdmin
        [HttpGet]
        public PartialViewResult EditSubAdmin(int SubAdminId)
        {
            SubAdminModel subAdminModel = subAdminHelper.GetSubAdminDetails(SubAdminId);
            return PartialView("EditSubAdmin", subAdminModel);
        }

        // GET: EditSubAdmin
        [HttpPost]
        public ActionResult EditSubAdmin(SubAdminModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.SelectedUserClientPrivilege))
                {
                    result = subAdminHelper.AddUpdate(model);
                    if (result == -1)
                    {
                        strMsg = "Email already exits. Please use another Email";
                    }
                    else if (result == 0)
                    {
                        strMsg = "Strata Board member updated successfully.";
                    }
                    else
                    {
                        strMsg = "Strata Board member creation failed.";
                    }
                }
                else
                {
                    result = -5;
                    strMsg = "Please select at least one user access privilege.";
                }

            }
            //return RedirectToAction("index");
            return Json(new { result = result, Msg = strMsg });
        }

        // POST: DeleteSubAdmin
        [HttpPost]
        public JsonResult DeleteSubAdmin(int SubAdminId)
        {
            int result = subAdminHelper.DeleteSubAdmin(SubAdminId);
            if (result == 0)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Strata Board member has been deleted successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Strata Board member deletion failed.");
            }
            return Json(new { success = true });
        }

        // POST: ActivateDeActivateSubAdmin
        [HttpPost]
        public JsonResult ActivateDeActivateSubAdmin(int SubAdminId, int IsActive)
        {
            int result = subAdminHelper.ActivateDeActivateSubAdmin(SubAdminId, IsActive);
            if (result == 0)
            {
                if (IsActive == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Strata Board member has been DeActivated successfully.");
                }
                else if (IsActive == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Strata Board member has been Activated successfully.");
                }
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Strata Board member DeActivation/Activation failed.");
            }
            return Json(new { success = true });
        }

        // POST: ActivateDeActivateSubAdmin

        public JsonResult ResendCredentialsMail(int SubAdminId)
        {
            string strMsg = string.Empty;
            string result = EmailSender.FncSend_StratasBoard_RegistrationMail_ToSubAdminClient(SubAdminId);
            if (result == "success")
            {
                strMsg = "Credentials has been sent successfully.";
            }
            else
            {
                strMsg = "Credentials has not been sent.";
            }
            return Json(new { result = result, Msg = strMsg }, JsonRequestBehavior.AllowGet);
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
            var subAdminModel = subAdminHelper.GetAllSubAdminDetails(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = subAdminModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListSubAdmin", subAdminModel);
            return Json(jsonModel);
        }
    }
}