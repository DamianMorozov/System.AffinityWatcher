using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AffinityWatcher.Tests
{
    [TestFixture]
    internal class ProcessWatcherTests
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
        public void ProcessWatcher_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var str in EnumValues.GetString())
            {
                foreach (var lon in EnumValues.GetLong())
                {
                    Assert.DoesNotThrow(() =>
                    {
                        var procConfig = new ProcConfig(str, lon);
                        _ = new ProcessWatcher(new List<ProcConfig>() { procConfig });
                    });
                    TestContext.WriteLine($@"Assert.DoesNotThrow(() => new ProcConfig({str}, {lon}))");
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        var procConfig = new ProcConfig(str, lon);
                        _ = new ProcessWatcher(new List<ProcConfig>() { procConfig });
                    }));
                    TestContext.WriteLine($@"Assert.DoesNotThrowAsync(async () => new ProcConfig({str}, {lon}))");
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void Start_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var str in EnumValues.GetString())
            {
                foreach (var lon in EnumValues.GetLong())
                {
                    Assert.DoesNotThrow(() =>
                    {
                        var procConfig = new ProcConfig(str, lon);
                        var processWatcher = new ProcessWatcher(new List<ProcConfig>() { procConfig });
                        processWatcher.Start();
                    });
                    TestContext.WriteLine($@"Assert.DoesNotThrow(() => new ProcConfig({str}, {lon}))");
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        var procConfig = new ProcConfig(str, lon);
                        var processWatcher = new ProcessWatcher(new List<ProcConfig>() { procConfig });
                        processWatcher.Start();
                    }));
                    TestContext.WriteLine($@"Assert.DoesNotThrowAsync(async () => new ProcConfig({str}, {lon}))");
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void Stop_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var str in EnumValues.GetString())
            {
                foreach (var lon in EnumValues.GetLong())
                {
                    Assert.DoesNotThrow(() =>
                    {
                        var procConfig = new ProcConfig(str, lon);
                        var processWatcher = new ProcessWatcher(new List<ProcConfig>() { procConfig });
                        processWatcher.Start();
                        processWatcher.Stop();
                    });
                    TestContext.WriteLine($@"Assert.DoesNotThrow(() => new ProcConfig({str}, {lon}))");
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        var procConfig = new ProcConfig(str, lon);
                        var processWatcher = new ProcessWatcher(new List<ProcConfig>() { procConfig });
                        processWatcher.Start();
                        processWatcher.Stop();
                    }));
                    TestContext.WriteLine($@"Assert.DoesNotThrowAsync(async () => new ProcConfig({str}, {lon}))");
                }
            }

            Utils.MethodComplete();
        }
    }
}