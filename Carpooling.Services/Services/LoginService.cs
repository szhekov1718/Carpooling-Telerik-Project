using System;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Models;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.EntityFrameworkCore;


namespace Carpooling.Services.Services
{
    public class LoginService : ILoginService
    {
        private readonly CarpoolingContext carpoolingContext;
        private readonly IUserService userService;

        public LoginService(CarpoolingContext carpoolingContext, IUserService userService)
        {
            this.carpoolingContext = carpoolingContext;
            this.userService = userService;
        }

        public async Task<User> CheckUsersPasswordKeyAsync(string key)
        {
            var user = await this.carpoolingContext.Users
                .Where(u => u.Password == key)
                .FirstOrDefaultAsync();

            if(user != null)
            {
                return user;
            }

            throw new AuthorisationException(ExceptionMessages.InvalidUserKey);
        }

        public async Task<string> CheckUserCredentialsAsync(LoginDTO loginDTO)
        {
            var user = await this.userService.GetUserByUserNameAsync(loginDTO.Username);

            if(user == null || loginDTO.Password == null)
            {
                throw new ArgumentException(ExceptionMessages.InvalidCredentials);
            }

            var password = this.userService.HashPassword(loginDTO.Password);

            if(password != user.Password)
            {
                throw new PasswordIncorrectException(ExceptionMessages.InvalidPassword);
            }

            return password;
        }
    }
}
