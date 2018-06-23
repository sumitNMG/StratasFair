using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class DashBoardModel
    {
        public List<AlertAndNotificationModel> AlertAndNotificationModelList { get; set; }
        public List<ScheduleMeetingModel> ScheduleMeetingModelList { get; set; }
        public List<MaintenanceScheduleModel> MaintenanceScheduleModelList { get; set; }
        public List<StrataOwnerBookingRequestModel> StrataOwnerBookingRequestModelList { get; set; }
    }
}
