using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StratasFair.BusinessEntity
{
    public class VendorQuotationModel
    {
        [Key]
        public long QuotationId { get; set; }
        [Required(ErrorMessage = "Requester name required")]
        [MinLength(5, ErrorMessage = "Min 2 character required")]
        [MaxLength(100, ErrorMessage = "Max 100 characters")]
        public string RequesterName { get; set; }

        [Required(ErrorMessage = "Requester email required")]
        [MinLength(5, ErrorMessage = "Min 5 character required")]
        [MaxLength(100, ErrorMessage = "Max 100 characters")]
        public string RequesterEmailId { get; set; }
        
        public Nullable<bool> IsShowEmail { get; set; }

        [Required(ErrorMessage="Discipline required")]
        public Nullable<long> DisciplineId { get; set; }

        public string DisciplineName{get;set;}

        public Nullable<long> VendorId { get; set; }

        [Required(ErrorMessage="Vendor required")]
        public string VendorList { get;set;}

        public string VendorName{get;set;}
        [Required(ErrorMessage="Details required")]
        [MinLength(5,ErrorMessage="Min 5 characters")]
        [MaxLength(500,ErrorMessage="Max 500 characters")]
        public string Details { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<long> StrataBoardId { get; set; }
        public string StrataBoardName { get; set; }
    }
}
