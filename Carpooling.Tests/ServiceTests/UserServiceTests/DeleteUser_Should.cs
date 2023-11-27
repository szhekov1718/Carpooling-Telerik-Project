using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class DeleteUser_Should
    {
        [TestMethod]
        public async Task DeleteCustomer()
        {
            var options = Utils.GetOptions(nameof(DeleteCustomer));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                var customer = await sut.DeleteUserAsync("Stenlyto");
                Assert.IsTrue(customer);
            }
        }

        [TestMethod]
        public async Task DeleteCustomerThrowArgumentExceptionIfUserNameExist()
        {
            var options = Utils.GetOptions(nameof(DeleteCustomerThrowArgumentExceptionIfUserNameExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.DeleteUserAsync("soni"));
            }
        }
    }
}
