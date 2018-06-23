using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class StrataOwnerMyRequestModel
    {
        public int RequestId { get; set; }
        public int UserClientId { get; set; }

        [Required(ErrorMessage = "Request Title required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Request Title must be at least 2 character long")]
        public string RequestTitle { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }

        [Required(ErrorMessage = "Request Details required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Request Details must be at least 5 character long")]
        public string Details { get; set; }
        public string Status { get; set; }
        public string StatusRemark { get; set; }

        public int TotalDays { get; set; }
    }
}
