using StratasFair.BusinessEntity.Admin;
using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    public class LogOnController : Controller
    {
        // GET: Administrator/LogOn
        [HttpGet]
        public ActionResult Index()
        {
            if (AdminSessionData.AdminUserId != 0)
            {
                return Redirect(Url.Content("~/Login"));
            }
            else
            {
                LogOnModel model = new LogOnModel();
                //model.RememberMe = true;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Index(LogOnModel model, string returnUrl)
        {
            try
            {
                string pwd = model.Password;
                if (ModelState.IsValid)
                {
                    model.UserType = "S";  // Admin Users
                    LoginHelper loginhelper = new LoginHelper();
                    string message = string.Empty;
                    int result = -1;
                    DataTable dt = loginhelper.AuthenticateUser(model, out result);
                    if (result == 1)
                    {
                        AdminSessionData.AdminUserName = model.UserName;
                        AdminSessionData.AdminName = dt.Rows[0]["Name"].ToString();
                        AdminSessionData.AdminUserId = Convert.ToInt32(dt.Rows[0]["userId"].ToString());
                        AdminSessionData.AdminRoleId = Convert.ToInt32(dt.Rows[0]["roleId"].ToString());
                        AdminSessionData.AdminCreatedOn = dt.Rows[0]["createdOn"].ToString();
                        AdminSessionData.AdminLastLoginOn = dt.Rows[0]["LastLogin"].ToString();
                        AdminSessionData.AdminRoleName = dt.Rows[0]["RoleName"].ToString();
                        
                        if (model.RememberMe)
                        {
                            HttpCookie cookie = new HttpCookie("adminLogin");
                             
                            cookie.Values.Add("LoginName", model.UserName.Trim());
                            cookie.Values.Add("Password", pwd.Trim());
                            cookie.Values.Add("DtExp", DateTime.Now.AddDays(30).ToString());
                            cookie.Expires = DateTime.Now.AddDays(30);
                            Response.Cookies.Add(cookie);
                        }
                        else
                        {
                            if (Request.Cookies["adminLogin"] != null)
                            {
                                HttpCookie objCookie = Request.Cookies["adminLogin"];
                                objCookie.Values.Add("DtExp", DateTime.Now.AddDays(-5).ToString());
                                objCookie.Expires = DateTime.Now.AddDays(-5);
                                Response.Cookies.Add(objCookie);
                            }
                        }



                        return RedirectToAction("Index", "dashboard");
                    }
                    else if (result == 0)
                    {
                        model.Message = "Your account is inactive";
                    }
                    else if (result == -2)
                    {
                        model.Message = "Your password is invalid.";
                    }
                    else if (result == -3)
                    {
                        model.Message = "Your Username is invalid.";
                    }
                    else
                    {
                        model.Message = "Invalid Username or Password";
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                new AppError().LogMe(ex);
                model.Message = "Invalid LoginId or Password";
                return View(model);
            }
        }


        public ActionResult ForgetPassword()
        {
            ForgetPasswordModel ObjModel = new ForgetPasswordModel();
            return View(ObjModel);
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgetPasswordModel objForgetPasswordModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    objForgetPasswordModel.Flag = 4;
                    LoginHelper loginhelper = new LoginHelper();
                    DataTable dt = loginhelper.AdminPasswordReminder(objForgetPasswordModel);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int sucess = EmailSender.FncSendAdminPasswordReminderMail(dt.Rows[0]["emailid"].ToString(), dt.Rows[0]["password"].ToString(), dt.Rows[0]["loginid"].ToString(), dt.Rows[0]["username"].ToString(), null);
                        if (sucess == 1)
                        {
                            ViewBag.Style = "color:Green !important";
                            objForgetPasswordModel.Message = "Your password has been sent. Please check your inbox. It usually takes a few minutes but when we're busy.it may take longer.";
                        }
                        else
                        {
                            ViewBag.Message = "color:Red !important";
                            objForgetPasswordModel.Message = "Please try again.";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "color:Red !important";
                        objForgetPasswordModel.Message = "Email address does not exists. Please try again.";
                    }

                }
                return View(objForgetPasswordModel);
            }
            catch
            {
                ViewBag.Message = "color:Red !important";
                objForgetPasswordModel.Message = "Email address does not exists. Please try again.";
                return View(objForgetPasswordModel);
            }
        }

    }
}