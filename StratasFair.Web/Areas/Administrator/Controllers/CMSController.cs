using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    [AdminSessionExpire]
    public class CMSController : Controller
    {
        #region //Manage Page Code//

        List<CMSModel> ActiveCMSList = new List<CMSModel>();
        List<CMSModel> InActiveCMSList = new List<CMSModel>();
        // GET: Administrator/CMS        
        public ActionResult Index()
        {
            return BindCMSList();
        }

        private ActionResult BindCMSList()
        {
            CMSHelper cmsHelper = new CMSHelper();
            CMSModel model = new CMSModel();
            model.Flag = 1;
            var CMSList = cmsHelper.GetAll(model.Flag).ToList();
            ActiveCMSList = CMSList.Where(x => x.Status == 1).ToList();
            InActiveCMSList = CMSList.Where(x => x.Status == 0).ToList();
            return View(Tuple.Create(ActiveCMSList, InActiveCMSList, model));
        }


        public ActionResult Edit(int Id)
        {
            CMSHelper cmsHelper = new CMSHelper();
            CMSModel model = new CMSModel();
            model = cmsHelper.GetByID(Id);
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(model.Status));
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CMSModel objCMS)
        {
            CMSHelper cmsHelper = new CMSHelper();
            int count = cmsHelper.UpateCMS(objCMS);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "CMS Updated Successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "CMS Update Failed.");
            }
            return RedirectToAction("Index");

        }

        public ActionResult InActivate(int Id)
        {
            CMSHelper cmsHelper = new CMSHelper();
            CMSModel model = new CMSModel()
            {
                Flag = 3,
                ModifiedBy = AdminSessionData.AdminUserId,
                ContentId = Id,
                Status = 0
            };
            int count = cmsHelper.ActiveDeactiveCMS(model);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "CMS Deactivated Successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "CMS Deactivation failed.");
            }
            return RedirectToAction("Index");
        }


        public ActionResult Activate(int Id)
        {
            CMSModel model = new CMSModel();
            CMSHelper cmsHelper = new CMSHelper();
            model.Flag = 3;
            model.ModifiedBy = AdminSessionData.AdminUserId;
            model.ContentId = Id;
            model.Status = 1;
            int count = cmsHelper.ActiveDeactiveCMS(model);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Template Activated Successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Template Activation failed.");
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}