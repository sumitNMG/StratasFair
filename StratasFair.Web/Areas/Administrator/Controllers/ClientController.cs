using iTextSharp.text;
using iTextSharp.text.pdf;
using StratasFair.Business;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    [AdminSessionExpire]
    public class ClientController : Controller
    {
        // GET: Administrator/Client
        public ActionResult Index()
        {
            StrataBoardModel strataBoardModel = new StrataBoardModel();
            return BindList(strataBoardModel);
        }

        List<StrataBoardModel> ActiveList = new List<StrataBoardModel>();
        List<StrataBoardModel> InActiveList = new List<StrataBoardModel>();

        private ActionResult BindList(StrataBoardModel strataBoardModel)
        {
            StrataBoardHelper _Helper = new StrataBoardHelper();
            var List = _Helper.GetAll(strataBoardModel);
            ActiveList = List.Where(x => x.Status == 1).ToList();
            InActiveList = List.Where(x => x.Status == 0).ToList();
            return View(Tuple.Create(ActiveList, InActiveList, strataBoardModel));
        }

        [HttpPost]
        public ActionResult Index(StrataBoardModel strataBoardModel)
        {
            ModelState["ExpiredOnTo"].Errors.Clear();
            ModelState["ExpiredOnFrom"].Errors.Clear();
            ModelState["Keyword"].Errors.Clear();
            return BindList(strataBoardModel);
        }



        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.StatusList = AppLogic.BindDDStatus(1);
            StrataBoardModel strataBoardModel = new StrataBoardModel();
            strataBoardModel.Status = 1;
            return View(strataBoardModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(StrataBoardModel strataBoardModel)
        {
            if (ModelState.IsValid)
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
                long stratasBoardId = strataBoardHelper.AddUpdate(strataBoardModel);
                if (stratasBoardId > 0)
                {
                    // stratas board admin added successfully
                    try
                    {
                        // send mail to stratas board admin with auto generate password.
                        string result = EmailSender.FncSend_StratasBoard_RegistrationMail_ToClient(stratasBoardId);
                        if (result == "success")
                        {
                            TempData["CommonMessage"] = AppLogic.setMessage(0, "Record added successfully.");
                        }
                        else
                        {
                            TempData["CommonMessage"] = AppLogic.setMessage(2, "Error, while sending email to stratasboard administrator. Resend mail again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        new AppError().LogMe(ex);
                    }
                   
                    return RedirectToAction("Index");
                }
                else if (stratasBoardId == -4)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Stratasboard name already exists.");
                    ViewBag.StatusList = AppLogic.BindDDStatus(1);
                    return View(strataBoardModel);
                }
                else if (stratasBoardId == -5)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Unique name already exists.");
                    ViewBag.StatusList = AppLogic.BindDDStatus(1);
                    return View(strataBoardModel);
                }
                else if (stratasBoardId == -6)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Email address already exists.");
                    ViewBag.StatusList = AppLogic.BindDDStatus(1);
                    return View(strataBoardModel);
                }
                else
                {
                    ViewBag.StatusList = AppLogic.BindDDStatus(1);
                    TempData["CommonMessage"] = AppLogic.setMessage(2, "Error, Please try again.");
                    return View(strataBoardModel);
                }

            }

            ViewBag.StatusList = AppLogic.BindDDStatus(1);
            return View(strataBoardModel);
        }



        [HttpGet]
        public ActionResult Edit(long? Id)
        {

            StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
            StrataBoardModel strataBoardModel = new StrataBoardModel();
            strataBoardModel = strataBoardHelper.GetStratasBoardByID(Id.Value);

            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(strataBoardModel.Status));
            return View(strataBoardModel);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(StrataBoardModel strataBoardModel)
        {

            ModelState.Remove("SubscriptionId");

            if (ModelState.IsValid)
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
                long stratasBoardId = strataBoardHelper.AddUpdate(strataBoardModel);

                if (stratasBoardId > 0)
                {

                    // send mail to stratas board admin with updated unique URL.
                    if (strataBoardModel.OldPortalLink.Trim() != strataBoardModel.PortalLink.Trim())
                    {
                        string result = EmailSender.FncSend_StratasBoard_UniqueURLUpdate_ToClient(stratasBoardId);
                    }

                    TempData["CommonMessage"] = AppLogic.setMessage(0, "Record updated successfully.");
                    return RedirectToAction("Index");
                }
                else if (stratasBoardId == -4)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Stratasboard name already exists.");
                }
                else if (stratasBoardId == -5)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Unique name already exists.");
                }
                else if (stratasBoardId == -6)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Email address already exists.");
                }
                else if (stratasBoardId == -7)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Unique name can't be updated.");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(2, "Error, Please try again.");
                }
            }
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(strataBoardModel.Status));
            return View(strataBoardModel);
        }


        public ActionResult Active(long Id)
        {

            StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
            StrataBoardModel strataBoardModel = new StrataBoardModel();
            strataBoardModel = strataBoardHelper.GetStratasBoardByID(Id);
            strataBoardModel.Status = 1;
            int count = strataBoardHelper.ActDeact(strataBoardModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record activated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record activation failed.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Deactive(long Id)
        {
            StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
            StrataBoardModel strataBoardModel = new StrataBoardModel();
            strataBoardModel = strataBoardHelper.GetStratasBoardByID(Id);
            strataBoardModel.Status = 0;
            int count = strataBoardHelper.ActDeact(strataBoardModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deactivated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deactivation failed.");
            }
            return RedirectToAction("Index");

        }


        public ActionResult Delete(long Id)
        {
            StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
            StrataBoardModel strataBoardModel = new StrataBoardModel();
            strataBoardModel = strataBoardHelper.GetStratasBoardByID(Id);

            int count = strataBoardHelper.Delete(strataBoardModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deleted successfully.");
            }
            else if (count == -2)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record can't be deleted as other users exists.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deletion failed.");
            }
            return RedirectToAction("Index");
        }


        public ActionResult ClientExportToCsv(string dfrom, string dto, string kw)
        {
            try
            {
                ConvertToCSV objCSV = new ConvertToCSV();

                string FilePath = "~/Content/Resources/Reports/csvData.csv";
                objCSV.fncDeleteFile(FilePath);

                StrataBoardHelper objLedgerHelper = new StrataBoardHelper();
                StrataBoardModel objLedgerModel = new StrataBoardModel
                {
                    PageNumber = 1,
                    PageSize = 100000000
                };

                objLedgerModel.ExpiredOnFrom = DateTime.Parse(dfrom);
                objLedgerModel.ExpiredOnTo = DateTime.Parse(dto);
                objLedgerModel.Keyword = kw;


                List<StrataBoardModel> listLedgerModel = objLedgerHelper.GetAll(objLedgerModel);

                DataTable dt = new DataTable();
                dt.Columns.Add("Stratasboard name");
                dt.Columns.Add("Unique name");
                dt.Columns.Add("Name");
                dt.Columns.Add("Email address");

                dt.Columns.Add("Contact number");
                dt.Columns.Add("Created on");
                dt.Columns.Add("Subscription name");
                dt.Columns.Add("Validity");
                dt.Columns.Add("Expiration date");
                dt.Columns.Add("Allowed user");
                dt.Columns.Add("IsSmsForAlert");
                dt.Columns.Add("IsSmsForReminder");
                dt.Columns.Add("Status");

                if (listLedgerModel.Count() != 0)
                {
                    foreach (var item in listLedgerModel)
                    {
                        DataRow row = dt.NewRow();
                        row["Stratasboard name"] = item.StratasBoardName;
                        row["Unique name"] = item.PortalLink;
                        row["Name"] = item.FirstName + " " + item.LastName;
                        row["Email address"] = item.EmailId;

                        row["Contact number"] = item.ContactNumber;
                        row["Created on"] = AppLogic.GetDateFromatForCsv(Convert.ToDateTime(item.CreatedOn));
                        row["Subscription name"] = item.SubscriptionName;
                        row["Validity"] = item.ValidityText;
                        row["Expiration date"] = AppLogic.GetDateFromatForCsv(Convert.ToDateTime(item.ExpiryDate));
                        row["Allowed user"] = item.AllowedUser;
                        row["IsSmsForAlert"] = AppLogic.GetYesNo(item.IsSmsForAlert);
                        row["IsSmsForReminder"] = AppLogic.GetYesNo(item.IsSmsForReminder);
                        row["Status"] = AppLogic.GetStatus(item.Status.ToString());
                        dt.Rows.Add(row);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    objCSV.CreateCSVFile(dt, Server.MapPath("~/Content/Resources/Reports/") + "csvData.csv");

                    objCSV.FncDownLoadFiles("stratasboard_" + DateTime.Now.ToShortDateString() + ".csv", FilePath);
                }


            }
            catch (Exception Ex)
            {
                string Err = Ex.Message.ToString();
            }
            return Redirect(Url.Content(ConfigurationManager.AppSettings["WebsiteRootPath"] + "securehost/rootlogin/client"));
        }

        public ActionResult ClientExportToPdf(string dfrom, string dto, string kw)
        {
            try
            {

                ConvertToPdf convertToPdf = new ConvertToPdf();
                string filename = "stratasboard_" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + ".pdf";
                string FilePath = "~/Content/Resources/Reports/" + filename;
                convertToPdf.FncDeleteFile(FilePath);

                StrataBoardHelper objLedgerHelper = new StrataBoardHelper();
                StrataBoardModel objLedgerModel = new StrataBoardModel
                {
                    PageNumber = 1,
                    PageSize = 100000000
                };

                objLedgerModel.ExpiredOnFrom = DateTime.Parse(dfrom);
                objLedgerModel.ExpiredOnTo = DateTime.Parse(dto);
                objLedgerModel.Keyword = kw;

                List<StrataBoardModel> listLedgerModel = objLedgerHelper.GetAll(objLedgerModel);

                DataTable dt = new DataTable();
                dt.Columns.Add("Stratasboard name");
                dt.Columns.Add("Unique name");
                dt.Columns.Add("Name");
                dt.Columns.Add("Email address");
                dt.Columns.Add("Contact number");
                dt.Columns.Add("Created on");
                dt.Columns.Add("Subscription name");
                dt.Columns.Add("Validity");
                dt.Columns.Add("Expiration date");
                dt.Columns.Add("Allowed user");
                dt.Columns.Add("IsSmsForAlert");
                dt.Columns.Add("IsSmsForReminder");
                dt.Columns.Add("Status");

                if (listLedgerModel.Count() != 0)
                {
                    foreach (var item in listLedgerModel)
                    {
                        DataRow row = dt.NewRow();
                        row["Stratasboard name"] = item.StratasBoardName;
                        row["Unique name"] = item.PortalLink;
                        row["Name"] = item.FirstName + " " + item.LastName;
                        row["Email address"] = item.EmailId;
                        row["Contact number"] = item.ContactNumber;
                        row["Created on"] = AppLogic.GetDateFromatForCsv(Convert.ToDateTime(item.CreatedOn));
                        row["Subscription name"] = item.SubscriptionName;
                        row["Validity"] = item.ValidityText;
                        row["Expiration date"] = AppLogic.GetDateFromatForCsv(Convert.ToDateTime(item.ExpiryDate));
                        row["Allowed user"] = item.AllowedUser;
                        row["IsSmsForAlert"] = AppLogic.GetYesNo(item.IsSmsForAlert);
                        row["IsSmsForReminder"] = AppLogic.GetYesNo(item.IsSmsForReminder);
                        row["Status"] = AppLogic.GetStatus(item.Status.ToString());
                        dt.Rows.Add(row);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    FileStream fs = new FileStream(Server.MapPath(FilePath), FileMode.Create, FileAccess.Write, FileShare.None);

                    iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(PageSize.A4);
                    //Rectangle rec = new Rectangle(PageSize.A4.Rotate());
                    //rec.BackgroundColor = new BaseColor(System.Drawing.Color.WhiteSmoke);
                    //rec2.BackgroundColor = new CMYKColor(25, 90, 25, 0);


                    //72 points = 1 inch
                    //Left Margin: 36pt => 0.5 inch
                    //Right Margin: 72pt => 1 inch
                    //Top Margin: 108pt => 1.5 inch
                    //Bottom Margini: 180pt => 2.5 inch


                    Document doc = new Document(rec, 18, 18, 72, 72);
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                    doc.Open();
                    doc.AddTitle("stratasboard listing");
                    doc.AddCreator("stratafair");
                    //doc.AddSubject("This is an Example 4 of Chapter 1 of Book 'iText in Action'");
                    //doc.AddKeywords("Metadata, iTextSharp 5.4.4, Chapter 1, Tutorial");
                    //doc.AddAuthor("pankaj jain");
                    //doc.AddHeader("Nothing", "No Header");


                    //Paragraph para = new Paragraph("Hello World");
                    // Setting paragraph's text alignment using iTextSharp.text.Element class
                    // para.Alignment = Element.ALIGN_JUSTIFIED;
                    // Adding this 'para' to the Document object
                    // doc.Add(para);

                    PdfPTable table = new PdfPTable(13);

                    table.HorizontalAlignment = 0;
                    table.TotalWidth = 550f;
                    table.LockedWidth = true;

                    // Font fontHeader = FontFactory.GetFont("ARIAL", 8);


                    // sum must equal to Total Width of table
                    // number of sizes must equal to number of columns
                    float[] widths = new float[] { 80f, 80f, 50f, 80f, 50f, 80f, 80f, 40f, 50f, 50f, 50f, 40f, 40f };
                    table.SetWidths(widths);


                    int iColCount = dt.Columns.Count;
                    for (int i = 0; i < iColCount; i++)
                    {
                        //PdfPCell pdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[i].ToString(), fontHeader)));
                        //table.AddCell(pdfPCell);
                        ConvertToPdf.AddHeaderCell(table, dt.Columns[i].ToString(), 1, 7, "C", "C");
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < iColCount; i++)
                        {
                            ConvertToPdf.AddCell(table, dr[i].ToString(), 1, 7, "C", "C");
                        }
                    }


                    doc.Add(table);
                    doc.Close();
                    convertToPdf.FncDownLoadPDFFile(FilePath);

                }
            }
            catch (Exception Ex)
            {
                string Err = Ex.Message.ToString();
            }

            return Redirect(Url.Content(ConfigurationManager.AppSettings["WebsiteRootPathAdmin"] + "securehost/rootlogin/client"));
        }


        public JsonResult SendMessage(string ReceipientEmailId, long StratasBoardId, string Message)
        {
            string strMsg = "NotOk";
            string strData = "";
            try
            {
                // mail will go to strata registered email id, admin email id.
                string result = EmailSender.FncSendReminderMailToClient(ReceipientEmailId, CommonData.GetAdminEmailForNotification(), StratasBoardId, Message);

                if (result == "success")
                {
                    strMsg = "Ok";
                    strData = "Message sent successfully.";
                }
                else
                {
                    strData = "Error, Please try again." + result;
                }
            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                strData = "Error, Please try again.";
            }
            return Json(new { msg = strMsg, data = strData }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListSubscription(long? id)
        {
            SubscriptionModel subscriptionModel = new SubscriptionModel();
            subscriptionModel.StratasBoardId = id.Value;
            BindSubscriptionList(subscriptionModel);
            return View();
        }


        List<SubscriptionModel> SubscriptionActiveList = new List<SubscriptionModel>();
        private ActionResult BindSubscriptionList(SubscriptionModel subscriptionModel)
        {
            StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
            var List = strataBoardHelper.GetStrataBoardSubscription(subscriptionModel.StratasBoardId);
            SubscriptionActiveList = List.OrderByDescending(x => x.Status).ToList();
            return View(Tuple.Create(SubscriptionActiveList, subscriptionModel));
        }


        public ActionResult AddSubscription(long? Id)
        {
            RenewSubscriptionModel subscriptionModel = new RenewSubscriptionModel();
            subscriptionModel.StratasBoardId = Id.Value;
            return View(subscriptionModel);
        }


        [HttpPost]
        public ActionResult AddSubscription(RenewSubscriptionModel renewSubscriptionModel)
        {
            if (ModelState.IsValid)
            {
                StrataBoardHelper strataBoardHelper = new StrataBoardHelper();
                int stratasBoardSubscriptionId = strataBoardHelper.RenewSubscription(renewSubscriptionModel);
                if (stratasBoardSubscriptionId > 0)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(0, "Record updated successfully.");
                    return RedirectToAction("listsubscription/" + renewSubscriptionModel.StratasBoardId);
                }
                else if (stratasBoardSubscriptionId == -1)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(2, "StratasBoard Users are more than subscription package allowed user. select another subscription.");
                    return View(renewSubscriptionModel);
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(2, "Error, Please try again.");
                    return View(renewSubscriptionModel);
                }
            }
            return View(renewSubscriptionModel);
        }

        public JsonResult GetSubscriptionDetail(int Id)
        {
            SubscriptionModel subscriptionModel = new SubscriptionModel();
            SubscriptionHelper subscriptionHelper = new SubscriptionHelper();
            subscriptionModel = subscriptionHelper.GetByID(Id);
            subscriptionModel.SubscriptionWithValidity = AppLogic.CalculateSubscriptionExpiryDate(DateTime.UtcNow, subscriptionModel.Validity).ToString("dd MMM yyyy");
            return Json(subscriptionModel, JsonRequestBehavior.AllowGet);
        }
      
    }
}