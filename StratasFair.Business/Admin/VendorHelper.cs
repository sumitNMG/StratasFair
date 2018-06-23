using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Admin
{
    public class VendorHelper
    {
        StratasFairDBEntities context;
        public VendorHelper()
        {
            if (context == null)
            {
                context = new StratasFairDBEntities();
            }
        }

        public List<VendorSearchModel> GetAll(VendorSearchModel model)
        {
            return context.vw_GetVendor
                .Where(x => x.DisciplineId == (model.DisciplineId == null ? x.DisciplineId : model.DisciplineId)
                && x.AdminApproval == (model.AdminApproval == null ? x.AdminApproval : model.AdminApproval))
                .Select(x => new VendorSearchModel()
                {
                    VendorId = x.VendorId,
                    VendorName = x.VendorName,
                    EmailId = x.EmailId,
                    Password = x.Password,
                    DisciplineId = x.DisciplineId,
                    DisciplineName = x.DisciplineName,
                    OtherDisciplineName = x.OtherDisciplineName,
                    MobileNumber = x.MobileNumber,
                    CompanyBrief = x.CompanyBrief,
                    ActualTradeAndBusinessFile = x.ActualTradeAndBusinessFile,
                    TradeAndBusinessFile = x.TradeAndBusinessFile,
                    ImageFile = x.ImageFile,
                    ActualImageFile = x.ActualImageFile,
                    Remark = x.Remark,
                    AdminApproval = x.AdminApproval,
                    AdminApprovalText = x.AdminApprovalText,
                    Status = x.Status,
                    CreatedOn = x.CreatedOn,
                }).ToList();
        }

        public VendorModel GetByID(long Id)
        {
            return context.vw_GetVendor.Where(x => x.VendorId == Id).Select(x => new VendorModel()
            {
                VendorId = x.VendorId,
                VendorName = x.VendorName,
                EmailId = x.EmailId,
                Password=x.Password,
                DisciplineId = x.DisciplineId.Value,
                DisciplineName = x.DisciplineName,
                OtherDisciplineName = x.OtherDisciplineName,
                MobileNumber = x.MobileNumber,
                CompanyBrief = x.CompanyBrief,
                
                ActualTradeAndBusinessFile = x.ActualTradeAndBusinessFile,
                TradeAndBusinessFile = x.TradeAndBusinessFile,
                OldTradeAndBusinessFile = x.TradeAndBusinessFile,
                
                ImageFile = x.ImageFile,
                OldImageFile = x.ImageFile,
                ActualImageFile=x.ActualImageFile,

                Remark = x.Remark,
                AdminApproval = x.AdminApproval.Value,
                AdminApprovalPrev = x.AdminApproval.Value,
                Status = x.Status,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedBy.Value,
             
            }).FirstOrDefault();
        }


        public long AddUpdate(VendorModel objectModel)
        {
            if (objectModel.VendorId > 0)
            {
                if (context.tblVendors.Any(x => x.EmailId == objectModel.EmailId && x.VendorId != objectModel.VendorId))
                {
                    // record already exists
                    return -1;
                }
                else
                {
                    return update(objectModel);
                }
            }
            else
            {
                if (context.tblVendors.Any(x => x.EmailId == objectModel.EmailId))
                {
                    // record already exists
                    return -1;
                }
                else
                {
                    return update(objectModel);
                }
            }
        }


        public long update(VendorModel objectModel)
        {
            long count = -2;

            tblVendor tblVendorDb = new tblVendor();
            tblVendorDb.VendorName = objectModel.VendorName;
            tblVendorDb.EmailId = objectModel.EmailId;
            tblVendorDb.DisciplineId = objectModel.DisciplineId;
            tblVendorDb.OtherDisciplineName = objectModel.OtherDisciplineName;
            tblVendorDb.MobileNumber = objectModel.MobileNumber;
            tblVendorDb.CompanyBrief = objectModel.CompanyBrief;
            tblVendorDb.TradeAndBusinessFile = objectModel.TradeAndBusinessFile;
            tblVendorDb.ActualTradeAndBusinessFile = objectModel.ActualTradeAndBusinessFile;
            tblVendorDb.ImageFile = objectModel.ImageFile;
            tblVendorDb.ActualImageFile = objectModel.ActualImageFile;
            tblVendorDb.AdminApproval = objectModel.AdminApproval;
            tblVendorDb.Remark = objectModel.AdminApproval == 2 ? objectModel.Remark : "";

            if (objectModel.VendorId > 0)
            {
                // Table Vendor
              
                tblVendorDb.VendorId = objectModel.VendorId;
                tblVendorDb.ModifiedBy = AdminSessionData.AdminUserId;
                tblVendorDb.ModifiedOn = DateTime.UtcNow;
                tblVendorDb.Status = (byte)objectModel.Status;

                context.tblVendors.Attach(tblVendorDb);
                context.Entry(tblVendorDb).Property(x => x.VendorName).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.DisciplineId).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.MobileNumber).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.CompanyBrief).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.TradeAndBusinessFile).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.ActualTradeAndBusinessFile).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.ImageFile).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.ActualImageFile).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.AdminApproval).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.Remark).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.ModifiedBy).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.ModifiedOn).IsModified = true;
                context.Entry(tblVendorDb).Property(x => x.Status).IsModified = true;
                count = context.SaveChanges();
                if (count >= 0)
                    count = objectModel.VendorId;
                else
                    count = -2;
            }
            else
            {
               
                try
                {
                    tblVendorDb.CreatedBy = AdminSessionData.AdminUserId;
                    tblVendorDb.CreatedOn = DateTime.UtcNow;
                    tblVendorDb.CreatedFromIp = HttpContext.Current.Request.UserHostAddress;
                    context.tblVendors.Add(tblVendorDb);
                    context.SaveChanges();
                    long _vendorId = tblVendorDb.VendorId;
                    count = context.SaveChanges();
                    if (count >= 0)
                        count = _vendorId;
                    else
                        count = -2;   // any error is there
                }
                catch (Exception ex)
                {
                    new AppError().LogMe(ex);
                    count = -2;   // any error is there
                }
            }
            return count;
        }


        public int ActDeact(VendorModel objectModel)
        {
            tblVendor tblVendorDb = new tblVendor();
            tblVendorDb.VendorId = objectModel.VendorId;
            tblVendorDb.Status = objectModel.Status;

            context.tblVendors.Attach(tblVendorDb);
            context.Entry(tblVendorDb).Property(x => x.Status).IsModified = true;

            int count = context.SaveChanges();
            if (count == 1)
                count = 0;
            else
                count = -1;

            return count;
        }

        public int Delete(VendorModel objectModel)
        {
            int count = -1;

            // check the vendor id if exists in another table 
            //if (context.tblDocumentTemplateImages.Any(o => o.TemplateId == objectModel.TemplateId))
            //{
            //    count = -2;
            //    return count;
            //}

            tblVendor tblVendorDb = new tblVendor();
            tblVendorDb.VendorId = objectModel.VendorId;
            context.Entry(tblVendorDb).State = EntityState.Deleted;
            count = context.SaveChanges();
            if (count == 1)
                count = 0;
            else
                count = -1;
            return count;
        }
    }
}
