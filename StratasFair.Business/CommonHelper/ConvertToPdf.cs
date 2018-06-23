using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.CommonHelper
{
    public class ConvertToPdf
    {


        public void FncDownLoadPDFFile(string FilePath)
        {
            try
            {
                if (FilePath != "")
                {

                    string path = HttpContext.Current.Server.MapPath(FilePath);
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    if (file.Exists)
                    {
                        string basepath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

                        HttpContext.Current.Response.ContentType = "pdf/application";
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


        public bool FncDeleteFile(string FilePath)
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


        public void CreatePDFFile(DataTable dt, string strFilePath)
        {

            try
            {

                FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                Rectangle rec = new Rectangle(PageSize.A4);
                Document doc = new Document(rec, 36, 72, 108, 180);

                PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                doc.Open();
                Paragraph para = new Paragraph("Hello World Hello World Hello World Hello World Hello World Hello World Hello World Hello World Hello World Hello World Hello World");

                doc.Add(para);
             

                doc.Close();


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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="table">Table object where cell is to be inserted</param>
        /// <param name="text">Text of the cell</param>
        /// <param name="rowspan">How many rowspan required for corresponding cell</param>
        /// <param name="fontsize">What will be the font size for the cell text</param>
        /// <param name="halign">Horizontal align may be C-center, L-left, R-right</param>
        /// <param name="valign">Vertical align may be C-center, L-left, R-right</param>
        public static void AddCell(PdfPTable table, string text, int rowspan, int fontsize, string halign, string valign)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, fontsize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            PdfPCell cell = new PdfPCell(new Phrase(text, times));
            cell.Rowspan = rowspan;
            if (halign.ToLower() == "c") { cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER; } else if (halign.ToLower() == "l") { cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT; } else if (halign.ToLower() == "r") { cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT; }
            if (valign.ToLower() == "c") { cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE; } else if (valign.ToLower() == "l") { cell.VerticalAlignment = PdfPCell.ALIGN_LEFT; } else if (valign.ToLower() == "r") { cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT; }
         
            table.AddCell(cell);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="table">Table object where cell is to be inserted</param>
        /// <param name="text">Text of the cell</param>
        /// <param name="rowspan">How many rowspan required for corresponding cell</param>
        /// <param name="fontsize">What will be the font size for the cell text</param>
        /// <param name="halign">Horizontal align may be C-center, L-left, R-right</param>
        /// <param name="valign">Vertical align may be C-center, L-left, R-right</param>
        public static void AddHeaderCell(PdfPTable table, string text, int rowspan, int fontsize, string halign, string valign)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, fontsize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            PdfPCell cell = new PdfPCell(new Phrase(text, times));
            cell.Rowspan = rowspan;
            if (halign.ToLower() == "c") { cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER; } else if (halign.ToLower() == "l") { cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT; } else if (halign.ToLower() == "r") { cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT; }
            if (valign.ToLower() == "c") { cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE; } else if (valign.ToLower() == "l") { cell.VerticalAlignment = PdfPCell.ALIGN_LEFT; } else if (valign.ToLower() == "r") { cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT; }

            table.AddCell(cell);
        }
    }


}
