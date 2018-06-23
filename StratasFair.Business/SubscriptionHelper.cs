using StratasFair.BusinessEntity;
using StratasFair.Business.Admin;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Business
{
    public class SubscriptionHelper
    {
         StratasFairDBEntities context;
         public SubscriptionHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }

         public List<SubscriptionModel> GetAll()
         {
             return context.vw_GetSubscriptionMaster.Select(x => new SubscriptionModel() { SubscriptionId = x.SubscriptionId, SubscriptionName = x.SubscriptionName, Validity = x.Validity, ValidityPeriod=x.ValidityPeriod, AllowedUser = x.AllowedUser.Value, IsSmsForAlert = x.IsSmsForAlert.Value, IsSmsForAlertText=x.IsSmsForAlertText, IsSmsForReminder = x.IsSmsForReminder.Value, IsSmsForReminderText=x.IsSmsForReminderText, CreatedOn = (DateTime)x.CreatedOn, Status = x.STATUS.Value }).ToList();
         }


         public SubscriptionModel GetByID(int Id)
         {
             return context.vw_GetSubscriptionMaster.Where(x => x.SubscriptionId == Id).Select(x => new SubscriptionModel() { SubscriptionId = x.SubscriptionId, SubscriptionName = x.SubscriptionName, Validity = x.Validity, ValidityPeriod = x.ValidityPeriod, AllowedUser = x.AllowedUser.Value, IsSmsForAlert = x.IsSmsForAlert.Value, IsSmsForAlertText = x.IsSmsForAlertText, IsSmsForReminder = x.IsSmsForReminder.Value, IsSmsForReminderText = x.IsSmsForReminderText, CreatedOn = (DateTime)x.CreatedOn, Status = x.STATUS.Value }).FirstOrDefault();
         }

         public int AddUpdate(SubscriptionModel subscriptionModel)
         {


             if (subscriptionModel.SubscriptionId > 0)
             {
                 if (context.tblSubscriptions.Any(x => x.SubscriptionName == subscriptionModel.SubscriptionName && x.Validity == subscriptionModel.Validity && x.SubscriptionId != subscriptionModel.SubscriptionId))
                 {
                     return 1;
                 }
                 else
                 {
                     return update(subscriptionModel);
                 }
             }
             else
             {
                 if (context.tblSubscriptions.Any(x => x.SubscriptionName == subscriptionModel.SubscriptionName && x.Validity == subscriptionModel.Validity))
                 {
                     return 1;
                 }
                 else
                 {
                     return update(subscriptionModel);
                 }
             }
         }

         public int update(SubscriptionModel subscriptionModel)
         {

             tblSubscription tblSubscriptionDb = new tblSubscription();
             tblSubscriptionDb.SubscriptionId = subscriptionModel.SubscriptionId;
             tblSubscriptionDb.SubscriptionName = subscriptionModel.SubscriptionName;
             tblSubscriptionDb.Validity = subscriptionModel.Validity;
             tblSubscriptionDb.AllowedUser = subscriptionModel.AllowedUser;
             tblSubscriptionDb.IsSmsForAlert = subscriptionModel.IsSmsForAlert;
             tblSubscriptionDb.IsSmsForReminder = subscriptionModel.IsSmsForReminder;
             tblSubscriptionDb.Status = subscriptionModel.Status;

             if (subscriptionModel.SubscriptionId > 0)
             {
                 tblSubscriptionDb.ModifiedBy = AdminSessionData.AdminUserId;
                 tblSubscriptionDb.ModifiedOn = DateTime.Now;
                 tblSubscriptionDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;

                 context.tblSubscriptions.Attach(tblSubscriptionDb);
                 context.Entry(tblSubscriptionDb).Property(x => x.SubscriptionName).IsModified = true;
                 context.Entry(tblSubscriptionDb).Property(x => x.Validity).IsModified = true;
                 context.Entry(tblSubscriptionDb).Property(x => x.AllowedUser).IsModified = true;
                 context.Entry(tblSubscriptionDb).Property(x => x.IsSmsForAlert).IsModified = true;
                 context.Entry(tblSubscriptionDb).Property(x => x.IsSmsForReminder).IsModified = true;
                 context.Entry(tblSubscriptionDb).Property(x => x.Status).IsModified = true;
                 context.Entry(tblSubscriptionDb).Property(x => x.ModifiedBy).IsModified = true;
                 context.Entry(tblSubscriptionDb).Property(x => x.ModifiedOn).IsModified = true;
                 context.Entry(tblSubscriptionDb).Property(x => x.ModifiedFromIP).IsModified = true;

             }
             else
             {
               
                 tblSubscriptionDb.CreatedBy = AdminSessionData.AdminUserId;
                 tblSubscriptionDb.CreatedOn = DateTime.Now;
                 tblSubscriptionDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                 context.tblSubscriptions.Add(tblSubscriptionDb);
             }

             int count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;

             return count;
         }


         public int ActDeact(SubscriptionModel subscriptionModel)
         {
             tblSubscription tblSubscriptionDb = new tblSubscription();
             tblSubscriptionDb.SubscriptionId = subscriptionModel.SubscriptionId;
             tblSubscriptionDb.Status = subscriptionModel.Status;

             context.tblSubscriptions.Attach(tblSubscriptionDb);
             context.Entry(tblSubscriptionDb).Property(x => x.Status).IsModified = true;

             int count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;

             return count;
         }


         public int Delete(SubscriptionModel subscriptionModel)
         {
             int count = -2;
             // check the subscription associated with strata, if exists then return...
             if (context.tblStratasBoardSubscriptions.Any(o => o.SubscriptionId == subscriptionModel.SubscriptionId))
             {
                 return count;
             }


             tblSubscription tblSubscriptionDb = new tblSubscription();
             tblSubscriptionDb.SubscriptionId = subscriptionModel.SubscriptionId;
             context.Entry(tblSubscriptionDb).State = EntityState.Deleted;
             count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;
             return count;
         }

       


    }
}
