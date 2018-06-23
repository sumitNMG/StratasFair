using StratasFair.Business;
using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Routing;

namespace StratasFair.Web.App_Start
{
    public class AdminSessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // check  sessions here
            if (filterContext.ActionDescriptor.ActionName.ToLower() != "fileuploads")
            {
                if (AdminSessionData.AdminUserId == 0)
                {
                    filterContext.Result = new RedirectResult("~/securehost/rootlogin/");
                    return;
                }

                string controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

                string action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                if (controller.ToLower() == "report")
                {
                    controller = controller + "/" + action;
                }
                if (!CheckModule(1, controller))
                {
                    filterContext.Result = new RedirectResult("~/securehost/rootlogin/dashboard/noaccess");
                }
            }

            base.OnActionExecuting(filterContext);
        }


        private static bool CheckModule(int flag, string navigateUrl)
        {

            //.Replace("ssicontent", "client") : for giving content access to client
            navigateUrl = navigateUrl.TrimEnd('/').TrimStart('/').ToLower();
            bool isValid = false;
            var sqlCon = new SqlConnection(SqlHelper.GetConnectionString());
            sqlCon.Open();

            var sqlCmd = new SqlCommand();
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandText = "usp_GetModulesByUserId";
            sqlCmd.Connection = sqlCon;

            sqlCmd.Parameters.AddWithValue("@Flag", flag);
            sqlCmd.Parameters.AddWithValue("@AdminUserId", AdminSessionData.AdminUserId);
            sqlCmd.Parameters.AddWithValue("@NavigateUrl", "/" + navigateUrl + "/");

            using (SqlDataReader reader = sqlCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    isValid = true;
                }
            }

            sqlCon.Close();
            if (navigateUrl.ToLower() == "Dashboard".ToLower()
                || navigateUrl.ToLower() == "ChangePassword".ToLower()
                || navigateUrl.ToLower() == "Error".ToLower()
                || navigateUrl.ToLower() == "profile"
                || navigateUrl.ToLower() == "report/contactusexporttocsv"
                  || navigateUrl.ToLower() == "report/upcomingrenewalexporttocsv"
                  || navigateUrl.ToLower() == "report/expiredmembershipexporttocsv"
                )
            {
                isValid = true;
            }


            return isValid;
        }



    }


    public class StrataBoardOwnerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            string PortalUrl = string.Empty;
            var urlBuilder = new System.UriBuilder(HttpContext.Current.Request.Url.AbsoluteUri);
            if (urlBuilder.Uri.Authority.Contains("demo2projects"))
            {
                PortalUrl = urlBuilder.Uri.AbsolutePath.Replace("stratasfair/", "").ToString().Split('/')[0];
            }
            else
            {
                PortalUrl = urlBuilder.Uri.AbsolutePath.TrimStart('/').ToString().Split('/')[0];
            }


            if (ClientSessionData.ClientRoleName != "Owner")
            {
                filterContext.Result = new RedirectResult("~/" + ClientSessionData.ClientPortalLink + "/dashboard");
                return;
            }

            
            base.OnActionExecuting(filterContext);
        }
    }



    public class StrataBoardAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            string PortalUrl = string.Empty;
            var urlBuilder = new System.UriBuilder(HttpContext.Current.Request.Url.AbsoluteUri);
            if (urlBuilder.Uri.Authority.Contains("demo2projects"))
            {
                PortalUrl = urlBuilder.Uri.AbsolutePath.Replace("stratasfair/", "").ToString().Split('/')[0];
            }
            else
            {
                PortalUrl = urlBuilder.Uri.AbsolutePath.TrimStart('/').ToString().Split('/')[0];
            }


            if (ClientSessionData.ClientRoleName == "Owner")
            {
                filterContext.Result = new RedirectResult("~/" + ClientSessionData.ClientPortalLink + "/dashboard");
                return;
            }
            else if (ClientSessionData.ClientRoleName == "SubAdmin")
            {
                string controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
                if (!CheckModule(1, controller))
                {
                    filterContext.Result = new RedirectResult("~/" + ClientSessionData.ClientPortalLink + "/dashboard");
                }
            }
            base.OnActionExecuting(filterContext);
        }

        private static bool CheckModule(int flag, string navigateUrl)
        {
            navigateUrl = navigateUrl.TrimEnd('/').TrimStart('/').ToLower();
            bool isValid = false;

            using (var db = new StratasFair.Data.StratasFairDBEntities())
            {
                var moduleList = db.Usp_GetModulesByUserClientId(ClientSessionData.UserClientId).ToList();
                for (int i = 0; i < moduleList.Count; i++)
                {
                    if (moduleList[i].PageLink.Contains(navigateUrl + "/"))
                    {
                        isValid = true;
                        break;
                    }
                }

            }

            //if (navigateUrl.ToLower() == "Dashboard".ToLower())
            //{
            //    isValid = true;
            //}


            return isValid;
        }
    }





    public class ClientSessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // check  sessions here
            if (filterContext.ActionDescriptor.ActionName.ToLower() != "fileuploads")
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
                var urlBuilder = new System.UriBuilder(HttpContext.Current.Request.Url.AbsoluteUri);
                Uri uri = urlBuilder.Uri;

                string Url = urlBuilder.Path;
                string PortalUrl = string.Empty;
                if (urlBuilder.Uri.Authority.Contains("demo2projects"))
                {
                    PortalUrl = urlBuilder.Uri.AbsolutePath.Replace("/stratasfair/", "").ToString();
                }
                else
                {
                    PortalUrl = urlBuilder.Uri.AbsolutePath.TrimStart('/').ToString();
                }
                int endIndex = PortalUrl.IndexOf('/');
                PortalUrl = PortalUrl.Substring(0, endIndex);
                bool IsValid = strataBoardHelper.IsValidPortalLink(PortalUrl);
                if (IsValid)
                {
                    if (ClientSessionData.UserClientId == 0)
                    {
                        filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["WebsiteRootPath"].ToString() + PortalUrl + "/login");
                    }
                    else if (ClientSessionData.UserClientId > 0)
                    {
                        if (ClientSessionData.ClientPortalLink != PortalUrl)
                        {
                            filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["WebsiteRootPath"].ToString() + PortalUrl + "/logon/notbelonguser");
                        }
                        else if (ClientSessionData.ClientRoleName.ToLower() == "owner" && !ClientSessionData.ClientIsProfileCompleted)
                        {
                            filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["WebsiteRootPath"].ToString() + PortalUrl + "/logon/completeprofile");
                        }
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["WebsiteRootPath"].ToString() + PortalUrl + "/logon/invalidportallink");
                    // redirect to page not found
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }




    public class VendorSessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // check  sessions here
            if (filterContext.ActionDescriptor.ActionName.ToLower() != "fileuploads")
            {
                if (VendorSessionData.Instance.VendorId <= 0)
                {
                    filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["WebsiteRootPath"].ToString() + "vendor/login");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}