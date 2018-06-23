using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StratasFair.BusinessEntity.Front
{
    public class PollAnswerModel
    {
        public long PollAnswerId { get; set; }
        public Nullable<long> PollId { get; set; }
        public Nullable<long> PollOptionId { get; set; }
        public string PollOptionText { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
