using TaskBoard.DTOs;

namespace TaskBoard.Services
{
    public interface ITaskItemService
    {
        Task<List<TaskItemDto>> GetAllTasksAsync();
        Task<List<TaskItemDto>> GetTasksByBoardIdAsync(int boardId);
        Task<TaskItemDto> GetTaskByIdAsync(int id);
        Task<TaskItemDto> CreateTaskAsync(CreateTaskItemDto createTaskDto);
        Task<TaskItemDto> UpdateTaskAsync(int id, UpdateTaskItemDto updateTaskDto);
        Task DeleteTaskAsync(int id);
    }
}
