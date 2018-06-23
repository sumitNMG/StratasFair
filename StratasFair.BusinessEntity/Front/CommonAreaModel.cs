using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class CommonAreaModel
    {
        public int CommonAreaId { get; set; }

        [Required(ErrorMessage = "Resource/Common Area required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Resource/Common Area must be at least 2 character long")]
        public string CommonAreaName { get; set; }

        public Nullable<long> CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
