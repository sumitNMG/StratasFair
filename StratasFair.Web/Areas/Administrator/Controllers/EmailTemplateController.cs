using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
    [AdminSessionExpire]
    public class EmailTemplateController : Controller
    {
        #region //Manage Email Template Code//

        List<EmailTemplateModel> ActiveTemplateList = new List<EmailTemplateModel>();
        List<EmailTemplateModel> InActiveTemplateList = new List<EmailTemplateModel>();
        public ActionResult Index()
        {
            return BindEmailTemplate();
        }

        private ActionResult BindEmailTemplate()
        {
            CMSHelper cmsHelper = new CMSHelper();
            EmailTemplateModel objEmail = new EmailTemplateModel();
            var List = cmsHelper.GetAllTemplates(1).ToList();
            ActiveTemplateList = List.Where(x => x.Status == 1).ToList();
            InActiveTemplateList = List.Where(x => x.Status == 0).ToList();
            return View("Index", Tuple.Create(ActiveTemplateList, InActiveTemplateList, objEmail));
        }

        public ActionResult Edit(int Id)
        {
            string rootPath = ConfigurationManager.AppSettings["WebsiteRootPathAdmin"];
            CMSHelper cmsHelper = new CMSHelper();

            EmailTemplateModel model = new EmailTemplateModel();

            model = cmsHelper.GetTemplateByID(Id);
            // model.ConfigValue = model.ConfigValue.Replace("{BASEPATH}", rootPath);
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(model.Status));
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EmailTemplateModel objEmail)
        {
            CMSHelper cmsHelper = new CMSHelper();
            int count = cmsHelper.UpateTemplate(objEmail);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Email Template Updated Successfully.");
                return RedirectToAction("Index");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Update Failed.");
                ModelState.AddModelError("ConfigValue", "Config Value is required.");
                return View(objEmail);
            }
        }

        public ActionResult DeActTemplate(int Id)
        {
            CMSHelper cmsHelper = new CMSHelper();
            int count = cmsHelper.ActDeActTemplate(Id, 0);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Template deactivated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Template deactivation failed.");
            }
            return RedirectToAction("Index");

        }

        public ActionResult ActTemplate(int Id)
        {
            CMSHelper cmsHelper = new CMSHelper();
            int count = cmsHelper.ActDeActTemplate(Id, 1);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Template activated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Template activation failed.");
            }
            return RedirectToAction("Index");
        }


        #endregion
    }
}