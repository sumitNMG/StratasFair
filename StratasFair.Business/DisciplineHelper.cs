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

namespace StratasFair.Business
{
    public class DisciplineHelper
    {
         StratasFairDBEntities context;
         public DisciplineHelper()
        {
            if (context == null)
                context = new StratasFairDBEntities();
        }

         public List<DisciplineModel> GetAll()
         {
             return context.tblDisciplines.Select(x => new DisciplineModel() { DisciplineId = x.DisciplineId, DisciplineName = x.DisciplineName, CreatedOn = (DateTime)x.CreatedOn, Status = x.Status.Value }).ToList();
         }

         public DisciplineModel GetByID(int Id)
         {
             return context.tblDisciplines.Where(x => x.DisciplineId == Id).Select(x => new DisciplineModel() { DisciplineId = x.DisciplineId, DisciplineName = x.DisciplineName, CreatedOn = (DateTime)x.CreatedOn, Status = x.Status.Value }).FirstOrDefault();
         }

      

         public int AddUpdate(DisciplineModel objectModel)
         {
             if (objectModel.DisciplineId > 0)
             {
                 if (context.tblDisciplines.Any(x => x.DisciplineName == objectModel.DisciplineName && x.DisciplineId != objectModel.DisciplineId))
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
                 if (context.tblDisciplines.Any(x => x.DisciplineName == objectModel.DisciplineName ))
                 {
                     return 1;
                 }
                 else
                 {
                     return update(objectModel);
                 }
             }
         }

         public int update(DisciplineModel objectModel)
         {

             tblDiscipline tblDisciplineDb = new tblDiscipline();
             tblDisciplineDb.DisciplineId = objectModel.DisciplineId;
             tblDisciplineDb.DisciplineName = objectModel.DisciplineName;
             tblDisciplineDb.Status = objectModel.Status;

             if (objectModel.DisciplineId > 0)
             {

                 tblDisciplineDb.ModifiedBy = AdminSessionData.AdminUserId;
                 tblDisciplineDb.ModifiedOn = DateTime.Now;
                 tblDisciplineDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;

                 context.tblDisciplines.Attach(tblDisciplineDb);
                 context.Entry(tblDisciplineDb).Property(x => x.DisciplineName).IsModified = true;
                 context.Entry(tblDisciplineDb).Property(x => x.Status).IsModified = true;
                 context.Entry(tblDisciplineDb).Property(x => x.ModifiedBy).IsModified = true;
                 context.Entry(tblDisciplineDb).Property(x => x.ModifiedOn).IsModified = true;
                 context.Entry(tblDisciplineDb).Property(x => x.ModifiedFromIP).IsModified = true;
             }
             else
             {
                 tblDisciplineDb.CreatedBy = AdminSessionData.AdminUserId;
                 tblDisciplineDb.CreatedOn = DateTime.Now;
                 tblDisciplineDb.CreatedFromIp = HttpContext.Current.Request.UserHostAddress;
                 context.tblDisciplines.Add(tblDisciplineDb);
             }

             int count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;

             return count;
         }


         public int ActDeact(DisciplineModel objectModel)
         {
             tblDiscipline tblDisciplineDb = new tblDiscipline();
             tblDisciplineDb.DisciplineId = objectModel.DisciplineId;
             tblDisciplineDb.Status = objectModel.Status;

             context.tblDisciplines.Attach(tblDisciplineDb);
             context.Entry(tblDisciplineDb).Property(x => x.Status).IsModified = true;

             int count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;

             return count;
         }


         public int Delete(DisciplineModel objectModel)
         {
             int count = -2;

             // check the vendor information if exists in another table 
             if (context.tblVendors.Any(o => o.DisciplineId == objectModel.DisciplineId))
             {
                 return count;
             }


             tblDiscipline tblDisciplineDb = new tblDiscipline();
             tblDisciplineDb.DisciplineId = objectModel.DisciplineId;
             context.Entry(tblDisciplineDb).State = EntityState.Deleted;
             count = context.SaveChanges();
             if (count == 1)
                 count = 0;
             else
                 count = -1;
             return count;
         }

    }
}
