using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Common;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers.Admin
{
    [ClientSessionExpire]
    [StrataBoardAdmin]
    public class NoticeBoardController : Controller
    {
        NoticeBoardHelper noticeBoardHelper = new NoticeBoardHelper();

        // GET: Notice Board Latest Alerts

        public ActionResult Index()
        {
            int BlockSize = 10;
            NoticeBoardModel noticeBoardModel = new NoticeBoardModel();
            noticeBoardModel.AlertAndNotificationModelList = noticeBoardHelper.GetAllAlertAndNotification(1, BlockSize);
            return View(noticeBoardModel);
        }

        #region Latest Alerts


        [StrataBoardAdmin]
        [ChildActionOnly]
        public ActionResult ListAlertAndNotification(List<AlertAndNotificationModel> model)
        {
            return PartialView(model);
        }

        // POST: GeneralInformationInfinateScroll

        [StrataBoardAdmin]
        [HttpPost]
        public ActionResult LatestAlertInfinateScroll(int BlockNumber)
        {
            int BlockSize = 10;
            var AlertAndNotificationModel = noticeBoardHelper.GetAllAlertAndNotification(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = AlertAndNotificationModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListAlertAndNotification", AlertAndNotificationModel);
            return Json(jsonModel);
        }

        // GET: AddAlertAndNotification
        [HttpGet]
        public ActionResult AddAlertAndNotification()
        {
            AlertAndNotificationModel model = new AlertAndNotificationModel();
            return PartialView("AddAlertAndNotification", model);
        }

        // POST: AddAlertAndNotification
        [HttpPost]
        public ActionResult AddAlertAndNotification(AlertAndNotificationModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = noticeBoardHelper.AddUpdateAlertAndNotification(model);
                if (result == -1)
                {
                    strMsg = "Alert already exists for Common Area with given date";
                }
                else if (result == 1)
                {
                    strMsg = "Alert created successfully.";
                }
                else
                {
                    strMsg = "Alert creation failed.";
                }
            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: EditAlertAndNotification
        [HttpGet]
        public ActionResult EditAlertAndNotification(int AlertAndNotificationId)
        {
            AlertAndNotificationModel model = noticeBoardHelper.GetAlertAndNotificationDetail(AlertAndNotificationId);
            return PartialView("EditAlertAndNotification", model);
        }

        // POST: EditAlertAndNotification
        [HttpPost]
        public ActionResult EditAlertAndNotification(AlertAndNotificationModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = noticeBoardHelper.AddUpdateAlertAndNotification(model);
                if (result == -1)
                {
                    strMsg = "Alert already exists for Common Area with given date";
                }
                else if (result == 1)
                {
                    strMsg = "Alert updated successfully.";
                }
                else
                {
                    strMsg = "Alert updation failed.";
                }
            }
            return Json(new { result = result, Msg = strMsg });
        }

        // GET: ViewAlertAndNotification
        [HttpGet]
        public ActionResult ViewAlertAndNotification(int AlertAndNotificationId)
        {
            AlertAndNotificationModel model = noticeBoardHelper.GetAlertAndNotificationDetail(AlertAndNotificationId);
            return PartialView("ViewAlertAndNotification", model);
        }

        // POST: DeleteAlertAndNotification
        [HttpPost]
        public JsonResult DeleteAlertAndNotification(int AlertAndNotificationId)
        {
            int result = noticeBoardHelper.DeleteAlertAndNotification(AlertAndNotificationId);
            if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Alert has been deleted successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Alert deletion failed.");
            }
            return Json(new { success = true });
        }

        // POST: ResendAlertAndNotification
        [HttpGet]
        public JsonResult ResendAlertAndNotification(int AlertAndNotificationId)
        {
            string strMsg =string.Empty;
            AlertAndNotificationModel model = noticeBoardHelper.GetAlertAndNotificationDetail(AlertAndNotificationId);
            int result = noticeBoardHelper.ResendAlertAndNotification(model);
            if (result == 1)
            {
                strMsg = "Alert has been sent successfully to Strata owners";
            }
            else
            {
                strMsg = "Alert has not been sent";
            }
            return Json(new { result = result, Msg = strMsg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Meeting Schedule

        // GET: Notice Board meetings Schedule 
        public ActionResult MeetingSchedule()
        {
            int BlockSize = 10;
            NoticeBoardModel noticeBoardModel = new NoticeBoardModel();
            noticeBoardModel.ScheduleMeetingModelList = noticeBoardHelper.GetAllScheduleMeeting(1, BlockSize);
            return View(noticeBoardModel);
        }

        [ChildActionOnly]
        public ActionResult ListScheduleMeeting(List<ScheduleMeetingModel> model)
        {
            return PartialView(model);
        }

        // GET: AddScheduleMeeting
        [HttpGet]
        public ActionResult AddScheduleMeeting()
        {
            ScheduleMeetingModel model = new ScheduleMeetingModel();
            model.StrataOwners = noticeBoardHelper.GetAllStrataOwners();
            return PartialView("AddScheduleMeeting", model);
        }

        // POST: AddScheduleMeeting
        [HttpPost]
        public ActionResult AddScheduleMeeting(ScheduleMeetingModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.SelectedMeetingInvite) || !string.IsNullOrEmpty(model.ScheduleMeetingInviteIds))
                {
                    result = noticeBoardHelper.AddUpdateScheduleMeeting(model);
                    if (result == -1)
                    {
                        strMsg = "ScheduleMeeting already exists with the same name";
                    }
                    else if (result == 1)
                    {
                        strMsg = "ScheduleMeeting created successfully.";
                    }
                    else
                    {
                        strMsg = "ScheduleMeeting creation failed.";
                    }
                }
                else
                {
                    strMsg = "Please select atleast one owner for invite.";
                }
            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: EditScheduleMeeting
        [HttpGet]
        public ActionResult EditScheduleMeeting(int MeetingId)
        {
            ScheduleMeetingModel model = noticeBoardHelper.GetScheduleMeetingDetail(MeetingId, false);
            model.StrataOwners = noticeBoardHelper.GetAllStrataOwners();
            return PartialView("EditScheduleMeeting", model);
        }

        // POST: EditScheduleMeeting
        [HttpPost]
        public ActionResult EditScheduleMeeting(ScheduleMeetingModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.SelectedMeetingInvite) || !string.IsNullOrEmpty(model.ScheduleMeetingInviteIds))
                {
                    result = noticeBoardHelper.AddUpdateScheduleMeeting(model);
                    if (result == -1)
                    {
                        strMsg = "Schedule Meeting already exists with the same name";
                    }
                    else if (result == 1)
                    {
                        strMsg = "Schedule Meeting updated successfully.";
                    }
                    else
                    {
                        strMsg = "Schedule Meeting updation failed.";
                    }
                }
                else
                {
                    strMsg = "Please select atleast one owner for invite.";
                }
            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: ViewScheduleMeeting
        [HttpGet]
        public ActionResult ViewScheduleMeeting(int MeetingId)
        {
            ScheduleMeetingModel model = noticeBoardHelper.GetScheduleMeetingDetail(MeetingId, true);
            model.StrataOwners = noticeBoardHelper.GetAllStrataOwners();
            return PartialView("ViewScheduleMeeting", model);
        }

        private string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult =
                ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext
                (ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        // POST: ScheduleMeetingInfinateScroll
        [HttpPost]
        public ActionResult MeetingInfinateScroll(int BlockNumber)
        {
            int BlockSize = 10;
            var ScheduleMeetingModel = noticeBoardHelper.GetAllScheduleMeeting(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = ScheduleMeetingModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListScheduleMeeting", ScheduleMeetingModel);
            return Json(jsonModel);
        }

        // POST: DeleteScheduleMeeting
        [HttpPost]
        public JsonResult DeleteScheduleMeeting(int MeetingId)
        {
            int result = noticeBoardHelper.DeleteScheduleMeeting(MeetingId);
            if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Schedule Meeting has been deleted successfully.");
            }
            else if (result == -1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Schedule Meeting can not be deleted when it is booked");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Schedule Meeting deletion failed.");
            }
            return Json(new { success = true });
        }
        #endregion

        #region General Information

        // GET: Notice Board General Informations 
        public ActionResult GeneralInformation()
        {
            int BlockSize = 10;
            NoticeBoardModel noticeBoardModel = new NoticeBoardModel();
            noticeBoardModel.GeneralInformationModelList = noticeBoardHelper.GetAllGeneralInformation(1, BlockSize, string.Empty);
            return View(noticeBoardModel);
        }


        [ChildActionOnly]
        public ActionResult ListGeneralInformation(List<GeneralInformationModel> model)
        {
            return PartialView(model);
        }

        // POST: GeneralInformationInfinateScroll
        [HttpPost]
        public ActionResult InformationInfinateScroll(int BlockNumber, string SearchKeyword)
        {
            int BlockSize = 10;
            var GeneralInformationModel = noticeBoardHelper.GetAllGeneralInformation(BlockNumber, BlockSize, SearchKeyword);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = GeneralInformationModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListGeneralInformation", GeneralInformationModel);
            return Json(jsonModel);
        }

        // GET: AddGeneralInformation
        [HttpGet]
        public ActionResult AddGeneralInformation()
        {
            GeneralInformationModel model = new GeneralInformationModel();
            return PartialView("AddGeneralInformation", model);
        }

        // POST: AddGeneralInformation
        [HttpPost]
        public ActionResult AddGeneralInformation(GeneralInformationModel model, HttpPostedFileBase FileUpload)
        {
            int result = 0;
            string strMsg = "";
            try
            {
                var imageTypes = new string[]{    
                     "image/png",
                     "image/jpg",
                     "image/jpeg",
                    "image/pjpeg",
                    "application/pdf",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                    };
                if (!imageTypes.Contains(FileUpload.ContentType))
                {
                    strMsg = "User can add the file with extension (.jpg, .png, .pdf, .docx, .pptx, .xlsx)";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        model.ActualUploadFile = System.IO.Path.GetFileName(FileUpload.FileName);
                        model.UploadFile = Guid.NewGuid() + System.IO.Path.GetExtension(FileUpload.FileName);
                        result = noticeBoardHelper.AddUpdateGeneralInformation(model);

                        string path = "~/Content/Resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/generalinformation/";
                        if (!Directory.Exists(Server.MapPath(path)))
                        {
                            Directory.CreateDirectory(Server.MapPath(path));
                        }
                        string Mappedpath = Server.MapPath(path + model.UploadFile);


                        if (result == 1)
                        {
                            // save the file locally
                            FileUpload.SaveAs(Mappedpath);
                            // save the file on s3
                            int fileMapped = AwsS3Bucket.CreateFile("resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/generalinformation/" + model.UploadFile, Mappedpath);
                            // delete the file locally

                            if (System.IO.File.Exists(Mappedpath))
                            {
                                System.IO.File.Delete(Mappedpath);
                            }

                            strMsg = "General Information created successfully.";
                        }
                        else
                        {
                            strMsg = "General Information creation failed.";
                        }
                    }
                }
            }
            catch
            {

            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: EditGeneralInformation
        [HttpGet]
        public ActionResult EditGeneralInformation(int GeneralInformationId)
        {
            GeneralInformationModel model = noticeBoardHelper.GetGeneralInformationDetail(GeneralInformationId);
            return PartialView("EditGeneralInformation", model);
        }

        // POST: EditGeneralInformation
        [HttpPost]
        public ActionResult EditGeneralInformation(GeneralInformationModel model, HttpPostedFileBase FileUpload1)
        {
            int result = 0;
            string strMsg = "";
            try
            {
                var imageTypes = new string[]{    
                     "image/png",
                     "image/jpg",
                    "image/pjpeg",
                    "application/pdf",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                    };
                if (!imageTypes.Contains(FileUpload1.ContentType))
                {
                    strMsg = "User can add the file with extension (.jpg, .png, .pdf, .docx, .pptx, .xlsx)";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        model.ActualUploadFile = System.IO.Path.GetFileName(FileUpload1.FileName);
                        model.UploadFile = Guid.NewGuid() + System.IO.Path.GetExtension(FileUpload1.FileName);
                        result = noticeBoardHelper.AddUpdateGeneralInformation(model);

                        string path = "~/Content/Resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/generalinformation/";
                        if (!Directory.Exists(Server.MapPath(path)))
                        {
                            Directory.CreateDirectory(Server.MapPath(path));
                        }
                        string Mappedpath = Server.MapPath(path + model.UploadFile);


                        if (result == 1)
                        {
                            // save the file locally
                            FileUpload1.SaveAs(Mappedpath);
                            // save the file on s3
                            int fileMapped = AwsS3Bucket.CreateFile("resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/generalinformation/" + model.UploadFile, Mappedpath);
                            // delete the file locally

                            if (System.IO.File.Exists(Mappedpath))
                            {
                                System.IO.File.Delete(Mappedpath);
                            }

                            string OldProfilePath = Server.MapPath(path + model.OldUploadFile);
                            if (System.IO.File.Exists(OldProfilePath))
                            {
                                System.IO.File.Delete(OldProfilePath);
                            }
                            // delete the old file from s3
                            AwsS3Bucket.DeleteObject("resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/generalinformation/" + model.OldUploadFile);

                            strMsg = "General Information updated successfully.";
                        }
                        else
                        {
                            strMsg = "General Information updation failed.";
                        }
                    }
                }
            }
            catch
            {

            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: ViewGeneralInformation
        [HttpGet]
        public ActionResult ViewGeneralInformation(int GeneralInformationId)
        {
            GeneralInformationModel model = noticeBoardHelper.GetGeneralInformationDetail(GeneralInformationId);
            return PartialView("ViewGeneralInformation", model);
        }       

        // POST: DeleteGeneralInformation
        [HttpPost]
        public JsonResult DeleteGeneralInformation(int GeneralInformationId)
        {
            int result = noticeBoardHelper.DeleteGeneralInformation(GeneralInformationId);
            if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Schedule Meeting has been deleted successfully.");
            }
            else if (result == -1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(1, "Schedule Meeting can not be deleted when it is booked");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Schedule Meeting deletion failed.");
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public void DownloadUploadedFile(int GeneralInformationId)
        {
            string FilePath = "resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/generalinformation";
            GeneralInformationModel model = noticeBoardHelper.GetGeneralInformationDetail(GeneralInformationId);
            //return AwsS3Bucket.DownloadObject(model.UploadFile, FilePath);
           
        }

        #endregion

        #region Maintenance Schedule

        // GET: Notice Board meetings Schedule 
        public ActionResult MaintenanceSchedule()
        {
            int BlockSize = 10;
            NoticeBoardModel noticeBoardModel = new NoticeBoardModel();
            noticeBoardModel.MaintenanceScheduleModelList = noticeBoardHelper.GetAllMaintenanceSchedule(1, BlockSize);
            ViewBag.CommonAreaList = CommonData.GetCommonAreaList(ClientSessionData.ClientStrataBoardId);
            return View(noticeBoardModel);
        }

        [ChildActionOnly]
        public ActionResult ListMaintenanceSchedule(List<MaintenanceScheduleModel> model)
        {
            return PartialView(model);
        }

        // POST: GeneralInformationInfinateScroll
        [HttpPost]
        public ActionResult MaintenanceInfinateScroll(int BlockNumber)
        {
            int BlockSize = 10;
            var MaintenanceScheduleModel = noticeBoardHelper.GetAllMaintenanceSchedule(BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = MaintenanceScheduleModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListMaintenanceSchedule", MaintenanceScheduleModel);
            return Json(jsonModel);
        }

        // GET: AddMaintenanceSchedule
        [HttpGet]
        public ActionResult AddMaintenanceSchedule()
        {
            MaintenanceScheduleModel model = new MaintenanceScheduleModel();
            return PartialView("AddMaintenanceSchedule", model);
        }

        // POST: AddMaintenanceSchedule
        [HttpPost]
        public ActionResult AddMaintenanceSchedule(MaintenanceScheduleModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = noticeBoardHelper.AddUpdateMaintenanceSchedule(model);
                if (result == -1)
                {
                    strMsg = "Maintenance Schedule already exists for Common Area with given date";
                }
                else if (result == 1)
                {
                    strMsg = "Maintenance Schedule created successfully.";
                }
                else
                {
                    strMsg = "Maintenance Schedule creation failed.";
                }
            }
            return Json(new { result = result, Msg = strMsg });

        }

        // GET: EditMaintenanceSchedule
        [HttpGet]
        public ActionResult EditMaintenanceSchedule(int MaintenanceScheduleId)
        {
            MaintenanceScheduleModel model = noticeBoardHelper.GetMaintenanceScheduleDetail(MaintenanceScheduleId);
            ViewBag.CommonAreaList = CommonData.GetCommonAreaList(ClientSessionData.ClientStrataBoardId);
            return PartialView("EditMaintenanceSchedule", model);
        }

        // POST: EditMaintenanceSchedule
        [HttpPost]
        public ActionResult EditMaintenanceSchedule(MaintenanceScheduleModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = noticeBoardHelper.AddUpdateMaintenanceSchedule(model);
                if (result == -1)
                {
                    strMsg = "Maintenance Schedule already exists for Common Area with given date";
                }
                else if (result == 1)
                {
                    strMsg = "Maintenance Schedule updated successfully.";
                }
                else
                {
                    strMsg = "Maintenance Schedule updation failed.";
                }
            }
            return Json(new { result = result, Msg = strMsg });
        }

        // POST: DeleteScheduleMeeting
        [HttpPost]
        public JsonResult DeleteMaintenanceSchedule(int MaintenanceScheduleId)
        {
            int result = noticeBoardHelper.DeleteMaintenanceSchedule(MaintenanceScheduleId);
            if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Maintenance Schedule has been deleted successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Maintenance Schedule deletion failed.");
            }
            return Json(new { success = true });
        }

        #endregion
    }
}