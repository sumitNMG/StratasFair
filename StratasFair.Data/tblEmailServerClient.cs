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
    
    public partial class tblEmailServerClient
    {
        public int Id { get; set; }
        public string SenderDisplayName { get; set; }
        public string FromEmail { get; set; }
        public int StratasBoardId { get; set; }
        public Nullable<int> Active { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}