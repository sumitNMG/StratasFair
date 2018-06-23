using StratasFair.BusinessEntity.Admin;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.Business.Admin
{
    public class EmailServerHelper
    {
        public EmailServerHelper()
        {

        }

        public EmailServerModel GetEmailServerDetails(EmailServerModel emailServerModel)
        {
            using (var db = new StratasFairDBEntities())
            {
                var emailServer = db.tblEmailServers.Where(m => m.Active == 1).SingleOrDefault();
                if (emailServer != null)
                {
                    emailServerModel.Id = emailServer.Id;
                    emailServerModel.EmailServer = emailServer.EmailServer;
                    emailServerModel.NetworkUserId = emailServer.NetworkUserId;
                    emailServerModel.NetworkUserPassword = emailServer.NetworkUserPassword;
                    emailServerModel.SenderDisplayName = emailServer.SenderDisplayName;
                    emailServerModel.FromEmail = emailServer.FromEmail;
                    emailServerModel.Port = (int)emailServer.Port;
                }
            }
            return emailServerModel;
        }


        public int UpdateEmailServerSetting(EmailServerModel emailServerModel)
        {
            int result = -1;
            try
            {
                using (var db = new StratasFairDBEntities())
                {
                    if (emailServerModel.Id > 0)
                    {
                        // update the data
                        var emailServerdb = db.tblEmailServers.FirstOrDefault(m => m.Id == emailServerModel.Id);
                        emailServerdb.EmailServer = emailServerModel.EmailServer;
                        emailServerdb.NetworkUserId = emailServerModel.NetworkUserId;
                        emailServerdb.NetworkUserPassword = emailServerModel.NetworkUserPassword;
                        emailServerdb.SenderDisplayName = emailServerModel.SenderDisplayName;
                        emailServerdb.FromEmail = emailServerModel.FromEmail;
                        emailServerdb.Port = emailServerModel.Port;
                        db.Entry(emailServerdb).State = EntityState.Modified;
                    }
                    else
                    {
                        // insert the new data
                        var emailServerdb = new tblEmailServer();
                        emailServerdb.EmailServer = emailServerModel.EmailServer;
                        emailServerdb.NetworkUserId = emailServerModel.NetworkUserId;
                        emailServerdb.NetworkUserPassword = emailServerModel.NetworkUserPassword;
                        emailServerdb.SenderDisplayName = emailServerModel.SenderDisplayName;
                        emailServerdb.FromEmail = emailServerModel.FromEmail;
                        emailServerdb.Port = emailServerModel.Port;
                        emailServerdb.Active = 1;
                        db.tblEmailServers.Add(emailServerdb);
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
    }
}
