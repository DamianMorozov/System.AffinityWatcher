using NUnit.Framework;

namespace AffinityWatcher.Tests
{
    [TestFixture]
    internal class ProgramTests
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
        public void Foo_AreEqual()
        {
            Utils.MethodStart();

            int actual = default;
            // 
            int expected = default;
            TestContext.WriteLine($"actual/expected: {actual}");
            Assert.AreEqual(expected, actual);

            Utils.MethodComplete();
        }
    }
}