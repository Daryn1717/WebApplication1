using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class MyPurchase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FIO { get; set; }
    }
    public class PurchaseController : Controller
    {
        // GET: Purchase
        public ActionResult Index()
        {
            List<MyPurchase> purchases = new List<MyPurchase>();
            using (Model1 db = new Model1())
            {
                foreach(Purchases p in db.Purchases.ToList())
                {
                    purchases.Add(new MyPurchase()
                    {
                        Id = p.Id,
                        Title = db.Books.FirstOrDefault(b => b.Id == p.BookId).Title,
                        FIO = db.Users.FirstOrDefault(u => u.Id == p.UserId).FIO
                    });
                }
            }
            return View(purchases);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public ActionResult Create(Purchases purchase)
        {
            using (Model1 db = new Model1())
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {

            Purchases purchase;
            List<Books> books = new List<Books>();
            using (Model1 db = new Model1())
            {
                purchase = db.Purchases.FirstOrDefault(p => p.Id == id);
                foreach(int item in db.Books.Join(db.Purchases, b => b.Id, p => p.BookId, (b, p) => p.UserId).Where(p => p == purchase.UserId).ToList())
                {
                    books.Add(db.Books.FirstOrDefault(b => b.Id == item));
                }
                ViewBag.Books = new SelectList(db.Books.ToList(),"Id","Title");
                ViewBag.Users = new SelectList(db.Users.ToList(),"Id","FIO");
                ViewBag.UserBooks = books;
            }
            return View("Edit", purchase);
        }
        [HttpPost]
        public ActionResult Edit(Purchases purchase)
        {
            using (Model1 db = new Model1())
            {
                db.Entry(purchase).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            using (Model1 db = new Model1())
            {
                db.Purchases.Remove(db.Purchases.FirstOrDefault(p => p.Id == id));
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}