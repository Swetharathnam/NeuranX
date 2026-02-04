using Microsoft.AspNetCore.Mvc;
using TaskBoard.DTOs;
using TaskBoard.Exceptions;
using TaskBoard.Services;

namespace TaskBoard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly ILogger<BoardsController> _logger;

        public BoardsController(IBoardService boardService, ILogger<BoardsController> logger)
        {
            _boardService = boardService;
            _logger = logger;
        }

        /// <summary>
        /// Get all boards
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BoardDto>>> GetAllBoards()
        {
            try
            {
                var boards = await _boardService.GetAllBoardsAsync();
                return Ok(boards);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching boards: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        /// <summary>
        /// Get board by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BoardDto>> GetBoardById(int id)
        {
            try
            {
                var board = await _boardService.GetBoardByIdAsync(id);
                return Ok(board);
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning($"Board not found: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching board: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new board
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BoardDto>> CreateBoard([FromBody] CreateBoardDto createBoardDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createBoardDto.Name))
                {
                    return BadRequest("Board name is required");
                }

                var board = await _boardService.CreateBoardAsync(createBoardDto);
                return CreatedAtAction(nameof(GetBoardById), new { id = board.Id }, board);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating board: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing board
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BoardDto>> UpdateBoard(int id, [FromBody] UpdateBoardDto updateBoardDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(updateBoardDto.Name))
                {
                    return BadRequest("Board name is required");
                }

                var board = await _boardService.UpdateBoardAsync(id, updateBoardDto);
                return Ok(board);
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning($"Board not found: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating board: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a board
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            try
            {
                await _boardService.DeleteBoardAsync(id);
                return NoContent();
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning($"Board not found: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting board: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
