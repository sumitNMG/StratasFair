using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StratasFair.BusinessEntity;
using StratasFair.BusinessEntity.Front;
using StratasFair.Data;
using StratasFair.Business.CommonHelper;

namespace StratasFair.Business.Front
{
    public sealed class HomePageHelper
    {
        private HomePageHelper() { }
        private static readonly Lazy<HomePageHelper> lazy = new Lazy<HomePageHelper>(() => new HomePageHelper());
        public static HomePageHelper Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public string GetAboutUs()
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    return context.tblCMSMasters.Where(x => x.PageName.ToLower() == "about us").FirstOrDefault().Content;
                }
            }
            catch
            {
                return "";
            }
        }

        public List<AdvertisementModel> GetAdvertisement()
        {
            try
            {
                using (StratasFairDBEntities context = new StratasFairDBEntities())
                {
                    return context.tblAdvertisements.Where(x => x.Status == 1).Select(a => new AdvertisementModel()
                    {
                        AdvertisementId = a.AdvertisementId,
                        ImageFile = a.ImageFile,
                        RedirectionUrl = a.RedirectionUrl,
                        DisplayOrder = a.DisplayOrder,
                        Status = a.Status
                    }).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<FAQModel> GetFAQs()
        {
            StratasFairDBEntities _context = new StratasFairDBEntities();
            return _context.tblFAQs.Where(x => x.Status == 1).OrderBy(f => f.SortOrder).Select(f => new FAQModel()
            {
                FAQId = f.FAQId,
                Answer = f.Answer,
                Question = f.Question
            }).ToList();
        }

        public List<TestimonialModel> GetTestimonials()
        {
            StratasFairDBEntities _context = new StratasFairDBEntities();
            return _context.tblTestimonials.Where(x => x.Status == 1).OrderBy(o => o.DisplayOrder).Select(f => new TestimonialModel()
            {
                TestimonialId = f.TestimonialId,
                AuthorName = f.AuthorName,
                Designation = f.Designation,
                Description = f.Description,
                ImageFile = f.ImageFile
            }).ToList();
        }


        public int SaveMessage(ContactUsModel model)
        {
            try
            {
                StratasFairDBEntities _context = new StratasFairDBEntities();
                tblContactU contactUs = new tblContactU();
                contactUs.CreatedOn = DateTime.UtcNow;
                contactUs.CreatedFromIP = System.Web.HttpContext.Current.Request.UserHostAddress;
                contactUs.FirstName = model.FirstName;
                contactUs.EmailAddress = model.Email;
                contactUs.Message = model.Message;
                contactUs.Status = 1;
                _context.tblContactUs.Add(contactUs);
                if (_context.SaveChanges() > 0)
                {
                    EmailSender.FncSend_ContactUsMail_ToAdmin(model.FirstName, model.Email, model.Message);
                }
                return 1;
            }
            catch
            {
                return -1;
            }
        }
    }
}
