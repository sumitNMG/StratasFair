using StratasFair.Business;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StratasFair.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //var uniquenameList = new List<string>();
            //using (var context = new StratasFairDBEntities())
            //{
            //    uniquenameList = context.vw_GetStratasBoard.Where(x => x.STATUS == 1).Select(x => x.PortalLink).ToList();
            //}
            //if (uniquenameList != null && uniquenameList.Count > 0)
            //{
            //    for (int i = 0; i < uniquenameList.Count; i++)
            //    {
            //        routes.MapRoute(
            //           name: "Login" + i.ToString(),
            //           url: uniquenameList[i].ToString().ToLower() + "/login",
            //           defaults: new { controller = "logon", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //       );

            //        routes.MapRoute(
            //          name: "CompleteProfile" + i.ToString(),
            //          url: uniquenameList[i].ToString().ToLower() + "/logon/completeprofile",
            //          defaults: new { controller = "logon", action = "CompleteProfile" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //      );

            //        routes.MapRoute(
            //          name: "ForgotPassword" + i.ToString(),
            //          url: uniquenameList[i].ToString().ToLower() + "/logon/forgotpassword",
            //          defaults: new { controller = "logon", action = "forgotpassword" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //      );

            //        routes.MapRoute(
            //          name: "ResetPassword" + i.ToString(),
            //          url: uniquenameList[i].ToString().ToLower() + "/logon/resetpassword/{EncryptUserClientId}",
            //          defaults: new { controller = "logon", action = "resetpassword", EncryptUserClientId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //      );

            //        routes.MapRoute(
            //         name: "InvalidPortalLink" + i.ToString(),
            //         url: uniquenameList[i].ToString().ToLower() + "/logon/invalidportallink",
            //         defaults: new { controller = "logon", action = "invalidportallink" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //     );
            //        routes.MapRoute(
            //         name: "NotBelongUser" + i.ToString(),
            //         url: uniquenameList[i].ToString().ToLower() + "/logon/notbelonguser",
            //         defaults: new { controller = "logon", action = "notbelonguser" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //     );

            //        routes.MapRoute(
            //           name: "DashBoard" + i.ToString(),
            //           url: uniquenameList[i].ToString().ToLower() + "/dashboard",
            //           defaults: new { controller = "dashBoard", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //       );

            //        routes.MapRoute(
            //          name: "DashBoardSignOut" + i.ToString(),
            //          url: uniquenameList[i].ToString().ToLower() + "/dashboard/signout",
            //          defaults: new { controller = "dashBoard", action = "signout" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //      );

            //        routes.MapRoute(
            //          name: "SettingsMyProfile" + i.ToString(),
            //          url: uniquenameList[i].ToString().ToLower() + "/settings/myprofile",
            //          defaults: new { controller = "settings", action = "myprofile" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //      );
            //        routes.MapRoute(
            //          name: "SettingsOwnerMyProfile" + i.ToString(),
            //          url: uniquenameList[i].ToString().ToLower() + "/settings/ownermyprofile",
            //          defaults: new { controller = "settings", action = "ownermyprofile" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //      );

            //        routes.MapRoute(
            //         name: "SettingsUniqueURLRequest" + i.ToString(),
            //         url: uniquenameList[i].ToString().ToLower() + "/settings/uniqueurlrequest",
            //         defaults: new { controller = "settings", action = "uniqueurlrequest" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //     );

            //        routes.MapRoute(
            //        name: "SettingsResetPassword" + i.ToString(),
            //        url: uniquenameList[i].ToString().ToLower() + "/settings/resetpassword",
            //        defaults: new { controller = "settings", action = "resetpassword" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //    );

            //        routes.MapRoute(
            //       name: "SettingsEnableDisableAdminNotification" + i.ToString(),
            //       url: uniquenameList[i].ToString().ToLower() + "/settings/enabledisableadminnotification",
            //       defaults: new { controller = "settings", action = "enabledisableadminnotification" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //   );

            //        routes.MapRoute(
            //     name: "SettingsEnableDisableOwnerNotification" + i.ToString(),
            //     url: uniquenameList[i].ToString().ToLower() + "/settings/enabledisableownernotification",
            //     defaults: new { controller = "settings", action = "enabledisableownernotification" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            // );
            //        routes.MapRoute(
            //     name: "SettingsUpdateAdminNotificationSetting" + i.ToString(),
            //     url: uniquenameList[i].ToString().ToLower() + "/settings/updateadminnotificationsetting",
            //     defaults: new { controller = "settings", action = "updateadminnotificationsetting" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //    );
            //        routes.MapRoute(
            //    name: "SettingsUpdateOwnerNotificationSetting" + i.ToString(),
            //    url: uniquenameList[i].ToString().ToLower() + "/settings/updateownernotificationsetting",
            //    defaults: new { controller = "settings", action = "updateownernotificationsetting" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //   );

            //        routes.MapRoute(
            //    name: "SettingsManageSubAdmin" + i.ToString(),
            //    url: uniquenameList[i].ToString().ToLower() + "/managesubadmin",
            //    defaults: new { controller = "managesubadmin", action = "index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //   );

            //        routes.MapRoute(
            //   name: "LogonTestPass" + i.ToString(),
            //   url: uniquenameList[i].ToString().ToLower() + "/logon/testpass/{id}",
            //   defaults: new { controller = "logon", action = "testpass", id = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //  );


            //        routes.MapRoute(
            //         name: "TestPage" + i.ToString(),
            //         url: uniquenameList[i].ToString().ToLower() + "/logon/testpage",
            //         defaults: new { controller = "logon", action = "testpage", id = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
            //        );

            //    }
            //}


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "StratasFair.Web.Controllers" },
                constraints: new { controller = @"(Home|Vendor)" }
              );
        }
    }




    public class DynamicRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            if (HttpContext.Current.Request.Url.Segments.Length > 1)
            {
                var portalname = "";
                if (HttpContext.Current.Request.Url.Segments[1].Replace("/", "").ToLower() != "stratasfair")
                {
                    portalname = HttpContext.Current.Request.Url.Segments[1].Replace("/", "");
                }
                else
                {
                    portalname = HttpContext.Current.Request.Url.Segments[2].Replace("/", "");
                }


                var uniquenameList = "";
                using (var context = new StratasFairDBEntities())
                {
                    uniquenameList = context.vw_GetStratasBoard.Where(x => x.STATUS == 1 && x.PortalLink == portalname).Select(x => x.PortalLink).FirstOrDefault();
                }
                if (uniquenameList != null)
                {
                    if (uniquenameList.Length > 0)
                    {
                        routes.MapRoute(
                       name: "Login",
                       url: portalname.ToString().ToLower() + "/login",
                       defaults: new { controller = "logon", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                   );

                        routes.MapRoute(
                          name: "CompleteProfile",
                          url: portalname.ToString().ToLower() + "/logon/completeprofile",
                          defaults: new { controller = "logon", action = "CompleteProfile" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                      );

                        routes.MapRoute(
                          name: "ForgotPassword",
                          url: portalname.ToString().ToLower() + "/logon/forgotpassword",
                          defaults: new { controller = "logon", action = "forgotpassword" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                      );

                        routes.MapRoute(
                          name: "ResetPassword",
                          url: portalname.ToString().ToLower() + "/logon/resetpassword/{EncryptUserClientId}",
                          defaults: new { controller = "logon", action = "resetpassword", EncryptUserClientId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                      );

                        routes.MapRoute(
                         name: "InvalidPortalLink",
                         url: portalname.ToString().ToLower() + "/logon/invalidportallink",
                         defaults: new { controller = "logon", action = "invalidportallink" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                     );
                        routes.MapRoute(
                         name: "NotBelongUser",
                         url: portalname.ToString().ToLower() + "/logon/notbelonguser",
                         defaults: new { controller = "logon", action = "notbelonguser" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                     );

                        routes.MapRoute(
                           name: "DashBoard",
                           url: portalname.ToString().ToLower() + "/dashboard",
                           defaults: new { controller = "dashBoard", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                          name: "DashBoardSignOut",
                          url: portalname.ToString().ToLower() + "/dashboard/signout",
                          defaults: new { controller = "dashBoard", action = "signout" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                      );

                        routes.MapRoute(
                           name: "OwnerDashboard",
                           url: portalname.ToString().ToLower() + "/dashboard/ownerdashboard",
                           defaults: new { controller = "dashBoard", action = "ownerdashboard" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                           name: "DashboardViewLatestAlert",
                           url: portalname.ToString().ToLower() + "/dashboard/ViewLatestAlert",
                           defaults: new { controller = "dashBoard", action = "ViewLatestAlert" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                           name: "DashboardViewMaintenanceSchedule",
                           url: portalname.ToString().ToLower() + "/dashboard/ViewMaintenanceSchedule",
                           defaults: new { controller = "dashBoard", action = "ViewMaintenanceSchedule" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                           name: "DashboardViewScheduleMeeting",
                           url: portalname.ToString().ToLower() + "/dashboard/ViewScheduleMeeting",
                           defaults: new { controller = "dashBoard", action = "ViewScheduleMeeting" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                           name: "DashboardListLatestAlert",
                           url: portalname.ToString().ToLower() + "/dashboard/ListLatestAlert",
                           defaults: new { controller = "dashBoard", action = "ListLatestAlert" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                           name: "DashboardListMaintenanceSchedule",
                           url: portalname.ToString().ToLower() + "/dashboard/ListMaintenanceSchedule",
                           defaults: new { controller = "dashBoard", action = "ListMaintenanceSchedule" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                           name: "DashboardListScheduleMeeting",
                           url: portalname.ToString().ToLower() + "/dashboard/ListScheduleMeeting",
                           defaults: new { controller = "dashBoard", action = "ListScheduleMeeting" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                           name: "DashboardGetListOwnerBookingRequest",
                           url: portalname.ToString().ToLower() + "/dashboard/GetListOwnerBookingRequest",
                           defaults: new { controller = "dashBoard", action = "GetListOwnerBookingRequest" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );


                        routes.MapRoute(
                          name: "SettingsMyProfile",
                          url: portalname.ToString().ToLower() + "/settings/myprofile",
                          defaults: new { controller = "settings", action = "myprofile" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                      );
                        routes.MapRoute(
                          name: "SettingsOwnerMyProfile",
                          url: portalname.ToString().ToLower() + "/settings/ownermyprofile",
                          defaults: new { controller = "settings", action = "ownermyprofile" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                      );

                        //Start of route added by sanjeev//
                        routes.MapRoute(
                        name: "VendorQuotation",
                        url: portalname.ToString().ToLower() + "/vendor/quotation",
                        defaults: new { controller = "vendor", action = "quotation" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                        name: "VendorPMB",
                        url: "vendor/pmb/{id}",
                        defaults: new { controller = "vendor", action = "pmb", id = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                    );
                        routes.MapRoute(
                         name: "ClientPMB",
                        url: portalname.ToString().ToLower() + "/pmb/{id}",
                        defaults: new { controller = "vendor", action = "ownerpmb", id = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                      );

                        routes.MapRoute(
                        name: "VendorQuotationVendorList",
                        url: portalname.ToString().ToLower() + "/Vendor/GetVendorsList",
                        defaults: new { controller = "vendor", action = "getvendorslist", id = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                    );

                        routes.MapRoute(
                        name: "VendorQuotationDownloadFile",
                        url: portalname.ToString().ToLower() + "/Vendor/download",
                        defaults: new { controller = "vendor", action = "download", quoteid = UrlParameter.Optional, fileName = UrlParameter.Optional, actualFileName = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                    );

                        routes.MapRoute(
                       name: "VendorDirectory",
                       url: portalname.ToString().ToLower() + "/vendor/directory",
                       defaults: new { controller = "vendor", action = "directory" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                       name: "VendorDirectorySearch",
                       url: portalname.ToString().ToLower() + "/vendor/directory",
                       defaults: new { controller = "vendor", action = "directory" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                       name: "VendorDirectoryScrollLoad",
                       url: portalname.ToString().ToLower() + "/vendor/search",
                       defaults: new { controller = "vendor", action = "search", keyword = UrlParameter.Optional, disciplineid = UrlParameter.Optional, page = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );
                        //End of route added by sanjeev//

                        routes.MapRoute(
                         name: "SettingsUniqueURLRequest",
                         url: portalname.ToString().ToLower() + "/settings/uniqueurlrequest",
                         defaults: new { controller = "settings", action = "uniqueurlrequest" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                     );

                        routes.MapRoute(
                        name: "SettingsResetPassword",
                        url: portalname.ToString().ToLower() + "/settings/resetpassword",
                        defaults: new { controller = "settings", action = "resetpassword" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                    );

                        routes.MapRoute(
                       name: "SettingsEnableDisableAdminNotification",
                       url: portalname.ToString().ToLower() + "/settings/enabledisableadminnotification",
                       defaults: new { controller = "settings", action = "enabledisableadminnotification" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                   );

                        routes.MapRoute(
                     name: "SettingsEnableDisableOwnerNotification",
                     url: portalname.ToString().ToLower() + "/settings/enabledisableownernotification",
                     defaults: new { controller = "settings", action = "enabledisableownernotification" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );
                        routes.MapRoute(
                     name: "SettingsUpdateAdminNotificationSetting",
                     url: portalname.ToString().ToLower() + "/settings/updateadminnotificationsetting",
                     defaults: new { controller = "settings", action = "updateadminnotificationsetting" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                    );
                        routes.MapRoute(
                    name: "SettingsUpdateOwnerNotificationSetting",
                    url: portalname.ToString().ToLower() + "/settings/updateownernotificationsetting",
                    defaults: new { controller = "settings", action = "updateownernotificationsetting" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                   );

                        routes.MapRoute(
                   name: "EmailSettingClient",
                   url: portalname.ToString().ToLower() + "/EmailSettingClient",
                   defaults: new { controller = "EmailSettingClient", action = "index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                  name: "EmailSettingClientCheckemailsettings",
                  url: portalname.ToString().ToLower() + "/EmailSettingClient/checkemailsettings",
                  defaults: new { controller = "EmailSettingClient", action = "checkemailsettings" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                    name: "ManageSubAdmin",
                    url: portalname.ToString().ToLower() + "/managesubadmin",
                    defaults: new { controller = "managesubadmin", action = "index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                   );

                        routes.MapRoute(
                    name: "ViewUserPrivileges",
                    url: portalname.ToString().ToLower() + "/managesubadmin/ViewUserPrivileges/{SubAdminId}",
                    defaults: new { controller = "managesubadmin", action = "ViewUserPrivileges", SubAdminId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                   );

                        routes.MapRoute(
                 name: "ListSubAdmin",
                 url: portalname.ToString().ToLower() + "/managesubadmin/ListSubAdmin",
                 defaults: new { controller = "managesubadmin", action = "ListSubAdmin" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );
                        routes.MapRoute(
                 name: "SubAdminInfinateScroll",
                 url: portalname.ToString().ToLower() + "/managesubadmin/InfinateScroll",
                 defaults: new { controller = "managesubadmin", action = "InfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );

                        routes.MapRoute(
                  name: "AddSubAdmin",
                  url: portalname.ToString().ToLower() + "/managesubadmin/AddSubAdmin",
                  defaults: new { controller = "managesubadmin", action = "AddSubAdmin" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                   name: "EditSubAdmin",
                   url: portalname.ToString().ToLower() + "/managesubadmin/EditSubAdmin/{SubAdminId}",
                   defaults: new { controller = "managesubadmin", action = "EditSubAdmin", SubAdminId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                   name: "DeleteSubAdmin",
                   url: portalname.ToString().ToLower() + "/managesubadmin/DeleteSubAdmin/{SubAdminId}",
                   defaults: new { controller = "managesubadmin", action = "DeleteSubAdmin", SubAdminId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                  name: "ActivateDeActivateSubAdmin",
                  url: portalname.ToString().ToLower() + "/managesubadmin/ActivateDeActivateSubAdmin/{SubAdminId}/{IsActive}",
                  defaults: new { controller = "managesubadmin", action = "ActivateDeActivateSubAdmin", SubAdminId = UrlParameter.Optional, IsActive = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                 name: "ResendCredentialsMail",
                 url: portalname.ToString().ToLower() + "/managesubadmin/ResendCredentialsMail/{SubAdminId}",
                 defaults: new { controller = "managesubadmin", action = "ResendCredentialsMail", SubAdminId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );

                        routes.MapRoute(
                    name: "ManageStrataOwner",
                    url: portalname.ToString().ToLower() + "/ManageStrataOwner",
                    defaults: new { controller = "ManageStrataOwner", action = "index", BlockNumber = UrlParameter.Optional, ByFirstName = UrlParameter.Optional, ByLastName = UrlParameter.Optional, ByEmail = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                   );
                        routes.MapRoute(
                  name: "AddStrataOwner",
                  url: portalname.ToString().ToLower() + "/ManageStrataOwner/AddStrataOwner",
                  defaults: new { controller = "ManageStrataOwner", action = "AddStrataOwner" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                 name: "ListStrataOwner",
                 url: portalname.ToString().ToLower() + "/ManageStrataOwner/ListStrataOwner",
                 defaults: new { controller = "ManageStrataOwner", action = "ListStrataOwner" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                 name: "StrataOwnerInfinateScroll",
                 url: portalname.ToString().ToLower() + "/ManageStrataOwner/InfinateScroll",
                 defaults: new { controller = "ManageStrataOwner", action = "InfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );

                        routes.MapRoute(
                   name: "EditStrataOwner",
                   url: portalname.ToString().ToLower() + "/ManageStrataOwner/EditStrataOwner/{SubAdminId}",
                   defaults: new { controller = "ManageStrataOwner", action = "EditStrataOwner", SubAdminId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );
                        routes.MapRoute(
                  name: "ViewStrataOwner",
                  url: portalname.ToString().ToLower() + "/ManageStrataOwner/ViewStrataOwner/{SubAdminId}",
                  defaults: new { controller = "ManageStrataOwner", action = "ViewStrataOwner", SubAdminId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                   name: "DeleteStrataOwner",
                   url: portalname.ToString().ToLower() + "/ManageStrataOwner/DeleteStrataOwner/{SubAdminId}",
                   defaults: new { controller = "ManageStrataOwner", action = "DeleteStrataOwner", SubAdminId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                  name: "ActivateDeActivateStrataOwner",
                  url: portalname.ToString().ToLower() + "/ManageStrataOwner/ActivateDeActivateStrataOwner/{SubAdminId}/{IsActive}",
                  defaults: new { controller = "ManageStrataOwner", action = "ActivateDeActivateStrataOwner", SubAdminId = UrlParameter.Optional, IsActive = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                 name: "ResendOwnerCredentialsMail",
                 url: portalname.ToString().ToLower() + "/ManageStrataOwner/ResendCredentialsMail/{SubAdminId}",
                 defaults: new { controller = "ManageStrataOwner", action = "ResendCredentialsMail", SubAdminId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );

                        routes.MapRoute(
                name: "BulkUploadStrataOwner",
                url: portalname.ToString().ToLower() + "/ManageStrataOwner/BulkUploadStrataOwner",
                defaults: new { controller = "ManageStrataOwner", action = "BulkUploadStrataOwner" }, namespaces: new[] { "StratasFair.Web.Controllers" }
               );

                        routes.MapRoute(
        name: "BulkUploadStrataOwnerXLSXANDCSV",
        url: portalname.ToString().ToLower() + "/ManageStrataOwner/downloadimportuserfile",
        defaults: new { controller = "ManageStrataOwner", action = "DownloadImportUserFile" }, namespaces: new[] { "StratasFair.Web.Controllers" }
       );



                        routes.MapRoute(
               name: "CancelBulkUploadStrataOwner",
               url: portalname.ToString().ToLower() + "/ManageStrataOwner/CancelBulkUploadStrataOwner",
               defaults: new { controller = "ManageStrataOwner", action = "CancelBulkUploadStrataOwner" }, namespaces: new[] { "StratasFair.Web.Controllers" }
              );

                        routes.MapRoute(
                   name: "LogonTestPass",
                   url: portalname.ToString().ToLower() + "/logon/testpass/{id}",
                   defaults: new { controller = "logon", action = "testpass", id = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                   name: "OwnerMyRequest",
                   url: portalname.ToString().ToLower() + "/OwnerMyRequest",
                   defaults: new { controller = "OwnerMyRequest", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                   name: "ListOwnerMyRequest",
                   url: portalname.ToString().ToLower() + "/OwnerMyRequest/ListOwnerMyRequest",
                   defaults: new { controller = "OwnerMyRequest", action = "ListOwnerMyRequest" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                   name: "AddOwnerMyRequest",
                   url: portalname.ToString().ToLower() + "/OwnerMyRequest/AddOwnerMyRequest",
                   defaults: new { controller = "OwnerMyRequest", action = "AddOwnerMyRequest" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                   );

                        routes.MapRoute(
                   name: "DeleteOwnerMyRequest",
                   url: portalname.ToString().ToLower() + "/OwnerMyRequest/DeleteOwnerMyRequest/{RequestId}",
                   defaults: new { controller = "OwnerMyRequest", action = "DeleteOwnerMyRequest", RequestId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                name: "OwnerMyRequestInfinateScroll",
                url: portalname.ToString().ToLower() + "/OwnerMyRequest/InfinateScroll",
                defaults: new { controller = "OwnerMyRequest", action = "InfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
               );

                        routes.MapRoute(
                  name: "OwnerRequestBooking",
                  url: portalname.ToString().ToLower() + "/OwnerRequestBooking",
                  defaults: new { controller = "OwnerRequestBooking", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                  name: "ListOwnerBookingRequest",
                  url: portalname.ToString().ToLower() + "/OwnerRequestBooking/GetListOwnerBookingRequest",
                  defaults: new { controller = "OwnerRequestBooking", action = "GetListOwnerBookingRequest" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                  name: "AddOwnerBookingRequest",
                  url: portalname.ToString().ToLower() + "/OwnerRequestBooking/AddOwnerBookingRequest",
                  defaults: new { controller = "OwnerRequestBooking", action = "AddOwnerBookingRequest" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        /* Added for the My Calendar Page for Owner */
                        routes.MapRoute(
                        name: "MyCalendar",
                        url: portalname.ToString().ToLower() + "/mycalendar",
                        defaults: new { controller = "mycalendar", action = "index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                        name: "OwnerGeneralCalendarList",
                        url: portalname.ToString().ToLower() + "/mycalendar/getownergeneralcalendarlist",
                        defaults: new { controller = "mycalendar", action = "getownergeneralcalendarlist" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                        name: "AddCommonAreaBooking",
                        url: portalname.ToString().ToLower() + "/mycalendar/addcommonareabooking",
                        defaults: new { controller = "mycalendar", action = "addcommonareabooking" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );


                        /* Added for the My Calendar Page for Owner */

                        routes.MapRoute(
                          name: "strataBoardAdminPoll",
                          url: portalname.ToString().ToLower() + "/polls",
                          defaults: new { controller = "polls", action = "index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                   name: "addPollOptionsField",
                   url: portalname.ToString().ToLower() + "/polls/addnewoptionfield",
                   defaults: new { controller = "polls", action = "addnewoptionfield", optionid = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );

                        routes.MapRoute(
                  name: "deletePoll",
                  url: portalname.ToString().ToLower() + "/polls/delete",
                  defaults: new { controller = "polls", action = "delete", pollid = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                           name: "showPollChart",
                           url: portalname.ToString().ToLower() + "/polls/showchart",
                           defaults: new { controller = "polls", action = "showchart", pollid = UrlParameter.Optional, caption = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                  name: "ownerPoll",
                  url: portalname.ToString().ToLower() + "/ownerpoll",
                  defaults: new { controller = "ownerpoll", action = "index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );
                        routes.MapRoute(
                          name: "showPollChartOwner",
                          url: portalname.ToString().ToLower() + "/ownerpoll/showchart",
                          defaults: new { controller = "ownerpoll", action = "showchart", pollid = UrlParameter.Optional, caption = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );


                        routes.MapRoute(
                          name: "ownerSubmitOpinion",
                          url: portalname.ToString().ToLower() + "/ownerpoll/submitopinion",
                          defaults: new { controller = "ownerpoll", action = "submitopinion", optionId = UrlParameter.Optional, pollId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                          );

                        routes.MapRoute(
                         name: "ownerPollList",
                         url: portalname.ToString().ToLower() + "/ownerpoll/GetPollList",
                         defaults: new { controller = "ownerpoll", action = "GetPollList", pageNo = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                         );

                        routes.MapRoute(
                          name: "adminPollList",
                          url: portalname.ToString().ToLower() + "/polls/GetPollList",
                          defaults: new { controller = "polls", action = "GetPollList", pageNo = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                          );

                        routes.MapRoute(
                          name: "adminOwnerPMB",
                          url: portalname.ToString().ToLower() + "/adminpmb",
                          defaults: new { controller = "AdminPMB", action = "Index", id = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                          );

                        routes.MapRoute(
                           name: "adminOwnerList",
                           url: portalname.ToString().ToLower() + "/adminpmb/ownerlist",
                           defaults: new { controller = "AdminPMB", action = "OwnerList", pageNo = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                           );

                        routes.MapRoute(
                           name: "adminGetOwnerList",
                           url: portalname.ToString().ToLower() + "/adminpmb/getownerlist",
                           defaults: new { controller = "AdminPMB", action = "GetOwnerList", pageNo = UrlParameter.Optional, firstName = UrlParameter.Optional, lastName = UrlParameter.Optional, email = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                           );

                        routes.MapRoute(
                     name: "AdminPMBDownloadFile",
                     url: portalname.ToString().ToLower() + "/adminpmb/download",
                     defaults: new { controller = "adminpmb", action = "download" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );



                        routes.MapRoute(
                 name: "MyReminder",
                 url: portalname.ToString().ToLower() + "/MyReminder",
                 defaults: new { controller = "MyReminder", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );

                        routes.MapRoute(
                  name: "ListMyReminder",
                  url: portalname.ToString().ToLower() + "/MyReminder/ListMyReminder",
                  defaults: new { controller = "MyReminder", action = "ListMyReminder" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                  name: "AddMyReminder",
                  url: portalname.ToString().ToLower() + "/MyReminder/AddMyReminder",
                  defaults: new { controller = "MyReminder", action = "AddMyReminder" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                 name: "EditMyReminder",
                 url: portalname.ToString().ToLower() + "/MyReminder/EditMyReminder",
                 defaults: new { controller = "MyReminder", action = "EditMyReminder" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );
                        routes.MapRoute(
               name: "DeleteMyReminder",
               url: portalname.ToString().ToLower() + "/MyReminder/DeleteMyReminder",
               defaults: new { controller = "MyReminder", action = "DeleteMyReminder", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
               );

                        routes.MapRoute(
            name: "MyReminderInfinateScroll",
            url: portalname.ToString().ToLower() + "/MyReminder/InfinateScroll",
            defaults: new { controller = "MyReminder", action = "InfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
           );


                        routes.MapRoute(
                                         name: "CommonArea",
                                         url: portalname.ToString().ToLower() + "/CommonArea",
                                         defaults: new { controller = "CommonArea", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                                        );

                        routes.MapRoute(
                  name: "ListCommonArea",
                  url: portalname.ToString().ToLower() + "/CommonArea/ListCommonArea",
                  defaults: new { controller = "CommonArea", action = "ListCommonArea" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                  name: "AddCommonArea",
                  url: portalname.ToString().ToLower() + "/CommonArea/AddCommonArea",
                  defaults: new { controller = "CommonArea", action = "AddCommonArea" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                 name: "EditCommonArea",
                 url: portalname.ToString().ToLower() + "/CommonArea/EditCommonArea",
                 defaults: new { controller = "CommonArea", action = "EditCommonArea", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );
                        routes.MapRoute(
               name: "DeleteCommonArea",
               url: portalname.ToString().ToLower() + "/CommonArea/DeleteCommonArea",
               defaults: new { controller = "CommonArea", action = "DeleteCommonArea", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
               );

                        routes.MapRoute(
            name: "CommonAreaInfinateScroll",
            url: portalname.ToString().ToLower() + "/CommonArea/InfinateScroll",
            defaults: new { controller = "CommonArea", action = "InfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
           );



                        routes.MapRoute(
                            name: "BillType",
                            url: portalname.ToString().ToLower() + "/BillType",
                            defaults: new { controller = "BillType", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "ListBillType",
                            url: portalname.ToString().ToLower() + "/BillType/ListBillType",
                            defaults: new { controller = "BillType", action = "ListBillType" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "AddBillType",
                            url: portalname.ToString().ToLower() + "/BillType/AddBillType",
                            defaults: new { controller = "BillType", action = "AddBillType" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                 name: "EditBillType",
                 url: portalname.ToString().ToLower() + "/BillType/EditBillType",
                 defaults: new { controller = "BillType", action = "EditBillType", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );
                        routes.MapRoute(
               name: "DeleteBillType",
               url: portalname.ToString().ToLower() + "/BillType/DeleteBillType",
               defaults: new { controller = "BillType", action = "DeleteBillType", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
               );

                        routes.MapRoute(
            name: "BillTypeInfinateScroll",
            url: portalname.ToString().ToLower() + "/BillType/InfinateScroll",
            defaults: new { controller = "BillType", action = "InfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
           );


                        routes.MapRoute(
                               name: "NoticeBoard",
                               url: portalname.ToString().ToLower() + "/NoticeBoard",
                               defaults: new { controller = "NoticeBoard", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                                               );

                        routes.MapRoute(
                 name: "ListAlertAndNotification",
                 url: portalname.ToString().ToLower() + "/NoticeBoard/ListAlertAndNotification",
                 defaults: new { controller = "NoticeBoard", action = "ListAlertAndNotification" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );

                        routes.MapRoute(
                  name: "AddAlertAndNotification",
                  url: portalname.ToString().ToLower() + "/NoticeBoard/AddAlertAndNotification",
                  defaults: new { controller = "NoticeBoard", action = "AddAlertAndNotification" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );

                        routes.MapRoute(
                 name: "EditAlertAndNotification",
                 url: portalname.ToString().ToLower() + "/NoticeBoard/EditAlertAndNotification",
                 defaults: new { controller = "NoticeBoard", action = "EditAlertAndNotification", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                );

                        routes.MapRoute(
                            name: "ViewAlertAndNotification",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/ViewAlertAndNotification",
                            defaults: new { controller = "NoticeBoard", action = "ViewAlertAndNotification", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "ResendAlertAndNotification",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/ResendAlertAndNotification",
                            defaults: new { controller = "NoticeBoard", action = "ResendAlertAndNotification", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "DeleteAlertAndNotification",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/DeleteAlertAndNotification",
                            defaults: new { controller = "NoticeBoard", action = "DeleteAlertAndNotification", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "LatestAlertInfinateScroll",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/MeetingInfinateScroll",
                            defaults: new { controller = "NoticeBoard", action = "MeetingInfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "MeetingSchedule",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/MeetingSchedule",
                            defaults: new { controller = "NoticeBoard", action = "MeetingSchedule" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "ListScheduleMeeting",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/ListScheduleMeeting",
                            defaults: new { controller = "NoticeBoard", action = "ListScheduleMeeting" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "AddScheduleMeeting",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/AddScheduleMeeting",
                            defaults: new { controller = "NoticeBoard", action = "AddScheduleMeeting" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "EditScheduleMeeting",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/EditScheduleMeeting",
                            defaults: new { controller = "NoticeBoard", action = "EditScheduleMeeting", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "ViewScheduleMeeting",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/ViewScheduleMeeting",
                            defaults: new { controller = "NoticeBoard", action = "ViewScheduleMeeting", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "DeleteScheduleMeeting",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/DeleteScheduleMeeting",
                            defaults: new { controller = "NoticeBoard", action = "DeleteScheduleMeeting", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "ScheduleMeetingInfinateScroll",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/MeetingInfinateScroll",
                            defaults: new { controller = "NoticeBoard", action = "MeetingInfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );



                        /* Added for General information */

                    routes.MapRoute(
                        name: "ListNoticeBoardGeneralInformation",
                        url: portalname.ToString().ToLower() + "/NoticeBoard/ListGeneralInformation",
                        defaults: new { controller = "NoticeBoard", action = "ListGeneralInformation" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                    );

                        routes.MapRoute(
                            name: "NoticeBoardGeneralInformation",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/GeneralInformation",
                            defaults: new { controller = "NoticeBoard", action = "GeneralInformation" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "AddGeneralInformation",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/AddGeneralInformation",
                            defaults: new { controller = "NoticeBoard", action = "AddGeneralInformation" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "EditGeneralInformation",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/EditGeneralInformation",
                            defaults: new { controller = "NoticeBoard", action = "EditGeneralInformation", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "ViewGeneralInformation",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/ViewGeneralInformation",
                            defaults: new { controller = "NoticeBoard", action = "ViewGeneralInformation", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "DeleteGeneralInformation",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/DeleteGeneralInformation",
                            defaults: new { controller = "NoticeBoard", action = "DeleteGeneralInformation", optionId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "GeneralInformationInfinateScroll",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/InformationInfinateScroll",
                            defaults: new { controller = "NoticeBoard", action = "InformationInfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        /* END: Added for General information */


                        routes.MapRoute(
                            name: "MaintenanceSchedule",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/MaintenanceSchedule",
                            defaults: new { controller = "NoticeBoard", action = "MaintenanceSchedule" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "ListMaintenanceSchedule",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/ListMaintenanceSchedule",
                            defaults: new { controller = "NoticeBoard", action = "ListMaintenanceSchedule" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "AddMaintenanceSchedule",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/AddMaintenanceSchedule",
                            defaults: new { controller = "NoticeBoard", action = "AddMaintenanceSchedule" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "MaintenanceScheduleInfinateScroll",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/MaintenanceInfinateScroll",
                            defaults: new { controller = "NoticeBoard", action = "MaintenanceInfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "EditMaintenanceSchedule",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/EditMaintenanceSchedule",
                            defaults: new { controller = "NoticeBoard", action = "EditMaintenanceSchedule" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "DeleteMaintenanceSchedule",
                            url: portalname.ToString().ToLower() + "/NoticeBoard/DeleteMaintenanceSchedule",
                            defaults: new { controller = "NoticeBoard", action = "DeleteMaintenanceSchedule" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );


                        /* Added for Common Area Calendar for Admin */

                        routes.MapRoute(
                            name: "AdminCommonAreaCalendar",
                            url: portalname.ToString().ToLower() + "/calendar/admincommonarea",
                            defaults: new { controller = "calendar", action = "admincommonarea" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "AdminCommonAreaCalendarList",
                            url: portalname.ToString().ToLower() + "/calendar/getadmincalendarlist",
                            defaults: new { controller = "calendar", action = "getadmincalendarlist" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "AdminAddCommonAreaBooking",
                            url: portalname.ToString().ToLower() + "/calendar/addcommonareabooking",
                            defaults: new { controller = "calendar", action = "addcommonareabooking" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        /* END : Added for Common Area Calendar for Admin */




                        /* For Strata Other Request */
                        routes.MapRoute(
                            name: "MyOtherRequest",
                            url: portalname.ToString().ToLower() + "/MyOtherRequest",
                            defaults: new { controller = "MyOtherRequest", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "ListMyOtherRequest",
                            url: portalname.ToString().ToLower() + "/MyOtherRequest/ListMyOtherRequest",
                            defaults: new { controller = "MyOtherRequest", action = "ListMyOtherRequest" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "MyOtherRequestApprove",
                            url: portalname.ToString().ToLower() + "/MyOtherRequest/ApproveMyRequestStatus/{RequestId}",
                            defaults: new { controller = "MyOtherRequest", action = "ApproveMyRequestStatus", RequestId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "UpdateMyRequestStatus",
                            url: portalname.ToString().ToLower() + "/MyOtherRequest/UpdateMyRequestStatus/{RequestId}",
                            defaults: new { controller = "MyOtherRequest", action = "UpdateMyRequestStatus", RequestId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "MyOtherRequestReject",
                            url: portalname.ToString().ToLower() + "/MyOtherRequest/RejectMyRequestStatus/{RequestId}",
                            defaults: new { controller = "MyOtherRequest", action = "RejectMyRequestStatus", RequestId = UrlParameter.Optional }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "MyOtherRequestInfinateScroll",
                            url: portalname.ToString().ToLower() + "/MyOtherRequest/InfinateScroll",
                            defaults: new { controller = "MyOtherRequest", action = "InfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        /* End: For Strata Other Request */


                        /* For Strata Forum */
                        routes.MapRoute(
                            name: "strataForumListing",
                            url: portalname.ToString().ToLower() + "/myforum",
                            defaults: new { controller = "myforum", action = "index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "strataForumTopicAdd",
                            url: portalname.ToString().ToLower() + "/myforum/addtopic",
                            defaults: new { controller = "myforum", action = "addtopic" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                        name: "strataForumTopicAddReply",
                        url: portalname.ToString().ToLower() + "/myforum/addtopicreply",
                        defaults: new { controller = "myforum", action = "addtopicreply" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "strataForumLoadListing",
                            url: portalname.ToString().ToLower() + "/myforum/loadforumlist",
                            defaults: new { controller = "myforum", action = "loadforumlist" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "strataForumReplyLoadListing",
                            url: portalname.ToString().ToLower() + "/myforum/loadforumreplylist",
                            defaults: new { controller = "myforum", action = "loadforumreplylist" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "strataForumDownload",
                            url: portalname.ToString().ToLower() + "/myforum/download",
                            defaults: new { controller = "myforum", action = "download" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "strataForumTopicAddFlag",
                            url: portalname.ToString().ToLower() + "/myforum/addflagontopic",
                            defaults: new { controller = "myforum", action = "addflagontopic" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "strataForumTopicFlagDelete",
                            url: portalname.ToString().ToLower() + "/myforum/deleteflagontopic",
                            defaults: new { controller = "myforum", action = "deleteflagontopic" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                           name: "strataForumLoadFlaggedMessageListing",
                           url: portalname.ToString().ToLower() + "/myforum/loadflaggedmessage",
                           defaults: new { controller = "myforum", action = "loadflaggedmessage" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                       );

                        routes.MapRoute(
                         name: "strataForumReadMessage",
                         url: portalname.ToString().ToLower() + "/myforum/readmessage",
                         defaults: new { controller = "myforum", action = "readmessage" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                     );

                        /* END: For Strata Forum */



                        /* For General Information for Owners */

                        routes.MapRoute(
                            name: "GeneralInformation",
                            url: portalname.ToString().ToLower() + "/GeneralInformation",
                            defaults: new { controller = "GeneralInformation", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                        name: "ListGeneralInformation",
                        url: portalname.ToString().ToLower() + "/GeneralInformation/ListGeneralInformation",
                        defaults: new { controller = "GeneralInformation", action = "ListGeneralInformation" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                    );

                        routes.MapRoute(
                      name: "GenralInformationInfinateScroll",
                      url: portalname.ToString().ToLower() + "/GeneralInformation/GenralInformationInfinateScroll",
                      defaults: new { controller = "GeneralInformation", action = "GenralInformationInfinateScroll" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        routes.MapRoute(
                      name: "GeneralInforamtionDownloadUploadedFile",
                      url: portalname.ToString().ToLower() + "/GeneralInformation/DownloadUploadedFile",
                      defaults: new { controller = "GeneralInformation", action = "DownloadUploadedFile" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                  );

                        /* END: For General Information for Owners */



                        /* For Admin Booking Request Approval */

                        routes.MapRoute(
                            name: "AdminBookingRequestListing",
                            url: portalname.ToString().ToLower() + "/adminbookingrequest",
                            defaults: new { controller = "adminbookingrequest", action = "index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "AdminLoadBookingRequestListing",
                            url: portalname.ToString().ToLower() + "/adminbookingrequest/loadbookinglist",
                            defaults: new { controller = "adminbookingrequest", action = "loadbookinglist" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        routes.MapRoute(
                            name: "AdminBookingRequestStatusUpdate",
                            url: portalname.ToString().ToLower() + "/adminbookingrequest/updatestatus",
                            defaults: new { controller = "adminbookingrequest", action = "updatestatus" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                        );

                        /* END: For Admin Booking Request Approval */



                    }
                }
                else
                {
                    routes.MapRoute(
                       name: "Login",
                       url: portalname.ToString().ToLower() + "/login",
                       defaults: new { controller = "logon", action = "Index" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                   );

                    routes.MapRoute(
                     name: "InvalidPortalLink",
                     url: portalname.ToString().ToLower() + "/logon/invalidportallink",
                     defaults: new { controller = "logon", action = "invalidportallink" }, namespaces: new[] { "StratasFair.Web.Controllers" }
                 );
                }
            }
        }
    }




}
