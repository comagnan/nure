using System;
using System.Collections.Generic;
using System.Net.Http;
using Flurl.Http;
using Moq;
using Nure.PullRequest.Bitbucket;
using Xunit;

namespace Nure.UnitTests.PullRequest.Bitbucket
{
    public class BitbucketClientTests
    {
        private const string AN_INVALID_ADDRESS = "https://www.perdu.com";
        private const string A_VALID_ADDRESS = "https://www.perdu.com/comagnan/nure";
        private const string A_USERNAME = "root";
        private const string A_PASSWORD = "hunter3";
        private readonly Mock<IFlurlClient> m_UrlClientMock;

        public BitbucketClientTests()
        {
            m_UrlClientMock = new Mock<IFlurlClient>();
            m_UrlClientMock.Setup(client => client.Headers).Returns(new Dictionary<string, object>());
            m_UrlClientMock.Setup(client => client.HttpClient).Returns(new HttpClient());
        }

        [Fact]
        public void ClientParsesAddress()
        {
            string expectedEndpoint = "https://api.perdu.com/2.0/repositories/comagnan/nure/default-reviewers";

            try {
                BitbucketClient bitbucketClient = new BitbucketClient(A_VALID_ADDRESS, A_USERNAME, A_PASSWORD, m_UrlClientMock.Object);
                bitbucketClient.GetDefaultReviewers();
            } catch (AggregateException e) when (e.InnerException is FlurlHttpException httpException) {
                string requestUri = httpException.Call.Request.RequestUri.ToString();
                Assert.Equal(expectedEndpoint, requestUri);
            }
        }

        [Fact]
        public void ClientThrowsWithInvalidBitbucketAddress()
        {
            Assert.Throws<ArgumentException>(() => new BitbucketClient(AN_INVALID_ADDRESS, A_USERNAME, A_PASSWORD, m_UrlClientMock.Object));
        }
    }
}
