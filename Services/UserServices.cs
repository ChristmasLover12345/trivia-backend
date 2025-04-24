using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trivia_backend.Context;
using trivia_backend.Models;
using trivia_backend.Models.DTOS;

namespace trivia_backend.Services
{
    public class UserServices
    {
        
        private readonly DataContext _dataContext;
        public UserServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> CreateUser(LoginDTO newUser )
        {
            bool result = false;
            if(!await DoesUserExist(newUser.Username))
            {

                UserModel userToAdd = new();
                userToAdd.Username = newUser.Username;
                

            }

        }

       private async Task<bool> DoesUserExist(string username)
        {
            return await _dataContext.Users.AnyAsync(u => u.Username == username);
        }


    }
}