using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Admin
{
    public class LogOnModel : CommonModel
    {
        [Required(ErrorMessage = "Username required.")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 16.")]
        [RegularExpression("^([a-zA-Z0-9.]+)$", ErrorMessage = "Username not valid.")]
        [DataType(DataType.Text)]
        [Display(Name = "Username :")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid")]
        [DataType(DataType.Password)]
        [Display(Name = "Password :")]
        public string Password { get; set; }
        public string Message { get; set; }
        public string UserType { get; set; }
        public bool RememberMe
        {
            get;
            set;
        }
    }


    public class ForgetPasswordModel : CommonModel
    {
        [Required(ErrorMessage = "Email address required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email address must be at least 5 character long")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Valid email address is required.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address :")]
        public string Email { get; set; }
    }

    public class ChangePassword : CommonModel
    {
        [Required(ErrorMessage = "Current password required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password :")]
        public string CurrentPassword { get; set; }


        [Required(ErrorMessage = "New password required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid")]
        [DataType(DataType.Password)]
        [Display(Name = "New password :")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Confirm password required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password :")]
        public string ConfirmPassword { get; set; }

        public long UserId { get; set; }

    }
}