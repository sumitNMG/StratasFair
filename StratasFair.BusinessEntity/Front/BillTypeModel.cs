using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class BillTypeModel
    {
        public int BillTypeId { get; set; }

        [Required(ErrorMessage = "Bill Type required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Bill Type must be at least 5 character long")]
        public string BillTypeName { get; set; }

        [Required(ErrorMessage = "Occurrence required")]
        public Nullable<int> BillPeriod { get; set; }

        public string BillPeriodName { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
