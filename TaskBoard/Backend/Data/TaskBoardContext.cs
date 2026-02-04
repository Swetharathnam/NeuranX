using Microsoft.EntityFrameworkCore;
using TaskBoard.Models;
using TaskBoardModels = TaskBoard.Models;

namespace TaskBoard.Data
{
    public class TaskBoardContext : DbContext
    {
        public TaskBoardContext(DbContextOptions<TaskBoardContext> options) : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<TaskItem> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Board entity
            modelBuilder.Entity<Board>()
                .HasKey(b => b.Id);
            
            modelBuilder.Entity<Board>()
                .HasMany(b => b.Tasks)
                .WithOne(t => t.Board)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Board>()
                .Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Board>()
                .Property(b => b.Description)
                .HasMaxLength(500);

            // Configure TaskItem entity
            modelBuilder.Entity<TaskItem>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status)
                .HasConversion<int>();

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Priority)
                .HasConversion<int>();

            // Seed sample data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed boards
            modelBuilder.Entity<Board>().HasData(
                new Board { Id = 1, Name = "Project Alpha", Description = "Main project board", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Board { Id = 2, Name = "Project Beta", Description = "Secondary project", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );

            // Seed tasks
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Title = "Setup Database", Description = "Initialize database schema", Status = TaskBoardModels.TaskStatus.Done, Priority = TaskBoardModels.TaskPriority.High, BoardId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new TaskItem { Id = 2, Title = "Create API Endpoints", Description = "Build REST API", Status = TaskBoardModels.TaskStatus.InProgress, Priority = TaskBoardModels.TaskPriority.High, BoardId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new TaskItem { Id = 3, Title = "Build UI Components", Description = "Create Angular components", Status = TaskBoardModels.TaskStatus.Todo, Priority = TaskBoardModels.TaskPriority.Medium, BoardId = 1, DueDate = DateTime.UtcNow.AddDays(7), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new TaskItem { Id = 4, Title = "Write Tests", Description = "Unit and integration tests", Status = TaskBoardModels.TaskStatus.Todo, Priority = TaskBoardModels.TaskPriority.Medium, BoardId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}
