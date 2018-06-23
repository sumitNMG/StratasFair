using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class ChangePasswordModel : CommonModel
    {
        [Required(ErrorMessage = "Old password required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "New password required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "New password not valid (Only a-zA-Z0-9~!@$%^&*_-?).")]
        [DataType(DataType.Text)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Confirm new password required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Confirm new password not valid (Only a-zA-Z0-9~!@$%^&*_-?).")]
        [Compare("NewPassword", ErrorMessage = "New password and confirm new password should be same.")]
        [DataType(DataType.Text)]
        [Display(Name = "Confirm password")]
        public string ConfirmNewPassword { get; set; }
    }
}
