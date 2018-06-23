using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class GeneralInformationModel
    {
        public long GeneralInformationId { get; set; }

        [Required(ErrorMessage = "Title required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be at least 2 character long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description required")]
        public string Description { get; set; }

        public string UploadFile { get; set; }
        public string OldUploadFile { get; set; }
        public string ActualUploadFile { get; set; }
        public string CreatedOn { get; set; }
    }
}
