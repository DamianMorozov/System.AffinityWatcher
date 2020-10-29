namespace AffinityWatcher
{
    internal class ProcConfig
    {
        #region Public and private fields and properties

        public string ProcessName { get; }
        public long TargetAffinity { get; }

        #endregion

        #region Constructor and destructor

        public ProcConfig(string processName, long targetAffinity)
        {
            ProcessName = processName;
            TargetAffinity = targetAffinity;
        }

        #endregion
    }
}
