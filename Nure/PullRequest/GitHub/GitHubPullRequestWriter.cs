using System;
using Nure.Configuration;
using Octokit;

namespace Nure.PullRequest.GitHub
{
    public class GitHubPullRequestWriter : IPullRequestWriter
    {
        private readonly NureOptions m_NureOptions;
        private readonly string m_AccessToken;

        public GitHubPullRequestWriter(NureOptions p_NureOptions,
                                       string p_AccessToken)
        {
            m_NureOptions = p_NureOptions;
            m_AccessToken = p_AccessToken;
        }

        public void WritePullRequest(string p_BranchName)
        {
            string[] githubUri = new Uri(m_NureOptions.HostingUrl).Segments;
            GitHubClient client = new GitHubClient(new ProductHeaderValue("nure")) { Credentials = new Credentials(m_AccessToken) };
            client.PullRequest.Create(githubUri[1].TrimEnd('/'), githubUri[2].TrimEnd('/'), CreatePullRequest(p_BranchName)).Wait();
        }

        private NewPullRequest CreatePullRequest(string p_BranchName)
        {
            return new NewPullRequest(m_NureOptions.PullRequestTitle, p_BranchName, m_NureOptions.DefaultBranch) { Body = m_NureOptions.PullRequestDescription };
        }
    }
}
