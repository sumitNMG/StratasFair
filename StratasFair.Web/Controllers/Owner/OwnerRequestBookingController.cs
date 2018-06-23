using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers.Owner
{
    public class OwnerRequestBookingController : Controller
    {
        StrataOwnerHelper strataOwnerHelper = new StrataOwnerHelper();
        // GET: Index
        public ActionResult Index()
        {
            //int BlockSize = 12;
            //List<StrataOwnerBookingRequestModel> strataOwnerBookingRequestModelList = strataOwnerHelper.GetStrataOwnerBookingRequest(1, BlockSize);
            //return View(strataOwnerBookingRequestModelList);
            return View();
        }

        public ActionResult GetListOwnerBookingRequest()
        {
            int BlockSize = 12;
            List<StrataOwnerBookingRequestModel> strataOwnerBookingRequestModelList = strataOwnerHelper.GetStrataOwnerBookingRequest(1, BlockSize);
            return View(strataOwnerBookingRequestModelList);
        }       

        [ChildActionOnly]
        public ActionResult ListOwnerBookingRequest(List<StrataOwnerBookingRequestModel> model)
        {
            return PartialView(model);
        }

        // GET: AddOwnerMyRequest
        [HttpGet]
        public ActionResult AddOwnerBookingRequest()
        {
            StrataOwnerBookingRequestModel model = new StrataOwnerBookingRequestModel();
            model.CommonAreas = new SelectList(strataOwnerHelper.GetAllCommonAreas(), "CommonAreaId", "CommonAreaName");
            return PartialView("AddOwnerBookingRequest", model);
        }

        // POST: AddOwnerMyRequest
        [HttpPost]
        public ActionResult AddOwnerBookingRequest(StrataOwnerBookingRequestModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = strataOwnerHelper.AddStrataOwnerBookingRequest(model);
                if (result == -1)
                {
                    strMsg = "Booking Request already exists to this User for given dates";
                }
                else if (result == 1)
                {
                    strMsg = "Booking Request has been created successfully.";
                }
                else
                {
                    strMsg = "Booking Request creation failed.";
                }
            }
            return Json(new { result = result, Msg = strMsg });

        }
    }
}