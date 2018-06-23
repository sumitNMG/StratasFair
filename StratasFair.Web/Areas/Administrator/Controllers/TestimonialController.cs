using StratasFair.Business;
using StratasFair.Business.CommonHelper;
using StratasFair.BusinessEntity;
using StratasFair.Common;
using StratasFair.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StratasFair.Web.Areas.Administrator.Controllers
{
  
        [AdminSessionExpire]
        public class TestimonialController : Controller
        {
            int MaxLength = 5; // in MB
            int MaxImageWidth = 100;
            int MaxImageHeight = 100;

            // GET: Administrator/testimonial
            public ActionResult Index()
            {
                return BindList();
            }

            List<TestimonialModel> ActiveList = new List<TestimonialModel>();
            List<TestimonialModel> InActiveList = new List<TestimonialModel>();

            private ActionResult BindList()
            {
                TestimonialHelper _Helper = new TestimonialHelper();
                TestimonialModel testimonialModel = new TestimonialModel();
                var List = _Helper.GetAll();
                ActiveList = List.Where(x => x.Status == 1).ToList();
                InActiveList = List.Where(x => x.Status == 0).ToList();
                return View(Tuple.Create(ActiveList, InActiveList, testimonialModel));
            }

            [HttpGet]
            public ActionResult Add()
            {
                ViewBag.StatusList = AppLogic.BindDDStatus(1);
                return View();
            }

            [HttpPost]
            [ValidateInput(false)]
            public ActionResult Add(TestimonialModel testimonialModel)
            {

                // create a bucket if not exists
                string createBucket = AwsS3Bucket.CreateABucket();

                if (ModelState.ContainsKey("ImageType")) ModelState["ImageType"].Errors.Clear();
                var imageTypes = new string[]{    
                    "image/png",
                     "image/jpeg",
                    "image/pjpeg"
                };

                int _maxLength = MaxLength * 1024 * 1024;

                if (testimonialModel.ImageType == null || testimonialModel.ImageType.ContentLength == 0)
                {
                    ModelState.AddModelError("ImageType", "Image is required");
                }
                else if (!imageTypes.Contains(testimonialModel.ImageType.ContentType))
                {
                    ModelState.AddModelError("ImageType", "Please choose either a JPG or PNG image.");
                }
                else if (testimonialModel.ImageType.ContentLength > _maxLength)
                {
                    ModelState.AddModelError("ImageType", "Maximum allowed file size is " + MaxLength + " MB.");
                }
                //else if (testimonialModel.ImageType != null && imageTypes.Contains(testimonialModel.ImageType.ContentType) && testimonialModel.ImageType.ContentLength <= _maxLength)
                //{
                //    System.Drawing.Image img = System.Drawing.Image.FromStream(testimonialModel.ImageType.InputStream);
                //    int height = img.Height;
                //    int width = img.Width;

                //    if (width > MaxImageWidth || height > MaxImageHeight)
                //    {
                //        ModelState.AddModelError("ImageType", "Maximum allowed file dimension is " + MaxImageWidth + "*" + MaxImageHeight);
                //    }
                //}


                if (ModelState.IsValid)
                {
                    TestimonialHelper _Helper = new TestimonialHelper();
                    if (testimonialModel.ImageType != null)
                    {
                        string ext = System.IO.Path.GetExtension(testimonialModel.ImageType.FileName);
                        testimonialModel.ActualImageFile = testimonialModel.ImageType.FileName.ToString();
                        testimonialModel.ImageFile = Guid.NewGuid() + ext;

                        int count = _Helper.AddUpdate(testimonialModel);
                        if (count == 0)
                        {
                            string initialPath = "resources/testimonial";

                            // save the file locally
                            string path = Server.MapPath("~/Content/Resources/Testimonial/" + testimonialModel.ImageFile);
                            testimonialModel.ImageType.SaveAs(path);

                            // save the file on s3
                            int fileMapped = AwsS3Bucket.CreateFile(initialPath + "/" + testimonialModel.ImageFile, path);

                            // delete the file locally
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            TempData["CommonMessage"] = AppLogic.setMessage(count, "Record added successfully.");
                            return RedirectToAction("Index");
                        }
                        else if (count == 1)
                        {
                            TempData["CommonMessage"] = AppLogic.setMessage(count, "Record already exists.");
                            ViewBag.StatusList = AppLogic.BindDDStatus(1);
                            return View(testimonialModel);
                        }
                        else
                        {
                            ViewBag.StatusList = AppLogic.BindDDStatus(1);
                            TempData["CommonMessage"] = AppLogic.setMessage(count, "Error, Please try again.");
                            return View(testimonialModel);
                        }
                    }
                }
                ViewBag.StatusList = AppLogic.BindDDStatus(1);
                return View(testimonialModel);
            }

            [HttpGet]
            public ActionResult Edit(int Id)
            {
                TestimonialHelper _Helper = new TestimonialHelper();
                TestimonialModel testimonialModel = new TestimonialModel();
                testimonialModel = _Helper.GetByID(Id);
                ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(testimonialModel.Status));
                return View(testimonialModel);
            }

            [HttpPost]
            [ValidateInput(false)]
            public ActionResult Edit(TestimonialModel testimonialModel)
            {

                var imageTypes = new string[]{    
                     "image/png",
                     "image/jpeg",
                    "image/pjpeg"
                };

                int _maxLength = MaxLength * 1024 * 1024;

                ModelState.Remove("ImageType");
                if (testimonialModel.ImageType != null)
                {
                    if (!imageTypes.Contains(testimonialModel.ImageType.ContentType))
                    {
                        ModelState.AddModelError("ImageType", "Please choose either a JPG or PNG image.");
                    }
                    else if (testimonialModel.ImageType.ContentLength > _maxLength)
                    {
                        ModelState.AddModelError("ImageType", "Maximum allowed file size is " + MaxLength + " MB.");
                    }
                    //else if (imageTypes.Contains(testimonialModel.ImageType.ContentType) && testimonialModel.ImageType.ContentLength <= _maxLength)
                    //{
                    //    System.Drawing.Image img = System.Drawing.Image.FromStream(testimonialModel.ImageType.InputStream);
                    //    int height = img.Height;
                    //    int width = img.Width;

                    //    if (width > MaxImageWidth || height > MaxImageHeight)
                    //    {
                    //        ModelState.AddModelError("ImageType", "Maximum allowed file dimension is " + MaxImageWidth + "*" + MaxImageHeight);
                    //    }
                    //}
                }
              

                if (ModelState.IsValid)
                {
                    TestimonialHelper _helper = new TestimonialHelper();

                    int count = -1;
                    if (testimonialModel.ImageType != null)
                    {
                        testimonialModel.ActualImageFile = testimonialModel.ImageType.FileName.ToString();
                        string ext = System.IO.Path.GetExtension(testimonialModel.ImageType.FileName);


                        testimonialModel.ImageFile = Guid.NewGuid() + ext;
                        string path = Server.MapPath("~/Content/Resources/Testimonial/" + testimonialModel.ImageFile);
                        count = _helper.AddUpdate(testimonialModel);
                        if (count == 0)
                        {
                            // save the file locally
                            testimonialModel.ImageType.SaveAs(path);
                            // save the file on s3
                            int fileMapped = AwsS3Bucket.CreateFile("resources/testimonial/" + testimonialModel.ImageFile, path);
                            // delete the file locally
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }



                            string filePath = Server.MapPath("~/Content/Resources/Testimonial") + "/" + testimonialModel.OldImageFile;
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            // delete the old file from s3
                            AwsS3Bucket.DeleteObject("resources/testimonial/" + testimonialModel.OldImageFile);


                            TempData["CommonMessage"] = AppLogic.setMessage(count, "Record updated successfully.");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        count = _helper.AddUpdate(testimonialModel);
                    }


                    if (count == 0)
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(count, "Record updated successfully.");
                        return RedirectToAction("Index");
                    }
                    else if (count == 1)
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(count, "Record already exists.");
                        ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(testimonialModel.Status));
                        return View(testimonialModel);
                    }
                    else
                    {
                        TempData["CommonMessage"] = AppLogic.setMessage(count, "Error: Please try again.");
                        ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(testimonialModel.Status));
                        return View(testimonialModel);
                    }
                }
                ViewBag.StatusList = AppLogic.BindDDStatus(Convert.ToInt32(testimonialModel.Status));
                return View(testimonialModel);
            }

            public ActionResult Delete(int Id)
            {
                TestimonialHelper _helper = new TestimonialHelper();
                TestimonialModel testimonialModel = new TestimonialModel();
                testimonialModel = _helper.GetByID(Id);

                int count = _helper.Delete(testimonialModel);
                if (count == 0)
                {
                    string filePath = Server.MapPath("~/Content/Resources/Testimonial") + "/" + testimonialModel.ImageFile;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    // delete the file from s3
                    AwsS3Bucket.DeleteObject("resources/testimonial/" + testimonialModel.ImageFile);


                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deleted successfully.");
                }
                else
                {
                    TempData["CommonMessage"] = AppLogic.setMessage(count, "Record deletion failed.");
                }
                return RedirectToAction("Index");
            }

            public ActionResult Active(int Id)
            {
                TestimonialHelper _helper = new TestimonialHelper();
                TestimonialModel testimonialModel = new TestimonialModel();
                testimonialModel.TestimonialId = Id;
                testimonialModel.Status = 1;
                int count = _helper.ActDeact(testimonialModel);
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
                TestimonialHelper _helper = new TestimonialHelper();
                TestimonialModel testimonialModel = new TestimonialModel();
                testimonialModel.TestimonialId = Id;
                testimonialModel.Status = 0;
                int count = _helper.ActDeact(testimonialModel);
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