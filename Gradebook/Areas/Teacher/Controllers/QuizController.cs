﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gradebook.Areas.Teacher.Controllers
{
    public class QuizController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}