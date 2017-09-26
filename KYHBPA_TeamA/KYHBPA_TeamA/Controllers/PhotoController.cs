﻿using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KYHBPA_TeamA.Controllers
{
    public class PhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Photo
        public ActionResult Index()
        {
            var photos = db.Photos.Select(p => new DisplayPhotosViewModel()
            {
                Id = p.PhotoID,
                Data = p.PhotoData,
                Description = p.PhotoDesc,
                Title = p.PhotoTitle,
                Date = p.TimeStamp
            });

            return View(photos);
        }

        // GET: Photo/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

        // POST: Photo/Edit/5
        [HttpPost]
        public ActionResult Edit(DisplayPhotosViewModel photoVM, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var photoToUpdate = db.Photos.FirstOrDefault(x => x.PhotoID == photoVM.Id);
                    if (photoToUpdate != null)
                    {
                        photoToUpdate.PhotoDesc = photoVM.Description;
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
            if(id == null)
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

    }
}
