using System.Collections.Generic;
using System.Linq;
using NLog;
using Nure.Configuration;
using Nure.PullRequest.Bitbucket.Models;

namespace Nure.PullRequest.Bitbucket
{
    public class BitbucketPullRequestWriter : IPullRequestWriter
    {
        private static readonly ILogger s_Logger = LogManager.GetCurrentClassLogger();

        private readonly NureOptions m_NureOptions;
        private readonly IBitbucketClient m_BitbucketClient;

        public BitbucketPullRequestWriter(NureOptions p_NureOptions,
            IBitbucketClient p_BitbucketClient)
        {
            m_NureOptions = p_NureOptions;
            m_BitbucketClient = p_BitbucketClient;
        }

        public void WritePullRequest(string p_BranchName)
        {
            s_Logger.Info("Preparing Bitbucket pull request...");

            BitbucketUser currentUser = m_BitbucketClient.GetCurrentUser();
            List<BitbucketUser> defaultReviewers = m_BitbucketClient.GetDefaultReviewers().Where(reviewer => reviewer.UserId != currentUser.UserId).ToList();
            BitbucketPullRequest pullRequest = new BitbucketPullRequest {
                Title = m_NureOptions.PullRequestTitle,
                Description = m_NureOptions.PullRequestDescription,
                SourceBranch = new BitbucketBranchInfo { Branch = new BitbucketBranch { Name = p_BranchName } },
                DestinationBranch = new BitbucketBranchInfo { Branch = new BitbucketBranch { Name = m_NureOptions.DefaultBranch } },
                Reviewers = defaultReviewers,
                CloseBranchAfterMerge = true
            };

            m_BitbucketClient.SendPullRequest(pullRequest);
        }
    }
}
