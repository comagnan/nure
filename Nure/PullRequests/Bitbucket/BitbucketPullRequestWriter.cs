using System.Linq;
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

            var cloudClient = new BitbucketCloudClient(repository.BaseUrl, m_BasicAuthentication);
            var reviewers = cloudClient.GetRepositoryDefaultReviewersAsync(repository.WorkspaceId, repository.RepositoryId, 1).Result.Select(reviewer => new HasUuid { Uuid = reviewer.Uuid });
            var creationParameters = new PullRequestCreationParameters {
                CloseSourceBranch = true,
                Description = m_NureOptions.PullRequestDescription,
                Destination = new BranchInfo { Branch = new Branch { Name = m_NureOptions.DefaultBranch } },
                Reviewers = reviewers,
                Title = m_NureOptions.PullRequestTitle,
                Source = new BranchInfo { Branch = new Branch { Name = p_BranchName } }
            };

            cloudClient.CreateRepositoryPullRequestAsync(repository.WorkspaceId, repository.RepositoryId, creationParameters).Wait();
        }
    }
}
