using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class ProfileModel : CommonModel
    {
        [Required(ErrorMessage = "First name required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 100.")]
        [RegularExpression("^([a-zA-Z0-9' ']+)$", ErrorMessage = "First name not valid.")]
        [DataType(DataType.Text)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last name required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 100.")]
        [RegularExpression("^([a-zA-Z0-9' ']+)$", ErrorMessage = "Last name not valid.")]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email ID required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 100.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Valid email ID required.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }


        [Required(ErrorMessage = "Login ID required.")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 16.")]
        [RegularExpression("^([a-zA-Z0-9.]+)$", ErrorMessage = "User name not valid.")]
        [DataType(DataType.Text)]
        [Display(Name = "Login ID")]
        public string LoginId { get; set; }

        public long UserId { get; set; }
        public char UserType { get; set; }

    }
}
