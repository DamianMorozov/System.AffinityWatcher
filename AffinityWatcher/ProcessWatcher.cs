using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace AffinityWatcher
{
    internal class ProcessWatcher
    {
        #region Public and private fields and properties

        private readonly Dictionary<string, ProcConfig> _processes;
        private ManagementEventWatcher _watcher;

        #endregion

        #region Constructor and destructor

        public ProcessWatcher(IEnumerable<ProcConfig> procConfigs)
        {
            _watcher = null;
            _processes = new Dictionary<string, ProcConfig>();
            foreach (var config in procConfigs)
            {
                if (!string.IsNullOrEmpty(config.ProcessName))
                    _processes[config.ProcessName] = config;
            }
        }

        #endregion

        #region Public and private methods

        public void Start()
        {
            if (_watcher != null)
            {
                Stop();
            }

            try
            {
                ProcessRunedSearch();

                _watcher = new ManagementEventWatcher(new WqlEventQuery("select * from win32_processstarttrace"));
                //_watcher = new ManagementEventWatcher(new WqlEventQuery("select processid, executablepath, commandline from win32_process"));
                _watcher.EventArrived += ProcessStartHandler;
                _watcher.Start();
            }
            catch
            {
                _watcher = null;
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                _watcher.Stop();
            }
            finally
            {
                _watcher = null;
            }
        }

        private void ProcessStartHandler(object sender, EventArrivedEventArgs eventArgs)
        {
            if (eventArgs is null)
                return;
            PropertyDataCollection properties = eventArgs.NewEvent.Properties;
            var name = (string) properties["ProcessName"].Value;
            var timestamp = (ulong) properties["TIME_CREATED"].Value;
            var timeOf = DateTime.FromFileTime((long) timestamp);
            var pid = (uint) properties["ProcessID"].Value;

            // apply any matching process config we have
            if (_processes.TryGetValue(name, out ProcConfig processConfig))
            {
                Process proc;
                try
                {
                    proc = Process.GetProcessById((int)pid);
                    Console.WriteLine($@"Process detected. Name: {proc.ProcessName}, PID: {proc.Id}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Exception of detected process: {ex.Message}");
                    return;
                }

                try
                {
                    ChangeProcessAffinity(proc, processConfig.TargetAffinity);
                    Console.WriteLine($@"Changed affinity of process. Delay: {TimeSince(timeOf).TotalMilliseconds}ms.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Exception of changing affinity: {ex.Message}");
                }
            }
        }

        private void ProcessRunedSearch()
        {
            foreach (var proc in Process.GetProcesses())
            {
                if (_processes.TryGetValue($"{proc.ProcessName}.exe", out ProcConfig processConfig))
                {
                    Console.WriteLine($@"Process detected. Name: {proc.ProcessName}, PID: {proc.Id}.");
                    try
                    {
                        ChangeProcessAffinity(proc, processConfig.TargetAffinity);
                        Console.WriteLine(@"Changed affinity of process.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($@"Exception of changing affinity: {ex.Message}");
                    }
                }
            }
        }

        private TimeSpan TimeSince(DateTime then)
        {
            var now = DateTime.Now;
            return now > then ? now.Subtract(then) : new TimeSpan(0, 0, 0);
        }

        private static void ChangeProcessAffinity(Process ps, long affinityMask)
        {
            var oldMask = (long)ps.ProcessorAffinity;
            ps.ProcessorAffinity = (IntPtr)(oldMask & affinityMask);
        }

        #endregion
    }
}
