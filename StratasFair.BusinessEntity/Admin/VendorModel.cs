using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Admin
{
    public class VendorModel:CommonModel
    {
        [Key]
        public long VendorId { get; set; }

        [Required(ErrorMessage = "Vendor name required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 5 to 100.")]
        [RegularExpression("^([a-zA-Z0-9' '-.%()&]+)$", ErrorMessage = "Vendor name not valid.")]
        [DataType(DataType.Text)]
        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }


        [Required(ErrorMessage = "Discipline required.")]
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public string OtherDisciplineName { get; set; }

        [Required(ErrorMessage = "Email address required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email address must be at least 5 character long")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email address not valid.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address :")]
        public string EmailId { get; set; }


        [Required(ErrorMessage = "Mobile number required")]
        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessage = "Invalid mobile number")]
        [MinLength(7, ErrorMessage = "Mobile number should be 7-15 characters long")]
        [MaxLength(15, ErrorMessage = "Mobile number should be 7-15 characters long")]
        public string MobileNumber { get; set; }



        [Required(ErrorMessage = "Company brief required.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Length must be between 2 to 1000.")]        
        [DataType(DataType.Text)]
        [Display(Name = "Company brief")]
        public string CompanyBrief { get; set; }




        public string ActualTradeAndBusinessFile { get; set; }
        public string TradeAndBusinessFile { get; set; }
        public string OldTradeAndBusinessFile { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase TradeAndBusinessFileType { get; set; }


        public string ActualImageFile { get; set; }
        public string ImageFile { get; set; }
        public string OldImageFile { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageType { get; set; }

     




        [StringLength(250, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 250.")]
        [DataType(DataType.Text)]
        [Display(Name = "Rejection Remarks")]
        public string Remark { get; set; }




        public string Password { get; set; }
        public Nullable<int> AdminApproval { get; set; }
        public Nullable<int> AdminApprovalPrev { get; set; }

        public Nullable<int> Status { get; set; }

    

        public Nullable<Int64> CreatedBy { get; set; }
        public Nullable<Int64> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }


    
        public int? TotalPages { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> DisciplineList { get; set; }
        public List<VendorSearchModel> SearchResultList { get; set; }
     
   
    }
}
