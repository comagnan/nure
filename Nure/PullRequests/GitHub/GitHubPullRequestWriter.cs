// Copyright (c) 2005-2020, Coveo Solutions Inc.

using GitHub;
using Nure.Configuration;

namespace Nure.PullRequests
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
            GitHubClient client = new GitHubClient(new OAuth2Token(m_AccessToken));
            throw new System.NotImplementedException();
        }
    }
}
