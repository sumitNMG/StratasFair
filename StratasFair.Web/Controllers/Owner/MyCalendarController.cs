using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StratasFair.Web.Controllers.Owner
{
    [ClientSessionExpire]
    [StrataBoardOwner]
    public class MyCalendarController : Controller
    {
        // GET: MyCalendar
        public ActionResult Index()
        {
            ViewBag.CommonAreaList = CommonData.GetCommonAreaList(ClientSessionData.ClientStrataBoardId);
            return View();
        }


        /// <summary>
        /// Add the common area bookings
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddCommonAreaBooking(StrataOwnerBookingRequestModel model)
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
                int result = strataOwnerHelper.AddStrataOwnerBookingRequest(model);
                if (result == 1)
                {
                    val = 0;
                    strMsg = "Booking request has been created successfully.";
                }
                else if (result == -1)
                {
                    strMsg = "Booking request already exists to this User for given dates.";
                }
                else
                {
                    strMsg = "Booking request creation failed.";
                }
            }


            return Json(new { Msg = strMsg, Result = val }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Get the Owner calendar list
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetOwnerGeneralCalendarList(string type)
        {
            UserCalendarHelper helper = new UserCalendarHelper();
            var obj = helper.GetUserCalendarData(type, ClientSessionData.UserClientId, ClientSessionData.ClientStrataBoardId);
            string json = new JavaScriptSerializer().Serialize(obj);
            return Json(new { resultSet = json }, JsonRequestBehavior.AllowGet);
        }

      
    }
}