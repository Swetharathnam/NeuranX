using TaskBoard.DTOs;

namespace TaskBoard.Services
{
    public interface IBoardService
    {
        Task<List<BoardDto>> GetAllBoardsAsync();
        Task<BoardDto> GetBoardByIdAsync(int id);
        Task<BoardDto> CreateBoardAsync(CreateBoardDto createBoardDto);
        Task<BoardDto> UpdateBoardAsync(int id, UpdateBoardDto updateBoardDto);
        Task DeleteBoardAsync(int id);
    }
}
