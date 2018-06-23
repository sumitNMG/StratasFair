using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace StratasFair.BusinessEntity.Front
{
    public class PollQuestionModel
    {
        public PollQuestionModel()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.CreatedFromIP = System.Web.HttpContext.Current.Request.UserHostAddress;
        }
        public long PollId { get; set; }
        
        [Required(ErrorMessage = "Header is required")]
        [MinLength(5, ErrorMessage = "Min 5 characters")]
        [MaxLength(500, ErrorMessage = "Max 500 characters")]
        public string PollHeader { get; set; }

        [Required(ErrorMessage="End date is required")]
        public Nullable<DateTime> EndDate { get; set; }
        
        [Required(ErrorMessage = "Option is required")]
        [MinLength(5, ErrorMessage = "Min 2 characters")]
        [MaxLength(500, ErrorMessage = "Max 100 characters")]
        public string [] pollOption { get; set; }

        public long? StrataBoardId { get; set; }
        public bool IsAnswered { get; set; }
        public int TotalRecords { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedFromIP { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public Nullable<int> Status { get; set; }
        public List<PollOptionModel> pollOptionList { get; set; }
        public List<PollAnswerModel> pollAnswerList { get; set; }
    }
}
