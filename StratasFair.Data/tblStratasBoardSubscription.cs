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
    
    public partial class tblStratasBoardSubscription
    {
        public int StratasBoardSubscriptionId { get; set; }
        public Nullable<long> StratasBoardId { get; set; }
        public Nullable<int> SubscriptionId { get; set; }
        public string Validity { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> AllowedUser { get; set; }
        public Nullable<bool> IsSmsForAlert { get; set; }
        public Nullable<bool> IsSmsForReminder { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
