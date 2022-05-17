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
        private ResearchGateDatabaseEntities1 db = new ResearchGateDatabaseEntities1();


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

                if (object.ReferenceEquals(author.ImageFile, null))
                {
                    author.ProfileImage = "~/UsersImage/profImg.png";

                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(author.ImageFile.FileName);
                    string extension = Path.GetExtension(author.ImageFile.FileName);
                    fileName = DateTime.Now.ToString("yymmssfff") + fileName + extension;
                    author.ProfileImage = "~/UsersImage/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/UsersImage/"), fileName);
                    author.ImageFile.SaveAs(fileName);
                }
                

                db.Authors.Add(author);
                db.SaveChanges();
                ViewData["success"] = "You registered successfully.";
                return RedirectToAction("Login");
            }

            return View(author);
        }


        // GET: Authors/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
               // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
               // return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( HttpPostedFileBase ImageFile ,[Bind(Include = "ID,Fname,Mname,Lname,pass,ProfileImage,email,Mobile,university,ImageFile")] Author author )
        {
            string email = Session["email"].ToString();
            string pass = Session["pass"].ToString();
            if (ModelState.IsValid)
            {
                using (ResearchGateDatabaseEntities1 db = new ResearchGateDatabaseEntities1())
                {
                    var obj = db.Authors.Where(a => a.email.Equals(email) && a.pass.Equals(pass)).FirstOrDefault();
                    if (obj != null)
                    {
                        if (author.Fname != null)
                        {
                            obj.Fname = author.Fname;
                        }
                        if (author.Mname != null)
                        {
                            obj.Mname = author.Mname;
                        }
                        if (author.Lname != null)
                        {
                            obj.Lname = author.Lname;
                        }
                        if (author.pass != null)
                        {
                            obj.pass = author.pass;
                        }
                        if (author.email != null)
                        {
                            obj.email = author.email;
                        }
                        if (author.Mobile != null)
                        {
                            obj.Mobile = author.Mobile;
                        }
                        if (ImageFile != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                            string extension = Path.GetExtension(ImageFile.FileName);
                            fileName = DateTime.Now.ToString("yymmssfff") + fileName + extension;
                            obj.ProfileImage = "~/UsersImage/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/UsersImage/"), fileName);
                            ImageFile.SaveAs(fileName);
                        }
                        if (author.university != null)
                        {
                            obj.university = author.university;
                        }
                    }
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["ID"] = obj.ID.ToString();
                    Session["email"] = obj.email.ToString();
                    Session["Fname"] = obj.Fname.ToString();
                    Session["Lname"] = obj.Lname.ToString();
                    Session["ProfileImage"] = obj.ProfileImage.ToString();
                    Session["university"] = obj.university.ToString();
                    Session["Mname"] = obj.Mname.ToString();
                    Session["pass"] = obj.pass.ToString();
                    Session["Mobile"] = obj.Mobile.ToString();
                    return RedirectToAction("Review");
                }
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
                using (ResearchGateDatabaseEntities1 db = new ResearchGateDatabaseEntities1())
                {
                    var obj = db.Authors.Where(a => a.email.Equals(author.email) && a.pass.Equals(author.pass)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["ID"] = obj.ID.ToString();
                        Session["email"] = obj.email.ToString();
                        Session["Fname"] = obj.Fname.ToString();
                        Session["Lname"] = obj.Lname.ToString();
                        Session["ProfileImage"] = obj.ProfileImage.ToString();
                        Session["university"] = obj.university.ToString();
                        Session["Mname"] = obj.Mname.ToString();
                        Session["pass"] = obj.pass.ToString();
                        Session["Mobile"] = obj.Mobile.ToString();


                        
                        return RedirectToAction("UserDashBoard");
                    }
                    else
                    {
                        ModelState.AddModelError("LoginError", "Sorry, you entered incorrect data.");
                    }
                }
            }
            return View(author);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["ID"] != null)
            {
                var articles = from t in db.Articals
                               where true
                               select t;
                articles = articles.OrderByDescending(p => p.ID);
                return View(articles);
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
        public ActionResult VisitProfile(int? id)
        {
            if (id == null)
            {

            }
            return View(db.Authors.Find(id));
        }
        public ActionResult Review()
        {
            //Author obj = new Author();
            //obj.Fname = Session["Fname"].ToString();
            //obj.Mname = Session["Mname"].ToString();
            //obj.Lname = Session["Lname"].ToString();
            //obj.university = Session["university"].ToString();
            //obj.ProfileImage = Session["ProfileImage"].ToString();
            Author author = db.Authors.Find(int.Parse(Session["ID"].ToString()));
            return View(author);
        }
        [HttpGet]
        public ActionResult ReviewUserProfile(int id)
        {
            var obj = db.Authors.Where(a => a.ID.Equals(id)).FirstOrDefault();

            return View(obj);
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
