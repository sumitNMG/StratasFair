using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Common;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace StratasFair.Web.Controllers.Admin
{
    [ClientSessionExpire]
    [StrataBoardAdmin]
    public class ManageStrataOwnerController : Controller
    {
        StrataOwnerHelper strataOwnerHelper = new StrataOwnerHelper();

        // GET: Index
        [HttpGet]
        public ActionResult Index()
        {
            int BlockSize = 12;
            List<StrataOwnerModel> StrataOwnerModelList = strataOwnerHelper.GetAllStrataOwnerDetails(1, BlockSize, string.Empty, string.Empty, string.Empty);
            return View(StrataOwnerModelList);
        }


        [ChildActionOnly]
        public PartialViewResult ListStrataOwner(List<StrataOwnerModel> model)
        {
            return PartialView("ListStrataOwner", model);
        }

        [HttpPost]
        public ActionResult InfinateScroll(int BlockNumber, string ByFirstName, string ByLastName, string ByEmail)
        {
            int BlockSize = 12;
            var strataOwnerModel = strataOwnerHelper.GetAllStrataOwnerDetails(BlockNumber, BlockSize, ByFirstName, ByLastName, ByEmail);
            JsonModel jsonModel = new JsonModel();
            if (strataOwnerModel != null)
            {
                if (strataOwnerModel.Count() > 0)
                {
                    jsonModel.NoMoreData = strataOwnerModel.Count < BlockSize;
                    jsonModel.HTMLString = RenderPartialViewToString("ListStrataOwner", strataOwnerModel);
                }
            }
            return Json(jsonModel);
        }

        // GET: AddStrataOwner
        [HttpGet]
        public ActionResult AddStrataOwner()
        {
            StrataOwnerModel strataOwnerModel = new StrataOwnerModel();
            return PartialView("AddStrataOwner", strataOwnerModel);
        }

        // POST: AddStrataOwner
        [HttpPost]
        public ActionResult AddStrataOwner(StrataOwnerModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = strataOwnerHelper.AddUpdate(model);
                if (result == -1)
                {
                    strMsg = "Email already exits. Please use another Email.";
                }
                else if (result == 0)
                {
                    strMsg = "Strata Owner has been created successfully.";
                }
                else if (result == -3)
                {
                    strMsg = "No. of Subscription of User of this StrataBoard Exceeds";
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Strata Owner creation failed.");
                    strMsg = "Strata Owner creation failed.";
                }
            }
            else
            {
                strMsg = "Please enter all mandatory fields with*";
            }
            return Json(new { result = result, Msg = strMsg });
        }

        // GET: BulkUploadStrataOwner
        [HttpGet]
        public ActionResult BulkUploadStrataOwner()
        {
            return View("BulkUploadStrataOwner");
        }

        // POST: BulkUploadStrataOwner
        [HttpPost]
        public JsonResult BulkUploadStrataOwner(HttpPostedFileBase FileUpload)
        {
            int result = 0;
            string strMsg = "";
            try
            {
                if (FileUpload != null)
                {
                    StrataOwnerModel strataboardModel = new StrataOwnerModel();
                    DataTable dt = new DataTable();
                    if (FileUpload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(FileUpload.FileName);
                        string PhysicalPath = "~/Content/Resources/BulkUpload/";
                        if (!Directory.Exists(Server.MapPath(PhysicalPath)))
                        {
                            Directory.CreateDirectory(Server.MapPath(PhysicalPath));
                        }
                        string path = Path.Combine(Server.MapPath("~/Content/Resources/BulkUpload/"), fileName);

                        string extension = Path.GetExtension(FileUpload.FileName);
                        string savedFileName = "~/Content/Resources/BulkUpload/" + fileName;
                        FileUpload.SaveAs(path);
                        if (extension == ".xls" || extension == ".xlsx" || extension == ".csv")
                        {
                            if (extension == ".xls" || extension == ".xlsx")
                            {
                                //The below code reads the excel file.

                                var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", Server.MapPath(savedFileName));
                                var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                                var ds = new DataSet();
                                adapter.Fill(ds, "results");
                                dt = ds.Tables["results"];
                            }
                            else if (extension == ".csv")
                            {
                                dt = ProcessCSV(path);
                            }
                            foreach (DataRow row in dt.Rows)
                            {
                                strataboardModel.FirstName = row["First Name"].ToString();
                                strataboardModel.LastName = row["Last Name"].ToString();
                                strataboardModel.Email = row["Email Address"].ToString();
                                strataboardModel.UnitNumber = row["Unit Number"].ToString();
                                //Data saved to database.
                                if (!string.IsNullOrEmpty(strataboardModel.FirstName) && !string.IsNullOrEmpty(strataboardModel.LastName) && !string.IsNullOrEmpty(strataboardModel.Email) && !string.IsNullOrEmpty(strataboardModel.UnitNumber))
                                {
                                    strataOwnerHelper.AddUpdate(strataboardModel);
                                    result++;
                                }
                            }
                            if (result > 0)
                            {
                                int fileMapped = AwsS3Bucket.CreateFile("resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/ImportUsers/" + FileUpload.FileName, path);
                                System.IO.File.Delete(path);
                                strMsg = "Bulk upload of strata owners are successfully saved.";
                            }
                            else
                            {
                                System.IO.File.Delete(path);
                                strMsg = "Uploaded file is empty. please fill data before upload.";
                            }
                        }
                        else
                        {
                            strMsg = "Please upload only Excel (.xls, .xlsx) OR Csv(.csv) File";
                        }
                    }
                    else
                    {
                        strMsg = "Uploaded file is empty. please fill data before upload.";
                    }
                }
                else
                {                  
                    strMsg = "Please upload a file before submit.";
                }

            }
            catch (Exception ex)
            {
                strMsg = "Bulk upload failed due to " + ex.Message;
            }
            return Json(new { counter = result, msg = strMsg }, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public FileResult DownloadImportUserFile(string type)
        {

            string path = "";
            if (type == "samplefile")
            {
                path = "~/content/resources/importuser/sample-file-importuser.xlsx";
                return File(Path.Combine(Server.MapPath(path)), MimeMapping.GetMimeMapping("sample-file-importuser.xlsx"), System.IO.Path.GetFileName(path));
            }
            else if (type == "samplefilecsv")
            {
                path = "~/content/resources/importuser/sample-file-importuser.csv";
                return File(Path.Combine(Server.MapPath(path)), MimeMapping.GetMimeMapping("sample-file-importuser.csv"), System.IO.Path.GetFileName(path));
            }
            else
            {
                return null;
            }
        }


        // POST: CancelBulkUploadStrataOwner
        [HttpPost]
        public ActionResult CancelBulkUploadStrataOwner()
        {
            return RedirectToAction("index");
        }


        private DataTable ProcessCSV(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }

        // GET: ViewStrataOwner
        [HttpGet]
        public PartialViewResult ViewStrataOwner(int StrataOwnerId)
        {
            StrataOwnerModel strataOwnerModel = strataOwnerHelper.GetStrataOwnerDetails(StrataOwnerId);
            return PartialView("ViewStrataOwner", strataOwnerModel);
        }

        // GET: EditSubAdmin
        [HttpGet]
        public PartialViewResult EditStrataOwner(int StrataOwnerId)
        {
            StrataOwnerModel strataOwnerModel = strataOwnerHelper.GetStrataOwnerDetails(StrataOwnerId);
            return PartialView("EditStrataOwner", strataOwnerModel);
        }

        // POST: EditStrataOwner
        [HttpPost]
        public ActionResult EditStrataOwner(StrataOwnerModel model)
        {
            int result = 0;
            string strMsg = "";
            if (ModelState.IsValid)
            {
                result = strataOwnerHelper.AddUpdate(model);

                if (result == 0)
                {
                    strMsg = "Strata Owner has been updated successfully.";
                }
                else
                {
                    strMsg = "Strata Owner updation failed.";
                }
            }
            else
            {
                strMsg = "Please enter all mandatory fields with*";
            }
            return Json(new { result = result, Msg = strMsg });
        }

        // POST: DeleteStrataOwner
        [HttpPost]
        public JsonResult DeleteStrataOwner(int StrataOwnerId)
        {
            int result = strataOwnerHelper.DeleteStrataOwner(StrataOwnerId);
            if (result == 0)
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Strata Owner has been deleted successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Strata Owner deletion failed.");
            }
            return Json(new { success = true });
        }

        // POST: ActivateDeActivateStrataOwner
        [HttpPost]
        public JsonResult ActivateDeActivateStrataOwner(int StrataOwnerId, int IsActive)
        {
            int result = strataOwnerHelper.ActivateDeActivateStrataOwner(StrataOwnerId, IsActive);
            if (result == 0)
            {
                if (IsActive == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Strata Owner has been DeActivated successfully.");
                }
                else if (IsActive == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setFrontendMessage(0, "Strata Owner has been Activated successfully.");
                }
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setFrontendMessage(2, "Strata Owner deletion failed.");
            }
            return Json(new { success = true });
        }

        // POST: ResendCredentialsMail
        public JsonResult ResendCredentialsMail(int StrataOwnerId)
        {
            string strMsg = string.Empty;
            string result = EmailSender.FncSend_StratasBoard_RegistrationMail_ToStrataOwnerClient(StrataOwnerId);
            if (result == "success")
            {
                strMsg = "Credentials has been sent successfully.";
            }
            else
            {
                strMsg = "Credentials has not been sent.";
            }
            return Json(new { result = result, Msg = strMsg }, JsonRequestBehavior.AllowGet);
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
    }
}