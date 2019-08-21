using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        public ActionResult Index()
        {
            List<Books> books;
            using (Model1 db = new Model1())
            {
                books = db.Books.ToList();
            }
            return View(books);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(Books book)
        {
            using (Model1 db = new Model1())
            {
                db.Books.Add(book);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult Edit(int id,string title)
        {
            Books book;
            using (Model1 db = new Model1())
            {
                book = db.Books.FirstOrDefault(b => b.Id == id && b.Title == title); 
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Books book)
        {
            using (Model1 db = new Model1())
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}