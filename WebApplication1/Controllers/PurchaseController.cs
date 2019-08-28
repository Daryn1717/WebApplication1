using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class MyPurchase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FIO { get; set; }
        public string DateOfReturn { get; set; }
        public bool SendNotification { get; set; }
    }
    public class Debtor
    {
        public int UserId { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string RequiredReturnDate { get; set; }
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
                        FIO = db.Users.FirstOrDefault(u => u.Id == p.UserId).FIO,
                        DateOfReturn = (p.DateOfReturn == null)? "Не вернул" : p.DateOfReturn.ToString(),
                        SendNotification = (DateTime.Now > p.DateOfPurchase.AddDays(p.Term)) ? true : false
                    });
                }
            }
            return View(purchases);
        }
        public ActionResult Send(int id)
        {
            string email;
            #region
            string myEmail = "zatov.daryn@mail.ru";
            string password = "kingdaryn17";
            #endregion
            using (Model1 db = new Model1())
            {
                email = db.Users.FirstOrDefault(u => u.Id == db.Purchases.FirstOrDefault(p => p.Id == id).UserId).Email;
            }
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.mail.ru";
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(myEmail, password);

            MailMessage mm = new MailMessage(myEmail,email, "Просрочена дата возвращения книги", "Просим вас вернуть книгу!");
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
            return RedirectToAction("Index");
        }

        public ActionResult Download()
        {
            List<Debtor> debtors = new List<Debtor>();
            byte[] dataBytes;
            using (Model1 db = new Model1())
            {
                db.Purchases.Where(p => p.DateOfPurchase.AddDays(p.Term) > DateTime.Now).
                             Select(p => new { RequiredReturnDate = p.DateOfPurchase.AddDays(p.Term), p.UserId }).
                             ToList().ForEach(i =>
                             {
                                 Users user = db.Users.FirstOrDefault(u => u.Id == i.UserId);
                                 debtors.Add(new Debtor()
                                 {
                                     UserId = user.Id,
                                     FIO = user.FIO,
                                     Email = user.Email,
                                     RequiredReturnDate = i.RequiredReturnDate.ToString()
                                 });
                             });
                string dataString = "";
                debtors.ForEach(d => dataString += $"{d.UserId}){d.FIO} / {d.Email} / {d.RequiredReturnDate}");
                dataBytes = Encoding.UTF8.GetBytes(dataString);
            }
            return File(dataBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Debtors");
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
                purchase.DateOfPurchase = DateTime.Now;
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