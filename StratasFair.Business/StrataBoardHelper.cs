using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business
{
    public class StrataBoardHelper
    {
        StratasFairDBEntities context;
        public StrataBoardHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }

        public List<StrataBoardModel> GetAll(StrataBoardModel model)
        {
            var myQuery = context.vw_GetStratasBoard.ToList();
            if (model.ExpiredOnFrom.Year > 1900 && model.ExpiredOnTo.Year > 1900)
            {
                myQuery = myQuery.Where(x => (x.ExpiryDate >= model.ExpiredOnFrom && x.ExpiryDate <= model.ExpiredOnTo)).ToList();
            }
            else if (model.ExpiredOnFrom.Year > 1900)
            {
                myQuery = myQuery.Where(x => x.ExpiryDate >= model.ExpiredOnFrom).ToList();
            }
            else if (model.ExpiredOnTo.Year > 1900)
            {
                myQuery = myQuery.Where(x => x.ExpiryDate <= model.ExpiredOnTo).ToList();
            }

            if (model.Keyword != null && model.Keyword != "")
            {
                myQuery = myQuery.Where(x => x.StratasBoardName.Contains(model.Keyword)
                    //|| x.FirstName.Contains(model.Keyword)
                    //|| x.LastName.Contains(model.Keyword)
                    || x.PortalLink.Contains(model.Keyword)
                    || x.EmailId.Contains(model.Keyword)
                    ).ToList();
            }
            return myQuery
                //.Where(x => x.LicenseType == (model.LicenseType > 0 ? model.LicenseType : x.LicenseType))
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new StrataBoardModel()
                {
                    StratasBoardId = x.StratasBoardId,
                    StratasBoardName = x.StratasBoardName,
                    PortalLink = x.PortalLink,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EmailId = x.EmailId,
                    ContactNumber = x.ContactNumber,
                    CreatedOn = x.CreatedOn.Value,
                    SubscriptionId = x.SubscriptionId.Value,
                    ExpiryDate = x.ExpiryDate.ToString(),
                    AllowedUser = x.AllowedUser.Value,
                    Status = x.STATUS.Value,
                    SubscriptionName = x.SubscriptionName,
                    ValidityText = x.ValidityText,
                    IsSmsForAlert = x.IsSmsForAlert.Value,
                    IsSmsForReminder = x.IsSmsForReminder.Value,
                    RequestPortalLink = x.RequestPortalLink,
                    RequestMessage = x.RequestMessage,
                    RequestCreatedOn = x.RequestCreatedOn.ToString(),
                    RequestCounter = x.RequestCounter,
                    RequestCreatedBy = x.RequestCreatedBy

                }).ToList();
        }


        public StrataBoardModel GetStratasBoardByID(long? Id)
        {
            return context.vw_GetStratasBoard.Where(x => x.StratasBoardId == Id).Select(x => new StrataBoardModel()
            {
                StratasBoardId = x.StratasBoardId,
                UserClientId = x.UserClientId.Value,
                StratasBoardName = x.StratasBoardName,
                PortalLink = x.PortalLink,
                OldPortalLink = x.PortalLink,
                RequestPortalLink = x.RequestPortalLink,
                RequestMessage = x.RequestMessage,
                RequestCounter = x.RequestCounter,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailId = x.EmailId,
                ContactNumber = x.ContactNumber,
                CreatedOn = x.CreatedOn.Value,
                SubscriptionId = x.SubscriptionId.Value,
                ExpiryDate = x.ExpiryDate.ToString(),
                AllowedUser = x.AllowedUser.Value,
                Status = x.STATUS.Value,
                SubscriptionName = x.SubscriptionName,
                ValidityText = x.ValidityText,
                IsSmsForAlert = x.IsSmsForAlert.Value,
                IsSmsForReminder = x.IsSmsForReminder.Value,
                BuildingName = x.BuildingName
            }).FirstOrDefault();
        }


        public long AddUpdate(StrataBoardModel objectModel)
        {
            if (objectModel.StratasBoardId > 0)
            {
                if (objectModel.PortalLink != objectModel.OldPortalLink
                    && context.tblUserClients.Where(x => x.StratasBoardId == objectModel.StratasBoardId && x.RoleName.ToLower() != "a").Count() > 0)
                {
                    // unique name can't be updated when a sub-admin or Owner has already assigned to this StratasBoardId.
                    return -7;
                }

                else if (context.tblStratasBoards.Any(x => x.StratasBoardName == objectModel.StratasBoardName && x.StratasBoardId != objectModel.StratasBoardId))
                {
                    // stratasboard name already exists
                    return -4;
                }
                else if (context.tblStratasBoards.Any(x => x.PortalLink == objectModel.PortalLink && x.StratasBoardId != objectModel.StratasBoardId))
                {
                    // portal link already exists
                    return -5;
                }
                else if (context.tblUserClients.Any(x => x.EmailId == objectModel.EmailId && x.StratasBoardId != objectModel.StratasBoardId))
                {
                    // registered emailid already exists
                    return -6;
                }
                else
                {
                    return update(objectModel);
                }
            
               
            }
            else
            {
                if (context.tblStratasBoards.Any(x => x.StratasBoardName == objectModel.StratasBoardName))
                {
                    // stratasboard name already exists
                    return -4;
                }
                else if (context.tblStratasBoards.Any(x => x.PortalLink == objectModel.PortalLink))
                {
                    // portal link already exists
                    return -5;
                }
                else if (context.tblUserClients.Any(x => x.EmailId == objectModel.EmailId))
                {
                    // registered emailid already exists
                    return -6;
                }
                else
                {
                    return update(objectModel);
                }
            }
        }


        public long update(StrataBoardModel objectModel)
        {
            long count = -2;
            long stratasBoardId = 0;
            long userClientId = 0;
            if (objectModel.StratasBoardId > 0)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Table StratasBoard 
                        tblStratasBoard tblStratasBoardDb = new tblStratasBoard();
                        tblStratasBoardDb.StratasBoardId = objectModel.StratasBoardId;
                        tblStratasBoardDb.StratasBoardName = objectModel.StratasBoardName;
                        tblStratasBoardDb.BuildingName = objectModel.BuildingName;
                        tblStratasBoardDb.PortalLink = objectModel.PortalLink.Trim();
                        tblStratasBoardDb.Status = objectModel.Status;
                        tblStratasBoardDb.ModifiedBy = AdminSessionData.AdminUserId;
                        tblStratasBoardDb.ModifiedOn = DateTime.UtcNow;
                        tblStratasBoardDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                        if (objectModel.PortalLink == objectModel.RequestPortalLink)
                        {
                            tblStratasBoardDb.RequestCounter = 1;
                        }

                        context.tblStratasBoards.Attach(tblStratasBoardDb);
                        context.Entry(tblStratasBoardDb).Property(x => x.StratasBoardName).IsModified = true;
                        context.Entry(tblStratasBoardDb).Property(x => x.BuildingName).IsModified = true;
                        context.Entry(tblStratasBoardDb).Property(x => x.PortalLink).IsModified = true;
                        context.Entry(tblStratasBoardDb).Property(x => x.Status).IsModified = true;
                        context.Entry(tblStratasBoardDb).Property(x => x.ModifiedBy).IsModified = true;
                        context.Entry(tblStratasBoardDb).Property(x => x.ModifiedOn).IsModified = true;
                        context.Entry(tblStratasBoardDb).Property(x => x.ModifiedFromIP).IsModified = true;
                        context.Entry(tblStratasBoardDb).Property(x => x.RequestCounter).IsModified = true;

                        // Table UserClient
                        count = context.SaveChanges();
                        if (count >= 0)
                        {
                            tblUserClient tblUserClientDb = new tblUserClient();
                            tblUserClientDb.UserClientId = objectModel.UserClientId;
                            tblUserClientDb.FirstName = objectModel.FirstName;
                            tblUserClientDb.LastName = objectModel.LastName;
                            tblUserClientDb.EmailId = objectModel.EmailId;
                            tblUserClientDb.ContactNumber = objectModel.ContactNumber;
                            tblUserClientDb.ModifiedBy = AdminSessionData.AdminUserId;
                            tblUserClientDb.ModifiedOn = DateTime.UtcNow;
                            tblUserClientDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;

                            context.tblUserClients.Attach(tblUserClientDb);
                            context.Entry(tblUserClientDb).Property(x => x.FirstName).IsModified = true;
                            context.Entry(tblUserClientDb).Property(x => x.LastName).IsModified = true;
                            //context.Entry(tblUserClientDb).Property(x => x.EmailId).IsModified = true;
                            context.Entry(tblUserClientDb).Property(x => x.ContactNumber).IsModified = true;
                            context.Entry(tblUserClientDb).Property(x => x.ModifiedBy).IsModified = true;
                            context.Entry(tblUserClientDb).Property(x => x.ModifiedOn).IsModified = true;
                            context.Entry(tblUserClientDb).Property(x => x.ModifiedFromIP).IsModified = true;
                            count = context.SaveChanges();
                            if (count >= 0)
                            {
                                transaction.Commit();
                                count = objectModel.StratasBoardId; 
                            }
                            else
                            {
                                transaction.Rollback();
                                count = -2;   // any error is there
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            count = -2;   // any error is there
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        new AppError().LogMe(ex);
                        transaction.Rollback();
                        count = -2;   // any error is there
                    }
                }
            }
            else
            {
                using (var transaction = context.Database.BeginTransaction())
                {

                    var subscriptionDetails = context.tblSubscriptions.Where(x => x.SubscriptionId == objectModel.SubscriptionId).FirstOrDefault();
                    try
                    {
                        // Table StratasBoard
                        tblStratasBoard tblStratasBoardDb = new tblStratasBoard();
                        tblStratasBoardDb.StratasBoardName = objectModel.StratasBoardName;
                        tblStratasBoardDb.BuildingName = objectModel.BuildingName;
                        tblStratasBoardDb.PortalLink = objectModel.PortalLink;
                        tblStratasBoardDb.CreatedBy = AdminSessionData.AdminUserId;
                        tblStratasBoardDb.CreatedOn = DateTime.UtcNow;
                        tblStratasBoardDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                        tblStratasBoardDb.Status = objectModel.Status;
                        context.tblStratasBoards.Add(tblStratasBoardDb);
                        context.SaveChanges();
                        stratasBoardId = tblStratasBoardDb.StratasBoardId;
                        if (stratasBoardId > 0)
                        {
                            // Table UserClient
                            tblUserClient tblUserClientDb = new tblUserClient();
                            tblUserClientDb.FirstName = objectModel.FirstName;
                            tblUserClientDb.LastName = objectModel.LastName;
                            tblUserClientDb.EmailId = objectModel.EmailId;
                            tblUserClientDb.Password = AppLogic.EncryptPassword();
                            tblUserClientDb.ContactNumber = objectModel.ContactNumber;
                            tblUserClientDb.StratasBoardId = stratasBoardId;
                            tblUserClientDb.RoleName = "A";  // StratasBoard Admin
                            tblUserClientDb.IsEmailNotification = true;
                            tblUserClientDb.IsSMSNotification = true;
                            tblUserClientDb.IsProfileComplete = false;
                            tblUserClientDb.Status = 1;  // It will be active in both the cases
                            tblUserClientDb.CreatedBy = AdminSessionData.AdminUserId;
                            tblUserClientDb.CreatedOn = DateTime.UtcNow;
                            tblUserClientDb.LastLogin = DateTime.UtcNow;
                            tblUserClientDb.CurrentLogin = DateTime.UtcNow;
                            tblUserClientDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                            context.tblUserClients.Add(tblUserClientDb);
                            context.SaveChanges();
                            userClientId = tblUserClientDb.UserClientId;
                            if (userClientId > 0)
                            {
                                // Table StratasBoard Subscription
                                tblStratasBoardSubscription tblStratasBoardSubscriptionDb = new tblStratasBoardSubscription();
                                tblStratasBoardSubscriptionDb.StratasBoardId = stratasBoardId;
                                tblStratasBoardSubscriptionDb.SubscriptionId = objectModel.SubscriptionId;
                                tblStratasBoardSubscriptionDb.Validity = subscriptionDetails.Validity;
                                tblStratasBoardSubscriptionDb.IsSmsForAlert = subscriptionDetails.IsSmsForAlert;
                                tblStratasBoardSubscriptionDb.IsSmsForReminder = subscriptionDetails.IsSmsForReminder;
                                tblStratasBoardSubscriptionDb.AllowedUser = subscriptionDetails.AllowedUser;
                                tblStratasBoardSubscriptionDb.ExpiryDate = AppLogic.CalculateSubscriptionExpiryDate(DateTime.UtcNow, subscriptionDetails.Validity);
                                tblStratasBoardSubscriptionDb.CreatedBy = AdminSessionData.AdminUserId;
                                tblStratasBoardSubscriptionDb.CreatedOn = DateTime.UtcNow;
                                tblStratasBoardSubscriptionDb.Status = 1;
                                context.tblStratasBoardSubscriptions.Add(tblStratasBoardSubscriptionDb);
                                context.SaveChanges();
                                int stratasBoardSubscriptionId = tblStratasBoardSubscriptionDb.StratasBoardSubscriptionId;
                                if (stratasBoardSubscriptionId > 0)
                                {
                                    transaction.Commit();
                                    count = stratasBoardId;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    count = -2;   // any error is there
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                                count = -2;   // any error is there
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            count = -2;   // any error is there
                        }

                    }
                    catch (Exception ex)
                    {
                        new AppError().LogMe(ex);
                        transaction.Rollback();
                        count = -2;   // any error is there
                    }
                }

            }
            return count;
        }


        public int ActDeact(StrataBoardModel objectModel)
        {
            tblStratasBoard tblStratasBoardDb = new tblStratasBoard();
            tblStratasBoardDb.StratasBoardId = objectModel.StratasBoardId;
            tblStratasBoardDb.Status = objectModel.Status;

            context.tblStratasBoards.Attach(tblStratasBoardDb);
            context.Entry(tblStratasBoardDb).Property(x => x.Status).IsModified = true;

            int count = context.SaveChanges();
            if (count == 1)
                count = 0;
            else
                count = -1;

            return count;
        }

        public int Delete(StrataBoardModel objectModel)
        {
            int count = -1;

            // check the user exists on this strata, if exists then return...
            if (context.tblUserClients.Any(o => o.StratasBoardId == objectModel.StratasBoardId && o.RoleName.ToLower() != "a"))
            {
                count = -2;
                return count;
            }

            // Delete the users on this strata
            if (context.tblUserClients.Any(x => x.StratasBoardId == objectModel.StratasBoardId))
            {
                context.tblUserClients.RemoveRange(context.tblUserClients.Where(x => x.StratasBoardId == objectModel.StratasBoardId));
                context.SaveChanges();
                count = 0;
            }

            // Delete the subscription details on this strata
            if (context.tblStratasBoardSubscriptions.Any(x => x.StratasBoardId == objectModel.StratasBoardId))
            {
                context.tblStratasBoardSubscriptions.RemoveRange(context.tblStratasBoardSubscriptions.Where(x => x.StratasBoardId == objectModel.StratasBoardId));
                context.SaveChanges();
                count = 0;
            }


            tblStratasBoard tblStratasBoardDb = new tblStratasBoard();
            tblStratasBoardDb.StratasBoardId = objectModel.StratasBoardId;
            context.Entry(tblStratasBoardDb).State = EntityState.Deleted;
            count = context.SaveChanges();
            if (count == 1)
                count = 0;
            else
                count = -1;
            return count;
        }


        public List<SubscriptionModel> GetStrataBoardSubscription(long stratasBoardId)
        {
            return context.vw_GetStratasBoardSubscription
                   .Where(x => x.StratasBoardId == stratasBoardId)
                   .Select(x => new SubscriptionModel() { SubscriptionId = x.SubscriptionId.Value, SubscriptionName = x.SubscriptionName, ExpiryDate = x.ExpiryDate.Value, AllowedUser = x.AllowedUser.Value, ValidityText=x.ValidityText, IsSmsForAlertText=x.SmsForAlertText,IsSmsForReminderText=x.SmsForReminderText, Status = x.STATUS.Value, CreatedOn = x.CreatedOn.Value, StratasBoardId = x.StratasBoardId.Value }
                       ).ToList();
        }


        public int RenewSubscription(RenewSubscriptionModel objectModel)
        {
            int count = -1;
            int totalStrataUser = CommonData.GetStratasBoardTotalUser(objectModel.StratasBoardId);
            var subscriptionDetails = context.tblSubscriptions.Where(x => x.SubscriptionId == objectModel.SubscriptionId).FirstOrDefault();
            if (totalStrataUser > subscriptionDetails.AllowedUser)
            {
                // Strata User are more than subscription package allowed user.
                count = -1;
                return count;
            }
            try
            {

                // Table subscription update for previous added subscription 

                context.tblStratasBoardSubscriptions
                    .Where(x => x.StratasBoardId == objectModel.StratasBoardId)
                    .ToList()
                    .ForEach(a => a.Status = 0);

                count = context.SaveChanges();
                if (count >= 0)
                {
                    // Table StratasBoard Subscription
                    tblStratasBoardSubscription tblStratasBoardSubscriptionDb = new tblStratasBoardSubscription();
                    tblStratasBoardSubscriptionDb.StratasBoardId = objectModel.StratasBoardId;
                    tblStratasBoardSubscriptionDb.SubscriptionId = objectModel.SubscriptionId;
                    tblStratasBoardSubscriptionDb.Validity = subscriptionDetails.Validity;
                    tblStratasBoardSubscriptionDb.IsSmsForAlert = subscriptionDetails.IsSmsForAlert;
                    tblStratasBoardSubscriptionDb.IsSmsForReminder = subscriptionDetails.IsSmsForReminder;
                    tblStratasBoardSubscriptionDb.AllowedUser = subscriptionDetails.AllowedUser;
                    tblStratasBoardSubscriptionDb.ExpiryDate = AppLogic.CalculateSubscriptionExpiryDate(DateTime.UtcNow, subscriptionDetails.Validity);
                    tblStratasBoardSubscriptionDb.CreatedBy = AdminSessionData.AdminUserId;
                    tblStratasBoardSubscriptionDb.CreatedOn = DateTime.UtcNow;
                    tblStratasBoardSubscriptionDb.Status = 1;
                    context.tblStratasBoardSubscriptions.Add(tblStratasBoardSubscriptionDb);
                    context.SaveChanges();
                    if (tblStratasBoardSubscriptionDb.StratasBoardSubscriptionId > 0)
                    {
                        count = tblStratasBoardSubscriptionDb.StratasBoardSubscriptionId;
                    }
                    else
                    {
                        count = -2;   // any error is there
                    }
                }
                   
            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                count = -2;   // any error is there
            }
            
            return count;
        }



        public List<StrataBoardModel> GetStrataBoardToBeExpired()
        {
            DateTime currDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return context.vw_GetStratasBoard
                .Where(x => x.STATUS == 1 
                    && x.ExpiryDate.Value.Year == DateTime.Now.Year 
                    && x.ExpiryDate.Value.Month == DateTime.Now.Month
                    && x.ExpiryDate > System.DateTime.UtcNow )
                .OrderBy(x => x.CreatedOn)
                .Select(x => new StrataBoardModel()
                {
                    StratasBoardName = x.StratasBoardName,
                    SubscriptionName = x.SubscriptionName,
                    EmailId = x.EmailId,
                    ExpiryDate=x.ExpiryDate.ToString()
                }).ToList();
        }

        public RequestPortalLinkModel GetStratasBoardPortalLinkDetails(int Id)
        {
            var StratasBoardPortalLinkDetails =  context.vw_GetStratasBoard.Where(x => x.StratasBoardId == Id && x.STATUS == 1).Select(x => new RequestPortalLinkModel()
            {
                PortalLink = x.PortalLink,
                RequestPortalLink = x.RequestPortalLink,
                RequestMessage = x.RequestMessage,
                RequestCounter = x.RequestCounter,
                TotalUser = x.TotalUser.Value
            }).FirstOrDefault();
            return StratasBoardPortalLinkDetails;
        }

        public int UpdateURLRequestPortalLink(RequestPortalLinkModel requestPortalLinkModel)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int result = 0;
                if (context.tblUserClients.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.RoleName.ToLower() != "a").Count() > 0)
                {
                    return -1;
                }
                else
                {
                    if (requestPortalLinkModel.RequestCounter < 1)
                    {
                        var strataBoard = context.tblStratasBoards.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1).FirstOrDefault();
                        strataBoard.PortalLink = requestPortalLinkModel.PortalLink.Trim();
                        strataBoard.RequestPortalLink = requestPortalLinkModel.RequestPortalLink.Trim();
                        strataBoard.RequestMessage = requestPortalLinkModel.RequestMessage;
                        strataBoard.RequestCreatedBy = ClientSessionData.UserClientId;
                        strataBoard.RequestCreatedOn = DateTime.UtcNow;
                        context.Entry(strataBoard).State = System.Data.Entity.EntityState.Modified;
                        result = context.SaveChanges();
                        if (result > 0)
                        {
                            string MailResponse = string.Empty;
                            var AdminEmailNotificationSettings = context.tblAdminEmailNotificationSettings.Where(x => x.Status == 1).FirstOrDefault();
                            if (AdminEmailNotificationSettings != null)
                            {
                                if (AdminEmailNotificationSettings.EmailAddress.Count() > 0)
                                {
                                    string[] arrEmails = AdminEmailNotificationSettings.EmailAddress.Split(',').ToArray();
                                    foreach (var item in arrEmails)
                                    {
                                        MailResponse = EmailSender.FncSend_StratasBoard_RequestUniqueURL_ToStratasFairAdmin(item);
                                        string a = MailResponse;
                                    }
                                }
                            }
                            else
                            {
                                var Users = context.tblUsers.Where(x => x.Status == 1 && x.UserId == 1).FirstOrDefault();
                                if (Users != null)
                                {
                                    if (Users.EmailId.Count() > 0)
                                    {
                                        MailResponse = EmailSender.FncSend_StratasBoard_RequestUniqueURL_ToStratasFairAdmin(Users.EmailId.ToString());
                                        string a = MailResponse;
                                    }
                                }
                            }

                        }
                        return result;
                    }
                    else
                    {
                        return 0;
                    }
                }                
            }
            else
            {
                return -1;
            }
        }

        public bool IsValidPortalLink(string PortalLink)
        {
            bool IsValid = context.vw_GetStratasBoard.Where(x => x.STATUS == 1).Select(x => x.PortalLink == PortalLink).ToList().Count > 0;
            return IsValid;
        }
    }
}
