using System;
using System.Collections.Generic;
using System.Linq;
using Carpooling.Data.Models;
using Carpooling.Services;
using Carpooling.Services.DTO.IncomeDTOs;

namespace Carpooling.MVC.Models
{
    public class ModelMapper
    {
        public LoginDTO ToLoginDTO(LoginViewModel loginViewModel)
        {
            return new LoginDTO
            {
                Password = loginViewModel.Password,
                Username = loginViewModel.Username
            };
        }

        public UserDTO ToUserDTO(User user)
        {
            return new UserDTO
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.Image
            };
        }

        public ICollection<TripCandidatesViewModel> ToTripCandidatesViewModel(ICollection<TripCandidate> tripCandidates)
        {
            return tripCandidates.Select(t => new TripCandidatesViewModel()
            {
                Id = t.Id,
                Username = t.Passanger.Username,
                Email = t.Passanger.Email,
                PhoneNumber = t.Passanger.PhoneNumber,
                IsApproved = t.IsApproved,
                Rating = t.Passanger.UserRoles.Where(ur => ur.RoleId == Guid.Parse("943b692d-330e-405d-a019-c3d728442144")).Select(ur => ur.Rating).FirstOrDefault()
            }
            ).ToList();
        }

        public UserDTO ToUserDTO(CreateViewModel createViewModel)
        {
            return new UserDTO
            {
                Username = createViewModel.Username,
                Password = createViewModel.Password,
                Email = createViewModel.Email,
                FirstName = createViewModel.FirstName,
                LastName = createViewModel.LastName,
                PhoneNumber = createViewModel.PhoneNumber,
                ImageUrl = createViewModel.ImageUrl
            };
        }

        public UserView ToUserView(ICollection<UserDTO> userDTO)
        {
            return new UserView
            {
                userDTOs = userDTO
            };
        }

        public ICollection<TripViewModel> ToTripIEnumerableViewModel(ICollection<Trip> trip)
        {
            return trip.Select(t => new TripViewModel(t)).ToList();
        }

        public TripView ToTripViewFromTrip(ICollection<Trip> trip)
        {
            var tripViewModel = ToTripIEnumerableViewModel(trip);
            return new TripView(tripViewModel);
        }

        public TripView ToTripViewFromTripDTO(ICollection<TripDTO> tripDTOs)
        {
            var trips = tripDTOs.Select(x => new Trip()
            {
                Id = x.Id,
                StartDestination = x.StartDestination,
                EndDestination = x.EndDestination,
                Departure = x.Departure,
                FreeSpots = x.FreeSpots,
                TravelStatus = x.TravelStatus
            }).ToList();

            return ToTripViewFromTrip(trips);
        }

        public ICollection<FeedbackModel> ToFeedbackViewFromFeedback(ICollection<Feedback> feedbackDTOs)
        {
            var feedbackModel = feedbackDTOs.Select(f => new FeedbackModel(f)).ToList();

            return feedbackModel;
        }

        public Feedback ToFeedbackFromFeedbackViewModel(FeedbackViewModel feedbackViewModel)
        {
            var feedback = new Feedback
            {
                Id = feedbackViewModel.FeedbackId,
                Rating = feedbackViewModel.Rating,
                Description = feedbackViewModel.Comment
            };

            return feedback;
        }
    }
}
