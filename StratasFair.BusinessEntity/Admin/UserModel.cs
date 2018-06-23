using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class UserModel
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


        [Required(ErrorMessage = "Password required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Password not valid (Only a-zA-Z0-9~!@$%^&*_-?).")]
        [DataType(DataType.Text)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm password required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20.")]
        [RegularExpression("^([a-zA-Z0-9~!@$%^&*_?-]+)$", ErrorMessage = "Confirm password not valid (Only a-zA-Z0-9~!@$%^&*_-?).")]
        [Compare("Password", ErrorMessage = "Confirm password and password should be same.")]
        [DataType(DataType.Text)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Gender required.")]
        public string Gender { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Date of birth")]
        public string DOB { get; set; }
        public string DOBMMDDYYYY { get; set; }


        public string SearchKeywords { get; set; }
        public int Flag { get; set; }


        [Required(ErrorMessage = "Role required.")]
        public int RoleId { get; set; }
        public long UserId { get; set; }
        public int Status { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedFromIp { get; set; }
        public string Message { get; set; }
        public int IsShowMessage { get; set; }
        public int PageSize { get; set; }
        public int SerialNumber { get; set; }
        public List<System.Web.Mvc.SelectListItem> ListStatus { get; set; }

        public string CreatedOn { get; set; }
        public string RoleName { get; set; }
        public bool EnableDisable { get; set; }
        public string MessageClass { get; set; }
        public char UserType { get; set; }
        public int IsSuper { get; set; }

        [Display(Name = "Select role")]
        public List<RoleModel> ListRole { get; set; }
       // public List<CompanyModel> ListCompanyClient { get; set; }
        public int IsClientAssigned { get; set; }

        public string LastLogin { get; set; }
        public string LastSessionLength { get; set; }
        public string AverageSessionLength { get; set; }
    }
}
