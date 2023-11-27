using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Enums;
using Carpooling.Services.Exceptions.Exeception.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class GetUser_Should
    {
        [TestMethod]
        public async Task GetCustomerDTOWhitUserName()
        {
            var options = Utils.GetOptions(nameof(GetCustomerDTOWhitUserName));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = sut.GetBy("Stenlyto", FilterBy.Username.ToString());
                Assert.AreEqual("Stanislav", customer.FirstName);
                Assert.AreEqual("Simeonov", customer.LastName);
                Assert.AreEqual("stenlyto@abv.bg", customer.Email);
                Assert.AreEqual("Stenlyto", customer.Username);
                Assert.AreEqual("0854545454", customer.PhoneNumber);
            }
        }

        [TestMethod]
        public async Task GetCustomerDTOWhitEmail()
        {
            var options = Utils.GetOptions(nameof(GetCustomerDTOWhitEmail));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = sut.GetBy("stenlyto@abv.bg", FilterBy.Email.ToString());
                Assert.AreEqual("Stanislav", customer.FirstName);
                Assert.AreEqual("Simeonov", customer.LastName);
                Assert.AreEqual("stenlyto@abv.bg", customer.Email);
                Assert.AreEqual("Stenlyto", customer.Username);
                Assert.AreEqual("0854545454", customer.PhoneNumber);
            }
        }

        [TestMethod]
        public async Task GetCustomerDTOWhitPhoneNumber()
        {
            var options = Utils.GetOptions(nameof(GetCustomerDTOWhitPhoneNumber));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = sut.GetBy("0854545454", FilterBy.PhoneNumber.ToString());
                Assert.AreEqual("Stanislav", customer.FirstName);
                Assert.AreEqual("Simeonov", customer.LastName);
                Assert.AreEqual("stenlyto@abv.bg", customer.Email);
                Assert.AreEqual("Stenlyto", customer.Username);
                Assert.AreEqual("0854545454", customer.PhoneNumber);
            }
        }

        [TestMethod]
        public async Task ReturnNullIfCustomerWhitThatUserNameNotExist()
        {
            var options = Utils.GetOptions(nameof(ReturnNullIfCustomerWhitThatUserNameNotExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = sut.GetBy("st", FilterBy.Username.ToString());
                Assert.AreEqual(null, customer);
            }
        }

        [TestMethod]
        public async Task ReturnNullIfCustomerWhitThatEmailNotExist()
        {
            var options = Utils.GetOptions(nameof(ReturnNullIfCustomerWhitThatEmailNotExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = sut.GetBy("st", FilterBy.Email.ToString());
                Assert.AreEqual(null, customer);
            }
        }

        [TestMethod]
        public async Task ReturnNullIfCustomerWhitThatPhoneNumberNotExist()
        {
            var options = Utils.GetOptions(nameof(ReturnNullIfCustomerWhitThatPhoneNumberNotExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = sut.GetBy("st", FilterBy.PhoneNumber.ToString());
                Assert.AreEqual(null, customer);
            }
        }

        [TestMethod]
        public async Task ReturnCustomerNullIfCustomerWhitThatUserNameNotExist()
        {
            var options = Utils.GetOptions(nameof(ReturnCustomerNullIfCustomerWhitThatUserNameNotExist));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = await sut.GetUserByUserNameAsync("st");
                Assert.AreEqual(null, customer);
            }
        }

        [TestMethod]
        public async Task GetCustomerWhitUserName()
        {
            var options = Utils.GetOptions(nameof(GetCustomerWhitUserName));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = await sut.GetUserByUserNameAsync("Stenlyto");
                Assert.AreEqual("Stanislav", customer.FirstName);
                Assert.AreEqual("Simeonov", customer.LastName);
                Assert.AreEqual("stenlyto@abv.bg", customer.Email);
                Assert.AreEqual("Stenlyto", customer.Username);
                Assert.AreEqual("0854545454", customer.PhoneNumber);
            }
        }

        [TestMethod]
        public async Task GetAllCustomerWhitUserName()
        {
            var options = Utils.GetOptions(nameof(GetAllCustomerWhitUserName));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var customer = await sut.GetAllUsersAsync();
                var expected = new List<UserDTO>()
            {
                new UserDTO
                {
                    Username ="Stenlyto",
                    Password = "Stenlyto*12",
                    FirstName = "Stanislav",
                    LastName = "Simeonov",
                    Email = "stenlyto@abv.bg",
                    PhoneNumber = "0854545454"
                },
                new UserDTO
                {
                    Username ="Mirko",
                    Password = "Mirko*12",
                    FirstName = "Stanimir",
                    LastName = "Ivanov",
                    Email = "miro44@abv.bg",
                    PhoneNumber = "0864646464"
                },
                new UserDTO
                {
                    Username ="Pepi",
                    Password = "Pepi*22",
                    FirstName = "Petko",
                    LastName = "Mitev",
                    Email = "pepi14@abv.bg",
                    PhoneNumber = "0812122112"
                },
                new UserDTO
                {
                    Username ="gogi",
                    Password = "Gogi*12",
                    FirstName = "Georgi",
                    LastName = "Mitev",
                    Email = "gogi@abv.bg",
                    PhoneNumber = "0854545459"
                },
                new UserDTO
                {
                    Username ="tisho",
                    Password = "Tisho*12",
                    FirstName = "Todor",
                    LastName = "Todorov",
                    Email = "tisho@abv.bg",
                    PhoneNumber = "0854545453"
                },
                new UserDTO
                {
                    Username ="geri",
                    Password = "Geri*16",
                    FirstName = "Gergana",
                    LastName = "Azova",
                    Email = "geri@abv.bg",
                    PhoneNumber = "0899279132",
                }
            };
                Assert.AreEqual(expected.Count(), customer.Count());
                Assert.AreEqual(string.Join(",", expected), string.Join(",", customer));
            }
        }

        [TestMethod]
        public async Task GetAllCustomerThrowArgumentExceptionIfNoUsersExist()
        {
            var options = Utils.GetOptions(nameof(GetAllCustomerThrowArgumentExceptionIfNoUsersExist));

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);

                await Assert.ThrowsExceptionAsync<EntityNotFound>(() => sut.GetAllUsersAsync());
            }
        }
    }
}
