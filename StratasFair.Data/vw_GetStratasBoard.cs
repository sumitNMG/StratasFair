//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StratasFair.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_GetStratasBoard
    {
        public long StratasBoardId { get; set; }
        public string StratasBoardName { get; set; }
        public string PortalLink { get; set; }
        public string RequestPortalLink { get; set; }
        public string RequestMessage { get; set; }
        public Nullable<System.DateTime> RequestCreatedOn { get; set; }
        public int RequestCounter { get; set; }
        public long RequestCreatedBy { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public string BuildingName { get; set; }
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
        public Nullable<int> StratasBoardSubscriptionId { get; set; }
        public string ValidityText { get; set; }
        public Nullable<int> TotalUser { get; set; }
    }
}
