﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KYHBPA_TeamA.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.UI;

namespace KYHBPA_TeamA.Controllers
{
    public class BoardOfDirectorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BoardOfDirectors
        public ActionResult Index()
        {
            var BoardOfDirectors = db.BoardOfDirectors.Select(p => new DisplayBoardOfDirectorsViewModel()
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Title = p.Title,
                Email = p.Email,
                Description = p.Description,
                Rank = 1
            }).ToList();

            var newOrderedList = BoardOfDirectors.OrderBy(x => x.Rank);

            return View(newOrderedList);
        }

        // GET: BoardOfDirectors/Details/5
        public ActionResult Details(int id)
        {
            var boardOfDirectors = db.BoardOfDirectors.Find(id);
            var newBoardOfDirectors = new DisplayBoardOfDirectorsViewModel()
            {
                Id = boardOfDirectors.Id,
                FirstName = boardOfDirectors.FirstName,
                LastName = boardOfDirectors.LastName,
                Title = boardOfDirectors.Title,
                Email = boardOfDirectors.Email,
                Description = boardOfDirectors.Description,
            };

            return View(newBoardOfDirectors);
        }
        // GET: BoardOfDirectors/Admin
        [Authorize(Roles ="Admin")]
        public ActionResult Admin()
        {
            var BoardOfDirectors = db.BoardOfDirectors.Select(p => new DisplayBoardOfDirectorsViewModel()
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Title = p.Title,
                Email = p.Email,
                Description = p.Description,
            });

            return View(BoardOfDirectors);
        }

        // GET: BoardOfDirectors/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BoardOfDirectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBODMemberViewModel addViewModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var boardOfDirector = new BoardOfDirectors()
                {
                    Title = addViewModel.Title,
                    FirstName = addViewModel.FirstName,
                    LastName = addViewModel.LastName,
                    Email = addViewModel.Email,
                    Description = addViewModel.Description,
                };

                //byte[] uploadedFile = new byte[file.InputStream.Length];
                //addViewModel.File.InputStream.Read(uploadedFile, 0, file.ContentLength);
                if (file != null)
                {
                    var imageBeforeResize = Image.FromStream(file.InputStream);
                    var imageAfterResize = ResizeImage(imageBeforeResize, 400, 500);

                    var resizedByteArray = ImageToByte(imageAfterResize);

                    boardOfDirector.PhotoContent = resizedByteArray;
                    boardOfDirector.MimeType = file.ContentType;

                    db.BoardOfDirectors.Add(boardOfDirector);
                    db.SaveChanges();

                    return RedirectToAction("Admin");
                }
                else
                {
                    db.BoardOfDirectors.Add(boardOfDirector);
                    db.SaveChanges();

                    return RedirectToAction("Admin");
                }

            }
            else
            {
                return View();
            }
        }

        // GET: BoardOfDirectors/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var boardOfDirectors = db.BoardOfDirectors.Find(id);
            var vm = new EditBODMemberViewModel()
            {
                ID = boardOfDirectors.Id,
                FirstName = boardOfDirectors.FirstName,
                LastName = boardOfDirectors.LastName,
                Title = boardOfDirectors.Title,
                Email = boardOfDirectors.Email,
                PhotoContent = boardOfDirectors.PhotoContent,
                Description = boardOfDirectors.Description
            };

            if (boardOfDirectors == null)
                return HttpNotFound();

            return View(vm);
        }
        // POST: BoardOfDirectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBODMemberViewModel BODVM, FormCollection collection, HttpPostedFileBase file = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var BODToUpdate = db.BoardOfDirectors.FirstOrDefault(x => x.Id == BODVM.ID);
                    if (BODToUpdate != null)
                    {
                        BODToUpdate.FirstName = BODVM.FirstName;
                        BODToUpdate.LastName = BODVM.LastName;
                        BODToUpdate.Title = BODVM.Title;
                        BODToUpdate.Description = BODVM.Description;
                        BODToUpdate.Email = BODVM.Email;

                        if(file != null)
                        {
                            var imageBeforeResize = Image.FromStream(file.InputStream);
                            var imageAfterResize = ResizeImage(imageBeforeResize, 400, 500);

                            var resizedByteArray = ImageToByte(imageAfterResize);

                            BODToUpdate.PhotoContent = resizedByteArray;
                            BODToUpdate.MimeType = file.ContentType;
                        }
                    }
                                        

                    db.Entry(BODToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = string.Format($"{BODVM.FirstName} {BODVM.LastName} has been updated!");
                }
                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }

        // GET: BoardOfDirectors/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var boardOfDirectors = db.BoardOfDirectors.Find(id);
            var vm = new DisplayBoardOfDirectorsViewModel()
            {
                Id = boardOfDirectors.Id,
                FirstName = boardOfDirectors.FirstName,
                LastName = boardOfDirectors.LastName,
                Title = boardOfDirectors.Title,
                Email = boardOfDirectors.Email,
                Description = boardOfDirectors.Description,
                PhotoContent = boardOfDirectors.PhotoContent
            };

            if (boardOfDirectors == null)
                return HttpNotFound();

            return View(vm);
        }
        // POST: BoardOfDirectors/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var boardOfDirectors = db.BoardOfDirectors.Find(id);
                db.BoardOfDirectors.Remove(boardOfDirectors);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }


        /// <summary>
        /// Gets an image associated with BoD member
        /// </summary>
        /// <param name="id">Id of the board member</param>
        /// <returns>File Result of Image</returns>
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult GetBoDImage(int? id)
        {
            var boardOfDirector = new BoardOfDirectors();

            if(id == null)
            {
                return new HttpNotFoundResult();
            }

            boardOfDirector = db.BoardOfDirectors.Find(id);

            if (boardOfDirector.MimeType != null && boardOfDirector.PhotoContent != null)
                return File(boardOfDirector.PhotoContent, boardOfDirector.MimeType);
            else
            {
                return new FilePathResult(HttpContext.Server.MapPath("~/Content/blankProfileImage.jpeg"), "image/jpeg");
            }
        }


        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
