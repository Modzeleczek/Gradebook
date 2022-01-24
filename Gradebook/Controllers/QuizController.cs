using Gradebook.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gradebook.Utils;

namespace Gradebook.Controllers
{
    [ViewFilter]
    public partial class QuizController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        [Authorize(Roles = Role.TeacherStudent)]
        public ActionResult Index()
        {
            Quiz[] quizzes = null;
            if (User.IsInRole(Role.Teacher))
            {
                var teacherId = User.Identity.GetUserId();
                quizzes = Db.Quiz.Where(e => e.AuthorId == teacherId).ToArray();
            }
            else // if (User.IsInRole(Role.Student))
            {
                var studentId = User.Identity.GetUserId();
                var studentClass = Db.Student.Where(e => e.Id == studentId).Single().Class;
                quizzes = studentClass.QuizSharings.Select(e => e.Quiz).ToArray();
            }
            return View(quizzes);
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult Details(int id)
        {
            return View(Db.Quiz.Where(e => e.Id == id).Single());
        }

        private LinkedList<SelectListItem> GetSubjects()
        {
            var teacherId = User.Identity.GetUserId();
            var teacher = Db.Teacher.Where(e => e.Id == teacherId).Single();
            var teacherSubjects = teacher.TeacherClassSubjects.Distinct(new ComparerBySubject());
            var records = teacherSubjects.Select(e => e.Subject);
            var list = new LinkedList<SelectListItem>();
            foreach (var r in records)
                list.AddLast(new SelectListItem { Text = $"{r.Name}", Value = r.Id.ToString(), Selected = false });
            return list;
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult Create()
        {
            var subjects = GetSubjects();
            if (subjects.First != null)
                subjects.First.Value.Selected = true;
            ViewBag.Subjects = subjects;
            return View();
        }

        // POST
        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult Create(Quiz quiz)
        {
            if (ModelState.IsValid == false)
            {
                var subjects = GetSubjects();
                foreach (var s in subjects)
                    if (s.Value == quiz.SubjectId.ToString())
                    {
                        s.Selected = true;
                        break;
                    }
                ViewBag.Subjects = subjects;
                return View(quiz);
            }
            quiz.AuthorId = User.Identity.GetUserId();
            quiz.ModificationTime = DateTime.Now;
            Db.Quiz.Add(quiz);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult Edit(int id)
        {
            var subjects = GetSubjects();
            ViewBag.Subjects = subjects;
            return View(Db.Quiz.Where(e => e.Id == id).Single());
        }

        // POST
        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult Edit(Quiz quiz)
        {
            var subjects = GetSubjects();
            ViewBag.Subjects = subjects;
            if (ModelState.IsValid == false)
                return View(quiz);
            var record = Db.Quiz.Where(e => e.Id == quiz.Id).Single();
            record.SubjectId = quiz.SubjectId;
            record.Name = quiz.Name;
            record.Duration = quiz.Duration;
            record.ModificationTime = DateTime.Now;
            Db.SaveChanges();
            return View(record);
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteAnswer(int quizId, int answerId)
        {
            var answer = Db.ClosedQuestionOption.Where(e => e.Id == answerId).Single();
            answer.ClosedQuestion.Quiz.ModificationTime = DateTime.Now;
            Db.ClosedQuestionOption.Remove(answer);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        private void PrepareAddAnswerViewBag(int quizId, int questionId)
        {
            ViewBag.QuizId = quizId;
            ViewBag.QuestionId = questionId;
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult AddAnswer(int quizId, int questionId)
        {
            PrepareAddAnswerViewBag(quizId, questionId);
            return View();
        }

        // POST
        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult AddAnswer(int quizId, int questionId, ClosedQuestionOption answer)
        {
            if (ModelState.IsValid == false)
            {
                PrepareAddAnswerViewBag(quizId, questionId);
                return View();
            }
            answer.ClosedQuestionId = questionId;
            Db.ClosedQuestionOption.Add(answer);
            Db.SaveChanges();
            var quiz = Db.Quiz.Where(e => e.Id == quizId).Single();
            quiz.ModificationTime = DateTime.Now;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteQuestion(int quizId, int questionId)
        {
            var question = Db.ClosedQuestion.Where(e => e.Id == questionId).Single();
            question.Quiz.ModificationTime = DateTime.Now;
            Db.ClosedQuestion.Remove(question);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        private void PrepareAddQuestionViewBag(int quizId)
        {
            ViewBag.QuizId = quizId;
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult AddQuestion(int quizId)
        {
            PrepareAddQuestionViewBag(quizId);
            return View();
        }

        // POST
        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult AddQuestion(int quizId, ClosedQuestion question)
        {
            if (ModelState.IsValid == false)
            {
                PrepareAddQuestionViewBag(quizId);
                return View();
            }
            question.QuizId = quizId;
            Db.ClosedQuestion.Add(question);
            Db.SaveChanges();
            var quiz = Db.Quiz.Where(e => e.Id == quizId).Single();
            quiz.ModificationTime = DateTime.Now;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteSharing(int quizId, int classId)
        {
            var quizSharing = Db.QuizSharing.Where(e => e.QuizId == quizId && e.ClassId == classId).Single();
            Db.QuizSharing.Remove(quizSharing);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        private LinkedList<SelectListItem> GetUnusedClasses(int quizId)
        {
            // 1. sposób
            /* var allClasses = Db.Class.ToArray();
            var grantedClasses = Db.QuizSharing.Select(e => e.Class).ToArray();
            var records = allClasses.Except(grantedClasses); */
            var quiz = Db.Quiz.Where(e => e.Id == quizId).Single();
            var subjectId = quiz.SubjectId;
            if (subjectId == null)
                return new LinkedList<SelectListItem>();
            var grantedClasses = Db.QuizSharing.Where(e => e.QuizId == quizId).Select(e => e.ClassId);
            var nonGrantedClasses = Db.Class.Where(e => grantedClasses.Contains(e.Id) == false);
            var subjectHavingClasses = nonGrantedClasses.Where(e => e.TeacherClassSubjects.Select(tcs => tcs.SubjectId).Contains(subjectId ?? -1));
            var records = subjectHavingClasses.ToArray();
            var list = new LinkedList<SelectListItem>();
            foreach (var r in records)
                list.AddLast(new SelectListItem { Text = $"{r.Year} {r.Unit}", Value = r.Id.ToString(), Selected = false });
            return list;
        }

        private void PrepareAddQuizSharingViewBag(int quizId)
        {
            ViewBag.QuizId = quizId;
            var classes = GetUnusedClasses(quizId);
            if (classes.First != null)
                classes.First.Value.Selected = true;
            ViewBag.Classes = classes;
        }

        // GET
        [Authorize(Roles = Role.Teacher)]
        public ActionResult AddQuizSharing(int quizId)
        {
            PrepareAddQuizSharingViewBag(quizId);
            return View();
        }

        // POST
        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult AddQuizSharing(int quizId, QuizSharing quizSharing)
        {
            if (ModelState.IsValid == false)
            {
                PrepareAddQuizSharingViewBag(quizId);
                return View();
            }
            if (quizSharing.ClassId == 0)
                return RedirectToAction("Edit", new { id = quizId });
            Db.QuizSharing.Add(quizSharing);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        // GET
        /* [Authorize(Roles = Role.Teacher)]
        public ActionResult Delete(int id) // do naprawy
        {
            var quiz = Db.Quiz.Where(e => e.Id == id).Single();
            var questions = new LinkedList<ClosedQuestion>();
            var options = new LinkedList<ClosedQuestionOption>();
            var answers = new LinkedList<ClosedQuestionAnswer>();
            var attempts = new LinkedList<QuizAttempt>();
            foreach (var attempt in quiz.QuizAttempts)
                attempts.AddLast(attempt);
            foreach (var question in quiz.ClosedQuestions)
            {
                foreach (var option in question.Options)
                {
                    foreach (var answer in option.Answers)
                    {
                        answers.AddLast(answer);
                    }
                    options.AddLast(option);
                }
                questions.AddLast(question);
            }
            foreach (var question in questions)
            {
                foreach (var option in options)
                {
                    foreach (var answer in answers)
                    {
                        Db.ClosedQuestionAnswer.Remove(answer);
                        Db.SaveChanges();
                    }
                    Db.ClosedQuestionOption.Remove(option);
                    Db.SaveChanges();
                }
                Db.ClosedQuestion.Remove(question);
                Db.SaveChanges();
            }
            foreach (var attempt in attempts)
            {
                Db.QuizAttempt.Remove(attempt);
                Db.SaveChanges();
            }
            Db.Quiz.Remove(quiz);
            Db.SaveChanges();
            return RedirectToAction("Index");
        } */
    }
}