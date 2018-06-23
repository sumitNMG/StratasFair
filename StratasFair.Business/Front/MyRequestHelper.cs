using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.Business.Front
{
    public class MyRequestHelper
    {
        StratasFairDBEntities _context;
        public string _conString = String.Empty;

        public MyRequestHelper()
        {
            if (_context == null)
            {
                _context = new StratasFairDBEntities();
            }
        }

        public List<StrataOwnerMyRequestModel> GetStrataOwnerMyRequest(int BlockNumber, int BlockSize)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<StrataOwnerMyRequestModel> strataOwnerMyRequestModelList = new List<StrataOwnerMyRequestModel>();
                try
                {
                    var userRequests = _context.tblUserRequests.Where(x => x.UserClientId == ClientSessionData.UserClientId).OrderByDescending(x => x.CreatedOn).ToList();
                    foreach (var item in userRequests)
                    {
                        StrataOwnerMyRequestModel strataOwnerMyRequestModel = new StrataOwnerMyRequestModel();
                        strataOwnerMyRequestModel.RequestId = item.RequestId;
                        strataOwnerMyRequestModel.UserClientId = item.UserClientId;
                        strataOwnerMyRequestModel.RequestTitle = item.RequestTitle;
                        strataOwnerMyRequestModel.Status = item.Status == 2 ? "Rejected" : item.Status == 1 ? "Approved" : "Pending";
                        strataOwnerMyRequestModel.FromDate = item.FromDate != null ? item.FromDate.Value.ToString("dd MMM, yyyy") : "N/A";
                        strataOwnerMyRequestModel.ToDate = item.ToDate != null ? item.ToDate.Value.ToString("dd MMM, yyyy") : "N/A";
                        strataOwnerMyRequestModel.Details = item.Details;
                        if (item.FromDate != null && item.ToDate != null)
                        {
                            strataOwnerMyRequestModel.TotalDays = Convert.ToInt32((item.ToDate.Value - item.FromDate.Value).TotalDays + 1);
                        }
                        strataOwnerMyRequestModelList.Add(strataOwnerMyRequestModel);
                    }
                }
                catch
                {
                }
                strataOwnerMyRequestModelList = strataOwnerMyRequestModelList.Skip(startIndex).Take(BlockSize).ToList();
                return strataOwnerMyRequestModelList;
            }
            else
            {
                return null;
            }
        }

        public int AddStrataOwnerMyRequest(StrataOwnerMyRequestModel strataOwnerMyRequestModel)
        {
            int result = 0;
            try
            {
                tblUserRequest tblUserRequestDb = new tblUserRequest();
                tblUserRequestDb.UserClientId = ClientSessionData.UserClientId;
                tblUserRequestDb.RequestTitle = strataOwnerMyRequestModel.RequestTitle;
                tblUserRequestDb.FromDate = !string.IsNullOrEmpty(strataOwnerMyRequestModel.FromDate) ? Convert.ToDateTime(strataOwnerMyRequestModel.FromDate) : !string.IsNullOrEmpty(strataOwnerMyRequestModel.ToDate) ? Convert.ToDateTime(strataOwnerMyRequestModel.ToDate) : (DateTime?)null;
                tblUserRequestDb.ToDate = !string.IsNullOrEmpty(strataOwnerMyRequestModel.ToDate) ? Convert.ToDateTime(strataOwnerMyRequestModel.ToDate) : !string.IsNullOrEmpty(strataOwnerMyRequestModel.FromDate) ? Convert.ToDateTime(strataOwnerMyRequestModel.FromDate) : (DateTime?)null;
                tblUserRequestDb.Details = strataOwnerMyRequestModel.Details;
                tblUserRequestDb.Status = 0;
                tblUserRequestDb.CreatedOn = DateTime.UtcNow;
                tblUserRequestDb.ModifiedOn = DateTime.UtcNow;
                tblUserRequestDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;
                var UserRequests = _context.tblUserRequests.Where(x => x.UserClientId == ClientSessionData.UserClientId && x.RequestTitle == strataOwnerMyRequestModel.RequestTitle && x.Status != 2).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                if (UserRequests != null)
                {
                    if (UserRequests.FromDate != null && UserRequests.ToDate != null)
                    {
                        if (UserRequests.FromDate > Convert.ToDateTime(strataOwnerMyRequestModel.ToDate) || UserRequests.ToDate < Convert.ToDateTime(strataOwnerMyRequestModel.FromDate))
                        {

                            _context.tblUserRequests.Add(tblUserRequestDb);
                            result = _context.SaveChanges();
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strataOwnerMyRequestModel.FromDate) && !string.IsNullOrEmpty(strataOwnerMyRequestModel.ToDate))
                        {
                            _context.tblUserRequests.Add(tblUserRequestDb);
                            result = _context.SaveChanges();
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                }
                else
                {
                    _context.tblUserRequests.Add(tblUserRequestDb);
                    result = _context.SaveChanges();
                }
                if (result == 1)
                {
                    EmailSender.FncSend_StratasBoard_OwnerRequestMail_ToStrataAdmin(ClientSessionData.UserClientId, ClientSessionData.ClientStrataBoardId);
                }
            }
            catch
            {

            }
            return result;
        }

        public int DeleteStrataOwnerMyRequest(int RequestId)
        {
            int result = 0;
            try
            {

                var UserRequests = _context.tblUserRequests.Where(x => x.RequestId == RequestId && x.Status == 0).FirstOrDefault();

                if (UserRequests != null)
                {
                    _context.tblUserRequests.Remove(UserRequests);
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }


        public List<StrataOwnerMyRequestModel> GetStrataOwnerMyRequestForAdmin(int BlockNumber, int BlockSize)
        {
            if (ClientSessionData.UserClientId != 0)
            {
                int startIndex = (BlockNumber - 1) * BlockSize;
                List<StrataOwnerMyRequestModel> strataOwnerMyRequestModelList = new List<StrataOwnerMyRequestModel>();
                try
                {
                    var userRequests = _context.tblUserRequests.Where(x => x.StratasBoardId == ClientSessionData.ClientStrataBoardId).OrderByDescending(x => x.CreatedOn).ToList();
                    foreach (var item in userRequests)
                    {
                        StrataOwnerMyRequestModel strataOwnerMyRequestModel = new StrataOwnerMyRequestModel();
                        strataOwnerMyRequestModel.RequestId = item.RequestId;
                        strataOwnerMyRequestModel.UserClientId = item.UserClientId;
                        strataOwnerMyRequestModel.RequestTitle = item.RequestTitle;
                        strataOwnerMyRequestModel.Status = item.Status == 2 ? "Rejected" : item.Status == 1 ? "Approved" : "Pending...";
                        strataOwnerMyRequestModel.FromDate = item.FromDate != null ? item.FromDate.Value.ToString("dd MMM, yyyy") : "N/A";
                        strataOwnerMyRequestModel.ToDate = item.ToDate != null ? item.ToDate.Value.ToString("dd MMM, yyyy") : "N/A";
                        strataOwnerMyRequestModel.Details = item.Details;
                        if (item.FromDate != null && item.ToDate != null)
                        {
                            strataOwnerMyRequestModel.TotalDays = Convert.ToInt32((item.ToDate.Value - item.FromDate.Value).TotalDays + 1);
                        }
                        strataOwnerMyRequestModelList.Add(strataOwnerMyRequestModel);
                    }
                }
                catch
                {
                }
                strataOwnerMyRequestModelList = strataOwnerMyRequestModelList.Skip(startIndex).Take(BlockSize).ToList();
                return strataOwnerMyRequestModelList;
            }
            else
            {
                return null;
            }
        }

        public int UpdateMyRequestStatusToApprove(int RequestId)
        {
            int result = 0;
            try
            {
                var UserRequests = _context.tblUserRequests.AsNoTracking().Where(x => x.RequestId == RequestId && x.Status == 0).FirstOrDefault();
                if (UserRequests != null)
                {
                    tblUserRequest tblUserRequestDb = new tblUserRequest();
                    tblUserRequestDb.UserClientId = UserRequests.UserClientId;
                    tblUserRequestDb.Status = 1;   ///// For Approved 
                    tblUserRequestDb.RequestTitle = UserRequests.RequestTitle;
                    tblUserRequestDb.Details = UserRequests.Details;
                    tblUserRequestDb.FromDate = UserRequests.FromDate;
                    tblUserRequestDb.ToDate = UserRequests.ToDate;
                    tblUserRequestDb.CreatedOn = UserRequests.CreatedOn;
                    tblUserRequestDb.StatusRemark = string.Empty;
                    tblUserRequestDb.StatusUpdateBy = ClientSessionData.UserClientId;
                    tblUserRequestDb.StatusUpdateOn = DateTime.UtcNow;
                    tblUserRequestDb.ModifiedOn = DateTime.UtcNow;
                    tblUserRequestDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;

                    tblUserRequestDb.RequestId = RequestId;
                    _context.Entry(tblUserRequestDb).State = EntityState.Modified;
                    _context.Entry(tblUserRequestDb).Property(x => x.RequestTitle).IsModified = true;
                    _context.Entry(tblUserRequestDb).Property(x => x.UserClientId).IsModified = true;
                    _context.Entry(tblUserRequestDb).Property(x => x.Details).IsModified = true;
                    _context.Entry(tblUserRequestDb).Property(x => x.Status).IsModified = true;
                    _context.Entry(tblUserRequestDb).Property(x => x.StatusRemark).IsModified = true;
                    _context.Entry(tblUserRequestDb).Property(x => x.StatusUpdateBy).IsModified = true;
                    _context.Entry(tblUserRequestDb).Property(x => x.StatusUpdateOn).IsModified = true;
                    _context.Entry(tblUserRequestDb).Property(x => x.ModifiedOn).IsModified = true;
                    result = _context.SaveChanges();
                }
            }
            catch
            {

            }
            return result;
        }

        public int UpdateMyRequestStatusToReject(StrataOwnerMyRequestModel model)
        {
            int result = 0;
            try
            {
                var UserRequests = _context.tblUserRequests.AsNoTracking().Where(x => x.RequestId == model.RequestId && x.Status == 0).FirstOrDefault();
                if (UserRequests != null)
                {
                    tblUserRequest tblUserRequestDb = new tblUserRequest();
                    tblUserRequestDb.UserClientId = UserRequests.UserClientId;
                    tblUserRequestDb.Status = 2;   ///// For Rejected 
                    tblUserRequestDb.RequestTitle = UserRequests.RequestTitle;
                    tblUserRequestDb.Details = UserRequests.Details;
                    tblUserRequestDb.FromDate = UserRequests.FromDate;
                    tblUserRequestDb.ToDate = UserRequests.ToDate;
                    tblUserRequestDb.CreatedOn = UserRequests.CreatedOn;
                    tblUserRequestDb.StatusRemark = model.StatusRemark;
                    tblUserRequestDb.StatusUpdateBy = ClientSessionData.UserClientId;
                    tblUserRequestDb.StatusUpdateOn = DateTime.UtcNow;
                    tblUserRequestDb.ModifiedOn = DateTime.UtcNow;
                    tblUserRequestDb.StratasBoardId = ClientSessionData.ClientStrataBoardId;

                    if (model.RequestId > 0)
                    {
                        tblUserRequestDb.RequestId = model.RequestId;
                        _context.Entry(tblUserRequestDb).State = EntityState.Modified;
                        _context.Entry(tblUserRequestDb).Property(x => x.RequestTitle).IsModified = true;
                        _context.Entry(tblUserRequestDb).Property(x => x.UserClientId).IsModified = true;
                        _context.Entry(tblUserRequestDb).Property(x => x.Details).IsModified = true;
                        _context.Entry(tblUserRequestDb).Property(x => x.Status).IsModified = true;
                        _context.Entry(tblUserRequestDb).Property(x => x.StatusRemark).IsModified = true;
                        _context.Entry(tblUserRequestDb).Property(x => x.StatusUpdateBy).IsModified = true;
                        _context.Entry(tblUserRequestDb).Property(x => x.StatusUpdateOn).IsModified = true;
                        _context.Entry(tblUserRequestDb).Property(x => x.ModifiedOn).IsModified = true;
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
