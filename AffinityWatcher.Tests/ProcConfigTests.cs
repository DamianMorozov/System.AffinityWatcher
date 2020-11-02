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

            foreach (var name in EnumValues.GetString())
            {
                foreach (var affinity in EnumValues.GetLong())
                {
                    foreach (var user in EnumValues.GetString())
                    {
                        Assert.DoesNotThrow(() =>
                        {
                            _ = new ProcessConfig(name, affinity, user);
                        });
                        TestContext.WriteLine($@"Assert.DoesNotThrow(() => new ProcConfig({name}, {affinity}, {user}))");
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                        {
                            _ = new ProcessConfig(name, affinity, user);
                        }));
                        TestContext.WriteLine($@"Assert.DoesNotThrowAsync(async () => new ProcConfig({name}, {affinity}, {user}))");
                    }
                }
            }

            Utils.MethodComplete();
        }
    }
}