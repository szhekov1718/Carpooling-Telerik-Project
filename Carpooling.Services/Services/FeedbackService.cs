using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Enums;
using Carpooling.Data.Models;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly CarpoolingContext dbContext;
        private readonly IUserService userService;

        public FeedbackService(CarpoolingContext carpoolingContext, IUserService userService)
        {
            this.dbContext = carpoolingContext;
            this.userService = userService;
        }

        public async Task<FeedbackDTO> CreateFeedbackAsync(CreateFeedbackDTO feedbackDTO)
        {
            if(feedbackDTO == null)
            {
                throw new EntityNotFound(ExceptionMessages.EntityNotFound);
            }

            if(!feedbackDTO.Rating.Any())
            {
                throw new ArgumentException(ExceptionMessages.RatingRequired);
            }

            var user = await this.userService.GetUserByUserNameAsync(feedbackDTO.Username);

            if(user == null)
            {
                throw new UserNotExistException(ExceptionMessages.InvalidUser);
            }

            Regex wordFilter = new Regex("(damn|shit|fuck|asshole|bitch|faggot|darn|pussy|cunt|motherfucker|gosh|nigger|nigga|bastard|wanker|crap|dick|dickhead|cock|knob|prick|twat|bullshit|arse)");
            if(feedbackDTO.Description != null)
            {
                feedbackDTO.Description = wordFilter.Replace(feedbackDTO.Description, "*****");
            }
            Guid roleId;
            TravelRole role = TravelRole.Passenger;

            var tripsLikeDriver = await this.dbContext.Trips.Where(t => t.DriverId == user.Id && t.Id == feedbackDTO.TripId)
                                                            .FirstOrDefaultAsync();

            var tripsLikePassenger = await this.dbContext.TripCandidates.Include(tc => tc.Trip)
                                                                        .Where(tc => tc.TripId == feedbackDTO.TripId && tc.UserId == user.Id)
                                                                        .Select(tc => tc.Trip)
                                                                        .FirstOrDefaultAsync();

            if(tripsLikeDriver != null)
            {
                role = TravelRole.Driver;
            }
            else if(tripsLikePassenger == null)
            {
                throw new ArgumentException(ExceptionMessages.EntityNotFound);
            }

            roleId = await this.dbContext.Roles.Where(r => r.TravelRole == role)
                                               .Select(r => r.Id)
                                               .FirstOrDefaultAsync();

            var feedback = new Feedback
            {
                Rating = int.Parse(feedbackDTO.Rating),
                Description = feedbackDTO.Description,
                TripId = feedbackDTO.TripId,
                UserId = user.Id,
                RoleId = roleId
            };

            Guid tripId;
            await this.dbContext.Feedbacks.AddAsync(feedback);
            if(tripsLikeDriver == null)
            {
                tripsLikePassenger.Feedbacks.Add(feedback);
                tripId = tripsLikePassenger.Id;
            }
            else
            {
                tripsLikeDriver.Feedbacks.Add(feedback);
                tripId = tripsLikeDriver.Id;
            }

            await UpdateDriverPassengerRating(tripId, role, feedback.Rating);
            await this.dbContext.SaveChangesAsync();


            return new FeedbackDTO(feedback);
        }

        public async Task<Feedback> GetFeedbackAsync(Guid feedbackId)
        {
            var feedback = await this.dbContext.Feedbacks.Where(f => f.Id == feedbackId).FirstOrDefaultAsync();
            if(feedback == null)
            {
                throw new FeedbackException(ExceptionMessages.EntityNotFound);
            }

            return feedback;
        }

        public async Task EditFeedbackAsync(Feedback data)
        {
            var feedback = await this.GetFeedbackAsync(data.Id);
            feedback.Rating = data.Rating;

            if(data.Description != null)
            {
                feedback.Description = data.Description;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateDriverPassengerRating(Guid tripId, TravelRole creatorRole, int rating)
        {
            IList<Guid> usersIds = new List<Guid>();
            TravelRole role = TravelRole.Passenger;

            if(creatorRole == TravelRole.Passenger)
            {
                usersIds.Add(await this.dbContext.Trips.Where(t => t.Id == tripId).Select(t => t.DriverId).FirstOrDefaultAsync());
                role = TravelRole.Driver;
            }

            else
            {
                var passengers = await this.dbContext.TripCandidates.Where(tc => tc.TripId == tripId && tc.IsApproved == true).Select(tc => tc.UserId).ToListAsync();
                usersIds = passengers;
            }

            foreach(var user in usersIds)
            {
                var userRole = await this.dbContext.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == user && ur.Role.TravelRole == role).FirstOrDefaultAsync();
                userRole.RatingSum += rating;
                userRole.FeedbacksCount++;
                userRole.Rating = userRole.RatingSum / userRole.FeedbacksCount;
            }
            await this.dbContext.SaveChangesAsync();
        }

        public ICollection<UserRatingDTO> TopTenDirverPassengerRatings(TravelRole role)
        {
            var result = this.dbContext.UserRoles.Include(ur => ur.User).Include(ur => ur.Role).OrderByDescending(ur => ur.Rating)
                                                 .Where(ur => ur.Role.TravelRole == role)
                                                 .Take(10)
                                                 .Select(ur => new UserRatingDTO(ur))
                                                 .ToList();

            return result;
        }

        public async Task<ICollection<UserRatingDTO>> AllRatingsCountAsync()
        {
            var result = await this.dbContext.UserRoles.Include(u => u.User).Select(r => new UserRatingDTO(r)).ToListAsync();

            return result;
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid feedbackId)
        {
            var feedback = await this.dbContext.Feedbacks.Where(f => f.Id == feedbackId && f.UserId == userId)
                                                         .FirstOrDefaultAsync();
            if(feedback == null)
            {
                throw new FeedbackException(ExceptionMessages.InvalidFeedback);
            }

            feedback.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();

            return feedback.IsDeleted;
        }

        public async Task<ICollection<Feedback>> ListAllUserFeedbacksAsync(Guid id)
        {
            var feedbacks = await this.dbContext.Feedbacks.Include(f => f.Trip)
                                                          .Include(f => f.User)
                                                          .Include(f => f.Role)
                                                          .Where(f => f.UserId == id)
                                                          .ToListAsync();

            return feedbacks;
        }

        public async Task<IList<Feedback>> ListAllFeedbacksForUserAsync(Guid id)
        {
            var userTrips = await this.dbContext.Users
                .Where(u => u.Id == id)
                .SelectMany(u => u.Trips)
                .ToListAsync();

            List<Feedback> feedbacks = new List<Feedback>();

            foreach(var trip in userTrips)
            {
                var feedbackForTrip = await this.dbContext.Feedbacks
                    .Include(f => f.Role)
                    .Include(f => f.Trip)
                    .Where(f => f.TripId == trip.Id && f.UserId != id)
                    .ToListAsync();

                feedbacks = feedbacks.Concat(feedbackForTrip).ToList();
            }

            return feedbacks;
        }

        public async Task<ICollection<FeedbackDTO>> ListAllFeedbacksAsync()
        {
            var feedbacks = await this.dbContext.Feedbacks.Include(f => f.Trip)
                                                          .Include(f => f.User)
                                                          .Include(f => f.Role)
                                                          .Select(f => new FeedbackDTO(f))
                                                          .ToListAsync();

            return feedbacks;
        }
    }
}
