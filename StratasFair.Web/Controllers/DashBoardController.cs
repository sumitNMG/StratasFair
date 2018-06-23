using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers
{
    [ClientSessionExpire]
    public class DashBoardController : Controller
    {
        StrataOwnerHelper strataOwnerHelper = new StrataOwnerHelper();
        NoticeBoardHelper noticeBoardHelper = new NoticeBoardHelper();

        // GET: DashBoard
        public ActionResult Index()
        {
            ClientLogOnModel model = new ClientLogOnModel();
            ClientLoginHelper clientLoginHelper = new ClientLoginHelper();
            if (ClientSessionData.UserClientId != 0)
            {
                model = clientLoginHelper.GetById(ClientSessionData.UserClientId);
            }
            return View(model);
        }

        // GET: DashboardOwner
        public ActionResult OwnerDashboard()
        {
            DashBoardModel model = new DashBoardModel();
            if (ClientSessionData.UserClientId != 0)
            {
                int BlockSize = 3;
                model.AlertAndNotificationModelList = noticeBoardHelper.GetAllAlertAndNotification(1, BlockSize);
                model.MaintenanceScheduleModelList = noticeBoardHelper.GetAllMaintenanceSchedule(1, BlockSize);
                model.ScheduleMeetingModelList = noticeBoardHelper.GetAllScheduleMeeting(1, BlockSize);
                model.StrataOwnerBookingRequestModelList = strataOwnerHelper.GetStrataOwnerBookingRequest(1, BlockSize);
            }
            return View(model);
        }

        public ActionResult ViewLatestAlert(int AlertNotificationId)
        {
            AlertAndNotificationModel alertAndNotificationModel = new AlertAndNotificationModel();
            if (ClientSessionData.UserClientId != 0)
            {
                alertAndNotificationModel = noticeBoardHelper.GetAlertAndNotificationDetail(AlertNotificationId);
            }
            return PartialView("ViewLatestAlert", alertAndNotificationModel);
        }

        public ActionResult ViewMaintenanceSchedule(int MaintenanceScheduleId)
        {
            MaintenanceScheduleModel maintenanceScheduleModel = new MaintenanceScheduleModel();
            if (ClientSessionData.UserClientId != 0)
            {
                maintenanceScheduleModel = noticeBoardHelper.GetMaintenanceScheduleDetail(MaintenanceScheduleId);
            }
            return PartialView("ViewMaintenanceSchedule", maintenanceScheduleModel);
        }

        public ActionResult ViewScheduleMeeting(int MeetingId)
        {
            ScheduleMeetingModel scheduleMeetingModel = new ScheduleMeetingModel();
            if (ClientSessionData.UserClientId != 0)
            {
                scheduleMeetingModel = noticeBoardHelper.GetScheduleMeetingDetail(MeetingId, true);
            }
            return PartialView("ViewScheduleMeeting", scheduleMeetingModel);
        }

        public ActionResult LatestAlert()
        {
            DashBoardModel model = new DashBoardModel();
            NoticeBoardHelper noticeBoardHelper = new NoticeBoardHelper();
            List<AlertAndNotificationModel> alertAndNotificationModelList =new List<AlertAndNotificationModel>();
            if (ClientSessionData.UserClientId != 0)
            {
                int BlockSize = 10;
                alertAndNotificationModelList = noticeBoardHelper.GetAllAlertAndNotification(1, BlockSize);
            }
            return View(alertAndNotificationModelList);
        }

        [ChildActionOnly]
        public ActionResult ListLatestAlert(List<ScheduleMeetingModel> model)
        {
            return PartialView(model);
        }

        public ActionResult MaintenanceSchedule()
        {
            DashBoardModel model = new DashBoardModel();
            NoticeBoardHelper noticeBoardHelper = new NoticeBoardHelper();
            List<MaintenanceScheduleModel> maintenanceScheduleModelList = new List<MaintenanceScheduleModel>();
            if (ClientSessionData.UserClientId != 0)
            {
                int BlockSize = 10;
                maintenanceScheduleModelList = noticeBoardHelper.GetAllMaintenanceSchedule(1, BlockSize);
            }
            return View(maintenanceScheduleModelList);
        }

        [ChildActionOnly]
        public ActionResult ListMaintenanceSchedule(List<MaintenanceScheduleModel> model)
        {
            return PartialView(model);
        }

        public ActionResult ScheduleMeeting()
        {
            DashBoardModel model = new DashBoardModel();
            NoticeBoardHelper noticeBoardHelper = new NoticeBoardHelper();
            List<ScheduleMeetingModel> scheduleMeetingModelList = new List<ScheduleMeetingModel>();
            if (ClientSessionData.UserClientId != 0)
            {
                int BlockSize = 10;
                scheduleMeetingModelList = noticeBoardHelper.GetAllScheduleMeeting(1, BlockSize);
            }
            return View(scheduleMeetingModelList);
        }

        [ChildActionOnly]
        public ActionResult ListScheduleMeeting(List<ScheduleMeetingModel> model)
        {
            return PartialView(model);
        }

        public ActionResult GetListOwnerBookingRequest()
        {
            int BlockSize = 10;
            List<StrataOwnerBookingRequestModel> strataOwnerBookingRequestModelList = strataOwnerHelper.GetStrataOwnerBookingRequest(1, BlockSize, true);
            return View("~/Views/OwnerRequestBooking/GetListOwnerBookingRequest.cshtml", strataOwnerBookingRequestModelList);
        }

        // GET: SignOut
        public ActionResult SignOut()
        {
            string PortalLink = ClientSessionData.ClientPortalLink;
            ClientSessionData.UserClientId = 0;
            ClientSessionData.ClientUserName = "";
            ClientSessionData.ClientRoleName = "";
            ClientSessionData.ClientPortalLink = "";
            ClientSessionData.ClientCreatedOn = "";
            ClientSessionData.ClientLastLoginOn = "";
            ClientSessionData.ClientName = "";

            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.AppendHeader("Cache-Control", "no-store");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);


            return Redirect(Url.Content("~/" + PortalLink + "/Login"));
        }
    }
}