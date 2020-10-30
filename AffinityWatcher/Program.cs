using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AffinityWatcher
{
    internal class Program
    {
        #region Public and private fields and properties

        private const string ConfigName = @"watch_process.xml";

        #endregion

        #region Public and private methods

        internal static void Main()
        {
            try
            {
                var configs = ParseConfig(ConfigName);
                PrintConfigStatus(configs);

                var watcher = new ProcessWatcher(configs);
                watcher.Start();

                var task = Task.Run(async () =>
                {
                    while (!Console.KeyAvailable)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(100)).ConfigureAwait(false);
                    }
                });
                task.Wait();

                watcher.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"{ex.Message}");
            }
        }

        internal static void PrintConfigStatus(List<ProcConfig> configs)
        {
            Console.WriteLine($"{Environment.ProcessorCount} visible logical processors detected");
            Console.WriteLine($"Read data for {configs.Count} process(es):");
            foreach (var config in configs)
            {
                Console.WriteLine($"  {config.ProcessName} => {config.TargetAffinity}");
            }
        }

        /// <summary>
        /// Parse xml file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static List<ProcConfig> ParseConfig(string filePath)
        {
            var result = new List<ProcConfig>();

            var tree = XElement.Load(filePath);
            var xmlProcListElement = tree.Element("processes");
            if (xmlProcListElement == null)
            {
                throw new Exception("Process list not found!");
            }

            foreach (var xmlProc in xmlProcListElement.Elements().Where(el => el.Name == "process"))
            {
                var name = xmlProc.Attribute("name")?.Value;
                var affinityStr = xmlProc.Attribute("affinity")?.Value;

                if (name == null || affinityStr == null)
                {
                    throw new Exception("Invalid entry in process list (missing attributes)");
                }

                if (!long.TryParse(affinityStr, out var affinity))
                {
                    throw new Exception($@"Invalid affinity value in process list (name: {name})");
                }

                result.Add(new ProcConfig(name, affinity));
            }

            return result;
        }

        #endregion
    }
}
