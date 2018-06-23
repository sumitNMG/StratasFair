using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class RequestPortalLinkModel
    {
        public string PortalLink { get; set; }


        [Required(ErrorMessage = "Unique Url required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Unique Url Length must be between 6 to 100.")]
        [DataType(DataType.Text)]
        [Display(Name = "RequestPortalLink :")]
        public string RequestPortalLink { get; set; }


        [Required(ErrorMessage = "Request Message required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Message Length must be between 6 to 100.")]
        [DataType(DataType.Text)]
        [Display(Name = "RequestMessage :")]
        public string RequestMessage { get; set; }

        public int RequestCounter { get; set; }
        public int TotalUser { get; set; }
        public List<string> StrataFairAdminEmails { get;set;}

    }
}
