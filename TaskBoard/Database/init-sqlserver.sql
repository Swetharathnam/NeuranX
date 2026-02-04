-- SQL Server initialization script for Task Board application

-- Create Boards table
CREATE TABLE [Boards] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500),
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Boards] PRIMARY KEY ([Id])
);

-- Create TaskItems table
CREATE TABLE [Tasks] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(1000),
    [Status] int NOT NULL,
    [Priority] int NOT NULL,
    [DueDate] datetime2,
    [BoardId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tasks_Boards_BoardId] FOREIGN KEY ([BoardId]) REFERENCES [Boards] ([Id]) ON DELETE CASCADE
);

-- Create index on BoardId
CREATE INDEX [IX_Tasks_BoardId] ON [Tasks] ([BoardId]);

-- Insert sample data
INSERT INTO [Boards] ([Name], [Description], [CreatedAt], [UpdatedAt])
VALUES 
('Project Alpha', 'Main project board', GETUTCDATE(), GETUTCDATE()),
('Project Beta', 'Secondary project', GETUTCDATE(), GETUTCDATE());

INSERT INTO [Tasks] ([Title], [Description], [Status], [Priority], [DueDate], [BoardId], [CreatedAt], [UpdatedAt])
VALUES
('Setup Database', 'Initialize database schema', 2, 2, NULL, 1, GETUTCDATE(), GETUTCDATE()),
('Create API Endpoints', 'Build REST API', 1, 2, NULL, 1, GETUTCDATE(), GETUTCDATE()),
('Build UI Components', 'Create Angular components', 0, 1, DATEADD(day, 7, GETUTCDATE()), 1, GETUTCDATE(), GETUTCDATE()),
('Write Tests', 'Unit and integration tests', 0, 1, NULL, 2, GETUTCDATE(), GETUTCDATE());
