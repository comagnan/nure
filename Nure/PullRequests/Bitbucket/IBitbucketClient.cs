// Copyright (c) 2005-2020, Coveo Solutions Inc.

using System.Collections.Generic;
using Nure.PullRequests.Bitbucket.Models;

namespace Nure.PullRequests.Bitbucket
{
    public interface IBitbucketClient
    {
        BitbucketUser GetCurrentUser();
        List<BitbucketUser> GetDefaultReviewers();
        void SendPullRequest(BitbucketPullRequest p_BitbucketPullRequest);
    }
}
