using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.BusinessEntity.Front
{
    public class ClientLogOnModel : CommonModel
    {
        [Required(ErrorMessage = "Email address required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email address must be at least 5 character long")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Valid email address is required.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "User Email :")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Length must be between 6 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid")]
        [DataType(DataType.Password)]
        [Display(Name = "Password :")]
        public string Password { get; set; }

        public long UserClientId { get; set; }

        [Required(ErrorMessage = "First Name required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name required")]
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public long? StratasBoardId { get; set; }
        public string StrataBoardName { get; set; }
        public string BuildingName { get; set; }
        public string RoleName { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageType { get; set; }
        public string ProfilePicture { get; set; }
        public string OldProfilePicture { get; set; }
        public string LeaseCommenceDate { get; set; }
        public string LeaseEndDate { get; set; }
        [Required(ErrorMessage = "Unit Number required")]
        public string UnitNumber { get; set; }
        [Required(ErrorMessage = "Premise Type required")]
        public string PremiseType { get; set; }
        public bool IsProfileComplete { get; set; }
        public bool IsSMSNotification { get; set; }
        public bool IsEmailNotification { get; set; }
        public string StrataPortalLink { get; set; }
        public bool RememberMe
        {
            get;
            set;
        }

        public List<UserClientPrivilege> UserClientPrivilege { get; set; }
    }


    public class ClientForgetPasswordModel : CommonModel
    {
        [Required(ErrorMessage = "Email address required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email address must be at least 5 character long")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Valid email address is required.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address :")]
        public string Email { get; set; }
    }

    public class ClientChangePassword : CommonModel
    {
        [Required(ErrorMessage = "Old password required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Length must be between 6 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid")]
        [DataType(DataType.Password)]
        [Display(Name = "Old password :")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "New password required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Length must be between 6 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid")]
        [DataType(DataType.Password)]
        [Display(Name = "New password :")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Confirm password required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Length must be between 6 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid")]
        [Compare("NewPassword", ErrorMessage = "New password and confirm new password should be same.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password :")]
        public string ConfirmPassword { get; set; }

        public int UserClientId { get; set; }

    }
}
