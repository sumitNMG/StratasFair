using StratasFair.BusinessEntity.Admin;
using StratasFair.Business.Admin;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Admin
{
    public class EmailNotificationHelper
    {
        StratasFairDBEntities context;
        public EmailNotificationHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }

        #region // Admin Email Notification function//

        public EmailNotificationModel GetEmailNotificationDetails(EmailNotificationModel emailNotificationModel)
        {
            var adminEmailNotificationdb = context.tblAdminEmailNotificationSettings.Where(m => m.Status == 1).SingleOrDefault();
            if (adminEmailNotificationdb != null)
            {
                emailNotificationModel.AdminEmailNotificationSettingId = adminEmailNotificationdb.AdminEmailNotificationSettingId;
                emailNotificationModel.EmailAddress = adminEmailNotificationdb.EmailAddress;
                emailNotificationModel.Status = adminEmailNotificationdb.Status;
            }
            return emailNotificationModel;
        }

        public int UpdateEmailNotificationSetting(EmailNotificationModel emailNotificationModel)
        {
            int result = -1;
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    if (emailNotificationModel.AdminEmailNotificationSettingId > 0)
                    {
                        // update the data
                        var adminEmailNotificationdb = db.tblAdminEmailNotificationSettings.FirstOrDefault(m => m.AdminEmailNotificationSettingId == emailNotificationModel.AdminEmailNotificationSettingId);
                        adminEmailNotificationdb.EmailAddress = emailNotificationModel.EmailAddress;
                        adminEmailNotificationdb.ModifiedBy = AdminSessionData.AdminUserId;
                        adminEmailNotificationdb.ModifiedOn = DateTime.Now;
                        adminEmailNotificationdb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                        db.Entry(adminEmailNotificationdb).State = EntityState.Modified;
                    }
                    else
                    {
                        // insert the new data
                        var adminEmailNotificationdb = new tblAdminEmailNotificationSetting();
                        adminEmailNotificationdb.EmailAddress = emailNotificationModel.EmailAddress;
                        adminEmailNotificationdb.CreatedBy = AdminSessionData.AdminUserId;
                        adminEmailNotificationdb.CreatedOn = DateTime.Now;
                        adminEmailNotificationdb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                        adminEmailNotificationdb.Status = 1;
                        db.tblAdminEmailNotificationSettings.Add(adminEmailNotificationdb);
                    }
                    db.SaveChanges();
                    result = 1;
                }
                return result;
            }
            catch
            {
            }
            return result;
        }

        #endregion
    }
}
