using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trivia_backend.Models;
using trivia_backend.Models.DTOS;
using trivia_backend.Services;

namespace trivia_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly QuizServices _quizServices;
        public QuizController(QuizServices quizServices)
        {
            _quizServices = quizServices;
        }

        [HttpGet("GetAllQuizzes")]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var quizzes = await _quizServices.GetAllQuizzes();

            if (quizzes == null || !quizzes.Any())
            {
                return NotFound(new { message = "No quizzes found." });
            }

            return Ok(quizzes);
        }

        [HttpGet("GetQuizzesById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetQuizzesById(int id)
        {
            var quizzes = await _quizServices.GetQuizzesByCreatorId(id);

            if (quizzes == null || !quizzes.Any())
            {
                return NotFound(new { message = $"No quizzes found for creator with ID {id}." });
            }

            return Ok(quizzes);
        }

        [HttpGet("GetQuizById/{id}")]
        [Authorize]
        public async Task<IActionResult> getQuizById(int id)
        {
            var quiz = await _quizServices.GetQuizById(id);

            if (quiz == null)
            {
                return NotFound(new { message = $"Quiz with ID {id} not found." });
            }

            return Ok(quiz);
        }

        [HttpPost("CreateQuiz")]
        [Authorize]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDTO quizDto)
        {
            if (quizDto == null)
            {
                return BadRequest(new { message = "Quiz data is null." });
            }

            if (quizDto.CreatorId <= 0)
            {
                return BadRequest(new { message = "Invalid Creator ID." });
            }

            var result = await _quizServices.CreateQuiz(quizDto);

            if (!result)
            {
                return StatusCode(500, new { message = "An error occurred while creating the quiz." });
            }

            return Ok(new { message = "Quiz created successfully." });
        }

        [HttpPut("UpdateQuiz")]
        [Authorize]
        public async Task<IActionResult> UpdateQuiz([FromBody] QuizDTO quizDto)
        {
            if (quizDto == null)
            {
                return BadRequest(new { message = "Quiz data is null." });
            }

            if (quizDto.CreatorId <= 0)
            {
                return BadRequest(new { message = "Invalid Creator ID." });
            }

            var result = await _quizServices.UpdateQuiz(quizDto);

            if (!result)
            {
                return StatusCode(500, new { message = "An error occurred while updating the quiz." });
            }

            return Ok(new { message = "Quiz updated successfully." });
        }
    }
}
