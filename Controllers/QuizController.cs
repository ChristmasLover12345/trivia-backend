using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trivia_backend.Services;

namespace trivia_backend.Controllers
{
    public class QuizController
    {
        

        private readonly QuizServices _quizServices;
        public QuizController(QuizServices quizServices)
        {
            _quizServices = quizServices;
        }

        

    }
}