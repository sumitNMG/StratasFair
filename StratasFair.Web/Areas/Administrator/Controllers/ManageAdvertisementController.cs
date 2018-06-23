using StratasFair.Business;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity.Admin;
using StratasFair.Common;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
     //[AdminSessionExpire]
    public class ManageAdvertisementController : Controller
    {
        int MaxLength = 5; // in MB
        int MaxImageWidth = 300;
        int MaxImageHeight = 250;

        // GET: Administrator/ManageAdvertisement
        public ActionResult Index()
        {
            return BindList();
        }

        List<ManageAdvertisementModel> ActiveList = new List<ManageAdvertisementModel>();
        List<ManageAdvertisementModel> InActiveList = new List<ManageAdvertisementModel>();

        private ActionResult BindList()
        {
            AdvertisementHelper advertisementHelper = new AdvertisementHelper();
            ManageAdvertisementModel advertisementModel = new ManageAdvertisementModel();
            var List = advertisementHelper.GetAll();
            ActiveList = List.Where(x => x.Status == 1).ToList();
            InActiveList = List.Where(x => x.Status == 0).ToList();
            return View(Tuple.Create(ActiveList, InActiveList, advertisementModel));
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            AdvertisementHelper advertisementHelper = new AdvertisementHelper();
            ManageAdvertisementModel advertisementModel = new ManageAdvertisementModel();
            advertisementModel = advertisementHelper.GetByID(Id);
            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(advertisementModel.Status));
            return View(advertisementModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ManageAdvertisementModel advertisementModel)
        {

            var imageTypes = new string[]{    
                     "image/png",
                     "image/jpeg",
                    "image/pjpeg"
                };

            int _maxLength = MaxLength * 1024 * 1024;

            ModelState.Remove("ImageType");
            if (advertisementModel.ImageType != null)
            {
                if (!imageTypes.Contains(advertisementModel.ImageType.ContentType))
                {
                    ModelState.AddModelError("ImageType", "Please choose either a JPG or PNG image.");
                }
                else if (advertisementModel.ImageType.ContentLength > _maxLength)
                {
                    ModelState.AddModelError("ImageType", "Maximum allowed file size is " + MaxLength + " MB.");
                }
                else if (imageTypes.Contains(advertisementModel.ImageType.ContentType) && advertisementModel.ImageType.ContentLength <= _maxLength)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(advertisementModel.ImageType.InputStream);
                    int height = img.Height;
                    int width = img.Width;

                    if (width > MaxImageWidth || height > MaxImageHeight)
                    {
                        ModelState.AddModelError("ImageType", "Maximum allowed file dimension is " + MaxImageWidth + "*" + MaxImageHeight);
                    }
                }
            }


            if (ModelState.IsValid)
            {
                AdvertisementHelper advertisementHelper = new AdvertisementHelper();

                int count = -1;
                if (advertisementModel.ImageType != null)
                {
                    advertisementModel.ActualImageFile = advertisementModel.ImageType.FileName.ToString();
                    string ext = System.IO.Path.GetExtension(advertisementModel.ImageType.FileName);


                    advertisementModel.ImageFile = Guid.NewGuid() + ext;
                    string path = Server.MapPath("~/Content/Resources/Advertisement/" + advertisementModel.ImageFile);
                    count = advertisementHelper.AddUpdate(advertisementModel);
                    if (count == 0)
                    {
                        // save the file locally
                        advertisementModel.ImageType.SaveAs(path);
                        // save the file on s3
                        int fileMapped = AwsS3Bucket.CreateFile("resources/advertisement/" + advertisementModel.ImageFile, path);
                        // delete the file locally
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }



                        string filePath = Server.MapPath("~/Content/Resources/Advertisement") + "/" + advertisementModel.OldImageFile;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        // delete the old file from s3
                        AwsS3Bucket.DeleteObject("resources/advertisement/" + advertisementModel.OldImageFile);


                        TempData["CommonMessage"] = AppLogic.setMessage(count, "Record updated successfully.");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    count = advertisementHelper.AddUpdate(advertisementModel);
                }


                if (count == 0)
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Record updated successfully.");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Error: Please try again.");
                    ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(advertisementModel.Status));
                    return View(advertisementHelper);
                }
            }

            ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(advertisementModel.Status));
            return View(advertisementModel);
        }


        public ActionResult Active(int Id)
        {
            AdvertisementHelper advertisementHelper = new AdvertisementHelper();
            ManageAdvertisementModel advertisementModel = new ManageAdvertisementModel();
            advertisementModel.AdvertisementId = Id;
            advertisementModel.Status = 1;
            int count = advertisementHelper.ActDeact(advertisementModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record activated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record activation failed.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Deactive(int Id)
        {
            AdvertisementHelper advertisementHelper = new AdvertisementHelper();
            ManageAdvertisementModel advertisementModel = new ManageAdvertisementModel();
            advertisementModel.AdvertisementId = Id;
            advertisementModel.Status = 0;
            int count = advertisementHelper.ActDeact(advertisementModel);
            if (count == 0)
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deactivated successfully.");
            }
            else
            {
                TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deactivation failed.");
            }
            return RedirectToAction("Index");

        }
    }
}