using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: Administrator/ChangePassword
        public ActionResult Index()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ChangePasswordModel model)
        {
            try
            {
                AdminUserHelper objUserHelper = new AdminUserHelper();
                int val = objUserHelper.ChangeAdminPassword(ref model);
                if (val == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(-1, "Please enter correct old password.");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(0, model.Message = model.Message);
                }
            }
            catch (Exception ex)
            {

                model.IsShowMessage = 1;
                model.Message = ex.Message;
                model.MessageClass = "MsgRed";
            }
            return View(model);
        }
    }
}