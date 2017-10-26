using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KYHBPA_TeamA.Models;
using Microsoft.AspNet.Identity;

namespace KYHBPA_TeamA.Controllers
{
    public class MembershipsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Memberships
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Memberships.ToList());
        }

        // GET: Memberships/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membership membership = db.Memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            return View(membership);
        }

        // GET: Memberships/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Memberships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MembershipsViewModels.CreateMembershipViewModel viewModel)
        {
            //db.Users.Find(User.Identity.GetUserId()).Membership.ID == null
            //if (ModelState.IsValid)

            //{
            //    if(db.Users.Find(User.Identity.GetUserId()).Membership == null)
            //    {
            //        db.Users.Find(User.Identity.GetUserId()).Membership = membership;
            //        db.Memberships.Add(membership);
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //    else
            //    {
            //        return RedirectToAction("MembershipError");
            //    }
            //}
            //return View(membership);

            if (ModelState.IsValid)
            {
                if (db.Users.Find(User.Identity.GetUserId()).AppliedForMembership == false)
                {
                    var membership = new Membership()
                    {
                        DateofBirth = viewModel.DateofBirth,
                        PhoneNumber = viewModel.PhoneNumber,
                        Address = viewModel.Address,
                        City = viewModel.City,
                        State = viewModel.State,
                        ZipCode = viewModel.ZipCode,
                        LicenseNumber = viewModel.LicenseNumber,
                        IsOwner = viewModel.IsOwner,
                        IsTrainer = viewModel.IsTrainer,
                        Affiliation = viewModel.Affiliation,
                        ManagingPartner = viewModel.ManagingPartner,
                        AgreedToTerms = viewModel.AgreedToTerms,
                        Signature = viewModel.Signature,
                        MembershipEnrollment = DateTime.Now
                    };

                    if (membership.IsOwner == true && membership.IsTrainer == true)
                    {
                        membership.IsOwnerAndTrainer = true;
                    }

                    db.Users.Find(User.Identity.GetUserId()).AppliedForMembership = true;
                    db.Users.Find(User.Identity.GetUserId()).Membership = membership;
                    db.Memberships.Add(membership);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    return RedirectToAction("MembershipError");
                }
            }
            else
                return View();
        }

        public ActionResult MembershipError()
        {
            return View();
        }
    

        // GET: Memberships/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membership membership = db.Memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            return View(membership);
        }

        // POST: Memberships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateofBirth,MembershipEnrollment,PhoneNumber,Address,City,State,ZipCode,LicenseNumber,IsOwner,IsTrainer,IsOwnerAndTrainer,AgreedToTerms,Signature,Affiliation,ManagingPartner")] Membership membership)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(membership);
        }

        // GET: Memberships/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membership membership = db.Memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            return View(membership);
        }

        // POST: Memberships/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Membership membership = db.Memberships.Find(id);
            db.Memberships.Remove(membership);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
