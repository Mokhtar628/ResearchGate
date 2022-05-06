using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using System.IO;

namespace ResearchGate.Controllers
{
    public class AddArticalsController : Controller
    {
        private ResearchGateDatabaseEntities db = new ResearchGateDatabaseEntities();

        public ActionResult ArticalDetails()
        {

            return View();
        }

        // POST: AddArticals/ArticalDetails
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArticalDetails([Bind(Include = "ID,title,Paper,abstract,pdfFile,collaboratorsText")] Artical artical)
        {
           

            if (ModelState.IsValid)
             {
                 string fileName = Path.GetFileNameWithoutExtension(artical.pdfFile.FileName);
                 string extension = Path.GetExtension(artical.pdfFile.FileName);
                 fileName = DateTime.Now.ToString("yymmssfff") + fileName + extension;
                 artical.Paper = "~/Papers/" + fileName;
                 fileName = Path.Combine(Server.MapPath("~/Papers/"), fileName);
                 artical.pdfFile.SaveAs(fileName);
                 db.Articals.Add(artical);
                 db.SaveChanges();
                 //return RedirectToAction("Index"); // change it to article overview 
             }

            Artical lastArticalAdded = db.Articals.OrderByDescending(p => p.ID).First();
            string groupOfEmail = artical.collaboratorsText;
            List<string> listStrLineElements = groupOfEmail.Split(' ').ToList();
            List<int> collaboratorsId = getId_emails(listStrLineElements);
            addCollaborators(collaboratorsId, lastArticalAdded.ID);


            return View(artical);
        }


        public List<int> getId_emails(List<string> collaborators_email)
           {
            List < int > collaboratorsId= new List<int>();
            
            foreach (var email in collaborators_email)
            {
                
                var obj = db.Authors.Where(a => a.email.Equals(email)).FirstOrDefault();
                if (obj != null)
                {
                    collaboratorsId.Add(obj.ID);
                }
            }
            return collaboratorsId ;
           }


        public void addCollaborators (List<int> coId,int articaleId)
        {

            Artical artical = db.Articals.Find(articaleId);
            foreach (var item in coId)
            {
                
                Author author = db.Authors.Find(item);
                artical.Authors.Add(author);
               
            }
            db.SaveChanges();

        }

        public ActionResult ShowArtical ()
        {
            return View();
        }




        // GET: AddArticals/Details/5
        public ActionResult ShowArticalDetails(int? id=5)
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
      public ActionResult Comment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Comment([Bind(Include = "ID,comment,dates,ArticalId,AuthorID")] Comment comment)
        {
            //string date =DateTime.;

            if (ModelState.IsValid)
            {
                comment.AuthorID = int.Parse(Session["ID"].ToString());
                comment.dates = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
            }

           
            return View(comment);
        }



        // GET: AddArticals/Create
        public ActionResult Create()
        {
            return View();
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
