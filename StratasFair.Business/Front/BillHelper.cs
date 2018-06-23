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
    public class BillHelper
    {
        StratasFairDBEntities _context;
        public string _conString = String.Empty;

        public BillHelper()
        {
            if (_context == null)
            {
                _context = new StratasFairDBEntities();
            }
        }

        public List<BillTypeModel> GetAllBillTypes(int BlockNumber, int BlockSize)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<BillTypeModel> billTypeModelList = new List<BillTypeModel>();
                try
                {
                    var BillTypes = _context.tblBillTypes.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId && x.Status == 1).OrderByDescending(x=>x.CreatedOn).ToList();
                    foreach (var item in BillTypes)
                    {
                        BillTypeModel billTypeModel = new BillTypeModel();
                        billTypeModel.BillTypeId = item.BillTypeId;
                        billTypeModel.BillTypeName = item.BillTypeName;
                        billTypeModel.BillPeriod = item.BillPeriod;
                        billTypeModel.BillPeriodName = item.BillPeriod == 5 ? "Yearly" : item.BillPeriod == 4 ? "Half-Yearly" : item.BillPeriod == 3 ? "Quarterly" : item.BillPeriod == 2 ? "Monthly" : "Once";
                        billTypeModel.CreatedOn = item.CreatedOn != null ? item.CreatedOn.Value.ToString("dd MMM, yyyy") : "N/A";
                        billTypeModelList.Add(billTypeModel);
                    }
                }
                catch
                {
                }
                billTypeModelList = billTypeModelList.Skip(startIndex).Take(BlockSize).ToList();
                return billTypeModelList;
            }
            else
            {
                return null;
            }
        }

        public BillTypeModel GetBillTypeDetail(int BillTypeId)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                BillTypeModel billTypeModel = new BillTypeModel();
                try
                {
                    var BillTypes = _context.tblBillTypes.Where(x => x.BillTypeId == BillTypeId && x.Status == 1).FirstOrDefault();
                    if (BillTypes != null)
                    {
                        billTypeModel.BillTypeId = BillTypes.BillTypeId;
                        billTypeModel.BillTypeName = BillTypes.BillTypeName;
                        billTypeModel.BillPeriod = BillTypes.BillPeriod;
                    }
                }
                catch
                {
                }
                return billTypeModel;
            }
            else
            {
                return null;
            }
        }

        public int AddUpdateBillType(BillTypeModel billTypeModel)
        {
            int result = 0;
            try
            {
                tblBillType tblBillTypeDb = new tblBillType();
                tblBillTypeDb.BillTypeName = billTypeModel.BillTypeName;
                tblBillTypeDb.BillPeriod = billTypeModel.BillPeriod;
                tblBillTypeDb.CreatedBy = ClientSessionData.UserClientId;
                tblBillTypeDb.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblBillTypeDb.CreatedOn = DateTime.UtcNow;
                tblBillTypeDb.ModifiedBy = ClientSessionData.UserClientId;
                tblBillTypeDb.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                tblBillTypeDb.ModifiedOn = DateTime.UtcNow;
                tblBillTypeDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;
                tblBillTypeDb.Status = 1;

                if (billTypeModel.BillTypeId > 0)
                {
                    tblBillTypeDb.BillTypeId = billTypeModel.BillTypeId;
                    _context.tblBillTypes.Attach(tblBillTypeDb);
                    _context.Entry(tblBillTypeDb).Property(x => x.BillTypeName).IsModified = true;
                    _context.Entry(tblBillTypeDb).Property(x => x.BillPeriod).IsModified = true;
                    _context.Entry(tblBillTypeDb).Property(x => x.ModifiedBy).IsModified = true;
                    _context.Entry(tblBillTypeDb).Property(x => x.ModifiedOn).IsModified = true;
                    _context.Entry(tblBillTypeDb).Property(x => x.ModifiedFromIP).IsModified = true;
                    result = _context.SaveChanges();
                }
                else
                {
                    _context.tblBillTypes.Add(tblBillTypeDb);
                    result = _context.SaveChanges();
                }
                return result;
            }
            catch
            {
                result = -2;
            }
            return result;
        }

        public int DeleteBillType(int BillTypeId)
        {
            int result = 0;
            try
            {
                var BillTypeBooking = _context.tblOwnerBills.Where(x => x.BillTypeId == BillTypeId).FirstOrDefault();
                if(BillTypeBooking != null)
                {
                    var BillTypeMaintenance = _context.tblOwnerBillDetails.Where(x => x.OwnerBillId == BillTypeBooking.OwnerBillId).FirstOrDefault();
                    if (BillTypeBooking != null || BillTypeMaintenance != null)
                    {
                        result = -1;
                    }
                }
                else
                {
                    var BillTypes = _context.tblBillTypes.Where(x => x.BillTypeId == BillTypeId).FirstOrDefault();
                    if (BillTypes != null)
                    {
                        _context.tblBillTypes.Remove(BillTypes);
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
