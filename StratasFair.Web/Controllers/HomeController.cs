using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StratasFair.BusinessEntity.Front;
using StratasFair.Business.Front;

namespace StratasFair.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public int ContactUs(ContactUsModel model)
        {
            try
            {
                if (HomePageHelper.Instance.SaveMessage(model) > 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }
    }
}