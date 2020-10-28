using System;
using Nure.Configuration;
using Nure.PullRequest;
using Nure.PullRequest.Bitbucket;
using Nure.PullRequest.GitHub;
using Xunit;

namespace Nure.UnitTests.PullRequest
{
    public class PullRequestWriterFactoryTests
    {
        private const string A_USERNAME = "root";
        private const string A_PASSWORD = "hunter3";

        [Theory]
        [InlineData("bitbucket", typeof(BitbucketPullRequestWriter))]
        [InlineData("github", typeof(GitHubPullRequestWriter))]
        public void FactoryCreatesCorrectWriter(string p_HostingService,
                                                Type p_WriterType)
        {
            PullRequestWriterFactory pullRequestWriterFactory = new PullRequestWriterFactory(GivenNureOptions(p_HostingService), A_USERNAME, A_PASSWORD);
            Assert.Equal(p_WriterType, pullRequestWriterFactory.Create().GetType());
        }

        [Fact]
        public void FactoryThrowsWithInvalidHostingService()
        {
            PullRequestWriterFactory pullRequestWriterFactory = new PullRequestWriterFactory(GivenNureOptions("perdu"), A_USERNAME, A_PASSWORD);

            Assert.Throws<NotImplementedException>(() => pullRequestWriterFactory.Create());
        }

        private NureOptions GivenNureOptions(string p_HostingService) =>
            new NureOptions {
                HostingService = p_HostingService,
                HostingUrl = "https://perdu.com/project/repository"
            };
    }
}
