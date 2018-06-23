using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Drawing;


namespace StratasFair.BusinessEntity
{
    public class CommonModel
    {
        public CommonModel()
        {
            try
            {
                this.Flag = 0;
                this.RoleId = 0;                
                this.CreatedFromIp = HttpContext.Current.Request.UserHostAddress;                
                this.IsShowMessage = 0;
                this.IsShowPrivillage = 0;
                this.PageSize = 0;
                this.PageNumber = 0;
                this.Choice = 0;
                this.ParentId = 0;
                this.SerialNumber = 0;                
            }
            catch
            {

            }
        }
        public int Flag { get; set; }
        public int RoleId { get; set; }
        public Nullable<byte> Status { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedFromIp { get; set; }
        public string Message { get; set; }
        public int IsShowMessage { get; set; }
        public int IsShowPrivillage { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int? TotalCount { get; set; }
        public int SortBy { get; set; }
        public string SortDirection { get; set; }

        public int Choice { get; set; }
        public int ParentId { get; set; }
        public string MenuHTML { get; set; }
        public string ModuleIds { get; set; }
        public int SerialNumber { get; set; }
        public string MessageClass { get; set; }
        public string SearchKeywords { get; set; }
    }
}
