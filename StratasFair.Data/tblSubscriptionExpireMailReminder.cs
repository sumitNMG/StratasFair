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
    
    public partial class tblSubscriptionExpireMailReminder
    {
        public long ReminderId { get; set; }
        public Nullable<long> StratasBoardId { get; set; }
        public Nullable<int> StratasBoardSubscriptionId { get; set; }
        public Nullable<int> ReminderLevel { get; set; }
        public string ReceiverEmailId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
