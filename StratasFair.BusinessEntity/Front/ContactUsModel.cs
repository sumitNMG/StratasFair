using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StratasFair.BusinessEntity.Front
{
    public class ContactUsModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="First Name is required")]
        [MinLength(2,ErrorMessage="Min 2 characters required")]
        [MaxLength(100, ErrorMessage = "Invalid First Name. Max 100 characters.")]
        [RegularExpression("^([a-zA-Z0-9' ']+)$", ErrorMessage = "First name not valid.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage="Email Address is required")]
        [MinLength(5, ErrorMessage = "Invalid Email. Min 5 characters required")]
        [MaxLength(100,ErrorMessage="Invalid Email. Max 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [MinLength(5, ErrorMessage = "Min 5 characters required")]
        [MaxLength(500, ErrorMessage = "Max 500 characters.")]
        public string Message { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
    }
}
