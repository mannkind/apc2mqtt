using System;
using System.Linq;
using APC.Liasons;
using APC.Models.Options;
using APC.Models.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoMQTT.Utils;

namespace APCTest.Liasons
{
    [TestClass]
    public class MQTTLiasonTest
    {
        [TestMethod]
        public void MapDataTest()
        {
            var tests = new[] {
                new {
                    Q = new SlugMapping { SerialNo = BasicSerialNo, Slug = BasicSlug },
                    Resource = new Resource { SerialNo = BasicSerialNo, BCharge = BasicBCharge },
                    Expected = new { SerialNo = BasicSerialNo, BCharge = BasicBCharge.ToString("0"), Slug = BasicSlug, Found = true}
                },
                new {
                    Q = new SlugMapping { SerialNo = BasicSerialNo, Slug = BasicSlug },
                    Resource = new Resource { SerialNo = $"{BasicSerialNo}-fake" , BCharge = BasicBCharge },
                    Expected = new { SerialNo = string.Empty, BCharge = BasicBCharge.ToString("0"), Slug = string.Empty, Found = false}
                },
            };

            foreach (var test in tests)
            {
                var logger = new Mock<ILogger<MQTTLiason>>();
                var generator = new Mock<IMQTTGenerator>();
                var sharedOpts = Options.Create(new SharedOpts
                {
                    Resources = new[] { test.Q }.ToList(),
                });

                generator.Setup(x => x.BuildDiscovery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<System.Reflection.AssemblyName>(), false))
                    .Returns(new TwoMQTT.Models.MQTTDiscovery());
                generator.Setup(x => x.StateTopic(test.Q.Slug, nameof(Resource.BCharge)))
                    .Returns($"totes/{test.Q.Slug}/topic/{nameof(Resource.BCharge)}");

                var mqttLiason = new MQTTLiason(logger.Object, generator.Object, sharedOpts);
                var results = mqttLiason.MapData(test.Resource);
                var actual = results.FirstOrDefault();

                Assert.AreEqual(test.Expected.Found, results.Any(), "The mapping should exist if found.");
                if (test.Expected.Found)
                {
                    Assert.IsTrue(actual.topic.Contains(test.Expected.Slug), "The topic should contain the expected SerialNo.");
                    Assert.AreEqual(test.Expected.BCharge, actual.payload, "The payload be the expected BCharge.");
                }
            }
        }

        //[TestMethod]
        public void DiscoveriesTest()
        {
            var tests = new[] {
                new {
                    Q = new SlugMapping { SerialNo = BasicSerialNo, Slug = BasicSlug },
                    Resource = new Resource { SerialNo = BasicSerialNo, BCharge = BasicBCharge },
                    Expected = new { SerialNo = BasicSerialNo, BCharge = BasicBCharge, Slug = BasicSlug }
                },
            };

            foreach (var test in tests)
            {
                var logger = new Mock<ILogger<MQTTLiason>>();
                var generator = new Mock<IMQTTGenerator>();
                var sharedOpts = Options.Create(new SharedOpts
                {
                    Resources = new[] { test.Q }.ToList(),
                });

                generator.Setup(x => x.BuildDiscovery(test.Q.Slug, nameof(Resource.BCharge), It.IsAny<System.Reflection.AssemblyName>(), false))
                    .Returns(new TwoMQTT.Models.MQTTDiscovery());

                var mqttLiason = new MQTTLiason(logger.Object, generator.Object, sharedOpts);
                var results = mqttLiason.Discoveries();
                var result = results.FirstOrDefault();

                Assert.IsNotNull(result, "A discovery should exist.");
            }
        }

        private static string BasicSlug = "totallyaslug";
        private static double BasicBCharge = 28;
        private static string BasicSerialNo = "5881ABCDEF";
    }
}
