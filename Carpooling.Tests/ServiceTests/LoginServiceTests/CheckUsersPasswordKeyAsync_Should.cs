using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.LoginServiceTests
{
    [TestClass]
    public class CheckUsersPasswordKeyAsync_Should
    {
        [TestMethod]
        public async Task CheckUserPasswordKey_Should()
        {
            var options = Utils.GetOptions(nameof(CheckUserPasswordKey_Should));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new LoginService(actContext, sutHelp);

                var user = await sut.CheckUsersPasswordKeyAsync("cd5392bb8db39640ea5ee097ce0a832f8e506cc1c0ca685812575acd83ee5a8c");
                var user1 = await actContext.Users
                .FirstOrDefaultAsync(u => u.Password == "Stenlyto*12");

                Assert.AreEqual("cd5392bb8db39640ea5ee097ce0a832f8e506cc1c0ca685812575acd83ee5a8c", user.Password);
                Assert.AreEqual("Simeonov", user.LastName);
            }
        }

        [TestMethod]
        public async Task CheckUserPasswordKey_ShouldWhenParamIsNull()
        {
            var options = Utils.GetOptions(nameof(CheckUserPasswordKey_ShouldWhenParamIsNull));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new LoginService(actContext, sutHelp);


                await Assert.ThrowsExceptionAsync<AuthorisationException>(() => sut.CheckUsersPasswordKeyAsync(null));
            }
        }
    }
}
