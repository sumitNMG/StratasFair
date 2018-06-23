using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using StratasFair.Business.CommonHelper;
using System.Data;
using System.Web;
using StratasFair.BusinessEntity.Admin;
using StratasFair.BusinessEntity.Front;

namespace StratasFair.Business.Front
{
    public class StrataOwnerHelper
    {
        StratasFairDBEntities _context;
        public string _conString = String.Empty;

        public StrataOwnerHelper()
        {
            if (_context == null)
            {
                _context = new StratasFairDBEntities();
            }
        }

        public int AddUpdate(StrataOwnerModel objectModel)
        {
            if (objectModel.UserClientId > 0)
            {
                return AddUpdateStrataOwner(objectModel);
            }
            else
            {
                if (_context.tblUserClients.Any(x => x.EmailId == objectModel.Email && !x.IsDeleted))
                {
                    // registered emailid already exists
                    return -1;
                }
                else
                {
                    return AddUpdateStrataOwner(objectModel);
                }
            }
        }

        public int AddUpdateStrataOwner(StrataOwnerModel strataOwnerModel)
        {
            int result = 0;
            int userClientId = 0;

            //using (var transaction = _context.Database.BeginTransaction())
            //{
            try
            {
                tblUserClient tblUserClientDb = new tblUserClient();
                tblUserClientDb.FirstName = strataOwnerModel.FirstName;
                tblUserClientDb.LastName = strataOwnerModel.LastName;
                tblUserClientDb.EmailId = strataOwnerModel.Email;
                tblUserClientDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;
                tblUserClientDb.RoleName = "O";
                tblUserClientDb.UnitNumber = strataOwnerModel.UnitNumber;
                tblUserClientDb.CreatedBy = ClientSessionData.UserClientId;
                tblUserClientDb.CreatedOn = DateTime.UtcNow;
                tblUserClientDb.LastLogin = DateTime.UtcNow;
                tblUserClientDb.CurrentLogin = DateTime.UtcNow;
                if (strataOwnerModel.UserClientId == 0)
                {
                    tblUserClientDb.Password = AppLogic.EncryptPassword(); ;
                    tblUserClientDb.IsEmailNotification = false;
                    tblUserClientDb.IsSMSNotification = false;
                    tblUserClientDb.IsProfileComplete = false;
                    tblUserClientDb.Status = 1;  // It will be active in both the cases
                    var AllowedUser = _context.tblStratasBoardSubscriptions.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId).FirstOrDefault().AllowedUser;

                    var AddedUserClients = _context.tblUserClients.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1);
                    if (AddedUserClients.Count() <= AllowedUser)
                    {
                        _context.tblUserClients.Add(tblUserClientDb);
                        result = _context.SaveChanges();
                    }
                    else
                    {
                        result = -3;
                    }
                    userClientId = Convert.ToInt32(tblUserClientDb.UserClientId);
                    if (result == 1)
                    {
                        EmailSender.FncSend_StratasBoard_RegistrationMail_ToStrataOwnerClient(userClientId);
                        result = 0;
                    }
                }
                else
                {
                    tblUserClientDb.UserClientId = strataOwnerModel.UserClientId;
                    _context.tblUserClients.Attach(tblUserClientDb);
                    _context.Entry(tblUserClientDb).Property(x => x.FirstName).IsModified = true;
                    _context.Entry(tblUserClientDb).Property(x => x.LastName).IsModified = true;
                    _context.Entry(tblUserClientDb).Property(x => x.EmailId).IsModified = true;
                    _context.Entry(tblUserClientDb).Property(x => x.UnitNumber).IsModified = true;
                    result = _context.SaveChanges();
                    if (result == 1)
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                result = -2;   // any error is there
            }
            return result;
        }


        public List<StrataOwnerModel> GetAllStrataOwnerDetails(int BlockNumber, int BlockSize, string ByFirstName, string ByLastName, string ByEmail)
        {
            try
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                int result = -1;
                _conString = SqlHelper.GetConnectionString();
                SqlParameter prmStrataBoardID = SqlHelper.CreateParameter("@StrataBoardID", ClientSessionData.ClientStrataBoardId);
                SqlParameter prmType = SqlHelper.CreateParameter("@Type", 5);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmStrataBoardID, prmType, prmErr };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_GetClientUser", allParams);
                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }
                var AllowedUser = _context.tblStratasBoardSubscriptions.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId).FirstOrDefault().AllowedUser;

                var AddedUserClients = _context.tblUserClients.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId);

                List<StrataOwnerModel> StrataOwnerModelList = new List<StrataOwnerModel>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0].Copy();
                    ds.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ByFirstName) || !string.IsNullOrEmpty(ByLastName) || !string.IsNullOrEmpty(ByEmail))
                        {
                            ByFirstName = string.IsNullOrEmpty(ByFirstName) ? " " : ByFirstName;
                            ByLastName = string.IsNullOrEmpty(ByLastName) ? " " : ByLastName;
                            ByEmail = string.IsNullOrEmpty(ByEmail) ? " " : ByEmail;
                            DataRow[] rows = dt.Select("[FirstName] LIKE '" + ByFirstName + "%' OR [LastName] LIKE '" + ByLastName + "%' OR [EmailId] LIKE '" + ByEmail + "%'");
                            dt = rows.CopyToDataTable();
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var strataOwner = new StrataOwnerModel();
                            strataOwner.UserClientId = Convert.ToInt32(dt.Rows[i]["UserClientId"]);
                            strataOwner.FirstName = dt.Rows[i]["FirstName"].ToString();
                            strataOwner.LastName = dt.Rows[i]["LastName"].ToString();
                            strataOwner.Email = dt.Rows[i]["EmailId"].ToString();
                            strataOwner.UnitNumber = dt.Rows[i]["UnitNumber"].ToString();
                            strataOwner.CreatedOn = dt.Rows[i]["CreatedOn"].ToString();
                            strataOwner.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                            if (i == 0)
                            {
                                strataOwner.TotalNoofAccounts = Convert.ToInt32(AllowedUser);
                                strataOwner.TotalAddedAccounts = AddedUserClients.Count();
                                strataOwner.TotalAvailableAccounts = strataOwner.TotalNoofAccounts - strataOwner.TotalAddedAccounts;

                            }
                            StrataOwnerModelList.Add(strataOwner);
                        }
                    }
                    else
                    {
                        var strataOwner = new StrataOwnerModel();
                        strataOwner.TotalNoofAccounts = Convert.ToInt32(AllowedUser);
                        strataOwner.TotalAddedAccounts = AddedUserClients.Count();
                        strataOwner.TotalAvailableAccounts = strataOwner.TotalNoofAccounts - strataOwner.TotalAddedAccounts;

                        StrataOwnerModelList.Add(strataOwner);
                    }
                    StrataOwnerModelList = StrataOwnerModelList.Skip(startIndex).Take(BlockSize).ToList();
                    return StrataOwnerModelList;
                }
                else
                {
                    var strataOwner = new StrataOwnerModel();
                    strataOwner.TotalNoofAccounts = Convert.ToInt32(AllowedUser);
                    strataOwner.TotalAddedAccounts = AddedUserClients.Count();
                    strataOwner.TotalAvailableAccounts = strataOwner.TotalNoofAccounts - strataOwner.TotalAddedAccounts;

                    StrataOwnerModelList.Add(strataOwner);
                    return StrataOwnerModelList;
                }
            }
            catch
            {
                return null;
                throw;
            }
        }

        public StrataOwnerModel GetStrataOwnerDetails(int UserClientId)
        {
            try
            {

                int result = -1;
                _conString = SqlHelper.GetConnectionString();
                SqlParameter prmStrataBoardID = SqlHelper.CreateParameter("@StrataBoardID", ClientSessionData.ClientStrataBoardId);
                SqlParameter prmUserClientId = SqlHelper.CreateParameter("@UserClientId", UserClientId);
                SqlParameter prmType = SqlHelper.CreateParameter("@Type", 6);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmStrataBoardID, prmType, prmErr, prmUserClientId };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_GetClientUser", allParams);
                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0].Copy();
                    ds.Dispose();
                    var StrataOwner = new StrataOwnerModel();
                    StrataOwner.UserClientId = Convert.ToInt32(dt.Rows[0]["UserClientId"]);
                    StrataOwner.FirstName = dt.Rows[0]["FirstName"].ToString();
                    StrataOwner.LastName = dt.Rows[0]["LastName"].ToString();
                    StrataOwner.Email = dt.Rows[0]["EmailId"].ToString();
                    StrataOwner.UnitNumber = dt.Rows[0]["UnitNumber"].ToString();
                    StrataOwner.CreatedOn = dt.Rows[0]["CreatedOn"].ToString();
                    StrataOwner.Status = Convert.ToInt32(dt.Rows[0]["Status"]);
                    return StrataOwner;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public int DeleteStrataOwner(int StrataOwnerId)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    int result = 0;
                    var userClient = _context.tblUserClients.Where(x => x.UserClientId == StrataOwnerId).FirstOrDefault();
                    if (userClient.RoleName == "O")
                    {
                        userClient.IsDeleted = true;
                        _context.tblUserClients.Attach(userClient);
                        _context.Entry(userClient).Property(x => x.IsDeleted).IsModified = true;
                        result = _context.SaveChanges();
                        if (result == 1)
                            result = 0;
                    }
                    return result;
                }
                catch
                {
                    return -1;
                }
            }
            else
            {
                return -2;
            }
        }

        public int ActivateDeActivateStrataOwner(int StrataOwnerId, int IsActive)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    int result = 0;
                    var userClient = _context.tblUserClients.Where(x => x.UserClientId == StrataOwnerId).FirstOrDefault();
                    if (userClient.RoleName == "O")
                    {
                        userClient.Status = IsActive;
                        _context.tblUserClients.Attach(userClient);
                        _context.Entry(userClient).Property(x => x.Status).IsModified = true;
                        result = _context.SaveChanges();
                        if (result == 1)
                            result = 0;
                    }
                    return result;
                }
                catch
                {
                    return -1;
                }
            }
            else
            {
                return -2;
            }
        }

       

        public List<StrataOwnerBookingRequestModel> GetStrataOwnerBookingRequest(int BlockNumber, int BlockSize, bool bIsDashBoard = false)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<StrataOwnerBookingRequestModel> strataOwnerBookingRequestModelList = new List<StrataOwnerBookingRequestModel>();
                try
                {
                    var userBookingRequests = _context.vw_GetOwnerBookingRequest.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId).OrderByDescending(x => x.CreatedOn).ToList();
                    if(bIsDashBoard)
                    {
                        userBookingRequests = _context.vw_GetOwnerBookingRequest.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.AdminStatus != 0).OrderByDescending(x => x.CreatedOn).ToList();
                    }
                    //var userBookingRequests = (from t1 in _context.tblUserBookingRequests
                    //                           join t2 in _context.tblCommonAreas on t1.CommonAreaId equals t2.CommonAreaId
                    //                           where t1.StratasBoardId == ClientSessionData.ClientStrataBoardId && t2.Status == 1 orderby t1.CreatedOn descending
                    //                           select new { t1.BookingRequestId, t1.UserClientId, t1.CommonAreaId, t1.Subject, t1.FromDate, t1.ToDate, t1.Reason, t1.Status, t1.CreatedOn, t1.AdminStatus, t2.CommonAreaName }).ToList();                                     

                    foreach (var item in userBookingRequests)
                    {
                        StrataOwnerBookingRequestModel strataOwnerBookingRequestModel = new StrataOwnerBookingRequestModel();
                        strataOwnerBookingRequestModel.BookingRequestId = item.BookingRequestId;
                        strataOwnerBookingRequestModel.UserClientId = item.UserClientId;
                        strataOwnerBookingRequestModel.CommonAreaId = item.CommonAreaId.Value;
                        strataOwnerBookingRequestModel.AdminStatus = item.AdminStatus == 2 ? "Rejected" : item.AdminStatus == 1 ? "Approved" : "Pending";
                        string StartTime = item.FromDate.Value.ToString("hh:mm tt");
                        strataOwnerBookingRequestModel.FromDate = item.FromDate != null ? item.FromDate.Value.ToString("dd MMM yyyy") + ", " + StartTime : "";
                        string ToDate = item.FromDate.Value.ToString("hh:mm tt");
                        strataOwnerBookingRequestModel.ToDate = item.ToDate != null ? item.ToDate.Value.ToString("dd MMM yyyy") + ", " + StartTime : "";
                        strataOwnerBookingRequestModel.CreatedOn = item.CreatedOn != null ? item.CreatedOn.Value.ToString("dd MMM yyyy") : "";
                        strataOwnerBookingRequestModel.Subject = item.Subject;
                        strataOwnerBookingRequestModel.Reason = item.Reason;
                        strataOwnerBookingRequestModel.CommonAreaName = item.CommonAreaName;
                        if (item.FromDate != null && item.ToDate != null)
                        {
                            strataOwnerBookingRequestModel.TotalDays = Convert.ToInt32((item.ToDate.Value - item.FromDate.Value).TotalDays);
                        }
                        else
                        {
                            strataOwnerBookingRequestModel.TotalDays = 0;
                        }
                        strataOwnerBookingRequestModelList.Add(strataOwnerBookingRequestModel);
                    }
                }
                catch
                {
                }
                strataOwnerBookingRequestModelList = strataOwnerBookingRequestModelList.Skip(startIndex).Take(BlockSize).ToList();
                return strataOwnerBookingRequestModelList;
            }
            else
            {
                return null;
            }
        }

        public int AddStrataOwnerBookingRequest(StrataOwnerBookingRequestModel strataOwnerBookingRequestModel)
        {
            int result = 0;
            try
            {
                tblUserBookingRequest tblUserBookingRequestDb = new tblUserBookingRequest();
                tblUserBookingRequestDb.UserClientId = ClientSessionData.UserClientId;
                tblUserBookingRequestDb.Subject = strataOwnerBookingRequestModel.Subject;
                tblUserBookingRequestDb.FromDate = !string.IsNullOrEmpty(strataOwnerBookingRequestModel.FromDate) ? Convert.ToDateTime(strataOwnerBookingRequestModel.FromDate) : (DateTime?)null;
                tblUserBookingRequestDb.ToDate = !string.IsNullOrEmpty(strataOwnerBookingRequestModel.ToDate) ? Convert.ToDateTime(strataOwnerBookingRequestModel.ToDate) : (DateTime?)null;
                tblUserBookingRequestDb.Reason = strataOwnerBookingRequestModel.Reason;
                tblUserBookingRequestDb.Status = int.Parse(strataOwnerBookingRequestModel.Status);
                tblUserBookingRequestDb.AdminStatus = 1;
                tblUserBookingRequestDb.CommonAreaId = strataOwnerBookingRequestModel.CommonAreaId;
                tblUserBookingRequestDb.CreatedOn = DateTime.UtcNow;
                tblUserBookingRequestDb.ModifiedOn = DateTime.UtcNow;
                tblUserBookingRequestDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;


                var UserRequests = _context.tblUserBookingRequests.Where(x => x.UserClientId == ClientSessionData.UserClientId && x.CommonAreaId == strataOwnerBookingRequestModel.CommonAreaId && x.Status != 2).FirstOrDefault();
                if (UserRequests != null)
                {
                    if (UserRequests.FromDate > Convert.ToDateTime(strataOwnerBookingRequestModel.ToDate) || UserRequests.ToDate < Convert.ToDateTime(strataOwnerBookingRequestModel.FromDate))
                    {

                        _context.tblUserBookingRequests.Add(tblUserBookingRequestDb);
                        result = _context.SaveChanges();
                    }
                    else
                    {
                        result = -1;
                    }
                }
                else
                {
                    _context.tblUserBookingRequests.Add(tblUserBookingRequestDb);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }

        public int AddStrataAdminBookingRequest(BookingRequestModel strataOwnerBookingRequestModel)
        {
            int result = 0;
            try
            {
                tblUserBookingRequest tblUserBookingRequestDb = new tblUserBookingRequest();
                tblUserBookingRequestDb.UserClientId = ClientSessionData.UserClientId;
                tblUserBookingRequestDb.Subject = strataOwnerBookingRequestModel.Subject;
                tblUserBookingRequestDb.FromDate = !string.IsNullOrEmpty(strataOwnerBookingRequestModel.FromDate) ? Convert.ToDateTime(strataOwnerBookingRequestModel.FromDate) : (DateTime?)null;
                tblUserBookingRequestDb.ToDate = !string.IsNullOrEmpty(strataOwnerBookingRequestModel.ToDate) ? Convert.ToDateTime(strataOwnerBookingRequestModel.ToDate) : (DateTime?)null;
                tblUserBookingRequestDb.Reason = strataOwnerBookingRequestModel.Reason;
                tblUserBookingRequestDb.Status = int.Parse(strataOwnerBookingRequestModel.Status);
                tblUserBookingRequestDb.AdminStatus = strataOwnerBookingRequestModel.AdminStatus;
                tblUserBookingRequestDb.CommonAreaId = strataOwnerBookingRequestModel.CommonAreaId;
                tblUserBookingRequestDb.CreatedOn = DateTime.UtcNow;
                tblUserBookingRequestDb.ModifiedOn = DateTime.UtcNow;
                tblUserBookingRequestDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;

                // Get all the records related to that particular strataboard
                var UserRequests = _context.tblUserBookingRequests.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.CommonAreaId == strataOwnerBookingRequestModel.CommonAreaId && x.Status != 2).ToList();
                if (UserRequests != null && UserRequests.Count > 0)
                {
                    foreach (var item in UserRequests)
                    {
                        // Check any other booking is not there in the requested time slot
                        if ((Convert.ToDateTime(strataOwnerBookingRequestModel.FromDate) >= item.FromDate && Convert.ToDateTime(strataOwnerBookingRequestModel.FromDate) <= item.ToDate)
                            || (Convert.ToDateTime(strataOwnerBookingRequestModel.ToDate) >= item.FromDate && Convert.ToDateTime(strataOwnerBookingRequestModel.ToDate) <= item.ToDate)
                            || (Convert.ToDateTime(strataOwnerBookingRequestModel.FromDate) <= item.FromDate && Convert.ToDateTime(strataOwnerBookingRequestModel.ToDate) >= item.ToDate))
                        {
                            result = -1;
                            break;
                        }
                    }
                }
               

                if (result == 0)
                {
                    _context.tblUserBookingRequests.Add(tblUserBookingRequestDb);
                    result = _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                new AppError().LogMe(ex);
                result = -2;
            }
            return result;
        }

        public IEnumerable<CommonAreaModel> GetAllCommonAreas()
        {
            if (ClientSessionData.UserClientId != 0)
            {
                var commonAreaModelList = new List<CommonAreaModel>();
                try
                {
                    var CommonAreas = _context.tblCommonAreas.Where(x => x.Status == 1 && x.StratasBoardId == ClientSessionData.ClientStrataBoardId).ToList();
                    foreach (var item in CommonAreas)
                    {
                        CommonAreaModel commonAreaModel = new CommonAreaModel();
                        commonAreaModel.CommonAreaId = item.CommonAreaId;
                        commonAreaModel.CommonAreaName = item.CommonAreaName;
                        commonAreaModelList.Add(commonAreaModel);
                    }
                }
                catch
                {
                }
                return commonAreaModelList;
            }
            else
            {
                return null;
            }
        }

        //Poll Question List For Admin
        public List<StrataOwnerModel> GetOwnerList(StrataOwnerModel modelData)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    if (string.IsNullOrEmpty(modelData.FirstName) || string.IsNullOrWhiteSpace(modelData.FirstName))
                        modelData.FirstName = null;
                    if (string.IsNullOrEmpty(modelData.LastName) || string.IsNullOrWhiteSpace(modelData.LastName))
                        modelData.LastName = null;
                    if (string.IsNullOrEmpty(modelData.Email) || string.IsNullOrWhiteSpace(modelData.Email))
                        modelData.Email = null;

                    IQueryable<StrataOwnerModel> model = context.tblUserClients.Where(x =>
                        x.RoleName.ToUpper() == "O"
                        && x.StratasBoardId == ClientSessionData.ClientStrataBoardId
                        && x.Status == 1
                        && !x.IsDeleted
                        && x.IsProfileComplete
                        && x.FirstName.Contains(modelData.FirstName == null ? x.FirstName : modelData.FirstName)
                        && x.LastName.Contains(modelData.LastName == null ? x.LastName : modelData.LastName)
                        && x.EmailId.Contains(modelData.Email == null ? x.EmailId : modelData.Email)
                        ).Select(c => new StrataOwnerModel()
                        {
                            UserClientId = c.UserClientId,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            UnitNumber = c.UnitNumber,
                            Email=c.EmailId,
                            CreatedOn = c.CreatedOn.ToString()
                        });

                    var total = model.Count();
                    if (total != 0)
                    {
                        var pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPageSize"]);
                        var skip = pageSize * (modelData.PageNo - 1);
                        return model.OrderByDescending(m => m.CreatedOn).Skip(skip).Take(pageSize).ToList();
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
