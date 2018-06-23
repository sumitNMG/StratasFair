using StratasFair.BusinessEntity.Admin;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.Business.Admin
{
    public class ReportsHelper
    {
        StratasFairDBEntities context;
        public ReportsHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }

        public int? GetPageCount(int? TotalPages, int? PageSize)
        {
            int? count = (TotalPages / PageSize) + (TotalPages % PageSize > 0 ? 1 : 0);
            return count;
        }

        /// <summary>
        /// Get the contact us query report
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ContactUsReportModel> GetContactUsReport(ContactUsReportModel model)
        {
            return context.Usp_ContactUsReportAdmin(model.KeywordSearch, model.PageNumber, model.PageSize).Select(x => new ContactUsReportModel
            {
                Sno = x.Sno,
                ContactUsId = x.ContactUsId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Message = x.Message,
                CreatedOn = x.CreatedOn,
                TotalCount = x.TotalCount.Value,
                Status = x.Status,
                EmailAddress = x.EmailAddress
            }).ToList();
        }

        /// <summary>
        /// Get the Upcoming renewal record (/* Subscription expiring within 15 days from the current date */)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<UpcomingRenewalReportModel> GetUpcomingRenewalReport(UpcomingRenewalReportModel model)
        {
            return context.Usp_StratasBoardStatusReportAdmin(model.KeywordSearch, model.PageNumber, model.PageSize, 1).Select(x => new UpcomingRenewalReportModel
            {
                Sno = x.Sno,
                StratasBoardId = x.StratasBoardId,
                StratasBoardName = x.StratasBoardName,
                PortalLink = x.PortalLink,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                TotalCount = x.TotalCount.Value,
                STATUS = x.STATUS,
                UserClientId = x.UserClientId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailId = x.EmailId,
                ContactNumber = x.ContactNumber,
                ExpiryDate = x.ExpiryDate,
                AllowedUser = x.AllowedUser,
                IsSmsForAlert = x.IsSmsForAlert,
                IsSmsForReminder = x.IsSmsForReminder,
                SubscriptionId = x.SubscriptionId,
                SubscriptionName = x.SubscriptionName,
                ValidityText = x.ValidityText
            }).ToList();
        }

        /// <summary>
        /// Get the Expired membership record (/* Subscription expired as compared to the current date */)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ExpiredMembershipReportModel> GetExpiredMembershipReport(ExpiredMembershipReportModel model)
        {
            return context.Usp_StratasBoardStatusReportAdmin(model.KeywordSearch, model.PageNumber, model.PageSize, 2).Select(x => new ExpiredMembershipReportModel
            {
                Sno = x.Sno,
                StratasBoardId = x.StratasBoardId,
                StratasBoardName = x.StratasBoardName,
                PortalLink = x.PortalLink,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                TotalCount = x.TotalCount.Value,
                STATUS = x.STATUS,
                UserClientId = x.UserClientId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailId = x.EmailId,
                ContactNumber = x.ContactNumber,
                ExpiryDate = x.ExpiryDate,
                AllowedUser = x.AllowedUser,
                IsSmsForAlert = x.IsSmsForAlert,
                IsSmsForReminder = x.IsSmsForReminder,
                SubscriptionId = x.SubscriptionId,
                SubscriptionName = x.SubscriptionName,
                ValidityText = x.ValidityText
            }).ToList();
        }
    }
}
