using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trivia_backend.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<QuizModel>? Quizzes { get; set; } 
        public string? Salt { get; set; }
        public string? Hash { get; set; }
    }
}