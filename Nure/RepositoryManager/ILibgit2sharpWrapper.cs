// Copyright (c) 2005-2020, Coveo Solutions Inc.

namespace Nure.RepositoryManager
{
    public interface ILibgit2SharpWrapper
    {
        void CreateRepository(string p_DirectoryPath);
        void Fetch();
        void SetupBranch();
        void Stage();
        void Commit();
    }
}
