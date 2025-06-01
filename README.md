````markdown
# 📚 CourseManager MVC App

Welcome to the **CourseManager MVC** app – your go-to portal for managing student enrollments without breaking a sweat (or a database). Whether you're wrangling course data or tracking student responses, this web app has your back like a well-placed semicolon.

---

## 🧠 What’s This All About?

Imagine a world where managing courses and enrollments is… *pleasant*. This ASP.NET Core MVC application features a dynamic **1-to-many data model** that lets you easily:

- Add and manage courses 🏫  
- Enroll students like a breeze 🎓  
- Navigate with a clean UI 🧭  
- Notify students automagically via email ✉️  
- Track responses like a responsible grown-up ✅

All while keeping things secure with cookie-based session management 🍪.

---

## 🛠️ Tech Stack

| Layer        | Tech Used                 |
|--------------|---------------------------|
| Framework    | ASP.NET Core MVC          |
| Backend      | C#                        |
| ORM          | Entity Framework Core     |
| Database     | SQL Server (EF Migrations)|
| Notifications| Gmail SMTP Integration    |
| Sessions     | Cookie-Based Auth 🍪      |

---

## 🚀 Getting Started

Clone the repo and ride:

```bash
git clone https://github.com/your-username/CourseManager-MVC.git
cd CourseManager-MVC
````

### Setup Instructions

1. **Install Dependencies**
   Make sure you’ve got the [.NET SDK](https://dotnet.microsoft.com/en-us/download) installed.

2. **Update Database**
   Apply migrations and seed the DB:

   ```bash
   dotnet ef database update
   ```

3. **Run the App**
   Launch the web app:

   ```bash
   dotnet run
   ```

4. **Profit 💰**
   Access it at `https://localhost:5001` or `http://localhost:5000`

---

## 📬 Email Notifications

Yep, we notify students using Gmail SMTP. Configure your `appsettings.json` like so:

```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "Port": 587,
  "SenderName": "CourseManager",
  "SenderEmail": "your-email@gmail.com",
  "Username": "your-email@gmail.com",
  "Password": "your-app-password"
}
```

> 🔐 Use an **App Password** if you're on Google. Never hardcode real credentials — use secrets or environment variables in production.

---





