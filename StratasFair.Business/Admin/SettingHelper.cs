using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Admin
{
    public class SettingHelper
    {
        StratasFairDBEntities context;
        public SettingHelper()
        {
            if (context == null)
            {
                context = new StratasFairDBEntities();
            }
        }

        public List<SettingsModel> GetaLL()
        {
            try
            {
                return context.tblSettings.Select(x => new SettingsModel()
                {
                    SettingId = x.SettingId,
                    SettingName = x.SettingName,
                    SettingValue = x.SettingValue,
                    Status = x.Status
                }).ToList();
            }
            catch
            {
                return null;
            }

        }

        public SettingsModel GetById(int Id)
        {
            try
            {
                return context.tblSettings.Where(y => y.SettingId == Id).Select(x => new SettingsModel()
                {
                    SettingId = x.SettingId,
                    SettingName = x.SettingName,
                    SettingValue = x.SettingValue,
                    Status = x.Status,
                    DataType = x.DataType
                }).FirstOrDefault();
            }
            catch
            {
                return null;
            }

        }



        public int Update(SettingsModel objSettings)
        {
            int result = -1;
            try
            {
                SqlParameter prmSettingId = SqlHelper.CreateParameter("@SettingId", objSettings.SettingId);
                SqlParameter prmSettingName = SqlHelper.CreateParameter("@SettingName", objSettings.SettingName);
                SqlParameter prmSettingValue = SqlHelper.CreateParameter("@SettingValue", AppLogic.IsStringDouble(objSettings.SettingValue));
                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", objSettings.Status);
                SqlParameter prmCreatedBy = SqlHelper.CreateParameter("@CreatedBy", AdminSessionData.AdminUserId);
                SqlParameter prmCreatedFromIp = SqlHelper.CreateParameter("@CreatedFromIp", HttpContext.Current.Request.UserHostAddress);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objSettings.Flag);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmSettingId, prmSettingName, prmSettingValue, prmStatus, prmCreatedBy, prmCreatedFromIp, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, "Usp_AddUpdSettings", allParams);

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
    }
}
