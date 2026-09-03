# 📚 Library Management System

A backend RESTful API for managing library operations, built with **ASP.NET Core 9** and **C#**.

The system is designed around a clean **Layered Architecture** and provides separate workflows for **Administrators** and **Members**. It supports catalog management, member management, borrowing and returning items, authentication, and library analytics.

---

## 🚀 Features

### 👨‍💼 Admin Features

- Manage library catalog items
  - Books
  - Magazines
  - Newspapers
- Create, view, update, and delete library items
- Manage library members
- View member borrowing history
- Generate library analytics and reports
- View:
  - Most borrowed items
  - Least borrowed items
  - Borrowed items by type
  - Most active members
  - Fines over time

### 🧑 Member Features

- Register and log in
- Browse the library catalog
- Filter available library items
- View item details
- Borrow library items
- Return borrowed items
- View borrowing history and active transactions

### 🔐 Security & Authentication

- JWT Bearer Authentication
- ASP.NET Core Identity
- Role-Based Authorization
- Separate authorization for:
  - `Admin`
  - `Member`
- Protected API endpoints
- Email Confirmation
- Forgot Password
- Password Reset
- SMTP Email Integration
- User Secrets for sensitive email configuration

## 🧠 Tech Stack

| Technology | Purpose |
|---|---|
| **C#** | Programming Language |
| **ASP.NET Core 9** | Backend Web API Framework |
| **Entity Framework Core 9** | ORM & Database Access |
| **SQL Server** | Database |
| **ASP.NET Core Identity** | User & Role Management |
| **JWT Bearer Authentication** | Authentication & Authorization |
| **Scalar OpenAPI** | API Documentation |
| **Layered Architecture** | Application Architecture |

---

## 🏗️ Architecture

The project follows a **Layered Architecture** to separate responsibilities and improve maintainability.

```text
Library Management System
│
├── LibrarySystem.PL
│   └── Presentation Layer
│       ├── Controllers
│       ├── Areas
│       │   ├── Admin
│       │   └── Member
│       ├── Authentication
│       └── API Configuration
│
├── LibrarySystem.BLL
│   └── Business Logic Layer
│       ├── Services
│       ├── Interfaces
│       ├── DTOs
│       ├── Enums
│       └── Business Rules
│
└── LibrarySystem.DAL
    └── Data Access Layer
        ├── Models
        ├── ApplicationDbContext
        ├── Repositories
        └── Migrations
```

### Presentation Layer — `LibrarySystem.PL`

Responsible for handling HTTP requests and exposing the REST API.

Includes:

- Authentication controller
- Admin controllers
- Member controllers
- Role-based authorization
- Application configuration

### Business Logic Layer — `LibrarySystem.BLL`

Contains the application's business rules and services.

Main services include:

- `IdentityService`
- `LibraryItemService`
- `MemberService`
- `MemberCatalogService`
- `BorrowingService`
- `ReportService`

### Data Access Layer — `LibrarySystem.DAL`

Responsible for database-related operations.

Includes:

- Entity Framework Core models
- `ApplicationDbContext`
- Database relationships
- Repositories
- EF Core migrations

---

## 🗄️ Main Database Entities

The main entities in the system are:

- `ApplicationUser`
- `Member`
- `LibraryItem`
- `Book`
- `Magazine`
- `Newspaper`
- `BorrowTransaction`

### Library Item Hierarchy

`LibraryItem` is the base model for the different types of items available in the library.

```text
LibraryItem
├── Book
├── Magazine
└── Newspaper
```

### Borrowing Relationship

```text
Member
   │
   └── BorrowTransaction
            │
            └── LibraryItem
                    ├── Book
                    ├── Magazine
                    └── Newspaper
```

A `BorrowTransaction` stores information such as:

- Borrow date
- Due date
- Return date
- Fine
- Fine payment status
- Member
- Borrowed library item

---

# 📡 API Endpoints

## 🔐 Authentication

Base route:

```text
/api/Auth
```
| Method | Endpoint | Description | Access |
|---|---|---|---|
| `POST` | `/api/Auth/Register` | Register a new account and send confirmation email | Public |
| `POST` | `/api/Auth/Login` | Login and receive JWT | Public |
| `GET` | `/api/Auth/confirm-email` | Confirm user email address | Public |
| `POST` | `/api/Auth/forget-password` | Request a password reset token | Public |
| `POST` | `/api/Auth/reset-password` | Reset password using the reset token | Public |
| `GET` | `/api/Auth/test-auth` | Test authenticated access | Authenticated |
| `GET` | `/api/Auth/admin-only` | Test Admin authorization | Admin |
| `GET` | `/api/Auth/member-only` | Test Member authorization | Member |

---

## 👨‍💼 Admin API

All Admin endpoints require:

```http
Authorization: Bearer YOUR_JWT_TOKEN
```

and the authenticated user must have the `Admin` role.

### 📚 Library Items

Base route:

```text
/api/Admin/LibraryItems
```

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Admin/LibraryItems/books` | Add a book |
| `POST` | `/api/Admin/LibraryItems/magazines` | Add a magazine |
| `POST` | `/api/Admin/LibraryItems/newspapers` | Add a newspaper |
| `GET` | `/api/Admin/LibraryItems` | Get library items |
| `GET` | `/api/Admin/LibraryItems/{id}` | Get item by ID |
| `PUT` | `/api/Admin/LibraryItems/books/{id}` | Update a book |
| `PUT` | `/api/Admin/LibraryItems/magazines/{id}` | Update a magazine |
| `PUT` | `/api/Admin/LibraryItems/newspapers/{id}` | Update a newspaper |
| `DELETE` | `/api/Admin/LibraryItems/{id}` | Delete a library item |

### 👥 Members

Base route:

```text
/api/Admin/Members
```

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Admin/Members` | Get all members |
| `GET` | `/api/Admin/Members/{membershipId}` | Get member by membership ID |
| `PUT` | `/api/Admin/Members/{membershipId}` | Update member |
| `DELETE` | `/api/Admin/Members/{membershipId}` | Delete member |
| `POST` | `/api/Admin/Members` | Add member |
| `GET` | `/api/Admin/Members/{membershipId}/borrowings` | Get member borrowing history |

### 📊 Reports & Analytics

Base route:

```text
/api/Admin/Reports
```

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Admin/Reports/most-borrowed-items` | Get most borrowed items |
| `GET` | `/api/Admin/Reports/borrowed-items-by-type` | Get borrowed items grouped by type |
| `GET` | `/api/Admin/Reports/most-active-members` | Get most active members |
| `GET` | `/api/Admin/Reports/least-borrowed-items` | Get least borrowed items |
| `GET` | `/api/Admin/Reports/Fines-over-time` | Get fines over time |

---

## 🧑 Member API

All Member endpoints require:

```http
Authorization: Bearer YOUR_JWT_TOKEN
```

and the authenticated user must have the `Member` role.

### 📖 Catalog

Base route:

```text
/api/Member/Catalog
```

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Member/Catalog` | Browse and filter the catalog |
| `GET` | `/api/Member/Catalog/{libraryItemId}` | Get library item details |

### 🔄 Borrowings

Base route:

```text
/api/Member/Borrowings
```

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Member/Borrowings/{libraryItemId}` | Borrow a library item |
| `POST` | `/api/Member/Borrowings/{borrowTransactionId}/return` | Return a borrowed item |

---

# 🔑 Authorization Flow

The application uses JWT tokens to secure protected endpoints.

### 1. Register

Create a new account through:

```http
POST /api/Auth/Register
```

### 2. Login

Authenticate using:

```http
POST /api/Auth/Login
```

The API returns a JWT token.

### 3. Send the JWT

For protected endpoints, include:

```http
Authorization: Bearer YOUR_JWT_TOKEN
```

### 4. Role-Based Access

The API checks the user's role before allowing access.

```text
Admin
 ├── Library Items
 ├── Members
 └── Reports

Member
 ├── Catalog
 └── Borrowings
```

---

# 🗃️ Database

The project uses **SQL Server** with **Entity Framework Core**.

The default connection string is configured in:

```text
LibrarySystem.PL/appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=LibrarySystem;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> Update the connection string according to your local SQL Server configuration.

---

# ⚙️ Installation & Setup

## Prerequisites

Make sure you have installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- SQL Server
- Visual Studio 2022
- Git

## 1. Clone the Repository

```bash
git clone https://github.com/abdfahmawe/Library-Management-System.git
```

Move into the project:

```bash
cd Library-Management-System
```

## 2. Configure the Database

Open:

```text
LibrarySystem.PL/appsettings.json
```

Update the SQL Server connection string if necessary.

## 3. Apply Migrations

From the solution directory, run:

```bash
dotnet ef database update --project LibrarySystem.DAL --startup-project LibrarySystem.PL
```

## 4. Run the Application

You can run the application using:

```bash
dotnet run --project LibrarySystem.PL
```

Or open the solution in **Visual Studio 2022** and run the `LibrarySystem.PL` project.

---

# 📖 API Documentation

The project uses **Scalar OpenAPI** for API documentation.

After running the application, open the API documentation URL configured by the application.

You can also use:

- Scalar
- Postman
- `.http` requests included in the project

For protected endpoints, authenticate first and provide the JWT token in the request headers.

---

# 📁 Project Structure

```text
Library-Management-System
│
├── LibrarySystem.BLL
│   ├── Common
│   │   └── Enums
│   ├── DTOs
│   │   ├── Request
│   │   └── Response
│   ├── Services
│   │   ├── Classes
│   │   └── Interfaces
│   └── Setting
│
├── LibrarySystem.DAL
│   ├── Data
│   ├── Migrations
│   ├── Models
│   └── Repositories
│
├── LibrarySystem.PL
│   ├── Areas
│   │   ├── Admin
│   │   │   └── Controllers
│   │   └── Member
│   │       └── Controllers
│   ├── Controllers
│   ├── Properties
│   ├── Program.cs
│   └── appsettings.json
│
├── LibrarySystem.sln
└── .gitignore
```

---

# 🧪 Testing the API

The API can be tested using:

- Scalar OpenAPI
- Postman
- Insomnia
- Visual Studio `.http` requests

Recommended authentication flow:

```text
Register
   ↓
Login
   ↓
Receive JWT
   ↓
Add Authorization Header
   ↓
Access Protected Endpoints
```

Example:

```http
Authorization: Bearer eyJhbGciOi...
```

---

# 📌 Business Workflow

A typical borrowing workflow looks like this:

```text
Member
  │
  ▼
Browse Catalog
  │
  ▼
Select Available Item
  │
  ▼
Borrow Item
  │
  ▼
BorrowTransaction Created
  │
  ▼
Item Becomes Unavailable
  │
  ▼
Return Item
  │
  ▼
ReturnTransaction Processed
  │
  ▼
Item Becomes Available
```

If an item is returned late, the system can calculate and track the associated fine and its payment status.

---

# 📈 Reports & Analytics

The Admin reporting module provides analytics for library activity, including:

- Most borrowed items
- Least borrowed items
- Borrowed items by type
- Most active members
- Fines over time

These reports provide administrators with useful information about library usage and borrowing behavior.

---

# 🔮 Future Improvements

Possible future improvements include:

- Email notifications
- Borrowing due-date reminders
- Fine payment integration
- Reservation / hold system
- Pagination for catalog and member lists
- Advanced catalog search
- Sorting and filtering improvements
- Admin dashboard
- Member dashboard
- Automated tests
- Docker support
- CI/CD pipeline

---

# 👨‍💻 Author

**Abd Al-Rahman Hamdan**

Software Engineer | Backend Developer

- GitHub: https://github.com/abdfahmawe

---

## ⭐ Support

If you find this project useful, consider giving the repository a ⭐ on GitHub.

---
