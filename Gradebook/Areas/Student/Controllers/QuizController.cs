using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Student.Controllers
{
    [Authorize(Roles = Role.Student), ViewFilter]
    public class QuizController : ControllerBase
    {
        public ActionResult List()
        {
            var studentId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var student = studentSearch.Single();
            if (student.ClassId == null) return ErrorView("You do not belong to any class.");
            var quizzes = student.Class.QuizSharings.Select(e => e.Quiz).ToArray();
            var attemptIds = new int[quizzes.Length];
            for (int i = 0; i < quizzes.Length; ++i)
            {
                var attemptSearch = quizzes[i].QuizAttempts.Where(e => e.DoerId == studentId);
                if (attemptSearch.Count() == 1) attemptIds[i] = attemptSearch.Single().Id;
                else attemptIds[i] = -1;
            }
            ViewBag.AttemptIds = attemptIds;
            return View(quizzes);
        }

        public ActionResult Do(int? quizId) // start rozwiązywania
        {
            var studentId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var student = studentSearch.Single();
            if (student.ClassId == null) return ErrorView("You do not belong to any class.");
            var quizSearch = Db.Quiz.Where(e => e.Id == quizId);
            if (quizSearch.Count() != 1) return ErrorView("Such quiz does not exist.");
            var quiz = quizSearch.Single();
            var studentClassId = student.ClassId;
            if (!quiz.QuizSharings.Any(e => e.ClassId == studentClassId))
                return ErrorView("Such quiz has not been shared with your class.");
            if (!Db.ClosedQuestion.Any(e => e.QuizId == quizId))
                return ErrorView("This quiz does not contain any questions.");
            var classId = student.ClassId;
            var now = DateTime.Now;
            var attempts = quiz.QuizAttempts.Where(e => e.DoerId == studentId);
            if (attempts.Any()) // zaczęto już podejście
            {
                var existingAttempt = attempts.Single();
                if (existingAttempt.Finish > existingAttempt.Start) // jedyne podejście zostało już zakończone
                    return ErrorView("You have already done this quiz.");
                // jedyne podejście nie zostało jeszcze zakończone
                ViewBag.AttemptId = existingAttempt.Id;
                // teraz - początek podejścia = ile już minęło; pozostały czas = czas trwania quizu - ile już minęło 
                ViewBag.RemainingTime = Math.Truncate((TimeSpan.FromSeconds(quiz.Duration) - now.Subtract(existingAttempt.Start)).TotalSeconds);
            }
            else // jeszcze nie zaczęto żadnego podejścia
            {
                var newAttempt = new QuizAttempt { QuizId = quizId, DoerId = studentId, Start = now,
                    Finish = DateTime.MinValue };
                Db.QuizAttempt.Add(newAttempt);
                Db.SaveChanges();
                var attemptId = newAttempt.Id;
                ViewBag.AttemptId = attemptId;
                ViewBag.RemainingTime = Math.Truncate(TimeSpan.FromSeconds(quiz.Duration).TotalSeconds);
            }
            return View(quiz);
            // return RedirectToAction("AnswerQuestion", new { attemptId = quizAttempt.Id, questionId = orderedQuestions.Single().Id });
        }

        [HttpPost]
        public ActionResult Do(int? attemptId, IList<string> selectedAnswers)
        {
            var studentId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var student = studentSearch.Single();
            var attemptSearch = student.QuizAttempts.Where(e => e.Id == attemptId);
            if (attemptSearch.Count() != 1) return ErrorView("You have not done such quiz attempt.");
            var attempt = attemptSearch.Single();
            if (attempt.Finish <= attempt.Start)
            {
                attempt.Finish = DateTime.Now;
                if (EvaluateAttempt(attempt, selectedAnswers) == 1) // quiz nie jest przypisany do przedmiotu, więc nie wstawiamy oceny
                    return RedirectToAction("List", "Quiz");
                return RedirectToAction("List", "Grade");
            }
            else return ErrorView("This quiz attempt has already been evaluated.");
        }

        private int EvaluateAttempt(QuizAttempt attempt, IList<string> selectedAnswers)
        {
            if (selectedAnswers == null) selectedAnswers = new List<string>();
            var closedQuestions = attempt.Quiz.ClosedQuestions;
            float maxQuizPoints = 0;
            float scoredPoints = 0;
            var attemptId = attempt.Id;
            foreach (var question in closedQuestions)
            {
                int wellCheckedOptions = 0;
                foreach (var option in question.Options)
                {
                    bool userChecked = selectedAnswers.Contains(option.Id.ToString());
                    if (userChecked)
                        Db.ClosedQuestionAnswer.Add(
                            new ClosedQuestionAnswer { QuizAttemptId = attemptId, SelectedOptionId = option.Id });
                    if ((userChecked && option.IsCorrect) || (!userChecked && !option.IsCorrect))
                        ++wellCheckedOptions;
                }
                float questionPoints = question.Points;
                maxQuizPoints += questionPoints;
                scoredPoints += wellCheckedOptions * (questionPoints / question.Options.Count);
            }
            Db.SaveChanges();
            var grade = new Grade();
            var quiz = attempt.Quiz;
            if (!quiz.SubjectId.HasValue) return 1;
            grade.StudentId = attempt.DoerId;
            grade.SubjectId = quiz.SubjectId.Value;
            grade.TeacherId = quiz.AuthorId;
            var quizSharing = attempt.Doer.Class.QuizSharings.Where(e => e.QuizId == quiz.Id).Single();
            grade.Weight = quizSharing.GradeWeight;
            grade.Comment = quiz.Name;
            float percent = (float)scoredPoints / maxQuizPoints;
            grade.Value = GradeFromPercent(percent * 100);
            grade.ModificationTime = DateTime.Now;
            Db.Grade.Add(grade);
            Db.SaveChanges();
            attempt.GradeId = grade.Id;
            Db.SaveChanges();
            return 0;
        }

        public ActionResult AttemptReview(int? attemptId)
        {
            var studentId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var student = studentSearch.Single();
            var attemptSearch = student.QuizAttempts.Where(e => e.Id == attemptId);
            if (attemptSearch.Count() != 1) return ErrorView("You have not done such quiz attempt.");
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
    }
}
