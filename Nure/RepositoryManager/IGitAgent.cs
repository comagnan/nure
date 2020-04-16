// Copyright (c) 2005-2020, Coveo Solutions Inc.

using LibGit2Sharp;

namespace Nure.RepositoryManager
{
    public interface IGitAgent
    {
        void CreateRepository(string p_DirectoryPath);
        void Fetch(string p_RemoteName);
        string SetupBranch();
        void Stage();
        void Commit(Signature p_Signature);
    }
}
