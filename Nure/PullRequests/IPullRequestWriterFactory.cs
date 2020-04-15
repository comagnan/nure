// Copyright (c) 2005-2020, Coveo Solutions Inc.

using Nure.Configuration;

namespace Nure.PullRequests
{
    public interface IPullRequestWriterFactory
    {
        IPullRequestWriter Create();
    }
}
