using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;

namespace ResearchGate.Controllers
{
    public class AuthorsController : Controller
    {
        private ResearchGateDatabaseEntities db = new ResearchGateDatabaseEntities();


        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Fname,Mname,Lname,pass,ProfileImage,email,Mobile,university")] Author author)
        {
            if (ModelState.IsValid)
            {
                ////string fileName = Path.GetFileNameWithoutExtension(author.ImageFile.FileName);
                //string fileName = Path.GetFileNameWithoutExtension(author.ProfileImage);
                //string extension = Path.GetExtension(author.ImageFile.FileName)
                string fileName =author.ProfileImage;
                string extension = Path.GetExtension(author.ProfileImage);
                fileName = DateTime.Now.ToString("yymmssfff") + fileName;
                author.ProfileImage = "~/UsersImage/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/UsersImage"), fileName);
                //author.ImageFile.SaveAs(fileName);
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }


        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Fname,Mname,Lname,pass,ProfileImage,email,Mobile,university")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
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
