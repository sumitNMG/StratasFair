using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StratasFair.Business.Front
{
    public class CommonAreaHelper
    {

        StratasFairDBEntities _context;
        public string _conString = String.Empty;

        public CommonAreaHelper()
        {
            if (_context == null)
            {
                _context = new StratasFairDBEntities();
            }
        }

        public List<CommonAreaModel> GetAllCommonAreas(int BlockNumber, int BlockSize)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<CommonAreaModel> commonAreaModelList = new List<CommonAreaModel>();
                try
                {
                    var CommonAreas = _context.tblCommonAreas.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1).OrderByDescending(x => x.CreatedOn).ToList();
                    foreach (var item in CommonAreas)
                    {
                        CommonAreaModel commonAreaModel = new CommonAreaModel();
                        commonAreaModel.CommonAreaId = item.CommonAreaId;
                        commonAreaModel.CommonAreaName = item.CommonAreaName;
                        commonAreaModel.CreatedOn = item.CreatedOn != null ? item.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                        commonAreaModelList.Add(commonAreaModel);
                    }
                }
                catch
                {
                }
                commonAreaModelList = commonAreaModelList.Skip(startIndex).Take(BlockSize).ToList();
                return commonAreaModelList;
            }
            else
            {
                return null;
            }
        }

        public CommonAreaModel GetCommonAreaDetail(int CommonAreaId)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                CommonAreaModel commonAreaModel = new CommonAreaModel();
                try
                {
                    var CommonAreas = _context.tblCommonAreas.Where(x => x.CommonAreaId == CommonAreaId && x.Status == 1).FirstOrDefault();
                    if (CommonAreas != null)
                    {
                        commonAreaModel.CommonAreaId = CommonAreas.CommonAreaId;
                        commonAreaModel.CommonAreaName = CommonAreas.CommonAreaName;
                    }
                }
                catch
                {
                }
                return commonAreaModel;
            }
            else
            {
                return null;
            }
        }

        public int AddUpdateCommonArea(CommonAreaModel commonAreaModel)
        {
            int result = 0;
            try
            {
                tblCommonArea tblCommonAreaDb = new tblCommonArea();
                tblCommonAreaDb.CommonAreaName = commonAreaModel.CommonAreaName;
                tblCommonAreaDb.CreatedBy = ClientSessionData.UserClientId;
                tblCommonAreaDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblCommonAreaDb.CreatedOn = DateTime.UtcNow;
                tblCommonAreaDb.ModifiedBy = ClientSessionData.UserClientId;
                tblCommonAreaDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblCommonAreaDb.ModifiedOn = DateTime.UtcNow;
                tblCommonAreaDb.Status = 1;
                tblCommonAreaDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;

                var CommonAreas = _context.tblCommonAreas.Where(x => x.CommonAreaName == commonAreaModel.CommonAreaName && x.CommonAreaId != commonAreaModel.CommonAreaId).FirstOrDefault();
                if (CommonAreas != null)
                {
                    result = -1;
                }
                else
                {
                    if (commonAreaModel.CommonAreaId > 0)
                    {
                        tblCommonAreaDb.CommonAreaId = commonAreaModel.CommonAreaId;
                        _context.tblCommonAreas.Attach(tblCommonAreaDb);
                        _context.Entry(tblCommonAreaDb).Property(x => x.CommonAreaName).IsModified = true;
                        _context.Entry(tblCommonAreaDb).Property(x => x.ModifiedBy).IsModified = true;
                        _context.Entry(tblCommonAreaDb).Property(x => x.ModifiedOn).IsModified = true;
                        _context.Entry(tblCommonAreaDb).Property(x => x.ModifiedFromIP).IsModified = true;
                        result = _context.SaveChanges();
                    }
                    else
                    {
                        _context.tblCommonAreas.Add(tblCommonAreaDb);
                        result = _context.SaveChanges();
                    }
                }
                return result;
            }
            catch
            {
                result = -2;

            }
            return result;
        }

        public int DeleteCommonArea(int CommonAreaId)
        {
            int result = 0;
            try
            {
                var CommonAreaBooking = _context.tblUserBookingRequests.Where(x => x.CommonAreaId == CommonAreaId).FirstOrDefault();
                var CommonAreaMaintenance = _context.tblMaintenanceSchedules.Where(x => x.CommonAreaId == CommonAreaId).FirstOrDefault();
                if (CommonAreaBooking != null || CommonAreaMaintenance != null)
                {
                    result = -1;
                }
                else
                {
                    var CommonAreas = _context.tblCommonAreas.Where(x => x.CommonAreaId == CommonAreaId).FirstOrDefault();
                    if (CommonAreas != null)
                    {
                        _context.tblCommonAreas.Remove(CommonAreas);
                        result = _context.SaveChanges();
                    }
                }
            }
            catch
            {

            }
            return result;
        }
    }
}
