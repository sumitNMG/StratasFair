using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class EmailTemplateModel : CommonModel
    {
        [Key]
        public int TemplateMasterId { get; set; }
        [Required(ErrorMessage = "Title is Required.")]
        [RegularExpression(@"^([a-zA-Z0-9' '\.&_-]+)$", ErrorMessage = "Title is not valid (Only a-zA-Z_-).")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Name is Required.")]
        [RegularExpression(@"^([a-zA-Z0-9' '\.&_-]+)$", ErrorMessage = "Name is not valid (Only a-zA-Z_-).")]
        public string Name { get; set; }
        public string ConfigValue { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
}
