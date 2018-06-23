using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers
{
    public class AdminBookingRequestController : Controller
    {
        // GET: AdminBookingRequest
        public ActionResult Index()
        {
            ViewBag.CommonAreaList = CommonData.GetCommonAreaList(ClientSessionData.ClientStrataBoardId);
            ViewBag.AdminStatusList = CommonData.GetAdminStatusList();
            return View();
        }

        public PartialViewResult LoadBookingList(string commonAreaId, string requestDate, string adminStatus, string pageNo, string pageSize = "10")
        {
            BookingRequestHelper helper = new BookingRequestHelper();
            BookingRequestModel model = new BookingRequestModel();
            model.StratasBoardId = ClientSessionData.ClientStrataBoardId;
            var bookingList = helper.GetBookingListing(model);


            if (!string.IsNullOrEmpty(commonAreaId))
                bookingList = bookingList.Where(x => x.CommonAreaId == int.Parse(commonAreaId)).ToList();

            if (!string.IsNullOrEmpty(requestDate))
                bookingList = bookingList.Where(x => x.CreatedOn.ToShortDateString() == DateTime.Parse(requestDate).ToShortDateString()).ToList();

            if (!string.IsNullOrEmpty(adminStatus))
                bookingList = bookingList.Where(x => x.AdminStatus == int.Parse(adminStatus)).ToList();


            int totalRecords = bookingList.Count;

            BookingRequestModelView bookingRequestModel = new BookingRequestModelView();

            bookingRequestModel.TotalPages = totalRecords / Convert.ToInt32(pageSize);
            if (totalRecords % Convert.ToInt32(pageSize) > 0)
                bookingRequestModel.TotalPages++;

            var skip = Convert.ToInt32(pageSize) * (Convert.ToInt32(pageNo) - 1);
            bookingRequestModel.ListBooking = bookingList.Skip(skip).Take(Convert.ToInt32(pageSize)).ToList();

            return PartialView("_AdminBookingListPartial", bookingRequestModel);


        }


      
        public JsonResult UpdateStatus(string bookingRequestId, string reason, string adminStatus)
        {
            string msg = "ok";
            int data = -1;
            try
            {
                BookingRequestModel model = new BookingRequestModel();
                model.BookingRequestId = Convert.ToInt64(bookingRequestId);
                if (Convert.ToInt32(adminStatus) == 2)
                    model.AdminRemark = reason;
                model.AdminStatus = Convert.ToInt32(adminStatus);  // 1= Approved, 2= Rejected

                if (Convert.ToInt64(bookingRequestId) > 0 && (Convert.ToInt32(adminStatus) == 1 || Convert.ToInt32(adminStatus) == 2))
                {
                    BookingRequestHelper helper = new BookingRequestHelper();
                    int result = helper.UpdateAdminBookingStatus(model);
                    if (result == 1)
                    {

                        data = 0;

                        if (Convert.ToInt32(adminStatus) == 1)
                            msg = "You have approved request successfully.";
                        else
                        {
                            msg = "You have rejected request successfully.";    
                        }

                        //"Send email to owner for the status updated by stratasboard admin"
                        string emailStatus = EmailSender.BookingRequestUpdateMail(Convert.ToInt64(bookingRequestId), ClientSessionData.ClientStrataBoardId);
                        if (emailStatus == "success")
                        {
                            // Mail sent successfullly
                        }
                        else
                        {
                            // Mail is not delievered 
                        } 
                            
                    }
                    else
                    {
                        data = -4;
                        msg = "Error! try again later.";
                    }
                }
                else
                {
                    data = -3;
                    msg = "Error! try again later.";
                }
            }
            catch (Exception ex)
            {
                data = -2;
                msg = ex.Message;
            }
            return Json(new { Msg = msg, Counter = data }, JsonRequestBehavior.AllowGet);
        }

     
    }
}