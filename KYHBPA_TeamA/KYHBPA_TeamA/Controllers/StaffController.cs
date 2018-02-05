using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

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

        // GET: Staff/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CreateStaffViewModel staff, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var memberToAdd = new Staff()
                    {
                        Title = staff.Title,
                        FirstName = staff.FirstName,
                        LastName = staff.LastName,
                        Email = staff.Email,
                        Description = staff.Description
                    };

                    if( file != null)
                    {
                        var imageBeforeResize = Image.FromStream(file.InputStream);
                        var imageAfterResize = ResizeImage(imageBeforeResize, 400, 500);

                        var resizedByteArray = ImageToByte(imageAfterResize);
                        memberToAdd.PhotoContent = resizedByteArray;
                        memberToAdd.MimeType = file.ContentType;
                    }

                    _db.Staff.Add(memberToAdd);
                    _db.SaveChanges();
                }

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
            var memberToEdit = _db.Staff.Find(id);

            if (memberToEdit != null)
            {
                var vm = new StaffViewModels()
                {
                    Id = memberToEdit.Id,
                    FirstName = memberToEdit.FirstName,
                    LastName = memberToEdit.LastName,
                    Title = memberToEdit.Title,
                    Email = memberToEdit.Email
                };

                return View(vm);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(StaffViewModels staffMemberToEdit, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldStaffMember = _db.Staff.Find(staffMemberToEdit.Id);

                    if (oldStaffMember != null)
                    {
                        oldStaffMember.Title = staffMemberToEdit.Title;
                        oldStaffMember.FirstName = staffMemberToEdit.FirstName;
                        oldStaffMember.LastName = staffMemberToEdit.LastName;
                        oldStaffMember.Description = staffMemberToEdit.Description;
                        oldStaffMember.Email = staffMemberToEdit.Email;
                        
                        if(file != null)
                        {
                            var imageBeforeResize = Image.FromStream(file.InputStream);
                            var imageAfterResize = ResizeImage(imageBeforeResize, 400, 500);

                            var resizedByteArray = ImageToByte(imageAfterResize);

                            oldStaffMember.PhotoContent = resizedByteArray;
                            oldStaffMember.MimeType = file.ContentType;
                        }

                        _db.Entry(oldStaffMember).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                        TempData["message"] = string.Format($"{staffMemberToEdit.FirstName} {staffMemberToEdit.FirstName} has been updated!");
                    }
                }
                return RedirectToAction("Admin");
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
            var memberToDelete = _db.Staff.Find(id);

            if (memberToDelete != null)
            {
                var vm = new StaffViewModels()
                {
                    Id = memberToDelete.Id,
                    FirstName = memberToDelete.FirstName,
                    LastName = memberToDelete.LastName,
                    Title = memberToDelete.Title,
                    Email = memberToDelete.Email
                };

                return View(vm);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Staff/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var memberToDelete = _db.Staff.Find(id);

                if (memberToDelete != null)
                {
                    _db.Staff.Remove(memberToDelete);
                    _db.SaveChanges();
                }
                else
                {
                    return HttpNotFound();
                }

                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }

        // TODO: Can improve code reduce.  Same methods found in BoD Controller
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

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.ServerAndClient)]
        public FileResult GetStaffMemberImage(int id)
        {
            var staffMember = _db.Staff.Find(id);

            if (staffMember.MimeType != null && staffMember.PhotoContent != null)
                return File(staffMember.PhotoContent, staffMember.MimeType);
            else
            {
                return new FilePathResult(HttpContext.Server.MapPath("~/Content/blankProfileImage.jpeg"), "image/jpeg");
            }
        }
    }
}
