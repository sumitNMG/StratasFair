using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace StratasFair.BusinessEntity.Front
{
    public class VendorModel
    {
        [Key]
        public long VendorId { get; set; }

        [Required(ErrorMessage = "Vendor name required.")]
        [MinLength(5, ErrorMessage = "Min 2 characters")]
        [MaxLength(100, ErrorMessage = "Max 100 characters")]
        [RegularExpression("^([a-zA-Z0-9' ']+)$", ErrorMessage = "Vendor name not valid.")]
        [DataType(DataType.Text)]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Email ID required.")]
        [MinLength(5, ErrorMessage = "Email ID must be 5 characters long")]
        [MaxLength(100, ErrorMessage = "Email ID should be less than 100 characters")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be 6 characters long")]
        [MaxLength(20, ErrorMessage = "Max 20 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Descipline required.")]
        public Nullable<int> DisciplineId { get; set; }

        public string DisciplineName { get;set;}

        [MinLength(2, ErrorMessage = "Min 2 characters required")]
        [MaxLength(100, ErrorMessage = "Max 100 characters required")]
        public string OtherDisciplineName { get; set; }

        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessage = "Invalid mobile number")]
        [Required(ErrorMessage = "Mobile number is required")]
        [MinLength(10, ErrorMessage = "Invalid mobile. Min 10 characters required")]
        [MaxLength(15, ErrorMessage = "Invalid mobile. Max 15 characters")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Company brief is required")]
        [MinLength(5, ErrorMessage = "Min 5 characters required")]
        [MaxLength(500, ErrorMessage = "Max 500 characters")]
        public string CompanyBrief { get; set; }

        public string TradeAndBusinessFile { get; set; }
        public string ActualTradeAndBusinessFile { get; set; }
        public string ImageFile { get; set; }
        public string ActualImageFile { get; set; }
        public Nullable<int> AdminApproval { get; set; }
        public string Remark { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedFromIp { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<long> PasswordResetStatus { get; set; }
        public Nullable<DateTime> PasswordResetDate { get; set; }
    }

    public class VendorLoginModel
    {
        [Required(ErrorMessage = "Email ID required.")]
        [MinLength(5, ErrorMessage = "Email ID must be 5 characters long")]
        [MaxLength(100, ErrorMessage = "Email ID should be less than 100 characters")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be 6 characters long")]
        [MaxLength(20, ErrorMessage = "Max 20 characters")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
