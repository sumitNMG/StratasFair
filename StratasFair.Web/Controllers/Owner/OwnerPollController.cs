using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Web.App_Start;
namespace StratasFair.Web.Controllers.Owner
{
    [ClientSessionExpire]
    [StrataBoardOwner]
    public class OwnerPollController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPollList(int pageNo)
        {
            var polls = PollHelper.Instance.GetPollQuestionsForOwner(pageNo);
            if(polls == null)
                return Content("0");
            else if (polls.Count <=0)
                return Content("0");
            else 
                return PartialView("_OwnerPollListPartial", Tuple.Create(polls, pageNo));
            
        }

        [HttpPost]
        public JsonResult SubmitOpinion(long optionId, long pollId)
        {
            try
            {
                return Json(PollHelper.Instance.SubmitOpinion(optionId, pollId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}