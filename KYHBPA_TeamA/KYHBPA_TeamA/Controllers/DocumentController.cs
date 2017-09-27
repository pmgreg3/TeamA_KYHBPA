using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }


        // GET
        public ActionResult Create()
        {
            return View();
        }

        // POST: Document/Create
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Document/Edit/5
        [HttpPost]
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

        // GET: Document/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Document/Delete/5
        [HttpPost]
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

        /// <summary>
        /// Returns document from database if it is found
        /// </summary>
        /// <param name="id">ID of document to get</param>
        /// <returns>File of document to render</returns>
        public FileContentResult GetDocument(int id)
        {
            Document docToGet = db.Documents.Find(id);

            if (docToGet != null)
                return File(docToGet.DocumentContent, docToGet.DocumentDescription);
            else
                return null;
        }
    }
}