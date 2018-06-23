using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.NotificationService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new Service1() 
            //};
            //ServiceBase.Run(ServicesToRun);

            var options = ParseOptions(args);
            
            
            var serviceHelper = new ServiceHelper();

            // This process must be configured on each day of the for sending email to Admin  for the expiration of user subscription
            // Two mail will be send as reminder to stratafair admin : first mail and second mail - before 15 and 6 days respectively 
            if (options.DoSubscriptionExpireReminder)
            {
                serviceHelper.SubscriptionExpireReminderData();
                DeleteErrorFile(ConfigurationManager.AppSettings["SubscriptionExpireReminderLogFile"].ToString(), 5242880);  // after 5 MB file will be deleted
            }

            if (options.DoOwnerReminderService)
            {
                // To do
                serviceHelper.OwnerReminderService();
                DeleteErrorFile(ConfigurationManager.AppSettings["OwnerReminderServiceLogFile"].ToString(), 5242880);  // after 5 MB file will be deleted
            }
        }


        private static void DeleteErrorFile(string errorFilePath, Int64 sizeInBytes)
        {
            // 5MB = 5242880 Bytes
            try
            {
                if (System.IO.File.Exists(errorFilePath))
                {
                    System.IO.FileInfo errFile = new System.IO.FileInfo(errorFilePath);
                    if (errFile.Length > sizeInBytes)
                    {
                        System.IO.File.Delete(errorFilePath);
                    }
                }
            }
            catch
            {

            }
        }

        private static CommandOptions ParseOptions(string[] args)
        {
            var options = new CommandOptions();


            // must be run in the same serial as defined in this below code
            var doSubscriptionExpireReminder = GetArgVal(args, "--doSubscriptionExpireReminder");
            var doOwnerReminderService = GetArgVal(args, "--doOwnerReminderService");

            options.DoSubscriptionExpireReminder = !string.IsNullOrEmpty(doSubscriptionExpireReminder);
            options.DoOwnerReminderService = !string.IsNullOrEmpty(doOwnerReminderService);

            return options;
        }

        private static string GetArgVal(string[] args, params string[] paramNames)
        {
            var paramPos = paramNames.Where(p => Array.IndexOf(args, p) >= 0)
                .Select(p => Array.IndexOf(args, p) + 1).FirstOrDefault();
            if (paramPos > 0)
                return args.Length > paramPos && !args[paramPos].Contains("-") ? args[paramPos] : "true";

            return null;
        }
    }
}
