# POMQC — POM Quality Control System

A web-based **Quality Control (QC)** and **QA/QC** management system for tracking purchase orders (POs), defects, DHU (Defects per Hundred Units) reporting, and inspection checklists. Built with ASP.NET MVC and SQL Server.

---

## Features

- **QA/QC Checklist** — Manage and track QA/QC checklists by AIGL PO, customer PO, factory, division, inline date, and defect counts
- **DHU Reports** — Weekly and monthly DHU reports with export to Excel
- **Defect Management** — Defect tracking, categorization, and multi-column defect code support
- **Final Reports** — Final inspection and customer-PO reports with Excel export
- **Filtering** — Filter by customer, factory, PO, and date range
- **Dashboard** — Summary views and report aggregation
- **Authentication** — Forms-based authentication with configurable login
- **Export** — Excel export for DHU weekly/monthly and final reports

---

## Solution Structure

| Project | Description |
|--------|-------------|
| **POMQC.WebSite** | ASP.NET MVC 4 web application (UI, views, scripts, content) |
| **POMQC.Controller** | MVC controllers (Report, DHU, Defect, Final, Inspection, Account) |
| **POMQC.Service** | Business logic (DHU, Defect, Report, Checklist, Filter, Account) |
| **POMQC.ViewModel** | View models for views and API responses |
| **POMQC.Entity** | Domain entities and data models |
| **POMQC.Data** | Data access, repositories, and stored procedures |
| **POMQC.Utilities** | Shared utilities and helpers |

---

## Prerequisites

- **Visual Studio** 2017 or later (or compatible IDE)
- **.NET Framework 4.0**
- **SQL Server** (LocalDB, Express, or full instance)
- **NuGet** (included with Visual Studio)

---

## Getting Started

### 1. Clone and open the solution

```bash
git clone <repository-url>
cd pom
```

Open `POMQC.WebSite\POMQC.WebSite.sln` in Visual Studio.

### 2. Restore NuGet packages

- **Visual Studio:** Right-click the solution → **Restore NuGet Packages**
- Or enable automatic package restore:  
  **Tools → Options → Package Manager** → enable **Allow NuGet to download missing packages** (and/or **Automatically check for missing packages during build**)

If **Enable NuGet Package Restore** appears on the solution context menu, use it so the solution builds without manually restoring packages.

### 3. Configure the database

1. Create a SQL Server database named `POMQC` (or update the connection string to match your database).
2. In `POMQC.WebSite\Web.config`, set the `POMQC` connection string under `<connectionStrings>`:

```xml
<add name="POMQC" 
     connectionString="Data Source=YOUR_SERVER;Initial Catalog=POMQC;Trusted_Connection=True;" 
     providerName="System.Data.SqlClient" />
```

Replace `YOUR_SERVER` with your SQL Server instance (e.g. `localhost`, `(LocalDb)\v11.0`, or `.\SQLEXPRESS`).

3. Run any required database scripts or migrations so tables and stored procedures exist (see project/setup docs if provided).

### 4. Build and run

- Set **POMQC.WebSite** as the startup project.
- Press **F5** or use **Debug → Start Debugging**.

The site runs under IIS Express by default. Log in via the configured login URL (e.g. `~/Account/Login`).

---

## Configuration

Key settings in `Web.config`:

| Setting | Purpose |
|--------|--------|
| `connectionStrings` / `POMQC` | Main application database |
| `DefaultConnection` | ASP.NET identity/local DB (if used) |
| `Version` | Application version (e.g. 2.0.0.1) |
| `ResizeImage`, `PdfImageWidth`, `PdfImageHeight` | Image/PDF report options |
| `MailServer`, `MailPort`, `MailAdmin`, etc. | Email (notifications/errors) |
| `CacheTime` | Cache duration (minutes) |
| `AcceptedFileTypes`, `ImageFileTypes`, `DocumentFileTypes` | Upload restrictions |

Adjust these for your environment; avoid committing secrets (use config transforms or user secrets for production).

---

## Technology Stack

- **Backend:** ASP.NET MVC 4, .NET Framework 4.0  
- **ORM:** Entity Framework 5.x  
- **Database:** SQL Server  
- **Auth:** Forms authentication, DotNetOpenAuth (OpenID/OAuth)  
- **Front-end:** jQuery, jQuery UI, Bootstrap, Angular (ng-grid), Dropzone, Lightbox2, TableExport  
- **Caching:** Fos.Caching, Enyim (Memcached)  
- **Packages:** NuGet

---

## Project Conventions

- **Layered architecture:** Web → Controllers → Services → Repositories/Data → Entity  
- **Stored procedures:** Referenced via `POMQC.Data` (e.g. `StoredProcedure.cs`) and configured via `App_Data/Sp.xml` (Fos.Data.DbSettingProvider).  
- **Views:** Razor (`.cshtml`); shared partials for reports, filters, and checklists.

---

## License

See repository or solution for license information.

---

## Support

For issues or questions, use the project’s issue tracker or contact the maintainers.
