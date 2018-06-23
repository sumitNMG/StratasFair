using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity
{
    public class StrataBoardModel: CommonModel
    {
        [Key]
        public long StratasBoardId { get; set; }

        [Required(ErrorMessage = "Strata name required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 50.")]
        [DataType(DataType.Text)]
        [Display(Name = "Strata name")]
        public string StratasBoardName { get; set; }

        [Required(ErrorMessage = "Building Name/Address required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 100.")]
        [DataType(DataType.Text)]
        [Display(Name = "Building Name/Address")]
        public string BuildingName { get; set; }


        [Required(ErrorMessage = "Unique name required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 50.")]
        [RegularExpression("^([a-zA-Z0-9]+)$", ErrorMessage = "Unique name must be alphanumeric only.")]
        [DataType(DataType.Text)]
        [Display(Name = "Unique name")]
        public string PortalLink { get; set; }
        public string OldPortalLink { get; set; }



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


        [Required(ErrorMessage = "Email address required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 100.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Valid email address required.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string EmailId { get; set; }


        [Required(ErrorMessage = "Contact number required")]
        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessage = "Invalid mobile number")]
        [MinLength(7, ErrorMessage = "Contact number should be 7-15 characters long")]
        [MaxLength(15, ErrorMessage = "Contact number should be 7-15 characters long")]
        public string ContactNumber { get; set; }

        public string RoleName { get; set; }
        public int IsEmailNotification { get; set; }
        public int IsSMSNotification { get; set; }
        public int IsProfileComplete { get; set; }

        public bool IsSmsForAlert { get; set; }
        public bool IsSmsForReminder { get; set; }

        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public string ValidityText { get; set; }

        public int AllowedUser { get; set; }
        public string ExpiryDate { get; set; }
        public DateTime ExpiredOnFrom { get; set; }
        public DateTime ExpiredOnTo { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }

        public long UserClientId { get; set; }
        public string Keyword { get; set; }


        public string RequestPortalLink { get; set; }
        public string RequestMessage { get; set; }
        public string RequestCreatedOn { get; set; }
        public int RequestCounter { get; set; }
        public long RequestCreatedBy { get; set; }
    }

    
}
