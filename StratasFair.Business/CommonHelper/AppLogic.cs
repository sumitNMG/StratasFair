using Ionic.Zip;
using StratasFair.Business.CommonHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Business.CommonHelper
{
    public static class AppLogic
    {

        public static string secureKey = "4C72F08A";

        // just removes all <....> markeup from the text string. this is brute force, and may or may not give
        // the right aesthetic result to the text. it just brute force removes the markeup tags
        public static string StripHtml(String s)
        {
            return Regex.Replace(s, @"<(.|\n)*?>&nbsp;gt&l", string.Empty, RegexOptions.Compiled);
        }

        /// <summary>
        ///  Mask the specifc value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Mask(string value, int length)
        {
            string masked = "****";
            if (!String.IsNullOrEmpty(value) && value.Length > length)
            {
                masked += value.Substring(value.Length - length, length);
            }

            return masked;
        }



        public static List<SelectListItem> GetMonthNumber()
        {
            return new List<SelectListItem>
                       {
                           new SelectListItem {Text = "January (1)", Value = "1"},
                           new SelectListItem {Text = "February (2)", Value = "2"},
                           new SelectListItem {Text = "Month (3)", Value = "3"},
                           new SelectListItem {Text = "April (4)", Value = "4"},
                           new SelectListItem {Text = "May (5)", Value = "5"},
                           new SelectListItem {Text = "June (6)", Value = "6"},
                           new SelectListItem {Text = "July (7)", Value = "7"},
                           new SelectListItem {Text = "August (8)", Value = "8"},
                           new SelectListItem {Text = "September (9)", Value = "9"},
                           new SelectListItem {Text = "October (10)", Value = "10"},
                           new SelectListItem {Text = "November (11)", Value = "11"},
                           new SelectListItem {Text = "December (12)", Value = "12"}
                       };
        }

        public static List<SelectListItem> GetYearNumber()
        {
            int startYear = 2016;
            int endYear = DateTime.Now.Year;
            List<SelectListItem> itemList = new List<SelectListItem>();
            for (int i = startYear; i <= endYear; i++)
            {
                itemList.Add(new SelectListItem { Text = i.ToString().Substring(2, 2), Value = i.ToString() });
            }
            return itemList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">message for print</param>
        /// <param name="status">0 for error, 1 for success</param>
        /// <returns></returns>
        public static String GetMessage(string message, int status)
        {
            //string strMsgClass = "MsgGreen";
            //if (status == 0)
            //    strMsgClass = "MsgRed";

            string strMsgClass = "alert alert-success alert-dismissable";
            string strIcon = "fa fa-check";
            if (status == 0)
            {
                strMsgClass = "alert alert-danger alerset-dismissable";
                strIcon = "fa fa-ban";
            }


            return "<div class=\"alert-mesg\">" +
                     "<div class=\"" + strMsgClass + "\">" +
                         "<i class=\"" + strIcon + "\"></i>" +
                         "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">" +
                             "&times;" +
                         "</button>" +
                         message +
                      "</div>" +
                     "</div>";

            //return "<span class=\"" + strMsgClass + "\" >" + message + "</span>";
        }


        /// <summary>
        /// function to read a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String readingFile(string path)
        {
            StreamReader fp = File.OpenText(path);
            string result = fp.ReadToEnd();
            fp.Close();
            return result;
        }

        public static bool IsFileJPG_GIF_PNG(HttpPostedFileBase file)
        {
            var extension = Path.GetExtension(file.FileName ?? string.Empty);
            var validExtensions = new[] { ".jpg", ".jpeg", ".gif", ".png" };
            var isValid = validExtensions.Contains(extension.ToLower(), StringComparer.OrdinalIgnoreCase);
            return isValid;
        }

        public static bool IsFilePDF(HttpPostedFileBase file)
        {
            var extension = Path.GetExtension(file.FileName ?? string.Empty);
            var validExtensions = new[] { ".pdf" };
            var isValid = validExtensions.Contains(extension.ToLower(), StringComparer.OrdinalIgnoreCase);
            return isValid;
        }

        public static string FixDescription(string Desc, int Length)
        {
            string strNewDesc = "";
            strNewDesc = (new System.Text.RegularExpressions.Regex("<[^>]*>")).Replace(Desc, "");
            if (strNewDesc.Length > Length)
            {
                strNewDesc = strNewDesc.Substring(0, Length) + "...more";
            }
            return strNewDesc;
        }

        public static string FixDescriptionWithoutMore(string Desc, int Length)
        {
            string strNewDesc = "";
            strNewDesc = (new System.Text.RegularExpressions.Regex("<[^>]*>")).Replace(Desc, "");
            if (strNewDesc.Length > Length)
            {
                strNewDesc = strNewDesc.Substring(0, Length);
            }
            return strNewDesc;
        }

        public static List<SelectListItem> BindDDStatus(bool IsSelected)
        {
            List<SelectListItem> status = new List<SelectListItem>();
            if (IsSelected == true)
            {
                status.Add(new SelectListItem() { Text = "Active", Value = "True", Selected = IsSelected });
                status.Add(new SelectListItem() { Text = "In-Active", Value = "False" });
            }
            else
            {
                status.Add(new SelectListItem() { Text = "Active", Value = "True" });
                status.Add(new SelectListItem() { Text = "In-Active", Value = "False", Selected = IsSelected });
            }
            return status;
        }

        public static List<SelectListItem> BindDDStatus(int IsSelected)
        {
            List<SelectListItem> status = new List<SelectListItem>();
            if (IsSelected == 1)
            {
                status.Add(new SelectListItem() { Text = "Active", Value = "1", Selected = true });
                status.Add(new SelectListItem() { Text = "In-Active", Value = "0" });
            }
            else
            {
                status.Add(new SelectListItem() { Text = "Active", Value = "1" });
                status.Add(new SelectListItem() { Text = "In-Active", Value = "0", Selected = true });
            }
            return status;
        }


        public static List<SelectListItem> BindSubscriptionValidity()
        {
            List<SelectListItem> subscriptionValidityType = new List<SelectListItem>();

            subscriptionValidityType.Add(new SelectListItem() { Text = "Monthly (30 Days)", Value = "M" });
            subscriptionValidityType.Add(new SelectListItem() { Text = "Quarterly (90 Days)", Value = "Q" });
            subscriptionValidityType.Add(new SelectListItem() { Text = "Half Yearly (180 Days)", Value = "H" });
            subscriptionValidityType.Add(new SelectListItem() { Text = "Yearly (365 Days)", Value = "Y" });

            return subscriptionValidityType;
        }




        public static string setMessage(int count, string Msg)
        {
            string Message = String.Empty;
            if (count == 0)
            {
                Message = "<div class='alert alert-success alert-dismissable'><i class='fa fa-check'></i>" +
                "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                Msg + "</div>";
            }
            else if (count == 1)
            {
                Message = "<div class='alert alert-warning alert-dismissable'><i class='fa fa-exclamation-triangle'></i>" +
                "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                Msg + "</div>";
            }
            else
            {
                Message = "<div class='alert alert-danger alert-dismissable'><i class='fa fa-check'></i>" +
               "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
               Msg + "</div>";
            }
            return Message;
        }

        public static string setFrontendMessage(int count, string Msg)
        {
            string Message = "<div class='row'> <div class='col-lg-12 col-md-12 col-sm-12 col-xs-12 alert-box'>";
            if (count == 0)
            {
                Message = Message + "<div class='alert alert-success alert-white rounded'> <button type='button' data-dismiss='alert' aria-hidden='true' class='close'>X</button><div class='icon'> <i class='fa fa-check'></i></div>" + Msg + "</div>";
            }
            else if (count == 1)
            {
                Message = Message + "<div class='alert alert-warning alert-white rounded'> <button type='button' data-dismiss='alert' aria-hidden='true' class='close'>X</button><div class='icon'> <i class='fa fa-warning'></i></div> " + Msg + " </div>";
            }
            else if (count == 2)
            {
                Message = Message + "<div class='alert alert-info alert-white rounded'> <button type='button' data-dismiss='alert' aria-hidden='true' class='close'>X</button><div class='icon'> <i class='fa fa-info-circle'></i></div>" + Msg + "</div>";
            }
            else if (count == -1)
            {
                Message = Message + "<div class='alert alert-danger alert-white rounded'> <button type='button' data-dismiss='alert' aria-hidden='true' class='close'>X</button><div class='icon'> <i class='fa fa-info-circle'></i></div>" + Msg + "</div>";
            }
            Message = Message + "</div></div>";
            return Message;
        }

        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }




        public static string IsStringDouble(string str)
        {
            double n = 0;
            bool isDouble = double.TryParse(str, out n);
            if (isDouble)
            {
                if (n < 0.1)
                {
                    n = 0;
                }
                return n.ToString();
            }
            else
            {
                return str;
            }
        }

        public static int StringToInt(string str)
        {
            int n = 0;
            bool isInt = int.TryParse(str, out n);
            return n;
        }

        /// <summary>
        /// generate password for users in encrypted format
        /// </summary>
        /// <returns></returns>
        public static string EncryptPassword()
        {
            Encrypt64 encrypt = new Encrypt64();
            Random r = new Random();
            string password = encrypt.Encrypt("SF" + r.Next(11111, 99999).ToString(), secureKey);
            return password;
        }


        public static string EncryptPasswordString(string password)
        {
            Encrypt64 encrypt = new Encrypt64();
            Random r = new Random();
            return encrypt.Encrypt(password, secureKey);
        }



        /// <summary>
        /// decrypt password for users
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecryptPassword(string password)
        {
            Encrypt64 encrypt = new Encrypt64();
            string key = encrypt.Decrypt(password, secureKey);
            return key;
        }


        public static DateTime CalculateSubscriptionExpiryDate(DateTime dt, string validity)
        {
            DateTime returnDate = DateTime.UtcNow;
            switch (validity.ToLower())
            {
                case "m":
                    returnDate = dt.AddDays(30);
                    break;
                case "q":
                    returnDate = dt.AddDays(90);
                    break;
                case "h":
                    returnDate = dt.AddDays(180);
                    break;
                case "y":
                    returnDate = dt.AddDays(365);
                    break;
                default:
                    returnDate = dt;
                    break;
            }

            return returnDate;
        }



        public static string GetDateFromatForCsv(DateTime? dt)
        {
            try
            {
                if (dt == null)
                {
                    return "";
                }
                else
                {
                    return String.Format("{0:dd/MM/yyyy}", dt);
                }

            }
            catch
            {
                return "";
            }

        }

        public static string GetDateFromat(DateTime? dt)
        {
            try
            {
                if (dt == null)
                {
                    return "";
                }
                else
                {
                    return String.Format("{0:dd MMM yyyy}", dt);
                }

            }
            catch
            {
                return "";
            }

        }

        public static string FormatDecimal(decimal? val)
        {
            string strDecimal = Math.Round(Convert.ToDecimal(val), 2).ToString("G29");
            if (strDecimal.Contains("."))
            {
                if (strDecimal.Split('.')[1].Length == 1)
                {
                    strDecimal = strDecimal + "0";
                }
            }
            return strDecimal;
        }

        public static string GetStatus(string val)
        {
            try
            {
                if (val == "1")
                {
                    return "Active";
                }
                else
                {
                    return "Deactive";
                }

            }
            catch
            {
                return "";
            }
        }

        public static string GetYesNo(bool val)
        {
            if (val == true)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }



        public static List<SelectListItem> BindContactEnquiryType()
        {
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem() { Text = "General Enquiry", Value = "General Enquiry", Selected = true });
            status.Add(new SelectListItem() { Text = "Content Request", Value = "Content Request" });
            status.Add(new SelectListItem() { Text = "Feedback", Value = "Feedback" });
            status.Add(new SelectListItem() { Text = "Complaints", Value = "Complaints" });
            status.Add(new SelectListItem() { Text = "Technical Fault", Value = "Technical Fault" });
            return status;
        }





        public static string ReplaceIframeDimension(string iFrame, string type)
        {
            if (type == "admin")
            {
                iFrame = Regex.Replace(iFrame, @"width=""[^\s]*""", "width=\"200\"");
                iFrame = Regex.Replace(iFrame, @"height=""[^\s]*""", "height=\"150\"");
            }
            else if (type == "home")
            {
                iFrame = Regex.Replace(iFrame, @"width=""[^\s]*""", "width=\"200\"");
                iFrame = Regex.Replace(iFrame, @"height=""[^\s]*""", "height=\"150\"");
            }

            return iFrame;
        }

        public static string IframeYouTubeVideoImage(string iFrame)
        {
            Regex regexPattern = new Regex(@"src=\""\S+/embed/(?<videoId>\w+)");
            Match videoIdMatch = regexPattern.Match(iFrame);
            string videoID = string.Empty;
            if (videoIdMatch.Success)
            {
                videoID = videoIdMatch.Groups["videoId"].Value;
            }
            return "https://img.youtube.com/vi/" + videoID + "/0.jpg";
        }

        public static string IframeYouTubeVideoId(string iFrame)
        {
            Regex regexPattern = new Regex(@"src=\""\S+/embed/(?<videoId>\w+)");
            Match videoIdMatch = regexPattern.Match(iFrame);
            string videoID = string.Empty;
            if (videoIdMatch.Success)
            {
                videoID = videoIdMatch.Groups["videoId"].Value;
            }
            return videoID;
        }

        public static string IsUserMenuActive(string returnMenu)
        {
            string CurrentURL = HttpContext.Current.Request.Url.AbsoluteUri;
            if (CurrentURL.ToLower().Contains(returnMenu))
            {
                return "class=active";
            }

            else
            {
                return "";
            }
        }


        /// <summary>
        /// Path of the specified folder for deleting all folders and files within it + root folder
        /// </summary>
        /// <param name="mypath">Folder mapped path - to be deleted</param>
        /// <returns>true, false</returns>
        public static bool DeleteDirectoryContent(string mypath)
        {
            try
            {
                // deleting files in the specified folder
                string[] files = Directory.GetFiles(mypath);
                if (files.Length > 0)
                {
                    foreach (string filepath in files)
                    {
                        string filename = Path.GetFileName(filepath);
                        if (System.IO.File.Exists(Path.Combine(mypath, filename)))
                        {
                            System.IO.File.Delete(Path.Combine(mypath, filename));
                        }
                    }
                }

                // deleting folders in the specified folder - recursive
                string[] folders = Directory.GetDirectories(mypath);
                if (folders.Length > 0)
                {
                    foreach (string folderpath in folders)
                    {
                        DeleteDirectoryContent(folderpath);
                    }
                }

                // delete the folder
                if (Directory.Exists(mypath))
                {
                    Directory.Delete(mypath);
                }

                return true;
            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                return false;
            }
        }


        /// <summary>
        /// Unzip the file and delete the zip file
        /// </summary>
        /// <param name="mypath">Path of the root folder</param>
        /// <param name="myfile">File name with extension</param>
        /// <param name="isFileDelete">Zip file is to be delete or not</param>
        /// <returns></returns>
        public static bool Unzip(string mypath, string myfile, bool isFileDelete)
        {
            try
            {
                string filename = Path.Combine(mypath + "/" + myfile);

                using (ZipFile zip = ZipFile.Read(filename))
                {
                    zip.ExtractAll(mypath, ExtractExistingFileAction.OverwriteSilently);
                }
                // Get dirs recursively and copy files
                string[] folders = Directory.GetDirectories(mypath + "/" + Path.GetFileNameWithoutExtension(filename));
                foreach (string folderpath in folders)
                {
                    string name = Path.GetFileName(folderpath);
                    Directory.Move(folderpath, Path.Combine(mypath, name));
                }

                // Get Files & Copy
                string[] files = Directory.GetFiles(mypath + "/" + Path.GetFileNameWithoutExtension(filename));
                foreach (string filepath in files)
                {
                    string name = Path.GetFileName(filepath);
                    System.IO.File.Move(filepath, Path.Combine(mypath, name));
                }

                if (Directory.Exists(mypath + "/" + Path.GetFileNameWithoutExtension(filename)))
                {
                    Directory.Delete(mypath + "/" + Path.GetFileNameWithoutExtension(filename));
                }

                if (isFileDelete)
                {
                    if (System.IO.File.Exists(Path.Combine(mypath, Path.GetFileName(filename))))
                    {
                        System.IO.File.Delete(Path.Combine(mypath, Path.GetFileName(filename)));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                return false;
            }

        }


        public static string RemoveSpecialCharacters(string str, bool isRemoveSpace)
        {
            if (isRemoveSpace)
                str = string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string GenerateRandomString(int stringLength)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            characters += alphabets + small_alphabets + numbers;
            string otp = string.Empty;
            for (int i = 0; i < stringLength; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }

            return otp;
        }

        


        public static string FixDescriptionPrefix(string Desc, int Length, long val)
        {
            string strNewDesc = "";
            strNewDesc = (new System.Text.RegularExpressions.Regex("<[^>]*>")).Replace(Desc, "");
            if (strNewDesc.Length > Length)
            {
                strNewDesc = strNewDesc.Substring(0, Length) + "<span id='spnDot_" + val + "'>...</span>";
            }
            return strNewDesc;
        }

        public static string EndDescriptionSuffix(string Desc, int Length)
        {
            string strNewDesc = "";
            strNewDesc = (new System.Text.RegularExpressions.Regex("<[^>]*>")).Replace(Desc, "");
            if (strNewDesc.Length > Length)
            {
                strNewDesc = strNewDesc.Substring(Length, strNewDesc.Length - Length);
            }
            else
            {
                strNewDesc = "";
            }
            return strNewDesc;
        }
       
    }
}
