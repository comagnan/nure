using LibGit2Sharp;
using Nure.Update;

namespace Nure.RepositoryManager
{
    public interface IGitAgent
    {
        void CreateRepository(string p_DirectoryPath);

        string SetupBranch(string p_BranchNamePrefix,
            string p_RemoteName);

        void Fetch(RunTimeParameters p_Parameters,
            string p_RemoteName,
            string p_HostingUrl);

        void Stage();
        void Commit(Signature p_Signature);
        void Push(RunTimeParameters p_Parameters);
    }
}
