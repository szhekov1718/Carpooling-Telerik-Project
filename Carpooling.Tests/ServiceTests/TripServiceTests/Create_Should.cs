using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Models;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Carpooling.Tests.ServiceTests.TripServiceTests
{
    [TestClass]
    public class Create_Should
    {
        [TestMethod]
        public async Task ReturnTripSuccessfullyCreated_Should()
        {
            var options = Utils.GetOptions(nameof(ReturnTripSuccessfullyCreated_Should));
            var trips = Utils.SeedTrips();
            var users = Utils.SeedUsers();

            var newTrip = new Mock<Trip>().Object;
            newTrip.StartDestination = "Sofia";
            newTrip.EndDestination = "Plovdiv";
            newTrip.Departure = DateTime.Now.Date.AddDays(2);
            newTrip.FreeSpots = 3;

            var newUser = new Mock<User>().Object;
            newUser.Id = Guid.Parse("943b692d-330e-405d-a019-d3d728442148");
            newUser.Username = "Kircho";
            newUser.Password = "Kiro*123";
            newUser.Email = "kiro98@abv.bg";
            newUser.FirstName = "Kiro";
            newUser.LastName = "Kirov";
            newUser.PhoneNumber = "0895454545";
            newUser.Trips = new List<Trip>();

            var newTripDto = new Mock<TripDTO>(newTrip).Object;

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.Users.AddAsync(newUser);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);
                var result = await sut.CreateAsync(newTripDto, Guid.Parse("943b692d-330e-405d-a019-d3d728442148"));

                Assert.AreEqual(newTripDto.StartDestination, result.StartDestination);
                Assert.AreEqual(newTripDto.EndDestination, result.EndDestination);
                Assert.AreEqual(newTripDto.Departure, result.Departure);
                Assert.AreEqual(newTripDto.FreeSpots, result.FreeSpots);
            }
        }

        [TestMethod]
        public async Task ThrowWhenDTOIsNull_Should()
        {
            var options = Utils.GetOptions(nameof(ThrowWhenDTOIsNull_Should));

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.CreateAsync(null, Guid.Parse("943b692d-330e-405d-a019-c3d728442148")));
            }
        }

        [TestMethod]
        public async Task Throw_WhenDepartureTimeIsInThePast()
        {
            var options = Utils.GetOptions(nameof(Throw_WhenDepartureTimeIsInThePast));
            var trips = Utils.SeedTrips();

            var newTrip = new Mock<Trip>().Object;
            newTrip.StartDestination = "Sofia";
            newTrip.EndDestination = "Plovdiv";
            newTrip.Departure = DateTime.Now.Date.AddDays(-5);
            newTrip.FreeSpots = 3;

            var newTripDto = new Mock<TripDTO>(newTrip).Object;

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<InvalidDepartureTimeException>(() => sut.CreateAsync(newTripDto, Guid.Parse("943b692d-330e-405d-a019-c3d728442148")));
            }
        }

        [TestMethod]
        public async Task Throw_WhenFreeSpotsAreLessThanOne()
        {
            var options = Utils.GetOptions(nameof(Throw_WhenFreeSpotsAreLessThanOne));
            var trips = Utils.SeedTrips();

            var newTrip = new Mock<Trip>().Object;
            newTrip.StartDestination = "Sofia";
            newTrip.EndDestination = "Plovdiv";
            newTrip.Departure = DateTime.Now.Date.AddDays(5);
            newTrip.FreeSpots = 0;

            var newTripDto = new Mock<TripDTO>(newTrip).Object;

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(trips);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new TripService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateAsync(newTripDto, Guid.Parse("943b692d-330e-405d-a019-c3d728442148")));
            }
        }
    }
}
