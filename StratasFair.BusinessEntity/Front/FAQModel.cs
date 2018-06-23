using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StratasFair.BusinessEntity.Front
{
    public class FAQModel
    {
        public int FAQId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int SortOrder { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
    }
}
