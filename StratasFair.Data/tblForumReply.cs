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
    
    public partial class tblForumReply
    {
        public tblForumReply()
        {
            this.tblFlagComments = new HashSet<tblFlagComment>();
        }
    
        public long ReplyId { get; set; }
        public long TopicId { get; set; }
        public Nullable<long> StratasBoardId { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public string Message { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsFlagged { get; set; }
    
        public virtual ICollection<tblFlagComment> tblFlagComments { get; set; }
        public virtual tblForum tblForum { get; set; }
    }
}
