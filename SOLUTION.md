# SOLUTION.md

## Debugging Case Study: Tasks Not Displaying in the UI

### Issue

Tasks were not displaying in the frontend UI, with no obvious error shown.

### How I Diagnosed the Issue

I started by opening the browser DevTools and inspecting the **Network** tab to confirm whether the frontend was calling the task API and what response was being returned.

* If no data (or an empty response) was returned, I treated it as a backend/API issue.
* If valid data was returned, I focused on the frontend rendering and state management.

When no data was returned, I placed breakpoints in the API endpoint and stepped through the controller, service, and repository layers to identify where the data flow stopped.

When data was returned correctly, I placed breakpoints in `tasks.ts` and the relevant page/component to verify state updates and rendering conditions.

### Root Cause

The root cause depended on where the data flow broke:

* **No data in the Network response** → backend/API logic issue
* **Data present but not displayed** → frontend state or rendering issue

### Resolution

I resolved the issue by fixing the specific layer where the data flow failed, either by correcting backend logic or adjusting frontend state handling and rendering logic.

### Key Takeaway

When tasks don’t appear in the UI, I follow the data flow end to end:
**Browser → Network → API → Backend logic → Frontend state → Rendering**.

This approach allows me to quickly isolate frontend vs backend issues without guesswork.

---

## Solution Design and Trade-offs

### High-Level Design

I designed the Task Tracker as a small but complete full-stack system with a clear client–server boundary. The goal was to demonstrate clean architecture, predictable data flow, and ease of debugging rather than over-optimising for scale.

The backend follows a layered architecture (**API → Application → Domain → Infrastructure**), and the frontend is a React + TypeScript SPA consuming the API via HTTP.

---

### Backend Design Decisions

**MediatR for Application Flow**
I used **MediatR** to decouple controllers from application logic. Controllers delegate work to commands and handlers, keeping HTTP concerns separate from business logic.

**Trade-off:**
This adds an extra abstraction layer, but improves separation of concerns and testability.

**Validation Approach**
For this assessment, I used **data annotations** for request validation. Although **FluentValidation** is generally preferable for more complex scenarios, data annotations were sufficient and kept the solution lightweight.

**Trade-off:**
Data annotations are less expressive than FluentValidation, but reduce setup complexity and are appropriate for a take-home assignment.

**Layered Architecture**
Controllers are thin and focused on HTTP concerns. Business logic lives in the application layer, domain models remain framework-agnostic, and infrastructure handles data access using EF Core with an InMemory provider.

**Trade-off:**
This introduces more structure than a minimal API, but results in clearer boundaries, better testability, and easier debugging.

---

### Frontend Design Decisions

**Centralised Data Handling**
Task fetching and transformation logic lives in `tasks.ts` rather than being scattered across components.

**Explicit Data Flow**
UI state is driven directly from API responses. When tasks don’t appear, the debugging path is clear: **Network → API → state → render**.

**Trade-off:**
This adds a small amount of structure, but significantly improves debuggability and extensibility.


**Form Mode Handling (Create vs Edit)**
Instead of creating separate components for “Create” and “Edit” forms, I used a single form component and determined its behavior based on the current mode.

**Trade-off**: 
This reduces duplication and simplifies code, but slightly increases conditional logic within the form component. It’s a practical trade-off for a small app, improving maintainability without overcomplicating the structure.

---

### Testing Strategy

Backend tests focus on core behaviour rather than full end-to-end coverage. NUnit was used to keep the testing setup straightforward.

**Trade-off:**
Test depth is limited compared to a production system, but sufficient to demonstrate correctness, intent, and testability.
