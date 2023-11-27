using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class ChangeUserStatus_Should
    {
        [TestMethod]
        public async Task ChangeUserStatus()
        {
            var options = Utils.GetOptions(nameof(ChangeUserStatus));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                var isAdmin = await sut.MakeUserAdmin("Stenlyto");
                Assert.IsTrue(isAdmin);
            }
        }

        [TestMethod]
        public async Task ChangeUserStatusThrowArgumentExceptionIfUserNotExist()
        {
            var options = Utils.GetOptions(nameof(ChangeUserStatusThrowArgumentExceptionIfUserNotExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.MakeUserAdmin("ssssssss"));
            }
        }
    }
}
