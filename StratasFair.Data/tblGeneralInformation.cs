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
    
    public partial class tblGeneralInformation
    {
        public long GeneralInformationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UploadFile { get; set; }
        public string ActualUploadFile { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<long> StratasBoardId { get; set; }
    }
}