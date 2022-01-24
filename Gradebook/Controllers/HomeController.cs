using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gradebook.Models;

namespace Gradebook.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext.Create().Database.CreateIfNotExists();
            return RedirectToAction("Index", "GlobalAnnouncement");
        }

        public ActionResult About()
        {
            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            return RedirectToAction("Index");
        }
    }
}