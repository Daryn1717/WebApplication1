using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class AuthorController : Controller
    {
        public ActionResult Index()
        {
            List<Authors> authors;
            using (Model1 db = new Model1())
            {
                authors = db.Authors.ToList();             
            }
            return View(authors);
        }


        public ActionResult Delete(int id)
        {
            using (Model1 db = new Model1())
            {
                db.Authors.Remove(db.Authors.FirstOrDefault(a => a.Id == id));
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Edit(Authors author)
        {
            using (Model1 db = new Model1())
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id = 0)
        {
            Authors author;
            using (Model1 db = new Model1())
            {
                author = db.Authors.FirstOrDefault(a => a.Id == id);
                db.SaveChanges();
            }
            return View(author);
        }



        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Authors author)
        {
            using (Model1 db = new Model1())
            {
                db.Authors.Add(author);
                db.SaveChanges();
            }
            return Redirect("Index");
        }
    }
}