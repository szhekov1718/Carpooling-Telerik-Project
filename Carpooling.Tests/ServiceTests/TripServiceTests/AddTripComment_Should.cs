using System;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.TripServiceTests
{
    [TestClass]
    public class AddTripComment_Should
    {
        [TestMethod]
        public async Task AddTripCommentSuccessfully_Should()
        {
            var options = Utils.GetOptions(nameof(AddTripCommentSuccessfully_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var comments = Utils.SeedTripComments();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(comments);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                var result = await sut.AddTripCommentAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442145"),
                    Guid.Parse("943b692d-330e-405d-a019-c3d728442148"),
                    "The trip was not pleasant, because the driver was smoking in the car.");

                var trip = await actContext.TripComments.Where(c => c.TripId == Guid.Parse("943b692d-330e-405d-a019-c3d728442148")).FirstOrDefaultAsync();

                Assert.AreEqual(result, trip.Comment);
            }
        }

        [TestMethod]
        public async Task AddTripCommentWithWrongParameters_Should()
        {
            var options = Utils.GetOptions(nameof(AddTripCommentWithWrongParameters_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var comments = Utils.SeedTripComments();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(comments);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.AddTripCommentAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728445689"),
                   Guid.Parse("943b692d-220e-405d-a019-c3d728142146"),
                   "The trip was not pleasant, because the driver was smoking in the car."));
            }
        }
    }
}
