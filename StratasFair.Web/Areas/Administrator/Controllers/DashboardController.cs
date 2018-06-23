using StratasFair.Business;
using StratasFair.BusinessEntity.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Administrator/DashBoard
        [HttpGet]
        public ActionResult Index()
        {
            DashBoardModel dashBoardModel = new DashBoardModel();

            StrataBoardHelper helper = new StrataBoardHelper();
            dashBoardModel.StrataBoardExpiringList = helper.GetStrataBoardToBeExpired();
            return View(dashBoardModel);
        }


        [HttpGet]
        public ActionResult NoAccess()
        {
            return View();
        }
    }
}