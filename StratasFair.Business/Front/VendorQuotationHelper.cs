using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StratasFair.Data;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity;
using StratasFair.Common;
using System.Data;
using System.Web.Mvc;

namespace StratasFair.Business.Front
{
    public class VendorQuotationHelper
    {
        private VendorQuotationHelper() { }
        private static readonly Lazy<VendorQuotationHelper> lazy = new Lazy<VendorQuotationHelper>(() => new VendorQuotationHelper());
        public static VendorQuotationHelper Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public List<VendorQuotationModel> GetAllQuotations()
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    return context.Usp_GetQuotationRequest(string.IsNullOrEmpty(ClientSessionData.ClientUserName) ? null : ClientSessionData.ClientUserName).Select(m => new VendorQuotationModel()
                      {
                          VendorId = m.VendorId,
                          RequesterEmailId = m.RequesterEmailId,
                          RequesterName = m.RequesterName,
                          VendorName = m.VendorName,
                          DisciplineId = m.DisciplineId,
                          DisciplineName = m.DisciplineName,
                          Details = m.Details,
                          IsShowEmail = m.IsShowEmail,
                          CreatedBy = m.CreatedBy,
                          CreatedOn = m.CreatedOn,
                          CreatedFromIP = m.CreatedFromIP,
                          StrataBoardId = m.StratasBoardId,
                          StrataBoardName = m.StratasBoardName,
                          QuotationId = m.QuotationId
                      }).OrderByDescending(m => m.CreatedOn).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddNewQuotation(VendorQuotationModel _model)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    string [] vendorIds=_model.VendorList.Split(',');
                    List<tblQuotationRequest> _QuotationList = new List<tblQuotationRequest>();
                    foreach (var id in vendorIds)
                    {
                        tblQuotationRequest _Quotation = new tblQuotationRequest();
                        _Quotation.VendorId = Convert.ToInt64(id);
                        _Quotation.RequesterEmailId = _model.RequesterEmailId;
                        _Quotation.RequesterName = _model.RequesterName;
                        _Quotation.DisciplineId = _model.DisciplineId;
                        _Quotation.Details = _model.Details;
                        _Quotation.IsShowEmail = _model.IsShowEmail;
                        _Quotation.CreatedBy = ClientSessionData.UserClientId;
                        _Quotation.CreatedOn = DateTime.UtcNow;
                        _Quotation.CreatedFromIP = System.Web.HttpContext.Current.Request.UserHostAddress;
                        _QuotationList.Add(_Quotation);
                    }
                    context.tblQuotationRequests.AddRange(_QuotationList);
                    if (context.SaveChanges() > 0)
                    {
                        var vendors=context.USP_GetVendorsById(_model.VendorList).ToList();
                        foreach(var v in vendors)
                        {
                            EmailSender.FncSend_QuotRequestMail_To_Vendor(v.VendorName, v.EmailId, _model.RequesterName, _model.Details);
                        }
                    }
                }
                return 1;
            }
            catch (Exception)
            {
               return -1;
            }
        }
    }
}
