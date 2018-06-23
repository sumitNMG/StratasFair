using StratasFair.Business.CommonHelper;
using StratasFair.Business.Admin;
using StratasFair.BusinessEntity.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StratasFair.Web.App_Start;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    [AdminSessionExpire]
    public class ReportController : Controller
    {
        // GET: Administrator/Report
        public ActionResult Index()
        {
            return View();
        }

        #region [Contact Us]
        public ActionResult ContactUs(ContactUsReportModel model)
        {
            return BindContactUsReport(model);
        }

        private ActionResult BindContactUsReport(ContactUsReportModel model)
        {
            try
            {
                model.PageNumber = model.PageNumber == null ? 1 : model.PageNumber;
            }
            catch { model.PageNumber = 1; }
            model.PageSize = Convert.ToInt32(StratasFair.Business.CommonHelper.CommonData.FetchSettingValue("Admin.PageSize.Long")); // 2

            return View(model);
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUsReportModel model, int? id)
        {
            model.PageNumber = 1;
            return BindContactUsReport(model);
        }


        public ActionResult ContactUsExportToCsv()
        {
            try
            {
                ConvertToCSV objCSV = new ConvertToCSV();

                string FilePath = "~/Content/Resources/Reports/csvData.csv";
                objCSV.fncDeleteFile(FilePath);

                ReportsHelper objLedgerHelper = new ReportsHelper();
                ContactUsReportModel objLedgerModel = new ContactUsReportModel
                {
                    PageNumber = 1,
                    PageSize = 100000000
                };

                List<ContactUsReportModel> listLedgerModel = objLedgerHelper.GetContactUsReport(objLedgerModel);

                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("Email Address");
                dt.Columns.Add("Message");
                dt.Columns.Add("Created On");
            

                if (listLedgerModel.Count() != 0)
                {
                    foreach (var item in listLedgerModel)
                    {
                        DataRow row = dt.NewRow();
                        row["Name"] = item.FirstName;
                        row["Email Address"] = item.EmailAddress;
                        row["Message"] = item.Message;
                        row["Created On"] = StratasFair.Business.CommonHelper.AppLogic.GetDateFromatForCsv(item.CreatedOn);
                        dt.Rows.Add(row);
                    }
                }



                if (dt.Rows.Count > 0)
                {
                    objCSV.CreateCSVFile(dt, Server.MapPath("~/Content/Resources/Reports/") + "csvData.csv");

                    objCSV.FncDownLoadFiles("ContactUs_" + DateTime.Now.ToShortDateString() + ".csv", FilePath);
                }


            }
            catch (Exception Ex)
            {
                string Err = Ex.Message.ToString();
            }
            return Redirect(Url.Content("~/securehost/rootlogin/report/contactus"));
        }
        #endregion

        #region [Expired Membership]
        public ActionResult ExpiredMembership(ExpiredMembershipReportModel model)
        {
            return BindExpiredMembershipReport(model);
        }

        private ActionResult BindExpiredMembershipReport(ExpiredMembershipReportModel model)
        {
            try
            {
                model.PageNumber = model.PageNumber == null ? 1 : model.PageNumber;
            }
            catch { model.PageNumber = 1; }
            model.PageSize = Convert.ToInt32(CommonData.FetchSettingValue("Admin.PageSize.Long")); // 2

            return View(model);
        }

        [HttpPost]
        public ActionResult ExpiredMembership(ExpiredMembershipReportModel model, int? id)
        {
            model.PageNumber = 1;
            return BindExpiredMembershipReport(model);
        }


        public ActionResult ExpiredMembershipExportToCsv()
        {
            try
            {
                ConvertToCSV objCSV = new ConvertToCSV();

                string FilePath = "~/Content/Resources/Reports/csvData.csv";
                objCSV.fncDeleteFile(FilePath);

                ReportsHelper objLedgerHelper = new ReportsHelper();
                ExpiredMembershipReportModel objLedgerModel = new ExpiredMembershipReportModel
                {
                    PageNumber = 1,
                    PageSize = 100000000
                };

                List<ExpiredMembershipReportModel> listLedgerModel = objLedgerHelper.GetExpiredMembershipReport(objLedgerModel);
                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("Email address");
                dt.Columns.Add("Strata name");
                dt.Columns.Add("Contact number");
                dt.Columns.Add("Register on");
                dt.Columns.Add("Unique name");
                dt.Columns.Add("Subscription expired on");

                if (listLedgerModel.Count() != 0)
                {
                    foreach (var item in listLedgerModel)
                    {
                        DataRow row = dt.NewRow();
                        row["Name"] = item.FirstName + " " + item.LastName;
                        row["Email address"] = item.EmailId;
                        row["Strata name"] = item.StratasBoardName;
                        row["Contact number"] = item.ContactNumber;
                        row["Register on"] = AppLogic.GetDateFromatForCsv(item.CreatedOn);
                        row["Unique name"] = item.PortalLink;
                        row["Subscription expired on"] = AppLogic.GetDateFromatForCsv(item.ExpiryDate);
                        dt.Rows.Add(row);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    objCSV.CreateCSVFile(dt, Server.MapPath("~/Content/Resources/Reports/") + "csvData.csv");
                    objCSV.FncDownLoadFiles("ExpiredMembership_" + DateTime.Now.ToShortDateString() + ".csv", FilePath);
                }
            }
            catch (Exception Ex)
            {
                string Err = Ex.Message.ToString();
            }
            return Redirect(Url.Content("~/securehost/rootlogin/report/expiredmembership"));
        }
        #endregion

        #region [Upcoming Renewal]
        public ActionResult UpcomingRenewal(UpcomingRenewalReportModel model)
        {
            return BindUpcomingRenewalReport(model);
        }

        private ActionResult BindUpcomingRenewalReport(UpcomingRenewalReportModel model)
        {
            try
            {
                model.PageNumber = model.PageNumber == null ? 1 : model.PageNumber;
            }
            catch { model.PageNumber = 1; }
            model.PageSize = Convert.ToInt32(CommonData.FetchSettingValue("Admin.PageSize.Long")); // 2

            return View(model);
        }

        [HttpPost]
        public ActionResult UpcomingRenewal(UpcomingRenewalReportModel model, int? id)
        {
            model.PageNumber = 1;
            return BindUpcomingRenewalReport(model);
        }


        public ActionResult UpcomingRenewalExportToCsv()
        {
            try
            {
                ConvertToCSV objCSV = new ConvertToCSV();

                string FilePath = "~/Content/Resources/Reports/csvData.csv";
                objCSV.fncDeleteFile(FilePath);

                ReportsHelper objLedgerHelper = new ReportsHelper();
                UpcomingRenewalReportModel objLedgerModel = new UpcomingRenewalReportModel
                {
                    PageNumber = 1,
                    PageSize = 100000000
                };

                List<UpcomingRenewalReportModel> listLedgerModel = objLedgerHelper.GetUpcomingRenewalReport(objLedgerModel);
                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("Email address");
                dt.Columns.Add("Strata name");
                dt.Columns.Add("Contact number");
                dt.Columns.Add("Register on");
                dt.Columns.Add("Unique name");
                dt.Columns.Add("Subscription expiring on");
                if (listLedgerModel.Count() != 0)
                {
                    foreach (var item in listLedgerModel)
                    {
                        DataRow row = dt.NewRow();
                        row["Name"] = item.FirstName + " " + item.LastName;
                        row["Email address"] = item.EmailId;
                        row["Strata name"] = item.StratasBoardName;
                        row["Contact number"] = item.ContactNumber;
                        row["Register on"] = AppLogic.GetDateFromatForCsv(item.CreatedOn);
                        row["Unique name"] = item.PortalLink;
                        row["Subscription expiring on"] = AppLogic.GetDateFromatForCsv(item.ExpiryDate);
                        dt.Rows.Add(row);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    objCSV.CreateCSVFile(dt, Server.MapPath("~/Content/Resources/Reports/") + "csvData.csv");
                    objCSV.FncDownLoadFiles("UpcomingRenewal_" + DateTime.Now.ToShortDateString() + ".csv", FilePath);
                }
            }
            catch (Exception Ex)
            {
                string Err = Ex.Message.ToString();
            }
            return Redirect(Url.Content("~/securehost/rootlogin/report/upcomingrenewal"));
        }
        #endregion
    }
}