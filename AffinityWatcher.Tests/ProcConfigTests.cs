using NUnit.Framework;
using System.Threading.Tasks;

namespace AffinityWatcher.Tests
{
    [TestFixture]
    internal class ProcConfigTests
    {
        /// <summary>
        /// Setup private fields.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            Utils.MethodStart();
            // 
            Utils.MethodComplete();
        }

        /// <summary>
        /// Reset private fields to default state.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            Utils.MethodStart();
            // 
            Utils.MethodComplete();
        }

        [Test]
        public void ProcConfig_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var str in EnumValues.GetString())
            {
                foreach (var lon in EnumValues.GetLong())
                {
                    Assert.DoesNotThrow(() =>
                    {
                        _ = new ProcConfig(str, lon);
                    });
                    TestContext.WriteLine($@"Assert.DoesNotThrow(() => new ProcConfig({str}, {lon}))");
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        _ = new ProcConfig(str, lon);
                    }));
                    TestContext.WriteLine($@"Assert.DoesNotThrowAsync(async () => new ProcConfig({str}, {lon}))");
                }
            }

            Utils.MethodComplete();
        }
    }
}