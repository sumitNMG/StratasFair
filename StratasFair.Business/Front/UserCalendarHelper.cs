using StratasFair.BusinessEntity;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.Business.Front
{
     
    public class UserCalendarHelper
    {
        StratasFairDBEntities context;
        public UserCalendarHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }


        /// <summary>
        /// Fetch Owner Calendar Display Data 
        /// </summary>
        /// <param name="type">"owner-general" or "owner-commonarea"</param>
        /// <param name="userClientId">UserClientId</param>
        /// <param name="stratasBoardId">StratasBoardId</param>
        /// <returns></returns>
        public List<CalendarProperties> GetUserCalendarData(string type, long userClientId, long stratasBoardId)
        {
            List<CalendarProperties> listCalendarData = new List<CalendarProperties>();
          
            if (type == "owner-general")
            {
                // Owner General Calendar data will be populated 
                listCalendarData = context.Usp_GetOwnerGeneralCalendarData(userClientId, stratasBoardId).Select(x => new CalendarProperties
                {
                    title = CommonHelper.CommonData.DecorateCalendarElementTitle(x.TableType, x.Title),
                    description = x.Description,

                    // If startdate has time then casting as "yyyy-MM-dd HH:mm:ss" otherwise "yyyy-MM-dd"
                    // "HH:mm:ss" 24 Hours Format  // "hh:mm:ss" 12 Hours Format
                    start = (x.StartDate.Value.Hour > 0 || x.StartDate.Value.Minute > 0) ? x.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : x.StartDate.Value.ToString("yyyy-MM-dd"),

                    // If enddate has time then casting as "yyyy-MM-dd HH:mm:ss" otherwise "yyyy-MM-dd"
                    // If time is not added in the startdate and enddate then it will be full day event in calendar display and shown at the top of "Day" part in calendar.
                    // If time is added in startdate and enddate then range will be displayed in calendar in the "Day" section.
                    // If startdate(2018-06-28 00:00:00.000) and enddate(2018-06-29 00:00:00.000) are not same and time is not mentioned then one day will be added in the enddate((2018-06-30 00:00:00.000))
                    end = string.IsNullOrEmpty(x.EndDate.ToString()) ? "" : (((x.StartDate.Value.Day == x.EndDate.Value.Day && x.StartDate.Value.Month == x.EndDate.Value.Month) || (x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0)) ? ((x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0) ? x.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : x.EndDate.Value.ToString("yyyy-MM-dd")) : (((x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0) ? x.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") : x.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")))),
                    color = x.ColorCode
                }).ToList();
            }
            else if (type == "owner-commonarea")
            {
                // Owner Community Calendar data will be populated 
                listCalendarData = context.Usp_GetOwnerCommonResourceCalendarData(userClientId, stratasBoardId).Select(x => new CalendarProperties
                {
                    title = CommonHelper.CommonData.DecorateCalendarElementTitle(x.TableType, x.Title),
                    description = x.Description,

                    // If startdate has time then casting as "yyyy-MM-dd HH:mm:ss" otherwise "yyyy-MM-dd"
                    // "HH:mm:ss" 24 Hours Format   // "hh:mm:ss" 12 Hours Format
                    start = (x.StartDate.Value.Hour > 0 || x.StartDate.Value.Minute > 0) ? x.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : x.StartDate.Value.ToString("yyyy-MM-dd"),

                    // If enddate has time then casting as "yyyy-MM-dd HH:mm:ss" otherwise "yyyy-MM-dd"
                    // If time is not added in the startdate and enddate then it will be full day event in calendar display and shown at the top of "Day" part in calendar.
                    // If time is added in startdate and enddate then range will be displayed in calendar in the "Day" section.
                    // If startdate(2018-06-28 00:00:00.000) and enddate(2018-06-29 00:00:00.000) are not same and time is not mentioned then one day will be added in the enddate((2018-06-30 00:00:00.000))
                    end = string.IsNullOrEmpty(x.EndDate.ToString()) ? "" : (((x.StartDate.Value.Day == x.EndDate.Value.Day && x.StartDate.Value.Month == x.EndDate.Value.Month) || (x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0)) ? ((x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0) ? x.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : x.EndDate.Value.ToString("yyyy-MM-dd")) : (((x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0) ? x.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") : x.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")))),
                    color = x.ColorCode
                }).ToList();
            }
            else if (type == "admin-commonarea")
            {
                // Admin Community Calendar data will be populated 
                listCalendarData = context.Usp_GetAdminCommonResourceCalendarData(userClientId, stratasBoardId).Select(x => new CalendarProperties
                {
                    title = CommonHelper.CommonData.DecorateCalendarElementTitle(x.TableType, x.Title),
                    description = x.Description,

                    // If startdate has time then casting as "yyyy-MM-dd HH:mm:ss" otherwise "yyyy-MM-dd"
                    // "HH:mm:ss" 24 Hours Format   // "hh:mm:ss" 12 Hours Format
                    start = (x.StartDate.Value.Hour > 0 || x.StartDate.Value.Minute > 0) ? x.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : x.StartDate.Value.ToString("yyyy-MM-dd"),

                    // If enddate has time then casting as "yyyy-MM-dd HH:mm:ss" otherwise "yyyy-MM-dd"
                    // If time is not added in the startdate and enddate then it will be full day event in calendar display and shown at the top of "Day" part in calendar.
                    // If time is added in startdate and enddate then range will be displayed in calendar in the "Day" section.
                    // If startdate(2018-06-28 00:00:00.000) and enddate(2018-06-29 00:00:00.000) are not same and time is not mentioned then one day will be added in the enddate((2018-06-30 00:00:00.000))
                    end = string.IsNullOrEmpty(x.EndDate.ToString()) ? "" : (((x.StartDate.Value.Day == x.EndDate.Value.Day && x.StartDate.Value.Month == x.EndDate.Value.Month) || (x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0)) ? ((x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0) ? x.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : x.EndDate.Value.ToString("yyyy-MM-dd")) : (((x.EndDate.Value.Hour > 0 || x.EndDate.Value.Minute > 0) ? x.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") : x.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")))),
                    color = x.ColorCode,
                    role = x.RoleName
                }).ToList();


            }
            return listCalendarData;
        }


      
    }
}
