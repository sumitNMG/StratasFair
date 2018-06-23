﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StratasFair.BusinessEntity.Front
{
    public class StrataOwnerModel : CommonModel
    {
        public StrataOwnerModel()
        {
            this.PageNo = 1;
        }

        [HiddenInput(DisplayValue = false)]
        [Key]
        public long UserClientId { get; set; }

        [Required(ErrorMessage = "First Name required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First Name must be at least 2 character long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last Name must be at least 2 character long")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email address must be at least 5 character long")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Valid email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Unit Number required")]
        public string UnitNumber { get; set; }

        public string CreatedOn { get; set; }
        public int Status { get; set; }

        public int TotalNoofAccounts { get; set; }
        public int TotalAddedAccounts { get; set; }
        public int TotalAvailableAccounts { get; set; }

        public int PageNo { get;set; }

    }
}
