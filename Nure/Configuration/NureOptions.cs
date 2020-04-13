namespace Nure.Configuration
{
    public class NureOptions
    {
        private bool DEFAULT_ALLOW_PRELEASE_DEPENDENCIES = false;
        
        public NureOptions()
        {
            AllowPrereleaseDependencies = DEFAULT_ALLOW_PRELEASE_DEPENDENCIES;
        }

        public string HostingService { get; set; }
        public string DefaultBranch { get; set; }
        public string NureBranchPrefix { get; set; }
        public string CommitMessage { get; set; }
        public string PullRequestDescription { get; set; }
        public bool AllowPrereleaseDependencies { get; set; }

        public override string ToString()
        {
            return $"Hosting Service: {HostingService},\n" +
                $"Default Branch: {DefaultBranch},\n" +
                $"Nure Branch Prefix: {NureBranchPrefix},\n" +
                $"Commit Message: {CommitMessage},\n" +
                $"Pull Request Description: {PullRequestDescription ?? "null"},\n" +
                $"Allow Prerelease Dependencies: {AllowPrereleaseDependencies}\n";
        }
    }
}
