using System;
using System.Collections.Generic;
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
    public class FilterTripsByDestination_Should
    {
        [TestMethod]
        public async Task GetTripsByStartDestinaion_Should()
        {
            var options = Utils.GetOptions(nameof(GetTripsByStartDestinaion_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                arrContext.Trips.AddRange(trips);

                arrContext.SaveChanges();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                var result = await sut.FilterTripsByStartDestinationAsync("Plovdiv", "all");

                var filteredTrips = actContext.Trips
                    .Where(t => t.IsDeleted == false && t.StartDestination == "Plovdiv")
                    .Select(t => new TripDTO(t))
                    .ToList();

                Assert.AreEqual(filteredTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", filteredTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task GetAvailableTripsByStartDestinaion_Should()
        {
            var options = Utils.GetOptions(nameof(GetAvailableTripsByStartDestinaion_Should));
            var trips = Utils.SeedTrips();

            using(var arrContext = new CarpoolingContext(options))
            {
                arrContext.Trips.AddRange(trips);

                arrContext.SaveChanges();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                var result = await sut.FilterTripsByStartDestinationAsync("Plovdiv", "available");

                var filteredTrips = new List<TripDTO>
                {
                    new TripDTO
                    {
                        StartDestination = "Plovdiv",
                        EndDestination = "Sofia",
                        Departure = new DateTime(2021,12,15),
                        FreeSpots = 3
                    }
                };
                Assert.AreEqual(filteredTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", filteredTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task GetTripsByEndDestinaion_Should()
        {
            var options = Utils.GetOptions(nameof(GetTripsByEndDestinaion_Should));
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

                var result = await sut.FilterTripsByEndDestinationAsync("Plovdiv", "all");

                var filteredTrips = await actContext.Trips
                    .Where(t => t.IsDeleted == false && t.EndDestination == "Plovdiv")
                    .Select(t => new TripDTO(t))
                    .ToListAsync();

                Assert.AreEqual(filteredTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", filteredTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task GetAvailableTripsByEndDestinaion_Should()
        {
            var options = Utils.GetOptions(nameof(GetAvailableTripsByEndDestinaion_Should));
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

                var result = await sut.FilterTripsByEndDestinationAsync("Plovdiv", "available");

                var filteredTrips = await actContext.Trips
                    .Where(t => t.IsDeleted == false && t.EndDestination == "Plovdiv" && t.FreeSpots > 0 && t.Departure < DateTime.Now.Date)
                    .Select(t => new TripDTO(t))
                    .ToListAsync();

                Assert.AreEqual(filteredTrips.Count(), result.Count());
                Assert.AreEqual(string.Join(",", filteredTrips.ToList()), string.Join(",", result));
            }
        }

        [TestMethod]
        public async Task ThrowWhenGetTripsByEndDestinaionIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenGetTripsByEndDestinaionIsWrong_Should));
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

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.FilterTripsByEndDestinationAsync("ds", "all"));
            }
        }

        [TestMethod]
        public async Task ThrowWhenGetTripsByStartDestinaionIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenGetTripsByStartDestinaionIsWrong_Should));
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

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.FilterTripsByStartDestinationAsync("ds", "all"));
            }
        }

        [TestMethod]
        public async Task ThrowWhenGetTripsByStartDestinaionAvailableTripsIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenGetTripsByStartDestinaionAvailableTripsIsWrong_Should));
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

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.FilterTripsByStartDestinationAsync("ds", "available"));
            }
        }

        [TestMethod]
        public async Task ThrowWhenGetTripsByDestinationFilterIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenGetTripsByDestinationFilterIsWrong_Should));
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

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.FilterTripsByStartDestinationAsync("Plovdiv", "rumen radev"));
            }
        }

        [TestMethod]
        public async Task ThrowWhenGetTripsByEndDestinaionAvailableTripsIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenGetTripsByEndDestinaionAvailableTripsIsWrong_Should));
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

                await Assert.ThrowsExceptionAsync<TravelDoesNotExistException>(() => sut.FilterTripsByEndDestinationAsync("ds", "available"));
            }
        }

        [TestMethod]
        public async Task ThrowWhenGetTripsByEndDestinationFilterIsWrong_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenGetTripsByEndDestinationFilterIsWrong_Should));
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

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.FilterTripsByEndDestinationAsync("Plovdiv", "berbatov"));
            }
        }
    }
}
