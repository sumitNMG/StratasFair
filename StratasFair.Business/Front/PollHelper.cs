using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StratasFair.Data;
using System.Threading.Tasks;
using StratasFair.BusinessEntity.Front;
using System.Web;
namespace StratasFair.Business.Front
{
    public sealed class PollHelper
    {
        private PollHelper() { }
        private static readonly Lazy<PollHelper> lazy = new Lazy<PollHelper>(() => new PollHelper());
        public static PollHelper Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        #region //// StratasBoard Admin Section
        public long AddNewPoll(PollQuestionModel _model)
        {
            try
            {
                _model.CreatedBy = ClientSessionData.UserClientId;
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    tblPollQuestion _pollModel = new tblPollQuestion();
                    _pollModel.PollHeader = _model.PollHeader;
                    _pollModel.EndDate = _model.EndDate;
                    _pollModel.CreatedBy = _model.CreatedBy;
                    _pollModel.CreatedOn = _model.CreatedOn;
                    _pollModel.CreatedFromIP = _model.CreatedFromIP;
                    _pollModel.Status = 1;
                    _pollModel.StrataBoardId = ClientSessionData.ClientStrataBoardId;
                    context.tblPollQuestions.Add(_pollModel);
                    if (context.SaveChanges() > 0)
                    {
                        List<tblPollOption> _optionModel = new List<tblPollOption>();
                        foreach (var item in _model.pollOption)
                        {
                            _optionModel.Add(new tblPollOption()
                            {
                                OptionText = item,
                                PollId = _pollModel.PollId,
                                CreatedBy = _model.CreatedBy,
                                CreatedOn = _model.CreatedOn,
                                CreatedFromIP = _model.CreatedFromIP,
                                Status = 1
                            });
                        }
                        context.tblPollOptions.AddRange(_optionModel);
                        context.SaveChanges();
                    }
                    return 0;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }


        //Poll Question List For Admin
        public List<PollQuestionModel> GetPollQuestions(int pageNo)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    IQueryable<PollQuestionModel> model = context.tblPollQuestions.Where(x =>
                        x.CreatedBy == ClientSessionData.UserClientId
                        && x.StrataBoardId == ClientSessionData.ClientStrataBoardId
                        && x.Status == 1).Select(p => new PollQuestionModel()
                    {
                        PollId = p.PollId,
                        PollHeader = p.PollHeader,
                        EndDate = p.EndDate,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        CreatedFromIP = p.CreatedFromIP,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedFromIP = p.ModifiedFromIP,
                        ModifiedOn = p.ModifiedOn,
                        StrataBoardId = p.StrataBoardId,
                        pollOptionList = p.tblPollOptions.Where(to => to.Status == 1).Select(o => new PollOptionModel()
                        {
                            PollOptionId = o.PollOptionId,
                            OptionText = o.OptionText,
                            IsSelected = context.tblPollAnswers.Any(x => x.PollId == p.PollId && x.PollOptionId == o.PollOptionId && x.CreatedBy == ClientSessionData.UserClientId)
                        }).ToList()
                    });

                    var total = model.Count();
                    if (total != 0)
                    {
                        var pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPageSize"]);
                        var skip = pageSize * (pageNo - 1);
                        return model.OrderByDescending(m => m.CreatedOn).Skip(skip).Take(pageSize).ToArray().ToList();
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PollQuestionModel GetPollQuestionById(long PollId)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    return context.tblPollQuestions.Where(x =>
                        x.StrataBoardId == ClientSessionData.ClientStrataBoardId
                        && x.PollId == PollId
                        && x.Status == 1).Select(p => new PollQuestionModel()
                    {
                        PollId = p.PollId,
                        PollHeader = p.PollHeader,
                        EndDate = p.EndDate,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        CreatedFromIP = p.CreatedFromIP,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedFromIP = p.ModifiedFromIP,
                        ModifiedOn = p.ModifiedOn,
                        StrataBoardId = p.StrataBoardId,
                        pollOptionList = p.tblPollOptions.Where(to => to.Status == 1).Select(o => new PollOptionModel()
                        {
                            PollOptionId = o.PollOptionId,
                            OptionText = o.OptionText
                        }).ToList()
                    }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int UpdatePoll(PollQuestionModel _model)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    var _poll = context.tblPollQuestions.Where(x => x.PollId == _model.PollId && x.StrataBoardId == ClientSessionData.ClientStrataBoardId).FirstOrDefault();
                    if (_poll != null)
                    {
                        _poll.Status = _model.Status;
                        _poll.ModifiedBy = _model.ModifiedBy;
                        _poll.ModifiedOn = DateTime.UtcNow;
                        _poll.ModifiedFromIP = HttpContext.Current.Request.UserHostAddress;
                        context.tblPollQuestions.Attach(_poll);

                        context.Entry(_poll).Property(x => x.Status).IsModified = true;
                        context.Entry(_poll).Property(x => x.ModifiedBy).IsModified = true;
                        context.Entry(_poll).Property(x => x.ModifiedOn).IsModified = true;
                        context.Entry(_poll).Property(x => x.ModifiedFromIP).IsModified = true;
                        return context.SaveChanges();
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public PollQuestionModel GetPollAnswersByPollId(long strataFairAdminId, long PollId)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    return context.tblPollQuestions.Where(x => x.CreatedBy == strataFairAdminId && x.PollId == PollId && x.Status == 1).Select(p => new PollQuestionModel()
                    {
                        PollId = p.PollId,
                        PollHeader = p.PollHeader,
                        EndDate = p.EndDate,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        CreatedFromIP = p.CreatedFromIP,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedFromIP = p.ModifiedFromIP,
                        ModifiedOn = p.ModifiedOn,
                        StrataBoardId = p.StrataBoardId,
                        pollAnswerList = p.tblPollAnswers.Where(a => a.Status == 1).Select(o => new PollAnswerModel()
                        {
                            PollOptionId = o.PollOptionId,
                            PollId = o.PollId,
                            PollOptionText = p.tblPollOptions.Where(po => po.PollOptionId == o.PollId).FirstOrDefault().OptionText
                        }).ToList()
                    }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region ////StrataBoard Owner Section
        public List<PollQuestionModel> GetPollQuestionsForOwner(int pageNo)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    IQueryable<PollQuestionModel> model = context.tblPollQuestions.Where(x => x.Status == 1 && x.StrataBoardId == ClientSessionData.ClientStrataBoardId).Select(p => new PollQuestionModel()
                     {
                         PollId = p.PollId,
                         PollHeader = p.PollHeader,
                         EndDate = p.EndDate,
                         IsAnswered = context.tblPollAnswers.Any(x => x.PollId == p.PollId && x.CreatedBy == ClientSessionData.UserClientId),
                         CreatedBy = p.CreatedBy,
                         CreatedOn = p.CreatedOn,
                         CreatedFromIP = p.CreatedFromIP,
                         ModifiedBy = p.ModifiedBy,
                         ModifiedFromIP = p.ModifiedFromIP,
                         ModifiedOn = p.ModifiedOn,
                         StrataBoardId = p.StrataBoardId,
                         pollOptionList = p.tblPollOptions.Where(to => to.Status == 1).Select(o => new PollOptionModel()
                         {
                             PollOptionId = o.PollOptionId,
                             OptionText = o.OptionText,
                             IsSelected = context.tblPollAnswers.Any(x => x.PollId == p.PollId && x.PollOptionId == o.PollOptionId && x.CreatedBy == ClientSessionData.UserClientId)
                         }).ToList()
                     });

                    var total = model.Count();
                    if (total != 0)
                    {
                        var pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPageSize"]);
                        var skip = pageSize * (pageNo - 1);
                        return model.OrderBy(m => m.IsAnswered).ThenByDescending(t => t.CreatedOn).Skip(skip).Take(pageSize).ToArray().ToList();
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int SubmitOpinion(long optionId, long PollId)
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    tblPollAnswer answer = new tblPollAnswer();
                    answer.PollOptionId = optionId;
                    answer.PollId = PollId;
                    answer.CreatedBy = ClientSessionData.UserClientId;
                    answer.CreatedFromIP = HttpContext.Current.Request.UserHostAddress;
                    answer.CreatedOn = DateTime.UtcNow;
                    answer.Status = 1;
                    context.tblPollAnswers.Add(answer);
                    return context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion


        #region //Common functions
        public List<PollChartModel> GetPollChart(long PollId)
        {
            using (var context = new StratasFairDBEntities())
            {
                return context.USP_GetPollChart(PollId).Select(x => new PollChartModel()
                {
                    PollId = PollId,
                    PollOptionId = x.polloptionid,
                    PollOptionText = x.OptionText,
                    PollCount = x.total == null ? 0 : x.total
                }).ToList();
            }
        }
        #endregion
    }
}
