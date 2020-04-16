// Copyright (c) 2005-2020, Coveo Solutions Inc.

using System.Collections.Generic;

namespace Nure.Configuration
{
    public class NureOptions
    {
        private bool DEFAULT_ALLOW_PRELEASE_DEPENDENCIES = false;
        private const string DEFAULT_PULL_REQUEST_TITLE = "[NuRe] Dependency update";
        private const string DEFAULT_COMMIT_MESSAGE_PREFIX = "Update {} to {}";

        public NureOptions()
        {
            AllowPrereleaseDependencies = DEFAULT_ALLOW_PRELEASE_DEPENDENCIES;
            CustomNugetSources = new List<string>();
            PackagesToIgnore = new List<string>();
            CommitMessage = DEFAULT_COMMIT_MESSAGE_PREFIX;
            PullRequestTitle = DEFAULT_PULL_REQUEST_TITLE;
        }

        public string HostingService { get; set; }
        public string HostingUrl { get; set; }
        public string DefaultBranch { get; set; }
        public string NureBranchPrefix { get; set; }
        public string CommitMessage { get; set; }
        public string PullRequestTitle { get; set; }
        public string PullRequestDescription { get; set; }
        public List<string> CustomNugetSources { get; set; }
        public List<string> PackagesToIgnore { get; set; }
        public bool AllowPrereleaseDependencies { get; set; }

        public override string ToString()
        {
            return $"Hosting Service: {HostingService},\n" +
                $"Hosting Url: {HostingUrl},\n" +
                $"Default Branch: {DefaultBranch},\n" +
                $"NuRe Branch Prefix: {NureBranchPrefix},\n" +
                $"Commit Message: {CommitMessage},\n" +
                $"Pull Request Title: {PullRequestTitle},\n" +
                $"Pull Request Description: {PullRequestDescription ?? "null"},\n" +
                $"Custom NuGet Sources: [{string.Join(",", CustomNugetSources)}],\n" +
                $"Packages To Ignore: [{string.Join(",", PackagesToIgnore)}],\n" +
                $"Allow Prerelease Dependencies: {AllowPrereleaseDependencies}\n";
        }
    }
}
