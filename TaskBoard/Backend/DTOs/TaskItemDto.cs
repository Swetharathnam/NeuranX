using TaskBoardModels = TaskBoard.Models;

namespace TaskBoard.DTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskBoardModels.TaskStatus Status { get; set; }
        public TaskBoardModels.TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int BoardId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskBoardModels.TaskStatus Status { get; set; } = TaskBoardModels.TaskStatus.Todo;
        public TaskBoardModels.TaskPriority Priority { get; set; } = TaskBoardModels.TaskPriority.Medium;
        public DateTime? DueDate { get; set; }
        public int BoardId { get; set; }
    }

    public class UpdateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskBoardModels.TaskStatus Status { get; set; }
        public TaskBoardModels.TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
