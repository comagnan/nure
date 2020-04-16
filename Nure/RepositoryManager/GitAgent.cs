using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using NLog;
using Nure.RepositoryManager.Exceptions;
using Nure.Update;

namespace Nure.RepositoryManager
{
    public class GitAgent : IGitAgent
    {
        private static readonly ILogger s_Logger = LogManager.GetCurrentClassLogger();

        private Repository m_Repository;
        private string m_BranchName;
        private readonly string m_CommitMessagePrefix;

        public GitAgent(string p_CommitMessagePrefix)
        {
            m_CommitMessagePrefix = p_CommitMessagePrefix;
        }

        public void CreateRepository(string p_DirectoryPath)
        {
            if (!Directory.Exists(p_DirectoryPath)) {
                s_Logger.Error($"Invalid directory provided. Directory does not exist. Absolute Path: {p_DirectoryPath}");
                throw new InvalidDirectoryException("Invalid directory provided.");
            }

            if (!Repository.IsValid(p_DirectoryPath)) {
                s_Logger.Error($"This is not a valid git repository. Path Provided Repository: {p_DirectoryPath}");
                throw new InvalidRepositoryException("This is not a valid git repository.");
            }

            m_Repository = new Repository(p_DirectoryPath);
        }

        public void Fetch(RunTimeParameters p_Parameters,
            string p_RemoteName,
            string p_HostingUrl)
        {
            FetchOptions options = new FetchOptions {
                CredentialsProvider = (url,
                        usernameFromUrl,
                        types) =>
                    new UsernamePasswordCredentials { Username = p_Parameters.Username, Password = p_Parameters.Password }
            };

            s_Logger.Debug($"Fetching remote: {p_RemoteName}");
            string remoteUrl = new Uri(p_HostingUrl).AbsoluteUri;
            void ActionUpdater(RemoteUpdater p_I) => p_I.Url = remoteUrl;
            m_Repository.Network.Remotes.Update(p_RemoteName, (Action<RemoteUpdater>) ActionUpdater);

            var remote = m_Repository.Network.Remotes[p_RemoteName];
            s_Logger.Debug($"Remote Url remote: {remote.Url}");
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
            Commands.Fetch(m_Repository, remote.Name, refSpecs, options, "Fetching the latest changes.");
        }

        public string SetupBranch(string p_BranchNamePrefix,
            string p_RemoteName)
        {
            //todo Create logic for branch name
            string ticketName = "INNO-001";
            string guid = "alpha1234";
            m_BranchName = $"{p_BranchNamePrefix}{ticketName}_{guid}";
            s_Logger.Info($"Branch: {m_BranchName}");

            Branch localBranch;

            if (m_Repository.Branches[m_BranchName] == null) {
                localBranch = m_Repository.CreateBranch(m_BranchName);
            } else {
                //todo will not checkout due to a conflict if there are files there already
                localBranch = m_Repository.Branches[m_BranchName];
            }

            Commands.Checkout(m_Repository, localBranch);
            var remote = m_Repository.Network.Remotes[p_RemoteName];

            m_Repository.Branches.Update(localBranch,
                b => b.Remote = remote.Name,
                b => b.UpstreamBranch = localBranch.CanonicalName);
            return m_BranchName;
        }

        public void Stage()
        {
            m_Repository.Diff.Compare<TreeChanges>().ToList().ForEach(change => s_Logger.Info($"{change.Status}: {change.Path}"));

            s_Logger.Info("Staging the changes.");
            Commands.Stage(m_Repository, "*.csproj");
            Commands.Stage(m_Repository, "*.sln");
        }

        public void Commit(Signature p_Signature)
        {
            RepositoryStatus status = m_Repository.RetrieveStatus();
            if (!status.IsDirty) return;

            string commitMessage = $"{m_CommitMessagePrefix} - Dependency Update";
            s_Logger.Info($"Commit message. {commitMessage}");

            s_Logger.Info("Commiting the changes.");
            m_Repository.Commit(commitMessage, p_Signature, p_Signature, null);
        }

        public void Push(RunTimeParameters p_Parameters)
        {
            s_Logger.Info($"Pushing {m_BranchName}.");
            PushOptions options = new PushOptions {
                CredentialsProvider = (url,
                        usernameFromUrl,
                        types) =>
                    new UsernamePasswordCredentials { Username = p_Parameters.Username, Password = p_Parameters.Password }
            };
            m_Repository.Network.Push(m_Repository.Branches[m_BranchName], options);
            s_Logger.Info("Push Complete.");
        }
    }
}
