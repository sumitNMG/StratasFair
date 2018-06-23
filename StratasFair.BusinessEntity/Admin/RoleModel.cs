using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class RoleModel : CommonModel
    {
        [Required(ErrorMessage = "Role name required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 50.")]
        [RegularExpression("^([a-zA-Z0-9' ']+)$", ErrorMessage = "Role name not valid.")]
        [DataType(DataType.Text)]
        [Display(Name = "Role name")]
        public string RoleName { get; set; }


        [Required(ErrorMessage = "Role description required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 100.")]
        [DataType(DataType.Text)]
        [Display(Name = "Role description")]
        public string RoleDescription { get; set; }

        public string RoleType { get; set; }
    }
}
