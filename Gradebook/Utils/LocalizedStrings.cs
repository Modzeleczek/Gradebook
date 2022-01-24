using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradebook.Utils
{
    public class LocalizedStrings
    {
        private static Dictionary<string, string>[] CreateDictionary()
        {
            var ret = new Dictionary<string, string>[2]; // 0 - EN, 1 - PL
            ret[0] = new Dictionary<string, string>();
            ret[1] = new Dictionary<string, string>();
            return ret;
        }

        public class Absence
        {
            public static Dictionary<string, string>[] Create, Index;

            static Absence()
            {
                PCreate();
                PIndex();
            }

            private static void PCreate()
            {
                Create = CreateDictionary();
                var d = Create[0];
                d["Log in"] = "Log in";
                d["Email"] = "Email";
                d["Password"] = "Password";
                d["Use login details received from administrator."] = "Use login details received from administrator.";

                d = Create[1]; // polski
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Log in"] = "Log in";
                d["Email"] = "Email";
                d["Password"] = "Password";
                d["Use login details received from administrator."] = "Use login details received from administrator.";

                d = Index[1]; // polski
            }
        }

        public class Account
        {
            public static Dictionary<string, string>[] Login;

            static Account()
            {
                PLogin();
            }

            private static void PLogin()
            {
                Login = CreateDictionary();
                var d = Login[0];
                d["Log in"] = "Log in";
                d["Email"] = "Email";
                d["Password"] = "Password";
                d["Use login details received from administrator."] = "Use login details received from administrator.";
            }
        }

        public class Child
        {
            public static Dictionary<string, string>[] AbsenceList, ClassDetails, GradeList, Index, StudentDetails;
            
            static Child()
            {
                PAbsenceList();
                PClassDetails();
                PGradeList();
                PIndex();
                PStudentDetails();
            }

            private static void PAbsenceList()
            {
                AbsenceList = CreateDictionary();
                var d = AbsenceList[0];
                
            }

            private static void PClassDetails()
            {
                ClassDetails = CreateDictionary();
                var d = ClassDetails[0];
                d["Child class details"] = "Child class details";
                d["Class"] = "Class";
                d["Year"] = "Year";
                d["Unit"] = "Unit";
                d["Supervisor"] = "Supervisor";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Teachers"] = "Teachers";
                d["Email"] = "Email";
                d["Details"] = "Details";
                d["Subject"] = "Subject";
                d["Back"] = "Back";
            }

            private static void PGradeList()
            {
                GradeList = CreateDictionary();
                var d = GradeList[0];
                d["Grade list"] = "Grade list";
                d["No grades"] = "No grades";
                d["Value"] = "Value";
                d["Weight"] = "Weight";
                d["Comment"] = "Comment";
                d["Subject"] = "Subject";
                d["Surname"] = "Surname";
                d["Teacher name"] = "Teacher name";
                d["Teacher surname"] = "Teacher surname";
                d["Details"] = "Details";
                d["Delete"] = "Delete";
                d["Add grade"] = "Add grade";
                d["Back to student"] = "Back to student";
                d["Back"] = "Back";
                d["No subjects"] = "No subjects";
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Children"] = "Children";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Show"] = "Show";
                d["Show class"] = "Show class";
                d["Show grades"] = "Show grades";
                d["Show absences"] = "Show absences";
            }

            private static void PStudentDetails()
            {
                StudentDetails = CreateDictionary();
                var d = StudentDetails[0];
                d["Child student details"] = "Child student details";
                d["Parent"] = "Parent";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Phone number"] = "Phone number";
                d["No parent"] = "No parent";
                d["Class"] = "Class";
                d["Year"] = "Year";
                d["Unit"] = "Unit";
                d["No class"] = "No class";
                d["Back"] = "Back";
            }
        }

        public class Class
        {
            public static Dictionary<string, string>[] CreateAnnouncement, Details, Edit, Index;
            
            static Class()
            {
                PCreateAnnouncement();
                PDetails();
                PEdit();
                PIndex();
            }

            private static void PCreateAnnouncement()
            {
                CreateAnnouncement = CreateDictionary();
                var d = CreateAnnouncement[0];
                d["Create announcement"] = "Create announcement";
                d["Content"] = "Content";
                d["Attachment"] = "Attachment";
                d["Send"] = "Send";
                d["Back"] = "Back";
                d["Send by website"] = "Send by website";
                d["Send by email"] = "Send by email";
            }

            private static void PDetails()
            {
                Details = CreateDictionary();
                var d = Details[0];
                d["Class details"] = "Class details";
                d["Year"] = "Year";
                d["Unit"] = "Unit";
                d["Supervisor"] = "Supervisor";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Students"] = "Students";
                d["Email"] = "Email";
                d["Teachers"] = "Teachers";
                d["Details"] = "Details";
                d["Subject"] = "Subject";
                d["Back"] = "Back";
                d["Send announcement to parents"] = "Send announcement to parents";
            }

            private static void PEdit()
            {
                Edit = CreateDictionary();
                var d = Edit[0];
                d["Edit class"] = "Edit class";
                d["Back"] = "Back";
                d["Supervisor"] = "Supervisor";
                d["Year"] = "Year";
                d["Unit"] = "Unit";
                d["Save"] = "Save";
                d["Students"] = "Students";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Delete"] = "Delete";
                d["Student"] = "Student";
                d["Add student to class"] = "Add student to class";
                d["Teachers"] = "Teachers";
                d["Subject name"] = "Subject name";
                d["Teacher"] = "Teacher";
                d["Subject"] = "Subject";
                d["Add teacher to class"] = "Add teacher to class";
                d["Back"] = "Back";

                /* d = Edit[1];
                d["Edit class"] = "Edytuj klasę";
                d["Class"] = "Klasa";
                d["Back"] = "Powrót";
                d["Supervisor"] = "Wychowawca";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Save"] = "Zapisz";
                d["Students"] = "Uczniowie";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Delete"] = "Usuń";
                d["Student"] = "Uczeń";
                d["Add student to class"] = "Dodaj ucznia do klasy";
                d["Teachers"] = "Nauczyciele";
                d["Subject name"] = "Nazwa";
                d["Teacher"] = "Nauczyciel";
                d["Subject"] = "Przedmiot";
                d["Add teacher to class"] = "Dodaj nauczyciela do klasy";
                d["Back"] = "Powrót"; */
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Classes"] = "Classes";
                d["Supervisor name"] = "Supervisor name";
                d["Supervisor surname"] = "Supervisor surname";
                d["Year"] = "Year";
                d["Unit"] = "Unit";
                d["Details"] = "Details";
            }
        }

        public class GlobalAnnouncement
        {
            public static Dictionary<string, string>[] Details, Index;

            static GlobalAnnouncement()
            {
                PDetails();
                PIndex();
            }

            private static void PDetails()
            {
                Details = CreateDictionary();
                var d = Details[0];
                d["Announcement details"] = "Announcement details";
                d["Author name"] = "Author name";
                d["Author surname"] = "Author surname";
                d["Modification time"] = "Modification time";
                d["Content"] = "Content";
                d["Back"] = "Back";
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Announcements"] = "Announcements";
                d["Modification time"] = "Modification time";
                d["Content"] = "Content";
                d["Edit"] = "Edit";
                d["Details"] = "Details";
                d["Delete"] = "Delete";
            }
        }

        public class Grade
        {
            public static Dictionary<string, string>[] Index;

            static Grade()
            {
                PIndex();
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Grades"] = "Grades";
                d["No grades"] = "No grades";
                d["Value"] = "Value";
                d["Weight"] = "Weight";
                d["Comment"] = "Comment";
                d["Subject"] = "Subject";
                d["Surname"] = "Surname";
                d["Teacher name"] = "Teacher name";
                d["Teacher surname"] = "Teacher surname";
                d["Details"] = "Details";
                d["Delete"] = "Delete";
                d["Add grade"] = "Add grade";
                d["Back to student"] = "Back to student";
                d["No subjects"] = "No subjects";
                d["Modification time"] = "Modification time";
            }
        }

        public class Message
        {
            public static Dictionary<string, string>[] Create, Details, Index;

            static Message()
            {
                PCreate();
                PDetails();
                PIndex();
            }

            private static void PCreate()
            {
                Create = CreateDictionary();
                var d = Create[0];
                d["Create message"] = "Create message";
                d["Content"] = "Content";
                d["Attachment"] = "Attachment";
                d["Recipients"] = "Recipients";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Delete"] = "Delete";
                d["Add recipient"] = "Add recipient";
                d["Send"] = "Send";
                d["Back"] = "Back";
            }

            private static void PDetails()
            {
                Details = CreateDictionary();
                var d = Details[0];
                d["Message details"] = "Message details";
                d["Received"] = "Received";
                d["Content"] = "Content";
                d["Sender name"] = "Sender name";
                d["Sender surname"] = "Sender surname";
                d["Sender email"] = "Sender email";
                d["Attachment"] = "Attachment";
                d["Sent"] = "Sent";
                d["Recipients"] = "Recipients";
                d["Back"] = "Back";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Messages"] = "Messages";
                d["Received"] = "Received";
                d["Time"] = "Time";
                d["Content"] = "Content";
                d["Sender name"] = "Sender name";
                d["Sender surname"] = "Sender surname";
                d["Sender email"] = "Sender email";
                d["Details"] = "Details";
                d["Sent"] = "Sent";
                d["Write new"] = "Write new";
            }
        }

        public class Quiz
        {
            public static Dictionary<string, string>[] Index, Create, Edit, AddAnswer, AddQuestion, AddQuizSharing, Do;
            
            static Quiz()
            {
                PIndex();
                PCreate();
                PEdit();
                PAddAnswer();
                PAddQuestion();
                PAddQuizSharing();
                PDo();
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Quizzes"] = "Quizzes";
                d["Create quiz"] = "Create quiz";
                d["Subject"] = "Subject";
                d["Name"] = "Name";
                d["Duration"] = "Duration [s]";
                d["Max attempts"] = "Max attempts";
                d["Modification time"] = "Modification time";
                d["Open from"] = "Open from";
                d["Open to"] = "Open to";
                d["Edit"] = "Edit";
                d["Details"] = "Details";
                d["Delete"] = "Delete";
                d["Do"] = "Do";

                d = Index[1];
                d["Quizzes"] = "Quizy";
                d["Create quiz"] = "Stwórz quiz";
                d["Subject"] = "Przedmiot";
                d["Name"] = "Nazwa";
                d["Duration"] = "Czas trwania [s]";
                d["Max attempts"] = "Maksymalna liczba podejść";
                d["Modification time"] = "Czas modyfikacji";
                d["Open from"] = "Otwarty od";
                d["Open to"] = "Otwarty do";
                d["Edit"] = "Edytuj";
                d["Details"] = "Szczegóły";
                d["Delete"] = "Usuń";
                d["Do"] = "Podejdź";
            }

            private static void PCreate()
            {
                Create = CreateDictionary();
                var d = Create[0];
                d["Create quiz"] = "Create quiz";
                d["Subject"] = "Subject";
                d["Quiz"] = "Quiz";
                d["Name"] = "Name";
                d["Duration [s]"] = "Duration [s]";

                d = Create[1];
                d["Create quiz"] = "Stwórz quiz";
                d["Subject"] = "Przedmiot";
                d["Quiz"] = "Quiz";
                d["Name"] = "Nazwa";
                d["Duration [s]"] = "Czas trwania [s]";
            }

            private static void PEdit()
            {
                Edit = CreateDictionary();
                var d = Edit[0];
                d["Edit quiz"] = "Edit quiz";
                d["Quiz"] = "Quiz";
                d["Back"] = "Back";
                d["Subject"] = "Subject";
                d["Name"] = "Name";
                d["Duration [s]"] = "Duration [s]";
                d["Save"] = "Save";
                d["Questions"] = "Questions";
                d["Content"] = "Content";
                d["Answers"] = "Answers";
                d["correct"] = "correct";
                d["incorrect"] = "incorrect";
                d["Delete answer"] = "Delete answer";
                d["Add answer"] = "Add answer";
                d["Delete question"] = "Delete question";
                d["Add question"] = "Add question";
                d["Sharings"] = "Sharings";
                d["Year"] = "Year";
                d["Unit"] = "Unit";
                d["Grade weight"] = "Grade weight";
                d["Delete"] = "Delete";
                d["Grant access"] = "Give access";

                d = Edit[1];
                d["Edit quiz"] = "Edytuj quiz";
                d["Quiz"] = "Quiz";
                d["Back"] = "Powrót";
                d["Subject"] = "Przedmiot";
                d["Name"] = "Nazwa";
                d["Duration [s]"] = "Czas trwania [s]";
                d["Save"] = "Zapisz";
                d["Questions"] = "Pytania";
                d["Content"] = "Treść";
                d["Answers"] = "Odpowiedzi";
                d["correct"] = "prawidłowa";
                d["incorrect"] = "nieprawidłowa";
                d["Delete answer"] = "Usuń odpowiedź";
                d["Add answer"] = "Dodaj odpowiedź";
                d["Delete question"] = "Usuń pytanie";
                d["Add question"] = "Dodaj pytanie";
                d["Sharings"] = "Udostępnienia";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Grade weight"] = "Waga oceny";
                d["Delete"] = "Usuń";
                d["Grant access"] = "Przyznaj dostęp";
            }

            private static void PAddAnswer()
            {
                AddAnswer = CreateDictionary();
                var d = AddAnswer[0];
                d["Add answer"] = "Add answer";
                d["Content"] = "Content";
                d["Correct?"] = "Correct?";
                d["Add"] = "Add";
                d["Back"] = "Back";

                d = AddAnswer[1];
                d["Add answer"] = "Dodaj odpowiedź";
                d["Content"] = "Treść";
                d["Correct?"] = "Prawidłowa?";
                d["Add"] = "Dodaj";
                d["Back"] = "Powrót";
            }

            private static void PAddQuestion()
            {
                AddQuestion = CreateDictionary();
                var d = AddQuestion[0];
                d["Add question"] = "Add question";
                d["Content"] = "Content";
                d["Add"] = "Add";
                d["Back"] = "Back";

                d = AddQuestion[1];
                d["Add question"] = "Dodaj pytanie";
                d["Content"] = "Treść";
                d["Add"] = "Dodaj";
                d["Back"] = "Powrót";
            }

            private static void PAddQuizSharing()
            {
                AddQuizSharing = CreateDictionary();
                var d = AddQuizSharing[0];
                d["Grant access"] = "Give access";
                d["Class"] = "Class";
                d["Grade weight"] = "Grade weight";
                d["Back"] = "Back";

                d = AddQuizSharing[1];
                d["Grant access"] = "Przyznaj dostęp";
                d["Class"] = "Klasa";
                d["Grade weight"] = "Waga oceny";
                d["Back"] = "Powrót";
            }

            private static void PDo()
            {
                Do = CreateDictionary();
                var d = Do[0];
                d["Quiz attempt"] = "Quiz attempt";
                d["Back"] = "Back";
                d["Time remaining"] = "Time remaining";
                d["Content"] = "Content";
                d["Answers"] = "Answers";
                d["Submit"] = "Submit";
                d["Your time has expired."] = "Your time has expired.";

                d = Do[1];
                d["Quiz attempt"] = "Podejście do quizu";
                d["Back"] = "Powrót";
                d["Time remaining"] = "Pozostały czas";
                d["Content"] = "Treść";
                d["Answers"] = "Odpowiedzi";
                d["Submit"] = "Zakończ";
                d["Your time has expired."] = "Czas minął.";
            }
        }

        public class Student
        {
            public static Dictionary<string, string>[] Details;

            static Student()
            {
                PDetails();
            }

            private static void PDetails()
            {
                Details = CreateDictionary();
                var d = Details[0];
                d["Student details"] = "Student details";
                d["Back to class"] = "Back to class";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Phone number"] = "Phone number";
                d["No parent"] = "No parent";
                d["Parent"] = "Parent";
                d["Class"] = "Class";
                d["Year"] = "Year";
                d["Unit"] = "Unit";
                d["No class"] = "No class";
                d["Show grades"] = "Show grades";
                d["Show absences"] = "Show absences";
            }
        }

        public class Subject
        {
            public static Dictionary<string, string>[] Index;

            static Subject()
            {
                PIndex();
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Subjects"] = "Subjects";
                d["Name"] = "Name";
                d["Syllabus"] = "Syllabus";
                d["Edit"] = "Edit";
                d["Details"] = "Details";
                d["Delete"] = "Delete";
            }
        }
    }
}