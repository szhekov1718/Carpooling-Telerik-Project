using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Carpooling.MVC.Models;
using Carpooling.Services;
using Carpooling.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Carpooling.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFeedbackService feedbackService;
        private readonly IUserService userService;
        private readonly ITripService tripService;

        public HomeController(ILogger<HomeController> logger, IFeedbackService feedbackService, IUserService userService, ITripService tripService)
        {
            _logger = logger;
            this.feedbackService = feedbackService;
            this.userService = userService;
            this.tripService = tripService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.userService.GetAllUsersAsync();
            var trips = await this.tripService.GetAllTripsAsync();
            var feedbacks = await this.feedbackService.ListAllFeedbacksAsync();
            var ratings = await this.feedbackService.AllRatingsCountAsync();
            var ratingsAverage = ratings.Count() / feedbacks.Count();

            var tripsDTO = trips.Select(t => new TripDTO(t)).ToList();

            var homeModel = new HomePageViewModel()
            {
                Users = users,
                Trips = tripsDTO,
                Feedbacks = feedbacks,
                Ratings = ratings,
                AverageRating = ratingsAverage
            };

            return View(homeModel);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactUsViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    using(MailMessage msz = new MailMessage())
                    {
                        msz.From = new MailAddress(viewModel.Email);

                        msz.To.Add("carpoolingtelerik@gmail.com");
                        msz.Subject = viewModel.Topic;
                        msz.Body = "From:  " + viewModel.Email + "\n\n" + "Phone Number:" + viewModel.PhoneNumber + "\n\n" + viewModel.Description;

                        SmtpClient smtp = new SmtpClient();

                        smtp.Host = "smtp.gmail.com";

                        smtp.Port = 587;

                        smtp.EnableSsl = true;


                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.UseDefaultCredentials = false;

                        smtp.Credentials = new System.Net.NetworkCredential("carpoolingtelerik@gmail.com", "qhtenlak1");

                        smtp.Send(msz);

                        ModelState.Clear();
                        ViewBag.Message = "Thank you for getting in touch! ";
                    };
                }
                catch(Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $"We are sorry, but there is a problem - {ex.Message}";
                }
            }

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        //public IActionResult TopDrivers()
        //{
        //    var topDrivers = this.feedbackService.TopTenDirverPassengerRatings(Data.Enums.TravelRole.Driver);
        //    //var topDriversView = topDrivers.Select(td => td.ToTopCustomerViewModel()).ToArray();

        //    //return View(topDriversView); // TODO
        //     throw new NotImplementedException();

        //}
        //public IActionResult TopPassengers()
        //{
        //    var topPassengers = this.feedbackService.TopTenDirverPassengerRatings(Data.Enums.TravelRole.Passenger);
        //    //var topPassengersView = topPassengers.Select(td => td.ToTopCustomerViewModel()).ToArray();

        //    //return View(topPassengersView); // TODO
        //     throw new NotImplementedException();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult PageNotFound()
        {
            return this.View();
        }
    }
}
