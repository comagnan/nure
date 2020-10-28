namespace Nure.Update
{
    internal sealed record NuGetPackage
    {
        public string Name { get; }
        public string RequestedVersion { get; }
        public string ResolvedVersion { get; }
        public string LatestVersion { get; }

        public NuGetPackage(string p_Name,
                            string p_RequestedVersion,
                            string p_ResolvedVersion,
                            string p_LatestVersion)
        {
            Name = p_Name;
            RequestedVersion = p_RequestedVersion;
            ResolvedVersion = p_ResolvedVersion;
            LatestVersion = p_LatestVersion;
        }
    }
}
