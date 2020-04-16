// Copyright (c) 2005-2020, Coveo Solutions Inc.

namespace Nure.PullRequests
{
    public interface IPullRequestWriter
    {
        void WritePullRequest(string p_BranchName);
    }
}
