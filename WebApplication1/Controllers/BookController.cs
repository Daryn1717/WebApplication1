using System;
using System.Collections.Generic;
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
    }
}