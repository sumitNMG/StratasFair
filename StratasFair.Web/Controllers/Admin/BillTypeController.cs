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
    [StrataBoardAdmin]
    public class BillTypeController : Controller
    {
        BillHelper billTypeHelper = new BillHelper();

        // GET: BillType
        public ActionResult Index()
        {
            int BlockSize = 12;
            List<BillTypeModel> BillTypeModelList = billTypeHelper.GetAllBillTypes(1, BlockSize);
            return View(BillTypeModelList);
        }

        [ChildActionOnly]
        public ActionResult ListBillType(List<BillTypeModel> model)
        {
            return PartialView(model);
        }

        // GET: AddBillType
        [HttpGet]
        public ActionResult AddBillType()
        {
            BillTypeModel model = new BillTypeModel();
            return PartialView(model);
        }

        // POST: AddBillType
        [HttpPost]
        public ActionResult AddBillType(BillTypeModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = billTypeHelper.AddUpdateBillType(model);
                if (result == -1)
                {
                    strMsg = "BillType already exists with the same name";
                }
                else if (result == 1)
                {
                    strMsg = "BillType created successfully.";
                }
                else
                {
                    strMsg = "BillType creation failed.";
                }
            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: EditBillType
        [HttpGet]
        public ActionResult EditBillType(int BillTypeId)
        {
            BillTypeModel model = billTypeHelper.GetBillTypeDetail(BillTypeId);
            return PartialView("EditBillType", model);
        }

        // POST: EditBillType
        [HttpPost]
        public ActionResult EditBillType(BillTypeModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = billTypeHelper.AddUpdateBillType(model);
                if (result == -1)
                {
                    strMsg = "BillType already exists with the same name";
                }
                else if (result == 1)
                {
                    strMsg = "BillType updated successfully.";
                }
                else
                {
                    strMsg = "BillType updation failed.";
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
            var BillTypeModel = billTypeHelper.GetAllBillTypes(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = BillTypeModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListBillType", BillTypeModel);
            return Json(jsonModel);
        }

        // POST: DeleteBillType
        [HttpPost]
        public JsonResult DeleteBillType(int BillTypeId)
        {
            int result = billTypeHelper.DeleteBillType(BillTypeId);
            if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Bill Type has been deleted successfully.");
            }
            else if (result == -1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Bill Type can not be deleted because it is assigned with Owner Bill");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Bill Type deletion failed.");
            }
            return Json(new { success = true });
        }
    }
}