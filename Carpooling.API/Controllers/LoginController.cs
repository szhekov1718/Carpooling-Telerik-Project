using System;
using System.Threading.Tasks;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        public readonly IUserService userServise;
        private readonly ILoginService loginService;

        public AccountsController(IUserService userServise, ILoginService loginService)
        {
            this.userServise = userServise;
            this.loginService = loginService;
        }

        /// <summary>
        /// Logs a user in the system
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Successfully logged in message</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Login([FromHeader] string username, [FromHeader] string password)
        {
            if(username == null || password == null)
            {
                return this.BadRequest(ExceptionMessages.CredentialsRequired);
            }

            var credentials = new LoginDTO(username, password);
            var userKey = await this.loginService.CheckUserCredentialsAsync(credentials);
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            this.Response.Cookies.Append("Login", userKey, options);

            return this.Ok("You login successful.");
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Logout()
        {
            this.Response.Cookies.Delete("Login");

            return this.Ok("You logout successful.");
        }

        /// <summary>
        /// Registers a user in the system
        /// </summary>
        /// <param name="userData"></param>
        /// <returns>Successful registration message</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userData)
        {
            if(userData == null)
            {
                return this.BadRequest(ExceptionMessages.InvalidRegisterData);
            }

            await this.userServise.CreateUserAsync(userData);

            return this.Ok("You registered successfully.");
        }
    }
}
