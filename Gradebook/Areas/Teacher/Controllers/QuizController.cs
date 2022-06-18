using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class QuizController : ControllerBase
    {
        public ActionResult List()
        {
            var teacherId = User.Identity.GetUserId();
            var quizzes = Db.Quiz.Where(e => e.AuthorId == teacherId).ToArray();
            return View(quizzes);
        }

        public ActionResult Create()
        {
            return View(new Quiz());
        }

        public JsonResult GetSubjects()
        {
            var teacherId = User.Identity.GetUserId();
            var teacherSearch = Db.Teacher.Where(e => e.Id == teacherId);
            if (teacherSearch.Count() != 1) return ErrorJson("You account does not exist.");
            var teacher = teacherSearch.Single();
            var teacherSubjects = teacher.TeacherClassSubjects.Distinct(new ComparerBySubject());
            var subjects = new LinkedList<object>();
            foreach (var ts in teacherSubjects)
            {
                var s = ts.Subject;
                subjects.AddLast(new { Id = s.Id, Name = s.Name });
            }
            return Json(subjects);
        }

        [HttpPost]
        public ActionResult Create(int? subjectId, string name, int? duration)
        {
            var d = LocalizedStrings.Quiz.Create[LanguageCookie.Read(Request.Cookies)];
            bool error = false;
            if (!subjectId.HasValue)
            { ViewBag.ValidationMessage = d["Select subject."]; error = true; }
            else if (!duration.HasValue || duration < 1)
            { ViewBag.ValidationMessage = d["Specify positive duration."]; error = true; }
            if (error) return View(new Quiz { SubjectId = subjectId, Name = name, Duration = duration ?? 0 });
            var teacherId = User.Identity.GetUserId();
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.SubjectId == subjectId && e.TeacherId == teacherId);
            if (tcsSearch.Count() == 0) return ErrorView("You do not teach such subject.");
            var quiz = new Quiz();
            quiz.SubjectId = subjectId;
            quiz.Name = name;
            quiz.Duration = duration.Value;
            quiz.AuthorId = teacherId;
            quiz.ModificationTime = DateTime.Now;
            Db.Quiz.Add(quiz);
            Db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Edit(int? id)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == id && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            return View(quizSearch.Single());
        }

        [HttpPost]
        public ActionResult Edit(int? id, int? subjectId, string name, int? duration)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == id && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            var quiz = quizSearch.Single();
            var d = LocalizedStrings.Quiz.Edit[LanguageCookie.Read(Request.Cookies)];
            if (!subjectId.HasValue)
            { ViewBag.ValidationMessage = d["Select subject."]; return View(quiz); }
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.SubjectId == subjectId && e.TeacherId == teacherId);
            if (tcsSearch.Count() == 0) return ErrorView("You do not teach such subject.");
            if (!duration.HasValue || duration < 1)
            { ViewBag.ValidationMessage = d["Specify positive duration."]; return View(quiz); }
            quiz.SubjectId = subjectId;
            quiz.Name = name;
            quiz.Duration = duration.Value;
            quiz.AuthorId = teacherId;
            quiz.ModificationTime = DateTime.Now;
            Db.SaveChanges();
            return View(quiz);
        }

        public ActionResult AddQuestion(int? quizId)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            ViewBag.QuizId = quizId;
            return View(new ClosedQuestion());
        }

        [HttpPost]
        public ActionResult AddQuestion(int? quizId, string content, float? points)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            var d = LocalizedStrings.Quiz.QuestionEditor[LanguageCookie.Read(Request.Cookies)];
            bool error = false;
            if (string.IsNullOrWhiteSpace(content)) { ViewBag.ValidationMessage = d["Specify content."]; error = true; }
            else if (!points.HasValue || points < 0) { ViewBag.ValidationMessage = d["Specify non-negative number of points."]; error = true; }
            var question = new ClosedQuestion();
            if (error)
            {
                ViewBag.QuizId = quizId;
                question.Content = content;
                question.Points = (points.HasValue ? (points.Value < 0 ? 0 : points.Value) : 0);
                return View(question);
            }
            question.QuizId = quizId;
            question.Content = content;
            question.Points = points.Value;
            Db.ClosedQuestion.Add(question);
            Db.SaveChanges();
            var quiz = quizSearch.Single();
            quiz.ModificationTime = DateTime.Now;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        public ActionResult EditQuestion(int? questionId)
        {
            var teacherId = User.Identity.GetUserId();
            var questionSearch = Db.ClosedQuestion.Where(e => e.Id == questionId && e.Quiz.AuthorId == teacherId);
            if (questionSearch.Count() != 1) return ErrorView("You have not created such question.");
            var question = questionSearch.Single();
            ViewBag.QuestionId = question.Id;
            ViewBag.QuizId = question.QuizId;
            return View(question);
        }

        [HttpPost]
        public ActionResult EditQuestion(int? questionId, string content, float? points)
        {
            var teacherId = User.Identity.GetUserId();
            var questionSearch = Db.ClosedQuestion.Where(e => e.Id == questionId && e.Quiz.AuthorId == teacherId);
            if (questionSearch.Count() != 1) return ErrorView("You have not created such question.");
            var question = questionSearch.Single();
            var d = LocalizedStrings.Quiz.QuestionEditor[LanguageCookie.Read(Request.Cookies)];
            bool error = false;
            if (string.IsNullOrWhiteSpace(content)) { ViewBag.ValidationMessage = d["Specify content."]; error = true; }
            else if (!points.HasValue || points < 0) { ViewBag.ValidationMessage = d["Specify non-negative number of points."]; error = true; }
            if (error)
            {
                ViewBag.QuestionId = question.Id;
                ViewBag.QuizId = question.QuizId;
                question.Content = content;
                question.Points = (points.HasValue ? (points.Value < 0 ? 0 : points.Value) : 0);
                return View(question);
            }
            question.Content = content;
            question.Points = points.Value;
            Db.SaveChanges();
            var quiz = question.Quiz;
            quiz.ModificationTime = DateTime.Now;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = question.QuizId });
        }

        public ActionResult DeleteQuestion(int? questionId)
        {
            var teacherId = User.Identity.GetUserId();
            var questionSearch = Db.ClosedQuestion.Where(e => e.Id == questionId && e.Quiz.AuthorId == teacherId);
            if (questionSearch.Count() != 1) return ErrorView("You have not created such question.");
            var question = questionSearch.Single();
            var quizId = question.QuizId;
            question.Quiz.ModificationTime = DateTime.Now;
            var options = question.Options.ToArray();
            foreach (var o in options) Db.ClosedQuestionOption.Remove(o);
            Db.SaveChanges();
            Db.ClosedQuestion.Remove(question);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        public ActionResult AddAnswer(int? questionId)
        {
            var teacherId = User.Identity.GetUserId();
            var questionSearch = Db.ClosedQuestion.Where(e => e.Id == questionId && e.Quiz.AuthorId == teacherId);
            if (questionSearch.Count() != 1) return ErrorView("You have not created such question.");
            ViewBag.QuizId = questionSearch.Single().QuizId;
            ViewBag.QuestionId = questionId;
            return View(new ClosedQuestionOption());
        }

        [HttpPost]
        public ActionResult AddAnswer(int? questionId, string content, bool? isCorrect)
        {
            var teacherId = User.Identity.GetUserId();
            var questionSearch = Db.ClosedQuestion.Where(e => e.Id == questionId && e.Quiz.AuthorId == teacherId);
            if (questionSearch.Count() != 1) return ErrorView("You have not created such question.");
            var d = LocalizedStrings.Quiz.AddAnswer[LanguageCookie.Read(Request.Cookies)];
            var question = questionSearch.Single();
            bool error = false;
            if (string.IsNullOrWhiteSpace(content))
            { ViewBag.ValidationMessage = d["Specify content."]; error = true; }
            else if (!isCorrect.HasValue) { ViewBag.ValidationMessage = d["Specify if answer is correct."]; error = true; }
            if (error)
            {
                ViewBag.QuizId = question.QuizId;
                ViewBag.QuestionId = questionId;
                return View(new ClosedQuestionOption { Content = content, IsCorrect = isCorrect ?? false });
            }
            var option = new ClosedQuestionOption();
            option.ClosedQuestionId = questionId;
            option.Content = content;
            option.IsCorrect = isCorrect.Value;
            Db.ClosedQuestionOption.Add(option);
            Db.SaveChanges();
            var quiz = question.Quiz;
            var quizId = quiz.Id;
            quiz.ModificationTime = DateTime.Now;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        public ActionResult DeleteAnswer(int? answerId)
        {
            var teacherId = User.Identity.GetUserId();
            var optionSearch = Db.ClosedQuestionOption.Where(e => e.Id == answerId && e.ClosedQuestion.Quiz.AuthorId == teacherId);
            if (optionSearch.Count() != 1) return ErrorView("You have not created such answer.");
            var option = optionSearch.Single();
            var quiz = option.ClosedQuestion.Quiz;
            var quizId = quiz.Id;
            quiz.ModificationTime = DateTime.Now;
            Db.ClosedQuestionOption.Remove(option);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        public JsonResult GetUnusedClasses(int? quizId)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorJson("You have not created such quiz.");
            var quiz = quizSearch.Single();
            var grantedClasses = quiz.QuizSharings.Select(e => e.ClassId);
            var nonGrantedClasses = Db.Class.Where(e => grantedClasses.Contains(e.Id) == false);
            if (!quiz.SubjectId.HasValue) return ErrorJson("Quiz belongs to no subject.");
            var subjectId = quiz.SubjectId.Value;
            var subjectHavingClasses = nonGrantedClasses.Where(e => e.TeacherClassSubjects.Select(tcs => tcs.SubjectId).Contains(subjectId));
            var classes = subjectHavingClasses.ToArray();
            var list = new LinkedList<object>();
            foreach (var c in classes)
                list.AddLast(new { Id = c.Id, Year = c.Year, Unit = c.Unit });
            return Json(list);
        }

        public ActionResult AddQuizSharing(int? quizId)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            ViewBag.QuizId = quizId;
            return View(new QuizSharing());
        }

        [HttpPost]
        public ActionResult AddQuizSharing(int? quizId, int? classId, float? gradeWeight)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            var quiz = quizSearch.Single();
            var d = LocalizedStrings.Quiz.AddQuizSharing[LanguageCookie.Read(Request.Cookies)];
            if (!classId.HasValue)
            {
                ViewBag.ValidationMessage = d["Select class."];
                ViewBag.QuizId = quizId;
                return View(new QuizSharing { ClassId = 0, GradeWeight = gradeWeight ?? 0});
            }
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.ClassId == classId && e.TeacherId == teacherId);
            if (tcsSearch.Count() == 0) return ErrorView("You do not teach in such class.");
            if (!gradeWeight.HasValue || gradeWeight < 0)
            {
                ViewBag.ValidationMessage = d["Specify non-negative grade weight."];
                ViewBag.QuizId = quizId;
                return View(new QuizSharing { ClassId = classId.Value, GradeWeight = gradeWeight ?? 0 });
            }
            var quizSharing = new QuizSharing { QuizId = quizId.Value, ClassId = classId.Value, GradeWeight = gradeWeight.Value };
            Db.QuizSharing.Add(quizSharing);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        public ActionResult DeleteSharing(int? quizId, int? classId)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            var quizSharingSearch = Db.QuizSharing.Where(e => e.QuizId == quizId && e.ClassId == classId);
            if (quizSharingSearch.Count() != 1) return ErrorView("You have not created such quiz sharing.");
            var quizSharing = quizSharingSearch.Single();
            Db.QuizSharing.Remove(quizSharing);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = quizId });
        }

        public ActionResult Delete(int? id)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == id && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            Db.QuizSharing.Where(e => e.QuizId == id).DeleteFromQuery();
            Db.ClosedQuestionAnswer.Where(e => e.QuizAttempt.QuizId == id).DeleteFromQuery();
            Db.QuizAttempt.Where(e => e.QuizId == id).DeleteFromQuery();
            Db.ClosedQuestionOption.Where(e => e.ClosedQuestion.QuizId == id).DeleteFromQuery();
            Db.ClosedQuestion.Where(e => e.QuizId == id).DeleteFromQuery();
            quizSearch.DeleteFromQuery();
            // nie używamy Db.SaveChanges, bo DeleteFromQuery usuwa bez pobierania obiektu z bazy danych do kontekstu
            return RedirectToAction("List");
        }

        public ActionResult AttemptList(int? quizId)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            var quizAttempts = Db.QuizAttempt.Where(e => e.QuizId == quizId).ToArray();
            ViewBag.QuizId = quizId;
            return View(quizAttempts);
        }

        [HttpPost]
        public ActionResult AttemptList(int? quizId, IList<string> selectedAttempts)
        {
            var teacherId = User.Identity.GetUserId();
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId && e.AuthorId == teacherId);
            if (quizSearch.Count() != 1) return ErrorView("You have not created such quiz.");
            var d = LocalizedStrings.Quiz.AttemptList[LanguageCookie.Read(Request.Cookies)];
            if (selectedAttempts == null)
            {
                ViewBag.ValidationMessage = d["You have selected no attempts."];
                var quizAttempts = Db.QuizAttempt.Where(e => e.QuizId == quizId).ToArray();
                ViewBag.QuizId = quizId;
                return View(quizAttempts);
            }
            foreach (var attemptId in selectedAttempts)
            {
                const string error = "Such attempt does not exist.";
                if (!int.TryParse(attemptId, out int intAttemptId)) return ErrorView(error);
                var attemptSearch = Db.QuizAttempt.Where(e => e.QuizId == quizId && e.Id == intAttemptId);
                if (attemptSearch.Count() != 1) return ErrorView(error);
                var attempt = attemptSearch.Single();
                var gradeId = attempt.GradeId;
                attempt.GradeId = null;
                Db.SaveChanges();
                Db.Grade.Where(e => e.Id == gradeId).DeleteFromQuery();
                Db.QuizAttempt.Where(e => e.Id == intAttemptId).DeleteFromQuery();
            }
            return RedirectToAction("List");
        }

        public ActionResult AttemptReview(int? attemptId)
        {
            var teacherId = User.Identity.GetUserId();
            var attemptSearch = Db.QuizAttempt.Where(e => e.Id == attemptId && e.Quiz.AuthorId == teacherId);
            if (attemptSearch.Count() != 1) return ErrorView("Such attempt of any of your quizzes does not exist.");
            var attempt = attemptSearch.Single();
            var closedQuestions = attempt.Quiz.ClosedQuestions;
            var selectedAnswers = attempt.ClosedQuestionAnswers.Select(e => e.SelectedOptionId).ToArray();
            var userOptionChecks = new LinkedList<bool>();
            var userQuestionPoints = new LinkedList<float>();
            float maxQuizPoints = 0;
            float scoredPoints = 0;
            foreach (var question in closedQuestions)
            {
                int wellCheckedOptions = 0;
                foreach (var option in question.Options)
                {
                    bool userSelected = selectedAnswers.Contains(option.Id);
                    userOptionChecks.AddLast(userSelected);
                    if ((userSelected && option.IsCorrect) || (!userSelected && !option.IsCorrect))
                        ++wellCheckedOptions;
                }
                float questionPoints = question.Points;
                maxQuizPoints += questionPoints;
                float userPoints = wellCheckedOptions * (questionPoints / question.Options.Count);
                scoredPoints += userPoints;
                userQuestionPoints.AddLast(userPoints);
            }
            ViewBag.UserOptionChecks = userOptionChecks;
            ViewBag.UserQuestionPoints = userQuestionPoints;
            ViewBag.MaxQuizPoints = maxQuizPoints;
            ViewBag.ScoredPoints = scoredPoints;
            return View(attempt);
        }
    }
}
