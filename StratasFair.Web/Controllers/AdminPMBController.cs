using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StratasFair.Business.Front;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using System.IO;
using System.Net;
using StratasFair.Common;
using StratasFair.Business.CommonHelper;

namespace StratasFair.Web.Controllers.Admin
{
    [ClientSessionExpire]
    public class AdminPMBController : Controller
    {
        // GET: PMB
        public ActionResult Index(long? id)
        {
            try
            {
                if(!PMBHelper.Instance.IsOwnerInStrataBoard(id, ClientSessionData.ClientStrataBoardId))
                {
                    TempData["Message"] = AppLogic.setFrontendMessage(-1, "Error: Please try again!");
                    return Redirect(Url.Content("~/"+ClientSessionData.ClientPortalLink+"/dashboard"));
                }
                AdminPMBModel _model = new AdminPMBModel();
                if (ClientSessionData.ClientRoleName == "Admin" || ClientSessionData.ClientRoleName == "SubAdmin")
                    _model.CreatedFor = id;
                else
                    _model.CreatedFor = ClientSessionData.UserClientId;

                return View(_model);
            }
            catch
            {
                TempData["Message"] = AppLogic.setFrontendMessage(-1, "Error: Please try again!");
                return Redirect(Url.Content("~/dashboard"));
            }
        }

        //Save PMB details
        //++++++++++++++//
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(AdminPMBModel _model)
        {
            bool hasError = false;
            if (_model.Attachment != null)
            {
                List<string> fileTypes = new List<string> { ".jpg", ".png", ".jpeg", ".pdf", ".ppt", ".xls", ".xlsx", ".doc", ".docx", ".txt" };
                if (!fileTypes.Contains(Path.GetExtension(_model.Attachment.FileName)))
                {
                    ModelState.AddModelError("Message", "Invalid File only " + String.Join(",", fileTypes) + " are allowed");
                    hasError = true;
                }
                if (_model.Attachment.ContentLength > (5 * 1024 * 1024))
                {
                    ModelState.AddModelError("Message", "Max file size 5 MB");
                    hasError = true;
                }
                if (string.IsNullOrEmpty(_model.Message))
                {
                    ModelState.AddModelError("Message", "Message is required");
                    hasError = true;
                }
                if (hasError)
                    return View(_model);
            }
            if (PMBHelper.Instance.AddNewAdminPMB(_model) > 0)
            {
            return Redirect(Url.Content("~/" + ClientSessionData.ClientPortalLink + "/adminpmb?id=" + _model.CreatedFor));
            }
            return View(_model);
        }
        //Download PMB attachment
        //++++++++++++++++++++++//

        [AcceptVerbs(HttpVerbs.Get)]
        public FileResult Download(AdminPMBModel model)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var content = client.DownloadData(AwsS3Bucket.AccessPath() + "/resources/admin-owner-pmb/" + model.StratasBoardId + "/pmbFiles/" + model.AttachedFileName);
                    using (var stream = new MemoryStream(content))
                    {
                        byte[] buff = stream.ToArray();
                        return File(buff, MimeMapping.GetMimeMapping(model.AttachedFileName), model.AttachedFileActualName);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = AppLogic.setMessage(-1, "Error! " + ex.Message);
                }
            }
            return null;
        }


        [StrataBoardAdmin]
        public ActionResult OwnerList()
        {
            return View(new StrataOwnerModel());
        }

        [HttpPost]
        [StrataBoardAdmin]
        public ActionResult OwnerList(StrataOwnerModel model)
        {
            return View(model);
        }

        [HttpGet]
        [StrataBoardAdmin]
        public ActionResult GetOwnerList(StrataOwnerModel model)
        {
            StrataOwnerHelper objHelper = new StrataOwnerHelper();
            var Owners = objHelper.GetOwnerList(model);
            if (Owners == null)
                return Content("0");
            if (Owners.Count <= 0)
                return Content("0");
            else
                return PartialView("_OwnerList", Owners);
        }
    }
}