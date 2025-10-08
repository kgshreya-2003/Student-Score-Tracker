
# 🎓 Student Score Tracker

A full-stack web application designed to help teachers efficiently track, manage, and analyze student performance — including grades, attendance, and feedback.  
This system consists of an **Angular frontend** and an **ASP.NET Core Web API backend**.  

---

## 🧩 Project Overview

The **Student Score Tracker** system simplifies academic record management for teachers.  
It allows educators to enter, view, and analyze students’ grades, attendance, and feedback in one centralized system.

The application provides:
- User-friendly **Angular interface**
- Secure **ASP.NET Core API** for data handling
- Integration with **SQL Server** for persistent storage

---

## 🗂️ Project Structure

Your GitHub repository is organized as follows:

```

Student-Score-Tracker/
│
├── frontend/          # Angular application (UI)
│   ├── src/
│   ├── angular.json
│   ├── package.json
│   └── ...
│
└── backend/           # ASP.NET Core Web API (Server)
├── Controllers/
├── Models/
├── Program.cs
├── appsettings.json
└── ...

````

This structure separates the **frontend** and **backend** clearly for easier maintenance and deployment.

---

## 💻 Tech Stack

### 🔹 Frontend
- Angular
- TypeScript
- Bootstrap
- HTML5, CSS3

### 🔹 Backend
- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server

---

## ⚙️ Setup Instructions

Follow these steps to run the project on your local system:

### Step 1 — Clone the Repository
```bash
git clone https://github.com/yourusername/Student-Score-Tracker.git
cd Student-Score-Tracker
````

### Step 2 — Folder Overview

After cloning, you’ll have:

```
Student-Score-Tracker/
├── frontend/
└── backend/
```

Each folder can be opened separately in VS Code or Visual Studio.

---

## 🖥️ Frontend Setup (Angular)

### Step 1 — Navigate to the Frontend Folder

```bash
cd frontend
```

### Step 2 — Install Dependencies

```bash
npm install
```

### Step 3 — Run the Angular App

```bash
ng serve
```

### Step 4 — Access the App

Open your browser and go to:

```
http://localhost:4200
```

If everything runs successfully, you’ll see your Student Score Tracker UI.

---

## ⚙️ Backend Setup (ASP.NET Core)

### Step 1 — Navigate to the Backend Folder

```bash
cd backend
```

### Step 2 — Open in Visual Studio or VS Code

Open the `backend` folder in **Visual Studio** (recommended) or **VS Code**.

### Step 3 — Configure the Database Connection

In `appsettings.json`, set your database connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=StudentScoreDB;Trusted_Connection=True;"
}
```

### Step 4 — Apply Migrations (If Using EF Core)

```bash
dotnet ef database update
```

### Step 5 — Run the API

```bash
dotnet run
```

The backend API will start running on:

```
http://localhost:5000  or  https://localhost:7128
```

---

## 🔗 API Endpoints (Example)

| Method | Endpoint           | Description             |
| ------ | ------------------ | ----------------------- |
| GET    | `/api/Students`    | Get all students        |
| POST   | `/api/Students`    | Add a new student       |
| GET    | `/api/Grades/{id}` | Get student grades      |
| POST   | `/api/Feedbacks`   | Submit feedback         |
| GET    | `/api/Reports`     | Generate student report |

You can test these endpoints using **Postman** or directly from your Angular app.

---

## 🗄️ Database Schema (Overview)

<img width="750" height="719" alt="Screenshot 2025-10-08 071950" src="https://github.com/user-attachments/assets/16049770-b79f-4aa8-8f52-27469acf2d9e" />

<img width="1675" height="618" alt="Screenshot 2025-10-08 072122" src="https://github.com/user-attachments/assets/7cde4355-dd59-4fd7-a692-59fa7738ce62" />

---

## ⚡ How It Works

1. Teachers log in to the system.
2. They can add or edit student data including grades and attendance.
3. The data is securely stored in the **SQL Server** database.
4. The Angular frontend displays and updates the data dynamically.
5. Reports can be generated for progress tracking and performance visualization.

---

