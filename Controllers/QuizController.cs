using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trivia_backend.Models;
using trivia_backend.Services;

namespace trivia_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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

            if (quizzes == null )
            {
                return NotFound("No quizzes found.");
            }

            return Ok(quizzes);
        }

        [HttpGet("GetQuizzesById/{id}")]
        public async Task<IActionResult> GetQuizzesById(int id)
        {
            var quizzes = await _quizServices.GetQuizzesByCreatorId(id);

            if (quizzes == null || quizzes.Count == 0)
            {
                return NotFound($"No quizzes found with ID {id}.");
            }

            return Ok(quizzes);
        }

        [HttpPost("CreateQuiz")]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizModel quiz)
        {
            if (quiz == null)
            {
                return BadRequest("Quiz data is null.");
            }

            if (quiz.CreatorId <= 0)
            {
                return BadRequest("Invalid Creator ID.");
            }

            var result = await _quizServices.CreateQuiz(quiz);

            if (!result)
            {
                return StatusCode(500, "An error occurred while creating the quiz.");
            }

            return Ok(new { message = "Quiz created successfully." });
        }

        [HttpPut("UpdateQuiz")]
        public async Task<IActionResult> UpdateQuiz([FromBody] QuizModel quiz)
        {
            if (quiz == null)
            {
                return BadRequest("Quiz data is null.");
            }

             if (quiz.CreatorId <= 0)
            {
                return BadRequest("Invalid Creator ID.");
            }

            var result = await _quizServices.UpdateQuiz(quiz);

            if (!result)
            {
                return StatusCode(500, "An error occurred while updating the quiz.");
            }

            return Ok(new {message = "Quiz updated successfully."});
        }

    }
}