using StratasFair.BusinessEntity.Admin;
using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
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

namespace StratasFair.Business.Admin
{
    public class AdminUserHelper
    {
        StratasFairDBEntities context;
        private String _conString = String.Empty;
        public AdminUserHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();

            try
            {
                _conString = SqlHelper.GetConnectionString();
            }
            catch
            {
                throw;
            }
        }

        public ProfileModel GetAdminUserProfileById(int Id)
        {
            return context.vw_UserDetails.Where(x => x.UserId == Id).Select(x => new ProfileModel() { FirstName = x.FirstName, LastName = x.LastName, EmailId = x.EmailId, LoginId = x.LoginId }).FirstOrDefault();
        }


        public int UpdateProfile(ProfileModel profileModel)
        {
            int count = -1;
            if (profileModel.CreatedBy > 0)
            {
                if (context.tblUsers.Any(x => x.EmailId == profileModel.EmailId && x.UserId != profileModel.CreatedBy))
                {
                    // Email id already exists
                    return -4;
                }
                else if (context.tblUsers.Any(x => x.LoginId == profileModel.LoginId && x.UserId != profileModel.CreatedBy))
                {
                    // Username already exists
                    return -3;
                }
                else
                {
                    tblUser tblUserDb = new tblUser();
                    tblUserDb.UserId = profileModel.CreatedBy;
                    tblUserDb.FirstName = profileModel.FirstName;
                    tblUserDb.LastName = profileModel.LastName;
                    tblUserDb.EmailId = profileModel.EmailId;
                    tblUserDb.LoginId = profileModel.LoginId;

                    tblUserDb.ModifiedBy = profileModel.CreatedBy;
                    tblUserDb.ModifiedOn = DateTime.UtcNow;
                    tblUserDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;


                    context.tblUsers.Attach(tblUserDb);
                    context.Entry(tblUserDb).Property(x => x.FirstName).IsModified = true;
                    context.Entry(tblUserDb).Property(x => x.LastName).IsModified = true;

                    context.Entry(tblUserDb).Property(x => x.EmailId).IsModified = true;
                    context.Entry(tblUserDb).Property(x => x.LoginId).IsModified = true;


                    context.Entry(tblUserDb).Property(x => x.ModifiedBy).IsModified = true;
                    context.Entry(tblUserDb).Property(x => x.ModifiedOn).IsModified = true;
                    context.Entry(tblUserDb).Property(x => x.ModifiedFromIP).IsModified = true;

                    count = context.SaveChanges();
                    if (count == 1)
                        count = 0;
                }
            }
            return count;
        }


        public int UpdateAdminPassword(ChangePassword objChangePassword)
        {
            int result = -1;

            try
            {
                SqlParameter prmUserId = SqlHelper.CreateParameter("@UserId", objChangePassword.UserId);
                SqlParameter prmPassword = SqlHelper.CreateParameter("@Password", objChangePassword.CurrentPassword);
                SqlParameter prmNewPassword = SqlHelper.CreateParameter("@NewPassword", objChangePassword.NewPassword);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objChangePassword.Flag);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);

                SqlParameter[] allParams = { prmUserId, prmPassword, prmNewPassword, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_AddUpAdminUser", allParams);

                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }
            }
            catch
            {
                throw;
            }

            return result;
        }



        //get data from datasource using SPROC (sample data access layer)
        public List<UserModel> GetAdminUserByStatus(UserModel objUserModels)
        {
            try
            {
                var sqlCon = new SqlConnection(_conString);
                sqlCon.Open();

                var sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "usp_GetUser";
                sqlCmd.Connection = sqlCon;

                sqlCmd.Parameters.AddWithValue("@Flag", objUserModels.Flag);
                sqlCmd.Parameters.AddWithValue("@Status", objUserModels.Status);
                sqlCmd.Parameters.AddWithValue("@SearchKeywords", objUserModels.SearchKeywords);

                var list = new List<UserModel>();
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new UserModel();
                        p.FirstName = reader["firstName"].ToString();
                        p.LastName = reader["lastName"].ToString();
                        p.LoginId = reader["loginId"].ToString();
                        p.EmailId = reader["EmailId"].ToString();
                        p.RoleName = reader["roleName"].ToString();
                        p.CreatedOn = reader["addedon"].ToString();
                        p.IsSuper = Convert.ToInt32(reader["IsSuper"]);
                        p.UserId = Convert.ToInt64(reader["userId"]);
                        list.Add(p);
                    }
                }

                sqlCon.Close();

                return list;
            }
            catch
            {
                throw;
            }
        }

        public int ChangeAdminPassword(ref ChangePasswordModel model)
        {
            Encrypt64 encrypt = new Encrypt64();
            model.NewPassword = encrypt.Encrypt(model.NewPassword, ConfigurationManager.AppSettings["SecureKey"].ToString());
            model.OldPassword = encrypt.Encrypt(model.OldPassword, ConfigurationManager.AppSettings["SecureKey"].ToString());
            SqlCommand Cmd;
            string msg = "";
            int err = 0;
            SqlConnection Con = new SqlConnection(SqlHelper.GetConnectionString());
            Cmd = new SqlCommand("Usp_ChangePassword", Con);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Add("@LoginId", SqlDbType.VarChar, 100);
            Cmd.Parameters.Add("@oldPassword", SqlDbType.VarChar, 100);
            Cmd.Parameters.Add("@newPassword", SqlDbType.VarChar, 100);
            Cmd.Parameters.Add("@Type", SqlDbType.Int);
            Cmd.Parameters.Add("@msg", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
            Cmd.Parameters.Add("@err", SqlDbType.Int).Direction = ParameterDirection.Output;
            Cmd.Parameters["@LoginId"].Value = AdminSessionData.AdminUserName;
            Cmd.Parameters["@oldPassword"].Value = model.OldPassword;
            Cmd.Parameters["@newPassword"].Value = model.NewPassword;
            Cmd.Parameters["@Type"].Value = 1;
            try
            {
                Con.Open();
                Cmd.ExecuteNonQuery();
                msg = (string)Cmd.Parameters["@msg"].Value;
                err = (int)Cmd.Parameters["@err"].Value;
                model.Message = msg;
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


        public int PerformActionOnUser(UserModel objUserModel)
        {
            int result = -1;

            try
            {
                Encrypt64 encrypt = new Encrypt64();
                SqlParameter prmUserId = SqlHelper.CreateParameter("@UserId", objUserModel.UserId);
                SqlParameter prmLoginId = SqlHelper.CreateParameter("@LoginId", objUserModel.LoginId);
                SqlParameter prmPassword = SqlHelper.CreateParameter("@Password", encrypt.Encrypt(objUserModel.Password, ConfigurationManager.AppSettings["SecureKey"].ToString()));
                SqlParameter prmFirstName = SqlHelper.CreateParameter("@FirstName", objUserModel.FirstName);
                SqlParameter prmLastName = SqlHelper.CreateParameter("@LastName", objUserModel.LastName);
                SqlParameter prmEmailId = SqlHelper.CreateParameter("@EmailId", objUserModel.EmailId);
                SqlParameter prmRoleId = SqlHelper.CreateParameter("@RoleId", objUserModel.RoleId);
                if (objUserModel.RoleId == 1)
                {
                    objUserModel.Status = 1;
                }
                SqlParameter prmGender = SqlHelper.CreateParameter("@Gender", objUserModel.Gender);
                SqlParameter prmDOB = SqlHelper.CreateParameter("@DateOfBirth", objUserModel.DOB == null ? Convert.ToDateTime("1/1/1900") : Convert.ToDateTime(objUserModel.DOB));
                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", objUserModel.Status);
                SqlParameter prmCreatedBy = SqlHelper.CreateParameter("@CreatedBy", objUserModel.CreatedBy);
                SqlParameter prmCreatedFromIp = SqlHelper.CreateParameter("@CreatedFromIp", objUserModel.CreatedFromIp);
                SqlParameter prmUserType = SqlHelper.CreateParameter("@UserType", objUserModel.UserType);

                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objUserModel.Flag);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);

                SqlParameter[] allParams = { prmUserId, prmLoginId, prmPassword, prmFirstName, prmLastName, prmEmailId, prmRoleId, prmGender, prmDOB, prmStatus, prmCreatedBy, prmCreatedFromIp, prmUserType, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_AddUpAdminUser", allParams);

                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        public UserModel GetUserById(UserModel objUserModel)
        {
            try
            {
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objUserModel.Flag);
                SqlParameter prmUserId = SqlHelper.CreateParameter("@UserId", objUserModel.UserId);
                SqlParameter[] allParams = { prmFlag, prmUserId };
                SqlDataReader drReader = SqlHelper.ExecuteReader(_conString, CommandType.StoredProcedure, "usp_GetUser", allParams);
                if (drReader.HasRows)
                {
                    if (drReader.Read())
                    {
                        Encrypt64 encryptPass = new Encrypt64();
                        objUserModel.UserId = Convert.ToInt64(drReader["userId"].ToString());
                        objUserModel.FirstName = drReader["firstName"].ToString();
                        objUserModel.LastName = drReader["lastName"].ToString();
                        objUserModel.LoginId = drReader["loginId"].ToString();
                        objUserModel.EmailId = drReader["EmailId"].ToString();
                        objUserModel.RoleId = Convert.ToInt32(drReader["roleId"].ToString());
                        objUserModel.Status = Convert.ToInt32(drReader["Status"].ToString());
                        objUserModel.Password = encryptPass.Decrypt(drReader["password"].ToString(), ConfigurationManager.AppSettings["SecureKey"].ToString());
                        objUserModel.ConfirmPassword = objUserModel.Password;
                        objUserModel.Gender = drReader["gender"].ToString();
                        objUserModel.DOBMMDDYYYY = drReader["DOBMMDDYYYY"].ToString();
                    }
                    else
                    {
                        drReader.Close();
                    }
                }
                drReader.Close();

                return objUserModel;
            }
            catch
            {
                throw;
            }
        }
    }
}
