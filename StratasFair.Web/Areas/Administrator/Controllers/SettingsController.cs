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
        public class SettingsController : Controller
        {
            List<SettingsModel> ActiveSetting = new List<SettingsModel>();
            List<SettingsModel> InActiveSetting = new List<SettingsModel>();
            // GET: Administrator/CMS        
            public ActionResult Index()
            {
                return BindAllSettings();
            }

            private ActionResult BindAllSettings()
            {
                SettingHelper settingHelper = new SettingHelper();
                SettingsModel model = new SettingsModel();
                model.Flag = 1;
                var List = settingHelper.GetaLL().ToList();
                ActiveSetting = List.Where(x => x.Status == 1).ToList();
                InActiveSetting = List.Where(x => x.Status == 0).ToList();
                return View(Tuple.Create(ActiveSetting, InActiveSetting, model));
            }

            public ActionResult Edit(int Id)
            {
                SettingHelper helper = new SettingHelper();
                SettingsModel result = helper.GetById(Id);
                ViewBag.Status = AppLogic.BindDDStatus(Convert.ToInt32(result.Status));
                return View(result);
            }

            [HttpPost]
            public ActionResult Edit(SettingsModel objSettings)
            {

                ModelState.Remove("SettingDescription");
                SettingHelper helper = new SettingHelper();
                objSettings.Flag = 1;
                objSettings.Status = 1;

                if (objSettings.DataType.ToLower() == "int")
                {
                    int num;
                    bool res = int.TryParse(objSettings.SettingValue, out num);
                    if (!res)
                    {
                        ModelState.AddModelError("SettingValue", "Invalid input");
                    }
                }

                if (ModelState.IsValid)
                {
                    int count = helper.Update(objSettings);
                    if (count == 0)
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(count, "Setting updated successfully.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(count, "Setting update failed.");
                    }
                }

                return View(objSettings);
            }

            public ActionResult InActivate(int Id)
            {
                SettingHelper helper = new SettingHelper();
                SettingsModel objSettings = helper.GetById(Id);
                objSettings.Flag = 1;
                objSettings.Status = 0;
                int count = helper.Update(objSettings);
                if (count == 0)
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Setting deactivated successfully.");
                else
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Setting deactivation failed.");
                return RedirectToAction("Index");
            }

            public ActionResult Activate(int Id)
            {
                SettingHelper helper = new SettingHelper();
                SettingsModel objSettings = helper.GetById(Id);
                objSettings.Flag = 1;
                objSettings.SettingId = Id;
                objSettings.Status = 1;
                int count = helper.Update(objSettings);
                if (count == 0)
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Setting activated successfully.");
                else
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Setting activation failed.");

                return RedirectToAction("Index");
            }
        }
    
}