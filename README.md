# Gradebook
Gradebook is an electronic grade book web application created with .NET and ASP.NET MVC framework. It is strongly backend-focused and therefore the views are repetitive and the application's overall appearance is not so exciting.

---
## Implemented features
- teacher, student and parent accounts
- creating subjects and assigning them to classes
- uploading syllabuses for subjects
- files for subjects uploaded by teachers
- assigning students to classes
- assigning supervisors to classes and teachers to subjects in classes
- adding lessons to classes and displaying them on a weekly schedule (timetable)
- announcements on the main page
- generating sheets of students' grades from a particular period
- periodic grade reports sent to parents' e-mail addresses
- sending text messages with optional attachments to other users
- sending announcements to parents by class supervisors via e-mail or Gradebook itself
- planning appointments (e.g. exams, homework, etc.) by teachers and browsing them in a calendar
- quizzes with close-ended questions prepared by teachers and taken by students
- English and Polish localization

---
## Database
Gradebook uses a database depicted by the diagram below. Boxes represent tables containing fields. A table's primary key consists of bold fields and italic fields are the table's foreign keys.

<img src="documentation/database.svg">

---
## Running
1. Set up a MySQL database server. Gradebook was successfully tested on MariaDB Server 10.2.40.

2. On the database server, create a user with permissions allowing him to create and use a new database. The following commands can be used on MariaDB 10.2.40 to create a user with all permissions:
    ```
    CREATE USER 'gradebook_admin'@'%' IDENTIFIED BY 'pass123';
    GRANT ALL PRIVILEGES ON *.* TO 'gradebook_admin'@'%' IDENTIFIED BY 'pass123' WITH GRANT OPTION;
    FLUSH PRIVILEGES;
    ```

3. Clone Gradebook repository.

4. Open `Gradebook.sln` in Visual Studio (19 and 22 were tested) with ASP.NET components installed.

5. The following message should appear: `Some NuGet packages are missing from this solution. Click to restore from your online package sources.` Click `Restore` to download NuGet packages required by the application.

6. In file `/Utils/EmailSender.cs`, method `CreateSmtp`, put details of your account on a SMTP server which Gradebook will use to send e-mail messages. You can use a public SMTP server (e.g. Gmail) or set up your own one.

7. In `Web.config` file, find tag `<connectionStrings>` and `<add>` tag inside it. Put your database user's details in `connectionString` attribute:
    ```
    connectionString="server=127.0.0.1;port=3306;database=GradebookDB;user=gradebook_admin;password=pass123"
    ```

8. Make sure that no database with the name put in `connectionString` attribute (`GradebookDB`) exists on your database server. Display the list of existing databases with:
    ```
    SHOW DATABASES;
    ```
    If `GradebookDB` exists, delete it using command:
    ```
    DROP DATABASE GradebookDB;
    ```
    Database name `GradebookDB` cannot be taken because at the first start Gradebook attempts to create a database with that name using Entity Framework Code First approach. Also, this is why the first start takes much longer than the following ones.

9. Build Gradebook using `Build > Build Solution`.

10. Run Gradebook using `Debug > Start Debugging`. A web browser should open, automatically go to `https://localhost:44304/` and then show the following error: `Could not find a part of the path '<path of Gradebook main directory>\bin\roslyn\csc.exe'.` To fix it, stop debugging and in `Tools > NuGet Package Manager > Packet Manager Console` execute command (according to [link (accessed 16.04.2023)](https://stackoverflow.com/a/34391473)):
    ```
    Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
    ```
    If a message about inconsistent line endings appears, choose to normalize them to `Windows (CR LF)` and proceed.

11. Run Gradebook again using `Debug > Start Debugging`. At the first start, the application creates the database and an administrator account with e-mail address `a.adminowski@szkola.pl` and password `administrator123`. It can be used to create more accounts for other users.

---
## Presentation
Click the image below to watch a presentation video on YouTube.

[<img src="https://i.imgur.com/xUR3eJn.png" width="500"/>](https://youtu.be/GC9dc8ezEV8)
