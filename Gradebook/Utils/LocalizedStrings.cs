using System.Collections.Generic;

namespace Gradebook.Utils
{
    public class LocalizedStrings
    {
        public abstract class LanguageDictionary
        {
            public abstract string this[string key] { get; set; }
        }

        public class RealDictionary : LanguageDictionary
        {
            private Dictionary<string, string> Dict = new Dictionary<string, string>();
            public override string this[string key]
            {
                get
                {
                    if (Dict.ContainsKey(key)) return Dict[key];
                    else return key;
                }
                set { Dict[key] = value; }
            }
        }

        public class FakeDictionary : LanguageDictionary // fałszywy słownik, który zwraca wartość równą podanemu kluczowi
        {
            public override string this[string key]
            {
                get { return key; }
                set { }
            }
        }

        public class DictionaryTuple
        {
            private LanguageDictionary[] Dictionaries;
            public DictionaryTuple()
            {
                Dictionaries = new LanguageDictionary[2]; // 0 - EN, 1 - PL
                Dictionaries[0] = new FakeDictionary();
                Dictionaries[1] = new RealDictionary();
            }

            public LanguageDictionary this[int languageId]
            {
                get { return Dictionaries[languageId]; }
            }
        }

        public class Absence
        {
            public static DictionaryTuple Create, Index;

            static Absence()
            {
                PCreate();
                PIndex();
            }

            private static void PCreate()
            {
                Create = new DictionaryTuple();
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
                Index = new DictionaryTuple();
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
            public static DictionaryTuple Login, VerifyCode, SendCode, EditOther, EditStudent, ConfirmEmail, ForgotPassword, ForgotPasswordConfirmation, Index, LoginDetails, Register, ResetPassword, ResetPasswordConfirmation;

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
                VerifyCode = new DictionaryTuple();
                var d = VerifyCode[0];
                d["Verify"] = "Verify";
                d["Code"] = "Code";
                d = VerifyCode[1];
                d["Verify"] = "Zweryfikuj";
                d["Code"] = "Kod";
            }
            private static void PSendCode()
            {
                SendCode = new DictionaryTuple();
                var d = SendCode[0];
                d["Send"] = "Send code";
                d = SendCode[1];
                d["Send"] = "Wyślij kod";
            }
            private static void PResetPasswordConfirmation()
            {
                ResetPasswordConfirmation = new DictionaryTuple();
                var d = ResetPasswordConfirmation[0];
                d["Reset password confirmation"] = "Reset password confirmation";
                d["Your password has been reset. Please "] = "Your password has been reset. Please ";
                d["click here to log in"] = "click here to log in";
                d = ResetPasswordConfirmation[1];
                d["Reset password confirmation"] = "Potwierdzenie resetu hasła";
                d["Your password has been reset. Please "] = "Twoje hasło zostało zresetowane. ";
                d["click here to log in"] = "Kliknij tutaj, aby się zalogować.";
            }

            private static void PLogin()
            {
                Login = new DictionaryTuple();
                var d = Login[0];
                d["Log in"] = "Log in";
                d["Email"] = "Email";
                d["Password"] = "Password";
                d["Use login details received from administrator."] = "Use login details received from administrator.";
                d["Forgot your password?"] = "Forgot your password?";
                d = Login[1];
                d["Log in"] = "Zaloguj";
                d["Email"] = "Email";
                d["Password"] = "Hasło";
                d["Use login details received from administrator."] = "Użyj danych otrzymanych od administratora.";
                d["Forgot your password?"] = "Zapomniałeś hasła?";
            }
            private static void PEditOther()
            {
                EditOther = new DictionaryTuple();
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
                EditStudent = new DictionaryTuple();
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
                ConfirmEmail = new DictionaryTuple();
                var d = ConfirmEmail[0];
                d["Confirm Email"] = "Confirm Email";
                d["Click here to Log in"] = "Click here to Log in";
                d = ConfirmEmail[1];
                d["Confirm Email"] = "Potwierdź Email";
                d["Click here to Log in"] = "Kliknij tu by się zalogować";
            }
            private static void PForgotPassword()
            {
                ForgotPassword = new DictionaryTuple();
                var d = ForgotPassword[0];
                d["Forgot password"] = "Forgot password";
                d["Enter your email."] = "Enter your email.";
                d["Email"] = "Email";
                d["Email link"] = "Email link";
                d["Reset password"] = "Reset password";
                d["Please reset your password by clicking <a href=\""] = "Please reset your password by clicking <a href=\"";
                d["\">here</a>"] = "\">here</a>";
                d = ForgotPassword[1];
                d["Forgot password"] = "Zapomniałem hasła";
                d["Enter your email."] = "Podaj swój adres email";
                d["Email"] = "Email";
                d["Email link"] = "Wyślij link emailem";
                d["Reset password"] = "Zresetuj hasło";
                d["Please reset your password by clicking <a href=\""] = "Zresetuj hasło klikając <a href=\"";
                d["\">here</a>"] = "\">tutaj</a>";
            }
            private static void PForgotPasswordConfirmation()
            {
                ForgotPasswordConfirmation = new DictionaryTuple();
                var d = ForgotPasswordConfirmation[0];
                d["Forgot password confirmation"] = "Forgot password confirmation";
                d["Please check your email to reset your password."] = "Please check your email to reset your password.";
                d = ForgotPasswordConfirmation[1];
                d["Forgot password confirmation"] = "Potwierdzenie przypomnienia hasła";
                d["Please check your email to reset your password."] = "Sprawdź email, aby zresetować hasło.";
            }
            private static void PIndex()
            {
                Index = new DictionaryTuple();
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
                LoginDetails = new DictionaryTuple();
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
                ResetPassword = new DictionaryTuple();
                var d = ResetPassword[0];
                d["Reset password"] = "Reset password";
                d["Reset your password"] = "Reset your password";
                // d["Email"] = "Email";
                d["New password"] = "New password";
                d["Confirm new password"] = "Confirm new password";
                d["Reset"] = "Reset";
                d = ResetPassword[1];
                d["Reset password"] = "Zresetuj hasło";
                d["Reset your password"] = "Zresetuj hasło";
                // d["Email"] = "Email";
                d["New password"] = "Nowe hasło";
                d["Confirm new password"] = "Potwierdź nowe hasło";
                d["Reset"] = "Zresetuj";
            }
            private static void PRegister()
            {
                Register = new DictionaryTuple();
                var d = Register[0];
                d["Create account"] = "Create account";
                d["Back"] = "Back";
                d["Name"] = "Name";
                d["Surname"] = "Surname";
                d["Email"] = "Email";
                d["Phone number"] = "Phone number";
                d["Role name"] = "Role name";
                d["Create"] = "Create";
                d = Register[1];
                d["Create account"] = "Stwórz konto";
                d["Back"] = "Powrót";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["Role name"] = "Rodzaj konta";
                d["Create"] = "Stwórz";
            }
        }

        public class Appointment
        {
            public static DictionaryTuple List_, Details;

            static Appointment()
            {
                PList();
                PDetails();
            }

            private static void PList()
            {
                List_ = new DictionaryTuple();
                var d = List_[1];
                d["Appointments"] = "Terminarz";
                d["Previous week"] = "Poprzedni tydzień";
                d["Next week"] = "Następny tydzień";
                d["monday"] = "poniedziałek";
                d["tuesday"] = "wtorek";
                d["wednesday"] = "środa";
                d["thursday"] = "czwartek";
                d["friday"] = "piątek";
                d["No appointments"] = "Brak zdarzeń";
                d["Delete"] = "Usuń";
            }

            private static void PDetails()
            {
                Details = new DictionaryTuple();
                var d = Details[1];
                d["Appointments"] = "Terminarz";
                d["Appointment details"] = "Szczegóły zdarzenia";
                d["Name"] = "Nazwa";
                d["Description"] = "Opis";
                d["Date"] = "Data";
                d["Class"] = "Klasa";
                d["Teacher"] = "Nauczyciel";
                d["Subject"] = "Przedmiot";
                d["Back"] = "Powrót";
            }
        }

        public class Child
        {
            public static DictionaryTuple AbsenceList, ClassDetails, GradeList, Index, StudentDetails;

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
                AbsenceList = new DictionaryTuple();
                var d = AbsenceList[0];
                d["Absence list"] = "Absence list";
                d["Date"] = "Date";
                d["Lesson number"] = "Lesson number";
                d["Is justified?"] = "Is justified?";
                d["yes"] = "Yes";
                d["no"] = "No";
                d["Back"] = "Back";
                d["No absences"] = "No absences";
                d = AbsenceList[1];
                d["Absence list"] = "Lista nieobecności";
                d["Date"] = "Data";
                d["Lesson number"] = "Numer lekcji";
                d["Is justified?"] = "Czy usprawiedliwiona?";
                d["yes"] = "Tak";
                d["no"] = "Nie";
                d["Back"] = "Powrót";
                d["No absences"] = "Brak nieobecności";
            }

            private static void PClassDetails()
            {
                ClassDetails = new DictionaryTuple();
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
                GradeList = new DictionaryTuple();
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
                d["Back to student"] = "Wróć do ucznia";
                d["Back"] = "Powrót";
                d["No subjects"] = "Brak przedmiotów";
            }

            private static void PIndex()
            {
                Index = new DictionaryTuple();
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
                StudentDetails = new DictionaryTuple();
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
            public static DictionaryTuple CreateAnnouncement, Details, Edit, List_, Create, CreateAppointment, GenerateGradeSheet;

            static Class()
            {
                PCreateAnnouncement();
                PDetails();
                PEdit();
                PList();
                PCreate();
                PCreateAppointment();
                PGenerateGradeSheet();
            }

            private static void PCreate()
            {
                Create = new DictionaryTuple();
                var d = Create[1];
                d["Create class"] = "Stwórz klasę";
                d["Supervisor"] = "Wychowawca";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Create"] = "Stwórz";
                d["All teachers are already supervisors."] = "Wszyscy nauczyciele są już wychowawcami.";
                d["Back"] = "Powrót";
                d["Refresh"] = "Odśwież";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Unit must be a single letter."] = "Oddział musi być pojedynczą literą.";
                d["Year must be positive integer."] = "Rok musi być dodatnią liczbą całkowitą.";
                d["Teacher does not exist."] = "Nauczyciel nie istnieje.";
                d["Teacher is already supervisor."] = "Nauczyciel już jest wychowawcą.";
            }

            private static void PCreateAnnouncement()
            {
                CreateAnnouncement = new DictionaryTuple();
                var d = CreateAnnouncement[1];
                d["Send announcement"] = "Wyślij ogłoszenie";
                d["Content"] = "Treść";
                d["Attachment"] = "Załącznik";
                d["Send"] = "Wyślij";
                d["Back"] = "Powrót";
                d["Send by website"] = "Wyślij przez stronę";
                d["Send by email"] = "Wyślij przez email";
                d["Specify content."] = "Podaj treść.";
            }

            private static void PDetails()
            {
                Details = new DictionaryTuple();
                var d = Details[1];
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
                d["Generate grade sheet"] = "Wygeneruj zestawienie ocen";
                d["Create appointment"] = "Dodaj zdarzenie";
            }

            private static void PEdit()
            {
                Edit = new DictionaryTuple();
                var d = Edit[1];
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
                d["Lessons"] = "Lekcje";
                d["Day"] = "Dzień";
                d["Time"] = "Godzina";
                d["Duration (min)"] = "Czas trwania (min)";
                d["Subject"] = "Przedmiot";
                d["Teacher"] = "Nauczyciel";
                d["Add lesson to class"] = "Dodaj lekcję do klasy";
                d["Teacher and subject"] = "Nauczyciel i przedmiot";
                d["Day and time"] = "Dzień i godzina";
                d["monday"] = "poniedziałek";
                d["tuesday"] = "wtorek";
                d["wednesday"] = "środa";
                d["thursday"] = "czwartek";
                d["friday"] = "piątek";
            }

            private static void PList()
            {
                List_ = new DictionaryTuple();
                var d = List_[1];
                d["Classes"] = "Klasy";
                d["Supervisor name"] = "Imię wychowawcy";
                d["Supervisor surname"] = "Nazwisko wychowawcy";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Details"] = "Szczegóły";
                d["Add"] = "Dodaj";
                d["Edit"] = "Edytuj";
                d["Delete"] = "Usuń";
                d["No classes"] = "Brak klas";
            }

            private static void PCreateAppointment()
            {
                CreateAppointment = new DictionaryTuple();
                var d = CreateAppointment[1];
                d["Create appointment"] = "Stwórz zdarzenie";
                d["Name"] = "Nazwa";
                d["Description"] = "Opis";
                d["Date"] = "Data";
                d["Create"] = "Stwórz";
                d["Back"] = "Powrót";
                d["Specify name."] = "Podaj nazwę.";
                d["Specify date."] = "Podaj datę.";
            }

            private static void PGenerateGradeSheet()
            {
                GenerateGradeSheet = new DictionaryTuple();
                var d = GenerateGradeSheet[1];
                d["Generate grade sheet"] = "Wygeneruj zestawienie ocen";
                d["Generate"] = "Wygeneruj";
                d["Grade sheet for class"] = "Zestawienie ocen klasy";
                d["You did not select any students and subjects."] = "Nie wybrałeś żadnych uczniów i przedmiotów.";
                d["No grades"] = "Brak ocen";
                d["Weight"] = "Waga";
                d["Comment"] = "Komentarz";
                d["Modification time"] = "Czas modyfikacji";
                d["Teacher"] = "Nauczyciel";
                d["No students were selected."] = "Nie wybrano żadnych uczniów.";
                d["Back to class"] = "Powrót do klasy";
            }
        }

        public class GlobalAnnouncement
        {
            public static DictionaryTuple Details, Index, Create, Delete, Edit;

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
                Edit = new DictionaryTuple();
                var d = Edit[1];
                d["Edit announcement"] = "Edytuj ogłoszenie";
                d["Edit"] = "Edytuj";
                d["Back"] = "Powrót";
                d["Specify content."] = "Podaj treść.";
                d["Save"] = "Zapisz";
                d["Content"] = "Treść";
            }
            private static void PDelete()
            {
                Delete = new DictionaryTuple();
                var d = Delete[1];
                d["Delete"] = "Usuń";
                d["Are you sure you want to delete this?"] = "Na pewno chcesz usunąć ogłoszenie?";
                d["Modification Time"] = "Czas modyfikacji";
                d["Content"] = "Treść";
                d["Back to List"] = "Powrót";
                d["Global announcement"] = "Ogłoszenie globalne";
            }
            private static void PCreate()
            {
                Create = new DictionaryTuple();
                var d = Create[1];
                d["Create announcement"] = "Stwórz ogłoszenie";
                d["Content"] = "Treść";
                d["Back"] = "Powrót";
                d["Specify content."] = "Podaj treść.";
                d["Create"] = "Stwórz";
            }

            private static void PDetails()
            {
                Details = new DictionaryTuple();
                var d = Details[1];
                d["Announcement details"] = "Szczegóły ogłoszenia";
                d["Author name"] = "Imię autora";
                d["Author surname"] = "Nazwisko autora";
                d["Modification time"] = "Data modyfikacji";
                d["Content"] = "Treść";
                d["Back"] = "Powrót";
            }

            private static void PIndex()
            {
                Index = new DictionaryTuple();
                var d = Index[1];
                d["Announcements"] = "Ogłoszenia";
                d["Modification time"] = "Data modyfikacji";
                d["Content"] = "Treść";
                d["Edit"] = "Edytuj";
                d["Details"] = "Szczegóły";
                d["Delete"] = "Usuń";
                d["No announcements"] = "Brak ogłoszeń";
                d["Add"] = "Dodaj";
            }
        }

        public class Grade
        {
            public static DictionaryTuple Index, Create;

            static Grade()
            {
                PIndex();
                PCreate();
            }

            private static void PCreate()
            {
                Create = new DictionaryTuple();
                var d = Create[0];
                d["Create a grade"] = "Create a grade";
                d["Subject"] = "Subject";
                d["Comment"] = "Comment";
                d["Weight"] = "Weight";
                d["Value"] = "Value";
                d["Back to student grades"] = "Back to student grades";
                d = Create[1];
                d["Create a grade"] = "Dodaj ocenę";
                d["Subject"] = "Przedmiot";
                d["Comment"] = "Komentarz";
                d["Weight"] = "Waga";
                d["Value"] = "Wartość";
                d["Back to student grades"] = "Powrót do ocen ucznia";
            }

            private static void PIndex()
            {
                Index = new DictionaryTuple();
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
            public static DictionaryTuple Create, Details, Index;

            static Message()
            {
                PCreate();
                PDetails();
                PIndex();
            }

            private static void PCreate()
            {
                Create = new DictionaryTuple();
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
                d["Content"] = "Treść";
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
                Details = new DictionaryTuple();
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
                d["Content"] = "Treść";
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
                Index = new DictionaryTuple();
                var d = Index[1];
                d["Messages"] = "Wiadomości";
                d["Received"] = "Otrzymane";
                d["Time"] = "Czas";
                d["Content"] = "Treść";
                d["Sender name"] = "Imię adresata";
                d["Sender surname"] = "Nazwisko adresata";
                d["Sender email"] = "Email adresata";
                d["Details"] = "Szczegóły";
                d["Sent"] = "Wysłane";
                d["Write new"] = "Napisz nową wiadomość";
                d["No received messages"] = "Brak otrzymanych wiadomości";
                d["No sent messages"] = "Brak wysłanych wiadomości";
            }
        }

        public class Quiz
        {
            public static DictionaryTuple Index, Create, Edit, AddAnswer, AddQuestion, AddQuizSharing, Do;

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
                Index = new DictionaryTuple();
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
                d["No quizzes"] = "Brak quizów";
            }

            private static void PCreate()
            {
                Create = new DictionaryTuple();
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
                Edit = new DictionaryTuple();
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
                AddAnswer = new DictionaryTuple();
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
                AddQuestion = new DictionaryTuple();
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
                AddQuizSharing = new DictionaryTuple();
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
                Do = new DictionaryTuple();
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
            public static DictionaryTuple Details;

            static Student()
            {
                PDetails();
            }

            private static void PDetails()
            {
                Details = new DictionaryTuple();
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
                d["Show absences"] = "Pokaż nieobecności";
            }
        }

        public class Subject
        {
            public static DictionaryTuple List_, AddFile, Create, Details, Edit;

            static Subject()
            {
                PList();
                PAddFile();
                PCreate();
                PDetails();
                PEdit();
            }
            private static void PEdit()
            {
                Edit = new DictionaryTuple();
                var d = Edit[1];
                d["Edit subject"] = "Edytuj przedmiot";
                d["Download syllabus"] = "Poberz sylabus";
                d["Delete syllabus"] = "Usuń sylabus";
                d["Back"] = "Powrót";
                d["Name"] = "Nazwa";
                d["Syllabus was not uploaded."] = "Sylabus nie został przesłany.";
                d["Save"] = "Zapisz";
                d["Specify a name."] = "Podaj nazwę.";
                d["Subject does not exist."] = "Przedmiot nie istnieje.";
                d["Syllabus"] = "Sylabus";
            }
            private static void PDetails()
            {
                Details = new DictionaryTuple();
                var d = Details[1];
                d["Subject details"] = "Szczegóły przedmiotu ";
                d["Download"] = "Pobierz";
                d["Name"] = "Nazwa";
                d["Delete"] = "Usuń";
                d["Syllabus"] = "Sylabus";
                d["Syllabus was not uploaded."] = "Sylabus nie został przesłany.";
                d["No files"] = "Brak plików";
                d["Add file"] = "Dodaj plik";
                d["Back"] = "Powrót";
                d["Files"] = "Pliki";
                d["Description"] = "Opis";
                d["Modification time"] = "Czas modyfikacji";
            }
            private static void PCreate()
            {
                Create = new DictionaryTuple();
                var d = Create[1];
                d["Create subject"] = "Stwórz przedmiot";
                d["Name"] = "Nazwa";
                d["Back"] = "Powrót";
                d["Create"] = "Stwórz";
                d["Specify a name."] = "Podaj nazwę.";
                d["Select a PDF file."] = "Wybierz plik PDF.";
                d["Syllabus"] = "Sylabus";
            }
            private static void PAddFile()
            {
                AddFile = new DictionaryTuple();
                var d = AddFile[1];
                d["Add file"] = "Dodaj plik";
                d["Description"] = "Opis";
                d["Back"] = "Powrót";
                d["Add"] = "Dodaj";
                d["Upload a TXT, PDF or ZIP file."] = "Prześlij plik TXT, PDF lub ZIP.";
            }
            private static void PList()
            {
                List_ = new DictionaryTuple();
                var d = List_[1];
                d["Subjects"] = "Przedmioty";
                d["Name"] = "Nazwa";
                d["Syllabus"] = "Sylabus";
                d["Edit"] = "Edytuj";
                d["Details"] = "Szczegóły";
                d["Delete"] = "Usuń";
                d["Add"] = "Dodaj";
                d["Syllabus was not uploaded."] = "Sylabus nie został przesłany.";
                d["Download"] = "Pobierz";
            }
        }

        public class Layout
        {
            public static DictionaryTuple _Layout, Login;

            static Layout()
            {
                P_Layout();
                PLogin();
            }

            private static void P_Layout()
            {
                _Layout = new DictionaryTuple();
                var d = _Layout[0];
                d["Announcements"] = "Announcements";
                d["Accounts"] = "Accounts";
                d["Subjects"] = "Subjects";
                d["Classes"] = "Classes";
                d["Grades"] = "Grades";
                d["Messages"] = "Messages";
                d["Quizzes"] = "Quizzes";
                d["Children"] = "Children";
                d["Absences"] = "Absences";
                d["Timetable"] = "Timetable";
                d = _Layout[1];
                d["Announcements"] = "Ogłoszenia";
                d["Accounts"] = "Konta";
                d["Subjects"] = "Przedmioty";
                d["Classes"] = "Klasy";
                d["Grades"] = "Oceny";
                d["Messages"] = "Wiadomości";
                d["Quizzes"] = "Quizy";
                d["Children"] = "Dzieci";
                d["Absences"] = "Nieobecności";
                d["Language"] = "Język";
                d["Timetable"] = "Plan lekcji";
                d["Appointments"] = "Terminarz";
            }

            private static void PLogin()
            {
                Login = new DictionaryTuple();
                var d = Login[1];
                d["Log in"] = "Zaloguj";
                d["Logged as"] = "Zalogowano jako";
                d["Logout"] = "Wyloguj";
            }
        }

        public class Manage
        {
            public static DictionaryTuple Index, ChangePassword;

            static Manage()
            {
                PIndex();
                PChangePassword();
            }

            private static void PIndex()
            {
                Index = new DictionaryTuple();
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
                ChangePassword = new DictionaryTuple();
                var d = ChangePassword[0];
                d["Change password"] = "Change password";
                d["Current password"] = "Current password";
                d["New password"] = "New password";
                d["Confirm password"] = "Confirm password";

                d = ChangePassword[1];
                d["Change password"] = "Zmień hasło";
                d["Current password"] = "Aktualne hasło";
                d["New password"] = "Nowe hasło";
                d["Confirm password"] = "Potwierdź hasło";
            }
        }

        public class Timetable
        {
            public static DictionaryTuple Index;

            static Timetable()
            {
                PIndex();
            }

            private static void PIndex()
            {
                Index = new DictionaryTuple();
                var d = Index[1];
                d["Timetable"] = "Plan lekcji";
                d["monday"] = "poniedziałek";
                d["tuesday"] = "wtorek";
                d["wednesday"] = "środa";
                d["thursday"] = "czwartek";
                d["friday"] = "piątek";
            }
        }

        public class GenericErrorView
        {
            public static DictionaryTuple GenericError;

            static GenericErrorView()
            {
                PGenericError();
            }

            private static void PGenericError()
            {
                GenericError = new DictionaryTuple();
                var d = GenericError[1];
                d["Error"] = "Błąd";
                d["Subject does not exist."] = "Przedmiot nie istnieje.";
                d["File does not exist."] = "Plik nie istnieje.";
                d["Announcement does not exist."] = "Ogłoszenie nie istnieje.";
                d["You do not teach such subject."] = "Nie uczysz takiego przedmiotu.";
                d["You do not own such file."] = "Nie posiadasz takiego pliku.";
                d["Your class does not have such subject."] = "Twoja klasa nie ma takiego przedmiotu.";
                d["Your class does not have access to such file."] = "Twoja klasa nie ma dostępu do takiego pliku.";
                d["Class does not exist."] = "Klasa nie istnieje.";
                d["You are not supervisor of such class."] = "Nie jesteś wychowawcą takiej klasy.";
                d["You do not teach such subject in such class."] = "Nie uczysz takiego przedmiotu w takiej klasie.";
            }
        }
    }
}
