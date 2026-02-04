-- SQLite initialization script for Task Board application

-- Create Boards table
CREATE TABLE Boards (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Create TaskItems table
CREATE TABLE Tasks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT,
    Status INTEGER NOT NULL,
    Priority INTEGER NOT NULL,
    DueDate DATETIME,
    BoardId INTEGER NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    FOREIGN KEY (BoardId) REFERENCES Boards(Id) ON DELETE CASCADE
);

-- Create index on BoardId
CREATE INDEX IX_Tasks_BoardId ON Tasks(BoardId);

-- Insert sample data
INSERT INTO Boards (Name, Description, CreatedAt, UpdatedAt)
VALUES 
('Project Alpha', 'Main project board', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Project Beta', 'Secondary project', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

INSERT INTO Tasks (Title, Description, Status, Priority, DueDate, BoardId, CreatedAt, UpdatedAt)
VALUES
('Setup Database', 'Initialize database schema', 2, 2, NULL, 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Create API Endpoints', 'Build REST API', 1, 2, NULL, 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Build UI Components', 'Create Angular components', 0, 1, datetime('now', '+7 days'), 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
('Write Tests', 'Unit and integration tests', 0, 1, NULL, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
