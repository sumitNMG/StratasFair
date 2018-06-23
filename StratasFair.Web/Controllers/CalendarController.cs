using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StratasFair.Web.Controllers
{
    [ClientSessionExpire]
    [StrataBoardAdmin]
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AdminCommonArea()
        {
            ViewBag.CommonAreaList = CommonData.GetCommonAreaList(ClientSessionData.ClientStrataBoardId);
            return View();
        }

        public JsonResult AddCommonAreaBooking(BookingRequestModel model)
        {
            string strMsg = "NotOk";
            int val = -1;

            if (model.CommonAreaId == 0)
            {
                strMsg = "Common area required.";
            }
            else
            {
                StrataOwnerHelper strataOwnerHelper = new StrataOwnerHelper();
                model.Status = "1";  
                model.AdminStatus = 1; // As area is booked by admin then no need for approval
                int result = strataOwnerHelper.AddStrataAdminBookingRequest(model);   
                if (result == 1)
                {
                    val = 0;
                    strMsg = "Booking request has been created successfully.";
                }
                else if (result == -1)
                {
                    strMsg = "Booking request already exists in these dates.";
                }
                else
                {
                    strMsg = "Booking request creation failed.";
                }
            }


            return Json(new { Msg = strMsg, Result = val }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetAdminCalendarList(string type)
        {
            UserCalendarHelper helper = new UserCalendarHelper();
            var obj = helper.GetUserCalendarData(type, ClientSessionData.UserClientId, ClientSessionData.ClientStrataBoardId);
            string json = new JavaScriptSerializer().Serialize(obj);
            return Json(new { resultSet = json }, JsonRequestBehavior.AllowGet);
        }
    }
}