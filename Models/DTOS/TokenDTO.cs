using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trivia_backend.Models.DTOS
{
    public class TokenDTO
    {
        public string? Token { get; set; }
        public int userId { get; set; }
        public string? Username { get; set; }
        public List<QuizModel>? Quizzes { get; set; } 
    }
}