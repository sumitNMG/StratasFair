using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.BusinessEntity
{
    public class TestimonialModel: CommonModel
    {
        [Key]
        public int TestimonialId { get; set; }

        [Required(ErrorMessage = "Name required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 100.")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Designation required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 100.")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Description required.")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 500.")]
        public string Description { get; set; }

        public string ImageFile { get; set; }
        public string OldImageFile { get; set; }
        public string ActualImageFile { get; set; }

       // [Required(ErrorMessage = "Image file required.")]
        [FileExtensions(ErrorMessage = "Must choose .png,.jpg file.", Extensions = "png, jpg, jpeg")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageType { get; set; }

        [Required(ErrorMessage = "Display order required.")]
        public int DisplayOrder { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }

    }
}
