using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trivia_backend.Context;
using trivia_backend.Models;

namespace trivia_backend.Services
{
    public class QuizServices
    {
        private readonly DataContext _dataContext;
        public QuizServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<QuizModel>> GetAllQuizzes() => await _dataContext.Quizzes.Include(q => q.Questions).ToListAsync();

        public async Task<List<QuizModel>> GetQuizzesByCreatorId(int id) => await _dataContext.Quizzes.Where(q => q.CreatorId == id).Include(q => q.Questions).ToListAsync();

        public async Task<bool> CreateQuiz(QuizModel quiz)
        {
            _dataContext.Quizzes.Add(quiz);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateQuiz(QuizModel quiz)
        {
            _dataContext.Quizzes.Update(quiz);
            return await _dataContext.SaveChangesAsync() != 0;
        }

    }
}