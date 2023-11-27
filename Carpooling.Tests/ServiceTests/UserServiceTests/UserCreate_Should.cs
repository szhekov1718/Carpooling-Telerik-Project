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
    public class UserCreate_Should
    {
        [TestMethod]
        public async Task CreateCustomer()
        {
            var options = Utils.GetOptions(nameof(CreateCustomer));
            var users = Utils.SeedUsers();

            using(var arrangeContext = new CarpoolingContext(options))
            {
                await arrangeContext.AddRangeAsync(users);

                await arrangeContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var user = new UserDTO();
                user.FirstName = "Sisy";
                user.LastName = "Ekimova";
                user.Username = "sis44";
                user.Email = "stenlyto123@gmail.com";
                user.Password = "Sis23!ekm";
                user.PhoneNumber = "0895636369";

                var customer = await sut.CreateUserAsync(user);
                Assert.AreEqual("Sisy", customer.FirstName);
                Assert.AreEqual("Ekimova", customer.LastName);
                Assert.AreEqual("stenlyto123@gmail.com", customer.Email);
                Assert.AreEqual("sis44", customer.Username);
                Assert.AreEqual("c615573d10bec69eb0504a71b33a307572a5be9f6fd222d2339e184fc0437a66", customer.Password);
                Assert.AreEqual("0895636369", customer.PhoneNumber);
            }
        }

        [TestMethod]
        public async Task ThrowArgumentExceptionIfUserNameExist()
        {
            var options = Utils.GetOptions(nameof(ThrowArgumentExceptionIfUserNameExist));
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
                    Username = "Stenlyto",
                    Email = "sis234@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<UserNameExistException>(() => sut.CreateUserAsync(user));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentExceptionIfEmailIsWrong()
        {
            var options = Utils.GetOptions(nameof(ThrowArgumentExceptionIfEmailIsWrong));
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
                    Username = "Stenlyto",
                    Email = null,
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<UserNameExistException>(() => sut.CreateUserAsync(user));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentExceptionIfUsernameLenghtNotRight()
        {
            var options = Utils.GetOptions(nameof(ThrowArgumentExceptionIfUsernameLenghtNotRight));
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
                    FirstName = "Sid",
                    LastName = "Eksda",
                    Username = "S",
                    Email = "sis234@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateUserAsync(user));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentExceptionIfUserFirstNameLenghtNotRight()
        {
            var options = Utils.GetOptions(nameof(ThrowArgumentExceptionIfUserFirstNameLenghtNotRight));
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
                    FirstName = "S",
                    LastName = "Ekimova",
                    Username = "Vito",
                    Email = "sis234@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateUserAsync(user));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentExceptionIfUserLastNameLenghtNotRight()
        {
            var options = Utils.GetOptions(nameof(ThrowArgumentExceptionIfUserLastNameLenghtNotRight));
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
                    FirstName = "Sid",
                    LastName = "E",
                    Username = "Vito",
                    Email = "sis234@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateUserAsync(user));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentExceptionIfEmailExist()
        {
            var options = Utils.GetOptions(nameof(ThrowArgumentExceptionIfEmailExist));
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
                    Username = "sis",
                    Email = "stenlyto@abv.bg",
                    Password = "Sis23!ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<EmailExistsException>(() => sut.CreateUserAsync(user));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentExceptionIfUserPasswordIsNotProvided()
        {
            var options = Utils.GetOptions(nameof(ThrowArgumentExceptionIfUserPasswordIsNotProvided));
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
                    Username = "sdfsdfsdf",
                    Email = "sis234@abv.bg",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateUserAsync(user));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentExceptionIfUserPasswordHasNoSpecialCharacter()
        {
            var options = Utils.GetOptions(nameof(ThrowArgumentExceptionIfUserPasswordHasNoSpecialCharacter));
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
                    Username = "Petppp",
                    Email = "sis234@abv.bg",
                    Password = "Sis23ekm",
                    PhoneNumber = "0895636369"
                };

                await Assert.ThrowsExceptionAsync<ArgumentException>(() => sut.CreateUserAsync(user));
            }
        }
    }
}
