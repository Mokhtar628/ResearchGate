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
        public ActionResult Create([Bind(Include = "ID,Fname,Mname,Lname,pass,ProfileImage,email,Mobile,university,ImageFile")] Author author)
        {
            if (ModelState.IsValid)
            {
                //TO DO: check if user didnot enter image and pass the default profile image
                string fileName = Path.GetFileNameWithoutExtension(author.ImageFile.FileName);
                string extension = Path.GetExtension(author.ImageFile.FileName);
                fileName = DateTime.Now.ToString("yymmssfff") + fileName + extension;
                author.ProfileImage = "~/UsersImage/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/UsersImage/"), fileName);
                author.ImageFile.SaveAs(fileName);

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


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Author author)
        {
            if (ModelState.IsValid)
            {
                using (ResearchGateDatabaseEntities db = new ResearchGateDatabaseEntities())
                {
                    var obj = db.Authors.Where(a => a.email.Equals(author.email) && a.pass.Equals(author.pass)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["ID"] = obj.ID.ToString();
                        Session["email"] = obj.email.ToString();
                        //Session["Fname"] = obj.Fname.ToString();
                        //Session["Lname"] = obj.Lname.ToString();
                        return RedirectToAction("UserDashBoard");
                    }
                }
            }
            return View(author);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["ID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login");
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
