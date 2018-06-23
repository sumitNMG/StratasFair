using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StratasFair.Data;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using StratasFair.Business.CommonHelper;
using System.IO;
using System.Web;
using StratasFair.Common;
namespace StratasFair.Business.Front
{
    public sealed class PMBHelper
    {
        private PMBHelper() { }
        private static readonly Lazy<PMBHelper> lazy = new Lazy<PMBHelper>(() => new PMBHelper());
        public static PMBHelper Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        public long AddNewPMB(PMBModel _model)
        {
            try
            {
                if (_model.Attachment != null)
                {
                    Guid guid = Guid.NewGuid();
                    string ext = System.IO.Path.GetExtension(_model.Attachment.FileName);
                    _model.AttachedActualFileName = _model.Attachment.FileName;
                    _model.AttachedFileName = guid.ToString() + ext;
                }
                tblPMB _pmbModel = new tblPMB();
                _pmbModel.QuotationRequestId = _model.QuotatoinId;
                _pmbModel.AttachedFileActualName = _model.AttachedActualFileName;
                _pmbModel.AttachedFileName = _model.AttachedFileName;
                _pmbModel.CreatedBy = _model.CreatedBy;
                _pmbModel.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                _pmbModel.CreatedOn = DateTime.UtcNow;
                _pmbModel.CreatedByUserType = _model.CreatedByUserType;
                _pmbModel.Message = _model.Message;
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    context.tblPMBs.Add(_pmbModel);
                    _model.PMBId = context.SaveChanges();
                }
                if (_model.PMBId > 0)
                {
                    if (_model.Attachment != null)
                    {
                        string path = string.Empty;
                        string initialPath = "resources/vendor-owner-pmb/" + _model.QuotatoinId;

                        // Add/Delete the new trade and business file and image details
                        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Content/" + initialPath + "/pmbFiles/")))
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Content/" + initialPath + "/pmbFiles/"));
                        }
                        // save the file locally
                        path = HttpContext.Current.Server.MapPath(Path.Combine("~/Content/" + initialPath + "/pmbFiles/" + _model.AttachedFileName));
                        _model.Attachment.SaveAs(path);

                        // save the file on s3
                        AwsS3Bucket.CreateFile(initialPath + "/pmbFiles/" + _model.AttachedFileName, path);

                        // delete the file locally
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
                return _model.PMBId;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public List<PMBModel> GetPMB(long QuotationRequestId)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    return context.Usp_GetPMB(QuotationRequestId).Select(x => new PMBModel()
                      {
                          PMBId = x.PMBId,
                          QuotatoinId = QuotationRequestId,
                          VendorId = x.VendorId,
                          VendorName = x.VendorName,
                          VendorImage = x.ImageFile,
                          ClientUserId = x.UserClientId,
                          ClientUserName = x.FullName,
                          Message = x.Message,
                          ClientImage = x.ProfilePicture,
                          AttachedActualFileName = x.AttachedFileActualName,
                          AttachedFileName = x.AttachedFileName,
                          CreatedBy = x.CreatedBy,
                          CreatedFromIP = x.CreatedFromIP,
                          CreatedOn = x.CreatedOn,
                          CreatedByUserType = x.CreatedByUserType,
                          StrataBoardId = x.StratasBoardId
                      }).OrderBy(m => m.CreatedOn).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public long AddNewAdminPMB(AdminPMBModel _model)
        {
            try
            {
                if (_model.Attachment != null)
                {
                    Guid guid = Guid.NewGuid();
                    string ext = System.IO.Path.GetExtension(_model.Attachment.FileName);
                    _model.AttachedFileActualName = _model.Attachment.FileName;
                    _model.AttachedFileName = guid.ToString() + ext;
                }
                tblAdminPMB _pmbModel = new tblAdminPMB();
                _pmbModel.AttachedFileActualName = _model.AttachedFileActualName;
                _pmbModel.AttachedFileName = _model.AttachedFileName;
                _pmbModel.Message = _model.Message;
                _pmbModel.CreatedBy = ClientSessionData.UserClientId;
                _pmbModel.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                _pmbModel.CreatedOn = DateTime.UtcNow;
                _pmbModel.CreatedFor = _model.CreatedFor;
                _pmbModel.CreatedByUserType = ClientSessionData.ClientRoleName;
                _pmbModel.StrataBoardId = ClientSessionData.ClientStrataBoardId;
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    context.tblAdminPMBs.Add(_pmbModel);
                    _model.PMBId = context.SaveChanges();
                }
                if (_model.PMBId > 0)
                {
                    if (_model.Attachment != null)
                    {
                        string path = string.Empty;
                        string initialPath = "resources/admin-owner-pmb/" + ClientSessionData.ClientStrataBoardId;

                        // Add/Delete the new trade and business file and image details
                        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Content/" + initialPath + "/pmbFiles/")))
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Content/" + initialPath + "/pmbFiles/"));
                        }
                        // save the file locally
                        path = HttpContext.Current.Server.MapPath(Path.Combine("~/Content/" + initialPath + "/pmbFiles/" + _model.AttachedFileName));
                        _model.Attachment.SaveAs(path);

                        // save the file on s3
                        AwsS3Bucket.CreateFile(initialPath + "/pmbFiles/" + _model.AttachedFileName, path);

                        // delete the file locally
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
                return _model.PMBId;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public List<AdminPMBModel> GetAdminPMB(long ? StrataBoardId, long? CreatedFor)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    return context.Usp_GetAdminPMB(StrataBoardId, CreatedFor).Select(x => new AdminPMBModel()
                    {
                        PMBId = x.PMBId,
                        OwnerId = x.OwnerId,
                        OwnerFullName = x.OwnerFullName,
                        OwnerProfilePicture = x.OwnerProfilePicture,
                        AdminId = x.AdminId,
                        AdminFullName = x.AdminFullName,
                        AdminProfilePicture = x.AdminProfilePicture,
                        Message = x.Message,
                        AttachedFileActualName = x.AttachedFileActualName,
                        AttachedFileName = x.AttachedFileName,
                        CreatedBy = x.CreatedBy,
                        CreatedFromIP = x.CreatedFromIP,
                        CreatedOn = x.CreatedOn,
                        CreatedFor=x.CreatedFor,
                        CreatedByUserType = x.CreatedByUserType,
                        StratasBoardId = x.StratasBoardId,
                        StratasBoardName = x.StratasBoardName,
                        CreatedByUserName=x.CreatedByUserName
                    }).OrderBy(m => m.CreatedOn).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool IsOwnerInStrataBoard(long ? UserClientId , long StrataBoardId)
        {
            try
            {
                using(StratasFairDBEntities context=new StratasFairDBEntities())
                {
                    return context.tblUserClients.Any(x => x.UserClientId == UserClientId && x.StratasBoardId == StrataBoardId);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
