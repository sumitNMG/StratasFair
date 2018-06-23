using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.BusinessEntity.Admin
{
    public class ManageAdvertisementModel
    {
        [Key]
        public int AdvertisementId { get; set; }

        [Required(ErrorMessage = "Redirection url required.")]
        [Url(ErrorMessage = "Invalid URL.")]
        public string RedirectionUrl { get; set; }


        public string ActualImageFile { get; set; }
        public string ImageFile { get; set; }
        public string OldImageFile { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageType { get; set; }

        [Required(ErrorMessage = "Display order required.")]
        public int DisplayOrder { get; set; }

        public Nullable<int> Status { get; set; }

        public Int64 CreatedBy { get; set; }
        public Int64 ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public string CreatedFromIp { get; set; }
        public string ModifiedFromIP { get; set; }
     
    }
}
