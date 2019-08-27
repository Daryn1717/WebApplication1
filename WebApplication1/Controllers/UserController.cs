using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            List<Users> users;
            using (Model1 db = new Model1())
            {
                users = db.Users.ToList();
            }
            return View(users);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public ActionResult Create(Users user)
        {
            using (Model1 db = new Model1())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Users user;
            using (Model1 db = new Model1())
            {
                user = db.Users.FirstOrDefault(u => u.Id == id);
            }
            return View("Edit", user);
        }
        [HttpPost]
        public ActionResult Edit(Users user)
        {
            using (Model1 db = new Model1())
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            using (Model1 db = new Model1())
            {
                db.Users.Remove(db.Users.FirstOrDefault(u => u.Id == id));
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}