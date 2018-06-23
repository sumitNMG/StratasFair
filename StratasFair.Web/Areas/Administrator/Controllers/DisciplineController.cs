using StratasFair.Business;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    [AdminSessionExpire]
    public class DisciplineController : Controller
    {
        // GET: Administrator/Discipline
        public ActionResult Index()
        {
            return BindList();
        }

        List<DisciplineModel> ActiveList = new List<DisciplineModel>();
        List<DisciplineModel> InActiveList = new List<DisciplineModel>();

        private ActionResult BindList()
        {
            DisciplineHelper _Helper = new DisciplineHelper();
            DisciplineModel DisciplineModel = new DisciplineModel();
            var List = _Helper.GetAll();
            ActiveList = List.Where(x => x.Status == 1).OrderByDescending(m=>m.CreatedOn).ToList();
            InActiveList = List.Where(x => x.Status == 0).OrderByDescending(m => m.CreatedOn).ToList();
            return View(Tuple.Create(ActiveList, InActiveList, DisciplineModel));
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.StatusList = AppLogic.BindDDStatus(1);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(DisciplineModel DisciplineModel)
        {

            if (ModelState.IsValid)
            {
                DisciplineHelper disciplineHelper = new DisciplineHelper();
                int count = disciplineHelper.AddUpdate(DisciplineModel);
                if (count == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Record added successfully.");
                    return RedirectToAction("Index");
                }
                else if (count == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Record already exists.");
                    ViewBag.StatusList = AppLogic.BindDDStatus(1);
                    return View(DisciplineModel);
                }
                else
                {
                    ViewBag.StatusList = AppLogic.BindDDStatus(1);
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Error, Please try again.");
                    return View(DisciplineModel);
                }
                
            }
            ViewBag.StatusList = AppLogic.BindDDStatus(1);
            return View(DisciplineModel);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            DisciplineHelper disciplineHelper = new DisciplineHelper();
            DisciplineModel DisciplineModel = new DisciplineModel();
            DisciplineModel = disciplineHelper.GetByID(Id);
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(DisciplineModel.Status));
            return View(DisciplineModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DisciplineModel disciplineModel)
        {
            if (ModelState.IsValid)
            {
                DisciplineHelper disciplineHelper = new DisciplineHelper();

                int count = -1;
                count = disciplineHelper.AddUpdate(disciplineModel);
                if (count == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Record updated successfully.");
                    return RedirectToAction("Index");
                }
                else if (count == 1)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Record already exists.");
                    ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(disciplineModel.Status));
                    return View(disciplineModel);
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Error: Please try again.");
                    ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(disciplineModel.Status));
                    return View(disciplineModel);
                }
            }
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(disciplineModel.Status));
            return View(disciplineModel);
        }

        public ActionResult Delete(int Id)
        {
            DisciplineHelper disciplineHelper = new DisciplineHelper();
            DisciplineModel disciplineModel = new DisciplineModel();
            disciplineModel = disciplineHelper.GetByID(Id);

            int count = disciplineHelper.Delete(disciplineModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deleted successfully.");
            }
            else if (count == -2)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Discipline is associated with vendor. it can't be deleted?");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deletion failed.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Active(int Id)
        {
            DisciplineHelper disciplineHelper = new DisciplineHelper();
            DisciplineModel disciplineModel = new DisciplineModel();
            disciplineModel.DisciplineId = Id;
            disciplineModel.Status = 1;
            int count = disciplineHelper.ActDeact(disciplineModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record activated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record activation failed.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Deactive(int Id)
        {
            DisciplineHelper disciplineHelper = new DisciplineHelper();
            DisciplineModel disciplineModel = new DisciplineModel();
            disciplineModel.DisciplineId = Id;
            disciplineModel.Status = 0;
            int count = disciplineHelper.ActDeact(disciplineModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deactivated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deactivation failed.");
            }
            return RedirectToAction("Index");

        }
    }
}