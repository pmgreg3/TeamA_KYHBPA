using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KYHBPA_TeamA.Models;

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
                PhotoContent = p.PhotoContent
            });

            return View(BoardOfDirectors);
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
                PhotoContent = boardOfDirectors.PhotoContent
            };

            return View(newBoardOfDirectors);
        }

        // GET: BoardOfDirectors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BoardOfDirectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddBoardofDirectorsViewModel addViewModel, HttpPostedFileBase bod = null)
        {
            if (ModelState.IsValid)
            {
                var boardOfDirectors = new BoardOfDirectors()
                {
                    Title = addViewModel.Title,
                    PhotoContent = new byte[bod.ContentLength],
   
                };
                bod.InputStream.Read(boardOfDirectors.PhotoContent, 0, bod.ContentLength);
                db.BoardOfDirectors.Add(boardOfDirectors);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: BoardOfDirectors/Edit/5
        public ActionResult Edit(int? id)
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
        // POST: BoardOfDirectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DisplayBoardOfDirectorsViewModel BODVM, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var BODToUpdate = db.BoardOfDirectors.FirstOrDefault(x => x.Id == BODVM.Id);
                    if (BODToUpdate != null)
                    {
                        BODToUpdate.Description = BODVM.Description;
                        BODToUpdate.FirstName = BODVM.FirstName;
                        BODToUpdate.LastName = BODVM.LastName;
                        BODToUpdate.Title = BODVM.Title;
                        BODToUpdate.Email = BODVM.Email;
                    }

                    db.Entry(BODToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = string.Format($"{BODVM.FirstName} {BODVM.LastName} has been updated!");
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BoardOfDirectors/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var boardOfDirectors = db.BoardOfDirectors.Find(id);
                db.BoardOfDirectors.Remove(boardOfDirectors);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Returns BOD member from database if it is found
        /// </summary>
        /// <param name="id">ID of BOD member to get</param>
        /// <returns>File of BOD member to render</returns>
        public FileContentResult GetBODMember(int id)
        {
            BoardOfDirectors BODToGet = db.BoardOfDirectors.Find(id);

            if (BODToGet != null)
                return File(BODToGet.PhotoContent, BODToGet.Description);
            else
                return null;
        }

    }
}
