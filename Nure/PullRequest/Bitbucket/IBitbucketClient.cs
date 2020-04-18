using System.Collections.Generic;
using Nure.PullRequest.Bitbucket.Models;

namespace Nure.PullRequest.Bitbucket
{
    public interface IBitbucketClient
    {
        BitbucketUser GetCurrentUser();
        List<BitbucketUser> GetDefaultReviewers();
        void SendPullRequest(BitbucketPullRequest p_BitbucketPullRequest);
    }
}
