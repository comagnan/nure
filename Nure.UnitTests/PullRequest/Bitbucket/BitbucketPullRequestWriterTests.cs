using System.Collections.Generic;
using Moq;
using Nure.Configuration;
using Nure.PullRequest.Bitbucket;
using Nure.PullRequest.Bitbucket.Models;
using Xunit;

namespace Nure.UnitTests.PullRequest.Bitbucket
{
    public class BitbucketPullRequestWriterTests
    {
        private const string A_BRANCH_NAME = "almost-heaven/west-verginia";
        private const string A_CURRENT_USER_ID = "a";
        private const string ANOTHER_USER_ID = "b";

        private readonly Mock<IBitbucketClient> m_ClientMock = new Mock<IBitbucketClient>();

        [Fact]
        public void ValidPullRequestIsSent()
        {
            BitbucketPullRequest bitbucketPullRequest = new BitbucketPullRequest();
            List<BitbucketUser> reviewers = GivenDefaultReviewers();
            NureOptions nureOptions = GivenNureOptions();

            m_ClientMock.Setup(client => client.GetCurrentUser()).Returns(new BitbucketUser());
            m_ClientMock.Setup(client => client.GetDefaultReviewers()).Returns(reviewers);
            m_ClientMock.Setup(client => client.SendPullRequest(It.IsAny<BitbucketPullRequest>())).Callback<BitbucketPullRequest>(pr => bitbucketPullRequest = pr);

            BitbucketPullRequestWriter pullRequestWriter = new BitbucketPullRequestWriter(nureOptions, m_ClientMock.Object);
            pullRequestWriter.WritePullRequest(A_BRANCH_NAME);

            m_ClientMock.Verify(client => client.GetCurrentUser());
            m_ClientMock.Verify(client => client.GetDefaultReviewers());
            m_ClientMock.Verify(client => client.SendPullRequest(It.IsAny<BitbucketPullRequest>()));

            Assert.Equal(nureOptions.PullRequestTitle, bitbucketPullRequest.Title);
            Assert.Equal(nureOptions.PullRequestDescription, bitbucketPullRequest.Description);
            Assert.Equal(A_BRANCH_NAME, bitbucketPullRequest.SourceBranch.Branch.Name);
            Assert.Equal(nureOptions.DefaultBranch, bitbucketPullRequest.DestinationBranch.Branch.Name);
            Assert.Equal(reviewers, bitbucketPullRequest.Reviewers);
        }

        [Fact]
        public void CurrentUserIsFilteredFromDefaultReviewers()
        {
            BitbucketPullRequest bitbucketPullRequest = new BitbucketPullRequest();

            m_ClientMock.Setup(client => client.GetCurrentUser()).Returns(new BitbucketUser { UserId = A_CURRENT_USER_ID });
            m_ClientMock.Setup(client => client.GetDefaultReviewers()).Returns(GivenDefaultReviewers);
            m_ClientMock.Setup(client => client.SendPullRequest(It.IsAny<BitbucketPullRequest>())).Callback<BitbucketPullRequest>(pr => bitbucketPullRequest = pr);

            BitbucketPullRequestWriter pullRequestWriter = new BitbucketPullRequestWriter(GivenNureOptions(), m_ClientMock.Object);
            pullRequestWriter.WritePullRequest(A_BRANCH_NAME);

            Assert.All(bitbucketPullRequest.Reviewers, reviewer => Assert.NotEqual(A_CURRENT_USER_ID, reviewer.UserId));
        }

        private NureOptions GivenNureOptions() =>
            new NureOptions {
                HostingService = "bitbucket",
                HostingUrl = "https://perdu.com/project/repository",
                DefaultBranch = "blue-ridge-mountains/shenandoah-river",
                PullRequestTitle = "But before we get into the PR proper",
                PullRequestDescription = "This content is brought to you by RAID - Shadow Legends"
            };

        private List<BitbucketUser> GivenDefaultReviewers() =>
            new List<BitbucketUser> {
                new BitbucketUser { UserId = A_CURRENT_USER_ID },
                new BitbucketUser { UserId = ANOTHER_USER_ID }
            };
    }
}
