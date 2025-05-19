using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trivia_backend.Context;
using trivia_backend.Models;
using trivia_backend.Models.DTOS;

namespace trivia_backend.Services
{
    public class QuizServices
    {
        private readonly DataContext _dataContext;

        public QuizServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<QuizDTO>> GetAllQuizzes()
        {
            var quizzes = await _dataContext.Quizzes.Include(q => q.Questions).ToListAsync();
            return quizzes.Select(q => ToDTO(q)).ToList();
        }

        public async Task<List<QuizDTO>> GetQuizzesByCreatorId(int creatorId)
        {
            var quizzes = await _dataContext.Quizzes
                .Where(q => q.CreatorId == creatorId)
                .Include(q => q.Questions)
                .ToListAsync();
            return quizzes.Select(q => ToDTO(q)).ToList();
        }

        public async Task<bool> CreateQuiz(CreateQuizDTO createQuizDTO)
        {
            var quiz = ToModel(createQuizDTO);
            _dataContext.Quizzes.Add(quiz);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateQuiz(QuizDTO quizDTO)
        {
            var quiz = await _dataContext.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizDTO.Id);

            if (quiz == null)
                return false;

            // Update quiz properties
            quiz.Title = quizDTO.Title;
            quiz.Description = quizDTO.Description;
            quiz.ImageUrl = quizDTO.ImageUrl;
            quiz.Difficulty = quizDTO.Difficulty;
            quiz.WinScore = quizDTO.WinScore;
            quiz.WinMessage = quizDTO.WinMessage;
            quiz.LoseMessage = quizDTO.LoseMessage;

            // Handle questions update - depends on your logic (add, update, delete)

            _dataContext.Quizzes.Update(quiz);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        // Mapping methods

        private QuizDTO ToDTO(QuizModel quiz) => new QuizDTO
        {
            Id = quiz.Id,
            CreatorId = quiz.CreatorId,
            Title = quiz.Title,
            Description = quiz.Description,
            ImageUrl = quiz.ImageUrl,
            Difficulty = quiz.Difficulty,
            WinScore = quiz.WinScore,
            WinMessage = quiz.WinMessage,
            LoseMessage = quiz.LoseMessage,
            Questions = quiz.Questions?.Select(q => new QuestionDTO
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                Score = q.Score,
                CorrectAnswer = q.CorrectAnswer,
                WrongAnswer1 = q.WrongAnswer1,
                WrongAnswer2 = q.WrongAnswer2,
                WrongAnswer3 = q.WrongAnswer3
            }).ToList()
        };

        private QuizModel ToModel(CreateQuizDTO dto) => new QuizModel
        {
            CreatorId = dto.CreatorId,
            Title = dto.Title,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            Difficulty = dto.Difficulty,
            WinScore = dto.WinScore,
            WinMessage = dto.WinMessage,
            LoseMessage = dto.LoseMessage,
            Questions = dto.Questions?.Select(q => new QuestionModel
            {
                QuestionText = q.QuestionText,
                Score = q.Score,
                CorrectAnswer = q.CorrectAnswer,
                WrongAnswer1 = q.WrongAnswer1,
                WrongAnswer2 = q.WrongAnswer2,
                WrongAnswer3 = q.WrongAnswer3
            }).ToList()
        };

    }
}