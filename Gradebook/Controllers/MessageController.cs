using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;

namespace Gradebook.Controllers
{
    [Authorize, ViewFilter]
    public class MessageController : Controller
    {
        ApplicationDbContext Db = ApplicationDbContext.Create();

        // GET: Message
        public ActionResult Index()
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

        // GET: Message/Details/5
        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var received = Db.MessageRecipient.Where(e => e.MessageId == id && e.RecipientId == userId);
            var sent = Db.Message.Where(e => e.Id == id && e.SenderId == userId);
            if (sent.Any()) // wiadomość jest wysłaną
            {
                ViewBag.IsSent = true;
                ViewBag.Recipients = Db.MessageRecipient.Where(e => e.MessageId == id).ToArray();
                return View(sent.Single());
            }
            else if (received.Any()) // wiadomość jest odebraną
            {
                ViewBag.IsSent = false;
                var message = received.Single().Message;
                return View(message);
            }
            return RedirectToAction("Index"); // wiadomość nie istnieje
        }

        // GET
        public ActionResult DownloadAttachment(int id)
        {
            var attachment = Db.Attachment.Where(e => e.Id == id).Single();
            var hex = attachment.Data;
            var type = attachment.FileType;
            var name = attachment.Name;
            return File(FileType.HexStringToByteArray(hex), type, name);
        }

        private LinkedList<SelectListItem> GetRecipients()
        {
            var records = Db.Users.Select(r => new { r.Id, r.Name, r.Surname, r.Email });
            var list = new LinkedList<SelectListItem>();
            foreach (var r in records)
                list.AddLast(new SelectListItem { Text = $"{r.Name} {r.Surname} | {r.Email}", Value = r.Id, Selected = false });
            return list;
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            var recipients = (LinkedList<ApplicationUser>)Session["Recipients"];
            if (recipients == null)
                recipients = new LinkedList<ApplicationUser>();
            ViewBag.Recipients = recipients;
            /*var attachments = (LinkedList<Attachment>)Session["Attachments"];
            if (attachments == null)
                attachments = new LinkedList<Attachment>();
            ViewBag.Attachments = attachments;*/
            return View();
        }

        // POST: Message/Create
        [HttpPost]
        public ActionResult Create(Message message, HttpPostedFileBase attachedFile)
        {
            var recipients = (LinkedList<ApplicationUser>)Session[SESSION_KEY];
            if (recipients == null)
                return RedirectToAction("Index");
            if (recipients.Count > 0)
            {
                message.SenderId = User.Identity.GetUserId();
                message.SendTime = DateTime.Now;
                // message.Content jest bindowane z formularza
                Db.Message.Add(message);
                Db.SaveChanges();
                foreach (var r in recipients)
                {
                    var messageRecipient = new MessageRecipient { MessageId = message.Id, RecipientId = r.Id };
                    Db.MessageRecipient.Add(messageRecipient);
                }
                if (attachedFile != null) // użytkownik wybrał plik
                    SaveAttachment(message.Id, attachedFile);
                Db.SaveChanges();
            }
            Session[SESSION_KEY] = null;
            return RedirectToAction("Index");
        }

        private void SaveAttachment(int messageId, HttpPostedFileBase file)
        {
            /*var fileTypeIds = Db.FileType.Where(e => e.Name == file.ContentType).Select(e => e.Id);
            if (fileTypeIds.Any())
            {
                var fileTypeId = fileTypeIds.Single();
                var attachment = new Attachment { MessageId = messageId, Name = file.FileName, FileTypeId = fileTypeId, Data = FileType.StreamToHexString(file.InputStream) };
                Db.Attachment.Add(attachment);
            }*/
            var attachment = new Attachment { MessageId = messageId, Name = file.FileName, FileType = file.ContentType, Data = FileType.StreamToHexString(file.InputStream) };
            Db.Attachment.Add(attachment);
        }

        // GET
        public ActionResult AddRecipient()
        {
            var recipients = GetRecipients();
            if (recipients.First != null)
                recipients.First.Value.Selected = true;
            ViewBag.Recipients = recipients;
            return View();
        }

        const string SESSION_KEY = "Recipients";
        // POST
        [HttpPost]
        public ActionResult AddRecipient(string userId)
        {
            if (Session[SESSION_KEY] == null)
                Session[SESSION_KEY] = new LinkedList<ApplicationUser>();
            var recipients = (LinkedList<ApplicationUser>)Session[SESSION_KEY];
            var user = Db.Users.Where(e => e.Id == userId).Single();
            recipients.AddLast(user);
            return RedirectToAction("Create");
        }

        public ActionResult DeleteRecipient(string userId)
        {
            if (Session[SESSION_KEY] == null)
                return RedirectToAction("Create");
            var recipients = (LinkedList<ApplicationUser>)Session[SESSION_KEY];
            var item = recipients.Where(e => e.Id == userId).Single();
            recipients.Remove(item);
            return RedirectToAction("Create");
        }

        /*// GET
        [AllowAnonymous]
        public ActionResult CreateFileTypes(string password)
        {
            if (password != "caviaporcellus")
                return RedirectToAction("About", "Home");
            var names = new string[] { FileType.TXT, FileType.PDF, FileType.ZIP };
            foreach (var n in names)
            {
                var ft = new FileType { Name = n };
                Db.FileType.Add(ft);
            }
            Db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }*/

        // GET
        public ActionResult AddAttachment()
        {
            return View();
        }

        // POST
        [HttpPost]
        public ActionResult AddAttachment(Attachment attachment)
        {
            return View();
        }
    }
}
