using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using StratasFair.Data;
using System.IO;
using StratasFair.Business.Admin;
using StratasFair.BusinessEntity.Admin;
using StratasFair.BusinessEntity;
using StratasFair.Business.Front;

namespace StratasFair.Business.CommonHelper
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
        StratasFairDBEntities _context;

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
                //string basepath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
                string RDURL = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                objMail.Body = objMail.Body.Replace("{BASEPATH}", RDURL);
                objMail.Body = objMail.Body.Replace("{RDURL}", RDURL);
                objMail.Body = objMail.Body.Replace("{YEAR}", DateTime.Now.Year.ToString());

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
            catch (Exception ex)
            {
                objMail.ErrorMessage = ex.Message;
                if (ojbStream != null)
                {
                    ojbStream.Close();
                    ojbStream.Dispose();
                }

                AppError appError = new AppError();
                appError.LogMe(ex);
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
        public static EmailSender GetCommonDataForSendMail(string EmailTo, string MailSubject)
        {

            EmailSender emailSender = new EmailSender();
            using (var db = new StratasFairDBEntities())
            {
                var emailServerDetail = db.tblEmailServers.Where(m => m.Active == 1).SingleOrDefault();

                emailSender.To = EmailTo;
                emailSender.Subject = MailSubject == null ? "" : MailSubject;
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
        public static EmailSender GetCommonDataForSendMailClient(string EmailTo, string MailSubject, long StratasBoardId)
        {
            EmailSender emailSender = new EmailSender();
            using (var db = new StratasFairDBEntities())
            {
                var emailServerDetail = db.tblEmailServers.Where(m => m.Active == 1).SingleOrDefault();
                var emailServerClientDetail = db.tblEmailServerClients.Where(m => m.Active == 1 && m.StratasBoardId == StratasBoardId).FirstOrDefault();

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


        public static string getConfigValue(string configKey)
        {
            using (var db = new StratasFairDBEntities())
            {
                var templateDetails = db.tblTemplateMasters.Where(m => m.Name == configKey && m.Status == 1).SingleOrDefault();
                if (templateDetails != null)
                {
                    return templateDetails.ConfigValue;
                }
                else
                {
                    return "";
                }

            }
        }


        public static void getConfigValue(string configKey, out string body, out string subject)
        {
            using (var db = new StratasFairDBEntities())
            {
                var templateDetails = db.tblTemplateMasters.Where(m => m.Name == configKey && m.Status == 1).SingleOrDefault();
                if (templateDetails != null)
                {
                    body = templateDetails.ConfigValue;
                    subject = templateDetails.Title;
                }
                else
                {
                    body = "";
                    subject = "";
                }
            }
        }


        public static int FncSendAdminPasswordReminderMail(string emailAddress, string password, string UserName, string AdminName, string verticalId)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();

                string body = "";
                string subject = "";
                getConfigValue("forget-password", out body, out subject);

                Encrypt64 encryptDecrypt = new Encrypt64();
                EmailSender objClsMail = GetCommonDataForSendMail(emailAddress, subject);

                StringBuilder mailBody = new StringBuilder();
                mailBody.Append(body);
                mailBody.Replace("{USERNAME}", UserName);
                mailBody.Replace("{ADMINNAME}", AdminName);
                mailBody.Replace("{PASSWORD}", encryptDecrypt.Decrypt(password, ConfigurationManager.AppSettings["SecureKey"].ToString()));
                mailBody.Replace("{URL}", url + "securehost/rootlogin/");
                mailBody.Replace("{BASEPATH}", url);
                objClsMail.Body = mailBody.ToString();
                FncSendMail(objClsMail);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public static string SendTestMailFromAdmin(string emailTo, string subject, string body)
        {
            try
            {
                EmailSender emailSender = GetCommonDataForSendMail(emailTo, subject);
                emailSender.Body = body == null ? "" : body;
                emailSender = FncSendMail(emailSender);
                return emailSender.ErrorMessage;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }



        public static string FncSendVendorApprovedMailFromAdmin(string emailAddress, string vendorName, string password)
        {
            try
            {
                Encrypt64 encryptDecrypt = new Encrypt64();

                string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                string body = "";
                string subject = "";
                getConfigValue("vendor-approved-email", out body, out subject);
                EmailSender emailSender = GetCommonDataForSendMail(emailAddress, subject);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append(body);
                mailBody.Replace("{PASSWORD}", encryptDecrypt.Decrypt(password, ConfigurationManager.AppSettings["SecureKey"].ToString()));
                mailBody.Replace("{URL}", url+"vendor/login");
                mailBody.Replace("{BASEPATH}/", url);
                mailBody.Replace("{EMAILADDRESS}", emailAddress);
                mailBody.Replace("{VENDORNAME}", vendorName);
                emailSender.Body = mailBody.ToString();
                emailSender = FncSendMail(emailSender);
                return emailSender.ErrorMessage;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        public static string FncSendVendorRejectedMailFromAdmin(string emailAddress, string vendorName, string remark)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                string body = "";
                string subject = "";
                getConfigValue("vendor-rejected-email", out body, out subject);
                EmailSender emailSender = GetCommonDataForSendMail(emailAddress, subject);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append(body);
                mailBody.Replace("{BASEPATH}/", url);
                mailBody.Replace("{EMAILADDRESS}", emailAddress);
                mailBody.Replace("{VENDORNAME}", vendorName);
                mailBody.Replace("{REMARK}", remark);
                emailSender.Body = mailBody.ToString();
                emailSender = FncSendMail(emailSender);
                return emailSender.ErrorMessage;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static int FncSendPasswordResetMailToVendor(string emailAddress,long vendorId, string VendorName)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();

                string body = "";
                string subject = "";
                getConfigValue("forget-password-vendor", out body, out subject);

                Encrypt64 enc = new Encrypt64();
                EmailSender objClsMail = GetCommonDataForSendMail(emailAddress, subject);

                StringBuilder mailBody = new StringBuilder();
                mailBody.Append(body);
                mailBody.Replace("{NAME}", VendorName);
                mailBody.Replace("{URL}", url + "vendor/resetpassword?email="+emailAddress);
                mailBody.Replace("{BASEPATH}", url);
                objClsMail.Body = mailBody.ToString();
                FncSendMail(objClsMail);
                return 1;
            }
            catch
            {
                return -1;
            }
        }


        public static string FncSend_StratasBoard_RegistrationMail_ToClient(long stratasBoardId)
        {
            try
            {

                using (var db = new StratasFairDBEntities())
                {
                    var stratasDetails = db.vw_GetStratasBoard.Where(m => m.StratasBoardId == stratasBoardId).SingleOrDefault();
                    if (stratasDetails != null)
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = "";
                        string subject = "";
                        getConfigValue("stratasboard-registration-mail", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMail(stratasDetails.EmailId, subject);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{PASSWORD}", AppLogic.DecryptPassword(stratasDetails.Password));
                        mailBody.Replace("{BASEPATH}/", url);
                        mailBody.Replace("{NAME}", stratasDetails.FirstName + " " + stratasDetails.LastName);
                        mailBody.Replace("{STRATANAME}", stratasDetails.StratasBoardName);
                        mailBody.Replace("{PORTALURL}", url + stratasDetails.PortalLink + "/login");
                        mailBody.Replace("{EMAILADDRESS}", stratasDetails.EmailId);
                        emailSender.Body = mailBody.ToString();

                        //for attachment

                        //emailSender.AttachmentList = new ArrayList(1);
                        //emailSender.AttachmentList.Add(System.Web.HttpContext.Current.Server.MapPath(objOrder.InvoiceFilePath));


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

        /// <summary>
        /// Send an email to client regarding the Unique URL change
        /// </summary>
        /// <param name="stratasBoardId"></param>
        /// <returns></returns>
        public static string FncSend_StratasBoard_UniqueURLUpdate_ToClient(long stratasBoardId)
        {
            try
            {

                using (var db = new StratasFairDBEntities())
                {
                    var stratasDetails = db.vw_GetStratasBoard.Where(m => m.StratasBoardId == stratasBoardId).SingleOrDefault();
                    if (stratasDetails != null)
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = "";
                        string subject = "";
                        getConfigValue("stratasboard-updateurl-mail", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMail(stratasDetails.EmailId, subject);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{PASSWORD}", AppLogic.DecryptPassword(stratasDetails.Password));
                        mailBody.Replace("{BASEPATH}/", url);
                        mailBody.Replace("{NAME}", stratasDetails.FirstName + " " + stratasDetails.LastName);
                        mailBody.Replace("{STRATANAME}", stratasDetails.StratasBoardName);
                        mailBody.Replace("{PORTALURL}", url + stratasDetails.PortalLink + "/login");
                        mailBody.Replace("{EMAILADDRESS}", stratasDetails.EmailId);
                        emailSender.Body = mailBody.ToString();

                        //for attachment

                        //emailSender.AttachmentList = new ArrayList(1);
                        //emailSender.AttachmentList.Add(System.Web.HttpContext.Current.Server.MapPath(objOrder.InvoiceFilePath));


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

        /// <summary>
        /// Send an email to StratasFair Admin to acknowledge regarding the Ubnique Url request by StrataBoard Admin
        /// </summary>
        /// <param name="stratasBoardId"></param>
        /// <returns></returns>
        public static string FncSend_StratasBoard_RequestUniqueURL_ToStratasFairAdmin(string ToEmail)
        {
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    var stratasDetails = db.vw_GetStratasBoard.Where(m => m.StratasBoardId == ClientSessionData.ClientStrataBoardId).SingleOrDefault();
                    if (stratasDetails != null)
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = "";
                        string subject = "";
                        getConfigValue("stratasboard-uniqueurl-request-mail", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMail(ToEmail, subject);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{BASEPATH}/", url);
                        mailBody.Replace("{NAME}", stratasDetails.FirstName + " " + stratasDetails.LastName);
                        mailBody.Replace("{STRATANAME}", stratasDetails.StratasBoardName);
                        mailBody.Replace("{PORTALURL}", url + stratasDetails.PortalLink + "/login");
                        mailBody.Replace("{REQUESTEDPORTALURL}", url + stratasDetails.RequestPortalLink + "/login");
                        mailBody.Replace("{EMAILADDRESS}", stratasDetails.EmailId);
                        emailSender.Body = mailBody.ToString();

                        //for attachment

                        //emailSender.AttachmentList = new ArrayList(1);
                        //emailSender.AttachmentList.Add(System.Web.HttpContext.Current.Server.MapPath(objOrder.InvoiceFilePath));


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

        

        public static string FncSend_StratasBoard_RegistrationMail_ToSubAdminClient(int UserClientId)
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
                        string subject = "";
                        getConfigValue("strataboard_subadmin_registration", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMail(userClients.EmailId, subject);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{PASSWORD}", AppLogic.DecryptPassword(userClients.Password));
                        mailBody.Replace("{BASEPATH}/", url);
                        mailBody.Replace("{NAME}", userClients.FirstName + " " + userClients.LastName);
                        mailBody.Replace("{PORTALURL}", url + ClientSessionData.ClientPortalLink + "/login");
                        mailBody.Replace("{EMAILADDRESS}", userClients.EmailId);
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

        public static string FncSend_StratasBoard_RegistrationMail_ToStrataOwnerClient(int UserClientId)
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
                        string subject = "";
                        getConfigValue("strataboard_owner_registration", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMail(userClients.EmailId, subject);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{PASSWORD}", AppLogic.DecryptPassword(userClients.Password));
                        mailBody.Replace("{BASEPATH}/", url);
                        mailBody.Replace("{NAME}", userClients.FirstName + " " + userClients.LastName);
                        mailBody.Replace("{PORTALURL}", url + ClientSessionData.ClientPortalLink + "/login");
                        mailBody.Replace("{EMAILADDRESS}", userClients.EmailId);
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

        public static string FncSend_StratasBoard_RegistrationMail_ToVendor(string VendorName, string VendorEmail)
        {
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                        string body = "";
                        string subject = "";
                        getConfigValue("vendor-registration-success", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMail(VendorEmail, subject);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{VENDORNAME}", VendorName);
                        mailBody.Replace("{BASEPATH}", ConfigurationManager.AppSettings["WebsiteRootPath"].ToString());
                        emailSender.Body = mailBody.ToString();
                        emailSender = FncSendMail(emailSender);
                        return emailSender.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static string FncSend_StratasBoard_ForgotPassword_ToClient(long stratasBoardId)
        {
            try
            {

                using (var db = new StratasFairDBEntities())
                {
                    var stratasDetails = db.vw_GetStratasBoard.Where(m => m.StratasBoardId == stratasBoardId).SingleOrDefault();
                    if (stratasDetails != null)
                    {
                        Encrypt64 encrypt = new Encrypt64();
                        string EncryptedUserClientId = encrypt.Encrypt(stratasDetails.UserClientId.ToString(), ConfigurationManager.AppSettings["SecureKey"].ToString());
                        EncryptedUserClientId = EncryptedUserClientId.Replace("+", "$");
                        EncryptedUserClientId = EncryptedUserClientId.Replace("/", "!");
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = string.Empty;
                        string subject = string.Empty;
                        getConfigValue("forget-password-client", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMail(stratasDetails.EmailId, subject);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);

                        mailBody.Replace("{BASEPATH}/", url);
                        mailBody.Replace("{URL}", url + stratasDetails.PortalLink + "/logon/ResetPassword/" + EncryptedUserClientId);
                        mailBody.Replace("{NAME}", stratasDetails.FirstName + " " + stratasDetails.LastName);
                        mailBody.Replace("{STRATANAME}", stratasDetails.StratasBoardName);
                        mailBody.Replace("{PORTALURL}", stratasDetails.PortalLink);
                        mailBody.Replace("{EMAILADDRESS}", stratasDetails.EmailId);
                        emailSender.Body = mailBody.ToString();

                        //for attachment

                        //emailSender.AttachmentList = new ArrayList(1);
                        //emailSender.AttachmentList.Add(System.Web.HttpContext.Current.Server.MapPath(objOrder.InvoiceFilePath));


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

        public static string FncSend_ContactUsMail_ToAdmin(string UserName, string Email, string Message)
        {
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    string body = "";
                    string subject = "";
                    string adminEmail = db.tblUsers.Where(x => x.RoleId == 1).FirstOrDefault().EmailId;
                    getConfigValue("contactus", out body, out subject);
                    EmailSender emailSender = GetCommonDataForSendMail(adminEmail, subject);                    
                    StringBuilder mailBody = new StringBuilder();
                    mailBody.Append(body);
                    mailBody.Replace("{BASEPATH}", ConfigurationManager.AppSettings["WebsiteRootPath"].ToString());
                    mailBody.Replace("{NAME}", UserName);
                    mailBody.Replace("{EMAIL}", Email);
                    mailBody.Replace("{MESSAGE}", Message);
                    mailBody.Replace("{YEAR}", DateTime.Now.Year.ToString());
                    emailSender.Body = mailBody.ToString();

                    emailSender = FncSendMail(emailSender);
                    return emailSender.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static int FncSend_QuotRequestMail_To_Vendor(string vendorName, string vendorEmail,string ownerName, string Details)
        {
            try
            {

                string body = "";
                string subject = "";
                getConfigValue("quotation-request-fromOwner", out body, out subject);

                Encrypt64 encryptDecrypt = new Encrypt64();
                EmailSender objClsMail = GetCommonDataForSendMail(vendorEmail, subject);

                StringBuilder mailBody = new StringBuilder();
                mailBody.Append(body);
                mailBody.Replace("{NAME}", vendorName);
                mailBody.Replace("{REQUESTER}", ownerName);
                mailBody.Replace("{DETAILS}", Details);
                mailBody.Replace("{BASEPATH}", ConfigurationManager.AppSettings["WebsiteRootPath"].ToString());
                objClsMail.Body = mailBody.ToString();
                FncSendMail(objClsMail);
                return 1;
            }
            catch
            {
                return -1;
            }
        }


        public static string FncSendReminderMailToClient(string emailAddress, string AdminEmailIds, long stratasBoardId, string message)
        {
            try
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
                StrataBoardModel strataBoardModel = new StrataBoardModel();
                strataBoardModel = strataBoardHelper.GetStratasBoardByID(stratasBoardId);


                //string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                //EmailSender objClsMail = GetCommonDataForSendMail(emailAddress, "Reminder from StratasFair");
                //StringBuilder mailBody = new StringBuilder();
                //mailBody.Append(getConfigValue("stratasboard-reminder-mail").Replace("{NAME}", strataBoardModel.StratasBoardName));
                //mailBody.Replace("{BASEPATH}", url);
                //mailBody.Replace("{MESSAGE}", message);
                //objClsMail.Body = mailBody.ToString();
                //objClsMail.Bcc = AdminEmailIds;

                //for attachment

                //objClsMail.AttachmentList = new ArrayList(1);
                //objClsMail.AttachmentList.Add(System.Web.HttpContext.Current.Server.MapPath(objOrder.InvoiceFilePath));

                //objClsMail = FncSendMail(objClsMail);
                //return objClsMail.ErrorMessage;
                return "success";

            }
            catch
            {
                return "";
            }
        }


        //public static string FncSendReminderMailToClient(string emailAddress, string AdminEmailIds, long companyId, string message)
        //{
        //    try
        //    {
        //        ClientHelper helper = new ClientHelper();
        //        ClientModel model = new ClientModel();
        //        model = helper.GetClientByID(companyId);


        //        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
        //        EmailSender objClsMail = GetCommonDataForSendMail(emailAddress, "Reminder from Communication Coach");
        //        StringBuilder mailBody = new StringBuilder();
        //        mailBody.Append(getConfigValue("company-reminder-mail").Replace("{NAME}", model.CompanyName));
        //        mailBody.Replace("{BASEPATH}", url);
        //        mailBody.Replace("{MESSAGE}", message);
        //        objClsMail.Body = mailBody.ToString();
        //        objClsMail.Bcc = AdminEmailIds;

        //        //for attachment

        //        //objClsMail.AttachmentList = new ArrayList(1);
        //        //objClsMail.AttachmentList.Add(System.Web.HttpContext.Current.Server.MapPath(objOrder.InvoiceFilePath));

        //        objClsMail = FncSendMail(objClsMail);
        //        return objClsMail.ErrorMessage;

        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}



        //public static string FncSendLicenseGenerateMailToClient(string emailAddress, string AdminEmailIds, string userName, string password, string name, string companyName)
        //{
        //    try
        //    {


        //        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
        //        EmailSender objClsMail = GetCommonDataForSendMail(emailAddress, "License generated for Communication Coach");
        //        StringBuilder mailBody = new StringBuilder();
        //        mailBody.Append(getConfigValue("company-admin-registration-mail").Replace("{NAME}", name));
        //        mailBody.Replace("{BASEPATH}", url);
        //        mailBody.Replace("{USERNAME}", userName);
        //        mailBody.Replace("{PASSWORD}", password);
        //        mailBody.Replace("{COMPANYNAME}", companyName);
        //        objClsMail.Body = mailBody.ToString();
        //        objClsMail.Bcc = AdminEmailIds;

        //        //for attachment

        //        //objClsMail.AttachmentList = new ArrayList(1);
        //        //objClsMail.AttachmentList.Add(System.Web.HttpContext.Current.Server.MapPath(objOrder.InvoiceFilePath));

        //        objClsMail = FncSendMail(objClsMail);
        //        return objClsMail.ErrorMessage;

        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        public static string FncSend_StratasBoard_OwnerRequestMail_ToStrataAdmin(int UserClientId, long StrataBoardId)
        {
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    var userClients = db.tblUserClients.Where(m => m.UserClientId == UserClientId).SingleOrDefault();
                    var stratasDetails = db.vw_GetStratasBoard.Where(m => m.StratasBoardId == ClientSessionData.ClientStrataBoardId).SingleOrDefault();
                    if (userClients != null)
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = "";
                        string subject = "";
                        string FromDate = string.Empty , ToDate = string.Empty, Details = string.Empty, RequestTitle = string.Empty;
                        getConfigValue("stratasboard_owner_request", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMailClient(stratasDetails.EmailId, subject, StrataBoardId);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        var userRequests = db.tblUserRequests.Where(x => x.UserClientId == ClientSessionData.UserClientId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                        if(userRequests != null)
                        {
                            RequestTitle = userRequests.RequestTitle;
                            Details = userRequests.Details;
                            FromDate = userRequests.FromDate != null ? userRequests.FromDate.Value.ToString("dd MMM, yyyy") : "N/A";
                            ToDate = userRequests.ToDate != null ? userRequests.ToDate.Value.ToString("dd MMM, yyyy") : "N/A";
                            
                        }
                        mailBody.Replace("{BASEPATH}/", url);
                        mailBody.Replace("{STRATAOWNER}", userClients.FirstName + " " + userClients.LastName);
                        mailBody.Replace("{EMAILADDRESS}", userClients.EmailId);
                        mailBody.Replace("{REQUESTTITLE}", RequestTitle);
                        if(FromDate == "N/A" || ToDate == "N/A")
                        {
                            mailBody.Replace("{DATES}", "N/A");
                        }
                        else
                        {
                            mailBody.Replace("{DATES}", FromDate + " to " + ToDate);
                        }                        
                        mailBody.Replace("{DETAILS}", Details);
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

        public static string FncSendScheduleMeetingInvite_ToStrataOwner(long? UserClientId, string MeetingDate, string Subject, string Message, long StrataBoardId)
        {
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    var userClients = db.tblUserClients.Where(m => m.UserClientId == UserClientId).SingleOrDefault();
                    if (userClients != null)
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = string.Empty;
                        string subject = string.Empty;
                        getConfigValue("stratasboard_schedule_meeting_invite", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMailClient(userClients.EmailId, subject, StrataBoardId);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{BASEPATH}", url);
                        mailBody.Replace("{NAME}", userClients.FirstName + " " + userClients.LastName);
                        mailBody.Replace("{SUBJECT}", Subject);
                        mailBody.Replace("{MESSAGE}", Message);
                        mailBody.Replace("{DATE}", MeetingDate);
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

        public static string FncSendAlertAndNotification_ToStrataOwner(long? UserClientId, string Type, string Title, string Message)
        {
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    var userClients = db.tblUserClients.Where(m => m.UserClientId == UserClientId).SingleOrDefault();
                    if (userClients != null)
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = string.Empty;
                        string subject = string.Empty;
                        getConfigValue("stratasboard_latest_alert", out body, out subject);
                        EmailSender emailSender = GetCommonDataForSendMailClient(userClients.EmailId, subject, ClientSessionData.ClientStrataBoardId);
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append(body);
                        mailBody.Replace("{BASEPATH}", url);
                        mailBody.Replace("{NAME}", userClients.FirstName + " " + userClients.LastName);
                        mailBody.Replace("{TYPE}", Type);
                        mailBody.Replace("{TITLE}", Title);
                        mailBody.Replace("{MESSAGE}", Message);
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


        /// <summary>
        /// Function is called in Booking Request is Approved/Rejected by Stratasboard Admin
        /// Function will be used in both the cases : Manage Other Request, Manage Common Area Booking Request
        /// </summary>
        /// <param name="bookingRequestId"></param>
        /// <param name="stratasBoardId"></param>
        /// <returns>Message if "success" then mail is delievered other follow the reason..</returns>
        public static string BookingRequestUpdateMail(long bookingRequestId, long stratasBoardId)
        {
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    var bookingRequest = db.vw_GetBookingList.Where(m => m.BookingRequestId == bookingRequestId).SingleOrDefault();
                    if (bookingRequest != null)
                    {
                        string url = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();
                        string body = "";
                        string subject = "";
                        string FromDate = string.Empty, ToDate = string.Empty;
                        if (bookingRequest.AdminStatus==1)
                            getConfigValue("booking-request-approved", out body, out subject);
                        else if (bookingRequest.AdminStatus == 2)
                            getConfigValue("booking-request-rejected", out body, out subject);

                        if (bookingRequest.RequesterEmailNotification)
                        {
                            EmailSender emailSender = GetCommonDataForSendMailClient(bookingRequest.RequesterEmailId, subject, stratasBoardId);
                            StringBuilder mailBody = new StringBuilder();
                            mailBody.Append(body);


                            FromDate = bookingRequest.FromDate != null ? (bookingRequest.FromDate.Value.Year == 1900 ? "N/A" : bookingRequest.FromDate.Value.ToString("dd MMM, yyyy hh:mm tt")) : "N/A";
                            ToDate = bookingRequest.ToDate != null ? (bookingRequest.ToDate.Value.Year == 1900 ? "N/A" : bookingRequest.ToDate.Value.ToString("dd MMM, yyyy hh:mm tt")) : "N/A";

                          
                            mailBody.Replace("{BASEPATH}/", url);
                            mailBody.Replace("{NAME}", bookingRequest.RequestedBy);
                            mailBody.Replace("{COMMONAREANAME}", bookingRequest.CommonAreaName);
                            mailBody.Replace("{SUBJECT}", bookingRequest.Subject);
                            mailBody.Replace("{REASON}", bookingRequest.Reason);
                            mailBody.Replace("{REQUESTDATE}", bookingRequest.CreatedOn.Value.ToString("dd MMM, yyyy"));

                            if (FromDate == "N/A" && ToDate == "N/A")
                                mailBody.Replace("{DATES}", "N/A");
                            else if (FromDate == "N/A" )
                                mailBody.Replace("{DATES}", ToDate);
                            else if (ToDate == "N/A")
                                mailBody.Replace("{DATES}", FromDate);
                            else
                                mailBody.Replace("{DATES}", FromDate + " to " + ToDate);
                            mailBody.Replace("{REJECTREASON}", bookingRequest.AdminRemark);
                           
                            emailSender.Body = mailBody.ToString();
                            emailSender = FncSendMail(emailSender);
                            return emailSender.ErrorMessage;
                        }
                        else
                        {
                            return "Email notification is disabled for user.";
                        }
                    }
                    else
                    {
                        return "Booking request error.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


    }
}
