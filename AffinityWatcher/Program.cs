using System;
using System.Collections.Generic;
using System.Linq;
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
            var configs = new List<ProcConfig>();
            try
            {
                ParseConfig(ConfigName, configs);
                PrintConfigStatus(configs);

                var watcher = new ProcessWatcher(configs);
                watcher.Start();

                while (!Console.KeyAvailable)
                {
                    System.Threading.Thread.Sleep(100);
                }

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
                Console.WriteLine("  {0} => {1}", config.ProcessName, config.TargetAffinity);
            }
        }

        internal static void ParseConfig(string filePath, List<ProcConfig> list)
        {
            list.Clear();

            var tree = XElement.Load(filePath);
            var xmlProcListElement = tree.Element("processes");
            if (xmlProcListElement == null)
            {
                throw new Exception("process list not found");
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
                    throw new Exception($@"Invalid affinity value in process list (name = ""{name}"")");
                }

                list.Add(new ProcConfig(name, affinity));
            }
        }

        #endregion
    }
}
