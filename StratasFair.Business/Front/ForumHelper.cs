using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Front
{
    public class ForumHelper
    {
        StratasFairDBEntities context;
        public ForumHelper()
        {
            if (context == null)
            {
                context = new StratasFairDBEntities();
            }
        }



        /// <summary>
        /// Add the new Topic message into the database
        /// </summary>
        /// <param name="objectModel"></param>
        /// <returns></returns>
        public string AddTopic(ForumModel objectModel)
        {

            ObjectParameter returnParameter = new ObjectParameter("Err", typeof(int));
            context.Usp_AddUpdForum(objectModel.Topic.Trim(), objectModel.TopicContent.Trim(), objectModel.UploadedFileName, objectModel.UploadedFileActualName, ClientSessionData.ClientStrataBoardId, ClientSessionData.UserClientId, DateTime.UtcNow, 1, ClientSessionData.UserClientId, HttpContext.Current.Request.UserHostAddress, false, returnParameter);
            if ((int)returnParameter.Value == 0)
            {
                return "success";
            }
            else
            {
                return "failed";
            }

        }


        /// <summary>
        /// Add the new topic message reply into the database.
        /// </summary>
        /// <param name="objectModel"></param>
        /// <returns></returns>
        public string AddTopicReply(ForumModelView objectModel)
        {

            ObjectParameter returnParameter = new ObjectParameter("Err", typeof(int));
            context.Usp_AddUpdForumReply(Convert.ToInt64(objectModel.TopicId), objectModel.Message.Trim(), ClientSessionData.ClientStrataBoardId, ClientSessionData.UserClientId, DateTime.UtcNow, 1, ClientSessionData.UserClientId, HttpContext.Current.Request.UserHostAddress, false, returnParameter);
            if ((int)returnParameter.Value == 0)
            {
                return "success";
            }
            else
            {
                return "failed";
            }

        }


        /// <summary>
        /// Get Forum list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ForumModel> GetForumListing(ForumModel model)
        {
            var bookingList = context.vw_GetForumList.Where(x => x.StratasBoardId == model.StratasBoardId && x.STATUS == 1).OrderByDescending(x => x.CreatedOn).ToList();
            List<ForumModel> objProjListingModel = new List<ForumModel>();
            foreach (var item in bookingList)
            {
                ForumModel objBookingList = new ForumModel
                {
                    TopicId = item.TopicId,
                    Topic = item.Topic,
                    TopicContent = item.TopicContent,
                    UploadedFileName = item.UploadedFileName,
                    UploadedFileActualName = item.UploadedFileActualName,
                    CreatedOn = item.CreatedOn.Value,
                    CreatedDate = item.CreatedDate,
                    CreatedTime = item.CreatedTime,
                    Status = item.STATUS.Value,
                    CreatedDateTime = item.CreatedDateTime,
                    CreatedBy = item.CreatedBy.Value,
                    StratasBoardId = item.StratasBoardId.Value,
                    IsFlagged = item.IsFlagged.Value,
                    CreatedByName = item.CreatedByName,
                    ReplyCounter = item.ReplyCounter.Value
                   // FlaggedByLoggedInUser = GetFlaggedUser(item.TopicId, ClientSessionData.UserClientId)
                };
                objProjListingModel.Add(objBookingList);
            }
            return objProjListingModel;

        }


        public List<ForumModel> GetAdminDashboardNewForumMessageListing(long stratasBoardId)
        {

            var bookingList = context.vw_GetForumList.Where(x => x.StratasBoardId == stratasBoardId && x.STATUS == 1).OrderByDescending(x => x.CreatedOn).ToList();
            List<ForumModel> objProjListingModel = new List<ForumModel>();
            foreach (var item in bookingList)
            {
                ForumModel objBookingList = new ForumModel
                {
                    TopicId = item.TopicId,
                    Topic = item.Topic,
                    TopicContent = item.TopicContent,
                    UploadedFileName = item.UploadedFileName,
                    UploadedFileActualName = item.UploadedFileActualName,
                    CreatedOn = item.CreatedOn.Value,
                    CreatedDate = item.CreatedDate,
                    CreatedTime = item.CreatedTime,
                    Status = item.STATUS.Value,
                    CreatedDateTime = item.CreatedDateTime,
                    CreatedBy = item.CreatedBy.Value,
                    StratasBoardId = item.StratasBoardId.Value,
                    IsFlagged = item.IsFlagged.Value,
                    CreatedByName = item.CreatedByName,
                    ReplyCounter = item.ReplyCounter.Value
                };
                objProjListingModel.Add(objBookingList);
            }
            return objProjListingModel;

        }


        public int GetFlaggedUser(long topicId, long userClientId )
        {
            if (context.tblFlagComments.Any(x => x.TopicId == topicId && x.CreatedBy == userClientId))
                return 1;
            else
                return 0;

        }


        /// <summary>
        /// Get the Forum reply listing as per Topic Id
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public List<ForumReplyModel> GetForumReplyListing(long topicId)
        {
            var forumReplyList = context.vw_GetForumListReply.Where(x => x.TopicId == topicId && x.STATUS == 1).ToList();
            List<ForumReplyModel> objForumReplyListingModel = new List<ForumReplyModel>();
            foreach (var item in forumReplyList)
            {
                ForumReplyModel replyList = new ForumReplyModel
                {
                    TopicId = item.TopicId,
                    ReplyId = item.ReplyId,
                    Message = item.Message,
                    CreatedOn = item.CreatedOn.Value,
                    CreatedDate = item.CreatedDate,
                    CreatedTime = item.CreatedTime,
                    Status = item.STATUS.Value,
                    CreatedDateTime = item.CreatedDateTime,
                    CreatedBy = item.CreatedBy.ToString(),
                    IsFlagged = item.IsFlagged.Value,
                    CreatedByName = item.CreatedByName
                };
                objForumReplyListingModel.Add(replyList);
            }
            return objForumReplyListingModel;
        }


        public int MarkFlagOnForum(ForumFlaggedModel objectModel)
        {
            int returnValue = -1;
            tblFlagComment tblFlagCommentDb = new tblFlagComment();
            if (objectModel.ForumMessageType == "topic")
                tblFlagCommentDb.TopicId = objectModel.AutoId;
            else
                tblFlagCommentDb.ReplyId = objectModel.AutoId;
            tblFlagCommentDb.Message = objectModel.Comment.Trim();
            tblFlagCommentDb.CreatedBy = ClientSessionData.UserClientId;
            tblFlagCommentDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
            tblFlagCommentDb.CreatedOn = DateTime.UtcNow;
            context.tblFlagComments.Add(tblFlagCommentDb);
          

            int count = context.SaveChanges();
            if (count == 1)
            {
                if (objectModel.ForumMessageType == "topic")
                {
                    tblForum tblForumDb = new tblForum();
                    tblForumDb = context.tblForums.Where(x => x.TopicId == objectModel.AutoId).FirstOrDefault();
                    tblForumDb.IsFlagged = true;
                    context.tblForums.Attach(tblForumDb);
                    context.Entry(tblForumDb).Property(x => x.IsFlagged).IsModified = true;
                    count = context.SaveChanges();
                    if (count == 1)
                        returnValue = 0;
                }
                else if (objectModel.ForumMessageType == "reply")
                {
                    tblForumReply tblForumReplyDb = new tblForumReply();
                    tblForumReplyDb = context.tblForumReplies.Where(x => x.ReplyId == objectModel.AutoId).FirstOrDefault();
                    tblForumReplyDb.IsFlagged = true;
                    context.tblForumReplies.Attach(tblForumReplyDb);
                    context.Entry(tblForumReplyDb).Property(x => x.IsFlagged).IsModified = true;
                    count = context.SaveChanges();
                    if (count == 1)
                        returnValue = 0;
                }
                else
                {
                    returnValue = -2;
                }
                
            }
            return returnValue;
        }


        public List<ForumFlaggedModel> GetFlaggedForumMessage(long id, string type)
        {
            var forumFlagMessageList = context.vw_GetFlaggedMessage.Where(x => x.AutoId == id && x.ForumMessageType == type).ToList();
            List<ForumFlaggedModel> objForumReplyListingModel = new List<ForumFlaggedModel>();
            foreach (var item in forumFlagMessageList)
            {
                ForumFlaggedModel replyList = new ForumFlaggedModel
                {
                    TopicId = item.TopicId.Value,
                    ReplyId = item.ReplyId.Value,
                    Comment = item.Message,
                    CreatedOn = item.CreatedOn.Value,
                    CreatedDate = item.CreatedDate,
                    CreatedTime = item.CreatedTime,
                    CreatedDateTime = item.CreatedDateTime,
                    CreatedBy = item.CreatedBy.ToString(),
                    CreatorName = item.CreatorName,
                    CreatorEmailId = item.CreatorEmailId
                };
                objForumReplyListingModel.Add(replyList);
            }
            return objForumReplyListingModel;
        }


        public int DeleteForum(ForumFlaggedModelView objectModel)
        {
            int returnValue = -1;
            int count = -1;
           
            if (objectModel.ForumMessageTypeView == "topic")
            {
                tblForum tblForumDb = new tblForum();
                tblForumDb = context.tblForums.Where(x => x.TopicId == objectModel.AutoIdView).FirstOrDefault();
                tblForumDb.Status = 2;
                context.tblForums.Attach(tblForumDb);
                context.Entry(tblForumDb).Property(x => x.Status).IsModified = true;
                count = context.SaveChanges();
                if (count == 1)
                    returnValue = 0;
            }
            else if (objectModel.ForumMessageTypeView == "reply")
            {
                tblForumReply tblForumReplyDb = new tblForumReply();
                tblForumReplyDb = context.tblForumReplies.Where(x => x.ReplyId == objectModel.AutoIdView).FirstOrDefault();
                tblForumReplyDb.Status = 2;
                context.tblForumReplies.Attach(tblForumReplyDb);
                context.Entry(tblForumReplyDb).Property(x => x.Status).IsModified = true;
                count = context.SaveChanges();
                if (count == 1)
                    returnValue = 0;
            }
            else
            {
                returnValue = -2;
            }

           
            return returnValue;
        }


        public int ReadMessage(long topicId, long userClientId, long stratasBoardId)
        {

            try
            {
                //var forumListRead = context.tblForumUsers.Join(context.tblForums, s => s.TopicId, g => g.TopicId, (s, g) => new { forumuser = s, forum = g })
                //    .Where(sg => sg.forumuser.TopicId == topicId && sg.forumuser.UserClientId == userClientId && sg.forum.Status == 1)
                //    .Select(sg => sg.forumuser).ToList();

                //var forumListRead =  context.tblForumUsers.Where(x => x.TopicId == topicId && x.UserClientId == userClientId).ToList();
                //forumListRead.ForEach(a => a.IsRead = 1);
                //int count = context.SaveChanges();

                //var forumListReplyRead = context.tblForumReplyUsers.Where(x => x.TopicId == topicId && x.UserClientId == userClientId).ToList();
                //forumListReplyRead.ForEach(a => a.IsRead = 1);
                //count = context.SaveChanges();

                context.Usp_UpdateReadMessage(userClientId, topicId);
                int unreadCount = CommonData.FetchDataCounters("new-forum-message", userClientId, stratasBoardId);
                return unreadCount;
            }
            catch { }

            return 0;
          
        }



    }

   
}
