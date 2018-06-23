using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StratasFair.Business.CommonHelper
{
    public static class CommonData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">message for print</param>
        /// <param name="status">0 for error, 1 for success</param>
        /// <returns></returns>
        public static String GetMessage(string message, int status)
        {
            //string strMsgClass = "MsgGreen";
            //if (status == 0)
            //    strMsgClass = "MsgRed";

            string strMsgClass = "alert alert-success alert-dismissable";
            string strIcon = "fa fa-check";
            if (status == 0)
            {
                strMsgClass = "alert alert-danger alert-dismissable";
                strIcon = "fa fa-ban";
            }


            return "<div class=\"alert-mesg\">" +
                     "<div class=\"" + strMsgClass + "\">" +
                         "<i class=\"" + strIcon + "\"></i>" +
                         "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">" +
                             "&times;" +
                         "</button>" +
                         message +
                      "</div>" +
                     "</div>";
        }

        public static string FetchSettingValue(string settingName)
        {
            string value = string.Empty;
            using (var db = new StratasFairDBEntities())
            {
                var settingdb = db.tblSettings.Where(x => x.SettingName.ToLower() == settingName && x.Status == 1).FirstOrDefault();
                if (settingdb != null)
                {
                    value = settingdb.SettingValue;
                }
            }
            return value;
        }

      



        public static int FetchDataCounters(string settingName)
        {
            int value = 0;
            using (var db = new StratasFairDBEntities())
            {
                switch (settingName.ToLower())
                {
                    case "vendor":
                        value = db.tblVendors.Where(x => x.Status == 1).Count();
                        break;
                    case "discipline":
                        value = db.tblDisciplines.Where(x => x.Status == 1).Count();
                        break;
                    case "testimonial":
                        value = db.tblTestimonials.Where(x => x.Status == 1).Count();
                        break;
                    case "subscription":
                        value = db.vw_GetSubscriptionMaster.Where(x => x.STATUS == 1).Count();
                        break;
                    case "stratasboard":
                        //DateTime currDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        //value = db.vw_GetStratasBoard.Where(x => x.STATUS == 1 && x.ExpiryDate >= currDate).Count();
                        value = db.vw_GetStratasBoard.Where(x => x.STATUS == 1).Count();
                        break;
                    case "new-forum-message":
                        DateTime currDate = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                        value = db.tblForums.Where(x => x.CreatedOn == currDate).Count();
                        break;
                    default: value = 0;
                        break;
                }
            }
            return value;
        }


        public static int FetchDataCounters(string settingName, long userClientId, long stratasBoardId)
        {
            int value = 0;
            using (var db = new StratasFairDBEntities())
            {
                switch (settingName.ToLower())
                {
                    case "new-forum-message":
                        //DateTime currDate = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());

                        int count = db.tblForumUsers.Where(x => x.UserClientId == userClientId && x.IsRead == 0).ToList().Count;
                        int count2 = db.tblForumReplyUsers.Where(x => x.UserClientId == userClientId && x.IsRead == 0).ToList().Count;

                        value = count + count2 ; //db.tblForums.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CreatedOn.Value) == System.Data.Entity.DbFunctions.TruncateTime(DateTime.UtcNow) && x.StratasBoardId == stratasBoardId).Count();
                        break;
                    default: value = 0;
                        break;
                }
            }
            return value;
        }

        public static List<SelectListItem> GetDisciplineList()
        {
            using (var context = new StratasFairDBEntities())
            {
                var result = context.vw_GetDiscipline.Where(x => x.Status == 1).OrderByDescending(o => o.DisplayOrder).ThenBy(n => n.DisciplineName).ToList();
                return result.Select(x => new SelectListItem()
                {
                    Value = x.DisciplineId.ToString(),
                    Text = x.DisciplineName
                }).ToList();
            }
        }

        public static List<SelectListItem> GetCommonAreaList(long? StratasBoardId)
        {
            using (var db = new StratasFairDBEntities())
            {
                return db.tblCommonAreas.Where(x => x.Status == 1 && x.StratasBoardId == StratasBoardId).OrderBy(x => x.CommonAreaName).Select(x => new SelectListItem()
                {
                    Value = x.CommonAreaId.ToString(),
                    Text = x.CommonAreaName
                }).ToList();
            }
        }

        public static List<SelectListItem> GetAdminStatusList()
        {
            return new List<SelectListItem>
                       {
                           new SelectListItem {Text = "Pending", Value = "0"},
                           new SelectListItem {Text = "Approved", Value = "1"},
                           new SelectListItem {Text = "Rejected", Value = "2"}
                       };
        }

        public static List<SelectListItem> GetSubscriptionList()
        {
            using (var context = new StratasFairDBEntities())
            {
                return context.vw_GetSubscriptionMaster.Where(x => x.STATUS == 1).Select(x => new SelectListItem()
                {
                    Value = x.SubscriptionId.ToString(),
                    Text = x.SubscriptionName
                }).ToList();
            }
        }

        public static List<SelectListItem> GetSubscriptionWithDaysList()
        {
            using (var context = new StratasFairDBEntities())
            {
                return context.vw_GetSubscriptionMaster.Where(x => x.STATUS == 1).Select(x => new SelectListItem()
                {
                    Value = x.SubscriptionId.ToString(),
                    Text = x.SubscriptionName + x.SubscriptionWithValidity
                }).ToList();
            }
        }

        public static List<StratasFair.BusinessEntity.DisciplineModel> GetDisciplineWithoutOtherList()
        {
            using (var context = new StratasFairDBEntities())
            {
                return context.vw_GetDiscipline.Where(x => 
                    x.Status == 1 
                    && x.DisciplineName.ToLower() != "other" 
                    && x.DisciplineName.ToLower() != "others")
                    .Select(x => new StratasFair.BusinessEntity.DisciplineModel()
                {
                    DisciplineId = x.DisciplineId,
                    DisciplineName = x.DisciplineName,
                    DisplayOrder=x.DisplayOrder
                }).OrderBy(y=>y.DisplayOrder).ToList();
            }
        }


        public static List<SelectListItem> GetVendorAdminStatus()
        {
            return new List<SelectListItem>
                       {
                           new SelectListItem {Text = "Pending", Value = "0"},
                           new SelectListItem {Text = "Approved", Value = "1"},
                           new SelectListItem {Text = "Rejected", Value = "2"}
                       };
        }

        public static string GetAdminEmailForNotification()
        {
            using (var context = new StratasFairDBEntities())
            {
                return context.tblAdminEmailNotificationSettings.Where(x => x.Status == 1).FirstOrDefault().EmailAddress;
            }
        }

        /// <summary>
        /// Get total number of user in stratasboard.
        /// </summary>
        /// <param name="stratasBoardId"></param>
        /// <returns></returns>
        public static int GetStratasBoardTotalUser(long stratasBoardId)
        {
            using (var context = new StratasFairDBEntities())
            {
                int counter = context.tblUserClients.Where(x => x.StratasBoardId == stratasBoardId).Count();
                return counter;
            }
        }

        /// <summary>
        /// Get all the stratas unique name
        /// </summary>
        /// <returns></returns>
        public static List<string> GetStratasUniqueName()
        {
            using (var context = new StratasFairDBEntities())
            {
                return context.vw_GetStratasBoard.Where(x => x.STATUS == 1).Select(x => x.PortalLink).ToList();
            }
        }


        public static bool IsVendorEmailExists(string email)
        {
            using (var context = new StratasFairDBEntities())
            {
                return context.tblVendors.Any(x => x.EmailId == email);
            }
        }


        /// <summary>
        /// Used for Updating Title in the calendar element display
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string DecorateCalendarElementTitle(string type, string title)
        {
            string returnString = title;
            switch (type)
            {
                case "maintenance":
                    returnString = "Maintenance: " + title;
                    break;
                case "request":
                    returnString = "Request: " + title;
                    break;
                case "meeting":
                    returnString = "Meeting: " + title;
                    break;
                case "reminder":
                    returnString = "Reminder: " + title;
                    break;
                case "booking":
                    returnString = "Booking: " + title;
                    break;
            }
            return returnString;
        }

    }
}
