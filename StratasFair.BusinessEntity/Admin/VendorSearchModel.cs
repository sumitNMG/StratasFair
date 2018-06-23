using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Admin
{
    public class VendorSearchModel
    {
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public Nullable<int> DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public string OtherDisciplineName { get; set; }
        public string MobileNumber { get; set; }
        public string CompanyBrief { get; set; }
        public string TradeAndBusinessFile { get; set; }
        public string ActualTradeAndBusinessFile { get; set; }
        public string ImageFile { get; set; }
        public string ActualImageFile { get; set; }
        public string Remark { get; set; }
        public Nullable<int> AdminApproval { get; set; }
        public string AdminApprovalText { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedFromIp { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
