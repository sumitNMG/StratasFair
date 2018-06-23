using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class ReminderModel
    {
        public long ReminderId { get; set; }

        [Required(ErrorMessage = "Reminder Subject required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Reminder Subject must be at least 2 character long")]
        public string ReminderSubject { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "Email Message contains Maximum 500 characters")]
        public string ReminderNote { get; set; }

        [StringLength(140, MinimumLength = 0, ErrorMessage = "Text Message contains Maximum 140 characters")]
        public string SmsText { get; set; }

        [Required(ErrorMessage = "Occurrence required")]
        public int? Occurrence { get; set; }

         
        public string ReminderOccurrence { get; set; }

        [Required(ErrorMessage = "Please select Start Date")]
        public string StartDateTime { get; set; }

        public string StartTime { get; set; }

         [Required(ErrorMessage = "Please select Start Time Hour")]
        public int StartTimeHour { get; set; }

         [Required(ErrorMessage = "Please select Start Time Minute")]
         public int StartTimeMinute { get; set; }

        [Required(ErrorMessage = "Please select Start Time AM/PM")]
         public string StartTimeAmPm { get; set; }  

        public long EditReminderId { get; set; }
        public long CreatedBy { get; set; } 
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public int? Status { get; set; }
    }
}
