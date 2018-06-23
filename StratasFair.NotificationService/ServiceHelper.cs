using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.NotificationService
{
    public class ServiceHelper
    {
        StratasFairDBEntities context;
        public String _conString = String.Empty;

        public ServiceHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }

        /// <summary>
        /// Send the reminder notification to stratafair admin for subscription expired in 15 and 6 days.
        /// </summary>
        public void SubscriptionExpireReminderData()
        {
            try
            {
                string adminEmailId = context.tblAdminEmailNotificationSettings.Where(x => x.Status == 1).FirstOrDefault().EmailAddress;
                LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- START - SubscriptionExpireReminderData  ");


                // get the stratasboard details whose subscription is going to be expired soon
                // Subscription details which date is going to expired in 15 days 
                DateTime dtFirstReminder = System.DateTime.UtcNow.AddDays(15);
                var subscriptionDetailsFirstReminder = context.vw_GetStratasBoard.Where(x => x.STATUS == 1 && x.ExpiryDate.Value.Year == dtFirstReminder.Year && x.ExpiryDate.Value.Month == dtFirstReminder.Month && x.ExpiryDate.Value.Day == dtFirstReminder.Day).ToList();
                if (subscriptionDetailsFirstReminder != null && subscriptionDetailsFirstReminder.Count > 0)
                {
                    foreach (var item in subscriptionDetailsFirstReminder)
                    {
                        if (!context.tblSubscriptionExpireMailReminders.Any(x => x.StratasBoardId == item.StratasBoardId && x.StratasBoardSubscriptionId == item.StratasBoardSubscriptionId && x.ReminderLevel == 1))
                        {
                            try
                            {
                                // send mail to stratasfair admin for information about expiration of subscription
                                string result = EmailSender.FncSendMailboxMessage(item.StratasBoardId, item.StratasBoardName, item.EmailId, item.ExpiryDate.Value, adminEmailId, 1);
                                if (result == "success")
                                {
                                    // "Mail send successfully.";
                                    LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- Mail send successfully to stratasfair admin for stratasboard named '" + item.StratasBoardName + "' - First Reminder - SubscriptionExpireReminderData ");

                                    int counter = AddMailReminder(item.StratasBoardId, item.StratasBoardSubscriptionId.Value, adminEmailId, 1);
                                    if (counter == 0)
                                    {
                                        LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- Record added successfully - First Reminder - SubscriptionExpireReminderData ");
                                    }
                                }
                                else
                                {
                                    LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- Mail Error - SubscriptionExpireReminderData " + result);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- Mail Error - SubscriptionExpireReminderData -- Exception: " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- No Record Found For subscription expiration data - First Reminder - SubscriptionExpireReminderData  ");
                }



                // get the stratasboard details whose subscription is going to be expired soon
                // Subscription details which date is going to expired in 6 days 
                DateTime dtSecondReminder = System.DateTime.UtcNow.AddDays(6);
                var subscriptionDetailsSecondReminder = context.vw_GetStratasBoard.Where(x => x.STATUS == 1 && x.ExpiryDate.Value.Year == dtSecondReminder.Year && x.ExpiryDate.Value.Month == dtSecondReminder.Month && x.ExpiryDate.Value.Day == dtSecondReminder.Day).ToList();
                if (subscriptionDetailsSecondReminder != null && subscriptionDetailsSecondReminder.Count > 0)
                {
                    foreach (var item in subscriptionDetailsSecondReminder)
                    {
                        if (!context.tblSubscriptionExpireMailReminders.Any(x => x.StratasBoardId == item.StratasBoardId && x.StratasBoardSubscriptionId == item.StratasBoardSubscriptionId && x.ReminderLevel == 2))
                        {
                            try
                            {
                                // send mail to stratasfair admin for information about expiration of subscription
                                string result = EmailSender.FncSendMailboxMessage(item.StratasBoardId, item.StratasBoardName, item.EmailId, item.ExpiryDate.Value, adminEmailId, 2);
                                if (result == "success")
                                {
                                    // "Mail send successfully.";
                                    LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "--  Mail send successfully to stratasfair admin for stratasboard named '" + item.StratasBoardName + "' - Second Reminder - SubscriptionExpireReminderData ");

                                    int counter = AddMailReminder(item.StratasBoardId, item.StratasBoardSubscriptionId.Value, adminEmailId, 2);
                                    if (counter == 0)
                                    {
                                        LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- Record added successfully - Second Reminder - SubscriptionExpireReminderData ");
                                    }
                                }
                                else
                                {
                                    LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- Mail Error - SubscriptionExpireReminderData ");
                                }
                            }
                            catch (Exception ex)
                            {
                                LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- Mail Error - SubscriptionExpireReminderData " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- No Record Found For license expiration data - Second Reminder - SubscriptionExpireReminderData  ");
                }
            }
            catch (Exception EX)
            {
                LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), EX.Message.ToString());
            }
            finally
            {
                LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), "-- END - SubscriptionExpireReminderData  ");
            }
        }

        /// <summary>
        /// Log the steps in txt file
        /// </summary>
        /// <param name="errorFile"></param>
        /// <param name="msg"></param>
        public static void LogEntryData(string errorFile, string msg)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(errorFile, true);
            sw.WriteLine(DateTime.UtcNow.ToString() + " - " + msg);
            sw.Close();
        }

        /// <summary>
        /// Add information in database regarding the email notification send to stratasfair admin
        /// </summary>
        /// <param name="stratasBoardId"></param>
        /// <param name="stratasBoardSubscriptionId"></param>
        /// <param name="adminEmailId"></param>
        /// <param name="reminderLevel"></param>
        /// <returns></returns>
        public int AddMailReminder(long stratasBoardId, int stratasBoardSubscriptionId, string adminEmailId, int reminderLevel)
        {
            int count = -1;
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    tblSubscriptionExpireMailReminder tblSubscriptionExpireMailReminderDb = new tblSubscriptionExpireMailReminder();
                    tblSubscriptionExpireMailReminderDb.StratasBoardId = stratasBoardId;
                    tblSubscriptionExpireMailReminderDb.StratasBoardSubscriptionId = stratasBoardSubscriptionId;
                    tblSubscriptionExpireMailReminderDb.ReminderLevel = reminderLevel;
                    tblSubscriptionExpireMailReminderDb.ReceiverEmailId = adminEmailId;
                    tblSubscriptionExpireMailReminderDb.CreatedOn = DateTime.UtcNow;
                    context.tblSubscriptionExpireMailReminders.Add(tblSubscriptionExpireMailReminderDb);
                    count = context.SaveChanges();
                    if (count == 1)
                    {
                        transaction.Commit();
                        count = 0;
                    }
                    else
                    {
                        LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), " --Transaction Rollback-- ");
                        transaction.Rollback();
                        count = -1;
                    }

                }
                catch (Exception ex)
                {
                    LogEntryData(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), " --Transaction Rollback-- " + ex.Message.ToString());
                    transaction.Rollback();
                    count = -2;   // any error is there
                }
            }
            return count;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// To Send the reminder notification to StrataBaord Owners.
        /// </summary>
        public void OwnerReminderService()
        {
            try
            {               
                LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), "-- START - OwnerReminderService  ");
                DateTime CurrentDate = DateTime.UtcNow;
                var OwnerReminders = context.tblReminders.Where(x => x.Status == 1 && x.StartDateTime <= CurrentDate).OrderBy(x => x.StartDateTime).ToList();

                if (OwnerReminders != null && OwnerReminders.Count > 0)
                {
                    string ReminderSentDate =string.Empty;
                    foreach (var OwnerRemindersItem in OwnerReminders)
                    {
                        if(!string.IsNullOrEmpty(OwnerRemindersItem.ReminderNote))
                        {
                            string ReminderDate = context.Usp_OwnerReminderService(Convert.ToInt32(OwnerRemindersItem.ReminderId), OwnerRemindersItem.Occurrence, OwnerRemindersItem.StartDateTime).FirstOrDefault();
                            ReminderDate = string.IsNullOrEmpty(ReminderDate) ? "NoRecord" : ReminderDate;
                            if (!string.IsNullOrEmpty(ReminderDate.ToString()))
                            {
                                if (ReminderDate != "01/01/1900")
                                {
                                    try
                                    {
                                        string result = "success"; // EmailSender.FncSendOwnerReminderMailBoxMessage(OwnerRemindersItem.CreatedBy, OwnerRemindersItem.ReminderSubject, OwnerRemindersItem.ReminderNote);
                                        if (result == "success")
                                        {
                                            // "Mail send successfully.";
                                            LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), "-- Reminder Mail send successfully to StrataBoard Owner");
                                            int Counter = 0;
                                            if (ReminderDate.ToString() == "NoRecord")
                                            {
                                                Counter = AddSentReminderDetails(OwnerRemindersItem.ReminderId, OwnerRemindersItem.StartDateTime, true);
                                            }
                                            else
                                            {
                                                Counter = AddSentReminderDetails(OwnerRemindersItem.ReminderId, Convert.ToDateTime(ReminderDate), true);
                                            }
                                            if (Counter == 1)
                                            {
                                                LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), "-- Reminder Details Record added successfully - OwnerReminderService ");
                                            }
                                        }
                                        else
                                        {
                                            LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), "-- Mail Error - OwnerReminderService " + result);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), "-- Mail Error - OwnerReminderService -- Exception: " + ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), "-- Mail Already Sent - OwnerReminderService");
                            }
                        }                        
                    }
                }
                else
                {
                    LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), "-- No Record Found - OwnerReminderService");
                }
            }
            catch
            {
                throw;
            }
        }


        private string GetSentReminderDate(long ReminderId, int? Occurrence, DateTime? StartDateTime)
        {
            _conString = SqlHelper.GetConnectionString();

            SqlParameter prmReminderId = SqlHelper.CreateParameter("@ReminderId", ReminderId);
            SqlParameter prmOccurence = SqlHelper.CreateParameter("@Occurence", Occurrence);
            SqlParameter prmCompareDate = SqlHelper.CreateParameter("@CompareDate", StartDateTime);
            SqlParameter[] allParams = { prmReminderId, prmOccurence, prmCompareDate };
            DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_OwnerReminderService", allParams);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dtResult = ds.Tables[0].Copy();
                ds.Dispose();
                if (dtResult.Rows.Count > 0)
                {
                    if(dtResult.Columns.Contains("CompareDate"))
                    {
                        string ReminderSentdate = dtResult.Rows[0]["CompareDate"].ToString();
                        return ReminderSentdate;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return "NoRecord";
                }
            }
            else
            {
                return "NoRecord";
            }
        }
        /// <summary>
        /// Add information in database table tblReminderDetails regarding the email notification send to StrataBoard Owner
        /// </summary>
        /// <param name="ReminderId"></param>
        /// <param name="StartDate"></param>
        /// <param name="IsReminderSent"></param>
        /// <returns></returns>
        public int AddSentReminderDetails(long ReminderId, DateTime? StartDate, bool IsReminderSent)
        {
            int count = -1;
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    tblReminderDetail tblReminderDetailDb = new tblReminderDetail();
                    tblReminderDetailDb.ReminderId = ReminderId;
                    tblReminderDetailDb.StartDate = StartDate;
                    tblReminderDetailDb.IsReminderSent = IsReminderSent;
                    tblReminderDetailDb.CreatedOn = DateTime.UtcNow;
                    tblReminderDetailDb.CreatedFromIP = "";
                    tblReminderDetailDb.Status = 1;
                    context.tblReminderDetails.Add(tblReminderDetailDb);
                    count = context.SaveChanges();
                    if (count == 1)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), " --Transaction Rollback-- ");
                        transaction.Rollback();
                        count = -1;
                    }
                }
                catch (Exception ex)
                {
                    LogEntryData(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), " --Transaction Rollback-- " + ex.Message.ToString());
                    transaction.Rollback();
                    count = -2;   // any error is there
                }
            }
            return count;
        }
    }
}
