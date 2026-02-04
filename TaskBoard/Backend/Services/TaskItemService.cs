using Microsoft.EntityFrameworkCore;
using TaskBoard.Data;
using TaskBoard.DTOs;
using TaskBoard.Exceptions;
using TaskBoard.Models;

namespace TaskBoard.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly TaskBoardContext _context;
        private readonly ILogger<TaskItemService> _logger;

        public TaskItemService(TaskBoardContext context, ILogger<TaskItemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TaskItemDto>> GetAllTasksAsync()
        {
            _logger.LogInformation("Fetching all tasks");
            var tasks = await _context.Tasks.ToListAsync();
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<List<TaskItemDto>> GetTasksByBoardIdAsync(int boardId)
        {
            _logger.LogInformation($"Fetching tasks for board ID: {boardId}");
            
            var boardExists = await _context.Boards.AnyAsync(b => b.Id == boardId);
            if (!boardExists)
            {
                _logger.LogWarning($"Board with ID {boardId} not found");
                throw new ResourceNotFoundException($"Board with ID {boardId} not found");
            }

            var tasks = await _context.Tasks
                .Where(t => t.BoardId == boardId)
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskItemDto> GetTaskByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching task with ID: {id}");
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                _logger.LogWarning($"Task with ID {id} not found");
                throw new ResourceNotFoundException($"Task with ID {id} not found");
            }

            return MapToDto(task);
        }

        public async Task<TaskItemDto> CreateTaskAsync(CreateTaskItemDto createTaskDto)
        {
            _logger.LogInformation($"Creating new task: {createTaskDto.Title}");

            var boardExists = await _context.Boards.AnyAsync(b => b.Id == createTaskDto.BoardId);
            if (!boardExists)
            {
                _logger.LogWarning($"Board with ID {createTaskDto.BoardId} not found");
                throw new ResourceNotFoundException($"Board with ID {createTaskDto.BoardId} not found");
            }

            var task = new TaskItem
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                Status = createTaskDto.Status,
                Priority = createTaskDto.Priority,
                DueDate = createTaskDto.DueDate,
                BoardId = createTaskDto.BoardId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Task created with ID: {task.Id}");
            return MapToDto(task);
        }

        public async Task<TaskItemDto> UpdateTaskAsync(int id, UpdateTaskItemDto updateTaskDto)
        {
            _logger.LogInformation($"Updating task with ID: {id}");
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                _logger.LogWarning($"Task with ID {id} not found");
                throw new ResourceNotFoundException($"Task with ID {id} not found");
            }

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.Status = updateTaskDto.Status;
            task.Priority = updateTaskDto.Priority;
            task.DueDate = updateTaskDto.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Task with ID {id} updated successfully");

            return MapToDto(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            _logger.LogInformation($"Deleting task with ID: {id}");
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                _logger.LogWarning($"Task with ID {id} not found");
                throw new ResourceNotFoundException($"Task with ID {id} not found");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Task with ID {id} deleted successfully");
        }

        private TaskItemDto MapToDto(TaskItem task)
        {
            return new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                BoardId = task.BoardId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}
