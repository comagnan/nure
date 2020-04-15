using System.Collections.Generic;

namespace Nure.Configuration
{
    public class NureOptions
    {
        private bool DEFAULT_ALLOW_PRELEASE_DEPENDENCIES = false;
        
        public NureOptions()
        {
            AllowPrereleaseDependencies = DEFAULT_ALLOW_PRELEASE_DEPENDENCIES;
            CustomNugetSources = new List<string>();
            PackagesToIgnore = new List<string>();
        }

        public string HostingService { get; set; }
        public string DefaultBranch { get; set; }
        public string NureBranchPrefix { get; set; }
        public string CommitMessage { get; set; }
        public string PullRequestDescription { get; set; }
        public List<string> CustomNugetSources { get; set; }
        public List<string> PackagesToIgnore { get; set; }
        public bool AllowPrereleaseDependencies { get; set; }

        public override string ToString()
        {
            return $"Hosting Service: {HostingService},\n" +
                $"Default Branch: {DefaultBranch},\n" +
                $"NuRe Branch Prefix: {NureBranchPrefix},\n" +
                $"Commit Message: {CommitMessage},\n" +
                $"Pull Request Description: {PullRequestDescription ?? "null"},\n" +
                $"Custom NuGet Sources: {CustomNugetSources},\n" +
                $"Packages To Ignore: {PackagesToIgnore},\n" +
                $"Allow Prerelease Dependencies: {AllowPrereleaseDependencies}\n";
        }
    }
}
