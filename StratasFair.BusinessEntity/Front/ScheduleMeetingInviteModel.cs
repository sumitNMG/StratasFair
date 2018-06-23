using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class ScheduleMeetingInviteModel
    {
        public long MeetingInviteId { get; set; }
        public Nullable<long> MeetingId { get; set; }
        public Nullable<long> UserClientId { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
