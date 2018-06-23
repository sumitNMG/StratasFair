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
    public class FaqController : Controller
    {
        List<FAQModel> ActiveFAQList = new List<FAQModel>();
        List<FAQModel> InActiveFAQList = new List<FAQModel>();
        // GET: Administrator/CMS        
        public ActionResult Index()
        {
            return BindFAQList();
        }

        private ActionResult BindFAQList()
        {
            FAQHelper _faqHelper = new FAQHelper();
            FAQModel objFAQ = new FAQModel();
            var FAQList = _faqHelper.GetAll().ToList();
            ActiveFAQList = FAQList.Where(x => x.Status == 1).ToList();
            InActiveFAQList = FAQList.Where(x => x.Status == 0).ToList();
            return View(Tuple.Create(ActiveFAQList, InActiveFAQList, objFAQ));
        }

        public ActionResult Add()
        {
            ViewBag.StatusList = AppLogic.BindDDStatus(1);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(FAQModel objFAQ)
        {


            FAQHelper faqHelper = new FAQHelper();
            int count = faqHelper.Add(objFAQ);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record added successfully.");
                return RedirectToAction("Index");
            }
            else if (count == 1)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record already exists.");
                return RedirectToAction("Add");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Error, Please try again.");
                return RedirectToAction("Add");
            }
        }

        public ActionResult Edit(int Id)
        {
            FAQHelper faqHelper = new FAQHelper();
            var result = faqHelper.GetByID(Id);
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(result.Status));
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(FAQModel objFAQ)
        {
            FAQHelper _faqHelper = new FAQHelper();
            objFAQ.Flag = 2;
            int count = _faqHelper.Upate(objFAQ);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record Updated Successfully.");
                return RedirectToAction("Index");
            }
            else if (count == 1)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record already exists.");
                return RedirectToAction("Index");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Error, Please try again.");
                return View(objFAQ);
            }
        }

        public ActionResult InActivate(int Id)
        {
            FAQModel objFAQ = new FAQModel();
            FAQHelper faqHelper = new FAQHelper();
            objFAQ.Flag = 2;
            objFAQ.FAQId = Id;
            objFAQ.Status = 0;
            int count = faqHelper.ActDeactFAQ(objFAQ);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record Deactivated Successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Error, Please try again.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Activate(int Id)
        {
            FAQModel objFAQ = new FAQModel();
            FAQHelper cmsHelper = new FAQHelper();
            objFAQ.Flag = 2;
            objFAQ.FAQId = Id;
            objFAQ.Status = 1;
            int count = cmsHelper.ActDeactFAQ(objFAQ);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record Activated Successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Error, Please try again.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            FAQModel model = new FAQModel();
            FAQHelper faqHelper = new FAQHelper();
            model.Flag = 3;
            model.FAQId = Id;
            int count = faqHelper.Delete(model);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deleted successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Error, Please try again.");
            }
            return RedirectToAction("Index");
        }


      
    }
}