using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Front
{

    public class NoticeBoardHelper
    {
        StratasFairDBEntities _context;
        public string _conString = String.Empty;

        public NoticeBoardHelper()
        {
            if (_context == null)
            {
                _context = new StratasFairDBEntities();
            }
        }

        #region Latest Alerts

        public List<AlertAndNotificationModel> GetAllAlertAndNotification(int BlockNumber, int BlockSize)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<AlertAndNotificationModel> alertAndNotificationModelList = new List<AlertAndNotificationModel>();
                try
                {
                    var AlertAndNotifications = _context.tblAlertAndNotifications.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1).OrderByDescending(x => x.CreatedOn).ToList();
                    foreach (var item in AlertAndNotifications)
                    {
                        AlertAndNotificationModel alertAndNotificationModel = new AlertAndNotificationModel();
                        alertAndNotificationModel.AlertNotificationId = item.AlertNotificationId;
                        alertAndNotificationModel.MessageType = item.MessageType;
                        alertAndNotificationModel.MessageTypeName = item.MessageType == 1 ? "Emergency Alert" : item.MessageType == 2 ? "Broadcast Notification" : "N/A";
                        alertAndNotificationModel.Title = item.Title;
                        alertAndNotificationModel.Message = item.Message;
                        alertAndNotificationModel.CreatedOn = item.CreatedOn != null ? item.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                        alertAndNotificationModelList.Add(alertAndNotificationModel);
                    }
                }
                catch
                {
                }
                alertAndNotificationModelList = alertAndNotificationModelList.Skip(startIndex).Take(BlockSize).ToList();
                return alertAndNotificationModelList;
            }
            else
            {
                return null;
            }
        }


        public AlertAndNotificationModel GetAlertAndNotificationDetail(int AlertNotificationId)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    var AlertAndNotifications = _context.tblAlertAndNotifications.Where(x => x.Status == 1 && x.AlertNotificationId == AlertNotificationId).FirstOrDefault();
                    AlertAndNotificationModel alertAndNotificationModel = new AlertAndNotificationModel();
                    alertAndNotificationModel.AlertNotificationId = AlertAndNotifications.AlertNotificationId;
                    alertAndNotificationModel.MessageType = AlertAndNotifications.MessageType;
                    alertAndNotificationModel.MessageTypeName = AlertAndNotifications.MessageType == 1 ? "Emergency Alert" : AlertAndNotifications.MessageType == 2 ? "Broadcast Notification" : "N/A";
                    alertAndNotificationModel.Title = AlertAndNotifications.Title;
                    alertAndNotificationModel.Message = AlertAndNotifications.Message;
                    alertAndNotificationModel.CreatedOn = AlertAndNotifications.CreatedOn != null ? AlertAndNotifications.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                    return alertAndNotificationModel;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public int AddUpdateAlertAndNotification(AlertAndNotificationModel alertAndNotificationModel)
        {
            int result = 0;
            try
            {
                tblAlertAndNotification tblAlertAndNotificationDb = new tblAlertAndNotification();
                tblAlertAndNotificationDb.Title = alertAndNotificationModel.Title;
                tblAlertAndNotificationDb.Message = alertAndNotificationModel.Message;
                tblAlertAndNotificationDb.MessageType = alertAndNotificationModel.MessageType;
                tblAlertAndNotificationDb.Status = 1;

                tblAlertAndNotificationDb.CreatedBy = ClientSessionData.UserClientId;
                tblAlertAndNotificationDb.CreatedOn = DateTime.UtcNow;
                tblAlertAndNotificationDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblAlertAndNotificationDb.ModifiedBy = ClientSessionData.UserClientId;
                tblAlertAndNotificationDb.ModifiedOn = DateTime.UtcNow;
                tblAlertAndNotificationDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblAlertAndNotificationDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;



                if (alertAndNotificationModel.AlertNotificationId > 0)
                {
                    tblAlertAndNotificationDb.AlertNotificationId = alertAndNotificationModel.AlertNotificationId;
                    _context.tblAlertAndNotifications.Attach(tblAlertAndNotificationDb);
                    _context.Entry(tblAlertAndNotificationDb).Property(x => x.Title).IsModified = true;
                    _context.Entry(tblAlertAndNotificationDb).Property(x => x.Message).IsModified = true;
                    _context.Entry(tblAlertAndNotificationDb).Property(x => x.MessageType).IsModified = true;
                    _context.Entry(tblAlertAndNotificationDb).Property(x => x.ModifiedBy).IsModified = true;
                    _context.Entry(tblAlertAndNotificationDb).Property(x => x.ModifiedOn).IsModified = true;
                    _context.Entry(tblAlertAndNotificationDb).Property(x => x.ModifiedFromIP).IsModified = true;
                    result = _context.SaveChanges();

                }
                else
                {
                    _context.tblAlertAndNotifications.Add(tblAlertAndNotificationDb);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }


        public int DeleteAlertAndNotification(int AlertNotificationId)
        {
            int result = 0;
            try
            {

                var AlertAndNotifications = _context.tblAlertAndNotifications.Where(x => x.AlertNotificationId == AlertNotificationId).FirstOrDefault();

                if (AlertAndNotifications != null)
                {
                    _context.tblAlertAndNotifications.Remove(AlertAndNotifications);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }

        public int ResendAlertAndNotification(AlertAndNotificationModel alertAndNotificationModel)
        {
            int result = 0;
            try
            {

                var StrataOwners = _context.tblUserClients.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1 && x.RoleName == "O").ToList();
                if (StrataOwners != null && StrataOwners.Count > 0)
                {
                    foreach (var item in StrataOwners)
                    {
                        EmailSender.FncSendAlertAndNotification_ToStrataOwner(item.UserClientId, alertAndNotificationModel.MessageTypeName, alertAndNotificationModel.Title, alertAndNotificationModel.Message);
                    }
                    result = 1;
                }               
            }
            catch
            {

            }
            return result;
        }

        #endregion

        #region Meeting Schedule
        public List<ScheduleMeetingModel> GetAllScheduleMeeting(int BlockNumber, int BlockSize)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<ScheduleMeetingModel> scheduleMeetingModelList = new List<ScheduleMeetingModel>();
                try
                {
                    var ScheduleMeetings = _context.tblScheduleMeetings.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1).OrderByDescending(x => x.CreatedOn).ToList();
                    foreach (var item in ScheduleMeetings)
                    {
                        int i = 0;
                        ScheduleMeetingModel scheduleMeetingModel = new ScheduleMeetingModel();
                        scheduleMeetingModel.MeetingId = item.MeetingId;
                        scheduleMeetingModel.Subject = item.Subject;
                        scheduleMeetingModel.Message = item.Message;
                        scheduleMeetingModel.MeetingTime = item.MeetingDate.Value.ToString("hh:mm tt");
                        scheduleMeetingModel.MeetingDate = item.MeetingDate != null ? item.MeetingDate.Value.ToString("dd MMM, yyyy") + ", " + scheduleMeetingModel.MeetingTime : "";
                        scheduleMeetingModel.CreatedOn = item.CreatedOn != null ? item.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                        if(i == 0)
                        {
                            scheduleMeetingModel.StrataOwners = this.GetAllStrataOwners();
                        }
                        scheduleMeetingModelList.Add(scheduleMeetingModel);
                        i++;
                    }
                }
                catch
                {
                }
                scheduleMeetingModelList = scheduleMeetingModelList.Skip(startIndex).Take(BlockSize).ToList();
                return scheduleMeetingModelList;
            }
            else
            {
                return null;
            }
        }


        public ScheduleMeetingModel GetScheduleMeetingDetail(int MeetingId, bool IsView)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    var ScheduleMeetings = _context.tblScheduleMeetings.Where(x => x.Status == 1 && x.MeetingId == MeetingId).FirstOrDefault();
                    ScheduleMeetingModel scheduleMeetingModel = new ScheduleMeetingModel();
                    scheduleMeetingModel.MeetingId = ScheduleMeetings.MeetingId;
                    scheduleMeetingModel.Subject = ScheduleMeetings.Subject;
                    scheduleMeetingModel.Message = ScheduleMeetings.Message;
                    scheduleMeetingModel.StartTimeHour = ScheduleMeetings.MeetingDate.Value.Hour;
                    scheduleMeetingModel.CreatedOn = ScheduleMeetings.CreatedOn != null ? ScheduleMeetings.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                    if (scheduleMeetingModel.StartTimeHour == 0)
                    {
                        scheduleMeetingModel.StartTimeHour = 12;
                        scheduleMeetingModel.StartTimeAmPm = "AM";
                    }
                    else if (scheduleMeetingModel.StartTimeHour >= 12)  //// For PM
                    {
                        if (scheduleMeetingModel.StartTimeHour > 12)
                        {
                            scheduleMeetingModel.StartTimeHour = scheduleMeetingModel.StartTimeHour - 12;
                        }
                        scheduleMeetingModel.StartTimeAmPm = "PM";
                    }
                    else  //// For AM
                    {                        
                        scheduleMeetingModel.StartTimeAmPm = "AM";
                    }
                    scheduleMeetingModel.StartTimeMinute = Convert.ToInt32(ScheduleMeetings.MeetingDate.Value.Minute.ToString().PadLeft(2, '0'));
                    if (IsView)
                    {
                        string MeetingTime = ScheduleMeetings.MeetingDate.Value.ToString("hh:mm tt");
                        scheduleMeetingModel.MeetingDate = ScheduleMeetings.MeetingDate != null ? ScheduleMeetings.MeetingDate.Value.ToString("dd MMM, yyyy") + ", " + MeetingTime : "";
                    }
                    else
                    {
                        scheduleMeetingModel.MeetingDate = ScheduleMeetings.MeetingDate != null ? ScheduleMeetings.MeetingDate.Value.ToString("dd MMM yyyy") : "";
                    }
                    var ScheduleMeetinginvites = _context.tblScheduleMeetingInvites.Where(x => x.MeetingId == MeetingId && x.Status == 1).ToList();
                    if (ScheduleMeetinginvites != null && ScheduleMeetinginvites.Count > 0)
                    {
                        for (int i = 0; i < ScheduleMeetinginvites.Count; i++)
                        {
                            scheduleMeetingModel.ScheduleMeetingInviteIds += Convert.ToString(ScheduleMeetinginvites[i].UserClientId) + ",";
                        }
                        scheduleMeetingModel.ScheduleMeetingInviteIds = scheduleMeetingModel.ScheduleMeetingInviteIds.Remove(scheduleMeetingModel.ScheduleMeetingInviteIds.LastIndexOf(","));
                    }
                    return scheduleMeetingModel;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<StrataOwnerModel> GetAllStrataOwners()
        {
            if (ClientSessionData.UserClientId != 0)
            {
                var strataOwnerModelList = new List<StrataOwnerModel>();
                try
                {
                    var StrataOwners = _context.tblUserClients.Where(x => x.Status == 1 && x.RoleName == "O" && x.StratasBoardId == ClientSessionData.ClientStrataBoardId).ToList();
                    foreach (var item in StrataOwners)
                    {
                        StrataOwnerModel strataOwnerModel = new StrataOwnerModel();
                        strataOwnerModel.UserClientId = item.UserClientId;
                        strataOwnerModel.Name = item.FirstName + " " + item.LastName;
                        strataOwnerModelList.Add(strataOwnerModel);
                    }
                }
                catch
                {
                }
                return strataOwnerModelList;
            }
            else
            {
                return null;
            }
        }

        public int AddUpdateScheduleMeeting(ScheduleMeetingModel scheduleMeetingModel)
        {
            int result = 0;
            try
            {
                tblScheduleMeeting tblScheduleMeetingDb = new tblScheduleMeeting();
                tblScheduleMeetingDb.Subject = scheduleMeetingModel.Subject;
                tblScheduleMeetingDb.Message = scheduleMeetingModel.Message;
                tblScheduleMeetingDb.Status = 1;
                tblScheduleMeetingDb.MeetingDate = Convert.ToDateTime(scheduleMeetingModel.MeetingDate);

                if (scheduleMeetingModel.StartTimeAmPm == "PM")
                {
                    if (scheduleMeetingModel.StartTimeHour != 12)
                    {
                        scheduleMeetingModel.StartTimeHour = Convert.ToInt32(scheduleMeetingModel.StartTimeHour) + 12;
                    }
                }
                else if (scheduleMeetingModel.StartTimeAmPm == "AM")
                {
                    if (scheduleMeetingModel.StartTimeHour == 12)
                    {
                        scheduleMeetingModel.StartTimeHour = 0;
                    }
                }
                tblScheduleMeetingDb.MeetingDate = tblScheduleMeetingDb.MeetingDate.Value.AddHours(Convert.ToInt32(scheduleMeetingModel.StartTimeHour)).AddMinutes(Convert.ToInt32(scheduleMeetingModel.StartTimeMinute));

                tblScheduleMeetingDb.CreatedBy = ClientSessionData.UserClientId;
                tblScheduleMeetingDb.CreatedOn = DateTime.UtcNow;
                tblScheduleMeetingDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblScheduleMeetingDb.ModifiedBy = ClientSessionData.UserClientId;
                tblScheduleMeetingDb.ModifiedOn = DateTime.UtcNow;
                tblScheduleMeetingDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblScheduleMeetingDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;


                if (scheduleMeetingModel.MeetingId > 0)
                {
                    tblScheduleMeetingDb.MeetingId = scheduleMeetingModel.MeetingId;
                    _context.tblScheduleMeetings.Attach(tblScheduleMeetingDb);
                    _context.Entry(tblScheduleMeetingDb).Property(x => x.Subject).IsModified = true;
                    _context.Entry(tblScheduleMeetingDb).Property(x => x.Message).IsModified = true;
                    _context.Entry(tblScheduleMeetingDb).Property(x => x.MeetingDate).IsModified = true;
                    _context.Entry(tblScheduleMeetingDb).Property(x => x.ModifiedBy).IsModified = true;
                    _context.Entry(tblScheduleMeetingDb).Property(x => x.ModifiedOn).IsModified = true;
                    _context.Entry(tblScheduleMeetingDb).Property(x => x.ModifiedFromIP).IsModified = true;
                    result = _context.SaveChanges();

                }
                else
                {
                    _context.tblScheduleMeetings.Add(tblScheduleMeetingDb);
                    result = _context.SaveChanges();
                }

                int MeetingId = Convert.ToInt32(tblScheduleMeetingDb.MeetingId);
                if (MeetingId > 0)
                {
                    if (!string.IsNullOrEmpty(scheduleMeetingModel.SelectedMeetingInvite))
                    {
                        if (scheduleMeetingModel.SelectedMeetingInvite != scheduleMeetingModel.ScheduleMeetingInviteIds)
                        {
                            int[] selectedMeetingInviteArray = scheduleMeetingModel.SelectedMeetingInvite.Split(",".ToCharArray()).Select(x => int.Parse(x.ToString())).ToArray();
                            _context.tblScheduleMeetingInvites.RemoveRange(_context.tblScheduleMeetingInvites.Where(x => x.MeetingId == MeetingId));
                            foreach (var item in selectedMeetingInviteArray)
                            {
                                tblScheduleMeetingInvite tblScheduleMeetingInviteDb = new tblScheduleMeetingInvite();
                                tblScheduleMeetingInviteDb.MeetingId = MeetingId;
                                tblScheduleMeetingInviteDb.UserClientId = item;
                                tblScheduleMeetingInviteDb.Status = 1;
                                _context.tblScheduleMeetingInvites.Add(tblScheduleMeetingInviteDb);
                                result = _context.SaveChanges();
                                if (result == 1)
                                {
                                    string MeetingTime = tblScheduleMeetingDb.MeetingDate.Value.ToString("hh:mm tt");
                                    scheduleMeetingModel.MeetingDate = tblScheduleMeetingDb.MeetingDate != null ? tblScheduleMeetingDb.MeetingDate.Value.ToString("dd MMM yyyy") + ", " + MeetingTime : "";
                                    EmailSender.FncSendScheduleMeetingInvite_ToStrataOwner(item, scheduleMeetingModel.MeetingDate, scheduleMeetingModel.Subject, scheduleMeetingModel.Message, ClientSessionData.ClientStrataBoardId );
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return result;
        }


        public int DeleteScheduleMeeting(int MeetingId)
        {
            int result = 0;
            try
            {

                var ScheduleMeetings = _context.tblScheduleMeetings.Where(x => x.MeetingId == MeetingId).FirstOrDefault();

                if (ScheduleMeetings != null)
                {
                    _context.tblScheduleMeetings.Remove(ScheduleMeetings);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }

        #endregion

        #region General Information
        public List<GeneralInformationModel> GetAllGeneralInformation(int BlockNumber, int BlockSize, string SearchKeyword)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<GeneralInformationModel> generalInformationModelList = new List<GeneralInformationModel>();
                try
                {
                    var GeneralInformations = _context.tblGeneralInformations.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1).OrderByDescending(x => x.CreatedOn).ToList();                     

                    if(!string.IsNullOrEmpty(SearchKeyword))
                    {
                        GeneralInformations = _context.tblGeneralInformations.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1 && (x.Title.StartsWith(SearchKeyword) || x.Description.StartsWith(SearchKeyword))).OrderByDescending(x => x.CreatedOn).ToList();    
                    }                   

                    foreach (var item in GeneralInformations)
                    {
                        GeneralInformationModel generalInformationModel = new GeneralInformationModel();
                        generalInformationModel.GeneralInformationId = item.GeneralInformationId;
                        generalInformationModel.Title = item.Title;
                        generalInformationModel.Description = item.Description;
                        generalInformationModel.UploadFile = item.UploadFile;
                        generalInformationModel.ActualUploadFile = item.ActualUploadFile;
                        generalInformationModel.CreatedOn = item.CreatedOn != null ? item.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                        generalInformationModelList.Add(generalInformationModel);
                    }
                }
                catch
                {
                }
                generalInformationModelList = generalInformationModelList.Skip(startIndex).Take(BlockSize).ToList();
                return generalInformationModelList;
            }
            else
            {
                return null;
            }
        }

        public GeneralInformationModel GetGeneralInformationDetail(int GeneralInformationId)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                GeneralInformationModel generalInformationModel = new GeneralInformationModel();
                try
                {
                    var GeneralInformation = _context.tblGeneralInformations.Where(x => x.GeneralInformationId == GeneralInformationId && x.Status == 1).FirstOrDefault();
                    if(GeneralInformation != null)
                    {
                        generalInformationModel.GeneralInformationId = GeneralInformation.GeneralInformationId;
                        generalInformationModel.Title = GeneralInformation.Title;
                        generalInformationModel.Description = GeneralInformation.Description;
                        generalInformationModel.UploadFile = GeneralInformation.UploadFile;
                        generalInformationModel.OldUploadFile = GeneralInformation.UploadFile;
                        generalInformationModel.ActualUploadFile = GeneralInformation.ActualUploadFile;
                        generalInformationModel.CreatedOn = GeneralInformation.CreatedOn != null ? GeneralInformation.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                    }
                }
                catch
                {
                }
                return generalInformationModel;
            }
            else
            {
                return null;
            }
        }


        public int AddUpdateGeneralInformation(GeneralInformationModel generalInformationModel)
        {
            int result = 0;
            try
            {
                tblGeneralInformation tblGeneralInformationDb = new tblGeneralInformation();
                tblGeneralInformationDb.Title = generalInformationModel.Title;
                tblGeneralInformationDb.Description = generalInformationModel.Description;
                tblGeneralInformationDb.UploadFile = generalInformationModel.UploadFile;
                tblGeneralInformationDb.ActualUploadFile = generalInformationModel.ActualUploadFile;
                tblGeneralInformationDb.Status = 1;
                tblGeneralInformationDb.CreatedBy = ClientSessionData.UserClientId;
                tblGeneralInformationDb.CreatedOn = DateTime.UtcNow;
                tblGeneralInformationDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblGeneralInformationDb.ModifiedBy = ClientSessionData.UserClientId;
                tblGeneralInformationDb.ModifiedOn = DateTime.UtcNow;
                tblGeneralInformationDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblGeneralInformationDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;


                if (generalInformationModel.GeneralInformationId > 0)
                {
                    tblGeneralInformationDb.GeneralInformationId = generalInformationModel.GeneralInformationId;
                    _context.tblGeneralInformations.Attach(tblGeneralInformationDb);
                    _context.Entry(tblGeneralInformationDb).Property(x => x.Title).IsModified = true;
                    _context.Entry(tblGeneralInformationDb).Property(x => x.Description).IsModified = true;
                    _context.Entry(tblGeneralInformationDb).Property(x => x.ActualUploadFile).IsModified = true;
                    _context.Entry(tblGeneralInformationDb).Property(x => x.UploadFile).IsModified = true;
                    _context.Entry(tblGeneralInformationDb).Property(x => x.ModifiedBy).IsModified = true;
                    _context.Entry(tblGeneralInformationDb).Property(x => x.ModifiedOn).IsModified = true;
                    _context.Entry(tblGeneralInformationDb).Property(x => x.ModifiedFromIP).IsModified = true;
                    result = _context.SaveChanges();
                }
                else
                {
                    _context.tblGeneralInformations.Add(tblGeneralInformationDb);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }

        public int DeleteGeneralInformation(int GeneralInformationId)
        {
            int result = 0;
            try
            {

                var GeneralInformations = _context.tblGeneralInformations.Where(x => x.GeneralInformationId == GeneralInformationId).FirstOrDefault();

                if (GeneralInformations != null)
                {
                    _context.tblGeneralInformations.Remove(GeneralInformations);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }

        #endregion

        #region Maintenance Schedule
        public List<MaintenanceScheduleModel> GetAllMaintenanceSchedule(int BlockNumber, int BlockSize)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<MaintenanceScheduleModel> maintenanceScheduleModelList = new List<MaintenanceScheduleModel>();
                try
                {
                    var MaintenanceSchedules = _context.vw_GetMaintenanceSchedule.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId).ToList();


                    foreach (var item in MaintenanceSchedules)
                    {
                        int i = 0;
                        MaintenanceScheduleModel maintenanceScheduleModel = new MaintenanceScheduleModel();
                        maintenanceScheduleModel.MaintenanceScheduleId = item.MaintenanceScheduleId;
                        maintenanceScheduleModel.Description = string.IsNullOrEmpty(item.Description) ? "N/A" : item.Description;
                        maintenanceScheduleModel.StartTime = item.ScheduleDate.Value.ToString("hh:mm tt");
                        maintenanceScheduleModel.ScheduleDate = item.ScheduleDate != null ? item.ScheduleDate.Value.ToString("dd MMM, yyyy") + " at " + maintenanceScheduleModel.StartTime : "";
                        maintenanceScheduleModel.CreatedOn = item.CreatedOn != null ? item.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                        maintenanceScheduleModel.CommonAreaName = item.CommonAreaName;

                        maintenanceScheduleModelList.Add(maintenanceScheduleModel);
                        i++;
                    }
                }
                catch
                {
                }
                maintenanceScheduleModelList = maintenanceScheduleModelList.Skip(startIndex).Take(BlockSize).ToList();
                return maintenanceScheduleModelList;
            }
            else
            {
                return null;
            }
        }

        public MaintenanceScheduleModel GetMaintenanceScheduleDetail(int MaintenanceScheduleId)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                MaintenanceScheduleModel maintenanceScheduleModel = new MaintenanceScheduleModel();
                try
                {
                    var MaintenanceSchedule = _context.vw_GetMaintenanceSchedule.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.MaintenanceScheduleId == MaintenanceScheduleId).FirstOrDefault();
                    if (MaintenanceSchedule != null)
                    {
                        maintenanceScheduleModel.MaintenanceScheduleId = MaintenanceSchedule.MaintenanceScheduleId;
                        maintenanceScheduleModel.CommonAreaId = MaintenanceSchedule.CommonAreaId.Value;
                        maintenanceScheduleModel.Description = MaintenanceSchedule.Description;
                        maintenanceScheduleModel.StartTimeHour = MaintenanceSchedule.ScheduleDate.Value.Hour;
                        maintenanceScheduleModel.CommonAreaName = MaintenanceSchedule.CommonAreaName;
                        maintenanceScheduleModel.CreatedOn = MaintenanceSchedule.CreatedOn != null ? MaintenanceSchedule.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                        maintenanceScheduleModel.ScheduleDate = MaintenanceSchedule.ScheduleDate != null ? MaintenanceSchedule.ScheduleDate.Value.ToString("dd MMM yyyy") : "";
                        if (maintenanceScheduleModel.StartTimeHour == 0)
                        {
                            maintenanceScheduleModel.StartTimeHour = 12;
                            maintenanceScheduleModel.StartTimeAmPm = "AM";
                        }
                        else if (maintenanceScheduleModel.StartTimeHour >= 12)  //// For PM
                        {
                            if (maintenanceScheduleModel.StartTimeHour > 12)
                            {
                                maintenanceScheduleModel.StartTimeHour = maintenanceScheduleModel.StartTimeHour - 12;
                            }
                            maintenanceScheduleModel.StartTimeAmPm = "PM";
                        }
                        else  //// For AM
                        {
                            maintenanceScheduleModel.StartTimeAmPm = "AM";
                        }
                        maintenanceScheduleModel.StartTimeMinute = Convert.ToInt32(MaintenanceSchedule.ScheduleDate.Value.Minute.ToString().PadLeft(2, '0'));
                    }
                }
                catch
                {
                }
                return maintenanceScheduleModel;
            }
            else
            {
                return null;
            }
        }

        public int AddUpdateMaintenanceSchedule(MaintenanceScheduleModel maintenanceScheduleModel)
        {
            int result = 0;
            try
            {
                tblMaintenanceSchedule tblMaintenanceScheduleDb = new tblMaintenanceSchedule();
                tblMaintenanceScheduleDb.CommonAreaId = maintenanceScheduleModel.CommonAreaId;
                tblMaintenanceScheduleDb.Description = maintenanceScheduleModel.Description;
                tblMaintenanceScheduleDb.Status = 1;
                tblMaintenanceScheduleDb.CreatedBy = ClientSessionData.UserClientId;
                tblMaintenanceScheduleDb.CreatedOn = DateTime.UtcNow;
                tblMaintenanceScheduleDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblMaintenanceScheduleDb.ModifiedBy = ClientSessionData.UserClientId;
                tblMaintenanceScheduleDb.ModifiedOn = DateTime.UtcNow;
                tblMaintenanceScheduleDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblMaintenanceScheduleDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;
                tblMaintenanceScheduleDb.ScheduleDate = Convert.ToDateTime(maintenanceScheduleModel.ScheduleDate);

                if (maintenanceScheduleModel.StartTimeAmPm == "PM")
                {
                    if (maintenanceScheduleModel.StartTimeHour != 12)
                    {
                        maintenanceScheduleModel.StartTimeHour = Convert.ToInt32(maintenanceScheduleModel.StartTimeHour) + 12;
                    }
                }
                else if (maintenanceScheduleModel.StartTimeAmPm == "AM")
                {
                    if (maintenanceScheduleModel.StartTimeHour == 12)
                    {
                        maintenanceScheduleModel.StartTimeHour = 0;
                    }
                }
                tblMaintenanceScheduleDb.ScheduleDate = tblMaintenanceScheduleDb.ScheduleDate.Value.AddHours(Convert.ToInt32(maintenanceScheduleModel.StartTimeHour)).AddMinutes(Convert.ToInt32(maintenanceScheduleModel.StartTimeMinute));

                var MaintenanceSchedule = _context.tblMaintenanceSchedules.Where(x => x.CommonAreaId == tblMaintenanceScheduleDb.CommonAreaId && x.ScheduleDate == tblMaintenanceScheduleDb.ScheduleDate).FirstOrDefault();

                if (MaintenanceSchedule != null)
                {
                    result = -1;
                }
                else
                {
                    if (maintenanceScheduleModel.MaintenanceScheduleId > 0)
                    {
                        tblMaintenanceScheduleDb.MaintenanceScheduleId = maintenanceScheduleModel.MaintenanceScheduleId;
                        _context.tblMaintenanceSchedules.Attach(tblMaintenanceScheduleDb);
                        _context.Entry(tblMaintenanceScheduleDb).Property(x => x.Description).IsModified = true;
                        _context.Entry(tblMaintenanceScheduleDb).Property(x => x.CommonAreaId).IsModified = true;
                        _context.Entry(tblMaintenanceScheduleDb).Property(x => x.ScheduleDate).IsModified = true;
                        _context.Entry(tblMaintenanceScheduleDb).Property(x => x.ModifiedBy).IsModified = true;
                        _context.Entry(tblMaintenanceScheduleDb).Property(x => x.ModifiedOn).IsModified = true;
                        _context.Entry(tblMaintenanceScheduleDb).Property(x => x.ModifiedFromIP).IsModified = true;
                        result = _context.SaveChanges();
                    }
                    else
                    {
                        _context.tblMaintenanceSchedules.Add(tblMaintenanceScheduleDb);
                        result = _context.SaveChanges();
                    }
                }
                
            }
            catch
            {

            }
            return result;
        }

        public int DeleteMaintenanceSchedule(int MaintenanceScheduleId)
        {
            int result = 0;
            try
            {

                var MaintenanceSchedule = _context.tblMaintenanceSchedules.Where(x => x.MaintenanceScheduleId == MaintenanceScheduleId).FirstOrDefault();

                if (MaintenanceSchedule != null)
                {
                    _context.tblMaintenanceSchedules.Remove(MaintenanceSchedule);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }

        #endregion
    }
}
