﻿using System.Web.Mvc;
using Gradebook.Utils;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Controllers
{
    [AllowAnonymous, ViewFilter]
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            var l = Role.GetLinks(User)[0];
            return RedirectToAction(l.Action, l.Controller, new { area = l.Area });
        }

        public ActionResult GenericError(string message)
        {
            return ErrorView(message);
        }
    }
}
