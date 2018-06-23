using StratasFair.Business.CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Front
{
    public class VendorSessionData
    {
        private VendorSessionData() { }
        private static readonly Lazy<VendorSessionData> lazy = new Lazy<VendorSessionData>(() => new VendorSessionData());
        public static VendorSessionData Instance
        {
            get
            {
                return lazy.Value;
            }
        }


        /* For the VendorId */
        const string VendorIdKey = "VendorId";
        public long VendorId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["VendorId"]); }
        }

        /* For the Vendor email id */
        const string VendorEmailIdKey = "VendorEmailId";
        public string VendorEmailId
        {
            get { return Convert.ToString(HttpContext.Current.Session["VendorEmailId"]); }
        }

        /* For the Vendor name */
        const string VendorNameKey = "VendorName";
        public string VendorName
        {
            get { return Convert.ToString(HttpContext.Current.Session["VendorName"]); }
        }

        /* For the Vendor name */
        const string VendorMobileKey = "VendorMobile";
        public string VendorMobile
        {
            get { return Convert.ToString(HttpContext.Current.Session["VendorMobile"]); }
        }

        /* For the Vendor ProfilePicture */
        const string VendorProfilePictureKey = "VendorProfilePicture";
        public string VendorProfilePicture
        {
            get { return Convert.ToString(HttpContext.Current.Session["VendorProfilePicture"]); }
        }

        /* For the Vendor created on date */
        const string VendorCreatedOnKey = "VendorCreatedOn";
        public string VendorCreatedOn
        {
            get { return Convert.ToString(HttpContext.Current.Session["VendorCreatedOn"]); }
        }
    }
}
