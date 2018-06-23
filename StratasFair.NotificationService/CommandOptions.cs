using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.NotificationService
{
    internal class CommandOptions
    {
       // private const string WebsitePathKey = "WebsitePath";


       // public string WebsitePath { get; set; }
        public bool DoSubscriptionExpireReminder { get; set; }
        public bool DoOwnerReminderService { get; set; }
      

        public CommandOptions()
        {
            var currentPath = Path.GetDirectoryName(GetType().Assembly.GetName().FullName);
            if (currentPath == null)
                throw new IOException("Application path not found");

           // var websitePath = default(string);

            //try
            //{
            //    websitePath = ConfigurationManager.AppSettings[WebsitePathKey];
            //}
            //catch (Exception)
            //{
            //}

            //WebsitePath = websitePath;
        }
    }
}
