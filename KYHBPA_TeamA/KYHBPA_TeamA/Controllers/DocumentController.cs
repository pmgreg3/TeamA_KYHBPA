using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KYHBPA_TeamA.Controllers
{
    public class DocumentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Document
        public ActionResult Index()
        {
            var docs = db.Documents.Select(d => new DisplayDocumentsViewModel()
            {
                Id = d.DocumentId,
                Content = d.DocumentContent,
                Description = d.DocumentDescription,
                Title = d.DocumentName,
                Date = d.DocumentUploadDateTime
            });

            return View(docs);
        }

        // GET: Document/Details/5
        public ActionResult Details(int id)
        {
            var document = db.Documents.Find(id);
            var newDocument = new DisplayDocumentsViewModel()
            {
                Id = document.DocumentId,
                Content = document.DocumentContent,
                Description = document.DocumentDescription,
                Title = document.DocumentName,
                Date = document.DocumentUploadDateTime
            };

            return View(newDocument);
        }

        // GET: Document/Admin
        [Authorize(Roles="Admin")]
        public ActionResult Admin()
        {
            var docs = db.Documents.Select(d => new DisplayDocumentsViewModel()
            {
                Id = d.DocumentId,
                Content = d.DocumentContent,
                Description = d.DocumentDescription,
                Title = d.DocumentName,
                Date = d.DocumentUploadDateTime
            });

            return View(docs);
        }


        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Document/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(AddDocumentViewModel addViewModel, HttpPostedFileBase file = null)
        {
            if (ModelState.IsValid)
            {
                var doc = new Document()
                {
                    DocumentUploadDateTime = DateTime.Now,
                    DocumentDescription = addViewModel.Description,
                    DocumentContent = new byte[file.ContentLength],
                    DocumentName = addViewModel.Title
                };
                file.InputStream.Read(doc.DocumentContent, 0, file.ContentLength);
                db.Documents.Add(doc);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: Document/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var document = db.Documents.Find(id);
            var vm = new DisplayDocumentsViewModel()
            {
                Id = document.DocumentId,
                Content = document.DocumentContent,
                Title = document.DocumentName,
                Description = document.DocumentDescription                
            };

            if (document == null)
                return HttpNotFound();

            return View(vm);
        }

        // POST: Document/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(DisplayDocumentsViewModel documentVM, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var documentToUpdate = db.Documents.FirstOrDefault(x => x.DocumentId == documentVM.Id);
                    if (documentToUpdate != null)
                    {
                        documentToUpdate.DocumentDescription = documentVM.Description;
                        documentToUpdate.DocumentName = documentVM.Title;

                        if(file != null)
                        {
                            byte[] uploadedFile = new byte[file.InputStream.Length];
                            file.InputStream.Read(uploadedFile, 0, file.ContentLength);
                            documentToUpdate.DocumentContent = uploadedFile;
                        }
                    }

                    db.Entry(documentToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = string.Format($"{documentVM.Title} document has been updated!");
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Document/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            
            var document = db.Documents.Find(id);
            
            if(document == null)
            {
                return HttpNotFound();
            }
            else
            {
                var vm = new DisplayDocumentsViewModel()
                {
                    Id = document.DocumentId,
                    Content = document.DocumentContent,
                    Date = document.DocumentUploadDateTime,
                    Title = document.DocumentName,
                    Description = document.DocumentDescription
                };

                return View(vm);
            }

        }

        // POST: Document/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var document = db.Documents.Find(id);
                db.Documents.Remove(document);
                db.SaveChanges();
                return RedirectToAction("Admin");

            }
            catch
            {
                return RedirectToAction("Admin");
            }
        }

        /// <summary>
        /// Returns document from database if it is found
        /// </summary>
        /// <param name="id">ID of document to get</param>
        /// <returns>File of document to render</returns>
        public FileContentResult GetDocument(int id)
        {
            Document docToGet = db.Documents.Find(id);

            if (docToGet != null)
                return File(docToGet.DocumentContent, "application/pdf");
            else
                return null;
        }
    }
}