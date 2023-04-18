using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Controllers
{
    [Authorize, ViewFilter]
    public class MessageController : ControllerBase
    {
        public ActionResult List()
        {
            var userId = User.Identity.GetUserId();
            var user = Db.Users.Where(e => e.Id == userId).Single();
            var received = new LinkedList<Message>();
            foreach (var r in user.MessageReceipts)
                received.AddLast(r.Message);
            var sent = new LinkedList<Message>();
            foreach (var s in user.SentMessages)
                sent.AddLast(s);
            ViewBag.Received = received;
            ViewBag.Sent = sent;
            return View();
        }

        public ActionResult Details(int? id)
        {
            var search = Db.Message.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such message does not exist.");
            var message = search.Single();
            var userId = User.Identity.GetUserId();
            if (message.SenderId == userId) // Message is a sent one.
            {
                ViewBag.Recipients = Db.MessageRecipient.Where(e => e.MessageId == id).ToList();
                return View("SentDetails", message);
            }
            else
            {
                var received = Db.MessageRecipient.Where(e => e.MessageId == id && e.RecipientId == userId);
                if (received.Count() == 1)
                {
                    return View("ReceivedDetails", message);
                }
            }
            return ErrorView("You are not sender nor recipient of such message.");
        }

        public ActionResult DownloadAttachment(int? id)
        {
            var search = Db.Attachment.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such attachment does not exist.");
            var attachment = search.Single();
            var userId = User.Identity.GetUserId();
            if (attachment.Message.SenderId != userId)
            {
                var recipientSearch = Db.MessageRecipient.Where(e => e.MessageId == attachment.MessageId && e.RecipientId == userId);
                if (recipientSearch.Count() != 1) return ErrorView("You are not sender nor recipient of such recipient.");
            }
            var hex = attachment.Data;
            var type = attachment.FileType;
            var name = attachment.Name;
            return File(FileType.HexStringToByteArray(hex), type, name);
        }

        public ActionResult Create()
        {
            var recipients = (LinkedList<ApplicationUser>)Session["Recipients"];
            if (recipients == null)
                recipients = new LinkedList<ApplicationUser>();
            ViewBag.Recipients = recipients;
            return View();
        }

        [HttpPost]
        public ActionResult Create(string content, HttpPostedFileBase[] attachedFiles)
        {
            var d = LocalizedStrings.Message.Create[LanguageCookie.Read(Request.Cookies)];
            var message = new Message { Content = content };
            var recipients = (LinkedList<ApplicationUser>)Session[SESSION_KEY];
            if (recipients == null || recipients.Count == 0)
            { Session[SESSION_KEY] = null; ViewBag.ValidationMessage = d["Message does not have recipients."]; return View(message); }
            if (string.IsNullOrEmpty(content))
            { ViewBag.ValidationMessage = d["Specify content."]; ViewBag.Recipients = recipients; return View(message); }
            message.SenderId = User.Identity.GetUserId();
            message.SendTime = DateTime.Now;
            Db.Message.Add(message);
            Db.SaveChanges();
            foreach (var r in recipients)
            {
                var messageRecipient = new MessageRecipient { MessageId = message.Id, RecipientId = r.Id };
                Db.MessageRecipient.Add(messageRecipient);
            }
            // User chose file(s).
            if (attachedFiles != null && attachedFiles.Length != 0)
            {
                foreach (var f in attachedFiles)
                {
                    if (f == null)
                        continue;
                    var attachment = new Attachment { MessageId = message.Id, Name = f.FileName, FileType = f.ContentType, Data = FileType.StreamToHexString(f.InputStream) };
                    Db.Attachment.Add(attachment);
                }
            }
            Db.SaveChanges();
            Session[SESSION_KEY] = null;
            return RedirectToAction("List");
        }

        public ActionResult AddRecipient()
        {
            return View();
        }

        public JsonResult GetUsers()
        {
            if (Session[SESSION_KEY] == null)
                Session[SESSION_KEY] = new LinkedList<ApplicationUser>();
            var alreadyAdded = (LinkedList<ApplicationUser>)Session[SESSION_KEY];
            var allUsers = Db.Users.ToArray();
            var comparer = new Utils.Comparer<ApplicationUser>((x, y) => x.Id == y.Id, obj => obj.Id.GetHashCode());
            var unused = allUsers.Except(alreadyAdded, comparer);
            var list = new LinkedList<object>();
            foreach (var u in unused)
                list.AddLast(new { Id = u.Id, Name = u.Name, Surname = u.Surname, Email = u.Email });
            return Json(list);
        }

        const string SESSION_KEY = "Recipients";
        [HttpPost]
        public ActionResult AddRecipient(string userId)
        {
            if (Session[SESSION_KEY] == null)
                Session[SESSION_KEY] = new LinkedList<ApplicationUser>();
            var recipients = (LinkedList<ApplicationUser>)Session[SESSION_KEY];
            var searchInDatabase = Db.Users.Where(e => e.Id == userId);
            if (searchInDatabase.Count() != 1) return ErrorView("Such user does not exist in database.");
            var searchInSession = recipients.Where(e => e.Id == userId);
            if (searchInSession.Count() != 0) return ErrorView("The user is already added.");
            var user = searchInDatabase.Single();
            recipients.AddLast(user);
            return RedirectToAction("Create");
        }

        public ActionResult DeleteRecipient(string userId)
        {
            if (Session[SESSION_KEY] == null)
                return RedirectToAction("Create");
            var recipients = (LinkedList<ApplicationUser>)Session[SESSION_KEY];
            var searchInSession = recipients.Where(e => e.Id == userId);
            if (searchInSession.Count() != 1) return ErrorView("Such user does not exist in session.");
            recipients.Remove(searchInSession.Single());
            return RedirectToAction("Create");
        }
    }
}
