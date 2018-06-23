using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StratasFair.BusinessEntity
{
    public class VendorSearchModel
    {
        [MinLength(1,ErrorMessage="Min 1 character required")]
        [MaxLength(100, ErrorMessage="Max 100 characters")]
        public string keyword{get;set;}
        public int? disciplineid { get; set; }
    }
}
