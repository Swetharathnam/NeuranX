using Microsoft.EntityFrameworkCore;
using TaskBoard.Data;
using TaskBoard.DTOs;
using TaskBoard.Exceptions;
using TaskBoard.Models;

namespace TaskBoard.Services
{
    public class BoardService : IBoardService
    {
        private readonly TaskBoardContext _context;
        private readonly ILogger<BoardService> _logger;

        public BoardService(TaskBoardContext context, ILogger<BoardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<BoardDto>> GetAllBoardsAsync()
        {
            _logger.LogInformation("Fetching all boards");
            var boards = await _context.Boards
                .Include(b => b.Tasks)
                .ToListAsync();

            return boards.Select(MapToDto).ToList();
        }

        public async Task<BoardDto> GetBoardByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching board with ID: {id}");
            var board = await _context.Boards
                .Include(b => b.Tasks)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (board == null)
            {
                _logger.LogWarning($"Board with ID {id} not found");
                throw new ResourceNotFoundException($"Board with ID {id} not found");
            }

            return MapToDto(board);
        }

        public async Task<BoardDto> CreateBoardAsync(CreateBoardDto createBoardDto)
        {
            _logger.LogInformation($"Creating new board: {createBoardDto.Name}");

            var board = new Board
            {
                Name = createBoardDto.Name,
                Description = createBoardDto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Board created with ID: {board.Id}");
            return MapToDto(board);
        }

        public async Task<BoardDto> UpdateBoardAsync(int id, UpdateBoardDto updateBoardDto)
        {
            _logger.LogInformation($"Updating board with ID: {id}");
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == id);

            if (board == null)
            {
                _logger.LogWarning($"Board with ID {id} not found");
                throw new ResourceNotFoundException($"Board with ID {id} not found");
            }

            board.Name = updateBoardDto.Name;
            board.Description = updateBoardDto.Description;
            board.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Board with ID {id} updated successfully");

            return MapToDto(board);
        }

        public async Task DeleteBoardAsync(int id)
        {
            _logger.LogInformation($"Deleting board with ID: {id}");
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == id);

            if (board == null)
            {
                _logger.LogWarning($"Board with ID {id} not found");
                throw new ResourceNotFoundException($"Board with ID {id} not found");
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Board with ID {id} deleted successfully");
        }

        private BoardDto MapToDto(Board board)
        {
            return new BoardDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                CreatedAt = board.CreatedAt,
                UpdatedAt = board.UpdatedAt,
                Tasks = board.Tasks.Select(t => new TaskItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    BoardId = t.BoardId,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                }).ToList()
            };
        }
    }
}
