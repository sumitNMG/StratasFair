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
    public class ProfileController : Controller
    {
        // GET: Administrator/Profile
        public ActionResult Index()
        {

            AdminUserHelper objHelper = new AdminUserHelper();
            ProfileModel profileModel = new ProfileModel();

            profileModel = objHelper.GetAdminUserProfileById(AdminSessionData.AdminUserId);

            return View(profileModel);
        }


        [HttpPost]
        public ActionResult Index(ProfileModel profileModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    profileModel.CreatedBy = AdminSessionData.AdminUserId;
                    profileModel.CreatedFromIp = Request.UserHostAddress;


                    AdminUserHelper objHelper = new AdminUserHelper();
                    int result = objHelper.UpdateProfile(profileModel);
                    if (result == 0)
                    {
                        AdminSessionData.AdminUserName = profileModel.LoginId;
                        AdminSessionData.AdminName = profileModel.FirstName + " " + profileModel.LastName;

                        TempData["CommonMessage"] = AppLogic.setMessage(0, "Record updated successfully.");
                        // return RedirectToAction("List", "User");
                    }

                    else if (result == -4)
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(1, "Email address already exists.");
                    }
                    else if (result == -3)
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(1, "User name already exists.");
                    }
                    else
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error, please try again.");
                    }
                }
            }
            catch (Exception ex)
            {

                profileModel.IsShowMessage = 1;
                profileModel.Message = ex.Message;
                profileModel.MessageClass = "MsgRed";
            }

            return View(profileModel);

        }
    }
}