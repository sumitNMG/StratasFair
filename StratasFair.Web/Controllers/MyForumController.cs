using StratasFair.Business.Front;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using StratasFair.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Controllers
{
    public class MyForumController : Controller
    {
        // GET: Forum
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add the new message
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddTopic(ForumModel model)
        {
            int result = 0;
            string strMsg = "";
            try
            {
                if (String.IsNullOrEmpty(model.Topic) || model.Topic.Trim() == "")
                {
                    result = -3;
                    strMsg = "Topic required.";
                }
                else if (String.IsNullOrEmpty(model.TopicContent) || model.TopicContent.Trim() == "")
                {
                    result = -4;
                    strMsg = "Comment required.";
                }
                
                if (model.ImageType != null)
                {
                    string ext = Path.GetExtension(model.ImageType.FileName).ToLower();
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".pdf" && ext != ".png" && ext != ".xlsx" && ext != ".docx" && ext != ".pptx")
                    {
                        result = -1;
                        strMsg = "Please upload only .jpg, .png, .pdf, .pptx, .docx or .xlsx File";
                    }
                    else
                    {
                        Guid guid = Guid.NewGuid();
                        model.UploadedFileActualName = model.ImageType.FileName;
                        model.UploadedFileName = guid.ToString() + ext;


                        string path = string.Empty;
                        string initialPath = "~/content/resources/strataboard";
                        if (!Directory.Exists(Server.MapPath(initialPath)))
                            Directory.CreateDirectory(Server.MapPath(initialPath));

                        initialPath = initialPath + "/" + ClientSessionData.ClientStrataBoardId;
                        if (!Directory.Exists(Server.MapPath(initialPath)))
                            Directory.CreateDirectory(Server.MapPath(initialPath));

                        initialPath = initialPath + "/myforum";
                        if (!Directory.Exists(Server.MapPath(initialPath)))
                            Directory.CreateDirectory(Server.MapPath(initialPath));

                        // save the file locally
                        path = Server.MapPath(Path.Combine(initialPath + "/" + model.UploadedFileName));
                        model.ImageType.SaveAs(path);

                        // save the file on s3
                        int fileMapped = AwsS3Bucket.CreateFile("resources/strataboard/" + ClientSessionData.ClientStrataBoardId + "/myforum/" + model.UploadedFileName, path);
                        if (fileMapped != 0)
                        {
                            result = -2;
                            strMsg = "Error is uploading file in S3, Try again later.";
                        }
                        // delete the file locally
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }


                if (result == 0)
                {
                    ForumHelper helper = new ForumHelper();
                    string msg = helper.AddTopic(model);
                    if (msg != "success")
                    {
                        strMsg = "Error: Try again later";
                        result = -3;
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = "Exception: " + ex.Message;
                result = -5;
            }
            return Json(new { counter = result, msg = strMsg }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Load the Forum message listing
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PartialViewResult LoadForumList(string keyword, string sortBy, string pageNo, string pageSize = "10")
        {
            ForumHelper helper = new ForumHelper();
            ForumModel model = new ForumModel();
            model.StratasBoardId = ClientSessionData.ClientStrataBoardId;
            var forumList = helper.GetForumListing(model);


            if (!string.IsNullOrEmpty(keyword))
                forumList = forumList.Where(x => x.Topic.ToLower().Contains(keyword.ToLower()) || x.TopicContent.ToLower().Contains(keyword.ToLower())).ToList();

            if(!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "date asc")
                    forumList = forumList.OrderBy(x => x.CreatedOn).ToList();
                else if (sortBy == "date desc")
                    forumList = forumList.OrderByDescending(x => x.CreatedOn).ToList();
                else if (sortBy == "title asc")
                    forumList = forumList.OrderBy(x => x.Topic).ToList();
                else if (sortBy == "title desc")
                    forumList = forumList.OrderByDescending(x => x.Topic).ToList();
                else
                    forumList = forumList.OrderByDescending(x => x.CreatedOn).ToList();
            }


            int totalRecords = forumList.Count;

            ForumModelView forumViewModel = new ForumModelView();

            forumViewModel.TotalPages = totalRecords / Convert.ToInt32(pageSize);
            if (totalRecords % Convert.ToInt32(pageSize) > 0)
                forumViewModel.TotalPages++;

            var skip = Convert.ToInt32(pageSize) * (Convert.ToInt32(pageNo) - 1);
            forumViewModel.ListForum = forumList.Skip(skip).Take(Convert.ToInt32(pageSize)).ToList();

            return PartialView("_ForumListPartial", forumViewModel);


        }


        /// <summary>
        /// Download the uploaded file with message
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public FileResult Download(ForumModel model)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var content = client.DownloadData(AwsS3Bucket.AccessPath() + "/resources/strataboard/" + model.StratasBoardId + "/myforum/" + model.UploadedFileName);
                    using (var stream = new MemoryStream(content))
                    {
                        byte[] buff = stream.ToArray();
                        return File(buff, MimeMapping.GetMimeMapping(model.UploadedFileName), model.UploadedFileActualName);
                    }
                }
                catch {
                }
            }
            return null;
        }


        /// <summary>
        /// Add the Message Reply 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddTopicReply(ForumModelView model)
        {
            int result = -1;
            string strMsg = "";
            try
            {
                if (String.IsNullOrEmpty(model.Message) || model.Message.Trim() == "")
                {
                    result = -2;
                    strMsg = "Message required.";
                }
                else
                {
                    ForumHelper helper = new ForumHelper();
                    string msg = helper.AddTopicReply(model);
                    if (msg == "success")
                    {
                        strMsg = "You have replied successfully.";
                        result = 0;
                    }
                    else
                    {
                        strMsg = "Error! Try again later.";
                        result = -3;
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = "Exception: " + ex.Message;
            }
            return Json(new { counter = result, msg = strMsg, topicId = model.TopicId }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Load the forum message reply list
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public PartialViewResult LoadForumReplyList(string topicId)
        {
            ForumReplyModel model = new ForumReplyModel();
            model.TopicId = Convert.ToInt64(topicId);
            return PartialView("_ForumReplyListPartial", model);
        }


       
        [HttpPost]
        public JsonResult AddFlagOnTopic(ForumFlaggedModel model)
        {
            int result = -1;
            string strMsg = "";
            try
            {
                if (String.IsNullOrEmpty(model.ForumMessageType) || model.ForumMessageType.Trim() == "")
                {
                    result = -4;
                    strMsg = "Error! Try again later.";
                }
                else if (model.AutoId <= 0)
                {
                    result = -5;
                    strMsg = "Error! Try again later.";
                }
                else if (String.IsNullOrEmpty(model.Comment) || model.Comment.Trim() == "")
                {
                    result = -2;
                    strMsg = "Comment required.";
                }
                else
                {
                    ForumHelper helper = new ForumHelper();
                    int returnValue = helper.MarkFlagOnForum(model);
                    if (returnValue == 0)
                    {
                        strMsg = "You have flagged it successfully.";
                        result = 0;
                    }
                    else
                    {
                        strMsg = "Error! Try again later.";
                        result = -3;
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = "Exception: " + ex.Message;
            }
            return Json(new { counter = result, msg = strMsg, autoid = model.AutoId, type = model.ForumMessageType, userRole= ClientSessionData.ClientRoleName.ToLower() }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult LoadFlaggedMessage(string autoId, string forumMessageType)
        {
            ForumFlaggedModelView model = new ForumFlaggedModelView();
            model.AutoIdView = Convert.ToInt64(autoId);
            model.ForumMessageTypeView = forumMessageType;
            return PartialView("_ViewFlagForumMessage", model);
        }

        [HttpPost]
        public JsonResult DeleteFlagOnTopic(ForumFlaggedModelView model)
        {
            int result = -1;
            string strMsg = "";
            try
            {

                if (String.IsNullOrEmpty(model.ForumMessageTypeView) || model.ForumMessageTypeView.Trim() == "")
                {
                    result = -4;
                    strMsg = "Error! Try again later.";
                }
                else if (model.AutoIdView <= 0)
                {
                    result = -5;
                    strMsg = "Error! Try again later.";
                }
                else
                {
                    ForumHelper helper = new ForumHelper();
                    int returnValue = helper.DeleteForum(model);
                    if (returnValue == 0)
                    {
                        strMsg = "You have deleted it successfully.";
                        result = 0;
                    }
                    else
                    {
                        strMsg = "Error! Try again later.";
                        result = -3;
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = "Exception: " + ex.Message;
            }
            return Json(new { counter = result, msg = strMsg, autoid = model.AutoIdView, type = model.ForumMessageTypeView }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ReadMessage(long topicId)
        {
            int result = 0;
            try
            {
                if (topicId > 0)
                {
                    ForumHelper helper = new ForumHelper();
                    int remainingMessageCount = helper.ReadMessage(topicId, ClientSessionData.UserClientId, ClientSessionData.ClientStrataBoardId);
                    if (remainingMessageCount > 0)
                    {
                        result = remainingMessageCount;
                    }
                }
            }
            catch {
            }
            return Json(new { counter = result }, JsonRequestBehavior.AllowGet);
        }

    }
}