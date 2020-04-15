// Copyright (c) 2005-2020, Coveo Solutions Inc.

namespace Nure.PullRequests
{
    public interface IPullRequestWriter
    {
        void Write(string p_BranchName);
    }
}
