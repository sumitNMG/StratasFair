using StratasFair.BusinessEntity;
using StratasFair.Business.Admin;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Business
{
    public class TestimonialHelper
    {
         StratasFairDBEntities context;
         public TestimonialHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }

         public List<TestimonialModel> GetAll()
         {
             return context.tblTestimonials.Select(x => new TestimonialModel() { TestimonialId = x.TestimonialId, AuthorName = x.AuthorName, Designation=x.Designation, Description=x.Description, OldImageFile = x.ImageFile, ImageFile = x.ImageFile, CreatedOn = (DateTime)x.CreatedOn, Status = x.Status.Value, DisplayOrder=x.DisplayOrder.Value }).ToList();
         }

         public TestimonialModel GetByID(int Id)
         {
             return context.tblTestimonials.Where(x => x.TestimonialId == Id).Select(x => new TestimonialModel() { TestimonialId = x.TestimonialId, AuthorName = x.AuthorName, Designation = x.Designation, Description=x.Description, OldImageFile = x.ImageFile, ImageFile = x.ImageFile, CreatedOn = (DateTime)x.CreatedOn, Status = x.Status.Value, DisplayOrder = x.DisplayOrder.Value }).FirstOrDefault();
         }

      

         public int AddUpdate(TestimonialModel objectModel)
         {
             if (objectModel.TestimonialId > 0)
             {
                 if (context.tblTestimonials.Any(x => x.AuthorName == objectModel.AuthorName && x.Designation == objectModel.Designation && x.TestimonialId != objectModel.TestimonialId))
                 {
                     return 1;
                 }
                 else
                 {
                     return update(objectModel);
                 }
             }
             else
             {
                 if (context.tblTestimonials.Any(x => x.AuthorName == objectModel.AuthorName && x.Designation == objectModel.Designation))
                 {
                     return 1;
                 }
                 else
                 {
                     return update(objectModel);
                 }
             }
         }

         public int update(TestimonialModel objectModel)
         {

             tblTestimonial tblTestimonialDb = new tblTestimonial();
             tblTestimonialDb.TestimonialId = objectModel.TestimonialId;
             tblTestimonialDb.AuthorName = objectModel.AuthorName;
             tblTestimonialDb.Designation = objectModel.Designation;
             tblTestimonialDb.Description = objectModel.Description;
             tblTestimonialDb.DisplayOrder = objectModel.DisplayOrder;
             tblTestimonialDb.Status = objectModel.Status;

             tblTestimonialDb.ImageFile = objectModel.ImageFile;
             tblTestimonialDb.ActualImageFile = objectModel.ActualImageFile;

             if (objectModel.TestimonialId > 0)
             {

                 tblTestimonialDb.ModifiedBy = AdminSessionData.AdminUserId;
                 tblTestimonialDb.ModifiedOn = DateTime.Now;
                 tblTestimonialDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;

                 context.tblTestimonials.Attach(tblTestimonialDb);
                 context.Entry(tblTestimonialDb).Property(x => x.AuthorName).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.Designation).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.Description).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.DisplayOrder).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.ImageFile).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.ActualImageFile).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.Status).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.ModifiedBy).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.ModifiedOn).IsModified = true;
                 context.Entry(tblTestimonialDb).Property(x => x.ModifiedFromIP).IsModified = true;
             }
             else
             {
                 tblTestimonialDb.CreatedBy = AdminSessionData.AdminUserId;
                 tblTestimonialDb.CreatedOn = DateTime.Now;
                 tblTestimonialDb.CreatedFromIp = HttpContext.Current.Request.UserHostAddress;
                 context.tblTestimonials.Add(tblTestimonialDb);
             }

             int count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;

             return count;
         }


         public int ActDeact(TestimonialModel objectModel)
         {
             tblTestimonial tblTestimonialDb = new tblTestimonial();
             tblTestimonialDb.TestimonialId = objectModel.TestimonialId;
             tblTestimonialDb.Status = objectModel.Status;

             context.tblTestimonials.Attach(tblTestimonialDb);
             context.Entry(tblTestimonialDb).Property(x => x.Status).IsModified = true;

             int count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;

             return count;
         }


         public int Delete(TestimonialModel objectModel)
         {
             int count = -1;

             // check the langauge id if exists in another table 
             //if (!context.tblMemberships.Any(o => o.MembershipId == objectModel.FaqCategoryId))
             //{
             //    count = -2;
             //    return count;
             //}


             tblTestimonial tblTestimonialDb = new tblTestimonial();
             tblTestimonialDb.TestimonialId = objectModel.TestimonialId;
             context.Entry(tblTestimonialDb).State = EntityState.Deleted;
             count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;
             return count;
         }


    }
}
