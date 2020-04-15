// Copyright (c) 2005-2020, Coveo Solutions Inc.

using Bitbucket.Cloud.Net;
using Bitbucket.Cloud.Net.Common.Authentication;
using Bitbucket.Cloud.Net.Models.v2;
using Nure.Configuration;

namespace Nure.PullRequests
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

        public void Write(string p_BranchName)
        {
            string destinationBranch = m_NureOptions.DefaultBranch;
            string pullRequestDescription = m_NureOptions.PullRequestDescription;
            
            BitbucketCloudClient cloudClient = new BitbucketCloudClient("", m_BasicAuthentication);
            cloudClient.CreateRepositoryPullRequestAsync("frick", "frick", new PullRequestCreationParameters {
                CloseSourceBranch = true,
                Description = m_NureOptions.PullRequestDescription,
                Destination = new HasName(),
                Reviewers = cloudClient.GetRepositoryDefaultReviewersAsync("frick", "frick", 1).Result,
                Title = "NuRe Dependency Update",
                Source = new HasName()
            }).Wait();
            
            throw new System.NotImplementedException();
        }
    }
}
