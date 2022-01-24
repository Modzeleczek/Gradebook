using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gradebook.Controllers
{
    public partial class QuizController : Controller
    {
        // GET
        [Authorize(Roles = Role.Student)]
        public ActionResult Do(int quizId) // start rozwiązywania
        {
            ViewBag.Error = "";
            if (Db.ClosedQuestion.Where(e => e.QuizId == quizId).Any() == false)
            {
                ViewBag.Error = "Quiz does not contain any questions.";
                return View();
            }
            var studentId = User.Identity.GetUserId();
            var classId = Db.Student.Where(e => e.Id == studentId).Single().ClassId;
            if (Db.QuizSharing.Where(e => e.ClassId == classId).Any() == false)
            {
                ViewBag.Error = "Your class does not have access to this quiz.";
                return View();
            }
            var now = DateTime.Now;
            var quiz = Db.Quiz.Where(e => e.Id == quizId).Single();
            var attempts = Db.QuizAttempt.Where(e => e.QuizId == quizId && e.DoerId == studentId);
            if (attempts.Any()) // zaczęto już podejście
            {
                var existingAttempt = attempts.Single();
                if (existingAttempt.Finish > existingAttempt.Start) // jedyne podejście zostało już zakończone
                {
                    ViewBag.Error = "You have already done this quiz.";
                    return View();
                }
                // jedyne podejście nie zostało jeszcze zakończone
                ViewBag.AttemptId = existingAttempt.Id;
                // teraz - początek podejścia = ile już minęło; pozostały czas = czas trwania quizu - ile już minęło 
                ViewBag.RemainingTime = Math.Truncate((TimeSpan.FromSeconds(quiz.Duration) - now.Subtract(existingAttempt.Start)).TotalSeconds);
            }
            else // jeszcze nie zaczęto żadnego podejścia
            {
                var quizAttempt = new QuizAttempt { QuizId = quizId, DoerId = studentId, Start = now };
                Db.QuizAttempt.Add(quizAttempt);
                Db.SaveChanges();
                var attemptId = quizAttempt.Id;
                ViewBag.AttemptId = attemptId;
                // SetTimer(quiz.Duration, attemptId);
                ViewBag.RemainingTime = Math.Truncate(TimeSpan.FromSeconds(quiz.Duration).TotalSeconds);
            }
            return View(quiz);
            // return RedirectToAction("AnswerQuestion", new { attemptId = quizAttempt.Id, questionId = orderedQuestions.Single().Id });
        }

        private void SetTimer(int seconds, int attemptId)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = (seconds + 10) * 1000; // opóźnienie, żeby zdążyło się wykonać EvaluateAttempt wywołane w POST Do wysłane przez JSowy timer
            timer.Elapsed += (sender, eventArgs) =>
            {
                var attempt = Db.QuizAttempt.Where(e => e.Id == attemptId).Single();
                if (attempt.Finish <= attempt.Start)
                {
                    attempt.Finish = eventArgs.SignalTime;
                    EvaluateAttempt(attempt, null);
                }
            };
            timer.AutoReset = false;
            timer.Start();
        }

        [HttpPost]
        public ActionResult Do(int attemptId, IList<string> selectedAnswers)
        {
            var attempt = Db.QuizAttempt.Where(e => e.Id == attemptId).Single();
            if (attempt.Finish <= attempt.Start)
            {
                attempt.Finish = DateTime.Now;
                EvaluateAttempt(attempt, selectedAnswers);
                return RedirectToAction("Index", "Grade");
            }
            return RedirectToAction("Index");
        }

        private void EvaluateAttempt(QuizAttempt attempt, IList<string> selectedAnswers)
        {
            if (selectedAnswers == null)
                selectedAnswers = new List<string>();
            // var attempt = Db.QuizAttempt.Where(e => e.Id == attemptId).Single();
            var closedQuestions = attempt.Quiz.ClosedQuestions;
            int usersCorrectQuestions = 0;
            foreach (var question in closedQuestions)
            {
                bool questionCorrect = true;
                foreach (var option in question.Options)
                {
                    var userSelected = selectedAnswers.Contains(option.Id.ToString());
                    if ((userSelected && !option.IsCorrect) || (!userSelected && option.IsCorrect))
                    {
                        questionCorrect = false;
                        break;
                    }
                }
                if (questionCorrect)
                    ++usersCorrectQuestions;
            }
            var grade = new Grade();
            grade.StudentId = attempt.DoerId;
            var quiz = attempt.Quiz;
            grade.SubjectId = quiz.SubjectId ?? -1;
            grade.TeacherId = quiz.AuthorId;
            var quizSharing = attempt.Doer.Class.QuizSharings.Where(e => e.QuizId == quiz.Id).Single();
            grade.Weight = quizSharing.GradeWeight;
            grade.Comment = quiz.Name;
            double percent = (double)usersCorrectQuestions / closedQuestions.Count;
            grade.Value = GradeFromPercent(percent * 100);
            grade.ModificationTime = DateTime.Now;
            Db.Grade.Add(grade);
            Db.SaveChanges();
        }

        private float GradeFromPercent(double percent)
        {
            if (percent <= 30) return 1F;
            if (percent <= 34) return 1.5F;
            if (percent <= 39) return 1.75F;
            if (percent <= 49) return 2F;
            if (percent <= 54) return 2.5F;
            if (percent <= 59) return 2.75F;
            if (percent <= 64) return 3F;
            if (percent <= 69) return 3.5F;
            if (percent <= 74) return 3.75F;
            if (percent <= 80) return 4F;
            if (percent <= 85) return 4.5F;
            if (percent <= 89) return 4.75F;
            if (percent <= 94) return 5F;
            if (percent <= 96) return 5.5F;
            if (percent <= 98) return 5.75F;
            return 6F;
        }

        /* private bool HasAccess()
        {

        }

        private bool IsTimeUp(int attemptId)
        {
            var attempt = Db.QuizAttempt.Where(e => e.Id == attemptId).Single();
            var start = attempt.Start;
            return DateTime.Now.Subtract(start).TotalSeconds > attempt.Quiz.Duration;
            // DateTime.Now > Db.QuizSharing.Where(e => attempt.Doer.ClassId
        }

        // GET
        [Authorize(Roles = Role.Student)]
        public ActionResult AnswerQuestion(int attemptId, int questionId) // wyświetlenie pytania
        {
            
        } */
    }
}