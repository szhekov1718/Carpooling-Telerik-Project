using System;
using System.Threading.Tasks;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksController : ControllerBase
    {
        private IFeedbackService feedbackService;
        private ILoginService loginService;

        public FeedbacksController(IFeedbackService feedbackService, ILoginService loginService)
        {
            this.feedbackService = feedbackService;
            this.loginService = loginService;
        }

        /// <summary>
        /// Creates a feedback in the system
        /// </summary>
        /// <param name="createFeedbackDTO"></param>
        /// <returns>The created feedback</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackDTO createFeedbackDTO)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                if(createFeedbackDTO.Username != userData.Username)
                {
                    return this.BadRequest(ExceptionMessages.InvalidAction);
                }

                var feedback = await this.feedbackService.CreateFeedbackAsync(createFeedbackDTO);

                return this.Ok(feedback);
            }
            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Deletes a feedback from the system
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns>Boolean value true, when the feedback is deleted</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpDelete("")]
        public async Task<IActionResult> Delete([FromBody] Guid feedbackId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var feedback = await this.feedbackService.DeleteAsync(userData.Id, feedbackId);

                return this.Ok(feedback);
            }
            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all feedback
        /// </summary>
        /// <returns>A collection of all feedbacks</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var feedback = await this.feedbackService.ListAllFeedbacksAsync();

                return this.Ok(feedback);
            }
            return this.BadRequest(ExceptionMessages.LoginRequired);
        }
    }
}
