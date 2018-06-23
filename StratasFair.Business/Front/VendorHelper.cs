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
using StratasFair.BusinessEntity.Front;
using StratasFair.Common;
using System.Data;

namespace StratasFair.Business.Front
{
    public sealed class VendorHelper
    {
        private VendorHelper() { }
        private static readonly Lazy<VendorHelper> lazy = new Lazy<VendorHelper>(() => new VendorHelper());
        public static VendorHelper Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public long AddNewVendor(VendorModel objectModel, HttpPostedFileBase image, HttpPostedFileBase tradeFile)
        {
            long _vendorId = -2;
            if (!IsEmailExists(objectModel.EmailId))
            {
                if (image != null)
                {
                    Guid g = Guid.NewGuid();
                    objectModel.ActualImageFile = image.FileName;
                    objectModel.ImageFile = g.ToString() + Path.GetExtension(image.FileName);
                }

                if (tradeFile != null)
                {
                    Guid g2 = Guid.NewGuid();
                    objectModel.ActualTradeAndBusinessFile = tradeFile.FileName;
                    objectModel.TradeAndBusinessFile = g2.ToString() + Path.GetExtension(tradeFile.FileName);
                }
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
                tblVendorDb.AdminApproval = 0;
                tblVendorDb.Status = 1;
                Encrypt64 enc = new Encrypt64();
                tblVendorDb.Password = enc.Encrypt(AppLogic.GenerateRandomString(8));
                tblVendorDb.CreatedOn = DateTime.UtcNow;
                tblVendorDb.CreatedFromIp = HttpContext.Current.Request.UserHostAddress;
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    context.tblVendors.Add(tblVendorDb);
                    context.SaveChanges();
                }
                _vendorId = tblVendorDb.VendorId;
                try
                {
                    if (_vendorId > 0)
                    {
                        string path = string.Empty;
                        int fileMapped = -1;
                        string initialPath = "resources/vendor/" + _vendorId;

                        if (image != null)
                        {
                            // Add/Delete the new trade and business file and image details
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Content/" + initialPath + "/ProfilePicture/")))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Content/" + initialPath + "/ProfilePicture/"));
                            }
                            // save the file locally
                            path = HttpContext.Current.Server.MapPath(Path.Combine("~/Content/" + initialPath + "/ProfilePicture/" + objectModel.ImageFile));
                            image.SaveAs(path);

                            // save the file on s3
                            fileMapped = AwsS3Bucket.CreateFile(initialPath + "/ProfilePicture/" + objectModel.ImageFile, path);

                            // delete the file locally
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        if (tradeFile != null)
                        {
                            // Add/Delete the new trade and business file and image details
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Content/" + initialPath + "/TradeFile/")))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Content/" + initialPath + "/TradeFile/"));
                            }
                            // save the file locally
                            path = HttpContext.Current.Server.MapPath(Path.Combine("~/Content/" + initialPath + "/TradeFile/" + objectModel.TradeAndBusinessFile));
                            tradeFile.SaveAs(path);

                            // save the file on s3
                            fileMapped = AwsS3Bucket.CreateFile(initialPath + "/TradeFile/" + objectModel.TradeAndBusinessFile, path);

                            //delete the file locally
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    new AppError().LogMe(ex);
                    // any error is there
                }
                return _vendorId;
            }
            else
            {
                return -3;
            }
        }

        public bool IsEmailExists(string emailId)
        {
            using (StratasFairDBEntities context = new StratasFairDBEntities())
            {
                return context.tblVendors.Any(x => x.EmailId == emailId);
            };
        }

        public bool IsVendorActive(string emailId)
        {
            using (StratasFairDBEntities context = new StratasFairDBEntities())
            {
                return context.tblVendors.Any(x => x.Status == 1 && x.EmailId == emailId);
            }
        }

        public VendorModel GetVendorByEmail(string EmailId)
        {
            using (StratasFairDBEntities context = new StratasFairDBEntities())
            {
                return context.tblVendors.Where(x => x.EmailId == EmailId).Select(v => new VendorModel()
                {
                    ActualImageFile = v.ActualImageFile,
                    EmailId = v.EmailId,
                    CompanyBrief = v.CompanyBrief,
                    TradeAndBusinessFile = v.TradeAndBusinessFile,
                    ImageFile = v.ImageFile,
                    Password = v.Password,
                    VendorName = v.VendorName,
                    ActualTradeAndBusinessFile = v.ActualTradeAndBusinessFile,
                    AdminApproval = v.AdminApproval,
                    CreatedBy = v.CreatedBy,
                    CreatedFromIp = v.CreatedFromIp,
                    CreatedOn = v.CreatedOn,
                    DisciplineId = v.DisciplineId,
                    MobileNumber = v.MobileNumber,
                    ModifiedBy = v.ModifiedBy,
                    ModifiedFromIP = v.ModifiedFromIP,
                    ModifiedOn = v.ModifiedOn,
                    OtherDisciplineName = v.OtherDisciplineName,
                    Remark = v.Remark,
                    Status = v.Status,
                    VendorId = v.VendorId,
                    DisciplineName = context.tblDisciplines.Where(d => d.DisciplineId == v.DisciplineId).FirstOrDefault().DisciplineName,
                    PasswordResetDate = v.Pwd_Reset_Req_Date,
                    PasswordResetStatus = v.Pwd_Reset_Req_Status
                }).FirstOrDefault();
            }
        }

        public VendorModel GetVendorById(long Id)
        {
            using (StratasFairDBEntities context = new StratasFairDBEntities())
            {
                return context.tblVendors.Where(x => x.VendorId == Id).Select(v => new VendorModel()
                {
                    ActualImageFile = v.ActualImageFile,
                    EmailId = v.EmailId,
                    CompanyBrief = v.CompanyBrief,
                    TradeAndBusinessFile = v.TradeAndBusinessFile,
                    ImageFile = v.ImageFile,
                    Password = v.Password,
                    VendorName = v.VendorName,
                    ActualTradeAndBusinessFile = v.ActualTradeAndBusinessFile,
                    AdminApproval = v.AdminApproval,
                    CreatedBy = v.CreatedBy,
                    CreatedFromIp = v.CreatedFromIp,
                    CreatedOn = v.CreatedOn,
                    DisciplineId = v.DisciplineId,
                    MobileNumber = v.MobileNumber,
                    ModifiedBy = v.ModifiedBy,
                    ModifiedFromIP = v.ModifiedFromIP,
                    ModifiedOn = v.ModifiedOn,
                    OtherDisciplineName = v.OtherDisciplineName,
                    Remark = v.Remark,
                    Status = v.Status,
                    VendorId = v.VendorId,
                    DisciplineName = context.tblDisciplines.Where(d=>d.DisciplineId==v.DisciplineId).FirstOrDefault().DisciplineName,
                    PasswordResetDate=v.Pwd_Reset_Req_Date,
                    PasswordResetStatus=v.Pwd_Reset_Req_Status
                }).FirstOrDefault();
            }
        }

        public List<VendorModel> GetVendorList()
        {
            using (StratasFairDBEntities context = new StratasFairDBEntities())
            {
                return context.vw_GetVendor.Where(x => x.Status == 1 && x.AdminApproval == 1).Select(v => new VendorModel()
                {
                    VendorId = v.VendorId,
                    ActualImageFile = v.ActualImageFile,
                    EmailId = v.EmailId,
                    CompanyBrief = v.CompanyBrief,
                    TradeAndBusinessFile = v.TradeAndBusinessFile,
                    ImageFile = v.ImageFile,
                    Password = v.Password,
                    VendorName = v.VendorName,
                    ActualTradeAndBusinessFile = v.ActualTradeAndBusinessFile,
                    AdminApproval = v.AdminApproval,
                    CreatedBy = v.CreatedBy,
                    CreatedFromIp = v.CreatedFromIp,
                    CreatedOn = v.CreatedOn,
                    MobileNumber = v.MobileNumber,
                    ModifiedBy = v.ModifiedBy,
                    ModifiedFromIP = v.ModifiedFromIP,
                    ModifiedOn = v.ModifiedOn,
                    OtherDisciplineName = v.OtherDisciplineName,
                    Remark = v.Remark,
                    Status = v.Status,
                    DisciplineId = v.DisciplineId,
                    DisciplineName = v.DisciplineName
                }).OrderByDescending(o => o.CreatedOn).ToList();
            }
        }

        public List<VendorModel> SearchVendors(string keyword, int? disciplineId, int? pageNo)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["VendorDirectoryPageSize"]);
                    return context.Usp_SearchVendor(keyword, disciplineId, pageSize, pageNo).Select(v => new VendorModel()
                    {
                        VendorId = v.VendorId,
                        ActualImageFile = v.ActualImageFile,
                        EmailId = v.EmailId,
                        CompanyBrief = v.CompanyBrief,
                        TradeAndBusinessFile = v.TradeAndBusinessFile,
                        ImageFile = v.ImageFile,
                        Password = v.Password,
                        VendorName = v.VendorName,
                        ActualTradeAndBusinessFile = v.ActualTradeAndBusinessFile,
                        AdminApproval = v.AdminApproval,
                        CreatedBy = v.CreatedBy,
                        CreatedFromIp = v.CreatedFromIp,
                        CreatedOn = v.CreatedOn,
                        MobileNumber = v.MobileNumber,
                        ModifiedBy = v.ModifiedBy,
                        ModifiedFromIP = v.ModifiedFromIP,
                        ModifiedOn = v.ModifiedOn,
                        OtherDisciplineName = v.OtherDisciplineName,
                        Remark = v.Remark,
                        Status = v.Status,
                        DisciplineId = v.DisciplineId,
                        DisciplineName = v.DisciplineName
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<VendorModel> GetVendorsByDiscipline(long DisciplineId)
        {
            using (StratasFairDBEntities context = new StratasFairDBEntities())
            {
                return context.vw_GetVendor.Where(x => x.DisciplineId == DisciplineId && x.Status == 1 && x.AdminApproval == 1).Select(v => new VendorModel()
                {
                    ActualImageFile = v.ActualImageFile,
                    EmailId = v.EmailId,
                    CompanyBrief = v.CompanyBrief,
                    TradeAndBusinessFile = v.TradeAndBusinessFile,
                    ImageFile = v.ImageFile,
                    Password = v.Password,
                    VendorName = v.VendorName,
                    ActualTradeAndBusinessFile = v.ActualTradeAndBusinessFile,
                    AdminApproval = v.AdminApproval,
                    CreatedBy = v.CreatedBy,
                    CreatedFromIp = v.CreatedFromIp,
                    CreatedOn = v.CreatedOn,
                    DisciplineId = v.DisciplineId,
                    MobileNumber = v.MobileNumber,
                    ModifiedBy = v.ModifiedBy,
                    ModifiedFromIP = v.ModifiedFromIP,
                    ModifiedOn = v.ModifiedOn,
                    OtherDisciplineName = v.OtherDisciplineName,
                    Remark = v.Remark,
                    Status = v.Status,
                    VendorId = v.VendorId,
                    DisciplineName = v.DisciplineName
                }).OrderByDescending(o => o.CreatedOn).ToList();
            }
        }


        public int ResetVendorPassword(string email, string password)
        {
            using (StratasFairDBEntities context = new StratasFairDBEntities())
            {
                var Vendor = context.tblVendors.Where(x => x.EmailId == email).FirstOrDefault();
                Vendor.Password = password;
                Vendor.Pwd_Reset_Req_Status = 1;
                Vendor.ModifiedOn = DateTime.UtcNow;
                Vendor.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                context.tblVendors.Attach(Vendor);

                context.Entry(Vendor).Property(x => x.Password).IsModified = true;
                context.Entry(Vendor).Property(x => x.Pwd_Reset_Req_Status).IsModified = true;
                context.Entry(Vendor).Property(x => x.ModifiedOn).IsModified = true;
                context.Entry(Vendor).Property(x => x.ModifiedFromIP).IsModified = true;

                return context.SaveChanges();
            }
        }

        public int AddOtherDiscipline(string disciplineName)
        {
            try
            {
                tblDiscipline discipline = new tblDiscipline();
                discipline.DisciplineName = disciplineName;
                discipline.CreatedBy = 0;
                discipline.CreatedOn = DateTime.UtcNow;
                discipline.CreatedFromIp = HttpContext.Current.Request.UserHostAddress;
                discipline.Status = 1;
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    context.tblDisciplines.Add(discipline);
                    context.SaveChanges();
                }
                return discipline.DisciplineId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPasswordRequestDate(string emailId)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    tblVendor _vendor = context.tblVendors.Where(x => x.EmailId == emailId).FirstOrDefault();
                    _vendor.Pwd_Reset_Req_Date = DateTime.UtcNow;
                    _vendor.Pwd_Reset_Req_Status = 0;
                    context.tblVendors.Attach(_vendor);
                    context.Entry(_vendor).Property(x => x.Pwd_Reset_Req_Date).IsModified = true;
                    context.Entry(_vendor).Property(x => x.Pwd_Reset_Req_Status).IsModified = true;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsDisciplineExists(string disciplineName)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    return context.tblDisciplines.Any(x => x.DisciplineName == disciplineName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
