using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Admin;
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
    [StrataBoardAdmin]
    public class CommonAreaController : Controller
    {
        CommonAreaHelper commonAreaHelper = new CommonAreaHelper();

        // GET: CommonArea
        public ActionResult Index()
        {
            int BlockSize = 12;
            List<CommonAreaModel> commonAreaModelList = commonAreaHelper.GetAllCommonAreas(1, BlockSize);
            return View(commonAreaModelList);
        }

        [ChildActionOnly]
        public ActionResult ListCommonArea(List<CommonAreaModel> model)
        {
            return PartialView(model);
        }

        // GET: AddOwnerMyRequest
        [HttpGet]
        public ActionResult AddCommonArea()
        {
            CommonAreaModel model = new CommonAreaModel();
            return PartialView(model);
        }

        // POST: AddOwnerMyRequest
        [HttpPost]
        public ActionResult AddCommonArea(CommonAreaModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = commonAreaHelper.AddUpdateCommonArea(model);
                if (result == -1)
                {
                    strMsg = "CommonArea already exists with the same name";
                }
                else if (result == 1)
                {
                    strMsg = "CommonArea created successfully.";
                }
                else
                {
                    strMsg = "CommonArea creation failed.";
                }
            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: AddOwnerMyRequest
        [HttpGet]
        public ActionResult EditCommonArea(int CommonAreaId)
        {
            CommonAreaModel model = commonAreaHelper.GetCommonAreaDetail(CommonAreaId);
            return PartialView("EditCommonArea", model);
        }

        // POST: AddOwnerMyRequest
        [HttpPost]
        public ActionResult EditCommonArea(CommonAreaModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = commonAreaHelper.AddUpdateCommonArea(model);
                if (result == -1)
                {
                    strMsg = "CommonArea already exists with the same name";
                }
                else if (result == 1)
                {
                    strMsg = "CommonArea updated successfully.";
                }
                else
                {
                    strMsg = "CommonArea updation failed.";
                }
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
            var CommonAreaModel = commonAreaHelper.GetAllCommonAreas(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = CommonAreaModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListCommonArea", CommonAreaModel);
            return Json(jsonModel);
        }

        // POST: DeleteOwnerMyRequest
        [HttpPost]
        public JsonResult DeleteCommonArea(int CommonAreaId)
        {
            int result = commonAreaHelper.DeleteCommonArea(CommonAreaId);
            if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "CommonArea has been deleted successfully.");
            }
            else if (result == -1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Resource/Common Area can not be deleted because it is either booked or booking is in process");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "CommonArea deletion failed.");
            }
            return Json(new { success = true });
        }
    }
}