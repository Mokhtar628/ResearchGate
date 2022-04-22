using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;

namespace ResearchGate.Controllers
{
    public class AddArticalsController : Controller
    {
        private ResearchGateDatabaseEntities db = new ResearchGateDatabaseEntities();


        // GET: AddArticals
        public ActionResult SelectFile()
        {
                return View();
        }

        // GET: AddArticals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artical artical = db.Articals.Find(id);
            if (artical == null)
            {
                return HttpNotFound();
            }
            return View(artical);
        }

        // GET: AddArticals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddArticals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,title,Paper,abstract")] Artical artical)
        {
            if (ModelState.IsValid)
            {
                db.Articals.Add(artical);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artical);
        }

        // GET: AddArticals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artical artical = db.Articals.Find(id);
            if (artical == null)
            {
                return HttpNotFound();
            }
            return View(artical);
        }

        // POST: AddArticals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,title,Paper,abstract")] Artical artical)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artical).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artical);
        }

        // GET: AddArticals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artical artical = db.Articals.Find(id);
            if (artical == null)
            {
                return HttpNotFound();
            }
            return View(artical);
        }

        // POST: AddArticals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artical artical = db.Articals.Find(id);
            db.Articals.Remove(artical);
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
