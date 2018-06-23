using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace StratasFair.BusinessEntity
{
    public class PMBModel
    {
        public long PMBId { get; set; }
        public long QuotatoinId { get; set; }
        public long? VendorId { get; set; }
        public long? ClientUserId { get; set; }
        public string VendorName { get; set; }
        public string ClientUserName { get; set; }
        public string VendorImage { get; set; }
        public string ClientImage { get; set; }
        public long ? StrataBoardId { get; set; }
        public string StrataBoardName { get; set; }
        public string AttachedActualFileName { get; set; }
        public string AttachedFileName { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        [Required(ErrorMessage = "Message required")]
        [MinLength(2, ErrorMessage = "Min 2 characters")]
        [MaxLength(500, ErrorMessage = "Max 500 characters")]
        public string Message { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public string CreatedFromIP { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public string ModifiedFromIP { get; set; }
        public string CreatedByUserType { get; set; }
    }

    public class AdminPMBModel
    {
        public long PMBId { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        public string AttachedFileName { get; set; }
        public string AttachedFileActualName { get; set; }
        public Nullable<long> OwnerId { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerProfilePicture { get; set; }
        public Nullable<long> AdminId { get; set; }
        public string AdminFullName { get; set; }
        public string AdminProfilePicture { get; set; }
        public Nullable<long> StratasBoardId { get; set; }
        public string StratasBoardName { get; set; }
        public string CreatedByUserType { get; set; }
        public string CreatedByUserName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<long> CreatedFor { get; set; }
        public string CreatedFromIP { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public string ModifiedFromIP { get; set; }
        [Required(ErrorMessage = "Message required")]
        [MinLength(2, ErrorMessage = "Min 2 characters")]
        [MaxLength(500, ErrorMessage = "Max 500 characters")]
        public string Message { get; set; }
    }
}
