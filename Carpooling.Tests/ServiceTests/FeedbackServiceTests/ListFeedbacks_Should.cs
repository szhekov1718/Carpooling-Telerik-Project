using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Models;
using Carpooling.Services;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.FeedbackServiceTests
{
    [TestClass]
    public class ListFeedbacks_Should
    {
        [TestMethod]
        public async Task ListAllFeedbackss_Should()
        {
            var options = Utils.GetOptions(nameof(ListAllFeedbackss_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var roles = Utils.SeedRoles();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Roles.AddRangeAsync(roles);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);
                var result = await sut.ListAllFeedbacksAsync();

                Assert.AreEqual(actContext.Feedbacks.Count(), result.Count());
                Assert.AreEqual(string.Join(",", actContext.Feedbacks.Select(f => new FeedbackDTO(f)).ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task ListAllFeedbackssForUser_Should()
        {
            var options = Utils.GetOptions(nameof(ListAllFeedbackssForUser_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var roles = Utils.SeedRoles();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Roles.AddRangeAsync(roles);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);
                var result = await sut.ListAllFeedbacksForUserAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442143"));

                var userTrips = await actContext.Users
                .Where(u => u.Id == Guid.Parse("943b692d-330e-405d-a019-c3d728442143"))
                .SelectMany(u => u.Trips)
                .ToListAsync();

                List<Feedback> feedbackz = new List<Feedback>();

                foreach(var trip in userTrips)
                {
                    var feedbackForTrip = await actContext.Feedbacks
                        .Include(f => f.Role)
                        .Include(f => f.Trip)
                        .Where(f => f.TripId == trip.Id && f.UserId != Guid.Parse("943b692d-330e-405d-a019-c3d728442143"))
                        .ToListAsync();

                    feedbackz = feedbackz.Concat(feedbackForTrip).ToList();
                }

                Assert.AreEqual(feedbackz.Count(), result.Count());
            }
        }

        [TestMethod]
        public async Task ListAllUserFeedback_Should()
        {
            var options = Utils.GetOptions(nameof(ListAllUserFeedback_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var roles = Utils.SeedRoles();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Roles.AddRangeAsync(roles);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);
                var result = await sut.ListAllUserFeedbacksAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442143"));

                var feedbackz = await actContext.Feedbacks.Include(f => f.Trip)
                                                          .Include(f => f.User)
                                                          .Include(f => f.Role)
                                                          .Where(f => f.UserId == Guid.Parse("943b692d-330e-405d-a019-c3d728442143"))
                                                          .ToListAsync();

                Assert.AreEqual(feedbackz.Count(), result.Count());
            }
        }

        [TestMethod]
        public async Task ListAllRatingCount_Should()
        {
            var options = Utils.GetOptions(nameof(ListAllRatingCount_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var roles = Utils.SeedRoles();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Roles.AddRangeAsync(roles);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);
                var result = await sut.AllRatingsCountAsync();

                var ratings = await actContext.UserRoles.Include(u => u.User).Select(r => new UserRatingDTO(r)).ToListAsync();

                Assert.AreEqual(ratings.Count(), result.Count());
            }
        }

        [TestMethod]
        public async Task ListTopTenDriverPassengerRatings_Should()
        {
            var options = Utils.GetOptions(nameof(ListTopTenDriverPassengerRatings_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var roles = Utils.SeedRoles();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Roles.AddRangeAsync(roles);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);
                var result = sut.TopTenDirverPassengerRatings(Data.Enums.TravelRole.Driver);

                var ratings = actContext.UserRoles.Include(ur => ur.User).Include(ur => ur.Role).OrderByDescending(ur => ur.Rating)
                                                 .Where(ur => ur.Role.TravelRole == Data.Enums.TravelRole.Driver)
                                                 .Take(10)
                                                 .Select(ur => new UserRatingDTO(ur))
                                                 .ToList();

                Assert.AreEqual(ratings.Count(), result.Count());
            }
        }

        [TestMethod]
        public async Task GetFeedback_Should()
        {
            var options = Utils.GetOptions(nameof(GetFeedback_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var roles = Utils.SeedRoles();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Roles.AddRangeAsync(roles);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);
                var result = await sut.GetFeedbackAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442151"));

                var feedback = await actContext.Feedbacks.Where(f => f.Id == Guid.Parse("943b692d-330e-405d-a019-c3d728442151")).FirstOrDefaultAsync();

                Assert.AreEqual(feedback.Description, result.Description);
                Assert.AreEqual(feedback.Rating, result.Rating);

            }
        }

        [TestMethod]
        public async Task GetFeedbackIfParamIsNUll_Should()
        {
            var options = Utils.GetOptions(nameof(GetFeedbackIfParamIsNUll_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var roles = Utils.SeedRoles();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.Trips.AddRangeAsync(trips);
                await arrContext.Users.AddRangeAsync(users);
                await arrContext.Roles.AddRangeAsync(roles);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<FeedbackException>(() => sut.GetFeedbackAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442159")));
            }
        }
    }
}
