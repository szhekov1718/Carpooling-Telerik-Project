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
    public class ApplyForTrip_Should
    {
        [TestMethod]
        public async Task ApplyForTripSuccessfully_Should()
        {
            var options = Utils.GetOptions(nameof(ApplyForTripSuccessfully_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.ApplyForTripAsync("gogi", Guid.Parse("943b692d-330e-405d-a019-c3d728442146"));

                var trip = await actContext.Trips.FirstOrDefaultAsync(t => t.Id == Guid.Parse("943b692d-330e-405d-a019-c3d728442146"));

                Assert.IsTrue(actContext.TripCandidates.Include(t => t.Passanger).Where(t => t.TripId == Guid.Parse("943b692d-330e-405d-a019-c3d728442146") && t.Passanger.Username == "gogi").First() != null);
            }
        }

        [TestMethod]
        public async Task ThrowWhenUsernameIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenUsernameIsWrong_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.ApplyForTripAsync("gogicha", Guid.Parse("943b692d-330e-405d-a019-c3d728442146")));
            }
        }

        [TestMethod]
        public async Task ThrowWhenUserIsBlocked_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenUserIsBlocked_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var tripCandidates = Utils.SeedTripCandidates();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.Trips.AddRangeAsync(trips);
                await arrangeContext.Users.AddRangeAsync(users);
                await arrangeContext.TripCandidates.AddRangeAsync(tripCandidates);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<AuthorisationException>(() => sut.ApplyForTripAsync("geri", Guid.Parse("943b692d-330e-405d-a019-c3d728442146")));
            }
        }

        [TestMethod]
        public async Task ThrowWhenIDIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenIDIsWrong_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.ApplyForTripAsync("gogi", Guid.Parse("943b692d-330e-405d-a019-c3d718442146")));
            }
        }

        [TestMethod]
        public async Task DriverCancelTrip_ShouldThrowArgumentException()
        {
            var options = Utils.GetOptions(nameof(DriverCancelTrip_ShouldThrowArgumentException));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();
            var tripCandidates = Utils.SeedTripCandidates();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.Trips.AddRangeAsync(trips);
                await arrangeContext.Users.AddRangeAsync(users);
                await arrangeContext.TripCandidates.AddRangeAsync(tripCandidates);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.DriverCancelTrip(Guid.Parse("943b692d-220e-405d-a019-c3d728442146")));
            }
        }
    }
}
