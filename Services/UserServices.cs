using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<bool> CreateUser(LoginDTO newUser )
        {
            bool result = false;
            if(!await DoesUserExist(newUser.Username))
            {

                UserModel userToAdd = new();
                userToAdd.Username = newUser.Username;

                PasswordDTO hasedPassord = HashPassword(newUser.Password);
                userToAdd.Hash = hasedPassord.Hash;
                userToAdd.Salt = hasedPassord.Salt;

                _dataContext.Users.Add(userToAdd);
                result = await _dataContext.SaveChangesAsync() !=0;

                result = true;


            }

            return result;

        }

       private async Task<bool> DoesUserExist(string username)
        {
            return await _dataContext.Users.AnyAsync(u => u.Username == username);
        }

        private static PasswordDTO HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(saltBytes);
            string hash;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256)){
                hash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }

            PasswordDTO hashedPassword = new();
            hashedPassword.Salt = salt;
            hashedPassword.Hash = hash;
            return hashedPassword;

        }


        public async Task<TokenDTO> Login(LoginDTO user)
        {
            TokenDTO result = new();

            UserModel foundUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if(foundUser == null)
            {
                return result;
            }

            if (VerifyPassword(user.Password, foundUser.Salt, foundUser.Hash))
            {

                // JWT: JSON web token = a type of token used for authentication or transfering information
                // Bearer Token: A token that grants access to a resource, such as an API. JWT can be used as a bearer token, but there are other types of tokens that can be used as a bearer token.

                // Setting the string that will be encrypted int our JWT
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345superSecretKey@345"));

                // Now to encrypt our secret key
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                // Set the options for our token to define properties such as where the token is issued from, where it is allowed to be used, and how long it is valid for
                var tokenOptions = new JwtSecurityToken(
                    // Issuer: where is this token allowed to be generated from
                    issuer: "trivia-api-g3d7dwczhma0hzdt.westus-01.azurewebsites.net",
                    // audience: where this token is allowed to authenticate.
                    // issuer and audience should be the same since our api os handling both login and authentication
                    audience: "trivia-api-g3d7dwczhma0hzdt.westus-01.azurewebsites.net",
                    // Claims = additional options for authentication
                    claims: new List<Claim>(),
                    // Sets the token expiration date. in other words, this is what makes our tokens temporary, thus keeping our access to our rescources safe and secure
                    expires: DateTime.Now.AddMinutes(60),
                    // This attaches our newly encrypted super secret key that was turned into sign on credentials.
                    signingCredentials: signingCredentials

                );

                // Generate our JWT and save the token as a string into a variable
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                result.Token = tokenString;
                result.userId = foundUser.Id;
                result.Username = foundUser.Username;
                
            }

            return result;
        }

        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            string newHash; 
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                newHash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }

            return newHash == hash;

        }


        public async Task<UserModel> GetUserById(int id) => await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}