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
    
    public partial class vw_GetStratasBoardSubscription
    {
        public int StratasBoardSubscriptionId { get; set; }
        public Nullable<long> StratasBoardId { get; set; }
        public string StratasBoardName { get; set; }
        public Nullable<int> SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public string Validity { get; set; }
        public string ValidityText { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> AllowedUser { get; set; }
        public Nullable<bool> IsSmsForAlert { get; set; }
        public string SmsForAlertText { get; set; }
        public Nullable<bool> IsSmsForReminder { get; set; }
        public string SmsForReminderText { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string StatusText { get; set; }
    }
}