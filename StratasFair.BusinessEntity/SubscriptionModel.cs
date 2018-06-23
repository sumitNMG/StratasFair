using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity
{
    public class SubscriptionModel: CommonModel
    {
        [Key]
        public int SubscriptionId { get; set; }

        [Required(ErrorMessage = "Subscription required.")]
        [StringLength(100, ErrorMessage = "Only 100 Characters Allowed")]
        public string SubscriptionName { get; set; }

        [Required(ErrorMessage = "Validity required.")]
        public string Validity { get; set; }

        [Required(ErrorMessage = "Allowed user required.")]
        [Range(1, 9999, ErrorMessage = "Enter number between 1 to 9999.")]
        public int AllowedUser { get; set; }

        public bool IsSmsForAlert { get; set; }
        public bool IsSmsForReminder { get; set; }

        public string SubscriptionWithValidity { get; set; }
        public string ValidityPeriod { get; set; }
        public string IsSmsForAlertText { get; set; }
        public string IsSmsForReminderText { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }

        public long StratasBoardId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ValidityText { get; set; }
    }

    public class RenewSubscriptionModel 
    {
        [Required(ErrorMessage = "Subscription required.")]
        public int SubscriptionId { get; set; }
        public long StratasBoardId { get; set; }
    }
}
