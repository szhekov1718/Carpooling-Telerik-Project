using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class ChangeTripStatus_Should
    {
        [TestMethod]
        public async Task ChangeTripStatus()
        {
            var options = Utils.GetOptions(nameof(ChangeTripStatus));
            var users = Utils.SeedTrips();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                var isCompleted = await sut.DriverMarkTripCompleted(Guid.Parse("943b692d-330e-405d-a019-c3d728442143"), Guid.Parse("943b692d-330e-405d-a019-c3d728442146"));
                Assert.AreEqual("Completed", isCompleted.ToString());
            }
        }

        [TestMethod]
        public async Task ChangeTripStatusThrowArgumentExceptionIfDriverNotExist()
        {
            var options = Utils.GetOptions(nameof(ChangeTripStatusThrowArgumentExceptionIfDriverNotExist));
            var users = Utils.SeedTrips();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.DriverMarkTripCompleted(Guid.Parse("943b692d-230e-405d-a019-c3d728442143"), Guid.Parse("943b692d-330e-405d-a019-c3d728442146")));
            }
        }

        [TestMethod]
        public async Task ChangeTripStatusThrowArgumentExceptionIfTripNotExist()
        {
            var options = Utils.GetOptions(nameof(ChangeTripStatusThrowArgumentExceptionIfTripNotExist));
            var users = Utils.SeedTrips();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.DriverMarkTripCompleted(Guid.Parse("943b692d-230e-405d-a019-c3d728442143"), Guid.Parse("943b692d-230e-405d-a019-c3d728442146")));
            }
        }
    }
}
