using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.LoginServiceTests
{
    [TestClass]
    public class CheckUserCredentialsAsync_Should
    {
        [TestMethod]
        public async Task CheckUserCredentials_Should()
        {
            var options = Utils.GetOptions(nameof(CheckUserCredentials_Should));
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

                var loginDTO = new LoginDTO()
                {
                    Username = "Stenlyto",
                    Password = "Stenlyto*12"
                };

                var methodCheck = await sut.CheckUserCredentialsAsync(loginDTO);

                Assert.AreEqual("cd5392bb8db39640ea5ee097ce0a832f8e506cc1c0ca685812575acd83ee5a8c", methodCheck);
            }
        }

        [TestMethod]
        public async Task CheckUserCredentials_ShouldWhenParamIsNull()
        {
            var options = Utils.GetOptions(nameof(CheckUserCredentials_ShouldWhenParamIsNull));
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

                var loginDTO = new LoginDTO()
                {
                    Username = "Stenlyto",
                    Password = null
                };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CheckUserCredentialsAsync(loginDTO));
            }
        }

        [TestMethod]
        public async Task CheckUserCredentials_ShouldWhenPassIsWrong()
        {
            var options = Utils.GetOptions(nameof(CheckUserCredentials_ShouldWhenPassIsWrong));
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

                var loginDTO = new LoginDTO()
                {
                    Username = "Stenlyto",
                    Password = "cd5392bb8db39640ea5ee097ce0a832f8e506cc1c0ca685812575acd83ee5a8c"
                };

                await Assert.ThrowsExceptionAsync<PasswordIncorrectException>(() => sut.CheckUserCredentialsAsync(loginDTO));
            }
        }
    }
}
