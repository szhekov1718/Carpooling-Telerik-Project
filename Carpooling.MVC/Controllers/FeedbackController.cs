using System;
using System.Threading.Tasks;
using Carpooling.MVC.Models;
using Carpooling.Services;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.MVC.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ILoginService loginService;
        private readonly ModelMapper modelMapper;
        private readonly IFeedbackService feedbackService;
        private readonly ITripService tripService;
        public FeedbackController(ITripService tripService, IFeedbackService feedbackService, ILoginService loginService, ModelMapper modelMapper)
        {
            this.tripService = tripService;
            this.loginService = loginService;
            this.modelMapper = modelMapper;
            this.feedbackService = feedbackService;
        }

        public async Task<IActionResult> Index()
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
            var userFeedbacks = await this.feedbackService.ListAllUserFeedbacksAsync(userData.Id);
            var foodbackView = this.modelMapper.ToFeedbackViewFromFeedback(userFeedbacks);

            return this.View("UserFeedbacks", foodbackView);
        }

        public async Task<IActionResult> FeedbackForMe()
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
            var feedbackForUser = await this.feedbackService.ListAllFeedbacksForUserAsync(userData.Id);
            var foodbackView = this.modelMapper.ToFeedbackViewFromFeedback(feedbackForUser);

            return this.View("FeedbackForUser", foodbackView);
        }

        [ActionName("EditForm")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            try
            {
                var feedback = await this.feedbackService.GetFeedbackAsync(Id);
                var feedbackView = new FeedbackViewModel(feedback);

                return this.View("Edit", feedbackView);
            }
            catch(FeedbackException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status404NotFound;

                return this.View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FeedbackViewModel feedbackViewModel)
        {
            if(!ModelState.IsValid)
            {
                return this.View(feedbackViewModel);
            }

            var feedback = this.modelMapper.ToFeedbackFromFeedbackViewModel(feedbackViewModel);
            await this.feedbackService.EditFeedbackAsync(feedback);

            return this.RedirectToAction("Index");
        }

        public IActionResult LeaveFeedbackForm(Guid Id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            FeedbackViewModel commentData = new FeedbackViewModel();
            commentData.TripId = Id;

            return this.View("AddFeedback", commentData);
        }

        [HttpPost]
        public async Task<IActionResult> LeaveFeedback(FeedbackViewModel feedbackData)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];

            try
            {
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var feedback = new CreateFeedbackDTO()
                {
                    Rating = feedbackData.Rating.ToString(),
                    Description = feedbackData.Comment,
                    TripId = feedbackData.TripId,
                    Username = userData.Username
                };

                var result = await this.feedbackService.CreateFeedbackAsync(feedback);

                return this.RedirectToAction("MyTrips", "Trip");
            }
            catch(EntityNotFound ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
