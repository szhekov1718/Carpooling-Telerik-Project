using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class GetUserRating_Should
    {
        [TestMethod]
        public async Task GetUserRatingForDriver()
        {
            var options = Utils.GetOptions(nameof(GetUserRatingForDriver));
            var users = Utils.SeedUsers();
            var roles = Utils.SeedRoles();
            var feedbacks = Utils.SeedFeedbacks();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.AddRangeAsync(roles);
                await arrangeContext.AddRangeAsync(feedbacks);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                var customer = await sut.UserRatingLikePassengerOrDriver(Guid.Parse("943b692d-330e-405d-a019-c3d728442141"), "Driver");
                Assert.AreEqual("Mirko", customer.Username);
            }
        }
    }
}
