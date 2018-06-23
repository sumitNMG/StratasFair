using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Front
{
    public class ScheduleMeetingModel
    {
        public long MeetingId { get; set; }

        [Required(ErrorMessage = "Subject required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Subject must be at least 2 character long")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message required")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Message must be at least 5 character long")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Meeting Date required")]
        public string MeetingDate { get; set; }

        public string ScheduleMeetingInviteIds { get; set; }
        public List<StrataOwnerModel> StrataOwners { get; set; }

        public string SelectedMeetingInvite { get; set; }

        [Required(ErrorMessage = "Start time hour required")]
        public int StartTimeHour { get; set; }
        [Required(ErrorMessage = "Start time minute required")]
        public int StartTimeMinute { get; set; }

        public string StartTimeAmPm { get; set; }

        public string MeetingTime { get; set; }
        
        public string CreatedOn { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
