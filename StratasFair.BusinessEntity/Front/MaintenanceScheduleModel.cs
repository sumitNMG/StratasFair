using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Front
{
    public class MaintenanceScheduleModel
    {
        public long MaintenanceScheduleId { get; set; }

        [Required(ErrorMessage = "Resource/Common area required.")]
        public int CommonAreaId { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Description must be at least 2 character long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Meeting Date required")]
        public string ScheduleDate { get; set; }

        public string CreatedOn { get; set; }
        public string CommonAreaName { get; set; }

        [Required(ErrorMessage = "Start time hour required")]
        public int StartTimeHour { get; set; }

        [Required(ErrorMessage = "Start time minute required")]
        public int StartTimeMinute { get; set; }

        public string StartTimeAmPm { get; set; }

        public string StartTime { get; set; }

    }
}
