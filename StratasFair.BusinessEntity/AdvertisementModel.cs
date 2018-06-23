using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity
{
    public class AdvertisementModel
    {
        public long AdvertisementId { get; set; }
        public string ImageFile { get; set; }
        public string RedirectionUrl { get; set; }
        public int ? DisplayOrder { get; set; }
        public int? Status { get; set; }
    }
}
