using StratasFair.Business.Admin;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business
{
    public class AdvertisementHelper
    {
        StratasFairDBEntities context;
        public AdvertisementHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }

        public List<ManageAdvertisementModel> GetAll()
        {
            return context.tblAdvertisements.Select(x => new ManageAdvertisementModel() { AdvertisementId = x.AdvertisementId, RedirectionUrl = x.RedirectionUrl, OldImageFile = x.ImageFile, ImageFile = x.ImageFile, ActualImageFile = x.ActualImageFile, CreatedOn = (DateTime)x.CreatedOn, Status = x.Status.Value, DisplayOrder = x.DisplayOrder.Value }).ToList();
        }

        public ManageAdvertisementModel GetByID(int Id)
        {
            return context.tblAdvertisements.Where(x => x.AdvertisementId == Id).Select(x => new ManageAdvertisementModel() { AdvertisementId = x.AdvertisementId, RedirectionUrl = x.RedirectionUrl, OldImageFile = x.ImageFile, ImageFile = x.ImageFile,ActualImageFile = x.ActualImageFile, CreatedOn = (DateTime)x.CreatedOn, Status = x.Status.Value, DisplayOrder = x.DisplayOrder.Value }).FirstOrDefault();
        }

        public int AddUpdate(ManageAdvertisementModel objectModel)
        {

            tblAdvertisement tblAdvertisementDb = new tblAdvertisement();
            tblAdvertisementDb.AdvertisementId = objectModel.AdvertisementId;
            tblAdvertisementDb.RedirectionUrl = objectModel.RedirectionUrl;
            tblAdvertisementDb.DisplayOrder = objectModel.DisplayOrder;
            tblAdvertisementDb.Status = objectModel.Status;
            tblAdvertisementDb.ImageFile = objectModel.ImageFile;
            tblAdvertisementDb.ActualImageFile = objectModel.ActualImageFile;

            if (objectModel.AdvertisementId > 0)
            {

                tblAdvertisementDb.ModifiedBy = AdminSessionData.AdminUserId;
                tblAdvertisementDb.ModifiedOn = DateTime.Now;
                tblAdvertisementDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;

                context.tblAdvertisements.Attach(tblAdvertisementDb);
                context.Entry(tblAdvertisementDb).Property(x => x.RedirectionUrl).IsModified = true;
                context.Entry(tblAdvertisementDb).Property(x => x.DisplayOrder).IsModified = true;
                context.Entry(tblAdvertisementDb).Property(x => x.ImageFile).IsModified = true;
                context.Entry(tblAdvertisementDb).Property(x => x.ActualImageFile).IsModified = true;
                context.Entry(tblAdvertisementDb).Property(x => x.Status).IsModified = true;
                context.Entry(tblAdvertisementDb).Property(x => x.ModifiedBy).IsModified = true;
                context.Entry(tblAdvertisementDb).Property(x => x.ModifiedOn).IsModified = true;
                context.Entry(tblAdvertisementDb).Property(x => x.ModifiedFromIP).IsModified = true;
            }
            else
            {
                tblAdvertisementDb.CreatedBy = AdminSessionData.AdminUserId;
                tblAdvertisementDb.CreatedOn = DateTime.Now;
                tblAdvertisementDb.CreatedFromIp = HttpContext.Current.Request.UserHostAddress;
                context.tblAdvertisements.Add(tblAdvertisementDb);
            }

            int count = context.SaveChanges();
            if (count == 1)
                count = 0;
            else
                count = -1;

            return count;
        }

        public int ActDeact(ManageAdvertisementModel objectModel)
        {
            tblAdvertisement tblAdvertisementDb = new tblAdvertisement();
            tblAdvertisementDb.AdvertisementId = objectModel.AdvertisementId;
            tblAdvertisementDb.Status = objectModel.Status;

            context.tblAdvertisements.Attach(tblAdvertisementDb);
            context.Entry(tblAdvertisementDb).Property(x => x.Status).IsModified = true;

            int count = context.SaveChanges();
            if (count == 1)
                count = 0;
            else
                count = -1;

            return count;
        }
    }
}
