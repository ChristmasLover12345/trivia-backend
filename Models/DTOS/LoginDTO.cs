using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trivia_backend.Models.DTOS
{
    public class LoginDTO
    {
        
        public string? Username { get; set; }
        public string? Password { get; set; }

    }
}