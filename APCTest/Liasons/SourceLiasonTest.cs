using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APC.DataAccess;
using APC.Liasons;
using APC.Models.Options;
using APC.Models.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace APCTest.Liasons
{
    [TestClass]
    public class SourceLiasonTest
    {
        [TestMethod]
        public async Task FetchAllAsyncTest()
        {
            var tests = new[] {
                new {
                    Q = new SlugMapping { SerialNo = BasicSerialNo, Slug = BasicSlug },
                    Expected = new { SerialNo = BasicSerialNo, BCharge = BasicBCharge }
                },
            };

            foreach (var test in tests)
            {
                var logger = new Mock<ILogger<SourceLiason>>();
                var sourceDAO = new Mock<ISourceDAO>();
                var opts = Options.Create(new SourceOpts());
                var sharedOpts = Options.Create(new SharedOpts
                {
                    Resources = new[] { test.Q }.ToList(),
                });

                sourceDAO.Setup(x => x.FetchOneAsync(test.Q, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new APC.Models.Source.Response
                     {
                         SerialNo = test.Expected.SerialNo,
                         BCharge = test.Expected.BCharge,
                     });

                var sourceLiason = new SourceLiason(logger.Object, sourceDAO.Object, opts, sharedOpts);
                await foreach (var result in sourceLiason.ReceiveDataAsync())
                {
                    Assert.AreEqual(test.Expected.SerialNo, result.SerialNo);
                    Assert.AreEqual(test.Expected.BCharge, result.BCharge);
                }
            }
        }

        private static string BasicSlug = "totallyaslug";
        private static double BasicBCharge = 28;
        private static string BasicSerialNo = "15873525";
    }
}
