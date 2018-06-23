using StratasFair.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.NotificationService
{
    public class EmailSender
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public string SenderDisplayName { get; set; }
        public string To { get; set; }
        public string Bcc { get; set; }
        public string Body { get; set; }


        public string Host { get; set; }
        public int Port { get; set; }
        public AttachmentCollection Attachments { get; set; }
        public ArrayList AttachmentList { get; set; }
        public string MailStatus { get; set; }
        public string NetworkUserId { get; set; }
        public string NetworkUserPassword { get; set; }
        public string ErrorMessage { get; set; }


        public static EmailSender FncSendMail(EmailSender objMail)
        {
            FileStream ojbStream = null;
            try
            {
                string returnStatus = "";
                if (!ValidateMailObject(objMail, out returnStatus))
                {
                    if (returnStatus.Length > 0)
                    {
                        objMail.MailStatus = "Mail was not sent because " + returnStatus + " was not provided.";
                    }
                    return objMail;
                }

                MailMessage msg = new MailMessage();
                msg.HeadersEncoding = System.Text.Encoding.UTF8;
                msg.BodyEncoding = System.Text.Encoding.UTF8;

                msg.Subject = objMail.Subject.Trim().Length == 0 ? "No Subject" : objMail.Subject.Trim();
                msg.From = new MailAddress(objMail.From, objMail.SenderDisplayName);
                msg.To.Clear();
                msg.To.Add(objMail.To);
                msg.Bcc.Clear();
                string RDURL = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                objMail.Body = objMail.Body.Replace("{BASEPATH}", RDURL);
                objMail.Body = objMail.Body.Replace("{RDURL}", RDURL);
                objMail.Body = objMail.Body.Replace("{COPYYEAR}", DateTime.Now.Year.ToString());

                msg.Body = objMail.Body;
                if (objMail.Bcc != null && objMail.Bcc.Length > 0)
                {
                    msg.Bcc.Add(objMail.Bcc);
                }

                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.Attachments.Clear();

                if (objMail.AttachmentList != null)
                {
                    if (objMail.AttachmentList.Count > 0)
                    {
                        foreach (string attachmentItem in objMail.AttachmentList)
                        {
                            ojbStream = File.OpenRead(attachmentItem);
                            Attachment objAttachment = new Attachment(attachmentItem);
                            msg.Attachments.Add(new Attachment(ojbStream, Path.GetFileName(attachmentItem)));
                        }
                    }
                }

                SmtpClient sc = new SmtpClient(objMail.Host);
                sc.Port = objMail.Port;
                if (ConfigurationManager.AppSettings["IsReqSSLInMail"].ToUpper().Trim() == "Y")
                {
                    sc.EnableSsl = true;
                }
                else
                {
                    sc.EnableSsl = false;
                }
                sc.Credentials = new System.Net.NetworkCredential(objMail.NetworkUserId, objMail.NetworkUserPassword);
                sc.Send(msg);
                if (ojbStream != null)
                {
                    ojbStream.Close();
                    ojbStream.Dispose();
                }
                objMail.ErrorMessage = "success";
            }

            catch (SmtpFailedRecipientException ex)
            {
                 string failed = ex.FailedRecipient;
                  Exception exp =  ex.GetBaseException();
            }
            catch (Exception ex)
            {
                objMail.ErrorMessage = ex.Message;
                if (ojbStream != null)
                {
                    ojbStream.Close();
                    ojbStream.Dispose();
                }
            }
            return objMail;
        }

        private static bool ValidateMailObject(EmailSender objMail, out string _status)
        {
            _status = "";
            try
            {

                if (objMail.To == null || objMail.To.Trim() == "")
                {
                    _status = "To";
                    return false;
                }
                if (objMail.From == null || objMail.From.Trim() == "")
                {
                    _status = "From";
                    return false;
                }
                if (objMail.Host == null || objMail.Host.Trim() == "")
                {
                    _status = "Host";
                    return false;
                }
            }
            catch
            {
                throw;
            }
            return true;
        }

        /// <summary>
        /// Get the common setting from database
        /// </summary>
        /// <param name="EmailTo"></param>
        /// <param name="MailSubject"></param>
        /// <returns></returns>
        public static EmailSender GetCommonDataForSendMail()
        {


            EmailSender emailSender = new EmailSender();
            using (var db = new StratasFairDBEntities())
            {
                var emailServerDetail = db.tblEmailServers.Where(m => m.Active == 1).SingleOrDefault();
                emailSender.SenderDisplayName = emailServerDetail.SenderDisplayName;
                emailSender.From = emailServerDetail.FromEmail;
                emailSender.Host = emailServerDetail.EmailServer;
                emailSender.Port = (int)emailServerDetail.Port;
                emailSender.NetworkUserId = emailServerDetail.NetworkUserId;
                emailSender.NetworkUserPassword = emailServerDetail.NetworkUserPassword;
            }
            return emailSender;
        }

        /// <summary>
        /// Get the common setting from database
        /// </summary>
        /// <param name="EmailTo"></param>
        /// <param name="MailSubject"></param>
        /// <returns></returns>
        public static EmailSender GetCommonDataForSendMailClient(string EmailTo, string MailSubject, long? StrataBoardId)
        {
            EmailSender emailSender = new EmailSender();
            using (var db = new StratasFairDBEntities())
            {
                var emailServerDetail = db.tblEmailServers.Where(m => m.Active == 1).SingleOrDefault();
                var emailServerClientDetail = db.tblEmailServerClients.Where(m => m.Active == 1 && m.StratasBoardId == StrataBoardId).FirstOrDefault();

                emailSender.To = EmailTo;
                emailSender.Subject = MailSubject == null ? "" : MailSubject;
                emailSender.SenderDisplayName = emailServerClientDetail.SenderDisplayName;
                emailSender.From = emailServerClientDetail.FromEmail;
                emailSender.Host = emailServerDetail.EmailServer;
                emailSender.Port = (int)emailServerDetail.Port;
                emailSender.NetworkUserId = emailServerDetail.NetworkUserId;
                emailSender.NetworkUserPassword = emailServerDetail.NetworkUserPassword;
            }
            return emailSender;
        }


     

        /// <summary>
        /// function to read a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String ReadingFile(string path)
        {
            StreamReader fp = File.OpenText(path);
            string result = fp.ReadToEnd();
            fp.Close();
            return result;
        }



        /// <summary>
        /// Send the mail to admin about the expiration of subscription
        /// </summary>
        /// <param name="stratasBoardId"></param>
        /// <param name="strataName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="expiryDate"></param>
        /// <param name="adminEmailAddress"></param>
        /// <param name="reminderLevel"></param>
        /// <returns></returns>
        public static string FncSendMailboxMessage(long stratasBoardId, string strataName, string emailAddress, DateTime expiryDate, string adminEmailAddress, int reminderLevel)
        {
            try
            {
                EmailSender objClsMail = GetCommonDataForSendMail();
                objClsMail.To = adminEmailAddress;
                if (reminderLevel == 1)
                {
                    objClsMail.Subject = "First Subscription expired Reminder Message";
                }
                else
                {
                    objClsMail.Subject = "Second Subscription expired Reminder Message";
                }

                StringBuilder mailBody = new StringBuilder();
              
                mailBody.Append("Hi Admin, <br/><br/> Subscription is going to be expired soon for stratasboard:");
                mailBody.Append("<br/><br/><b>Stratasboard name</b> : " + strataName);
                mailBody.Append("<br/><b>Email address</b> : " + emailAddress);
                mailBody.Append("<br/><b>Expiration date</b> : " + expiryDate.ToString("MMM dd, yyyy"));
                objClsMail.Body = mailBody.ToString();
                objClsMail = FncSendMail(objClsMail);
                return objClsMail.ErrorMessage;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        public static string FncSendOwnerReminderMailBoxMessage(long? UserClientId, string ReminderSubject, string ReminderNote)
        {
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    var userClients = db.tblUserClients.Where(m => m.UserClientId == UserClientId).SingleOrDefault();
                    if (userClients != null)
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = "";
                        getConfigValue("stratasboard_owner_reminder", out body);
                        EmailSender emailSender = GetCommonDataForSendMailClient(userClients.EmailId, ReminderSubject, userClients.StratasBoardId);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{BASEPATH}", url);
                        mailBody.Replace("{NAME}", userClients.FirstName + " " + userClients.LastName);
                        mailBody.Replace("{NOTE}", ReminderNote);
                        mailBody.Replace("{YEAR}", DateTime.Now.Year.ToString());
                        emailSender.Body = mailBody.ToString();

                        emailSender = FncSendMail(emailSender);
                        return emailSender.ErrorMessage;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static void getConfigValue(string configKey, out string body)
        {
            using (var db = new StratasFairDBEntities())
            {
                var templateDetails = db.tblTemplateMasters.Where(m => m.Name == configKey && m.Status == 1).SingleOrDefault();
                if (templateDetails != null)
                {
                    body = templateDetails.ConfigValue;
                }
                else
                {
                    body = "";
                }
            }
        }

    }

}
