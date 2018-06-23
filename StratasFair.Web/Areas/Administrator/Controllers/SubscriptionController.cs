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
    public class SubscriptionController : Controller
    {
        // GET: Administrator/Subscription
        List<SubscriptionModel> ActiveList = new List<SubscriptionModel>();
        List<SubscriptionModel> InActiveList = new List<SubscriptionModel>();
        public ActionResult Index()
        {
            return BindSubscription();
        }
        private ActionResult BindSubscription()
        {
            SubscriptionHelper helper = new SubscriptionHelper();
            SubscriptionModel subscriptionModel = new SubscriptionModel();
            var List = helper.GetAll().ToList();
            ActiveList = List.Where(x => x.Status == 1).ToList();
            InActiveList = List.Where(x => x.Status == 0).ToList();
            return View(Tuple.Create(ActiveList, InActiveList, subscriptionModel));
        }

        public ActionResult Add()
        {
            ViewBag.StatusList = AppLogic.BindDDStatus(1);
            ViewBag.SubscriptionValidityList = AppLogic.BindSubscriptionValidity();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(SubscriptionModel subscriptionModel)
        {
            SubscriptionHelper helper = new SubscriptionHelper();
            int count = helper.AddUpdate(subscriptionModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record added successfully.");
            }
            else if (count == 1)
            {
                ViewBag.StatusList = AppLogic.BindDDStatus(1);
                ViewBag.SubscriptionValidityList = AppLogic.BindSubscriptionValidity();
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record already exist. Name and validity must be different.");
            }
            else
            {
                ViewBag.StatusList = AppLogic.BindDDStatus(1);
                ViewBag.SubscriptionValidityList = AppLogic.BindSubscriptionValidity();
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Try again later.");
            }
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int Id)
        {
            SubscriptionHelper helper = new SubscriptionHelper();
            var result = helper.GetByID(Id);
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(result.Status));
            ViewBag.SubscriptionValidityList = AppLogic.BindSubscriptionValidity();
            return View(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SubscriptionModel subscriptionModel)
        {
            SubscriptionHelper helper = new SubscriptionHelper();
            int count = helper.AddUpdate(subscriptionModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record updated successfully.");
            }
            else if (count == 1)
            {
                ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(subscriptionModel.Status));
                ViewBag.SubscriptionValidityList = AppLogic.BindSubscriptionValidity();
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record already exist. Name and validity must be different.");
            }
            else
            {
                ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(subscriptionModel.Status));
                ViewBag.SubscriptionValidityList = AppLogic.BindSubscriptionValidity();
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record updation failed.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult InActivate(int Id)
        {
            SubscriptionHelper helper = new SubscriptionHelper();
            SubscriptionModel subscriptionModel = new SubscriptionModel();
            subscriptionModel = helper.GetByID(Id);
            subscriptionModel.Status = 0;
            int count = helper.ActDeact(subscriptionModel);
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

        public ActionResult Activate(int Id)
        {
            SubscriptionHelper helper = new SubscriptionHelper();
            SubscriptionModel subscriptionModel = new SubscriptionModel();
            subscriptionModel = helper.GetByID(Id);
            subscriptionModel.Status = 1;
            int count = helper.ActDeact(subscriptionModel);
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

        public ActionResult Delete(int Id)
        {
            SubscriptionHelper helper = new SubscriptionHelper();
            SubscriptionModel subscriptionModel = new SubscriptionModel();
            subscriptionModel = helper.GetByID(Id);
            int count = helper.Delete(subscriptionModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deleted successfully.");
            }
            else if (count == -2)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Subscription is associated with strataboard. it can't be deleted?");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deletion failed.");
            }
            return RedirectToAction("Index");
        }
    }
}