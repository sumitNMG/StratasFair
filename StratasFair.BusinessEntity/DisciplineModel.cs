using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.BusinessEntity
{
    public class DisciplineModel:CommonModel
    {
        [Key]
        public int DisciplineId { get; set; }

        [Required(ErrorMessage = "Discipline required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 100.")]
        public string DisciplineName { get; set; }
        public long DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedFromIP { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }
    }
}
