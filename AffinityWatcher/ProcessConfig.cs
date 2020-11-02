namespace AffinityWatcher
{
    internal class ProcessConfig
    {
        #region Public and private fields and properties

        public string Name { get; }
        public long Affinity { get; }
        public string User { get; }

        #endregion

        #region Constructor and destructor

        public ProcessConfig(string name, long affinity, string user)
        {
            Name = name;
            Affinity = affinity;
            User = user;
        }

        #endregion
    }
}
