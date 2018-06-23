using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using StratasFair.Web.App_Start;
using StratasFair.Business.CommonHelper;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;


namespace StratasFair.Web.Controllers
{
    [ClientSessionExpire]
    public class PollsController : Controller
    {
        // GET: Index
        [StrataBoardAdmin]
        [HttpGet]
        public ActionResult Index()
        {
            PollQuestionModel model = new PollQuestionModel() { pollOption = new string[8] };
            return View();
        }

        public ActionResult GetPollList(int pageNo)
        {
            var polls = PollHelper.Instance.GetPollQuestions(pageNo);
            if (polls == null)
                return Content("0");
            else if (polls.Count <= 0)
                return Content("0");
            else
                return PartialView("_AdminPollListPartial", polls);
        }

        [StrataBoardAdmin]
        [HttpPost]
        public long Index(PollQuestionModel model)
        {
            try
            {
                return PollHelper.Instance.AddNewPoll(model);
            }
            catch
            {
                return -1;
            }
        }
        [StrataBoardAdmin]
        public PartialViewResult AddNewOptionField(int optionid)
        {
            return PartialView("~/Views/Shared/_newOptionField.cshtml", optionid);
        }

        [StrataBoardAdmin]
        [HttpPost]
        public JsonResult Delete(long pollid)
        {
            try
            {
                PollQuestionModel _model = new PollQuestionModel()
                {
                    PollId = pollid,
                    Status = 2
                };
                if (PollHelper.Instance.UpdatePoll(_model) > 0)
                    return Json(0);
                else
                    return Json(-1);
            }
            catch
            {
                return Json(-1);
            }
        }


        [HttpGet]
        public PartialViewResult showchart(long pollid, string caption)
        {
            return PartialView("_pollChart", PieChartData(caption, PollHelper.Instance.GetPollChart(pollid)));
        }


        public static string PieChartData(string heading, List<PollChartModel> Data)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (!Data.Any(x => x.PollCount > 0))
            {
                return "0";
            }

            sb.AppendFormat("<chart ");
            sb.AppendFormat("width='100%' ");
            sb.AppendFormat("height='100%' ");
            sb.AppendFormat("caption='" + heading + "' ");
            sb.AppendFormat("subcaption='' ");
            sb.AppendFormat("palettecolors='#33fff9,#fc8776,#fcf7b0,#c1f10b,#8290fa,#f527b3,#fb4b5b,#d2e2e7' ");
            sb.AppendFormat("bgcolor='#FFFDF6' ");
            sb.AppendFormat("labelFontColor='#333333' ");
            sb.AppendFormat("use3dlighting='0' ");
            sb.AppendFormat("showshadow='1' ");
            sb.AppendFormat("showborder='1' ");
            sb.AppendFormat("borderColor='#999999' ");
            sb.AppendFormat("enablesmartlabels='1' ");
            sb.AppendFormat("startingangle='180' ");
            sb.AppendFormat("showpercentvalues='1' ");
            sb.AppendFormat("showpercentintooltip='0' ");
            sb.AppendFormat("decimals='0' ");
            sb.AppendFormat("captionfontsize='14' ");
            sb.AppendFormat("subcaptionfontsize='14' ");
            sb.AppendFormat("subcaptionfontbold='0' ");
            sb.AppendFormat("tooltipcolor='#ffffff' ");
            sb.AppendFormat("tooltipborderthickness='0' ");
            sb.AppendFormat("tooltipbgcolor='#000000' ");
            sb.AppendFormat("tooltipbgalpha='80' ");
            sb.AppendFormat("tooltipborderradius='2' ");
            sb.AppendFormat("tooltippadding='5' ");
            sb.AppendFormat("showhovereffect='1' ");
            sb.AppendFormat("formatNumberScale='0' ");
            sb.AppendFormat("showlegend='1' ");
            sb.AppendFormat("legendbgcolor='transparent' ");
            sb.AppendFormat("legendborderalpha='0' ");
            sb.AppendFormat("legendshadow='0' ");
            sb.AppendFormat("legenditemfontsize='12' ");
            sb.AppendFormat("legenditemfontcolor='#000000' ");
            sb.AppendFormat("showLabels='1' ");
            sb.AppendFormat("usedataplotcolorforlabels='1'>");

            foreach (var item in Data)
            {
                sb.AppendFormat("<set label='" + item.PollOptionText + "' value='" + item.PollCount + "' />");
            }

            sb.AppendFormat("</chart>");
            return sb.ToString();
        }
    }
}