# Task Tracker

## Overview
This project is a full-stack Task Tracker built as a take-home assignment.

It consists of:

- **Backend (C# / ASP.NET Core)**:
  - **TaskTracker.Api** – API endpoints and controllers
  - **TaskTracker.Application** – application logic and services
  - **TaskTracker.Domain** – domain models and enums
  - **TaskTracker.Infrastructure** – data access and EF Core InMemory setup
  - **TaskTracker.Tests** – backend tests (NUnit)
- **Frontend (React + TypeScript SPA)**:
  - **task-tracker** – single-page application consuming the API

The application demonstrates RESTful API design, layered architecture, client-server interaction, validation, error handling, and basic testing.

---

## Tech Stack & Versions

### Backend
- .NET SDK: **8.x**
- ASP.NET Core Web API
- Entity Framework Core **InMemory** provider
- **NUnit** (tests)
- Swagger / OpenAPI

### Frontend
- React + TypeScript
- Vite
- Node.js **22.0.0**

---

## Project Structure

```
repo-root/
├─ Backend/
│   └─ TaskTracker/
│       ├─ TaskTracker.Api
│       ├─ TaskTracker.Application
│       ├─ TaskTracker.Domain
│       ├─ TaskTracker.Infrastructure
│       └─ TaskTracker.Tests
├─ Frontend/
│   └─ task-tracker
```

## Running the Application

### Backend API
```
cd Backend/TaskTracker/TaskTracker.Api
dotnet restore
dotnet run
```
API runs at: http://localhost:5005

Swagger UI: http://localhost:5005/swagger

### Front end 
```
cd Frontend/task-tracker
npm install
npm run dev
```