using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class PollChartModel
    {
        public long PollId { get; set; }
        public long PollOptionId { get; set; }
        public string PollOptionText { get; set; }
        public int ? PollCount { get;set;}
    }
}
