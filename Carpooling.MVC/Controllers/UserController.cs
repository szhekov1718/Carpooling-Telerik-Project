using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.MVC.Models;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILoginService loginService;
        private readonly ModelMapper modelMapper;
        private readonly IUserService userService;

        public UserController(IUserService userService, ILoginService loginService, ModelMapper modelMapper)
        {
            this.loginService = loginService;
            this.modelMapper = modelMapper;
            this.userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }
            if(this.Request.Cookies["UserRole"] != "Admin")
            {
                this.ViewBag.Error = ExceptionMessages.NoAuthority;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return this.View("Error");
            }
            var usersView = await GetAllUsers();

            return this.View(usersView);
        }

        public async Task<IActionResult> Update()
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
            var model = this.modelMapper.ToUserDTO(userData);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update([Bind("Username, Password, PhoneNumber, FirstName, LastName, Email, ImageUrl")] UserDTO model)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var key = this.Request.Cookies["UserKey"];

                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var trip = await this.userService.UpdateUserAsync(userData.Username, model);

                return this.RedirectToAction(nameof(Index), "Home");
            }
            catch(ArgumentException ex)
            {
                if(ex.Message == "Email already exists.")
                {
                    this.ModelState.AddModelError("Email", ex.Message);
                }
                else
                {
                    this.ModelState.AddModelError("Username", ex.Message);
                }
            }
            catch(UserNameExistException ex)
            {
                this.ModelState.AddModelError("Username", ex.Message);
            }
            catch(EmailIncorrectException ex)
            {
                this.ModelState.AddModelError("Email", ex.Message);
            }
            catch(NudeAvatarException ex)
            {
                this.ModelState.AddModelError("ImageUrl", ex.Message);
            }

            return this.View(model);
        }
        public async Task<IActionResult> BlockUser(string username)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }
            if(this.Request.Cookies["UserRole"] != "Admin")
            {
                this.ViewBag.Error = ExceptionMessages.NoAuthority;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return this.View("Error");
            }

            var users = await this.userService.BlockUnblockUserAsync(username, "Block");

            return this.RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> UnblockUser(string username)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

            if(this.Request.Cookies["UserRole"] != "Admin" || userData.Username == username)
            {
                this.ViewBag.Error = ExceptionMessages.NoAuthority;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return this.View("Error");
            }

            var users = await this.userService.BlockUnblockUserAsync(username, "Unblock");

            return this.RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> MakeAdmin(string username)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }
            if(this.Request.Cookies["UserRole"] != "Admin")
            {
                this.ViewBag.Error = ExceptionMessages.NoAuthority;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return this.View("Error");
            }

            var users = await this.userService.MakeUserAdmin(username);

            return this.RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> Delete(string username)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }
            if(this.Request.Cookies["UserRole"] != "Admin")
            {
                this.ViewBag.Error = ExceptionMessages.NoAuthority;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return this.View("Error");
            }

            var users = await this.userService.DeleteUserAsync(username);

            return this.RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> SearchBy(UserView userView)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }
            if(this.Request.Cookies["UserRole"] != "Admin")
            {
                this.ViewBag.Error = ExceptionMessages.NoAuthority;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return this.View("Error");
            }
            ICollection<UserDTO> user = new Collection<UserDTO>();
            switch(userView.SearchAttribute)
            {
                case "Username":
                user.Add(this.userService.GetBy(userView.Value, userView.SearchAttribute));
                break;

                case "Email":
                user.Add(this.userService.GetBy(userView.Value, userView.SearchAttribute));
                break;

                default:
                user.Add(this.userService.GetBy(userView.Value, userView.SearchAttribute));
                break;
            }

            if(user.ToList().FirstOrDefault() == null)
            {
                ModelState.AddModelError("Value", ExceptionMessages.EntityNotFound);
                var usersView = await GetAllUsers();

                return this.View("Index", usersView);
            }

            userView = this.modelMapper.ToUserView(user);

            return this.View("Index", userView);
        }

        public async Task<UserView> GetAllUsers()
        {
            var users = await this.userService.GetAllUsersAsync();
            var usersView = this.modelMapper.ToUserView(users);

            return usersView;
        }
    }
}
