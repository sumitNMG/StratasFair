using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Web;

namespace StratasFair.Business.CommonHelper
{
    public class ConvertToCSV
    {
        public ConvertToCSV()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Function name :  fncDeleteFile(string FilePath, string FileName)
        /// Description : To Delete Files From The folder
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool fncDeleteFile(string FilePath)
        {
            bool Flag = false;
            try
            {
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(FilePath)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(FilePath));
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
            }
            catch (Exception Ex)
            {
                string Err = Ex.Message.ToString();
            }
            return Flag;
        }

        public string GetRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            string guidResult = System.Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", string.Empty);

            // Make sure length is valid
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            // Return the first length bytes
            return guidResult.Substring(0, length);
        }

        /// <summary>
        /// Function name : FncDownLoadFiles(string filename, string FilePath)
        /// Description : To Download files
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="FilePath"></param>
        public void FncDownLoadFiles(string filename, string FilePath)
        {
            try
            {
                if (filename != "" && FilePath != "")
                {


                    string path = HttpContext.Current.Server.MapPath(FilePath);
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    // to download file if file exist in Uploaded Assignment folder.
                    if (file.Exists)
                    {
                        string basepath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
                        string file6 = FilePath + filename;
                        string url = @file6.ToString();

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                        HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.WriteFile(file.FullName, false);
                        HttpContext.Current.Response.End();
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("File Not Found");
                    }
                }
            }
            catch (Exception Ex)
            {
                string Err = Ex.Message.ToString();
            }
        }

        /// <summary>
        /// Function name : CreateCSVFile(DataTable dt, string strFilePath)
        /// Description : To export datatable in to a csv
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strFilePath"></param>
        public void CreateCSVFile(DataTable dt, string strFilePath)
        {
            try
            {
                // Create the CSV file to which grid data will be exported.
                StreamWriter sw = new StreamWriter(strFilePath.Trim(), false);
                // First we will write the headers.
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                // Now write all the rows.
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString());
                        }
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            catch (Exception Ex)
            {
                string Err = Ex.Message.ToString();
            }
        }

    
    }
}
