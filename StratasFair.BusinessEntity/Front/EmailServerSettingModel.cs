using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class EmailServerSettingModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Sender display name required.")]
        public string SenderDisplayName { get; set; }

        [Required(ErrorMessage = "From email address required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email address must be at least 5 character long")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Valid email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string FromEmail { get; set; }

        [Required(ErrorMessage = "Email address required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email address must be at least 5 character long")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Valid email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string EmailForTest { get; set; }
      
    }
}
