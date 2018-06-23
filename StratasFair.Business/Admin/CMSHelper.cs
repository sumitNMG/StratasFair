using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Business.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.Business.Admin
{
    public class CMSHelper
    {
        private String _conString = String.Empty;
        public CMSHelper()
        {
            try
            {
                _conString = SqlHelper.GetConnectionString();
            }
            catch
            {
                throw;
            }
        }



        public List<CMSModel> GetAll(int Flag)
        {
            try
            {
                var sqlCon = new SqlConnection(_conString);
                sqlCon.Open();

                var sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "Usp_GetCMS";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Connection = sqlCon;

                var list = new List<CMSModel>();
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new CMSModel();
                        p.ContentId = Convert.ToInt32(reader["ContentId"]);
                        p.PageName = reader["PageName"].ToString();
                        //p.PageUrl = reader["PageUrl"].ToString();
                        p.MetaTitle = reader["MetaTitle"].ToString();
                        p.MetaKeyword = reader["MetaKeyword"].ToString();
                        p.MetaDescription = reader["MetaDescription"].ToString();
                        p.Content = reader["Content"].ToString();
                        p.ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
                        p.ModifiedOn = reader["ModifiedOn"].ToString();
                        p.Status = Convert.ToInt32(reader["Status"]);
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


        public CMSModel GetByID(int Id)
        {
            try
            {
                var sqlCon = new SqlConnection(_conString);
                sqlCon.Open();

                var sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "Usp_GetCMS";
                sqlCmd.Parameters.AddWithValue("@ContentId", Id);
                sqlCmd.Parameters.AddWithValue("@Flag", 1);
                sqlCmd.Connection = sqlCon;

                CMSModel p = null;
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        p = new CMSModel();
                        p.ContentId = Convert.ToInt32(reader["ContentId"]);
                        p.PageName = reader["PageName"].ToString();
                        //p.PageUrl = reader["PageUrl"].ToString();
                        p.MetaTitle = reader["MetaTitle"].ToString();
                        p.MetaKeyword = reader["MetaKeyword"].ToString();
                        p.MetaDescription = reader["MetaDescription"].ToString();
                        p.Content = reader["Content"].ToString();
                        p.ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
                        p.ModifiedOn = reader["ModifiedOn"].ToString();
                        p.Status = Convert.ToInt32(reader["Status"]);
                    }
                }
                sqlCon.Close();
                return p;
            }
            catch
            {
                throw;
            }
        }

        public int ActiveDeactiveCMS(CMSModel objCMS)
        {
            int result = -1;
            try
            {
                SqlParameter prmContentId = SqlHelper.CreateParameter("@ContentId", objCMS.ContentId);
                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", objCMS.Status);
                SqlParameter prmModifiedBy = SqlHelper.CreateParameter("@ModifiedBy", objCMS.ModifiedBy);
                SqlParameter prmModifiedFromIp = SqlHelper.CreateParameter("@ModifiedFromIp", objCMS.ModifiedFromIp);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objCMS.Flag);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmContentId, prmStatus, prmModifiedBy, prmModifiedFromIp, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_GetCMS", allParams);

                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public int UpateCMS(CMSModel objCMS)
        {
            int result = -1;
            try
            {
                SqlParameter prmContentId = SqlHelper.CreateParameter("@ContentId", objCMS.ContentId);
                SqlParameter prmPageName = SqlHelper.CreateParameter("@PageName", objCMS.PageName);
                //SqlParameter prmPageUrl = SqlHelper.CreateParameter("@PageUrl", objCMS.PageUrl);
                SqlParameter prmMetaTitle = SqlHelper.CreateParameter("@MetaTitle", objCMS.MetaTitle);
                SqlParameter prmMetaKey = SqlHelper.CreateParameter("@MetaKeyword", objCMS.MetaKeyword);
                SqlParameter prmMetaDescription = SqlHelper.CreateParameter("@MetaDescription", objCMS.MetaDescription);
                SqlParameter prmContent = SqlHelper.CreateParameter("@Content", objCMS.Content);
                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", objCMS.Status);
                SqlParameter prmModifiedBy = SqlHelper.CreateParameter("@ModifiedBy", AdminSessionData.AdminUserId);
                SqlParameter prmModifiedFromIp = SqlHelper.CreateParameter("@ModifiedFromIp", objCMS.ModifiedFromIp);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", 2);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmContentId, prmPageName, prmMetaTitle, prmMetaKey, prmMetaDescription, prmContent, prmStatus, prmModifiedBy, prmModifiedFromIp, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_GetCMS", allParams);

                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }
                return result;
            }
            catch { throw; }
        }

        #region //Email Template Code//
        public List<EmailTemplateModel> GetAllTemplates(int Flag)
        {
            try
            {
                var sqlCon = new SqlConnection(_conString);
                sqlCon.Open();

                var sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "Usp_GetEmailTemplate";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Connection = sqlCon;

                var list = new List<EmailTemplateModel>();
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new EmailTemplateModel();
                        p.TemplateMasterId = Convert.ToInt32(reader["TemplateMasterId"]);
                        p.Title = reader["Title"].ToString();
                        p.Name = reader["Name"].ToString();
                        p.ConfigValue = AppLogic.StripHtml(reader["ConfigValue"].ToString());
                        p.ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
                        p.ModifiedOn = reader["ModifiedOn"].ToString();
                        p.Status = Convert.ToByte(reader["Status"]);
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

        public EmailTemplateModel GetTemplateByID(int Id)
        {
            try
            {
                var sqlCon = new SqlConnection(_conString);
                sqlCon.Open();

                var sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "Usp_GetEmailTemplate";
                sqlCmd.Parameters.AddWithValue("@TemplateMasterId", Id);
                sqlCmd.Parameters.AddWithValue("@Flag", 1);
                sqlCmd.Connection = sqlCon;

                EmailTemplateModel p = null;
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p = new EmailTemplateModel();
                        p.TemplateMasterId = Convert.ToInt32(reader["TemplateMasterId"]);
                        p.Title = reader["Title"].ToString();
                        p.Name = reader["Name"].ToString();
                        p.ConfigValue = AppLogic.StripHtml(reader["ConfigValue"].ToString());
                        p.ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
                        p.ModifiedOn = reader["ModifiedOn"].ToString();
                        p.Status = Convert.ToByte(reader["Status"]);
                    }
                }
                sqlCon.Close();
                return p;
            }
            catch
            {
                throw;
            }
        }


        public int UpateTemplate(EmailTemplateModel model)
        {
            int result = -1;
            try
            {
                SqlParameter prmTemplateId = SqlHelper.CreateParameter("@TemplateMasterId", model.TemplateMasterId);
                SqlParameter prmTitle = SqlHelper.CreateParameter("@Title", model.Title);
                SqlParameter prmConfigValue = SqlHelper.CreateParameter("@ConfigValue", model.ConfigValue);
                SqlParameter prmModifiedBy = SqlHelper.CreateParameter("@ModifiedBy", AdminSessionData.AdminUserId);
                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", model.Status);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", 1);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmTemplateId, prmTitle, prmConfigValue, prmStatus, prmModifiedBy, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_AddUpdTemplate", allParams);

                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }
                return result;
            }
            catch { throw; }
        }

        public int ActDeActTemplate(int TemplateId, int Status)
        {
            int result = -1;
            try
            {
                SqlParameter prmTemplateId = SqlHelper.CreateParameter("@TemplateMasterId", TemplateId);
                SqlParameter prmModifiedBy = SqlHelper.CreateParameter("@ModifiedBy", AdminSessionData.AdminUserId);
                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", Status);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", 1);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmTemplateId, prmStatus, prmModifiedBy, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_AddUpdTemplate", allParams);

                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }
                return result;
            }
            catch { throw; }
        }


        #endregion
    }
}
