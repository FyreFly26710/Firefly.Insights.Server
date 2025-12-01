# EF Core CLI Commands Cheat Sheet

This document summarizes the most commonly used **Entity Framework Core (EF Core) CLI commands** for managing migrations and the database in a .NET project.

---

## **Prerequisites**

* EF Core packages installed:

  ```bash
  dotnet add package Microsoft.EntityFrameworkCore
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer
  dotnet add package Microsoft.EntityFrameworkCore.Tools
  ```
* A properly configured `DbContext` (e.g., `ContentsContext`).

---

## **1. Add a Migration**

Creates a new migration based on changes in your `DbContext` and entity classes.

```bash
dotnet ef migrations add <MigrationName>
```

**Example:**

```bash
dotnet ef migrations add InitialCreate --output-dir Infrastructure/Migrations
```

**Optional flags:**

* `--context <DbContextName>`: specify which DbContext to use
* `--output-dir <Folder>`: place migration files in a custom folder

---

## **2. Apply Migrations / Update Database**

Applies all pending migrations to the database.

```bash
dotnet ef database update
```

**Optional flags:**

* `--context <DbContextName>`: specify DbContext
* `--migration <MigrationName>`: update to a specific migration
* Example:

```bash
dotnet ef database update InitialCreate
```

---

## **3. Remove Last Migration**

Deletes the last migration if it has **not been applied** to the database yet.

```bash
dotnet ef migrations remove
```

**Optional flag:**

* `--context <DbContextName>`: specify DbContext

---

## **4. List All Migrations**

Displays a list of all migrations in your project.

```bash
dotnet ef migrations list
```

**Optional flag:**

* `--context <DbContextName>`: specify DbContext

---

## **5. Other Useful Commands**

* **Script migrations to SQL**:

```bash
dotnet ef migrations script
```

Generates a SQL script for applying migrations.

* **Specify target migration**:

```bash
dotnet ef migrations script <FromMigration> <ToMigration>
```

* **Help / list all commands**:

```bash
dotnet ef --help
```

---

## **6. Notes & Best Practices**

* Always **review generated migrations** before applying to production.
* For enums stored as strings:

```csharp
entity.Property(t => t.Type)
      .HasConversion<string>();
```

* Use `HasColumnType` for very large strings (e.g., `nvarchar(max)` in SQL Server).
* Only configure relationships **once per dependent entity** to avoid duplicates.

---

This cheat sheet is designed for quick reference when working with EF Core CLI in any .NET project.
