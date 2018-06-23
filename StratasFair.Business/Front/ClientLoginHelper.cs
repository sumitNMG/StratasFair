using Org.BouncyCastle.Asn1.Ocsp;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Admin;
using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Front
{
    public class ClientLoginHelper
    {

        StratasFairDBEntities _context;

        public ClientLoginHelper()
        {
            if (_context == null)
            {
                _context = new StratasFairDBEntities();
            }
        }

        public String _conString = String.Empty;

      /// <summary>
      ///  Used to Authenticate Client User
      /// </summary>
      /// <param name="objModel"></param>
      /// <param name="result"></param>
      /// <returns></returns>
        public DataTable AuthenticateClientUser(ClientLogOnModel objModel, out int result)
        {
            try
            {
                objModel.Password = AppLogic.EncryptPasswordString(objModel.Password);

                result = -1;
                _conString = SqlHelper.GetConnectionString();
                SqlParameter prmEmail = SqlHelper.CreateParameter("@EmailId", objModel.Email);
                SqlParameter prmPassword = SqlHelper.CreateParameter("@Password", objModel.Password);
                SqlParameter prmType = SqlHelper.CreateParameter("@Type", 1);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmEmail, prmPassword,prmType, prmErr };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_GetClientUser", allParams);
                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable Dt = ds.Tables[0].Copy();
                    ds.Dispose();
                    return Dt;
                }
                else
                {
                    return (DataTable)null;
                }
            }
            catch
            {
                throw;
            }
        }

        public int ClientPasswordReminder(ClientForgetPasswordModel objModel)
        {
            try
            {
                _conString = SqlHelper.GetConnectionString();
                int StratasBoardId = 0;
                SqlParameter prmUserName = SqlHelper.CreateParameter("@EmailId", objModel.Email);
                SqlParameter prmType = SqlHelper.CreateParameter("@Type", 2);
                SqlParameter[] allParams = { prmUserName, prmType };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_GetClientUser", allParams);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable Dt = ds.Tables[0].Copy();
                    StratasBoardId = Convert.ToInt32(Dt.Rows[0]["StratasBoardId"]);
                    ds.Dispose();
                    return StratasBoardId;
                }
                else
                {
                    return StratasBoardId;
                }
            }
            catch
            {
                throw;
            }
        }

        

        public int CompleteProfile(ClientLogOnModel clientLogOnModel)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                var userClient = _context.tblUserClients.Where(x => x.UserClientId == ClientSessionData.UserClientId).FirstOrDefault();
                userClient.FirstName = clientLogOnModel.FirstName;
                userClient.LastName = clientLogOnModel.LastName;
                userClient.ContactNumber = clientLogOnModel.ContactNumber;
                userClient.PremiseType = clientLogOnModel.PremiseType;

                userClient.ProfilePicture = clientLogOnModel.ProfilePicture;
                userClient.LeaseCommenceDate = string.IsNullOrEmpty(clientLogOnModel.LeaseCommenceDate) ? (DateTime?)null : Convert.ToDateTime(clientLogOnModel.LeaseCommenceDate);
                userClient.LeaseEndDate = string.IsNullOrEmpty(clientLogOnModel.LeaseEndDate) ? (DateTime?)null : Convert.ToDateTime(clientLogOnModel.LeaseEndDate);
                userClient.UnitNumber = clientLogOnModel.UnitNumber;
                userClient.IsProfileComplete = true;
                _context.Entry(userClient).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                ClientSessionData.ClientName = userClient.FirstName + " " + userClient.LastName;
                ClientSessionData.ClientProfilePicture = userClient.ProfilePicture;
            }
            return 1;
        }

        public ClientLogOnModel GetById(int userClientId)
        {
            try
            {
                var objMember = _context.tblUserClients.Where(x => x.UserClientId == userClientId).FirstOrDefault();

                ClientLogOnModel userClient = new ClientLogOnModel
                {
                    UserClientId = objMember.UserClientId,
                    FirstName = objMember.FirstName,
                    LastName = objMember.LastName,
                    Email = objMember.EmailId,
                    ContactNumber = objMember.ContactNumber,
                    StratasBoardId = objMember.StratasBoardId,
                    RoleName = objMember.RoleName,
                    PremiseType = objMember.PremiseType,
                    ProfilePicture = objMember.ProfilePicture,
                    OldProfilePicture = objMember.ProfilePicture,
                    LeaseCommenceDate = objMember.LeaseCommenceDate != null ? objMember.LeaseCommenceDate.Value.ToShortDateString() : "",
                    LeaseEndDate = objMember.LeaseEndDate != null ? objMember.LeaseEndDate.Value.ToShortDateString() : "",
                    UnitNumber = objMember.UnitNumber,
                    IsProfileComplete = objMember.IsProfileComplete,
                    StrataBoardName=objMember.tblStratasBoard.StratasBoardName,
                    BuildingName = objMember.tblStratasBoard.BuildingName,
                    IsSMSNotification =objMember.IsSMSNotification,
                    IsEmailNotification = objMember.IsEmailNotification,
                    StrataPortalLink = objMember.tblStratasBoard.PortalLink
                };

                return userClient;
            }
            catch
            {

                return new ClientLogOnModel();
            }
        }

        public int ForgotChangePassword(ClientChangePassword model)
        {
            string Constr = ConfigurationManager.ConnectionStrings["StratasFairConnectionStr"].ConnectionString;
            model.NewPassword = AppLogic.EncryptPasswordString(model.NewPassword);
            SqlCommand Cmd;
            int err = 0;
            SqlConnection Con = new SqlConnection(Constr);
            Cmd = new SqlCommand("Usp_ChangePassword", Con);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Add("@UserClientId", SqlDbType.VarChar, 100);
            Cmd.Parameters.Add("@newPassword", SqlDbType.VarChar, 100);
            Cmd.Parameters.Add("@Type", SqlDbType.Int);
            Cmd.Parameters.Add("@msg", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            Cmd.Parameters.Add("@err", SqlDbType.Int).Direction = ParameterDirection.Output;
            Cmd.Parameters["@UserClientId"].Value = model.UserClientId;
            Cmd.Parameters["@newPassword"].Value = model.NewPassword;
            Cmd.Parameters["@Type"].Value = 2;
            try
            {
                Con.Open();
                Cmd.ExecuteNonQuery();
                err = (int)Cmd.Parameters["@err"].Value;
                return err;
            }
            catch
            {
                throw;
            }
            finally
            {
                Con.Close();
                Cmd.Dispose();
            }
        }

        public int UpdateProfile(ClientLogOnModel clientLogOnModel, bool IsOwner)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    var userClient = _context.tblUserClients.Where(x => x.UserClientId == ClientSessionData.UserClientId).FirstOrDefault();
                    userClient.FirstName  = clientLogOnModel.FirstName;
                    userClient.LastName = clientLogOnModel.LastName;
                    if (!string.IsNullOrEmpty(clientLogOnModel.ProfilePicture))
                    {
                        userClient.ProfilePicture = clientLogOnModel.ProfilePicture;
                    }

                    if (IsOwner)
                    {
                        userClient.ContactNumber = clientLogOnModel.ContactNumber;
                        userClient.PremiseType = clientLogOnModel.PremiseType;

                        userClient.LeaseCommenceDate = string.IsNullOrEmpty(clientLogOnModel.LeaseCommenceDate) ? (DateTime?)null : Convert.ToDateTime(clientLogOnModel.LeaseCommenceDate);
                        userClient.LeaseEndDate = string.IsNullOrEmpty(clientLogOnModel.LeaseEndDate) ? (DateTime?)null : Convert.ToDateTime(clientLogOnModel.LeaseEndDate);
                        userClient.UnitNumber = clientLogOnModel.UnitNumber;
                    }
                    _context.Entry(userClient).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    ClientSessionData.ClientName = userClient.FirstName + " " + userClient.LastName;
                    ClientSessionData.ClientProfilePicture = userClient.ProfilePicture;
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public int ResetChangePassword(ClientChangePassword model)
        {
            string Constr = ConfigurationManager.ConnectionStrings["StratasFairConnectionStr"].ConnectionString;
           string a = AppLogic.DecryptPassword(model.OldPassword);
            model.OldPassword = AppLogic.EncryptPasswordString(model.OldPassword);
            model.NewPassword = AppLogic.EncryptPasswordString(model.NewPassword);
            SqlCommand Cmd;
            int err = 0;
            SqlConnection Con = new SqlConnection(Constr);
            Cmd = new SqlCommand("Usp_ChangePassword", Con);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Add("@UserClientId", SqlDbType.VarChar, 100);
            Cmd.Parameters.Add("@oldpassword", SqlDbType.VarChar, 100);
            Cmd.Parameters.Add("@newPassword", SqlDbType.VarChar, 100);
            Cmd.Parameters.Add("@Type", SqlDbType.Int);
            Cmd.Parameters.Add("@msg", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            Cmd.Parameters.Add("@err", SqlDbType.Int).Direction = ParameterDirection.Output;
            Cmd.Parameters["@UserClientId"].Value = ClientSessionData.UserClientId;
            Cmd.Parameters["@oldpassword"].Value = model.OldPassword;
            Cmd.Parameters["@newPassword"].Value = model.NewPassword;
            Cmd.Parameters["@Type"].Value = 3;
            try
            {
                Con.Open();
                Cmd.ExecuteNonQuery();
                err = (int)Cmd.Parameters["@err"].Value;
                return err;
            }
            catch
            {
                throw;
            }
            finally
            {
                Con.Close();
                Cmd.Dispose();
            }
        }

        public int UpdateEnableDisableNotification(bool IsEnable, int Type, bool IsAdmin)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                try
                {
                    var userClient = _context.tblUserClients.Where(x => x.UserClientId == ClientSessionData.UserClientId).FirstOrDefault();
                    if(IsAdmin)
                    {
                        userClient.IsEmailNotification = IsEnable;
                        _context.tblUserClients.Attach(userClient);
                        _context.Entry(userClient).Property(x => x.IsEmailNotification).IsModified = true;
                        _context.SaveChanges();
                        ClientSessionData.ClientIsEmailNotification = userClient.IsEmailNotification;
                        return 1;                        
                    }
                    else
                    {
                        if (Type == 1)
                        {
                            userClient.IsEmailNotification = IsEnable;
                            _context.tblUserClients.Attach(userClient);
                            _context.Entry(userClient).Property(x => x.IsEmailNotification).IsModified = true;
                            _context.SaveChanges();
                            ClientSessionData.ClientIsEmailNotification = userClient.IsEmailNotification;
                        }
                        else if (Type == 2)
                        {
                            userClient.IsSMSNotification = IsEnable;
                            _context.tblUserClients.Attach(userClient);
                            _context.Entry(userClient).Property(x => x.IsSMSNotification).IsModified = true;
                            _context.SaveChanges();
                            ClientSessionData.ClientIsSMSNotification = userClient.IsSMSNotification;
                        }
                        return 1;
                    }
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public string TestPassword(int userClientId)
        {
            string Pass =string.Empty;
            try
            {
                var objMember = _context.tblUserClients.Where(x => x.UserClientId == userClientId).FirstOrDefault();
                Pass = AppLogic.DecryptPassword(objMember.Password);

                return Pass;
            }
            catch
            {

                return Pass;
            }
        }

        public List<UserClientPrivilege> GetUserClientPrivileges(int UserClientId)
        {
            try
            {
                _conString = SqlHelper.GetConnectionString();
                SqlParameter prmUserClientId = SqlHelper.CreateParameter("@UserClientId", UserClientId);
                SqlParameter prmType = SqlHelper.CreateParameter("@Type", 7);
                SqlParameter[] allParams = { prmUserClientId, prmType };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_GetClientUser", allParams);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0].Copy();
                    List<UserClientPrivilege> userClientPrivilegeList = new List<UserClientPrivilege>();
                    if(dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count ; i++)
                        {
                            UserClientPrivilege userClientPrivilege = new UserClientPrivilege();
                            userClientPrivilege.PageId = Convert.ToInt32(dt.Rows[i]["PageId"]);
                            userClientPrivilege.PageName = dt.Rows[i]["PageName"].ToString();
                            userClientPrivilege.PageLink = dt.Rows[i]["PageLink"].ToString();
                            userClientPrivilege.PageLevel = Convert.ToInt32(dt.Rows[i]["Level"]);
                            userClientPrivilege.ParentPageId = Convert.ToInt32(dt.Rows[i]["ParentPageId"]);
                            userClientPrivilege.PageLinkIconName = dt.Rows[i]["PageLinkIconName"].ToString();

                            userClientPrivilegeList.Add(userClientPrivilege);
                        }
                    }
                    return userClientPrivilegeList;
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

        public string GetPortalUrlFromCurrentUrl()
        {
            string PortalUrl = string.Empty;
            if (HttpContext.Current.Request.Url.Segments[1].Replace("/", "").ToLower() != "stratasfair")
            {
                PortalUrl = HttpContext.Current.Request.Url.Segments[1].Replace("/", "");
            }
            else
            {
                PortalUrl = HttpContext.Current.Request.Url.Segments[2].Replace("/", "");
            }
            return PortalUrl;
        }
    }
}
