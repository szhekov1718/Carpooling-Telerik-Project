using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class BlockUnblockUser_Should
    {
        [TestMethod]
        public async Task BlockUser()
        {
            var options = Utils.GetOptions(nameof(BlockUser));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                var customer = await sut.BlockUnblockUserAsync("Stenlyto", "Block");
                Assert.AreEqual("Stanislav", customer.FirstName);
                Assert.AreEqual("Simeonov", customer.LastName);
                Assert.AreEqual("stenlyto@abv.bg", customer.Email);
                Assert.AreEqual("Stenlyto", customer.Username);
                Assert.AreEqual("0854545454", customer.PhoneNumber);
                Assert.IsTrue(customer.IsBlocked);
            }
        }

        [TestMethod]
        public async Task IsUserBlocked()
        {
            var options = Utils.GetOptions(nameof(IsUserBlocked));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                var customer = await sut.IsUserBlocked("Stenlyto");

                Assert.IsFalse(customer);
            }
        }

        [TestMethod]
        public async Task IsUserBlockedWrongUsername()
        {
            var options = Utils.GetOptions(nameof(IsUserBlockedWrongUsername));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.IsUserBlocked("Milionera"));
            }
        }

        [TestMethod]
        public async Task BlockUnblockUserThrowArgumentExceptionIfUserNameExist()
        {
            var options = Utils.GetOptions(nameof(BlockUnblockUserThrowArgumentExceptionIfUserNameExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.BlockUnblockUserAsync("soni", "Block"));
            }
        }

        [TestMethod]
        public async Task UnblockUser()
        {
            var options = Utils.GetOptions(nameof(UnblockUser));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                var customer = await sut.BlockUnblockUserAsync("Mirko", "Unblock");
                Assert.AreEqual("Stanimir", customer.FirstName);
                Assert.AreEqual("Ivanov", customer.LastName);
                Assert.AreEqual("miro44@abv.bg", customer.Email);
                Assert.AreEqual("Mirko", customer.Username);
                Assert.AreEqual("0864646464", customer.PhoneNumber);
                Assert.IsTrue(!customer.IsBlocked);
            }
        }

        [TestMethod]
        public async Task UnblockUserThrowArgumentExceptionIfUserNameExist()
        {
            var options = Utils.GetOptions(nameof(UnblockUserThrowArgumentExceptionIfUserNameExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.BlockUnblockUserAsync("soni", "Unblock"));
            }
        }
    }
}
