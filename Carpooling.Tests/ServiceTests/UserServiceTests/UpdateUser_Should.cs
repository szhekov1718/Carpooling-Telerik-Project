using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class UpdateUser_Should
    {
        [TestMethod]
        public async Task UpdateCustomer()
        {
            var options = Utils.GetOptions(nameof(UpdateCustomer));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var user = new UserDTO
                {
                    FirstName = "Sisy",
                    LastName = "Ekimova",
                    Username = "sis44",
                    Email = "sis234@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                var customer = await sut.UpdateUserAsync("Stenlyto", user);
                Assert.AreEqual("Sisy", customer.FirstName);
                Assert.AreEqual("Ekimova", customer.LastName);
                Assert.AreEqual("sis234@abv.bg", customer.Email);
                Assert.AreEqual("sis44", customer.Username);
                Assert.AreEqual("0895636369", customer.PhoneNumber);
            }
        }

        [TestMethod]
        public async Task UpdateCustomerThrowArgumentExceptionIfEmailExist()
        {
            var options = Utils.GetOptions(nameof(UpdateCustomerThrowArgumentExceptionIfEmailExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var user = new UserDTO
                {
                    FirstName = "Sisy",
                    LastName = "Ekimova",
                    Username = "sis44",
                    Email = "miro44@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.UpdateUserAsync("Stenlyto", user));
            }
        }

        [TestMethod]
        public async Task UpdateCustomerThrowArgumentExceptionIfEmailIsIncorrect()
        {
            var options = Utils.GetOptions(nameof(UpdateCustomerThrowArgumentExceptionIfEmailIsIncorrect));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var user = new UserDTO
                {
                    FirstName = "Sisy",
                    LastName = "Ekimova",
                    Username = "sis44",
                    Email = "miro44abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<EmailIncorrectException>(() => sut.UpdateUserAsync("Stenlyto", user));
            }
        }

        [TestMethod]
        public async Task UpdateCustomerThrowArgumentExceptionIfUsernameIsWrong()
        {
            var options = Utils.GetOptions(nameof(UpdateCustomerThrowArgumentExceptionIfUsernameIsWrong));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var user = new UserDTO
                {
                    FirstName = "Sisy",
                    LastName = "Ekimova",
                    Username = "sis44",
                    Email = "miro44@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.UpdateUserAsync("LunaPrezident", user));
            }
        }

        [TestMethod]
        public async Task UpdateCustomerThrowArgumentExceptionIfDTOIsNull()
        {
            var options = Utils.GetOptions(nameof(UpdateCustomerThrowArgumentExceptionIfDTOIsNull));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var user = new UserDTO
                {
                    FirstName = "Sisy",
                    LastName = "Ekimova",
                    Username = "sis44",
                    Email = "miro44@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<UserNotExistException>(() => sut.UpdateUserAsync("Stenlyto", null));
            }
        }

        [TestMethod]
        public async Task UpdateCustomerThrowArgumentExceptionIfUserNameExist()
        {
            var options = Utils.GetOptions(nameof(UpdateCustomerThrowArgumentExceptionIfUserNameExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var user = new UserDTO
                {
                    FirstName = "Sisy",
                    LastName = "Ekimova",
                    Username = "Mirko",
                    Email = "miro44@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.UpdateUserAsync("Stenlyto", user));
            }
        }
    }
}
