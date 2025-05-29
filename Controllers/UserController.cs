using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trivia_backend.Models;
using trivia_backend.Models.DTOS;
using trivia_backend.Services;

namespace trivia_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDTO newUser)
        {
            bool result = await _userServices.CreateUser(newUser);

            if (!result)
            {
                return BadRequest(new{ message = "Username already exists or user creation failed."});
            } 

            return Ok(new {message = "User registered successfully."});
        }

        
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginUser)
        {
            var result = await _userServices.Login(loginUser);

            if (result == null || result.Token == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }
            

            return Ok(result);
        }



      
        [HttpGet("GetUser/{id}")]
        [Authorize]
        public async Task<ActionResult> GetUserById(int id)
        {
            var user = await _userServices.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            return Ok(user);
        }

    }
}