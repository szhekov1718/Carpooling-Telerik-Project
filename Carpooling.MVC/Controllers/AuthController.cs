using System;
using System.Threading.Tasks;
using Carpooling.MVC.Models;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemo.Web.Controllers
{

    public class AuthController : Controller
    {
        private IUserService userServise;
        private ILoginService loginService;
        private UserDTO userDTO;
        private ModelMapper modelMapper;

        public AuthController(IUserService userServise, ILoginService loginService, UserDTO userDTO, ModelMapper modelMapper)
        {
            this.userServise = userServise;
            this.loginService = loginService;
            this.userDTO = userDTO;
            this.modelMapper = modelMapper;
        }

        //GET: /auth/login
        [ActionName("LoginForm")]
        public IActionResult Login()
        {
            var loginViewModel = new LoginViewModel();

            return this.View("Login", loginViewModel);
        }

        //POST: /auth/login
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username, Password")] LoginViewModel loginViewModel)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View(loginViewModel);
            }
            try
            {
                var loginDTO = this.modelMapper.ToLoginDTO(loginViewModel);
                var userKey = await this.loginService.CheckUserCredentialsAsync(loginDTO);
                var userRole = await this.userServise.GetUserByUserNameAsync(loginViewModel.Username);
                var role = "User";
                if(userRole.IsAdmin)
                {
                    role = "Admin";
                }
                this.Response.Cookies.Append("UserKey", userKey);
                this.Response.Cookies.Append("UserRole", role);
                this.Response.Cookies.Append("UserName", userRole.Username);

                return this.RedirectToAction("Index", "Home");
            }
            catch(ArgumentException ex)
            {
                this.ModelState.AddModelError("Username", ex.Message);
                this.ModelState.AddModelError("Password", ex.Message);
            }
            catch(PasswordIncorrectException ex)
            {
                this.ModelState.AddModelError("Password", ex.Message);
            }

            return this.View(loginViewModel);
        }

        //GET: /auth/logout
        public IActionResult Logout()
        {
            this.Response.Cookies.Delete("UserKey");
            this.Response.Cookies.Delete("UserRole");
            this.Response.Cookies.Delete("UserName");

            return this.RedirectToAction("index", "home");
        }

        //GET: /auth/register
        [ActionName("RegisterForm")]
        public IActionResult Register()
        {
            var registerViewModel = new CreateViewModel();

            return this.View("Register", registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Username, Password, PhoneNumber, FirstName ,LastName ,Email, ImageUrl")] CreateViewModel createViewModel)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View(createViewModel);
            }

            try
            {
                var userDTO = this.modelMapper.ToUserDTO(createViewModel);
                var user = await this.userServise.CreateUserAsync(userDTO);

                return this.RedirectToAction("Index", "Home");
            }

            catch(ArgumentException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }
            catch(UserNameExistException ex)
            {
                this.ModelState.AddModelError("Username", ex.Message);
            }
            catch(EmailIncorrectException ex)
            {
                this.ModelState.AddModelError("Email", ex.Message);
            }
            catch(EmailExistsException ex)
            {
                this.ModelState.AddModelError("Email", ex.Message);
            }
            catch(NudeAvatarException ex)
            {
                this.ModelState.AddModelError("ImageUrl", ex.Message);
            }

            return this.View(createViewModel);
        }
    }
}
