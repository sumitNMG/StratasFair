using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class SettingsModel : CommonModel
    {
        [Key]
        public int SettingId { get; set; }

        [Required(ErrorMessage = "Setting Name is Required")]
        [RegularExpression(@"^([a-zA-Z0-9' '\.&_-]+)$", ErrorMessage = "Setting Name not valid.")]
        public string SettingName { get; set; }

        [Required(ErrorMessage = "Setting Description is Required")]
        public string SettingDescription { get; set; }

        [Required(ErrorMessage = "Setting Value is Required")]
        public string SettingValue { get; set; }

        public string DataType { get; set; }
        public int Status { get; set; }
    }
}
