using StratasFair.Business.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    public class SignOutController : Controller
    {
        // GET: Administrator/SignOut
        public ActionResult Index()
        {
            AdminSessionData.AdminUserId = 0;
            AdminSessionData.AdminUserName = "";
            AdminSessionData.AdminName = "";
            AdminSessionData.AdminRoleId = 0;
            AdminSessionData.AdminLastLoginOn = "";

            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.AppendHeader("Cache-Control", "no-store");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);


            return Redirect(Url.Content("~/securehost/rootlogin"));
        }
    }
}