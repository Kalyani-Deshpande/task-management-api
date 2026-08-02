# ✅ Task Management API

A production-grade **REST API** built with **ASP.NET Core 10**, featuring JWT authentication, Entity Framework Core, PostgreSQL, and Docker.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=csharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=flat&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat&logo=docker&logoColor=white)
![xUnit](https://img.shields.io/badge/Tests-5%20passing-512BD4?style=flat)

## ✨ Features

- **JWT Authentication** — register, login, token-based protected routes
- **Task CRUD** — create, read, update, delete tasks with filtering
- **Task Status Lifecycle** — Todo → InProgress → Done → Cancelled
- **Task Priority** — Low, Medium, High, Critical
- **Entity Framework Core** — code-first with auto database migration
- **Request Logging Middleware** — logs method, path, status, latency
- **Swagger/OpenAPI** — fully documented with JWT bearer support
- **Docker Compose** — PostgreSQL + API in one command
- **xUnit Tests** — 5 passing unit tests with InMemory database

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 |
| Language | C# 12 |
| ORM | Entity Framework Core 10 |
| Database | PostgreSQL 16 |
| Auth | JWT Bearer tokens + BCrypt |
| Documentation | Swagger / OpenAPI 3.0 |
| Testing | xUnit + InMemory EF Core |
| Containerisation | Docker + Docker Compose |

## 📁 Project Structure

```
task-management-api/
├── Controllers/
│   ├── AuthController.cs       # Register + Login endpoints
│   └── TasksController.cs      # Task CRUD endpoints
├── Data/
│   └── AppDbContext.cs         # EF Core DbContext
├── DTOs/
│   ├── AuthDtos.cs             # Register/Login/Response DTOs
│   └── TaskDtos.cs             # Create/Update/Response DTOs
├── Middleware/
│   └── RequestLoggingMiddleware.cs
├── Models/
│   ├── User.cs                 # User entity
│   └── TaskItem.cs             # Task entity with Status/Priority enums
├── Services/
│   ├── AuthService.cs          # JWT generation + BCrypt
│   └── TaskService.cs          # Task business logic
├── TaskManagementApi.Tests/
│   └── AuthTests.cs            # 5 xUnit tests
├── docker-compose.yml
├── Dockerfile
└── Program.cs
```

## 🚀 Getting Started

### Prerequisites
- Docker Desktop
- .NET 10 SDK

### Run with Docker

```bash
git clone https://github.com/Kalyani-Deshpande/task-management-api.git
cd task-management-api
docker compose up -d postgres
dotnet run
```

Visit **http://localhost:5043/api-docs**

## 📚 API Endpoints

### Auth
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/auth/register` | Register new user | None |
| POST | `/api/auth/login` | Login + get JWT | None |

### Tasks
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/tasks` | Get all tasks (filterable) | ✅ |
| GET | `/api/tasks/{id}` | Get task by ID | ✅ |
| POST | `/api/tasks` | Create task | ✅ |
| PUT | `/api/tasks/{id}` | Update task | ✅ |
| DELETE | `/api/tasks/{id}` | Delete task | ✅ |
| PATCH | `/api/tasks/{id}/status` | Update status only | ✅ |

### Other
| Method | Endpoint | Description |
|---|---|---|
| GET | `/health` | Health check |

## 🧪 Running Tests

```bash
cd TaskManagementApi.Tests
dotnet test
```

```
Test summary: total: 5, failed: 0, succeeded: 5
```

## 📄 Licence

MIT