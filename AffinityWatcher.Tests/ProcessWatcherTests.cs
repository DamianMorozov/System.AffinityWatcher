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

            foreach (var name in EnumValues.GetString())
            {
                foreach (var affinity in EnumValues.GetLong())
                {
                    foreach (var user in EnumValues.GetString())
                    {
                        Assert.DoesNotThrow(() =>
                        {
                            var procConfig = new ProcessConfig(name, affinity, user);
                            _ = new ProcessWatcher(new List<ProcessConfig>() { procConfig });
                        });
                        TestContext.WriteLine($@"Assert.DoesNotThrow(() => new ProcConfig({name}, {affinity}, {user}))");
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                        {
                            var procConfig = new ProcessConfig(name, affinity, user);
                            _ = new ProcessWatcher(new List<ProcessConfig>() { procConfig });
                        }));
                        TestContext.WriteLine($@"Assert.DoesNotThrowAsync(async () => new ProcConfig({name}, {affinity}, {user}))");
                    }
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void Start_DoesNotThrow()
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
                            var procConfig = new ProcessConfig(name, affinity, user);
                            var processWatcher = new ProcessWatcher(new List<ProcessConfig>() { procConfig });
                            processWatcher.Start();
                        });
                        TestContext.WriteLine($@"Assert.DoesNotThrow(() => new ProcConfig({name}, {affinity}, {user}))");
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                        {
                            var procConfig = new ProcessConfig(name, affinity, user);
                            var processWatcher = new ProcessWatcher(new List<ProcessConfig>() { procConfig });
                            processWatcher.Start();
                        }));
                        TestContext.WriteLine($@"Assert.DoesNotThrowAsync(async () => new ProcConfig({name}, {affinity}, {user}))");
                    }
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void Stop_DoesNotThrow()
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
                            var procConfig = new ProcessConfig(name, affinity, user);
                            var processWatcher = new ProcessWatcher(new List<ProcessConfig>() { procConfig });
                            processWatcher.Start();
                            processWatcher.Stop();
                        });
                        TestContext.WriteLine($@"Assert.DoesNotThrow(() => new ProcConfig({name}, {affinity}, {user}))");
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                        {
                            var procConfig = new ProcessConfig(name, affinity, user);
                            var processWatcher = new ProcessWatcher(new List<ProcessConfig>() { procConfig });
                            processWatcher.Start();
                            processWatcher.Stop();
                        }));
                        TestContext.WriteLine($@"Assert.DoesNotThrowAsync(async () => new ProcConfig({name}, {affinity}, {user}))");
                    }
                }
            }

            Utils.MethodComplete();
        }
    }
}