using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace StratasFair.Business.Admin
{
    public static class AdminSessionData
    {
        /* For the admin user id */
        const string AdminUserIdKey = "AdminUserId";
        public static int AdminUserId
        {
            get { return HttpContext.Current.Session[AdminUserIdKey] != null ? (int)HttpContext.Current.Session[AdminUserIdKey] : 0; }
            set { HttpContext.Current.Session[AdminUserIdKey] = value; }
        }

        /* For the admin role id */
        const string AdminRoleIdKey = "AdminRoleId";
        public static int AdminRoleId
        {
            get { return HttpContext.Current.Session[AdminRoleIdKey] != null ? (int)HttpContext.Current.Session[AdminRoleIdKey] : 0; }
            set { HttpContext.Current.Session[AdminRoleIdKey] = value; }
        }

        /* For the admin role name */
        const string AdminRoleNameKey = "AdminRoleName";
        public static string AdminRoleName
        {
            get { return HttpContext.Current.Session[AdminRoleNameKey] != null ? (string)HttpContext.Current.Session[AdminRoleNameKey] : ""; }
            set { HttpContext.Current.Session[AdminRoleNameKey] = value; }
        }


        /* For the admin user name */
        const string AdminUserNameKey = "AdminUserName";
        public static string AdminUserName
        {
            get { return HttpContext.Current.Session[AdminUserNameKey] != null ? (string)HttpContext.Current.Session[AdminUserNameKey] : ""; }
            set { HttpContext.Current.Session[AdminUserNameKey] = value; }
        }


        /* For the admin name */
        const string AdminNameKey = "AdminName";
        public static string AdminName
        {
            get { return HttpContext.Current.Session[AdminNameKey] != null ? (string)HttpContext.Current.Session[AdminNameKey] : ""; }
            set { HttpContext.Current.Session[AdminNameKey] = value; }
        }


        /* For the admin created on date */
        const string AdminCreatedOnKey = "AdminCreatedOn";
        public static string AdminCreatedOn
        {
            get { return HttpContext.Current.Session[AdminCreatedOnKey] != null ? (string)HttpContext.Current.Session[AdminCreatedOnKey] : ""; }
            set { HttpContext.Current.Session[AdminCreatedOnKey] = value; }
        }

        /* For the admin last login date */
        const string AdminLastLoginOnKey = "AdminLastLoginOn";
        public static string AdminLastLoginOn
        {
            get { return HttpContext.Current.Session[AdminLastLoginOnKey] != null ? (string)HttpContext.Current.Session[AdminLastLoginOnKey] : ""; }
            set { HttpContext.Current.Session[AdminLastLoginOnKey] = value; }
        }

        /* For the admin session log id */
        const string LogTimeKey = "LogTime";
        public static long LogTime
        {
            get { return HttpContext.Current.Session[LogTimeKey] != null ? (long)HttpContext.Current.Session[LogTimeKey] : 0; }
            set { HttpContext.Current.Session[LogTimeKey] = value; }
        }
    }
}
