using System;
using System.Collections.Generic;
using Flurl;
using Flurl.Http;
using NLog;
using Nure.PullRequest.Bitbucket.Models;

namespace Nure.PullRequest.Bitbucket
{
    public class BitbucketClient : IBitbucketClient
    {
        private const string API_VERSION = "2.0";
        private static readonly ILogger s_Logger = LogManager.GetCurrentClassLogger();

        private readonly string m_BaseUrl;
        private readonly string m_WorkspaceId;
        private readonly string m_RepositoryId;

        private readonly string m_Username;
        private readonly string m_Password;
        private readonly IFlurlClient m_FlurlClient;

        public BitbucketClient(string p_BitbucketAddress,
                               string p_Username,
                               string p_Password,
                               IFlurlClient p_FlurlClient)
        {
            m_Username = p_Username;
            m_Password = p_Password;
            m_FlurlClient = p_FlurlClient;

            Uri bitbucketUri = new Uri(p_BitbucketAddress);
            if (bitbucketUri.Segments.Length < 3) {
                throw new ArgumentException($"The given Bitbucket address ({bitbucketUri}) does not appear to contain the workspace and project.");
            }

            string hostname = bitbucketUri.Authority.Replace("www.", "");
            m_BaseUrl = $"https://api.{hostname}";
            m_WorkspaceId = bitbucketUri.Segments[1].TrimEnd('/');
            m_RepositoryId = bitbucketUri.Segments[2].TrimEnd('/');
        }

        public BitbucketUser GetCurrentUser()
        {
            s_Logger.Info("Getting current user from Bitbucket.");

            return new Url(m_BaseUrl).AppendPathSegments(API_VERSION, "user").WithBasicAuth(m_Username, m_Password).WithClient(m_FlurlClient).GetJsonAsync<BitbucketUser>().Result;
        }

        public List<BitbucketUser> GetDefaultReviewers()
        {
            s_Logger.Info($"Getting default reviewers of repository {m_RepositoryId} from Bitbucket.");
            BitbucketPagedResult<BitbucketUser> reviewers = new Url(m_BaseUrl).AppendPathSegments(API_VERSION, "repositories", m_WorkspaceId, m_RepositoryId, "default-reviewers").WithBasicAuth(m_Username, m_Password).WithClient(m_FlurlClient).GetJsonAsync<BitbucketPagedResult<BitbucketUser>>().Result;

            return reviewers.Values;
        }

        public void SendPullRequest(BitbucketPullRequest p_BitbucketPullRequest)
        {
            s_Logger.Info($"Sending pull request merging {p_BitbucketPullRequest.SourceBranch.Branch.Name} into {p_BitbucketPullRequest.DestinationBranch.Branch.Name}");

            new Url(m_BaseUrl).AppendPathSegments(API_VERSION, "repositories", m_WorkspaceId, m_RepositoryId, "pullrequests").WithBasicAuth(m_Username, m_Password).WithClient(m_FlurlClient).PostJsonAsync(p_BitbucketPullRequest).Wait();
        }
    }
}
