using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Administrator), ViewFilter]
    public class GlobalAnnouncementController : ControllerBase
    {
        public ActionResult List()
        {
            return View(Db.GlobalAnnouncement.ToArray());
        }

        public ActionResult Details(int? id)
        {
            var search = Db.GlobalAnnouncement.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Announcement does not exist.");
            return View(search.Single());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string content)
        {
            var d = LocalizedStrings.GlobalAnnouncement.Create[LanguageCookie.Read(Request.Cookies)];
            if (string.IsNullOrEmpty(content)) { ViewBag.ValidationMessage = d["Specify content."]; return View(); }
            var ga = new GlobalAnnouncement();
            ga.Content = content;
            ga.ModificationTime = DateTime.Now;
            ga.AuthorId = User.Identity.GetUserId();
            Db.GlobalAnnouncement.Add(ga);
            Db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Edit(int? id)
        {
            var d = LocalizedStrings.GlobalAnnouncement.Edit[LanguageCookie.Read(Request.Cookies)];
            var search = Db.GlobalAnnouncement.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Announcement does not exist.");
            return View(search.Single());
        }

        [HttpPost]
        public ActionResult Edit(int? id, string content)
        {
            var d = LocalizedStrings.GlobalAnnouncement.Edit[LanguageCookie.Read(Request.Cookies)];
            var search = Db.GlobalAnnouncement.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Announcement does not exist.");
            var ga = search.Single();
            if (string.IsNullOrEmpty(content)) { ViewBag.ValidationMessage = d["Specify content."]; return View(ga); }
            ga.Content = content;
            ga.ModificationTime = DateTime.Now;
            ga.AuthorId = User.Identity.GetUserId();
            Db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Delete(int? id)
        {
            var search = Db.GlobalAnnouncement.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Announcement does not exist.");
            var ga = search.Single();
            Db.GlobalAnnouncement.Remove(ga);
            Db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
