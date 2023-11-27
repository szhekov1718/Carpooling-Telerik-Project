using System;
using System.Threading.Tasks;
using Carpooling.Services;
using Carpooling.Services.Contracts;
using Carpooling.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService tripService;
        private readonly ILoginService loginService;
        public TripsController(ITripService tripService, ILoginService loginService)
        {
            this.tripService = tripService;
            this.loginService = loginService;
        }

        /// <summary>
        /// Creates a new trip in the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The created trip</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] TripDTO model)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                var trip = await this.tripService.CreateAsync(model, userData.Id);

                return Created("post", trip);
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Deletes a specific trip in the system
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>A boolean value true, when the trip is deleted from the system</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpDelete("")]
        public async Task<IActionResult> Delete([FromQuery] Guid tripId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var trip = await this.tripService.DeleteAsync(tripId);

                    return this.Ok(trip);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Finds a specific trip by ID
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns>The specified trip</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("{tripId}")]
        public async Task<IActionResult> Get(Guid tripId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var trip = await this.tripService.GetTripAsync(tripId);

                    return Ok(trip);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all existing trips in the system
        /// </summary>
        /// <returns>A collection of all existing trips in the system</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var result = await this.tripService.GetAllTripsAsync();

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Updates an already existing trip
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>The updated trip</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpPut("")]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] TripDTO model)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var result = await this.tripService.UpdateAsync(model, id);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// A user applies for a specified trip
        /// </summary>
        /// <param name="username"></param>
        /// <param name="tripId"></param>
        /// <returns>The trip for which the user has applied</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("{tripId}/apply/users/{username}")]
        public async Task<IActionResult> ApplyForTrip(string username, Guid tripId)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                    if(username != userData.Username)
                    {
                        return this.BadRequest(ExceptionMessages.NoAuthority);
                    }

                    var result = await this.tripService.ApplyForTripAsync(username, tripId);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all available trips in the system
        /// </summary>
        /// <returns>A collection of all available trips in the system</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("available")]
        public async Task<IActionResult> GetAllAvailableTrips()
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var result = await this.tripService.GetAllAvailableTripsAsync();

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all trips/all available trips for a specific start destination
        /// </summary>
        /// <param name="startDestination"></param>
        /// <param name="allOrAvailableTrips"></param>
        /// <returns>A list of the filtered trips for this start destination</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("filter/destination")]
        public async Task<IActionResult> FilterTripsByStartDestination([FromQuery] string startDestination, string allOrAvailableTrips)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var result = await this.tripService.FilterTripsByStartDestinationAsync(startDestination, allOrAvailableTrips);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all trips/all available trips for a specific departure time
        /// </summary>
        /// <param name="departure"></param>
        /// <param name="allOrAvailableTrips"></param>
        /// <returns>A list of the filtered trips for this departure time</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("filter/departure")]
        public async Task<IActionResult> FilterTripsByDepartureTime([FromQuery] DateTime departure, string allOrAvailableTrips)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var result = await this.tripService.FilterTripsByDepartureAsync(departure, allOrAvailableTrips);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all trips/all available trips for a specific end destination
        /// </summary>
        /// <param name="endDestination"></param>
        /// <param name="allOrAvailableTrips"></param>
        /// <returns>A list of the filtered trips for this end destination</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("filter/endDestination")]
        public async Task<IActionResult> FilterTripsByEndDestination([FromQuery] string endDestination, string allOrAvailableTrips)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var result = await this.tripService.FilterTripsByEndDestinationAsync(endDestination, allOrAvailableTrips);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all past trips for the user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A list of past trips for the user</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("past/users/{username}")]
        public async Task<IActionResult> GetUserPastTrips([FromQuery] string username)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                    if(username != userData.Username)
                    {
                        return this.BadRequest(ExceptionMessages.NoAuthority);
                    }

                    var result = await this.tripService.GetUserPastTripsAsync(username);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }


        /// <summary>
        /// Lists all future trips for the user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A list of future trips for the user</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("future/users/{username}")]
        public async Task<IActionResult> GetUserFutureTrips([FromQuery] string username)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                    if(username != userData.Username)
                    {
                        return this.BadRequest(ExceptionMessages.NoAuthority);
                    }

                    var result = await this.tripService.GetUserFutureTripsAsync(username);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Lists all trips for a specific user and his role in the trip
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <returns>A collection of trips for this user and role</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpGet("users")]
        public async Task<IActionResult> GetTripsByUserRole([FromQuery] string username, string role)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                    if(username != userData.Username)
                    {
                        return this.BadRequest(ExceptionMessages.NoAuthority);
                    }

                    var result = await this.tripService.GetTripsByUserRoleAsync(username, role);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }

        /// <summary>
        /// Adding comment for the trip
        /// </summary>
        /// <param name="tripId"></param>
        /// <param name="comment"></param>
        /// <returns>The comment as a string</returns>
        /// <Author>Stanislav Staykov</Author>
        [HttpPost("comment")]
        public async Task<IActionResult> AddTripComment([FromQuery] Guid tripId, string comment)
        {
            if(this.Request.Cookies.ContainsKey("Login"))
            {
                var key = this.Request.Cookies["Login"];

                try
                {
                    var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                    var result = await this.tripService.AddTripCommentAsync(userData.Id, tripId, comment);

                    return Ok(result);
                }
                catch(ArgumentException ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.BadRequest(ExceptionMessages.LoginRequired);
        }
    }
}
