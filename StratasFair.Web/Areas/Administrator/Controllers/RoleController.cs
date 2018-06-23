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
    public class RoleController : Controller
    {
        // GET: Administrator/Role
        public ActionResult Index()
        {
            return View();
        }


        //Role Section

        public ActionResult Add()
        {
            RoleModel roleModel = new RoleModel();
            return View(roleModel);
        }

        [HttpPost]
        public ActionResult Add(RoleModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.IsShowPrivillage = 0;
                    model.IsShowMessage = 0;
                    model.Flag = 1;
                    model.CreatedBy = AdminSessionData.AdminUserId;
                    model.CreatedFromIp = Request.UserHostAddress;
                    model.RoleType = "S";

                    RoleHelper objRole = new RoleHelper();
                    int result = objRole.InsertRole(model);
                    if (result > 0)
                    {
                        model.RoleId = result;
                        model.RoleName = "";
                        model.RoleDescription = "";
                        model.MenuHTML = MenuHelper.GetModuleForRoles();
                        model.IsShowPrivillage = 1;
                        TempData["CommonMessage"] = AppLogic.setMessage(0, "Role added successfully. Please Add privileges.");
                    }
                    else if (result == -2)
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(1, "Record already exists.");
                    }
                    else
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error, Please try again.");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(-1, "Error: " + ex.Message);
                return View(model);
            }
        }


        [HttpGet]
        public ActionResult Edit(int? RoleId, RoleModel model)
        {
            model.Flag = 2;
            model.RoleId = (int)RoleId;
            model.MenuHTML = MenuHelper.GetNewEditModuleForRoles((int)RoleId);
            RoleHelper objHelper = new RoleHelper();
            model = objHelper.GetRoleById(model);

            // for clear of the validation class
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                model.Flag = 2;
                model.CreatedBy = AdminSessionData.AdminUserId;
                model.CreatedFromIp = Request.UserHostAddress;

                RoleHelper objRole = new RoleHelper();
                int result = objRole.UpdateRole(model);
                if (result == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(result, "Record updated successfully.");
                    return RedirectToAction("List", "Role");
                }
                else if (result == -3)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(result, "Record already exists.");
                }
                else if (result == -4)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(result, "Admin email exists.");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(result, "Error, Please try again.");
                }
                return RedirectToAction("List", "Role");
            }
            return View(model);

        }



        List<RoleModel> ActiveRolesList = new List<RoleModel>();
        List<RoleModel> InActiveRolesList = new List<RoleModel>();

        public ActionResult List(string msg)
        {
            RoleModel model = new RoleModel();

            //get grid parameters from URL/POST (if any)
            var activeGridParameters = GridParameters.GetGridParameters();
            int pageSize = 10;   //displayed rows per page


            model.Flag = 1;
            model.Status = 1;
            model.RoleType = "S";
            RoleHelper objHelper = new RoleHelper();



            ActiveRolesList = objHelper.GetRoleByStatus(model);
            var ActiveRole = GetDataUsingLINQ(activeGridParameters.Sort,       //order by column
                                        activeGridParameters.SortDirection,   //order by direction
                                        activeGridParameters.Page ?? 1,       //returned page
                                        pageSize, ActiveRolesList);               //displayed rows per page


            model.Status = 0;
            var InActiveGridParameters = GridParameters.GetGridParameters();
            InActiveRolesList = objHelper.GetRoleByStatus(model);
            var InActiveRole = GetDataUsingLINQ(InActiveGridParameters.Sort,       //order by column
                                        InActiveGridParameters.SortDirection,   //order by direction
                                        InActiveGridParameters.Page ?? 1,       //returned page
                                        pageSize, InActiveRolesList);               //displayed rows per page


            //set record count for use in view


            ViewBag.ActiveGridRecordCount = ActiveRolesList.Count;
            ViewBag.InActiveGridRecordCount = InActiveRolesList.Count;

            return View(Tuple.Create(ActiveRole, InActiveRole, model));
        }

        //get data from datasource using LINQ (sample data access layer)
        private IEnumerable<RoleModel> GetDataUsingLINQ(string sort, string sortDir, int page, int numRows, List<RoleModel> RolesList)
        {

            //List<RoleModels> list = RolesList.AsQueryable().Skip((page - 1) * numRows).Take(numRows).ToList();
            List<RoleModel> list = RolesList.AsQueryable().ToList();


            int tempSearilStart = 0;// (page - 1) * numRows;

            for (int i = 0; i < list.Count; i++)
            {
                list[i].SerialNumber = tempSearilStart + i + 1;
            }

            if (!string.IsNullOrEmpty(sort))
                return list.OrderBy(sort + " " + sortDir).ToList();
            else
            {
                return list.AsQueryable().ToList();
            }
        }


        [HttpGet]
        public ActionResult Delete(int? roleId, RoleModel model)
        {
            model.Flag = 3;
            model.RoleId = (int)roleId;
            RoleHelper objHelper = new RoleHelper();
            int result = objHelper.DeleteRole(model);
            if (result == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(result, "Record deleted successfully.");
            }
            else if (result == 1)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(result, "Record deletion failed. Please delete all the related records first.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(result, "Record deletion failed.");
            }
            return RedirectToAction("List", "Role");

        }

        [HttpGet]
        public ActionResult InActivate(int? roleId, RoleModel model)
        {
            model.Flag = 4;
            model.RoleId = (int)roleId;
            model.Status = 0;
            RoleHelper objHelper = new RoleHelper();
            int result = objHelper.ChangeStatusRole(model);
            if (result == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(result, "Record deactivated successfully.");

            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(result, "Record deactivation failed.");
            }
            return RedirectToAction("List", "Role");

        }

        [HttpGet]
        public ActionResult Activate(int? roleId, RoleModel model)
        {
            model.Flag = 4;
            model.RoleId = (int)roleId;
            model.Status = 1;
            RoleHelper objHelper = new RoleHelper();
            int result = objHelper.ChangeStatusRole(model);
            if (result == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(result, "Record activated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(result, "Record activation failed.");
            }
            return RedirectToAction("List", "Role", new { @At = "InActiveRoles" });

        }



        [HttpPost]
        public ActionResult AddPrivillage(string ModuleId, int RoleId)
        {
            try
            {
                int result = -1;
                RoleModel objRoleModel = new RoleModel();
                objRoleModel.Flag = 1;
                objRoleModel.RoleId = RoleId;
                objRoleModel.ModuleIds = ModuleId;
                objRoleModel.CreatedBy = AdminSessionData.AdminUserId;
                objRoleModel.CreatedFromIp = Request.UserHostAddress;

                RoleHelper objRoleHelper = new RoleHelper();
                result = objRoleHelper.InsertModuleOnRole(objRoleModel);
                if (result == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(result, "Record added successfully.");
                    return Json(new { ok = true, data = "", message = "Privileges added successfully." });
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(result, "Error, Please try again.");
                    return Json(new { ok = false, data = "", message = "Error, Please try again." });
                }
            }
            catch
            {
                return Json(new { ok = false, data = "", message = "" });
            }
        }
    }
}