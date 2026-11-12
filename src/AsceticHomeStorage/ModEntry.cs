namespace ProbablyStolen.AsceticHomeStorage
{
    public sealed class ModEntry : IMod
    {
        internal static ModLog? Log { get; private set; }
        internal static bool Enabled { get; private set; }
        public void Init(ModManifest manifest) => Log = new ModLog(manifest);
        public void OnEnable()
        {
            StoragePolicy.ResetDiagnostics();
            Enabled = true;
            Log?.Log("Enabled: restore ascetic main storage only after property is fully paid.");
        }
        public void OnDisable()
        {
            // Do not shrink a populated grid during play. Restart reconstructs vanilla dimensions.
            Enabled = false;
        }
    }
}
