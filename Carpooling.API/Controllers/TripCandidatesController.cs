using System;
using System.Threading.Tasks;
using Carpooling.Services.Contracts;
using Carpooling.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripCandidatesController : ControllerBase
    {
        private readonly ITripCandidateService tripCandidateService;
        private readonly ILoginService loginService;
        public TripCandidatesController(ITripCandidateService tripCandidateService, ILoginService loginService)
        {
            this.tripCandidateService = tripCandidateService;
            this.loginService = loginService;
        }

        /// <summary>
        /// Lists all trip candidates
        /// </summary>
        /// <returns>Collection of all trip candidates</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpGet("")]
        public async Task<IActionResult> GetAllTripCandidates()
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var result = await this.tripCandidateService.GetAllTripCandidatesAsync();

                return Ok(result);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Approves the trip candidate for the trip
        /// </summary>
        /// <param name="candidateId"></param>
        /// <param name="tripId"></param>
        /// <returns>Boolean value true, when the candidate is approved</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpPatch("approve")]
        public async Task<IActionResult> ApproveCandidate([FromQuery] Guid candidateId, Guid tripId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var result = await this.tripCandidateService.ApproveCandidateAsync(candidateId, tripId, userData.Id);

                return Ok(result);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Rejects the trip candidate from the trip
        /// </summary>
        /// <param name="candidateId"></param>
        /// <param name="tripId"></param>
        /// <returns>Boolean value false, when the candidate is rejected</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpPatch("reject")]
        public async Task<IActionResult> RejectCandidate([FromQuery] Guid candidateId, Guid tripId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var result = await this.tripCandidateService.RejectCandidateAsync(candidateId, tripId, userData.Id);

                return Ok(result);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all approved candidates for the trip
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>A list of approved candidates for the trip</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("{tripId}/users/approved")]
        public async Task<IActionResult> ListTripApprovedCandidates(Guid tripId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var result = await this.tripCandidateService.ListTripApprovedCandidatesAsync(tripId);

                return Ok(result);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all rejected candidates for the trip
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>A list of rejected candidates for the trip</returns>
        /// <Author>Stanimir Zhekov</Author>
        [HttpGet("{tripId}/users/rejected")]
        public async Task<IActionResult> ListTripRejectedCandidates(Guid tripId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var result = await this.tripCandidateService.ListTripRejectedCandidatesAsync(tripId);

                return Ok(result);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }
    }

}
