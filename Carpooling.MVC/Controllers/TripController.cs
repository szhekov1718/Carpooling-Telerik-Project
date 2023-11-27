using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.MVC.Models;
using Carpooling.Services;
using Carpooling.Services.Contracts;
using Carpooling.Services.Exceptions;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.MVC.Controllers
{
    public class TripController : Controller
    {
        private readonly ILoginService loginService;
        private readonly ITripCandidateService tripCandidateService;
        private readonly ModelMapper modelMapper;
        private readonly ITripService tripService;

        public TripController(
            ITripService tripService,
            ITripCandidateService tripCandidateService,
            ILoginService loginService,
            ModelMapper modelMapper)
        {
            this.tripService = tripService;
            this.tripCandidateService = tripCandidateService;
            this.loginService = loginService;
            this.modelMapper = modelMapper;
        }

        private IActionResult TripNotFound(Guid id)
        {
            this.Response.StatusCode = StatusCodes.Status404NotFound;
            this.ViewBag.Error = ExceptionMessages.InvalidTrip;
            return this.View("Error");
        }

        [ActionName("CreateForm")]
        public async Task<IActionResult> Create()
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
            var model = new TripDTO();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TripDTO model)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View("CreateForm", model);
            }

            try
            {
                var key = this.Request.Cookies["UserKey"];
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var trip = await this.tripService.CreateAsync(model, userData.Id);

                return this.RedirectToAction(nameof(Index), "Trip");
            }
            catch(EntityNotFound ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;

                return this.View("Error");
            }
            catch(InvalidDepartureTimeException ex)
            {
                this.ModelState.AddModelError("Departure", ex.Message);
            }
            catch(ArgumentException ex)
            {
                this.ModelState.AddModelError("FreeSpots", ex.Message);
            }

            return this.View("CreateForm", model);
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];

            try
            {
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var trip = await this.tripService.GetTripAsync(Id);

                return this.View(new TripViewModel(trip));
            }
            catch(Exception)
            {
                return this.TripNotFound(Id);
            }
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid Id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];

            try
            {
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var trip = await this.tripService.DeleteAsync(Id);
            }
            catch(Exception)
            {
                return this.TripNotFound(Id);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];

            try
            {
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var trip = await this.tripService.GetTripAsync(Id);

                return this.View(new TripViewModel(trip));
            }
            catch(TravelDoesNotExistException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;

                return this.View("Error");
            }
        }

        public async Task<IActionResult> Index()
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var tripsView = await this.AvailableTrips("");

            return this.View(tripsView);
        }

        public async Task<IActionResult> ApplyForTripForm(Guid id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];
            try
            {
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

                var result = new TripViewModel(await this.tripService.GetTripAsync(id));

                return this.View("ApplyForTrip", result);
            }
            catch(Exception)
            {
                return this.TripNotFound(id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForTrip(Guid id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];

            try
            {
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var result = await this.tripService.ApplyForTripAsync(userData.Username, id);

                return this.RedirectToAction(nameof(this.Index));
            }
            catch(AuthorisationException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            catch(UserNotExistException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status400BadRequest;
            }

            return this.View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            try
            {
                var tripCandidacy = await this.tripCandidateService.GetTripCandidateAsync(id);
                await this.tripCandidateService.ApproveCandidateAsync(tripCandidacy.UserId, tripCandidacy.TripId, tripCandidacy.DriverId);
                id = tripCandidacy.TripId;

                return this.View("TripCandidates", await this.GetTripCandidatesAsync(id));
            }
            catch(AuthorisationException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            catch(ArgumentException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            catch(TravelPassengerDoesNotExistException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status400BadRequest;
            }

            return this.View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            try
            {
                var tripCandidacy = await this.tripCandidateService.GetTripCandidateAsync(id);
                await this.tripCandidateService.RejectCandidateAsync(tripCandidacy.UserId, tripCandidacy.TripId, tripCandidacy.DriverId);
                id = tripCandidacy.TripId;

                return this.View("TripCandidates", await this.GetTripCandidatesAsync(id));
            }
            catch(AuthorisationException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            catch(ArgumentException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            catch(TravelPassengerDoesNotExistException ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status400BadRequest;
            }

            return this.View("Error");
        }

        public async Task<IActionResult> TripCandidates(Guid id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            try
            {
                return this.View("TripCandidates", await this.GetTripCandidatesAsync(id));
            }
            catch(EntityNotFound ex)
            {
                this.ViewBag.Error = ex.Message;
                this.Response.StatusCode = StatusCodes.Status204NoContent;
            }

            return this.View("Error");
        }

        public async Task<IActionResult> FilterBy(TripView tripView)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            if(tripView.Value == null)
            {
                ModelState.AddModelError("Value", "Value is required!");
                tripView = await this.AvailableTrips("");

                return this.View("Index", tripView);
            }

            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
            TripView trips = await FilterTipsBy(tripView, userData.Username);

            if(!ModelState.IsValid)
            {
                tripView = await this.AvailableTrips("");

                return this.View("Index", tripView);
            }

            return this.View("Index", trips);
        }

        public async Task<IActionResult> FilterUserTripsBy(TripView tripView)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            string[] values = { "EndDestination", "StartDestination", "Departure" };

            if(tripView.Value == null && values.Contains(tripView.SearchAttribute))
            {
                ModelState.AddModelError("Value", "Value is required!");
                tripView = await this.AvailableTrips("FilterUserTrips");

                return this.View("UserTrips", tripView);
            }

            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
            TripView trips = await FilterTipsBy(tripView, userData.Username);

            if(!ModelState.IsValid)
            {
                tripView = await this.AvailableTrips("FilterUserTrips");

                return this.View("UserTrips", tripView);
            }

            return this.View("UserTrips", trips);
        }

        public async Task<IActionResult> CancelTrip(Guid id)
        {
            await this.tripService.DriverCancelTrip(id);
            var tripView = await this.AvailableTrips("FilterUserTrips");

            return this.View("UserTrips", tripView);
        }

        public async Task<IActionResult> CancelTripCandidacy(Guid id)
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
            var tripCandidacy = await this.tripCandidateService.GetTripCandidateAsync(userData.Id, id);
            await this.tripCandidateService.PassengerDeclineTripCandidacyAsync(userData.Username, tripCandidacy.Id);
            TripView tripView;

            try
            {
                tripView = await this.AvailableTrips("FilterUserTrips");
            }
            catch(TravelDoesNotExistException)
            {
                tripView = new TripView();

                return this.View("UserTrips", tripView);
            }

            return this.View("UserTrips", tripView);
        }

        public async Task<IActionResult> MyTrips()
        {
            if(!this.Request.Cookies.ContainsKey("UserKey"))
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var key = this.Request.Cookies["UserKey"];
            try
            {
                var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);
                var allUserTrips = await this.tripService.GetTripsByUserRoleAsync(userData.Username, "all");
                var allUserTripsView = this.modelMapper.ToTripViewFromTripDTO(allUserTrips);

                return this.View("UserTrips", allUserTripsView);
            }
            catch(TravelDoesNotExistException)
            {
                TripView tripView = new TripView();

                return this.View("UserTrips", tripView);
            }
        }

        public async Task<TripView> AvailableTrips(string caller)
        {
            var key = this.Request.Cookies["UserKey"];
            var userData = await this.loginService.CheckUsersPasswordKeyAsync(key);

            if(this.Request.Cookies["UserRole"] == "Admin" && caller == "")
            {
                var allTrips = await this.tripService.GetAllTripsAsync();
                var allTripsToViewModel = this.modelMapper.ToTripViewFromTrip(allTrips);

                return allTripsToViewModel;
            }

            if(caller == "")
            {
                var trips = await this.tripService.AvailableTripsAsync();
                var tripsView = this.modelMapper.ToTripViewFromTrip(trips);

                return tripsView;
            }

            var userTrips = await this.tripService.GetTripsByUserRoleAsync(userData.Username, "all");
            var userTripsView = this.modelMapper.ToTripViewFromTripDTO(userTrips);

            return userTripsView;
        }

        public async Task<TripView> FilterTipsBy(TripView tripView, string username)
        {
            TripView trips = new TripView();
            try
            {

                IList<TripDTO> tripDTO = new List<TripDTO>();
                switch(tripView.SearchAttribute)
                {
                    case "Departure":
                    DateTime date;
                    if(!DateTime.TryParse(tripView.Value, out date))
                    {
                        ModelState.AddModelError("Value", ExceptionMessages.InvalidDateFormat);

                        return tripView;
                    }
                    tripDTO = await this.tripService.FilterTripsByDepartureAsync(date, "all");
                    trips = this.modelMapper.ToTripViewFromTripDTO(tripDTO);
                    break;

                    case "StartDestination":
                    tripDTO = await this.tripService.FilterTripsByStartDestinationAsync(tripView.Value, "all");
                    trips = this.modelMapper.ToTripViewFromTripDTO(tripDTO);
                    break;

                    case "EndDestination":
                    tripDTO = await this.tripService.FilterTripsByEndDestinationAsync(tripView.Value, "all");
                    trips = this.modelMapper.ToTripViewFromTripDTO(tripDTO);
                    break;

                    case "FutureTrips":
                    tripDTO = await this.tripService.GetUserFutureTripsAsync(username);
                    trips = this.modelMapper.ToTripViewFromTripDTO(tripDTO);
                    break;

                    case "PastTrips":
                    tripDTO = await this.tripService.GetUserPastTripsAsync(username);
                    trips = this.modelMapper.ToTripViewFromTripDTO(tripDTO);
                    break;

                    case "AsDriver":
                    tripDTO = await this.tripService.GetTripsByUserRoleAsync(username, "driver");
                    trips = this.modelMapper.ToTripViewFromTripDTO(tripDTO);
                    break;

                    default:
                    tripDTO = await this.tripService.GetTripsByUserRoleAsync(username, "passenger");
                    trips = this.modelMapper.ToTripViewFromTripDTO(tripDTO);
                    break;
                }

            }
            catch(TravelDoesNotExistException ex)
            {
                ModelState.AddModelError("Value", ex.Message);
            }

            return trips;
        }

        public async Task<ICollection<TripCandidatesViewModel>> GetTripCandidatesAsync(Guid id)
        {
            var candidates = await this.tripCandidateService.GetAllTripCandidatesForTripAsync(id);
            var candidatesView = this.modelMapper.ToTripCandidatesViewModel(candidates);

            return candidatesView;
        }
    }
}



