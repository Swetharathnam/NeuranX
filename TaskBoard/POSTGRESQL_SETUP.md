# PostgreSQL Setup Guide

Complete guide to set up the Task Board application with PostgreSQL.

## Prerequisites

- PostgreSQL 12 or later
- pgAdmin 4 (optional, for GUI management)
- psql command-line tool

## Step 1: Install PostgreSQL

### Windows
1. Download from: https://www.postgresql.org/download/windows/
2. Run the installer
3. Remember the **postgres** user password (set during installation)
4. Default port: **5432**

### Verify Installation
```powershell
psql --version
```

## Step 2: Start PostgreSQL Service

### Windows (PowerShell as Admin)
```powershell
# Check if running
Get-Service PostgreSQL*

# Start PostgreSQL
Start-Service PostgreSQL-x64-15
```

Replace `15` with your PostgreSQL version.

### Verify Connection
```powershell
psql -U postgres -c "SELECT version();"
```

## Step 3: Configuration Already Done ✅

The application is already configured for PostgreSQL with:

**Connection String** (in appsettings.json):
```
Host=localhost;Port=5432;Database=taskboard;Username=postgres;Password=postgres;
```

### Update Password (If Different)

Edit `Backend\appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=taskboard;Username=postgres;Password=YOUR_PASSWORD;"
  }
}
```

Replace `YOUR_PASSWORD` with your PostgreSQL postgres user password.

## Step 4: Create Database with EF Core

From the Backend folder:

```powershell
cd d:\NeuranX\TaskBoard\Backend

# Restore packages
dotnet restore

# Apply migrations (creates database and tables)
dotnet ef database update
```

**Expected output:**
```
info: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core initialized 'TaskBoardContext' using provider 'Npgsql.EntityFrameworkCore.PostgreSQL'
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (25ms) [Parameters=] in 31ms
      CREATE DATABASE taskboard;
```

## Step 5: Verify Database Creation

### Using psql (Command Line)
```powershell
psql -U postgres -c "SELECT datname FROM pg_database WHERE datname='taskboard';"
```

Should show: `taskboard` ✅

### View Tables
```powershell
psql -U postgres -d taskboard -c "\dt"
```

Should show:
```
            List of relations
 Schema |       Name        | Type  |  Owner
--------+-------------------+-------+----------
 public | Boards            | table | postgres
 public | Tasks             | table | postgres
```

### Using pgAdmin GUI (Optional)
1. Open pgAdmin
2. Right-click **Databases**
3. Select **Create** → **Database**
4. Name: `taskboard`
5. Owner: `postgres`
6. Click **Save**

## Step 6: Run the Application

### Terminal 1: Start Backend
```powershell
cd d:\NeuranX\TaskBoard\Backend
dotnet restore
dotnet run
```

Expected output:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

### Terminal 2: Start Frontend
```powershell
cd d:\NeuranX\TaskBoard\Frontend
npm start
```

### Open Application
Visit: **http://localhost:4200**

## Step 7: Verify Data

### Check Sample Data
```powershell
psql -U postgres -d taskboard -c "SELECT * FROM Boards;"
```

Should show 2 sample boards.

### Query Tasks
```powershell
psql -U postgres -d taskboard -c "SELECT id, title, status FROM Tasks;"
```

## Useful PostgreSQL Commands

```powershell
# Connect to database
psql -U postgres -d taskboard

# List all databases
\l

# List all tables
\dt

# View table structure
\d Boards

# Run query
SELECT * FROM Boards;

# Exit psql
\q
```

## Troubleshooting

### "Cannot connect to PostgreSQL"
- Check if PostgreSQL is running: `Get-Service PostgreSQL*`
- Verify connection string has correct password
- Ensure port 5432 is accessible

### "Database taskboard already exists"
```powershell
psql -U postgres -c "DROP DATABASE taskboard;"
```
Then run migrations again.

### "Password authentication failed"
- Verify postgres password in connection string
- Reset postgres password:
  ```powershell
  psql -U postgres -c "ALTER USER postgres WITH PASSWORD 'newpassword';"
  ```

### "Port 5432 already in use"
PostgreSQL uses this port by default. If needed, change in `postgresql.conf` or use different port in connection string:
```json
"Host=localhost;Port=5433;Database=taskboard;..."
```

### Restore from SQLite or SQL Server

If coming from another database, delete migrations:
```powershell
cd Backend
rm -r Migrations
```

Then create fresh migrations for PostgreSQL:
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Connection String Examples

| Scenario | Connection String |
|----------|-------------------|
| Local default | `Host=localhost;Port=5432;Database=taskboard;Username=postgres;Password=postgres;` |
| Remote server | `Host=192.168.1.100;Port=5432;Database=taskboard;Username=postgres;Password=mypass;` |
| SSL required | `Host=localhost;Port=5432;Database=taskboard;Username=postgres;Password=postgres;SSL Mode=Require;` |
| Custom port | `Host=localhost;Port=5433;Database=taskboard;Username=postgres;Password=postgres;` |

## Common Issues & Solutions

### Issue: "the database system is starting up"
**Solution:** Wait a few seconds and try again. PostgreSQL service is initializing.

### Issue: Migrations fail
**Solution:** 
```powershell
dotnet ef database drop
dotnet ef database update
```

### Issue: Can't connect after restart
**Solution:** 
```powershell
# Restart PostgreSQL service
Stop-Service PostgreSQL-x64-15
Start-Service PostgreSQL-x64-15
```

## Additional Resources

- [PostgreSQL Official Download](https://www.postgresql.org/download/)
- [pgAdmin Official Site](https://www.pgadmin.org/)
- [Npgsql Documentation](https://www.npgsql.org/)
- [EF Core PostgreSQL Provider](https://www.npgsql.org/efcore/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

## Next Steps

✅ PostgreSQL is configured
✅ Database will be created on first migration
✅ Sample data will be seeded automatically
✅ Application ready to use!

Run the backend and frontend to get started! 🚀
