using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trivia_backend.Models.DTOS
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public int Score { get; set; }

        public string? CorrectAnswer { get; set; }
        public string? WrongAnswer1 { get; set; }
        public string? WrongAnswer2 { get; set; }
        public string? WrongAnswer3 { get; set; }
    }
}