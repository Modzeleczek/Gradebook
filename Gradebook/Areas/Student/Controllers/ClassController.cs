using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gradebook.Areas.Student.Controllers
{
    public class ClassController : Controller
    {
        // GET: Student/Class
        public ActionResult Index()
        {
            return View();
        }
    }
}