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
        public QuizModel? Quiz { get; set; } // Navigation property

        public string? QuestionText { get; set; } 
        public int Score { get; set; } 

        public string? CorrectAnswer { get; set; } 
        public string? WrongAnswer1 { get; set; } 
        public string? WrongAnswer2 { get; set; } 
        public string? WrongAnswer3 { get; set; } 

    }
}