using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Front
{
    public class AlertAndNotificationModel
    {
        public long AlertNotificationId { get; set; }

        [Required(ErrorMessage = "Type required")]
        public Nullable<int> MessageType { get; set; }

        public string MessageTypeName { get; set; }

        [Required(ErrorMessage = "Title required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be at least 2 character long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Message required")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Message must be at least 2 character long")]
        public string Message { get; set; }

        public string CreatedOn { get; set; }
    }
}
