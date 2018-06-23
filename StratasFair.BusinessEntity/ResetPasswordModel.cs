using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StratasFair.BusinessEntity
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email ID required.")]
        [MinLength(5, ErrorMessage = "Email ID must be 5 characters long")]
        [MaxLength(100, ErrorMessage = "Email ID should be less than 100 characters")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Required(ErrorMessage="Password is required")]
        [MinLength(6, ErrorMessage = "Password must be 6 characters long")]
        [MaxLength(20, ErrorMessage = "Max 20 characters")]
        [Compare("ConfirmPassword",ErrorMessage="Password and Confirm Password do not match")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [MinLength(6, ErrorMessage = "Confirm Password must be 6 characters long")]
        [MaxLength(20, ErrorMessage = "Max 20 characters")]
        public string ConfirmPassword { get; set; }
    }
}
