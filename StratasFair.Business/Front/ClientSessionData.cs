using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Front
{
    public class ClientSessionData
    {
        /* For the Client user id */
        const string UserClientIdKey = "UserClientId";
        public static int UserClientId
        {
            get { return HttpContext.Current.Session[UserClientIdKey] != null ? (int)HttpContext.Current.Session[UserClientIdKey] : 0; }
            set { HttpContext.Current.Session[UserClientIdKey] = value; }
        }

        /* For the Client role name */
        const string ClientRoleNameKey = "ClientRoleName";
        public static string ClientRoleName
        {
            get { return HttpContext.Current.Session[ClientRoleNameKey] != null ? (string)HttpContext.Current.Session[ClientRoleNameKey] : ""; }
            set { HttpContext.Current.Session[ClientRoleNameKey] = value; }
        }


        /* For the Client user name */
        const string ClientUserNameKey = "ClientUserName";
        public static string ClientUserName
        {
            get { return HttpContext.Current.Session[ClientUserNameKey] != null ? (string)HttpContext.Current.Session[ClientUserNameKey] : ""; }
            set { HttpContext.Current.Session[ClientUserNameKey] = value; }
        }


        /* For the Client name */
        const string ClientNameKey = "ClientName";
        public static string ClientName
        {
            get { return HttpContext.Current.Session[ClientNameKey] != null ? (string)HttpContext.Current.Session[ClientNameKey] : ""; }
            set { HttpContext.Current.Session[ClientNameKey] = value; }
        }


        /* For the Client created on date */
        const string ClientCreatedOnKey = "ClientCreatedOn";
        public static string ClientCreatedOn
        {
            get { return HttpContext.Current.Session[ClientCreatedOnKey] != null ? (string)HttpContext.Current.Session[ClientCreatedOnKey] : ""; }
            set { HttpContext.Current.Session[ClientCreatedOnKey] = value; }
        }

        /* For the Client last login date */
        const string ClientLastLoginOnKey = "ClientLastLoginOn";
        public static string ClientLastLoginOn
        {
            get { return HttpContext.Current.Session[ClientLastLoginOnKey] != null ? (string)HttpContext.Current.Session[ClientLastLoginOnKey] : ""; }
            set { HttpContext.Current.Session[ClientLastLoginOnKey] = value; }
        }

        /* For the Client PortalLink */
        const string ClientPortalLinkKey = "ClientPortalLink";
        public static string ClientPortalLink
        {
            get { return HttpContext.Current.Session[ClientPortalLinkKey] != null ? (string)HttpContext.Current.Session[ClientPortalLinkKey] : ""; }
            set { HttpContext.Current.Session[ClientPortalLinkKey] = value; }
        }

        /* For the Client StrataBoardId */
        const string ClientStrataBoardIdKey = "ClientStrataBoardId";
        public static int ClientStrataBoardId
        {
            get { return HttpContext.Current.Session[ClientStrataBoardIdKey] != null ? (int)HttpContext.Current.Session[ClientStrataBoardIdKey] : 0; }
            set { HttpContext.Current.Session[ClientStrataBoardIdKey] = value; }
        }

        /* For the Client IsEmailNotification */
        const string ClientIsEmailNotificationKey = "ClientIsEmailNotification";
        public static bool ClientIsEmailNotification
        {
            get { return HttpContext.Current.Session[ClientIsEmailNotificationKey] != null ? (bool)HttpContext.Current.Session[ClientIsEmailNotificationKey] : false; }
            set { HttpContext.Current.Session[ClientIsEmailNotificationKey] = value; }
        }

        /* For the Client IsSMSNotification */
        const string ClientIsSMSNotificationKey = "ClientIsSMSNotification";
        public static bool ClientIsSMSNotification
        {
            get { return HttpContext.Current.Session[ClientIsSMSNotificationKey] != null ? (bool)HttpContext.Current.Session[ClientIsSMSNotificationKey] : false; }
            set { HttpContext.Current.Session[ClientIsSMSNotificationKey] = value; }
        }

        /* For the Client ProfilePicture */
        const string ClientProfilePictureKey = "ClientProfilePicture";
        public static string ClientProfilePicture
        {
            get { return HttpContext.Current.Session[ClientProfilePictureKey] != null ? (string)HttpContext.Current.Session[ClientProfilePictureKey] : ""; }
            set { HttpContext.Current.Session[ClientProfilePictureKey] = value; }
        }

        /* For the Client ProfilePicture */
        const string ClientIsProfileCompletedKey = "ClientIsProfileCompleted";
        public static bool ClientIsProfileCompleted
        {
            get { return HttpContext.Current.Session[ClientIsProfileCompletedKey] != null ? (bool)HttpContext.Current.Session[ClientIsProfileCompletedKey] : false; }
            set { HttpContext.Current.Session[ClientIsProfileCompletedKey] = value; }
        }
    }
}
