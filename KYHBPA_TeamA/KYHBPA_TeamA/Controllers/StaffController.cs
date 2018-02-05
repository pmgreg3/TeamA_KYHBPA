using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYHBPA_TeamA.Controllers
{
    public class StaffController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // Admin of Staff
        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            var staffMembers = _db.Staff.Select(m => new StaffViewModels()
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Title = m.Title,
                Email = m.Email,
                Description = m.Description
            });

            return View(staffMembers);
        }

        // GET: Staff
        public ActionResult Index()
        {
            var staffMembers = _db.Staff.Select(m => new StaffViewModels()
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Title = m.Title,
                Email = m.Email,
                Description = m.Description
            });

            return View(staffMembers);
        }

        // GET: Staff/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Staff/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Staff/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Staff/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Staff/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
