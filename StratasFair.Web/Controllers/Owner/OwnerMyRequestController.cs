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

namespace StratasFair.Web.Controllers.Owner
{
    [ClientSessionExpire]
    [StrataBoardOwner]
    public class OwnerMyRequestController : Controller
    {
        MyRequestHelper myRequestHelper = new MyRequestHelper();

        // GET: Index
        public ActionResult Index()
        {
            int BlockSize = 12;
            List<StrataOwnerMyRequestModel> strataOwnerMyRequestModelList = myRequestHelper.GetStrataOwnerMyRequest(1, BlockSize);
            return View(strataOwnerMyRequestModelList);
        }

        [ChildActionOnly]
        public ActionResult ListOwnerMyRequest(List<StrataOwnerMyRequestModel> model)
        {
            return PartialView(model);
        }

        // GET: AddOwnerMyRequest
        [HttpGet]
        public ActionResult AddOwnerMyRequest()
        {
            StrataOwnerMyRequestModel model = new StrataOwnerMyRequestModel();
            return PartialView(model);
        }

        // POST: AddOwnerMyRequest
        [HttpPost]
        public ActionResult AddOwnerMyRequest(StrataOwnerMyRequestModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = myRequestHelper.AddStrataOwnerMyRequest(model);
                if (result == -1)
                {
                    strMsg = "Request already exists to this User for given dates";
                }
                else if (result == 1)
                {
                    strMsg = "Request has been created successfully.";
                }
                else
                {
                    strMsg = "Request creation failed.";
                }
            }
            else
            {
                strMsg = "Request creation failed.";
            }
            return Json(new { result = result, Msg = strMsg });

        }

        // POST: DeleteOwnerMyRequest
        [HttpPost]
        public JsonResult DeleteOwnerMyRequest(int RequestId)
        {
            int result = myRequestHelper.DeleteStrataOwnerMyRequest(RequestId);
            if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Request has been deleted successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Request deletion failed.");
            }
            return Json(new { success = true });
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
            var strataOwnerMyRequestModel = myRequestHelper.GetStrataOwnerMyRequest(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = strataOwnerMyRequestModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListOwnerMyRequest", strataOwnerMyRequestModel);
            return Json(jsonModel);
        }
    }
}