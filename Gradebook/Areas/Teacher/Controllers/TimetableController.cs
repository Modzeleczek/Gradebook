﻿using Gradebook.Models;
using Gradebook.Models.ViewModels;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class TimetableController : ControllerBase
    {
        public ActionResult List()
        {
            var id = User.Identity.GetUserId();
            var tcss = Db.TeacherClassSubject.Where(e => e.TeacherId == id).ToArray();
            var lessons = new LinkedList<Lesson>();
            foreach (var tcs in tcss)
                foreach (var l in tcs.Lessons)
                    lessons.AddLast(l);
            var lessonMap = new Lesson[Days.Array.Length, LessonHours.Array.Length];
            foreach (var l in lessons)
                lessonMap[l.DayId, l.HourId] = l;
            return View(lessonMap);
        }
    }
}
