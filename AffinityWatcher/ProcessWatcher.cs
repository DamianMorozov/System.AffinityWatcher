using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace AffinityWatcher
{
    internal class ProcessWatcher
    {
        #region Public and private fields and properties

        private readonly Dictionary<string, ProcessConfig> _processes;
        private ManagementEventWatcher _watcher;

        #endregion

        #region Constructor and destructor

        public ProcessWatcher(IEnumerable<ProcessConfig> procConfigs)
        {
            _watcher = null;
            _processes = new Dictionary<string, ProcessConfig>();
            foreach (var config in procConfigs)
            {
                if (!string.IsNullOrEmpty(config.Name))
                    _processes[config.Name] = config;
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
            if (_processes.TryGetValue(name, out ProcessConfig processConfig))
            {
                var proc = Process.GetProcessById((int)pid);
                if (!string.IsNullOrEmpty(processConfig.User))
                {
                    var user = GetProcessUserName((int)pid);
                    if (processConfig.User.ToUpper().Equals(user.ToUpper()))
                    {
                        Console.WriteLine($@"Process detected. Name: {proc.ProcessName}, PID: {proc.Id}. User: {user}");
                        ChangeProcessAffinity(proc, processConfig.Affinity, timeOf);
                    }
                }
                else
                {
                    Console.WriteLine($@"Process detected. Name: {proc.ProcessName}, PID: {proc.Id}.");
                    ChangeProcessAffinity(proc, processConfig.Affinity, timeOf);
                }

            }
        }

        /// <summary>
        /// Get process username by pid.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private string GetProcessUserName(int pid)
        {
            var result = string.Empty;
            try
            {
                var searcher = new ManagementObjectSearcher($"select * from win32_process where processid = '{pid}'");
                if (searcher.Get().Count == 0)
                    return result;
                if (searcher.Get().Count == 1)
                {
                    foreach (var item in searcher.Get())
                    {
                        var process = (ManagementObject) item;
                        if (process["ExecutablePath"] != null)
                        {
                            //string ExecutablePath = process["ExecutablePath"].ToString();
                            var ownerInfo = new object[2];
                            process.InvokeMethod("GetOwner", ownerInfo);
                            result = ownerInfo[0].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (!(ex.InnerException is null))
                    msg += " " + ex.InnerException.Message;
                Console.WriteLine($@"Exception of getting process username. {msg}");
            }
            return result;
        }

        private void ProcessRunedSearch()
        {
            foreach (var proc in Process.GetProcesses())
            {
                if (_processes.TryGetValue($"{proc.ProcessName}.exe", out ProcessConfig processConfig))
                {
                    if (!string.IsNullOrEmpty(processConfig.User))
                    {
                        var user = GetProcessUserName(proc.Id);
                        if (processConfig.User.ToUpper().Equals(user.ToUpper()))
                        {
                            Console.WriteLine($@"Process detected. Name: {proc.ProcessName}, PID: {proc.Id}. User: {user}");
                            ChangeProcessAffinity(proc, processConfig.Affinity);
                        }
                    }
                    else
                    {
                        Console.WriteLine($@"Process detected. Name: {proc.ProcessName}, PID: {proc.Id}.");
                        ChangeProcessAffinity(proc, processConfig.Affinity);
                    }
                }
            }
        }

        private static TimeSpan TimeSince(DateTime then)
        {
            var now = DateTime.Now;
            return now > then ? now.Subtract(then) : new TimeSpan(0, 0, 0);
        }

        private static void ChangeProcessAffinity(Process ps, long affinityMask, DateTime timeOf = default)
        {
            try
            {
                var oldMask = (long)ps.ProcessorAffinity;
                ps.ProcessorAffinity = (IntPtr)(oldMask & affinityMask);
                Console.WriteLine(timeOf != default
                    ? $@"Changed affinity of process. Delay: {TimeSince(timeOf).TotalMilliseconds}ms."
                    : @"Changed affinity of process.");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (!(ex.InnerException is null))
                    msg += " " + ex.InnerException.Message;
                Console.WriteLine($@"Exception of changing process affinity. {msg}");
            }
        }

        #endregion
    }
}
