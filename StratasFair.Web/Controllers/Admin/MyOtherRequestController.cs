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
    [StrataBoardAdmin]
    public class MyOtherRequestController : Controller
    {
        MyRequestHelper myRequestHelper = new MyRequestHelper();

        // GET: MyOtherRequest
        public ActionResult Index()
        {
            int BlockSize = 12;
            List<StrataOwnerMyRequestModel> strataOwnerMyRequestModelList = myRequestHelper.GetStrataOwnerMyRequestForAdmin(1, BlockSize);
            return View(strataOwnerMyRequestModelList);
        }

        [ChildActionOnly]
        public ActionResult ListMyOtherRequest(List<StrataOwnerMyRequestModel> model)
        {
            return PartialView(model);
        }


        // GET: AddOwnerMyRequest
        [HttpGet]
        public ActionResult UpdateMyRequestStatus(int RequestId)
        {
            StrataOwnerMyRequestModel model = new StrataOwnerMyRequestModel();
            model.RequestId = RequestId;
            return PartialView("RejectMyRequestStatus", model);
        }


        // GET: AddOwnerMyRequest
        [HttpPost]
        public ActionResult ApproveMyRequestStatus(int RequestId)
        {
            int result = 0;
            string strMsg = "";
            result = myRequestHelper.UpdateMyRequestStatusToApprove(RequestId);
            if (result == 1)
            {
                strMsg = "Request has been Approved successfully";
            }
            else
            {
                strMsg = "Request updation failed.";
            }

            return Json(new { result = result, Msg = strMsg });
        }

        // GET: AddOwnerMyRequest
        [HttpPost]
        public ActionResult RejectMyRequestStatus(StrataOwnerMyRequestModel model)
        {
            int result = 0;
            string strMsg = "";
            result = myRequestHelper.UpdateMyRequestStatusToReject(model);
            if (result == 1)
            {
                strMsg = "Request has been Rejected successfully";
            }
            else
            {
                strMsg = "Request updation failed.";
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
            var strataOwnerMyRequestModel = myRequestHelper.GetStrataOwnerMyRequestForAdmin(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = strataOwnerMyRequestModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListMyOtherRequest", strataOwnerMyRequestModel);
            return Json(jsonModel);
        }
    }
}