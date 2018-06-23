using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Business.CommonHelper;


namespace StratasFair.Business.Admin
{
    public class LoginHelper
    {
        public String _conString = String.Empty;

        public DataTable AuthenticateUser(LogOnModel objModel, out int result)
        {
            try
            {
                Encrypt64 encrypt = new Encrypt64();
                objModel.Password = encrypt.Encrypt(objModel.Password, ConfigurationManager.AppSettings["SecureKey"].ToString());

                result = -1;
                _conString = SqlHelper.GetConnectionString();
                SqlParameter prmUserName = SqlHelper.CreateParameter("@LoginId", objModel.UserName);
                SqlParameter prmPassword = SqlHelper.CreateParameter("@Password", objModel.Password);
                SqlParameter prmUserType = SqlHelper.CreateParameter("@UserType", objModel.UserType);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmUserName, prmPassword, prmUserType, prmErr };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_AuthenticateUser", allParams);
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

        public DataTable AdminPasswordReminder(ForgetPasswordModel objModel)
        {
            try
            {
                _conString = SqlHelper.GetConnectionString();
                SqlParameter prmUserName = SqlHelper.CreateParameter("@EmailId", objModel.Email);
                SqlParameter prmQFlag = SqlHelper.CreateParameter("@Flag", objModel.Flag);
                SqlParameter[] allParams = { prmUserName, prmQFlag };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "usp_GetUser", allParams);
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
    }
}
