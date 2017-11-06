using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

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
                Data = p.PhotoData,
                Description = p.PhotoDesc,
                Title = p.PhotoTitle,
                Date = p.TimeStamp,
                InPhotoGallery = p.InPhotoGallery,
                IsPartnerOrg = p.IsPartnerOrg

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


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(photoViewModels.ToPagedList(pageNumber, pageSize));
        }

        // GET: Photo/Details/5
        public ActionResult Details(int id)
        {
            var photo = db.Photos.Find(id);
            var newPhoto = new DisplayPhotosViewModel()
            {
                Id = photo.PhotoID,
                Data = photo.PhotoData,
                Description = photo.PhotoDesc,
                Title = photo.PhotoTitle,
                Date = photo.TimeStamp
            };

            return View(newPhoto);
        }


        // GET
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photo/Create
        [HttpPost]
        public ActionResult Create(AddPhotoViewModel addViewModel, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var photo = new Photo()
                {
                    TimeStamp = DateTime.Now,
                    PhotoDesc = addViewModel.Description,
                    PhotoData = new byte[image.ContentLength],
                    PhotoTitle = addViewModel.Title,
                    MimeType = image.ContentType
                };
                image.InputStream.Read(photo.PhotoData, 0, image.ContentLength);
                db.Photos.Add(photo);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: Photo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var photo = db.Photos.Find(id);
            var vm = new EditPhotosViewModel()
            {
                Id = photo.PhotoID,
                Date = photo.TimeStamp,
                Description = photo.PhotoDesc,
                Title = photo.PhotoTitle,
                Data = photo.PhotoData,
                InPhotoGallery = photo.InPhotoGallery,
                IsPartnerOrg = photo.IsPartnerOrg
            };

            if (photo == null)
                return HttpNotFound();

            return View(vm);
        }

        // POST: Photo/Edit/5
        [HttpPost]
        public ActionResult Edit(EditPhotosViewModel photoVM, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var photoToUpdate = db.Photos.FirstOrDefault(x => x.PhotoID == photoVM.Id);
                    if (photoToUpdate != null)
                    {
                        photoToUpdate.PhotoDesc = photoVM.Description;
                        photoToUpdate.InPhotoGallery = photoVM.InPhotoGallery;
                        photoToUpdate.IsPartnerOrg = photoVM.IsPartnerOrg;
                        photoToUpdate.PhotoTitle = photoVM.Title;
                    }

                    db.Entry(photoToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = string.Format($"{photoVM.Title} photo has been updated!");
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Photo/Delete/5
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
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var photo = db.Photos.Find(id);
                db.Photos.Remove(photo);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public FileContentResult GetPhoto(int id)
        {
            Photo photoToGet = db.Photos.Find(id);

            if (photoToGet != null)
                return File(photoToGet.PhotoData, photoToGet.MimeType);
            else
                return null;
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
            var photos = db.Photos.Where(x => x.InPhotoGallery == true);

            foreach (var i in photos)
            {
                var photoToAdd = new DisplayPhotosViewModel()
                {
                    Id = i.PhotoID,
                    Data = i.PhotoData,
                    Description = i.PhotoDesc,
                    Title = i.PhotoTitle
                };
                vm.Photos.Add(photoToAdd);
            }

            return View(vm);
        }

        public ActionResult _PartnerOrgs()
        {
            var vm = new PartnerOrgViewModel
            {
                Partners = new List<DisplayPartnerOrgViewModel>()
            };

            var photos = db.Photos.Where(x => x.IsPartnerOrg == true);

            foreach (var i in photos)
            {
                var partnerToAdd = new DisplayPartnerOrgViewModel()
                {
                    Id = i.PhotoID,
                    Data = i.PhotoData,
                    Description = i.PhotoDesc,
                    Title = i.PhotoTitle
                };
                vm.Partners.Add(partnerToAdd);
            }
            return View(vm);
        }
    }
}
