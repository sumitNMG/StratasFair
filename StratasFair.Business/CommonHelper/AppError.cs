using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Business.CommonHelper
{
    public class AppError
    {
        private String _statusCode;
        private String _errMsg;
        private String _url;
        private String _referer;
        private String _browser;
        private String _os;
        private String _ip;


        public AppError()
        {


        }


        public void LogMe(Exception exGenerated)
        {
            try
            {
                if (!exGenerated.Message.Equals("Thread was being aborted."))
                {
                    NewLog(exGenerated);
                }
            }
            catch
            {
                // code left empty intentionally due to recursive in nature as well as not necessary to tackle such errors.
                // It will left suppress the errors.
            }
        }

        public void ErrorLog(ExceptionContext ex)
        {

            _statusCode = HttpContext.Current.Response.Status;
            _errMsg = ex.Exception.Message;
            _url = HttpContext.Current.Request.RawUrl;
            _referer = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            _browser = HttpContext.Current.Request.Browser.Browser + " " + HttpContext.Current.Request.Browser.Version;
            _os = HttpContext.Current.Request.Browser.Platform;
            _ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //String mask = "INSERT INTO tblAppErrorLog VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
            //String Qry = String.Format(mask, _statusCode, _errMsg, _datetime, _errStack, _url, _referer, _browser, _os, _ip, _siteId);
            //String conString = ConfigurationManager.AppSettings["sqlConnectionString"];
            //SqlHelper.ExecuteNonQuery(conString, CommandType.Text, Qry);

            SqlParameter prmStatusCode = SqlHelper.CreateParameter("@AppHttpStatus", _statusCode);
            SqlParameter prmErrMsg = SqlHelper.CreateParameter("@AppErrorMsg", _errMsg);
            SqlParameter prmUrl = SqlHelper.CreateParameter("@AppUrl", _url);
            SqlParameter prmReferer = SqlHelper.CreateParameter("@AppReferer", _referer);
            SqlParameter prmBrowser = SqlHelper.CreateParameter("@AppBrowser", _browser);
            SqlParameter prmOs = SqlHelper.CreateParameter("@AppOS", _os);
            SqlParameter prmIp = SqlHelper.CreateParameter("@AppIP", _ip);

            // create array of parameters
            SqlParameter[] allParams = { prmStatusCode, prmErrMsg, prmUrl, prmReferer, prmBrowser, prmOs, prmIp };

            // store data into the database            
            SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, "[Usp_LogAppError]", allParams);

        }


        public void NewLog(Exception ex)
        {
            _statusCode = HttpContext.Current.Response.Status;
            _errMsg = ex.Message;
            _url = HttpContext.Current.Request.RawUrl;
            _referer = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];            
            _browser = HttpContext.Current.Request.Browser.Browser + " " + HttpContext.Current.Request.Browser.Version;
            _os = HttpContext.Current.Request.Browser.Platform;
            _ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //String mask = "INSERT INTO tblAppErrorLog VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
            //String Qry = String.Format(mask, _statusCode, _errMsg, _datetime, _errStack, _url, _referer, _browser, _os, _ip, _siteId);
            //String conString = ConfigurationManager.AppSettings["sqlConnectionString"];
            //SqlHelper.ExecuteNonQuery(conString, CommandType.Text, Qry);

            SqlParameter prmStatusCode = SqlHelper.CreateParameter("@AppHttpStatus", _statusCode);
            SqlParameter prmErrMsg = SqlHelper.CreateParameter("@AppErrorMsg", _errMsg);
            SqlParameter prmUrl = SqlHelper.CreateParameter("@AppUrl", _url);
            SqlParameter prmReferer = SqlHelper.CreateParameter("@AppReferer", _referer);
            SqlParameter prmBrowser = SqlHelper.CreateParameter("@AppBrowser", _browser);
            SqlParameter prmOs = SqlHelper.CreateParameter("@AppOS", _os);
            SqlParameter prmIp = SqlHelper.CreateParameter("@AppIP", _ip);

            // create array of parameters
            SqlParameter[] allParams = { prmStatusCode, prmErrMsg, prmUrl, prmReferer, prmBrowser, prmOs, prmIp };

            // store data into the database
            SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(), CommandType.StoredProcedure, "Usp_LogAppError", allParams);
          
        }       

      

       

    }
}
