using System;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Exceptions.Exeception.User;
using Carpooling.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpooling.Tests.ServiceTests.FeedbackServiceTests
{
    [TestClass]
    public class Delete_Should
    {
        [TestMethod]
        public async Task DeleteFeedback_Should()
        {
            var options = Utils.GetOptions(nameof(DeleteFeedback_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);

                var result = await sut.DeleteAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442143"), Guid.Parse("943b692d-330e-405d-a019-c3d728442151"));

                Assert.IsTrue(result);
            }
        }
        [TestMethod]
        public async Task DeleteFeedbackInvalidFeedbackId_Should()
        {
            var options = Utils.GetOptions(nameof(DeleteFeedbackInvalidFeedbackId_Should));
            var candidates = Utils.SeedTripCandidates();
            var feedbacks = Utils.SeedFeedbacks();

            using(var arrContext = new CarpoolingContext(options))
            {
                await arrContext.TripCandidates.AddRangeAsync(candidates);
                await arrContext.Feedbacks.AddRangeAsync(feedbacks);
                await arrContext.SaveChangesAsync();
            }

            using(var actContext = new CarpoolingContext(options))
            {
                var sutHelp = new UserService(actContext);
                var sut = new FeedbackService(actContext, sutHelp);

                await Assert.ThrowsExceptionAsync<FeedbackException>(() => sut.DeleteAsync(Guid.Parse("943b692d-330e-405d-a019-c3d728442143"), Guid.Parse("943b692d-330e-405d-a019-c3d728440000")));
            }
        }
    }
}
