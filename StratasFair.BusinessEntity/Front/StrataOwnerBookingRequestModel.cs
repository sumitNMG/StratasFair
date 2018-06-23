using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Front
{
    public class StrataOwnerBookingRequestModel
    {
        public long BookingRequestId { get; set; }
        public long UserClientId { get; set; }

        [Required(ErrorMessage = "Common area required.")]
        public int CommonAreaId { get; set; }

        [Required(ErrorMessage = "Subject required.")]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "Subject must be at least 5 character long")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "FromDate required.")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "ToDate required.")]
        public string ToDate { get; set; }

        [Required(ErrorMessage = "Reason required.")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Reason must be at least 2 character long")]
        public string Reason { get; set; }

        public string CommonAreaName { get; set; }
        public string Status { get; set; }
        public string AdminStatus { get; set; }
        public int TotalDays { get; set; }
        public SelectList CommonAreas { get; set; }
        public string CreatedOn { get; set; }
    }
}
