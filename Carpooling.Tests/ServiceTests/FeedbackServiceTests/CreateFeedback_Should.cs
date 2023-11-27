using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Carpooling.Tests.ServiceTests.FeedbackServiceTests
{
    [TestClass]
    public class CreateFeedback_Should
    {
        [TestMethod]
        public async Task CreateFeedbackSuccessfully()
        {
            var options = Utils.GetOptions(nameof(CreateFeedbackSuccessfully));
            var trips = Utils.SeedTrips();
            var roles = Utils.SeedRoles();
            var userRoles = Utils.SeedUserRoles();
            var users = Utils.SeedUsers();
            var feedbacks = Utils.SeedFeedbacks();
            var tripCandidates = Utils.SeedTripCandidates();

            var newFeedback = new Mock<CreateFeedbackDTO>().Object;
            newFeedback.Rating = "5";
            newFeedback.Description = null;
            newFeedback.TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146");
            newFeedback.Username = "gogi";

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(feedbacks);
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(tripCandidates);
                await arrangeContext.AddRangeAsync(roles);
                await arrangeContext.AddRangeAsync(userRoles);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);
                var result = await sut.CreateFeedbackAsync(newFeedback);

                Assert.AreEqual(newFeedback.Username, result.Username);
            }
        }

        [TestMethod]
        public async Task CreateFeedbackWithNullParams()
        {
            var options = Utils.GetOptions(nameof(CreateFeedbackWithNullParams));
            var trips = Utils.SeedTrips();
            var roles = Utils.SeedRoles();
            var userRoles = Utils.SeedUserRoles();
            var users = Utils.SeedUsers();
            var feedbacks = Utils.SeedFeedbacks();
            var tripCandidates = Utils.SeedTripCandidates();

            var newFeedback = new Mock<CreateFeedbackDTO>().Object;
            newFeedback.Rating = "5";
            newFeedback.Description = null;
            newFeedback.TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146");
            newFeedback.Username = "gogi";

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(feedbacks);
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(tripCandidates);
                await arrangeContext.AddRangeAsync(roles);
                await arrangeContext.AddRangeAsync(userRoles);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.CreateFeedbackAsync(null));
            }
        }

        [TestMethod]
        public async Task CreateFeedbackWithNullUser()
        {
            var options = Utils.GetOptions(nameof(CreateFeedbackWithNullUser));
            var trips = Utils.SeedTrips();
            var roles = Utils.SeedRoles();
            var userRoles = Utils.SeedUserRoles();
            var users = Utils.SeedUsers();
            var feedbacks = Utils.SeedFeedbacks();
            var tripCandidates = Utils.SeedTripCandidates();

            var newFeedback = new Mock<CreateFeedbackDTO>().Object;
            newFeedback.Rating = "5";
            newFeedback.Description = null;
            newFeedback.TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146");

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(feedbacks);
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(tripCandidates);
                await arrangeContext.AddRangeAsync(roles);
                await arrangeContext.AddRangeAsync(userRoles);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.CreateFeedbackAsync(newFeedback));
            }
        }

        [TestMethod]
        public async Task CreateFeedbackWithNullRating()
        {
            var options = Utils.GetOptions(nameof(CreateFeedbackWithNullRating));
            var trips = Utils.SeedTrips();
            var roles = Utils.SeedRoles();
            var userRoles = Utils.SeedUserRoles();
            var users = Utils.SeedUsers();
            var feedbacks = Utils.SeedFeedbacks();
            var tripCandidates = Utils.SeedTripCandidates();

            var newFeedback = new Mock<CreateFeedbackDTO>().Object;
            newFeedback.Rating = "";
            newFeedback.Description = null;
            newFeedback.TripId = Guid.Parse("943b692d-330e-405d-a019-c3d728442146");
            newFeedback.Username = "gogi";

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(feedbacks);
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(tripCandidates);
                await arrangeContext.AddRangeAsync(roles);
                await arrangeContext.AddRangeAsync(userRoles);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateFeedbackAsync(newFeedback));
            }
        }
    }
}
