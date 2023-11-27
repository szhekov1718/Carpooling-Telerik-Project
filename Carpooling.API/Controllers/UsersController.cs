using System;
using System.Threading.Tasks;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Enums;
using Carpooling.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILoginService loginService;

        public UsersController(IUserService userService, ILoginService loginService)
        {
            this.userService = userService;
            this.loginService = loginService;
        }

        /// <summary>
        /// Creating a user in the system
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input"></param>
        /// <returns>Created user</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] UserDTO model)
        {
            var user = await this.userService.CreateUserAsync(model);

            return Created("post", user);
        }

        /// <summary>
        /// Deleting a user from the system
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Ok result</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpDelete("")]
        public async Task<IActionResult> Delete(string username)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                if(username != userData.Username)
                {
                    return this.BadRequest(ExceptionMessages.NoAuthority);
                }

                var user = await this.userService.DeleteUserAsync(username);

                return this.Ok();
            }
            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Finds a user by their username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The user with that username</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var user = this.userService.GetBy(username, FilterBy.Username.ToString());

                if(user != null)
                {
                    return Ok(user);
                }

                return this.BadRequest();
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Finds a user by their email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>The user with that email</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var user = this.userService.GetBy(email, FilterBy.Email.ToString());

                if(user != null)
                {
                    return Ok(user);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Finds a user by their phone number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns>The user with that phone number</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpGet("{phonenumber}")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var user = this.userService.GetBy(phone, FilterBy.PhoneNumber.ToString());

                if(user != null)
                {
                    return Ok(user);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all users
        /// </summary>
        /// <returns>Returns a collection of all users</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var user = await this.userService.GetAllUsersAsync();

            return Ok(user);
        }

        /// <summary>
        /// Updates a user's details
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns the updated user</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] UserDTO model)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var user = await this.userService.UpdateUserAsync(userData.Username, model);

                return Ok(user);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Blocks a user from applying to trips
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The blocked user</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpPatch("block")]
        public async Task<IActionResult> BlockUserAsync([FromQuery] string username)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                if(!userData.IsAdmin)
                {
                    return this.BadRequest(ExceptionMessages.NoAuthority);
                }

                var user = await this.userService.BlockUnblockUserAsync(username, "Block");

                return Ok(user);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Unblocks a user from applying to trips
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The unblocked user</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpPatch("unblock")]
        public async Task<IActionResult> UnblockUserAsync([FromQuery] string username)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                if(!userData.IsAdmin)
                {
                    return this.BadRequest(ExceptionMessages.NoAuthority);
                }

                var user = await this.userService.BlockUnblockUserAsync(username, "Unblock");

                return Ok(user);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Promotes a user to Admin role
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Boolean value true, when the user is now an admin</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpPatch("admin/{userId}")]
        public async Task<IActionResult> MakeUserAdmin(string username)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                if(!userData.IsAdmin)
                {
                    return this.BadRequest(ExceptionMessages.NoAuthority);
                }

                var user = await this.userService.MakeUserAdmin(username);

                return Ok(user);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// The driver marks a trip as completed
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>The travel status of the trip</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpPut("trip/complete/{tripId}")]
        public async Task<IActionResult> DriverMarkTripCompleted(Guid tripId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var user = await this.userService.DriverMarkTripCompleted(userData.Id, tripId);

                return Ok(user);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }
    }
}
