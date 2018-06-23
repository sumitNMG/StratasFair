using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    [AdminSessionExpire]
    public class UserController : Controller
    {
     
        // GET: /Admin/User/
        public ActionResult Index()
        {
            return View();
        }


        List<UserModel> ActiveUserList = new List<UserModel>();
        List<UserModel> InActiveUserList = new List<UserModel>();

        public ActionResult List()
        {
            UserModel model = new UserModel();
            return BindUserList(model);
        }

        [HttpPost]
        public ActionResult List([Bind(Prefix = "Item3")] UserModel model)
        {
            return BindUserList(model);
        }

        private ActionResult BindUserList(UserModel model)
        {
            if (TempData["message"] != null)
            {
                model.MessageClass = (string)TempData["MessageClass"];
                model.IsShowMessage = 1;
                model.Message = (string)TempData["message"];
            }
            else
            {
                model.MessageClass = "";
                model.IsShowMessage = 0;
                model.Message = "";
            }

            //get grid parameters from URL/POST (if any)
            var activeGridParameters = GridParameters.GetGridParameters();
            int pageSize = 10;   //displayed rows per page


            model.Flag = 1;
            model.Status = 1;
            AdminUserHelper objAdminUserHelper = new AdminUserHelper();



            ActiveUserList = objAdminUserHelper.GetAdminUserByStatus(model).ToList();  //.Where(x => x.IsSuper != 1)
            var ActiveUser = GetUserDataUsingLINQ(activeGridParameters.Sort,       //order by column
                                        activeGridParameters.SortDirection,   //order by direction
                                        activeGridParameters.Page ?? 1,       //returned page
                                        pageSize, ActiveUserList);               //displayed rows per page


            model.Status = 0;
            var InActiveGridParameters = GridParameters.GetGridParameters();
            InActiveUserList = objAdminUserHelper.GetAdminUserByStatus(model);
            var InActiveUser = GetUserDataUsingLINQ(InActiveGridParameters.Sort,       //order by column
                                        InActiveGridParameters.SortDirection,   //order by direction
                                        InActiveGridParameters.Page ?? 1,       //returned page
                                        pageSize, InActiveUserList);               //displayed rows per page


            //set record count for use in view


            ViewBag.ActiveGridRecordCount = ActiveUserList.Count;
            ViewBag.InActiveGridRecordCount = InActiveUserList.Count;

            return View(Tuple.Create(ActiveUser, InActiveUser, model));
        }


        //get data from datasource using LINQ (sample data access layer)
        private IEnumerable<UserModel> GetUserDataUsingLINQ(string sort, string sortDir, int page, int numRows, List<UserModel> UsersList)
        {
            //List<UserModel> list = UsersList.AsQueryable().Skip((page - 1) * numRows).Take(numRows).ToList();
            List<UserModel> list = UsersList.AsQueryable().ToList();

            int tempSerialStart = 0;// (page - 1) * numRows;            
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SerialNumber = tempSerialStart + i + 1;
            }

            if (!string.IsNullOrEmpty(sort))
                return list.OrderBy(sort + " " + sortDir).AsQueryable();
            else
            {
                return list.AsQueryable().ToList();
            }
        }



        public ActionResult Add()
        {

            UserModel objUserModel = new UserModel();

            RoleModel objRoleModel = new RoleModel();
            objRoleModel.Flag = 3;
            objRoleModel.Status = 1;
            objRoleModel.RoleType = "S";
            RoleHelper objRoleHelper = new RoleHelper();

            // RoleID=1 which is used for super admin
            objUserModel.ListRole = objRoleHelper.GetRoleByStatus(objRoleModel).Where(x => x.RoleId != 1).ToList();
            objUserModel.ListStatus = GetStatus();

            if (TempData["message"] != null)
            {
                objUserModel.MessageClass = (string)TempData["MessageClass"];
                objUserModel.IsShowMessage = 1;
                objUserModel.Message = (string)TempData["message"];
            }
            else
            {
                objUserModel.MessageClass = "";
                objUserModel.IsShowMessage = 0;
                objUserModel.Message = "";
            }
            objUserModel.Status = 1;
            return View(objUserModel);
        }

        public List<SelectListItem> GetStatus()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            itemList.Add(new SelectListItem { Text = "Active", Value = "1" });
            itemList.Add(new SelectListItem { Text = "Inactive", Value = "0" });
            return itemList;
        }

        [HttpPost]
        public ActionResult Add(UserModel model, List<int> ClientMappings)
        {

            if (ModelState.IsValid)
            {
                model.IsShowMessage = 0;
                model.Flag = 1;
                model.CreatedBy = AdminSessionData.AdminUserId;
                model.CreatedFromIp = Request.UserHostAddress;
                model.UserType = 'S';  // for admin user

                AdminUserHelper objAdminUserHelper = new AdminUserHelper();
                int result = objAdminUserHelper.PerformActionOnUser(model);
                if (result > 0)
                {
                    // result = new user id
                    TempData["CommonMessage"] = AppLogic.setMessage(0, "Record added successfully!");
                    return RedirectToAction("List", "User");
                }
                else if (result == -5)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "User name password are same");
                }
                else if (result == -4)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Email address already exists");
                }
                else if (result == -3)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "User name already exists");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error, Please try again");
                }
            }

            RoleModel objRoleModel = new RoleModel();
            objRoleModel.Flag = 3;
            objRoleModel.Status = 1;
            RoleHelper objRoleHelper = new RoleHelper();
            model.ListRole = objRoleHelper.GetRoleByStatus(objRoleModel);
            model.ListStatus = GetStatus();

            return View(model);

        }




        [HttpGet]
        public ActionResult Edit(int? userId, UserModel objUserModel)
        {
            RoleModel objRoleModel = new RoleModel();
            objRoleModel.Flag = 3;
            objRoleModel.Status = 1;
            objRoleModel.RoleType = "S";
            RoleHelper objRoleHelper = new RoleHelper();
            objUserModel.ListRole = objRoleHelper.GetRoleByStatus(objRoleModel);
            objUserModel.ListStatus = GetStatus();


            objUserModel.Flag = 2;
            objUserModel.UserId = (int)userId;
            AdminUserHelper objHelper = new AdminUserHelper();
            objUserModel = objHelper.GetUserById(objUserModel);
            objUserModel.DOB = objUserModel.DOBMMDDYYYY;
            // for clear of the validation class
            ModelState.Clear();

            return View(objUserModel);
        }

        [HttpPost]
        public ActionResult Edit(UserModel model)
        {

            if (ModelState.IsValid)
            {
                model.IsShowMessage = 0;
                model.Flag = 2;
                model.CreatedBy = AdminSessionData.AdminUserId;
                model.CreatedFromIp = Request.UserHostAddress;
                model.UserType = 'S';  // for admin user

                AdminUserHelper objHelper = new AdminUserHelper();
                int result = objHelper.PerformActionOnUser(model);
                if (result == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(0, "Record updated successfully!");
                    return RedirectToAction("List", "User");
                }
                else if (result == -5)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "User name password are same");
                }
                else if (result == -4)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "Email address already exists");
                }
                else if (result == -3)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(1, "User name already exists");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error, please try again");
                }


                return RedirectToAction("List", "User");
            }
            RoleModel objRoleModel = new RoleModel();
            objRoleModel.Flag = 3;
            objRoleModel.Status = 1;
            RoleHelper objRoleHelper = new RoleHelper();
            model.ListRole = objRoleHelper.GetRoleByStatus(objRoleModel);
            model.ListStatus = GetStatus();
            return View(model);

        }


        [HttpGet]
        public ActionResult Delete(int? userId, UserModel model)
        {
            model.Flag = 3;
            model.UserId = (int)userId;
            AdminUserHelper objHelper = new AdminUserHelper();
            int result = objHelper.PerformActionOnUser(model);
            if (result == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(0, "Record deleted successfully!");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error, Please try again");
            }
            return RedirectToAction("List", "User");

        }

        [HttpGet]
        public ActionResult InActivate(int? userId, UserModel model)
        {
            model.Flag = 4;
            model.UserId = (int)userId;
            model.Status = 0;
            AdminUserHelper objHelper = new AdminUserHelper();
            int result = objHelper.PerformActionOnUser(model);
            if (result == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(0, "Record inactivate successfully!");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error, please try again");
            }
            return RedirectToAction("List", "User");

        }

        [HttpGet]
        public ActionResult Activate(int? userId, UserModel model)
        {
            model.Flag = 4;
            model.UserId = (int)userId;
            model.Status = 1;
            AdminUserHelper objHelper = new AdminUserHelper();
            int result = objHelper.PerformActionOnUser(model);
            if (result == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(0, "Record activated successfully!");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error, please try again");
            }
            return RedirectToAction("List", "User", new { @At = "InActiveUser" });

        }

    }
}