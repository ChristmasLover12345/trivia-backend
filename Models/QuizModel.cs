using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trivia_backend.Models
{
    public class QuizModel
    {
        public int Id { get; set; }
       
        public int CreatorId { get; set; } // Foreign key

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Difficulty { get; set; }

        public int? WinScore { get; set; }
        public string? WinMessage { get; set; }
        public string? LoseMessage { get; set; }
        public Boolean IsDeleted { get; set; } 

        public  List<QuestionModel>? Questions { get; set; } 
    }
}