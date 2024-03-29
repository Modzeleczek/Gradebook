﻿using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Student.Controllers
{
    [Authorize(Roles = Role.Student), ViewFilter]
    public class ClassController : ControllerBase
    {
        public ActionResult Details()
        {
            var userId = User.Identity.GetUserId();
            var student = Db.Student.Where(e => e.Id == userId).Single();
            if (student.ClassId == null) return ErrorView("You do not belong to any class.");
            return View(student.Class);
        }
    }
}
