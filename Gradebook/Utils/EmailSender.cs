﻿using Gradebook.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Gradebook.Utils
{
    public static partial class EmailSender
    {
        private const string UserName = "lukasz.modzelewski1@gmail.com", Password = "huisayobarctkwaf";

        private static SmtpClient CreateSmtp()
        {
            var smtp = new SmtpClient();
            var credential = new NetworkCredential { UserName = UserName, Password = Password };
            smtp.Credentials = credential;
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            return smtp;
        }

        private static MailMessage CreateMessage(string replyTo, ICollection<ApplicationUser> recipients, string subject, string body, bool isBodyHtml)
        {
            var message = new MailMessage();
            // message.ReplyToList.Clear();
            message.ReplyToList.Add(replyTo);
            // Actual e-mail address of the sender.
            message.Sender = new MailAddress(UserName, "Gradebook");
            // From - it can be the sender's name, possibly with address.
            message.From = message.Sender;
            // message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            foreach (var r in recipients)
                message.To.Add(new MailAddress(r.Email, $"{r.Name} {r.Surname}"));
            return message;
        }

        public static void Send(string replyTo, ApplicationUser recipient, string subject, string body, HttpPostedFileBase file, bool isBodyHtml)
        {
            Send(replyTo, new ApplicationUser[] { recipient }, subject, body, file, isBodyHtml);
        }

        public static void Send(string replyTo, ICollection<ApplicationUser> recipients, string subject, string body, HttpPostedFileBase file, bool isBodyHtml)
        {
            var smtp = CreateSmtp();
            var message = CreateMessage(replyTo, recipients, subject, body, isBodyHtml);
            if (file != null)
                message.Attachments.Add(new System.Net.Mail.Attachment(file.InputStream, file.FileName, file.ContentType));
            smtp.Send(message);
            smtp.Dispose();
        }
    }
}
