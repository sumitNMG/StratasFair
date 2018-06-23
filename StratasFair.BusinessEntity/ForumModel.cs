using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.BusinessEntity
{
    public class ForumModel
    {
        [Key]
        public long TopicId { get; set; }

        [Required(ErrorMessage = "Topic required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 100.")]
        public string Topic { get; set; }


        [Required(ErrorMessage = "Commets required.")]
        [StringLength(4000, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 4000.")]
        public string TopicContent { get; set; }

        public string UploadedFileName { get; set; }
        public string OldUploadedFileName { get; set; }
        public string UploadedFileActualName { get; set; }

        [FileExtensions(ErrorMessage = "Must choose .jpg, .png, .pdf, .docx, .pptx, .xlsx file.", Extensions = "png, jpg, jpeg, pdf, docx, pptx, xlsx")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageType { get; set; }


        public long StratasBoardId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public bool IsFlagged { get; set; }
        public int Status { get; set; }

        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public string CreatedDateTime { get; set; }
        public string CreatedByName { get; set; }
        public int ReplyCounter { get; set; }

        public int FlaggedByLoggedInUser { get; set; }
    }


    public class ForumReplyModel
    {
        [Key]
        public long ReplyId { get; set; }
        public long TopicId { get; set; }

       

        [Required(ErrorMessage = "Message required.")]
        [StringLength(4000, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 4000.")]
        public string Message { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public bool IsFlagged { get; set; }
        public int Status { get; set; }

        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public string CreatedDateTime { get; set; }
        public string CreatedByName { get; set; }
      
    }


    public class ForumModelView
    {
        public List<ForumModel> ListForum { get; set; }
        public int TotalPages { get; set; }

        public string TopicId { get; set; }
        public string Message { get; set; }
    }


    public class ForumFlaggedModel
    {
        [Key]
        public long Id { get; set; }
        public long ReplyId { get; set; }
        public long TopicId { get; set; }
        public long AutoId { get; set; }

        [Required(ErrorMessage = "Comment required.")]
        [StringLength(4000, MinimumLength = 2, ErrorMessage = "Length must be between 2 to 4000.")]
        public string Comment { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }

        public string ForumMessageType { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public string CreatedDateTime { get; set; }
        public string CreatorName { get; set; }
        public string CreatorEmailId { get; set; }
    }


    public class ForumFlaggedModelView
    {
      
        public long ReplyId { get; set; }
        public long TopicId { get; set; }
        public long AutoIdView { get; set; }
     
        public string Comment { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }

        public string ForumMessageTypeView { get; set; }
    }
}
