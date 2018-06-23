using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Admin;
using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.Business.Admin
{
    public class EmailServerClientHelper
    {
        StratasFairDBEntities _context = new StratasFairDBEntities();
        public EmailServerClientHelper()
        {

        }

        public EmailServerSettingModel GetEmailServerClientDetails()
        {
            EmailServerSettingModel emailServerSettingModel = new EmailServerSettingModel();
            var emailServer = _context.tblEmailServerClients.Where(m => m.StratasBoardId == ClientSessionData.ClientStrataBoardId && m.Active == 1).SingleOrDefault();
            if (emailServer != null)
            {
                emailServerSettingModel.Id = emailServer.Id;
                emailServerSettingModel.SenderDisplayName = emailServer.SenderDisplayName;
                emailServerSettingModel.FromEmail = emailServer.FromEmail;
            }
            return emailServerSettingModel;
        }


        public int AddUpdateEmailServerSettingsClient(EmailServerSettingModel emailServerSettingModel)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    int result = -1;
                    tblEmailServerClient tblEmailServerClientdb = new tblEmailServerClient();
                    var EmailServerAdmin = _context.tblEmailServers.Where(x => x.Active == 1).FirstOrDefault();
                    tblEmailServerClientdb.SenderDisplayName = emailServerSettingModel.SenderDisplayName;
                    tblEmailServerClientdb.FromEmail = emailServerSettingModel.FromEmail;
                    tblEmailServerClientdb.StratasBoardId = ClientSessionData.ClientStrataBoardId;
                    tblEmailServerClientdb.Active = 1;
                    tblEmailServerClientdb.CreatedOn = DateTime.UtcNow;
                    tblEmailServerClientdb.ModifiedOn = DateTime.UtcNow;
                    if (emailServerSettingModel.Id == 0)
                    {
                        _context.tblEmailServerClients.Add(tblEmailServerClientdb);
                        result = _context.SaveChanges();
                    }
                    else
                    {
                        tblEmailServerClientdb.Id = emailServerSettingModel.Id;
                        _context.tblEmailServerClients.Attach(tblEmailServerClientdb);
                        _context.Entry(tblEmailServerClientdb).Property(x => x.SenderDisplayName).IsModified = true;
                        _context.Entry(tblEmailServerClientdb).Property(x => x.FromEmail).IsModified = true;
                        _context.Entry(tblEmailServerClientdb).Property(x => x.ModifiedOn).IsModified = true; 
                        result = _context.SaveChanges();
                        result = 0;
                    }
                    return result;
                }
                catch
                {
                    return -2;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
