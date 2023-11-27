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
    public class FilterTripsByDeparture_Should
    {
        [TestMethod]
        public async Task GetTripsByDeparture_Should()
        {
            var options = Utils.GetOptions(nameof(GetTripsByDeparture_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                var result = await sut.FilterTripsByDepartureAsync(new DateTime(2021, 1, 1), "all");

                var filteredTrips = await actContext.Trips
                    .Where(t => t.IsDeleted == false && t.Departure == new DateTime(2021, 1, 1))
                    .Select(t => new TripDTO(t))
                    .ToListAsync();

                Assert.AreEqual(filteredTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", filteredTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task GetAvailableTripsByDeparture_Should()
        {
            var options = Utils.GetOptions(nameof(GetAvailableTripsByDeparture_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                var result = await sut.FilterTripsByDepartureAsync(new DateTime(2021, 1, 1), "available");

                var filteredTrips = await actContext.Trips
                    .Where(t => t.IsDeleted == false && t.Departure == new DateTime(2021, 1, 1) && t.FreeSpots > 0 && t.Departure < DateTime.Now.Date)
                    .Select(t => new TripDTO(t))
                    .ToListAsync();

                Assert.AreEqual(filteredTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", filteredTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task ThrowIfGetTripsByDepartureWrongParams_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowIfGetTripsByDepartureWrongParams_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.FilterTripsByDepartureAsync(new DateTime(2021, 1, 5), "all"));
            }
        }

        [TestMethod]
        public async Task ThrowIfGetTripsByDepartureWrongFilter_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowIfGetTripsByDepartureWrongFilter_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.FilterTripsByDepartureAsync(new DateTime(2021, 1, 5), "pinokio"));
            }
        }

        [TestMethod]
        public async Task ThrowIfGetAvailableTripsByDepartureWrongParams_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowIfGetAvailableTripsByDepartureWrongParams_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.Trips.AddRangeAsync(trips);

                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.FilterTripsByDepartureAsync(new DateTime(2022, 1, 5), "available"));
            }
        }
    }
}
