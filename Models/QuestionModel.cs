using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trivia_backend.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public int QuizId { get; set; } // Foreign key

        public string? QuestionText { get; set; } 
        public int Score { get; set; } = 10;

        public string CorrectAnswer { get; set; } = string.Empty;
        public string WrongAnswer1 { get; set; } = string.Empty;
        public string WrongAnswer2 { get; set; } = string.Empty;
        public string WrongAnswer3 { get; set; } = string.Empty;

        public QuizModel? Quiz { get; set; } 


    }
}