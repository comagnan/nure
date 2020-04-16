// Copyright (c) 2005-2020, Coveo Solutions Inc.

using Bitbucket.Cloud.Net;
using Bitbucket.Cloud.Net.Common.Authentication;
using Bitbucket.Cloud.Net.Models.v2;
using Nure.Configuration;

namespace Nure.PullRequests.Bitbucket
{
    public class BitbucketPullRequestWriter : IPullRequestWriter
    {
        private readonly NureOptions m_NureOptions;
        private readonly BasicAuthentication m_BasicAuthentication;

        public BitbucketPullRequestWriter(NureOptions p_NureOptions,
            string p_BitbucketUsername,
            string p_BitbucketPassword)
        {
            m_NureOptions = p_NureOptions;
            m_BasicAuthentication = new BasicAuthentication(p_BitbucketUsername, p_BitbucketPassword);
        }

        public void WritePullRequest(string p_BranchName)
        {
            BitbucketRepository repository = new BitbucketRepository(m_NureOptions.HostingUrl);

            BitbucketCloudClient cloudClient = new BitbucketCloudClient(repository.BaseUrl, m_BasicAuthentication);
            cloudClient.CreateRepositoryPullRequestAsync(repository.WorkspaceId, repository.RepositoryId, new PullRequestCreationParameters {
                CloseSourceBranch = true,
                Description = m_NureOptions.PullRequestDescription,
                Destination = new Branch { Name = m_NureOptions.DefaultBranch },
                Reviewers = cloudClient.GetRepositoryDefaultReviewersAsync(repository.WorkspaceId, repository.RepositoryId, 1).Result,
                Title = "NuRe Dependency Update",
                Source = new Branch { Name = p_BranchName }
            }).Wait();
        }
    }
}
