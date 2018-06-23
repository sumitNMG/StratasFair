using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.Business.Front
{
    public class BookingRequestHelper
    {
        StratasFairDBEntities context;
        public BookingRequestHelper()
        {
            if (context == null)
            {
                context = new StratasFairDBEntities();
            }
        }

        /// <summary>
        /// Get the booking list for Stratasboard Admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<BookingRequestModel> GetBookingListing(BookingRequestModel model)
        {

            var bookingList = context.vw_GetBookingList.Where(x => x.StratasBoardId == model.StratasBoardId && x.STATUS == 1).OrderByDescending(x => x.CreatedOn).ToList();
            List<BookingRequestModel> objProjListingModel = new List<BookingRequestModel>();
            foreach (var item in bookingList)
            {
                BookingRequestModel objBookingList = new BookingRequestModel
                {
                    BookingRequestId = item.BookingRequestId,
                    UserClientId = item.UserClientId,
                    CommonAreaId = item.CommonAreaId.Value,
                    CommonAreaName= item.CommonAreaName,
                    Subject = item.Subject,
                    FromDate = item.FromDate.ToString(),
                    ToDate = item.ToDate.Value.ToString(),
                    Reason = item.Reason,
                    Status = item.STATUS.ToString(),
                    AdminStatus = item.AdminStatus.Value,
                    AdminRemark = item.AdminRemark,
                    AdminUserId = item.AdminUserId.Value,
                    CreatedOn = item.CreatedOn.Value,
                    ModifiedOn = item.ModifiedOn.Value,
                    StratasBoardId = item.StratasBoardId.Value,
                    NoOfDay = item.NoOfDay.Value,
                    AdminStatusText = item.AdminStatusText,
                    RequestType = item.RequestType
                };
                objProjListingModel.Add(objBookingList);
            }
            return objProjListingModel;

        }


        /// <summary>
        /// Update the booking status from StratasBoard Admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateAdminBookingStatus(BookingRequestModel model)
        {
            try
            {

                tblUserBookingRequest objModel = new tblUserBookingRequest();
                objModel = context.tblUserBookingRequests.Where(x => x.BookingRequestId == model.BookingRequestId && x.Status == 1).FirstOrDefault();
                objModel.AdminUserId = ClientSessionData.UserClientId;
                objModel.ModifiedOn = DateTime.UtcNow;
                objModel.AdminRemark = model.AdminRemark;
                objModel.AdminStatus = model.AdminStatus;

                context.tblUserBookingRequests.Attach(objModel);
                context.Entry(objModel).Property(x => x.AdminUserId).IsModified = true;
                context.Entry(objModel).Property(x => x.ModifiedOn).IsModified = true;
                context.Entry(objModel).Property(x => x.AdminRemark).IsModified = true;
                context.Entry(objModel).Property(x => x.AdminStatus).IsModified = true;
                return context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }


        /// <summary>
        /// Get the booking listing on stratasboard admin dashboard..
        /// </summary>
        /// <param name="stratasBoardId"></param>
        /// <returns></returns>
        public List<BookingRequestModel> GetAdminDashboardNewBookingListing(long stratasBoardId)
        {

            var bookingList = context.vw_GetBookingList.Where(x => x.StratasBoardId == stratasBoardId && x.STATUS == 1 && x.AdminStatus == 0 && x.RequestType == "bookingrequest").OrderByDescending(x => x.CreatedOn).ToList();
            List<BookingRequestModel> objProjListingModel = new List<BookingRequestModel>();
            foreach (var item in bookingList)
            {
                BookingRequestModel objBookingList = new BookingRequestModel
                {
                    BookingRequestId = item.BookingRequestId,
                    UserClientId = item.UserClientId,
                    CommonAreaId = item.CommonAreaId.Value,
                    CommonAreaName = item.CommonAreaName,
                    Subject = item.Subject,
                    FromDate = item.FromDate.ToString(),
                    ToDate = item.ToDate.Value.ToString(),
                    Reason = item.Reason,
                    Status = item.STATUS.ToString(),
                    AdminStatus = item.AdminStatus.Value,
                    AdminRemark = item.AdminRemark,
                    AdminUserId = item.AdminUserId.Value,
                    CreatedOn = item.CreatedOn.Value,
                    ModifiedOn = item.ModifiedOn.Value,
                    StratasBoardId = item.StratasBoardId.Value,
                    NoOfDay = item.NoOfDay.Value,
                    AdminStatusText = item.AdminStatusText,
                    RequestType = item.RequestType
                };
                objProjListingModel.Add(objBookingList);
            }
            return objProjListingModel;

        }

        //


    }
}
