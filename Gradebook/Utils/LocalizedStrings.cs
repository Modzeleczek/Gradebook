using System.Collections.Generic;

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
                d["Lesson number"] = "Lesson number";
                d["Back to student absences"] = "Back to student absences";
                d["Create absence"] = "Create absence";
                d["Date"] = "Date";
                d["Lesson number"] = "Lesson number";
                d["Is justified?"] = "Is justified?";
                d["No absences"] = "No absences";

                d = Create[1]; // polski
                d["Log in"] = "Zaloguj";
                d["Email"] = "Email";
                d["Password"] = "Hasło";
                d["Use login details received from administrator."] = "Użyj danych otrzymanych od administratora";
                d["Lesson number"] = "Numer lekcji";
                d["Back to student absences"] = "Powrót do nieobecności";
                d["Create absence"] = "Dodaj nieobecność";
                d["Date"] = "Data";
                d["Lesson number"] = "Numer lekcji";
                d["Is justified?"] = "Usprawiedliwiona?";
                d["No absences"] = "Brak nieobecności";
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Absences"] = "Absences";
                d["Log in"] = "Log in";
                d["Email"] = "Email";
                d["Password"] = "Password";
                d["Use login details received from administrator."] = "Use login details received from administrator.";
                d["yes"] = "yes";
                d["no"] = "no";
                d["Unjustify"] = "Unjustify";
                d["Justify"] = "Justify";
                d["Add absence"] = "Add absence";
                d["Back to student"] = "Back to student";
                d["Date"] = "Date";
                d["Lesson number"] = "Lesson number";
                d["Is justified?"] = "Is justified?";
                d["No absences"] = "No absences";

                d = Index[1]; // polski
                d["Absences"] = "Nieobecności";
                d["Log in"] = "Zaloguj";
                d["Email"] = "Email";
                d["Password"] = "Hasło";
                d["Use login details received from administrator."] = "Użyj danych otrzymanych od administratora.";
                d["yes"] = "tak";
                d["no"] = "nie";
                d["Unjustify"] = "Cofnij uspawiedliwienie";
                d["Justify"] = "Usprawiedliw";
                d["Add absence"] = "Dodaj nieobecność";
                d["Back to student"] = "Powrót do ucznia";
                d["Date"] = "Data";
                d["Lesson number"] = "Numer lekcji";
                d["Is justified?"] = "Usprawiedliwiona?";
                d["No absences"] = "Brak nieobecności";
            }
        }

        public class Account
        {
            public static Dictionary<string, string>[] Login, VerifyCode, SendCode, EditOther, EditStudent, ConfirmEmail, ForgotPassword, ForgotPasswordConfirmation, Index, LoginDetails, Register, ResetPassword, ResetPasswordConfirmation;

            static Account()
            {
                PLogin();
                PEditOther();
                PEditStudent();
                PConfirmEmail();
                PForgotPassword();
                PForgotPasswordConfirmation();
                PIndex();
                PLoginDetails();
                PRegister();
                PResetPassword();
                PResetPasswordConfirmation();
                PSendCode();
                PVerifyCode();
            }
            private static void PVerifyCode()
            {
                VerifyCode = CreateDictionary();
                var d = VerifyCode[0];
                d["Verify"] = "Verify";
                d["Code"] = "Code";
                d = VerifyCode[1];
                d["Verify"] = "Zweryfikuj";
                d["Code"] = "Kod";
            }
            private static void PSendCode()
            {
                SendCode = CreateDictionary();
                var d = SendCode[0];
                d["Send"] = "Send code";
                d = SendCode[1];
                d["Send"] = "Wyślij kod";
            }
            private static void PResetPasswordConfirmation()
            {
                ResetPasswordConfirmation = CreateDictionary();
                var d = ResetPasswordConfirmation[0];
                d["Reset password confirmation"] = "Reset password confirmation";
                d = ResetPasswordConfirmation[1];
                d["Reset password confirmation"] = "Potwierdzenie resetu hasła";


            }

            private static void PLogin()
            {
                Login = CreateDictionary();
                var d = Login[0];
                d["Log in"] = "Log in";
                d["Email"] = "Email";
                d["Password"] = "Password";
                d["Use login details received from administrator."] = "Use login details received from administrator.";
                d = Login[1];
                d["Log in"] = "Zaloguj";
                d["Email"] = "Email";
                d["Password"] = "Hasło";
                d["Use login details received from administrator."] = "Użyj danych otrzymanych od administratora.";
            }
            private static void PEditOther()
            {
                EditOther = CreateDictionary();
                var d = EditOther[0];
                d["Edit other"] = "Edit";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Phone number"] = "Phone number";
                d["Back"] = "Back";
                d["Save"] = "Save";
                d = EditOther[1];
                d["Edit other"] = "Edytuj";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["Back"] = "Powrót";
                d["Save"] = "Zapisz";
            }
            private static void PEditStudent()
            {
                EditStudent = CreateDictionary();
                var d = EditStudent[0];
                d["Edit student"] = "Edit";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Phone number"] = "Phone number";
                d["Parent"] = "Parent";
                d["Back"] = "Back";
                d["Save"] = "Save";
                d = EditStudent[1];
                d["Edit student"] = "Edytuj";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["Parent"] = "Rodzic";
                d["Back"] = "Powrót";
                d["Save"] = "Zapisz";
            }
            private static void PConfirmEmail()
            {
                ConfirmEmail = CreateDictionary();
                var d = ConfirmEmail[0];
                d["Confirm Email"] = "Confirm Email";
                d["Click here to Log in"] = "Click here to Log in";
                d = ConfirmEmail[1];
                d["Confirm Email"] = "Potwierdź Email";
                d["Click here to Log in"] = "Kliknij tu by się zalogować";
            }
            private static void PForgotPassword()
            {
                ForgotPassword = CreateDictionary();
                var d = ForgotPassword[0];
                d["Forgot your password?"] = "Forgot your password?";
                d = ForgotPassword[1];
                d["Forgot yout password?"] = "Zapomniałeś hasła?";
            }
            private static void PForgotPasswordConfirmation()
            {
                ForgotPasswordConfirmation = CreateDictionary();
                var d = ForgotPasswordConfirmation[0];
                d["Forgot Password Confirmation"] = "Forgot Password Confirmation";
                d = ForgotPasswordConfirmation[1];
                d["Forgot Password Confirmation"] = "Potwierdzenie przypomnienia hasła";
            }
            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Accounts"] = "Accounts";
                d["Create"] = "Create";
                d["Edit"] = "Edit";
                d["Reset Password"] = "Rest Password";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Phone number"] = "Phone number";
                d["Reset password"] = "Reset password";
                d["Administrator"] = "Administrator";
                d["Parent"] = "Parent";
                d["Student"] = "Student";
                d["Teacher"] = "Teacher";
                d["No accounts"] = "No accounts";
                d = Index[1];
                d["Accounts"] = "Konta";
                d["Create"] = "Stwórz";
                d["Edit"] = "Edytuj";
                d["Reset Password"] = "Zresetuj hasło";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["Reset password"] = "Zresetuj hasło";
                d["Administrator"] = "Administrator";
                d["Parent"] = "Rodzic";
                d["Student"] = "Uczeń";
                d["Teacher"] = "Nauczyciel";
                d["No accounts"] = "Brak kont";
            }
            private static void PLoginDetails()
            {
                LoginDetails = CreateDictionary();
                var d = LoginDetails[0];
                d["Login details"] = "Login details";
                d["Back"] = "Back";
                d["Copy username"] = "Copy username";
                d["Copy password"] = "Copy password";
                d["User name"] = "User name";
                d["Generated password"] = "Generated password";
                d = LoginDetails[1];
                d["Login details"] = "Dane logowania";
                d["Back"] = "Powrót";
                d["Copy username"] = "Kopiuj nazwę użytkownika";
                d["Copy password"] = "Kopiuj hasło";
                d["User name"] = "Nazwa użytkownika";
                d["Generated password"] = "Wygenerowane hasło";
            }
            private static void PResetPassword()
            {
                ResetPassword = CreateDictionary();
                var d = ResetPassword[0];
                d["Reset password"] = "Reset password";
                d["Email"] = "Email";
                d["Password"] = "Password";
                d["Confirm Password"] = "Confirm password";
                d = ResetPassword[1];
                d["Reset password"] = "Zresetuj hasło";
                d["Email"] = "Email";
                d["Password"] = "Hasło";
                d["Confirm Password"] = "Zawtwierdź hasło";
            }
            private static void PRegister()
            {
                Register = CreateDictionary();
                var d = Register[0];
                d["Create account"] = "Create account";
                d["Back"] = "Back";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Phone number"] = "Phone number";
                d["RoleName"] = "Role Name";
                d["Create"] = "Create";
                d = Register[1];
                d["Create account"] = "Stwórz konto";
                d["Back"] = "Powrót";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["RoleName"] = "Rodzaj konta";
                d["Create"] = "Stwórz";
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
                d["Absence list"] = "Absence list";
                d["Date"] = "Date";
                d["Lesson number"] = "Lesson number";
                d["Is justified?"] = "Is justified?";
                d["yes"] = "Yes";
                d["no"] = "No";
                d["Back"] = "Back";
                d = AbsenceList[1];
                d["Absence list"] = "Lista nieobecności";
                d["Date"] = "Data";
                d["Lesson number"] = "Numer lekcji";
                d["Is justified?"] = "Czy usprawiedliwiona?";
                d["yes"] = "Tak";
                d["no"] = "Nie";
                d["Back"] = "Back";
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
                d = ClassDetails[1];
                d["Child class details"] = "Informacje o dziecku";
                d["Class"] = "Klasa";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Supervisor"] = "Wychowawca";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Teachers"] = "Nauczyciel";
                d["Email"] = "Email";
                d["Details"] = "Szczegóły";
                d["Subject"] = "Przedmiot";
                d["Back"] = "Powrót";
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
                d = GradeList[1];
                d["Grade list"] = "Lista ocen";
                d["No grades"] = "Brak oceny";
                d["Value"] = "Wartość";
                d["Weight"] = "Waga";
                d["Comment"] = "Komentarz";
                d["Subject"] = "Przedmiot";
                d["Surname"] = "Nazwisko";
                d["Teacher name"] = "Imię nauczyciela";
                d["Teacher surname"] = "Nazwisko nauczyciela";
                d["Details"] = "Szczegóły";
                d["Delete"] = "Usuń";
                d["Add grade"] = "Dodaj ocenę";
                d["Back to student"] = "Wróć do ucznia";
                d["Back"] = "Powrót";
                d["No subjects"] = "Brak przedmiotów";
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
                d = Index[1];
                d["Children"] = "Dzieci";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Show"] = "Pokaż";
                d["Show class"] = "Pokaż klasę";
                d["Show grades"] = "Pokaż oceny";
                d["Show absences"] = "Pokaż nieobecności";
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
                d = StudentDetails[1];
                d["Child student details"] = "Informacje o dziecku";
                d["Parent"] = "Rodzic";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["No parent"] = "Brak rodziców";
                d["Class"] = "Klasa";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["No class"] = "Brak klasy ";
                d["Back"] = "Powrót";
            }
        }

        public class Class
        {
            public static Dictionary<string, string>[] CreateAnnouncement, Details, Edit, Index, Create;

            static Class()
            {
                PCreateAnnouncement();
                PDetails();
                PEdit();
                PIndex();
                PCreate();
            }
            private static void PCreate()
            {
                Create = CreateDictionary();
                var d = Create[0];
                d["Create class"] = "Create class";
                d["Supervisor"] = "Supervisor";
                d["Year"] = "Year";
                d["Unit"] = "Unit";
                d["Create"] = "Create";
                d["All teachers are already supervisors."] = "All teachers are already supervisors.";
                d["Back"] = "Back";
                d = Create[1];
                d["Create class"] = "Stwórz klasę";
                d["Supervisor"] = "Wychowawca";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Create"] = "Stwórz";
                d["All teachers are already supervisors."] = "Wszyscy nauczyciele są już wychowawcami.";
                d["Back"] = "Powrót";
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
                d = CreateAnnouncement[1];
                d["Create announcement"] = "Stwórz ogłoszenie";
                d["Content"] = "Zawartość";
                d["Attachment"] = "Załącznik";
                d["Send"] = "Wyślij";
                d["Back"] = "Powrót";
                d["Send by website"] = "Wyślij przez stronę";
                d["Send by email"] = "Wyślij przez email";
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
                d = Details[1];
                d["Class details"] = "Szczegóły klasy";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Supervisor"] = "Wychowawca";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Students"] = "Uczniowie";
                d["Email"] = "Email";
                d["Teachers"] = "Nauczyciele";
                d["Details"] = "Szczegóły";
                d["Subject"] = "Przedmiot";
                d["Back"] = "Powrót";
                d["Send announcement to parents"] = "Wyślij ogłoszenie do rodziców";
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
                d = Edit[1];
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
                d["Back"] = "Powrót";
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
                d["Create"] = "Create";
                d["Edit"] = "Edit";
                d["Delete"] = "Delete";
                d = Index[1];
                d["Classes"] = "Klasy";
                d["Supervisor name"] = "Imię wychowawcy";
                d["Supervisor surname"] = "Nazwisko wychowawcy";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Details"] = "Szczegóły";
                d["Create"] = "Stwórz";
                d["Edit"] = "Edytuj";
                d["Delete"] = "Usuń";
            }
        }

        public class GlobalAnnouncement
        {
            public static Dictionary<string, string>[] Details, Index, Create, Delete, Edit;

            static GlobalAnnouncement()
            {
                PDetails();
                PIndex();
                PCreate();
                PDelete();
                PEdit();
            }
            private static void PEdit()
            {
                Edit = CreateDictionary();
                var d = Edit[0];
                d["Edit announcement"] = "Edit announcement";
                d["Edit"] = "Edit";
                d["Back"] = "Back";
                d = Edit[1];
                d["Edit announcement"] = "Edytuj ogłoszenie";
                d["Edit"] = "Edytuj";
                d["Back"] = "Powrót";
            }
            private static void PDelete()
            {
                Delete = CreateDictionary();
                var d = Delete[0];
                d["Delete"] = "Delete";
                d["Are you sure you want to delete this?"] = "Are you sure you want to delete this ?";
                d["Modification Time"] = "Modification Time";
                d["Content"] = "Content";
                d["Back to List"] = "Back to list";
                d["Global announcement"] = "Global announcement";
                d = Delete[1];
                d["Delete"] = "Usuń";
                d["Are you sure you want to delete this?"] = "Na pewno chcesz usunąć ogłoszenie?";
                d["Modification Time"] = "Czas modyfikacji";
                d["Content"] = "Zawartość";
                d["Back to List"] = "Powrót";
                d["Global announcement"] = "Ogłoszenie globalne";
            }
            private static void PCreate()
            {
                Create = CreateDictionary();
                var d = Create[0];
                d["Create announcement"] = "Create announcement";
                d["Content"] = "Content";
                d["Back"] = "Back";
                d = Create[1];
                d["Create announcement"] = "Stwórz ogłoszenie";
                d["Content"] = "Zawartość";
                d["Back"] = "Powrót";
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
                d = Details[1];
                d["Announcement details"] = "Szczegóły ogłoszenia";
                d["Author name"] = "Imię autora";
                d["Author surname"] = "Nazwisko autora";
                d["Modification time"] = "Data modyfikacji";
                d["Content"] = "Zawartość";
                d["Back"] = "Porwót";
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
                d["No announcements"] = "No announcements";
                d["Create new"] = "Create new";
                d = Index[1];
                d["Announcements"] = "Ogłoszenia";
                d["Modification time"] = "Data modyfikacji";
                d["Content"] = "Zawartość";
                d["Edit"] = "Edytuj";
                d["Details"] = "Szczegóły";
                d["Delete"] = "Usuń";
                d["No announcements"] = "Brak ogłoszeń";
                d["Create new"] = "Stwórz";
            }
        }

        public class Grade
        {
            public static Dictionary<string, string>[] Index, Create;

            static Grade()
            {
                PIndex();
                PCreate();
            }

            private static void PCreate()
            {
                Create = CreateDictionary();
                var d = Create[0];
                d["Create a grade"] = "Create a grade";
                d["Subject"] = "Subject";
                d["Comment"] = "Comment";
                d["Weight"] = "Weight";
                d["Value"] = "Value";
                d = Create[1];
                d["Create a grade"] = "Dodaj ocenę";
                d["Subject"] = "Przedmiot";
                d["Comment"] = "Komentarz";
                d["Weight"] = "Waga";
                d["Value"] = "Wartość";
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
                d = Index[1];
                d["Grades"] = "Oceny";
                d["No grades"] = "Brak ocen";
                d["Value"] = "Wartość";
                d["Weight"] = "Waga";
                d["Comment"] = "Komentarz";
                d["Subject"] = "Przedmiot";
                d["Surname"] = "Nazwisko";
                d["Teacher name"] = "Imię nauczyciela";
                d["Teacher surname"] = "Nazwisko nauczyciela";
                d["Details"] = "Szczegóły";
                d["Delete"] = "Usuń";
                d["Add grade"] = "Dodaj ocenę";
                d["Back to student"] = "Powrót do ucznia";
                d["No subjects"] = "Brak przedmiotów";
                d["Modification time"] = "";

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
                d["No recipients"] = "No recipients";
                d = Create[1];
                d["Create message"] = "Stwórz wiadomość";
                d["Content"] = "Zawartość";
                d["Attachment"] = "Załącznik";
                d["Recipients"] = "Odbiorcy";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Delete"] = "Usuń";
                d["Add recipient"] = "Dodaj odbiorcę";
                d["Send"] = "Wyślij";
                d["Back"] = "Powrót";
                d["No recipients"] = "Brak odbiorców";
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
                d = Details[1];
                d["Message details"] = "Szczegóły wiadomości";
                d["Received"] = "Otrzyamno";
                d["Content"] = "Zawartość";
                d["Sender name"] = "Imię adresata";
                d["Sender surname"] = "Nazwisko adresata";
                d["Sender email"] = "Email adresata";
                d["Attachment"] = "Załącznik";
                d["Sent"] = "Wysłano";
                d["Recipients"] = "Odbiorcy";
                d["Back"] = "Powrót";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
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
                d["Write new"] = "Write new message";
                d["No received messages"] = "No received messages";
                d["No sent messages"] = "No sent messages";
                d = Index[1];
                d["Messages"] = "Wiadomości";
                d["Received"] = "Otrzymano";
                d["Time"] = "Czas";
                d["Content"] = "Zawartość";
                d["Sender name"] = "Imię adresata";
                d["Sender surname"] = "Nazwisko adresata";
                d["Sender email"] = "Email adresata";
                d["Details"] = "Szczegóły";
                d["Sent"] = "Wysłano";
                d["Write new"] = "Napisz nową wiadomość";
                d["No received messages"] = "Brak otrzymanych wiadomości";
                d["No sent messages"] = "Brak wysłanych wiadomości";
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
                d["Back"] = "Back";

                d = Create[1];
                d["Create quiz"] = "Stwórz quiz";
                d["Subject"] = "Przedmiot";
                d["Quiz"] = "Quiz";
                d["Name"] = "Nazwa";
                d["Duration [s]"] = "Czas trwania [s]";
                d["Back"] = "Powrót";
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
                d = Details[1];
                d["Student details"] = "Szczegóły ucznia";
                d["Back to class"] = "Powrót do klasy";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["No parent"] = "Brak rodziców";
                d["Parent"] = "Rodzic";
                d["Class"] = "Klasa";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["No class"] = "Brak klasy";
                d["Show grades"] = "Pokaż oceny";
                d["Show absences"] = "Pokważ nieobecności";
            }
        }

        public class Subject
        {
            public static Dictionary<string, string>[] Index, AddFile, Create, Details, Edit;

            static Subject()
            {
                PIndex();
                PAddFile();
                PCreate();
                PDetails();
                PEdit();
            }
            private static void PEdit()
            {
                Edit = CreateDictionary();
                var d = Edit[0];
                d["Edit subject"] = "Edit subject";
                d["Download current syllabus"] = "Download current syllabus";
                d["Delete current syllabus"] = "Delete current syllabus";
                d["Back"] = "Back";
                d["Name"] = "Name";
                d["Syllabus was not uploaded."] = "Syllabus was not uploaded.";
                d["Save"] = "Save";
                d = Edit[1];
                d["Edit subject"] = "Edytuj przedmiot";
                d["Download current syllabus"] = "Poberz aktualny sylabus";
                d["Delete current syllabus"] = "Usuń aktualny sylabus";
                d["Back"] = "Powrót";
                d["Name"] = "Nazwa";
                d["Syllabus was not uploaded."] = "Syllabus nie został przesłany.";
                d["Save"] = "Zapisz";
            }
            private static void PDetails()
            {
                Details = CreateDictionary();
                var d = Details[0];
                d["Subject details"] = "Subject details";
                d["Download"] = "Download";
                d["Name"] = "Name";
                d["Delete"] = "Delete";
                d["Syllabus"] = "Syllabus";
                d["Syllabus was not uploaded."] = "Syllabus was not uploaded.";
                d["No files."] = "No files.";
                d["Add file"] = "Add file";
                d["Back"] = "Back";
                d["Files"] = "Files";
                d["Description"] = "Description";
                d = Details[1];
                d["Subject details"] = "Szczegóły przedmiotu ";
                d["Download"] = "Pobierz";
                d["Name"] = "Nazwa";
                d["Delete"] = "Usuń";
                d["Syllabus"] = "Syllabus";
                d["Syllabus was not uploaded."] = "Syllabus nie został przesłany.";
                d["No files."] = "Brak plików.";
                d["Add file"] = "Dodaj plik";
                d["Back"] = "Powrót";
                d["Files"] = "Pliki";
                d["Description"] = "Opis";
            }
            private static void PCreate()
            {
                Create = CreateDictionary();
                var d = Create[0];
                d["Create Subject"] = "Create subject";
                d["Name"] = "Name";
                d["Back"] = "Back";
                d["Create"] = "Create";
                d = Create[1];
                d["Create Subject"] = "Stwórz przedmiot";
                d["Name"] = "Nazwa";
                d["Back"] = "Powrót";
                d["Create"] = "Stwórz";
            }

            private static void PAddFile()
            {
                AddFile = CreateDictionary();
                var d = AddFile[0];
                d["Add file"] = "Add file";
                d["Description"] = "Description";
                d["Back"] = "Back";
                d = AddFile[1];
                d["Add file"] = "Dodaj plik";
                d["Description"] = "Opis";
                d["Back"] = "Powrót";
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
                d["Create"] = "Create";
                d["Syllabus was not uploaded."] = "Syllabus was not uploaded.";
                d = Index[1];
                d["Subjects"] = "Przedmioty";
                d["Name"] = "Imię";
                d["Syllabus"] = "Sylabus";
                d["Edit"] = "Edytuj";
                d["Details"] = "Szczegóły";
                d["Delete"] = "Usuń";
                d["Create"] = "Stwórz";
                d["Syllabus was not uploaded."] = "Syllabus nie został przesłany.";
            }
        }

        public class Layout
        {
            public static Dictionary<string, string>[] Links, Login;

            static Layout()
            {
                PLinks();
                PLogin();
            }

            private static void PLinks()
            {
                Links = CreateDictionary();
                var d = Links[0];
                d["Announcements"] = "Announcements";
                d["Accounts"] = "Accounts";
                d["Subjects"] = "Subjects";
                d["Classes"] = "Classes";
                d["Grades"] = "Grades";
                d["Messages"] = "Messages";
                d["Quizzes"] = "Quizzes";
                d["Children"] = "Children";
                d["Absences"] = "Absences";

                d = Links[1];
                d["Announcements"] = "Ogłoszenia";
                d["Accounts"] = "Konta";
                d["Subjects"] = "Przedmioty";
                d["Classes"] = "Klasy";
                d["Grades"] = "Oceny";
                d["Messages"] = "Wiadomości";
                d["Quizzes"] = "Quizy";
                d["Children"] = "Dzieci";
                d["Absences"] = "Nieobecności";
            }

            private static void PLogin()
            {
                Login = CreateDictionary();
                var d = Login[0];
                d["Log in"] = "Log in";
                d["Logged as"] = "Logged as";
                d["Logout"] = "Logout";

                d = Login[1];
                d["Log in"] = "Zaloguj";
                d["Logged as"] = "Zalogowano jako";
                d["Logout"] = "Wyloguj";
            }
        }

        public class Manage
        {
            public static Dictionary<string, string>[] Index, ChangePassword;

            static Manage()
            {
                PIndex();
                PChangePassword();
            }

            private static void PIndex()
            {
                Index = CreateDictionary();
                var d = Index[0];
                d["Manage"] = "Manage";
                d["Change"] = "Change";
                d["Password"] = "Password";
                d["Create"] = "Create";

                d = Index[1];
                d["Manage"] = "Zarządzanie";
                d["Change"] = "Zmień";
                d["Password"] = "Hasło";
                d["Create"] = "Stwórz";
            }

            private static void PChangePassword()
            {
                ChangePassword = CreateDictionary();
                var d = ChangePassword[0];
                d["Change password"] = "Change password";
                d["Old password"] = "Old password";
                d["New password"] = "New password";
                d["Confirm password"] = "Confirm password";

                d = ChangePassword[1];
                d["Change password"] = "Zmień hasło";
                d["Old password"] = "Stare hasło";
                d["New password"] = "Nowe hasło";
                d["Confirm password"] = "Potwierdź hasło";
            }
        }
    }
}
