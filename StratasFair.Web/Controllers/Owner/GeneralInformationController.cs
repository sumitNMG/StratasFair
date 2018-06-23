using StratasFair.Business.Front;
using StratasFair.BusinessEntity.Front;
using StratasFair.Common;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers.Owner
{
    [ClientSessionExpire]
    [StrataBoardOwner]
    public class GeneralInformationController : Controller
    {
        NoticeBoardHelper noticeBoardHelper = new NoticeBoardHelper();

        // GET: GeneralInformation
        public ActionResult Index()
        {
            int BlockSize = 4;
            List<GeneralInformationModel> GeneralInformationModelList = new List<GeneralInformationModel>();
            GeneralInformationModelList = noticeBoardHelper.GetAllGeneralInformation(1, BlockSize, string.Empty);
            return View(GeneralInformationModelList);
        }

        [ChildActionOnly]
        public ActionResult ListGeneralInformation(List<GeneralInformationModel> model)
        {
            return PartialView(model);
        }

        private string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult =
                ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext
                (ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        // POST: GeneralInformationInfinateScroll
        [HttpPost]
        public ActionResult GenralInformationInfinateScroll(int BlockNumber, string SearchKeyword)
        {
            int BlockSize = 4;
            var GeneralInformationModel = noticeBoardHelper.GetAllGeneralInformation(BlockNumber, BlockSize, SearchKeyword);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = GeneralInformationModel.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("ListGeneralInformation", GeneralInformationModel);
            return Json(jsonModel);
        }

        [HttpGet]
        public FileResult DownloadUploadedFile(int GeneralInformationId)
        {
            string FilePath = "resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/generalinformation/";
            GeneralInformationModel model = noticeBoardHelper.GetGeneralInformationDetail(GeneralInformationId);
            string path = AwsS3Bucket.DownloadObject2(model.UploadFile, FilePath);
            return File(Path.Combine(Server.MapPath(path)), MimeMapping.GetMimeMapping(model.UploadFile), System.IO.Path.GetFileName(path));
        }

    }
}