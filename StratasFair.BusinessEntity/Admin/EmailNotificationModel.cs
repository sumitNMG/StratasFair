using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class EmailNotificationModel : CommonModel
    {
        public int AdminEmailNotificationSettingId { get; set; }

        [Required(ErrorMessage = "Email address required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email address must be at least 5 character long")]
        [RegularExpression(@"^([\w+-.%]+@[\w-.]+\.[A-Za-z]{2,4},*[\W]*)+$", ErrorMessage = "Valid email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }


        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public int Status { get; set; }
    }
}
