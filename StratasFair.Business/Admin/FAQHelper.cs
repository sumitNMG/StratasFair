using StratasFair.BusinessEntity.Admin;
using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
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
    public class FAQHelper
    {
        private String _conString = String.Empty;
        public FAQHelper()
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
        public List<FAQModel> GetAll()
        {
            try
            {
                var sqlCon = new SqlConnection(_conString);
                sqlCon.Open();

                var sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "Usp_GetFAQ";
                sqlCmd.Connection = sqlCon;

                var objFAQlist = new List<FAQModel>();
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var objFAQ = new FAQModel();
                        objFAQ.FAQId = Convert.ToInt32(reader["FAQId"]);
                        objFAQ.Question = reader["Question"].ToString();
                        objFAQ.Answer = reader["Answer"].ToString();
                        // objFAQ.FaqCategoryId = Convert.ToInt32(reader["FaqCategoryId"].ToString());
                        // objFAQ.Category = reader["Category"].ToString();
                        objFAQ.SortOrder = Convert.ToInt32(reader["SortOrder"].ToString());
                        objFAQ.Status = Convert.ToByte(reader["Status"]);
                        objFAQ.CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
                        objFAQlist.Add(objFAQ);
                    }
                }
                sqlCon.Close();
                return objFAQlist;
            }
            catch
            {
                throw;
            }
        }


        public FAQModel GetByID(int Id)
        {
            try
            {
                var sqlCon = new SqlConnection(_conString);
                sqlCon.Open();

                var sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "Usp_GetFAQ";
                sqlCmd.Parameters.AddWithValue("@FAQId", Id);
                sqlCmd.Connection = sqlCon;

                FAQModel objFAQ = null;
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        objFAQ = new FAQModel();
                        objFAQ.FAQId = Convert.ToInt32(reader["FAQId"]);
                        objFAQ.Question = reader["Question"].ToString();
                        objFAQ.Answer = reader["Answer"].ToString();
                        // objFAQ.FaqCategoryId = Convert.ToInt32(reader["FaqCategoryId"].ToString());
                        objFAQ.SortOrder = Convert.ToInt32(reader["SortOrder"].ToString());
                        objFAQ.Status = Convert.ToByte(reader["Status"]);
                    }
                }
                sqlCon.Close();
                return objFAQ;
            }
            catch
            {
                throw;
            }
        }

        public int Add(FAQModel objFAQ)
        {
            int result = -1;
            try
            {
                SqlParameter prmQuestion = SqlHelper.CreateParameter("@Question", objFAQ.Question);
                SqlParameter prmAnswer = SqlHelper.CreateParameter("@Answer", objFAQ.Answer);
                // SqlParameter prmFaqCategoryId = SqlHelper.CreateParameter("@FaqCategoryId", objFAQ.FaqCategoryId);
                SqlParameter prmSort = SqlHelper.CreateParameter("@SortOrder", objFAQ.SortOrder);

                SqlParameter prmCreatedBy = SqlHelper.CreateParameter("@CreatedBy", AdminSessionData.AdminUserId);
                SqlParameter prmCreatedOn = SqlHelper.CreateParameter("@CreatedOn", DateTime.UtcNow);
                SqlParameter prmCreatedFromIp = SqlHelper.CreateParameter("@CreatedFromIp", HttpContext.Current.Request.UserHostAddress);

                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", objFAQ.Status);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", 1);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmQuestion, prmAnswer, prmSort, prmCreatedBy, prmCreatedOn, prmCreatedFromIp, prmStatus, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_AddUpdFAQ", allParams);

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

        public int Upate(FAQModel objFAQ)
        {
            int result = -1;
            try
            {
                SqlParameter prmFAQId = SqlHelper.CreateParameter("@FAQId", objFAQ.FAQId);
                SqlParameter prmQuestion = SqlHelper.CreateParameter("@Question", objFAQ.Question);
                SqlParameter prmAnswer = SqlHelper.CreateParameter("@Answer", objFAQ.Answer);
                // SqlParameter prmFaqCategoryId = SqlHelper.CreateParameter("@FaqCategoryId", objFAQ.FaqCategoryId);
                SqlParameter prmSort = SqlHelper.CreateParameter("@SortOrder", objFAQ.SortOrder);

                SqlParameter prmCreatedBy = SqlHelper.CreateParameter("@CreatedBy", AdminSessionData.AdminUserId);
                SqlParameter prmCreatedOn = SqlHelper.CreateParameter("@CreatedOn", DateTime.Now);
                SqlParameter prmCreatedFromIp = SqlHelper.CreateParameter("@CreatedFromIp", HttpContext.Current.Request.UserHostAddress);


                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", objFAQ.Status);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objFAQ.Flag);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmFAQId, prmQuestion, prmAnswer, prmSort, prmCreatedBy, prmCreatedOn, prmCreatedFromIp, prmStatus, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_AddUpdFAQ", allParams);

                if (prmErr.Value != null)
                {
                    result = (int)prmErr.Value;
                }
                return result;
            }
            catch { throw; }
        }

        public int Delete(FAQModel objFAQ)
        {
            int result = -1;
            try
            {
                SqlParameter prmFAQId = SqlHelper.CreateParameter("@FAQId", objFAQ.FAQId);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objFAQ.Flag);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmFAQId, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_AddUpdFAQ", allParams);

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

        public int ActDeactFAQ(FAQModel objFAQ)
        {
            int result = -1;
            try
            {
                SqlParameter prmFAQId = SqlHelper.CreateParameter("@FAQId", objFAQ.FAQId);
                SqlParameter prmStatus = SqlHelper.CreateParameter("@Status", objFAQ.Status);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objFAQ.Flag);
                SqlParameter prmErr = SqlHelper.CreateParameter("@Err", -1, ParameterDirection.Output);
                SqlParameter[] allParams = { prmFAQId, prmStatus, prmFlag, prmErr };
                SqlHelper.ExecuteNonQuery(_conString, CommandType.StoredProcedure, "Usp_AddUpdFAQ", allParams);

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
    }
}
