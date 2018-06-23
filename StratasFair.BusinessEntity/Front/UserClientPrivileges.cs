using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity.Front
{
    public class UserClientPrivilege 
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageLink { get; set; }
        public bool IsChecked { get; set; }
        public int ParentPageId { get; set; }
        public int PageLevel { get; set; }
        public string PageLinkIconName { get; set; }
    }
}
