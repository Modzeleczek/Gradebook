using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;

namespace Gradebook.Controllers
{
    [Authorize(Roles = Role.Administrator), ViewFilter]
    public class GlobalAnnouncementController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        // GET: GlobalAnnouncement
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(Db.GlobalAnnouncement.ToArray());
        }

        // GET: GlobalAnnouncement/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var ga = Db.GlobalAnnouncement.Where(e => e.Id == id).Single();
            return View(ga);
        }

        // GET: GlobalAnnouncement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GlobalAnnouncement/Create
        [HttpPost]
        public ActionResult Create(GlobalAnnouncement globalAnnouncement)
        {
            if (ModelState.IsValid == false)
                return View();
            globalAnnouncement.ModificationTime = DateTime.Now;
            globalAnnouncement.AuthorId = User.Identity.GetUserId();
            Db.GlobalAnnouncement.Add(globalAnnouncement);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: GlobalAnnouncement/Edit/5
        public ActionResult Edit(int id)
        {
            var ga = Db.GlobalAnnouncement.Where(e => e.Id == id).Single();
            return View(ga);
        }

        // POST: GlobalAnnouncement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, GlobalAnnouncement globalAnnouncement)
        {
            if (ModelState.IsValid == false)
                return View();
            var record = Db.GlobalAnnouncement.Where(e => e.Id == id).Single();
            record.Content = globalAnnouncement.Content;
            record.ModificationTime = DateTime.Now;
            record.AuthorId = User.Identity.GetUserId();
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: GlobalAnnouncement/Delete/5
        public ActionResult Delete(int id)
        {
            var ga = Db.GlobalAnnouncement.Where(e => e.Id == id).Single();
            Db.GlobalAnnouncement.Remove(ga);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
