using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StratasFair.Business.CommonHelper;
using StratasFair.Data;
using System.Data.SqlClient;
using System.Data;
using StratasFair.BusinessEntity.Front;
namespace StratasFair.Business.Front
{

    public class SubAdminHelper
    {
        StratasFairDBEntities _context;
        public string _conString = String.Empty;

        public SubAdminHelper()
        {
            if (_context == null)
            {
                _context = new StratasFairDBEntities();
            }
        }

        public int AddUpdate(SubAdminModel objectModel)
        {
            if (objectModel.UserClientId > 0)
            {
                return AddUpdateSubAdmin(objectModel);
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
                    return AddUpdateSubAdmin(objectModel);
                }
            }
        }

        public int AddUpdateSubAdmin(SubAdminModel subAdminModel)
        {
            int result = 0;
            int userClientId = 0;

            //using (var transaction = _context.Database.BeginTransaction())
            //{
            try
            {
                tblUserClient tblUserClientDb = new tblUserClient();
                tblUserClientDb.FirstName = subAdminModel.FirstName;
                tblUserClientDb.LastName = subAdminModel.LastName;
                tblUserClientDb.EmailId = subAdminModel.Email;
                tblUserClientDb.Position = subAdminModel.Position;
                tblUserClientDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;
                tblUserClientDb.RoleName = "SA";
                tblUserClientDb.CreatedBy = ClientSessionData.UserClientId;
                tblUserClientDb.CreatedOn = DateTime.UtcNow;
                tblUserClientDb.LastLogin = DateTime.UtcNow;
                tblUserClientDb.CurrentLogin = DateTime.UtcNow;
                if (subAdminModel.UserClientId == 0)
                {
                    tblUserClientDb.Password = AppLogic.EncryptPassword();
                    // tblUserClientDb.ContactNumber = subAdminModel.ContactNumber;
                    tblUserClientDb.IsEmailNotification = false;
                    tblUserClientDb.IsSMSNotification = false;
                    tblUserClientDb.IsProfileComplete = false;
                    tblUserClientDb.Status = 1;  // It will be active in both the cases
                    var AllowedUser = _context.tblStratasBoardSubscriptions.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId).FirstOrDefault().AllowedUser;

                    var AddedUserClients = _context.tblUserClients.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1);
                    if (AddedUserClients.Count() <= AllowedUser)
                    {
                        _context.tblUserClients.Add(tblUserClientDb);
                        _context.SaveChanges();
                    }
                    else
                    {
                        result = -3;
                    }
                }
                else
                {
                    tblUserClientDb.UserClientId = subAdminModel.UserClientId;
                    _context.tblUserClients.Attach(tblUserClientDb);
                    _context.Entry(tblUserClientDb).Property(x => x.FirstName).IsModified = true;
                    _context.Entry(tblUserClientDb).Property(x => x.LastName).IsModified = true;
                    _context.Entry(tblUserClientDb).Property(x => x.EmailId).IsModified = true;
                    _context.Entry(tblUserClientDb).Property(x => x.Position).IsModified = true;
                    result = _context.SaveChanges();
                    if(result == 1)
                    {
                        result = 0;
                    }
                }
                userClientId = Convert.ToInt32(tblUserClientDb.UserClientId);
                if (userClientId > 0)
                {
                    if (!string.IsNullOrEmpty(subAdminModel.SelectedUserClientPrivilege))
                    {
                        if (subAdminModel.SelectedUserClientPrivilege != subAdminModel.PageIds)
                        {
                            int[] selectedPrivilegeArray = subAdminModel.SelectedUserClientPrivilege.Split(",".ToCharArray()).Select(x => int.Parse(x.ToString())).ToArray();
                            _context.tblUserPrivs.RemoveRange(_context.tblUserPrivs.Where(x => x.UserClientId == userClientId));
                            foreach (var item in selectedPrivilegeArray)
                            {
                                tblUserPriv tblUserPrivDb = new tblUserPriv();
                                tblUserPrivDb.PageId = item;
                                tblUserPrivDb.UserClientId = userClientId;
                                tblUserPrivDb.Status = 1;
                                tblUserPrivDb.CreatedBy = ClientSessionData.UserClientId;
                                tblUserPrivDb.CreatedOn = DateTime.UtcNow;
                                tblUserPrivDb.ModifiedBy = ClientSessionData.UserClientId;
                                tblUserPrivDb.ModifiedOn = DateTime.UtcNow;
                                _context.tblUserPrivs.Add(tblUserPrivDb);
                                result = _context.SaveChanges();
                            }
                            if (result == 1)
                            {
                                EmailSender.FncSend_StratasBoard_RegistrationMail_ToSubAdminClient(userClientId);
                            }
                        }
                    }
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                result = -2;   // any error is there
            }
            return result;
        }


        public List<SubAdminModel> GetAllSubAdminDetails(int BlockNumber, int BlockSize)
        {
            try
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                int result = -1;
                _conString = SqlHelper.GetConnectionString();
                SqlParameter prmStrataBoardID = SqlHelper.CreateParameter("@StrataBoardID", ClientSessionData.ClientStrataBoardId);
                SqlParameter prmType = SqlHelper.CreateParameter("@Type", 3);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmStrataBoardID, prmType, prmErr };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_GetClientUser", allParams);
                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }

                List<SubAdminModel> subAdminModelList = new List<SubAdminModel>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0].Copy();
                    ds.Dispose();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var subAdmin = new SubAdminModel();
                        subAdmin.UserClientId = Convert.ToInt32(dt.Rows[i]["UserClientId"]);
                        subAdmin.FirstName = dt.Rows[i]["FirstName"].ToString();
                        subAdmin.LastName = dt.Rows[i]["LastName"].ToString();
                        subAdmin.Email = dt.Rows[i]["EmailId"].ToString();
                        subAdmin.Position = dt.Rows[i]["Position"].ToString();
                        subAdmin.CreatedOn = dt.Rows[i]["CreatedOn"].ToString();
                        subAdmin.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                        if (i == (dt.Rows.Count - 1))
                        {
                            subAdmin.UserClientPrivilege = this.GetUserClientPrivileges();
                        }
                        subAdminModelList.Add(subAdmin);
                    }
                    subAdminModelList = subAdminModelList.Skip(startIndex).Take(BlockSize).ToList();
                    return subAdminModelList;
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

        public SubAdminModel GetSubAdminDetails(int UserClientId)
        {
            try
            {

                int result = -1;
                _conString = SqlHelper.GetConnectionString();
                SqlParameter prmStrataBoardID = SqlHelper.CreateParameter("@StrataBoardID", ClientSessionData.ClientStrataBoardId);
                SqlParameter prmUserClientId = SqlHelper.CreateParameter("@UserClientId", UserClientId);
                SqlParameter prmType = SqlHelper.CreateParameter("@Type", 4);
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
                    var subAdmin = new SubAdminModel();
                    subAdmin.UserClientId = Convert.ToInt32(dt.Rows[0]["UserClientId"]);
                    subAdmin.FirstName = dt.Rows[0]["FirstName"].ToString();
                    subAdmin.LastName = dt.Rows[0]["LastName"].ToString();
                    subAdmin.Email = dt.Rows[0]["EmailId"].ToString();
                    subAdmin.Position = dt.Rows[0]["Position"].ToString();
                    subAdmin.CreatedOn = dt.Rows[0]["CreatedOn"].ToString();
                    subAdmin.PageIds = dt.Rows[0]["PageIds"].ToString();
                    subAdmin.SelectedUserClientPrivilege = dt.Rows[0]["PageIds"].ToString();
                    subAdmin.UserClientPrivilege = this.GetUserClientPrivileges();
                    return subAdmin;
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

        public List<UserClientPrivilege> GetUserClientPrivileges()
        {
            try
            {
                _context = new StratasFairDBEntities();
                var objMember = _context.vw_UserClientPrivileges.ToList();
                var UserPrivilegeList = new List<UserClientPrivilege>();
                foreach (var item in objMember)
                {
                    UserClientPrivilege model = new UserClientPrivilege
                    {
                        PageId = item.PageId,
                        PageName = item.PageName,
                        PageLink = item.PageLink,
                        IsChecked = false
                    };
                    UserPrivilegeList.Add(model);
                }
                return UserPrivilegeList;

            }
            catch
            {
                return new List<UserClientPrivilege>();
            }

        }

        public int DeleteSubAdmin(int SubAdminId)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    int result = 0;
                    var userClient = _context.tblUserClients.Where(x => x.UserClientId == SubAdminId).FirstOrDefault();
                    if (userClient.RoleName == "SA")
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

        public int ActivateDeActivateSubAdmin(int SubAdminId, int IsActive)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    int result = 0;
                    var userClient = _context.tblUserClients.Where(x => x.UserClientId == SubAdminId).FirstOrDefault();
                    if (userClient.RoleName == "SA")
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

    }


}
