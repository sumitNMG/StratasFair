using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Admin
{
    public class FAQModel : CommonModel
    {
        [Key]
        public int FAQId { get; set; }

        //[Required(ErrorMessage = "Category required.")]
        //public int FaqCategoryId { get; set; }
        //public string Category { get; set; }

        [Required(ErrorMessage = "Question required.")]
        //[RegularExpression(@"^([a-zA-Z0-9' '\.\?&_-]+)$", ErrorMessage = "Special characters are not allowed (Only a-zA-Z0-9' '.?&_-).")]
        public string Question { get; set; }


        [Required(ErrorMessage = "Answer required.")]
        [AllowHtml]
        public string Answer { get; set; }



        [Required(ErrorMessage = "Sort order required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid number.")]
        public int SortOrder { get; set; }


        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
