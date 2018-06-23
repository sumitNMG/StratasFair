using StratasFair.BusinessEntity.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StratasFair.Web.Models
{
    public class SubAdminWebModel
    {
        public List<SubAdminModel> subAdminModelList { get; set; }
        public List<UserClientPrivilege> UserClientPrivilege { get; set; }
    }
}