using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace KYHBPA_TeamA.Controllers
{
    public class PhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Photo
        /// <summary>
        /// Pagination for indexing the photos
        /// </summary>
        /// <param name="sortOrder">Viewbag filter</param>
        /// <param name="currentFilter">Viewbag existing filter</param>
        /// <param name="searchString">Filter</param>
        /// <param name="page">Current page</param>
        /// <returns>View with index list</returns>
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Title = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var photoViewModels = db.Photos.Select(p => new DisplayPhotosViewModel()
            {
                Id = p.PhotoID,
                Description = p.PhotoDesc,
                Title = p.PhotoTitle,
                Date = p.TimeStamp,
                InPhotoGallery = p.InPhotoGallery,
                Link = p.Link
            }).Where(x => x.InPhotoGallery);

            if (!String.IsNullOrEmpty(searchString))
            {
                photoViewModels = photoViewModels.Where(p => p.Title.Contains(searchString)
                                       || p.Description.Contains(searchString));
            }

            if (photoViewModels.Count() > 0)
            {
                switch (sortOrder)
                {
                    case "title_desc":
                        photoViewModels = photoViewModels.OrderByDescending(s => s.Title);
                        break;
                    case "Date":
                        photoViewModels = photoViewModels.OrderBy(s => s.Date);
                        break;
                    case "date_desc":
                        photoViewModels = photoViewModels.OrderByDescending(s => s.Date);
                        break;
                    default:  // Title ascending 
                        photoViewModels = photoViewModels.OrderBy(s => s.Title);
                        break;
                }

                // new ordered list for masonry
                var newList = new List<DisplayPhotosViewModel>();

                // Order for masonry
                var photoViewModelsList = photoViewModels.OrderBy(x => x.Description.Length).ToList();

                int lastPhotoIterator = photoViewModels.Count() - 1;

                
                for(int i = 0; i < photoViewModelsList.Count / 2; i++)
                {

                    var smallerElement = photoViewModelsList.ElementAt(i);
                    var largerElement = photoViewModelsList.ElementAt(lastPhotoIterator);
                    lastPhotoIterator--;

                    if(photoViewModelsList.Count % 2 != 0)
                    {
                        var num = (photoViewModelsList.Count / 2) - 1;
                        if (i == num)
                        {
                            var additionalElement = photoViewModelsList.ElementAt(i + 1);
                            newList.Add(additionalElement);
                        }
                    }

                    newList.Add(smallerElement);
                    newList.Add(largerElement);
                }

                int pageSize = 9;
                int pageNumber = (page ?? 1);

                if (newList.Count != 0)
                {

                    return View(newList.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(photoViewModelsList.ToPagedList(pageNumber, pageSize));
                }


            }
            else
            {
                return View();
            }

        }

        // GET: Photo/Details/5
        public ActionResult Details(int id)
        {
            var photo = db.Photos.Find(id);
            var newPhoto = new DisplayPhotosViewModel()
            {
                Id = photo.PhotoID,
                Description = photo.PhotoDesc,
                Title = photo.PhotoTitle,
                Date = photo.TimeStamp
            };

            return View(newPhoto);
        }

        // GET: Photo/Admin
        [Authorize(Roles ="Admin")]
        public ActionResult Admin(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Title = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var photoViewModels = db.Photos.Select(p => new DisplayPhotosViewModel()
            {
                Id = p.PhotoID,
                Description = p.PhotoDesc,
                ShorterDescription = p.PhotoDesc.Substring(0,150) + "...",
                Title = p.PhotoTitle,
                Date = p.TimeStamp,
                InLandingPageCarousel = p.InLandingPageCarousel,
                InPartnerOrgCarousel = p.InPartnerOrgCarousel,
                InPhotoGallery = p.InPhotoGallery,
                Credit = p.Credit
            });

            if (!String.IsNullOrEmpty(searchString))
            {
                photoViewModels = photoViewModels.Where(p => p.Title.Contains(searchString)
                                       || p.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    photoViewModels = photoViewModels.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    photoViewModels = photoViewModels.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    photoViewModels = photoViewModels.OrderByDescending(s => s.Date);
                    break;
                default:  // Title ascending 
                    photoViewModels = photoViewModels.OrderBy(s => s.Title);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(photoViewModels.ToPagedList(pageNumber, pageSize));
        }


        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photo/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(AddPhotoViewModel addViewModel, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if(image != null)
                {
                    var photo = new Photo()
                    {
                        TimeStamp = DateTime.Now,
                        PhotoDesc = addViewModel.Description,
                        PhotoData = new byte[image.ContentLength],
                        PhotoTitle = addViewModel.Title,
                        Link = addViewModel.Link,
                        InLandingPageCarousel = addViewModel.InLandingPageCarousel,
                        InPartnerOrgCarousel = addViewModel.InPartnerOrgCarousel,
                        InPhotoGallery = addViewModel.InPhotoGallery,
                        Credit = addViewModel.Credit,
                        MimeType = image.ContentType
                    };
                    image.InputStream.Read(photo.PhotoData, 0, image.ContentLength);

                    var imageToResize = Image.FromStream(image.InputStream);

                    photo.ThumbnailPhotoContent = GetImageThumbnail(imageToResize); 

                    db.Photos.Add(photo);
                    db.SaveChanges();

                    return RedirectToAction("Admin");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        // GET: Photo/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var photo = db.Photos.Find(id);
            var vm = new EditPhotoViewModel()
            {
                Id = photo.PhotoID,
                Date = photo.TimeStamp,
                Description = photo.PhotoDesc,
                Title = photo.PhotoTitle,
                InLandingPageCarousel = photo.InLandingPageCarousel,
                InPartnerOrgCarousel = photo.InPartnerOrgCarousel,
                InPhotoGallery = photo.InPhotoGallery,
                Link = photo.Link,
                Credit = photo.Credit
            };

            if (photo == null)
                return HttpNotFound();

            return View(vm);
        }

        // POST: Photo/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(EditPhotoViewModel photoVM, FormCollection collection, HttpPostedFileBase image = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var photoToUpdate = db.Photos.FirstOrDefault(x => x.PhotoID == photoVM.Id);
                    if (photoToUpdate != null)
                    {
                        photoToUpdate.PhotoDesc = photoVM.Description;
                        photoToUpdate.InLandingPageCarousel = photoVM.InLandingPageCarousel;
                        photoToUpdate.InPartnerOrgCarousel = photoVM.InPartnerOrgCarousel;
                        photoToUpdate.InPhotoGallery = photoVM.InPhotoGallery;
                        photoToUpdate.PhotoTitle = photoVM.Title;
                        photoToUpdate.Link = photoVM.Link;
                        photoToUpdate.Credit = photoVM.Credit;
                    }

                    if(image != null)
                    {
                        byte[] uploadedImage = new byte[image.InputStream.Length];
                        image.InputStream.Read(uploadedImage, 0, image.ContentLength);
                        photoToUpdate.PhotoData = uploadedImage;
                        photoToUpdate.MimeType = image.ContentType;

                        var imageToResize = Image.FromStream(image.InputStream);

                        photoToUpdate.ThumbnailPhotoContent = GetImageThumbnail(imageToResize);
                    }

                    db.Entry(photoToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = string.Format($"{photoVM.Title} photo has been updated!");
                    return RedirectToAction("Admin");
                }

                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }

        // GET: Photo/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var photo = db.Photos.Find(id);
            var vm = new DisplayPhotosViewModel()
            {
                Id = photo.PhotoID,
                Date = photo.TimeStamp,
                Description = photo.PhotoDesc,
                Title = photo.PhotoTitle,
                Data = photo.PhotoData
            };

            if (photo == null)
                return HttpNotFound();

            return View(vm);
        }

        // POST: Photo/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var photo = db.Photos.Find(id);
                db.Photos.Remove(photo);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Returns photo from database if it is found
        /// </summary>
        /// <param name="id">ID of photo to get</param>
        /// <returns>File of image to render</returns>
        [OutputCache(Duration = 86400, Location = OutputCacheLocation.Client)]
        public ActionResult GetPhoto(int? id)
        {
            if (id == null)
                return new HttpNotFoundResult();

            Photo photoToGet = db.Photos.Find(id);

            if (photoToGet != null)
                return File(photoToGet.PhotoData, photoToGet.MimeType);
            else
                return new HttpNotFoundResult();
        }

        /// <summary>
        /// Returns thumbnail of photo
        /// </summary>
        /// <param name="id">Id of the photo</param>
        /// <returns>Returns file</returns>
        [OutputCache(Duration = 86400, Location = OutputCacheLocation.Client)]
        public ActionResult GetThumbnail(int? id)
        {
            if (id == null)
                return new HttpNotFoundResult();

            Photo photoToGet = db.Photos.Find(id);

            if (photoToGet != null && photoToGet.ThumbnailPhotoContent != null) 
                return File(photoToGet.ThumbnailPhotoContent, photoToGet.MimeType);
            else
                return new HttpNotFoundResult();
        }

        /// <summary>
        /// Gets photos for gallery
        /// </summary>
        /// <returns></returns>
        public ActionResult _ImageGallery()
        {
            var vm = new PhotoGalleryViewModel
            {
                Photos = new List<DisplayPhotosViewModel>()
            };
            var photos = db.Photos.Where(x => x.InLandingPageCarousel == true);

            foreach (var i in photos)
            {
                var photoToAdd = new DisplayPhotosViewModel()
                {
                    Id = i.PhotoID,
                    Data = i.PhotoData,
                    Description = i.PhotoDesc,
                    Title = i.PhotoTitle,
                    Credit = i.Credit
                };
                vm.Photos.Add(photoToAdd);
            }

            return View(vm);
        }

        public ActionResult _PartnerOrgs()
        {
            int NUM_ITEMS_IN_SLIDE = 4;

            var vm = new PartnerOrgSlides
            {
                Slides = new List<PartnerOrgSlide>()
            };


            var photos = db.Photos.Where(x => x.InPartnerOrgCarousel == true).OrderBy(x => x.PhotoTitle);
            var totalNumOfPartners = db.Photos.Where(x => x.InPartnerOrgCarousel == true).Count();
            var numOfSlides = Math.Ceiling((double)totalNumOfPartners / NUM_ITEMS_IN_SLIDE);

            for(int i = 0; i < numOfSlides; i++)
            {
                PartnerOrgSlide slide = new PartnerOrgSlide();
                int photoCounter = i * NUM_ITEMS_IN_SLIDE;
                var slidePhotos = photos.Skip(photoCounter).Take(NUM_ITEMS_IN_SLIDE);

                foreach(var photo in slidePhotos)
                {
                    var partnerToAdd = new DisplayPartnerOrgViewModel()
                    {
                        Id = photo.PhotoID,
                        Data = photo.PhotoData,
                        Description = photo.PhotoDesc,
                        Title = photo.PhotoTitle,
                        Link = photo.Link
                    };
                    slide.PartnerPhotos.Add(partnerToAdd);
                }
                vm.Slides.Add(slide);
            }
          
            return View(vm);
        }
         
        public byte[] GetImageThumbnail(Image image)
        {
            byte[] returnArray;
            
            var thumbSize = new Size()
            {
                Width = 500,
                Height = 500
            };

            int sourceWidth = image.Width;
            int sourceHeight = image.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)thumbSize.Width / (float)sourceWidth);
            nPercentH = ((float)thumbSize.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.CompositingMode = CompositingMode.SourceCopy;

            g.DrawImage(image, 0, 0, destWidth, destHeight);
            g.Dispose();

            using (MemoryStream ms = new MemoryStream())
            {
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
                b.Save(ms, getEncoderInfo("image/jpeg"), encoderParameters);

                //save the stream as byte array
                returnArray = ms.ToArray();
            }


            return returnArray;
        }

        private static ImageCodecInfo getEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType.ToLower() == mimeType.ToLower())
                    return encoders[j];
            }
            return null;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public ActionResult UpdateAllImagesWithoutThumbnails()
        {
            var imagesToUpdate = db.Photos.Where(x => x.ThumbnailPhotoContent == null && x.InPartnerOrgCarousel == false);

            foreach(var image in imagesToUpdate)
            {
                if(image.PhotoData != null)
                {
                    Image newImage = (Image)new ImageConverter().ConvertFrom(image.PhotoData);
                    var resizedImage = GetImageThumbnail(newImage);
                    image.ThumbnailPhotoContent = resizedImage;
                }
            }

            if(imagesToUpdate != null)
            {
                db.SaveChanges();

            }

            TempData["message"] = "Thumbnails successfully updated";
            return RedirectToAction("Admin");
        }
    }
}
