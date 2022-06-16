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
            public static DictionaryTuple List_;

            static Absence()
            {
                PList();
            }

            private static void PList()
            {
                List_ = new DictionaryTuple();
                var d = List_[1];
                d["Absences"] = "Nieobecności";
                d["yes"] = "tak";
                d["no"] = "nie";
                d["Unjustify"] = "Cofnij uspawiedliwienie";
                d["Justify"] = "Usprawiedliw";
                d["Date"] = "Data";
                d["Lesson number"] = "Numer lekcji";
                d["Is justified?"] = "Usprawiedliwiona?";
                d["No absences"] = "Brak nieobecności";
                d["Time"] = "Godzina";
                d["Subject"] = "Przedmiot";
                d["Teacher name"] = "Imię nauczyciela";
                d["Teacher surname"] = "Nazwisko nauczyciela";
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
                var d = VerifyCode[1];
                d["Verify"] = "Zweryfikuj";
                d["Code"] = "Kod";
            }
            private static void PSendCode()
            {
                SendCode = new DictionaryTuple();
                var d = SendCode[1];
                d["Send"] = "Wyślij kod";
            }
            private static void PResetPasswordConfirmation()
            {
                ResetPasswordConfirmation = new DictionaryTuple();
                var d = ResetPasswordConfirmation[1];
                d["Reset password confirmation"] = "Potwierdzenie resetu hasła";
                d["Your password has been reset. Please "] = "Twoje hasło zostało zresetowane. ";
                d["click here to log in"] = "Kliknij tutaj, aby się zalogować.";
            }

            private static void PLogin()
            {
                Login = new DictionaryTuple();
                var d = Login[1];
                d["Log in"] = "Zaloguj";
                d["Email"] = "Email";
                d["Password"] = "Hasło";
                d["Use login details received from administrator."] = "Użyj danych otrzymanych od administratora.";
                d["Forgot your password?"] = "Zapomniałeś hasła?";
            }
            private static void PEditOther()
            {
                EditOther = new DictionaryTuple();
                var d = EditOther[1];
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
                var d = EditStudent[1];
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
                var d = ConfirmEmail[1];
                d["Confirm Email"] = "Potwierdź Email";
                d["Click here to Log in"] = "Kliknij tu by się zalogować";
            }
            private static void PForgotPassword()
            {
                ForgotPassword = new DictionaryTuple();
                var d = ForgotPassword[1];
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
                var d = ForgotPasswordConfirmation[1];
                d["Forgot password confirmation"] = "Potwierdzenie przypomnienia hasła";
                d["Please check your email to reset your password."] = "Sprawdź email, aby zresetować hasło.";
            }
            private static void PIndex()
            {
                Index = new DictionaryTuple();
                var d = Index[1];
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
                var d = LoginDetails[1];
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
                var d = ResetPassword[1];
                d["Reset password"] = "Zresetuj hasło";
                d["Reset your password"] = "Zresetuj hasło";
                d["New password"] = "Nowe hasło";
                d["Confirm new password"] = "Potwierdź nowe hasło";
                d["Reset"] = "Zresetuj";
            }
            private static void PRegister()
            {
                Register = new DictionaryTuple();
                var d = Register[1];
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
                d["Sun"] = "Nie";
                d["Mon"] = "Pon";
                d["Tue"] = "Wto";
                d["Wed"] = "Śro";
                d["Thu"] = "Czw";
                d["Fri"] = "Pią";
                d["Sat"] = "Sob";
                d["January"] = "Styczeń";
                d["February"] = "Luty";
                d["March"] = "Marzec";
                d["April"] = "Kwiecień";
                d["May"] = "Maj";
                d["June"] = "Czerwiec";
                d["July"] = "Lipiec";
                d["August"] = "Sierpień";
                d["September"] = "Wrzesień";
                d["October"] = "Październik";
                d["November"] = "Listopad";
                d["December"] = "Grudzień";
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
                d["Delete"] = "Usuń";
            }
        }

        public class Child
        {
            public static DictionaryTuple AbsenceList, ClassDetails, GradeList, List_, StudentDetails;

            static Child()
            {
                PAbsenceList();
                PClassDetails();
                PGradeList();
                PList();
                PStudentDetails();
            }

            private static void PAbsenceList()
            {
                AbsenceList = new DictionaryTuple();
                var d = AbsenceList[1];
                d["Absence list"] = "Lista nieobecności";
                d["Date"] = "Data";
                d["Lesson number"] = "Numer lekcji";
                d["Is justified?"] = "Czy usprawiedliwiona?";
                d["yes"] = "tak";
                d["no"] = "nie";
                d["Back"] = "Powrót";
                d["No absences"] = "Brak nieobecności";
                d["Subject"] = "Przedmiot";
                d["Teacher name"] = "Imię nauczyciela";
                d["Teacher surname"] = "Nazwisko nauczyciela";
            }

            private static void PClassDetails()
            {
                ClassDetails = new DictionaryTuple();
                var d = ClassDetails[1];
                d["Class details"] = "Szczegóły klasy";
                d["Class"] = "Klasa";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["Supervisor"] = "Wychowawca";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Teachers"] = "Nauczyciele";
                d["Email"] = "Email";
                d["Details"] = "Szczegóły";
                d["Subject"] = "Przedmiot";
                d["Back"] = "Powrót";
                d["No class"] = "Brak klasy";
                d["No teachers"] = "Brak nauczycieli";
            }

            private static void PGradeList()
            {
                GradeList = new DictionaryTuple();
                var d = GradeList[1];
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
                d["No grades"] = "Brak ocen";
            }

            private static void PList()
            {
                List_ = new DictionaryTuple();
                var d = List_[1];
                d["Children"] = "Dzieci";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Show details"] = "Pokaż szczegóły";
                d["Show class"] = "Pokaż klasę";
                d["Show grades"] = "Pokaż oceny";
                d["Show absences"] = "Pokaż nieobecności";
                d["No children"] = "Brak dzieci";
            }

            private static void PStudentDetails()
            {
                StudentDetails = new DictionaryTuple();
                var d = StudentDetails[1];
                d["Student details"] = "Szczegóły ucznia";
                d["Parent"] = "Rodzic";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["No parent"] = "Brak rodzica";
                d["Class"] = "Klasa";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["No class"] = "Brak klasy";
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
                d["No students"] = "Brak uczniów";
                d["No teachers"] = "Brak nauczycieli";
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
                d["Remove"] = "Usuń";
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
                d["Day and time"] = "Dzień i godzina";
                d["Monday"] = "Poniedziałek";
                d["Tuesday"] = "Wtorek";
                d["Wednesday"] = "Środa";
                d["Thursday"] = "Czwartek";
                d["Friday"] = "Piątek";
                d["Room"] = "Sala";
                d["No students"] = "Brak uczniów";
                d["No teachers"] = "Brak nauczycieli";
                d["No lessons"] = "Brak lekcji";
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
                d["Create appointment"] = "Dodaj zdarzenie";
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
                d["Student"] = "Uczeń";
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
                d["Create announcement"] = "Dodaj ogłoszenie";
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
            public static DictionaryTuple List_;

            static Grade()
            {
                PList();
            }

            private static void PList()
            {
                List_ = new DictionaryTuple();
                var d = List_[1];
                d["Grades"] = "Oceny";
                d["No grades"] = "Brak ocen";
                d["Value"] = "Wartość";
                d["Weight"] = "Waga";
                d["Comment"] = "Komentarz";
                d["Subject"] = "Przedmiot";
                d["Teacher name"] = "Imię nauczyciela";
                d["Teacher surname"] = "Nazwisko nauczyciela";
                d["Details"] = "Szczegóły";
                d["Delete"] = "Usuń";
                d["Add grade"] = "Dodaj ocenę";
                d["Back to student"] = "Powrót do ucznia";
                d["No subjects"] = "Brak przedmiotów";
                d["Modification time"] = "Czas modyfikacji";
            }
        }

        public class Message
        {
            public static DictionaryTuple Create, Details, List_;

            static Message()
            {
                PCreate();
                PDetails();
                PList();
            }

            private static void PCreate()
            {
                Create = new DictionaryTuple();
                var d = Create[1];
                d["Create message"] = "Napisz wiadomość";
                d["Content"] = "Treść";
                d["Attachments"] = "Załączniki";
                d["Recipients"] = "Odbiorcy";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Delete"] = "Usuń";
                d["Add recipient"] = "Dodaj odbiorcę";
                d["Send"] = "Wyślij";
                d["Back"] = "Powrót";
                d["No recipients"] = "Brak odbiorców";
                d["Message does not have recipients."] = "Wiadomość nie ma odbiorców.";
                d["Specify content."] = "Podaj treść.";
                d["User"] = "Użytkownik";
                d["Cancel"] = "Anuluj";
                d["Add recipient"] = "Dodaj odbiorcę";
            }

            private static void PDetails()
            {
                Details = new DictionaryTuple();
                var d = Details[1];
                d["Message details"] = "Szczegóły wiadomości";
                d["Time"] = "Czas";
                d["Content"] = "Treść";
                d["Sender name"] = "Imię nadawcy";
                d["Sender surname"] = "Nazwisko nadawcy";
                d["Sender email"] = "Email nadawcy";
                d["Attachments"] = "Załączniki";
                d["Sent"] = "Wysłano";
                d["Recipients"] = "Odbiorcy";
                d["Back"] = "Powrót";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Attachment name"] = "Nazwa";
                d["Type"] = "Typ";
                d["Size (bytes)"] = "Rozmiar (bajty)";
                d["No attachments"] = "Brak załączników";
                d["No recipients"] = "Brak odbiorców";
            }

            private static void PList()
            {
                List_ = new DictionaryTuple();
                var d = List_[1];
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
                var d  = Index[1];
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
                var d = Create[1];
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
                var d = Edit[1];
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
                var d = AddAnswer[1];
                d["Add answer"] = "Dodaj odpowiedź";
                d["Content"] = "Treść";
                d["Correct?"] = "Prawidłowa?";
                d["Add"] = "Dodaj";
                d["Back"] = "Powrót";
            }

            private static void PAddQuestion()
            {
                AddQuestion = new DictionaryTuple();
                var d = AddQuestion[1];
                d["Add question"] = "Dodaj pytanie";
                d["Content"] = "Treść";
                d["Add"] = "Dodaj";
                d["Back"] = "Powrót";
            }

            private static void PAddQuizSharing()
            {
                AddQuizSharing = new DictionaryTuple();
                var d = AddQuizSharing[1];
                d["Grant access"] = "Przyznaj dostęp";
                d["Class"] = "Klasa";
                d["Grade weight"] = "Waga oceny";
                d["Back"] = "Powrót";
            }

            private static void PDo()
            {
                Do = new DictionaryTuple();
                var d = Do[1];
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
            public static DictionaryTuple Details, CreateGrade, CreateAbsence;

            static Student()
            {
                PDetails();
                PCreateGrade();
                PCreateAbsence();
            }

            private static void PDetails()
            {
                Details = new DictionaryTuple();
                var d = Details[1];
                d["Student details"] = "Szczegóły ucznia";
                d["Back to class"] = "Powrót do klasy";
                d["Name"] = "Imię";
                d["Surname"] = "Nazwisko";
                d["Email"] = "Email";
                d["Phone number"] = "Numer telefonu";
                d["No parent"] = "Brak rodzica";
                d["Parent"] = "Rodzic";
                d["Class"] = "Klasa";
                d["Year"] = "Rok";
                d["Unit"] = "Oddział";
                d["No class"] = "Brak klasy";
                d["Grades"] = "Oceny";
                d["No grades"] = "Brak ocen";
                d["Value"] = "Wartość";
                d["Weight"] = "Waga";
                d["Comment"] = "Komentarz";
                d["Teacher name"] = "Imię nauczyciela";
                d["Teacher surname"] = "Nazwisko nauczyciela";
                d["Delete grade"] = "Usuń ocenę";
                d["Add grade"] = "Dodaj ocenę";
                d["No subjects"] = "Brak przedmiotów";
                d["Modification time"] = "Czas modyfikacji";
                d["Absences"] = "Nieobecności";
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
                d["Back"] = "Powrót";
                d["Time"] = "Godzina";
                d["Subject"] = "Przedmiot";
                d["Delete"] = "Usuń";
            }

            private static void PCreateGrade()
            {
                CreateGrade = new DictionaryTuple();
                var d = CreateGrade[1];
                d["Create grade"] = "Dodaj ocenę";
                d["Subject"] = "Przedmiot";
                d["Comment"] = "Komentarz";
                d["Weight"] = "Waga";
                d["Value"] = "Wartość";
                d["Back"] = "Powrót";
                d["Specify value in range <1, 6>."] = "Podaj wartość z zakresu <1, 6>.";
                d["Specify weight."] = "Podaj wagę.";
                d["Select subject."] = "Wybierz przedmiot.";
                d["Create"] = "Stwórz";
            }

            private static void PCreateAbsence()
            {
                CreateAbsence = new DictionaryTuple();
                var d = CreateAbsence[1];
                d["Subject"] = "Przedmiot";
                d["Back"] = "Powrót";
                d["Create absence"] = "Dodaj nieobecność";
                d["Date"] = "Data";
                d["Lesson number"] = "Numer lekcji";
                d["Is justified?"] = "Usprawiedliwiona?";
                d["Create"] = "Stwórz";
                d["Time"] = "Godzina";
                d["Select subject."] = "Wybierz przedmiot.";
                d["Select date."] = "Wybierz datę.";
                d["Select time."] = "Wybierz godzinę.";
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
                d["Such subject does not exist."] = "Taki przedmiot nie istnieje.";
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
                d["No subjects"] = "Brak przedmiotów";
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
                var d = _Layout[1];
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
                d["Class"] = "Klasa";
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
                var d = Index[1];
                d["Manage"] = "Zarządzanie";
                d["Change"] = "Zmień";
                d["Password"] = "Hasło";
                d["Create"] = "Stwórz";
            }

            private static void PChangePassword()
            {
                ChangePassword = new DictionaryTuple();
                var d = ChangePassword[1];
                d["Change password"] = "Zmień hasło";
                d["Current password"] = "Aktualne hasło";
                d["New password"] = "Nowe hasło";
                d["Confirm password"] = "Potwierdź hasło";
            }
        }

        public class Timetable
        {
            public static DictionaryTuple List_;

            static Timetable()
            {
                PList();
            }

            private static void PList()
            {
                List_ = new DictionaryTuple();
                var d = List_[1];
                d["Timetable"] = "Plan lekcji";
                d["Monday"] = "Poniedziałek";
                d["Tuesday"] = "Wtorek";
                d["Wednesday"] = "Środa";
                d["Thursday"] = "Czwartek";
                d["Friday"] = "Piątek";
                d["Time"] = "Godzina";
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
                d["Such subject does not exist."] = "Taki przedmiot nie istnieje.";
                d["File does not exist."] = "Plik nie istnieje.";
                d["Such announcement does not exist."] = "Takie ogłoszenie nie istnieje.";
                d["You do not teach such subject."] = "Nie uczysz takiego przedmiotu.";
                d["You do not own such file."] = "Nie posiadasz takiego pliku.";
                d["Your class does not have such subject."] = "Twoja klasa nie ma takiego przedmiotu.";
                d["Your class does not have access to such file."] = "Twoja klasa nie ma dostępu do takiego pliku.";
                d["Such class does not exist."] = "Taka klasa nie istnieje.";
                d["You are not supervisor of such class."] = "Nie jesteś wychowawcą takiej klasy.";
                d["You do not teach such subject in such class."] = "Nie uczysz takiego przedmiotu w takiej klasie.";
                d["Such student does not exist."] = "Taki uczeń nie istnieje.";
                d["The teacher does not teach such subject in such class."] = "Nauczyciel nie uczy takiego przedmiotu w takiej klasie.";
                d["Such teacher does not exist."] = "Taki nauczyciel nie istnieje.";
                d["Such lesson does not exist."] = "Taka lekcja nie istnieje.";
                d["Such day does not exist."] = "Taki dzień nie istnieje.";
                d["Such room does not exist."] = "Taka sala nie istnieje.";
                d["Continue"] = "Kontynuuj";
                d["You do not belong to any class."] = "Nie należysz do żadnej klasy.";
                d["Such message does not exist."] = "Taka wiadomość nie istnieje.";
                d["You are not sender nor recipient of such message."] = "Nie jesteś nadawcą ani odbiorcą takiej wiadomości.";
                d["Such attachment does not exist."] = "Taki załącznik nie istnieje.";
                d["You are not sender nor recipient of such attachment."] = "Nie jesteś nadawcą ani odbiorcą takiego załącznika.";
                d["Such user does not exist in database."] = "Taki użytkownik nie istnieje w bazie danych.";
                d["Such user does not exist in session."] = "Taki użytkownik nie istnieje w sesji.";
                d["The user is already added."] = "Ten użytkownik jest już dodany.";
                d["You are not parent of a child in such class."] = "Nie jesteś rodzicem ucznia takiej klasy.";
                d["Your account does not exist."] = "Twoje konto nie istnieje.";
                d["Such appointment does not exist."] = "Takie zdarzenie nie istnieje.";
                d["You do not belong to such class."] = "Nie należysz do takiej klasy.";
                d["You do not own such appointment."] = "Nie posiadasz takiego zdarzenia.";
                d["You are not parent of such student."] = "Nie jesteś rodzicem takiego ucznia.";
                d["You do not teach in such class."] = "Nie uczysz w takiej klasie.";
                d["Such class does not have such lesson within your subject."] = "Taka klasa nie ma takiej lekcji w ramach twojego przedmiotu.";
                d["You have not created such absence."] = "Nie stworzyłeś takiej nieobecności.";
                d["Week day and date must be specified."] = "Dzień tygodnia i data muszą być określone.";
                d["You do not teach in such student's class."] = "Nie uczysz w klasie takiego ucznia.";
                d["You have not created such grade."] = "Nie stworzyłeś takiej oceny.";
            }
        }
    }
}
