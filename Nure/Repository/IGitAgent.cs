using LibGit2Sharp;
using Nure.Update;

namespace Nure.Repository
{
    public interface IGitAgent
    {
        void CreateRepository(string p_DirectoryPath);

        string SetupBranch(string p_BranchNamePrefix,
                           string p_RemoteName);

        void Fetch(RuntimeParameters p_Parameters,
                   string p_RemoteName,
                   string p_HostingUrl);

        void Stage();
        void Commit(Signature p_Signature);
        void Push(RuntimeParameters p_Parameters);
    }
}
