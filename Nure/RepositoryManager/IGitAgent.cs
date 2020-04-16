// Copyright (c) 2005-2020, Coveo Solutions Inc.

using LibGit2Sharp;
using Nure.Update;

namespace Nure.RepositoryManager
{
    public interface IGitAgent
    {
        void CreateRepository(string p_DirectoryPath);
        void Fetch(string p_RemoteName);
        void SetupBranch(string p_BranchNamePrefix, string p_RemoteName);
        void Stage();
        void Commit(Signature p_Signature);
        void Push(RunTimeParameters p_Parameters);
    }
}
