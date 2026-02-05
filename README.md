# Task Board Mini Application

A full-stack task management application built with .NET 6+ Web API, Angular, and Entity Framework Core.

## Features

- **Board Management**: Create, read, update, and delete task boards
- **Task Management**: Create, update, delete tasks with status and priority tracking
- **Status Tracking**: Tasks can be in "To Do", "In Progress", or "Done" states
- **Priority Levels**: Tasks support Low, Medium, and High priority levels
- **Due Dates**: Optional due date tracking for tasks
- **Async Operations**: Fully asynchronous backend operations
- **Clean Architecture**: Separation of concerns with services, DTOs, and controllers
- **Error Handling**: Comprehensive error handling with custom exceptions
- **CORS Enabled**: Frontend and backend integration support

## Project Structure

### Backend (.NET)
```
Backend/
├── Controllers/          # API Controllers
├── Models/              # Entity Models
├── Data/                # EF Core Context
├── Services/            # Business Logic
├── DTOs/                # Data Transfer Objects
├── Exceptions/          # Custom Exceptions
├── Program.cs           # Application Entry Point
└── appsettings.json     # Configuration
```

### Frontend (Angular)
```
Frontend/
├── src/
│   ├── app/
│   │   ├── components/  # Angular Components
│   │   ├── services/    # HTTP Services
│   │   ├── models/      # Type Definitions
│   │   └── pipes/       # Custom Pipes
│   └── main.ts          # App Bootstrap
└── package.json         # Dependencies
```

## Prerequisites

- .NET 6 or later SDK
- Node.js 18+
- Angular CLI 17+
- PostgreSQL 14+ (or SQL Server/SQLite)

## Getting Started

### Backend Setup

1. Navigate to the Backend directory:
```bash
cd Backend
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Apply Entity Framework migrations:
```bash
dotnet ef database update
```

4. Run the API server:
```bash
dotnet run
```

The API will be available at `https://localhost:5000` (development) or `http://localhost:5000`

### Frontend Setup

1. Navigate to the Frontend directory:
```bash
cd Frontend
```

2. Install npm packages:
```bash
npm install
```

3. Start the Angular development server:
```bash
npm start
```

Or using Angular CLI:
```bash
ng serve
```

The application will be available at `http://localhost:4200`

## API Endpoints

### Boards
- `GET /api/boards` - Get all boards
- `GET /api/boards/{id}` - Get board by ID
- `POST /api/boards` - Create a new board
- `PUT /api/boards/{id}` - Update a board
- `DELETE /api/boards/{id}` - Delete a board

### Tasks
- `GET /api/tasks` - Get all tasks
- `GET /api/tasks/board/{boardId}` - Get tasks for a specific board
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create a new task
- `PUT /api/tasks/{id}` - Update a task
- `DELETE /api/tasks/{id}` - Delete a task

## Database

### Using PostgreSQL (Active)
The application is currently configured to use PostgreSQL. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=taskboard;Username=postgres;Password=your_password;"
  }
}
```

Then apply migrations:
```bash
dotnet ef database update
```

### Using SQLite or SQL Server
The application supports SQLite and SQL Server as alternatives. Update the `ConnectionStrings` in `appsettings.json` accordingly. For SQLite, remove the connection string to fallback to default, or specify a file path. For SQL Server, provide a standard SQL Server connection string.

## Architecture Highlights

### Clean Architecture
- **Separation of Concerns**: Models, Services, Controllers, and DTOs are separated
- **Dependency Injection**: All services are injected via DI container
- **Repository Pattern**: Database access through Entity Framework Core

### Async Programming
- All database operations are asynchronous
- Controllers use async/await pattern
- Services implement async task operations

### Error Handling
- Custom exception classes for different scenarios
- Global error handling in controllers
- Proper HTTP status codes
- Logged errors for debugging

### API Design
- RESTful endpoint naming conventions
- Proper HTTP methods (GET, POST, PUT, DELETE)
- Meaningful HTTP status codes
- Request/Response DTOs for data contracts

### Angular Integration
- HttpClient for API communication
- Reactive Services
- Standalone components (Angular 17+)
- Two-way data binding with FormsModule
- Custom pipes for filtering

## Logging

The application includes logging configuration for:
- Application events
- Entity Framework Core queries (development only)
- Error tracking

View logs in the console output or configure file-based logging in `appsettings.json`.

## Testing the Application

1. Open `http://localhost:4200` in your browser
2. Create a new board using the sidebar
3. Add tasks to the board
4. Change task status by clicking the status button
5. Edit tasks using the edit button
6. Delete tasks or boards using the delete button

## Development Features

### Backend
- Swagger/OpenAPI documentation at `https://localhost:5000/` (development)
- Entity Framework Core Migrations support
- Database seeding with sample data

### Frontend
- Hot module reloading
- TypeScript strict mode
- Standalone components
- RxJS observables for async operations

## Configuration

### Backend
Edit `appsettings.json` and `appsettings.Development.json` for:
- Database connection strings
- Logging levels
- CORS origins

### Frontend
API URL is configured in `BoardService` and `TaskService`:
```typescript
private apiUrl = 'http://localhost:5000/api/boards';
```

## Build for Production

### Backend
```bash
dotnet publish -c Release
```

### Frontend
```bash
ng build --configuration production
```

## Future Enhancements

- User authentication and authorization
- Real-time updates with SignalR
- Task filtering and search
- Task assignment to users
- Activity history/audit logs
- Notifications
- Dark mode support

## Support

For issues or questions, please refer to:
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Angular Documentation](https://angular.io/docs)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

## License

This project is provided as an educational example.
