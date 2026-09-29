# ManagementEmployeeEnterprise

[![Framework](https://shields.io)](https://microsoft.com)
[![License](https://shields.io)](LICENSE)

An enterprise-grade Employee Management System built using **ASP.NET Core**. This repository serves as a graduation research project focused on building a secure, scalable, and robust architecture for corporate workforce data administration.

## 🚀 Key Features

*   **Comprehensive MVC Architecture:** Clean separation of concerns utilizing Controllers, Models, ViewModels, and Views.
*   **Anti-Fraud Service:** Integrated specialized security layer (`Services/AntiFraud`) to flag anomalies, prevent insider threats, or detect malicious entries.
*   **Automated Testing:** Dedicated testing suite (`MEE.Tests`) ensuring business logic and operational safety remain reliable.
*   **SQL Database Provisioning:** Pre-configured SQL creation scripts (`CreateTable.sql`) for seamless database schema deployment.

---

## 📂 Project Structure

```text
ManagementEmployeeEnterprise/
├── Controllers/            # Handles user requests and orchestrates workflow
├── Data/                   # Database context and data-access configurations
├── Models/                 # Core domain models and business entities
├── ViewModels/             # Strongly-typed data models specialized for frontend views
├── Views/                  # Razor templates defining the user interface
├── Services/               # Business logic layers
│   └── AntiFraud/          # Advanced fraud detection and internal security logic
├── MEE.Tests/              # Complete unit and integration testing suite
├── CreateTable.sql         # SQL schema setup file
├── Program.cs              # Application entry point and service dependency injection
└── appsettings.json        # Main application configuration file
```

---

## 🛠️ Prerequisites

Before setting up the project, ensure you have the following installed:
*   [.NET 10 SDK](https://microsoft.com) or later
*   [SQL Server](https://microsoft.com) (Express or Developer edition)
*   [Visual Studio 2022](https://microsoft.com) / [JetBrains Rider](https://jetbrains.com) / VS Code

---

## ⚙️ Setup and Installation

### 1. Clone the Repository
```bash
git clone https://github.com/cybersecuritynighty/ManagementEmployeeEnterprise.git
cd ManagementEmployeeEnterprise
```

### 2. Database Initialization
1. Connect to your SQL Server instance.
2. Create a new database (e.g., `ManagementEmployeeDB`).
3. Open and run the `CreateTable.sql` script against your newly created database to populate the required tables and constraints.

### 3. Application Configuration
Update the connection string in `appsettings.json` (or `appsettings.Development.json`) to match your database settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ManagementEmployeeDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

### 4. Build and Run the App
Restore dependencies and start the web application:
```bash
dotnet restore
dotnet build
dotnet run --project ManagementEmployeeEnterprise
```
Once started, navigate to `https://localhost:5001` or `http://localhost:5000` in your preferred web browser.

---

## 🧪 Testing

The repository comes with a full unit testing framework to guarantee security and system uptime. Execute the test runner using:

```bash
dotnet test
```

---

## 🛡️ License

This project is licensed under the MIT License. See the `LICENSE` file for more details.
