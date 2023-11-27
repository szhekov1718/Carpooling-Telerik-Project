using System;
using Carpooling.Data;
using Carpooling.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class HashPassword_Should
    {
        [TestMethod]
        public void HashPassword()
        {
            var options = Utils.GetOptions(nameof(HashPassword));

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var result = sut.HashPassword("Pepi*22");
                Assert.AreEqual("428ed7ff0b40a507792b58445998623722c87818bcbdec0869010e5340df6fb0", result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HashPasswordg()
        {
            var options = Utils.GetOptions(nameof(HashPasswordg));

            using(var actContext = new CarpoolingContext(options))
            {
                var sut = new UserService(actContext);
                var result = sut.HashPassword(null);
            }
        }
    }
}
