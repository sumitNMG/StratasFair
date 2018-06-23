using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Admin
{
    public class CMSModel : CommonModel
    {
        public CMSModel()
        {
            this.ModifiedFromIp = HttpContext.Current.Request.UserHostAddress;
        }

        public int ContentId { get; set; }
        [Required(ErrorMessage = "Page Name Required")]
        [RegularExpression(@"^([a-zA-Z0-9' '\.&_-]+)$", ErrorMessage = "Page Name is not valid (Only a-zA-Z0-9_-).")]
        public string PageName { get; set; }
        [Required(ErrorMessage = "Page Url Required")]
        [RegularExpression(@"^(http(s)?://)?([\w-]+\.)?([\w-]+\.)+[\w-]+([/])+(.*)?", ErrorMessage = "Invalid Url")]
        public string PageUrl { get; set; }
        [Required(ErrorMessage = "Meta Title is Required")]
        [RegularExpression(@"^([a-zA-Z0-9' '\.&_-]+)$", ErrorMessage = "Meta Title is not valid (Only a-zA-Z0-9_-.).")]
        public string MetaTitle { get; set; }
        [Required(ErrorMessage = "Meta Description is Required")]
        [RegularExpression(@"^([a-zA-Z0-9' '\.&_-]+)$", ErrorMessage = "Meta Description is not valid (Only a-zA-Z0-9_-.@).")]
        public string MetaDescription { get; set; }
        [Required(ErrorMessage = "Meta Keyword is Required")]
        [RegularExpression(@"^([a-zA-Z0-9' '\.&_-]+)$", ErrorMessage = "Meta Keyword is not valid (Only a-zA-Z0-9_-.).")]
        public string MetaKeyword { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Meta Content is Required")]
        public string Content { get; set; }
        public int Status { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedFromIp { get; set; }

    }
}
