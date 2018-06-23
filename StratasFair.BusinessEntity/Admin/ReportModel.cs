using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class ReportModel
    {
    }

    public class ContactUsReportModel
    {
        public Nullable<long> Sno { get; set; }
        public long ContactUsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> Status { get; set; }
        public int TotalCount { get; set; }

        public string KeywordSearch { get; set; }
        public Nullable<int> PageNumber { get; set; }
        public Nullable<int> PageSize { get; set; }
    }

    public class UpcomingRenewalReportModel
    {
        public Nullable<long> Sno { get; set; }
        public long StratasBoardId { get; set; }
        public string StratasBoardName { get; set; }
        public string PortalLink { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> STATUS { get; set; }
        public Nullable<long> UserClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> AllowedUser { get; set; }
        public Nullable<bool> IsSmsForAlert { get; set; }
        public Nullable<bool> IsSmsForReminder { get; set; }
        public Nullable<int> SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public string ValidityText { get; set; }
        public Nullable<int> TotalCount { get; set; }

        public string KeywordSearch { get; set; }
        public Nullable<int> PageNumber { get; set; }
        public Nullable<int> PageSize { get; set; }
    }


    public class ExpiredMembershipReportModel
    {
        public Nullable<long> Sno { get; set; }
        public long StratasBoardId { get; set; }
        public string StratasBoardName { get; set; }
        public string PortalLink { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> STATUS { get; set; }
        public Nullable<long> UserClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> AllowedUser { get; set; }
        public Nullable<bool> IsSmsForAlert { get; set; }
        public Nullable<bool> IsSmsForReminder { get; set; }
        public Nullable<int> SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public string ValidityText { get; set; }
        public Nullable<int> TotalCount { get; set; }

        public string KeywordSearch { get; set; }
        public Nullable<int> PageNumber { get; set; }
        public Nullable<int> PageSize { get; set; }
    }
}
