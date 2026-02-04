# SQL Server Setup Guide

This guide will help you set up the Task Board application with SQL Server.

## Prerequisites

- SQL Server 2016 or later (Express, Developer, or Standard edition)
- SQL Server Management Studio (SSMS) - Optional but recommended

## Step 1: Verify SQL Server Installation

### Check if SQL Server is Running

Open PowerShell and run:

```powershell
Get-Service MSSQLSERVER
```

If you see `Running` status, SQL Server is active. If it shows `Stopped`, start it:

```powershell
Start-Service MSSQLSERVER
```

### Find Your SQL Server Instance Name

Common SQL Server instance names:
- `localhost` - Default instance
- `LAPTOP-ABC\SQLEXPRESS` - Named instance (replace ABC with your computer name)
- `.` - Current computer (local instance)

## Step 2: Connection String Configuration

The connection string is already configured in:
- `Backend\appsettings.json`
- `Backend\appsettings.Development.json`

**Current connection string:**
```
Server=localhost;Database=TaskBoard;Trusted_Connection=true;TrustServerCertificate=true;
```

### Modify Connection String (If Needed)

If your SQL Server instance name is different, edit `Backend\appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=TaskBoard;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

**Examples:**
| Scenario | Connection String |
|----------|-------------------|
| Local default instance | `Server=localhost;Database=TaskBoard;Trusted_Connection=true;TrustServerCertificate=true;` |
| Named instance (SQL Express) | `Server=LAPTOP-JOHN\SQLEXPRESS;Database=TaskBoard;Trusted_Connection=true;TrustServerCertificate=true;` |
| Remote server | `Server=192.168.1.100;Database=TaskBoard;User Id=sa;Password=YourPassword;Encrypt=false;` |

## Step 3: Create Database with EF Core Migrations

Run these commands from the Backend folder:

```powershell
cd d:\NeuranX\TaskBoard\Backend

# Restore NuGet packages
dotnet restore

# Apply migrations (creates database and tables)
dotnet ef database update
```

**Expected output:**
```
info: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core initialized 'TaskBoardContext' using provider 'Microsoft.EntityFrameworkCore.SqlServer' 
      with options: MaxPoolSize=128
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (15ms) [Parameters=] in 18ms
      CREATE DATABASE [TaskBoard];
```

## Step 4: Verify Database Creation

### Using SSMS (Recommended)
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. In Object Explorer, expand **Databases**
4. You should see **TaskBoard** database
5. Expand it and verify tables: `Boards` and `Tasks`

### Using PowerShell
```powershell
sqlcmd -S localhost -E -Q "SELECT name FROM sys.databases WHERE name='TaskBoard';"
```

You should see:
```
TaskBoard
```

## Step 5: Run the Application

### Start the Backend API

```powershell
cd d:\NeuranX\TaskBoard\Backend
dotnet run
```

Expected output:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[15]
      Application started. Press Ctrl+C to exit.
```

### Start the Frontend (In Another Terminal)

```powershell
cd d:\NeuranX\TaskBoard\Frontend
npm start
```

### Open Application

Visit: **http://localhost:4200**

You should see the Task Board with sample data!

## Step 6: Verify Data in Database

### Using SSMS
1. Open SSMS
2. Connect to TaskBoard database
3. Right-click on `Boards` table → Select Top 1000 Rows
4. You should see sample data

### Using PowerShell
```powershell
sqlcmd -S localhost -E -d TaskBoard -Q "SELECT * FROM Boards;"
```

## Troubleshooting

### "Cannot connect to SQL Server"
- Check if SQL Server is running: `Get-Service MSSQLSERVER`
- Verify server name in connection string
- Check SQL Server instance is accessible

### "Login failed for user"
- Ensure using `Trusted_Connection=true` for Windows authentication
- Or provide valid `User Id` and `Password`

### "Database already exists"
- Drop existing database:
  ```powershell
  sqlcmd -S localhost -E -Q "DROP DATABASE TaskBoard;"
  ```
- Then run `dotnet ef database update` again

### Port 5000 already in use
- Change the port in `appsettings.json` or run on different port:
  ```powershell
  dotnet run --urls="http://localhost:5001"
  ```

## Switch Back to SQLite

To go back to SQLite (default):

1. Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=taskboard.db"
  }
}
```

2. Delete old migrations (optional):
```powershell
cd Backend
rm -r Migrations
```

3. Create new SQLite migrations:
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Additional Resources

- [SQL Server Express Free Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio Download](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- [Entity Framework Core - SQL Server](https://docs.microsoft.com/en-us/ef/core/providers/sql-server/)
- [Connection Strings Reference](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring)
