using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Front
{
    public class MyReminderHelper
    {
        StratasFairDBEntities _context;

        public MyReminderHelper()
        {
            if (_context == null)
                _context = new StratasFairDBEntities();
        }


        public List<ReminderModel> GetAllReminders(int BlockNumber, int BlockSize)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<ReminderModel> reminderModelList = new List<ReminderModel>();
                try
                {
                    var Reminders = _context.tblReminders.Where(x => x.Status == 1 && x.StratasBoardId == ClientSessionData.ClientStrataBoardId).OrderByDescending(x => x.CreatedOn).ToList();
                    foreach (var item in Reminders)
                    {
                        ReminderModel reminderModel = new ReminderModel();
                        reminderModel.ReminderId = item.ReminderId;
                        reminderModel.ReminderNote = string.IsNullOrEmpty(item.ReminderNote) ? "N/A" : item.ReminderNote;
                        reminderModel.SmsText = string.IsNullOrEmpty(item.SmsText) ? "N/A" : item.SmsText;
                        reminderModel.ReminderSubject = item.ReminderSubject;
                        reminderModel.Occurrence = item.Occurrence;
                        reminderModel.ReminderOccurrence = item.Occurrence == 2 ? "Daily" : item.Occurrence == 3 ? "Weekly" : item.Occurrence == 4 ? "Bi-Weekly" : item.Occurrence == 5 ? "Monthly" : item.Occurrence == 6 ? "Quarterly" : item.Occurrence == 7 ? "Semi-Annually" : "Once";
                        string StartTime = item.StartDateTime.Value.ToString("hh:mm tt");
                        reminderModel.StartDateTime = item.StartDateTime != null ? item.StartDateTime.Value.ToString("dd MMM, yyyy") + " at " + StartTime : "";
                        reminderModelList.Add(reminderModel);
                    }
                }
                catch
                {
                    return null;
                }
                reminderModelList = reminderModelList.Skip(startIndex).Take(BlockSize).ToList();
                return reminderModelList;
            }
            else
            {
                return null;
            }
        }

        public ReminderModel GetReminderDetail(int ReminderId)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    var Reminders = _context.tblReminders.Where(x => x.Status == 1 && x.ReminderId == ReminderId).FirstOrDefault();
                    ReminderModel reminderModel = new ReminderModel();
                    reminderModel.ReminderId = Reminders.ReminderId;
                    reminderModel.ReminderNote = Reminders.ReminderNote;
                    reminderModel.SmsText = Reminders.SmsText;
                    reminderModel.ReminderSubject = Reminders.ReminderSubject;
                    reminderModel.Occurrence = Reminders.Occurrence;
                    reminderModel.StartTimeHour = Reminders.StartDateTime.Value.Hour;
                    if (reminderModel.StartTimeHour == 0)
                    {
                        reminderModel.StartTimeHour = 12;
                        reminderModel.StartTimeAmPm = "AM";
                    }
                    else if (reminderModel.StartTimeHour >= 12)  //// For PM
                    {
                        if (reminderModel.StartTimeHour > 12)
                        {
                            reminderModel.StartTimeHour = reminderModel.StartTimeHour - 12;
                        }
                        reminderModel.StartTimeAmPm = "PM";
                    }
                    else  //// For AM
                    {
                        reminderModel.StartTimeAmPm = "AM";
                    }
                    reminderModel.StartTimeMinute = Convert.ToInt32(Reminders.StartDateTime.Value.Minute.ToString().PadLeft(2, '0'));
                    reminderModel.StartDateTime = Reminders.StartDateTime != null ? Reminders.StartDateTime.Value.ToString("dd MMM yyyy") : "";
                    return reminderModel;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public int AddMyReminder(ReminderModel reminderModel)
        {
            int result = 0;
            try
            {
                tblReminder tblReminderDb = new tblReminder();
                tblReminderDb.ReminderNote = reminderModel.ReminderNote;
                tblReminderDb.SmsText = reminderModel.SmsText;
                tblReminderDb.ReminderSubject = reminderModel.ReminderSubject;
                tblReminderDb.Occurrence = reminderModel.Occurrence;
                tblReminderDb.Status = 1;
                tblReminderDb.StartDateTime = Convert.ToDateTime(reminderModel.StartDateTime);
                if (reminderModel.StartTimeAmPm == "PM")
                {
                    if (reminderModel.StartTimeHour != 12)
                    {
                        reminderModel.StartTimeHour = Convert.ToInt32(reminderModel.StartTimeHour) + 12;
                    }
                }
                else if (reminderModel.StartTimeAmPm == "AM")
                {
                    if (reminderModel.StartTimeHour == 12)
                    {
                        reminderModel.StartTimeHour = 0;
                    }
                }
                tblReminderDb.StartDateTime = tblReminderDb.StartDateTime.Value.AddHours(Convert.ToInt32(reminderModel.StartTimeHour)).AddMinutes(Convert.ToInt32(reminderModel.StartTimeMinute));

                tblReminderDb.CreatedBy = ClientSessionData.UserClientId;
                tblReminderDb.CreatedOn = DateTime.UtcNow;
                tblReminderDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblReminderDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;
                

                if (reminderModel.ReminderId > 0)
                {
                    tblReminderDb.ReminderId = reminderModel.ReminderId;
                    _context.tblReminders.Attach(tblReminderDb);
                    _context.Entry(tblReminderDb).Property(x => x.ReminderNote).IsModified = true;
                    _context.Entry(tblReminderDb).Property(x => x.SmsText).IsModified = true;
                    _context.Entry(tblReminderDb).Property(x => x.ReminderSubject).IsModified = true;
                    _context.Entry(tblReminderDb).Property(x => x.Occurrence).IsModified = true;
                    _context.Entry(tblReminderDb).Property(x => x.StartDateTime).IsModified = true;
                    result = _context.SaveChanges();
                }
                else
                {
                    var UserReminder = _context.tblReminders.Where(x => x.ReminderSubject == reminderModel.ReminderSubject && x.StartDateTime == tblReminderDb.StartDateTime && x.Status == 1).FirstOrDefault();
                    if (UserReminder == null)
                    {
                        _context.tblReminders.Add(tblReminderDb);
                        result = _context.SaveChanges();
                    }
                    else
                    {
                        result = -1;
                    }
                }
            }
            catch
            {

            }
            return result;
        }


        public int DeleteMyReminder(int ReminderId)
        {
            int result = 0;
            try
            {

                var UserReminder = _context.tblReminders.Where(x => x.ReminderId == ReminderId).FirstOrDefault();

                if (UserReminder != null)
                {
                    _context.tblReminders.Remove(UserReminder);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }
    }
}
