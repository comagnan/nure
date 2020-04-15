// Copyright (c) 2005-2020, Coveo Solutions Inc.

using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using NLog;

namespace Nure.RepositoryManager
{
    public class LibGit2SharpWrapper : ILibgit2SharpWrapper
    {
        private static readonly ILogger s_Logger = LogManager.GetCurrentClassLogger();

        private Repository m_Repository;
        private readonly string m_CommitMessagePrefix;
        private readonly string m_DefaultBranch;

        public LibGit2SharpWrapper(string p_CommitMessagePrefix,
            string p_DefaultBranch)
        {
            m_CommitMessagePrefix = p_CommitMessagePrefix;
            m_DefaultBranch = p_DefaultBranch;
        }

        public void CreateRepository(string p_DirectoryPath)
        {
            if (!Directory.Exists(p_DirectoryPath)) {
                s_Logger.Error($"Invalid directory provided. Absolute Path: {p_DirectoryPath}");
                throw new InvalidProgramException(); //placehodler
            }

            if (!Repository.IsValid(p_DirectoryPath)) {
                s_Logger.Error($"Invalid Repository provided. Repository: {p_DirectoryPath}");
                throw new InvalidProgramException(); //placehodler
            }

            m_Repository = new Repository(p_DirectoryPath);
        }

        public void Fetch(string p_RemoteName)
        {
            var remote = m_Repository.Network.Remotes[p_RemoteName];
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
            Commands.Fetch(m_Repository, remote.Name, refSpecs, null, "Fetching the latest changes.");
        }

        public void SetupBranch()
        {
            Branch defaultBranch = m_Repository.Branches[m_DefaultBranch];

            if (defaultBranch == null) {
                s_Logger.Error("Could not locate the default branch.");
                return;
            }

            //Checkout default branch
            Commands.Checkout(m_Repository, defaultBranch);

            //todo Create logic for branch name
            string ticketName = "INNO-001";
            string guid = "alpha1234";
            string branchName = $"Nure_{ticketName}_{guid}";
            s_Logger.Info($"Branch: {branchName}");

            Commands.Checkout(m_Repository, m_Repository.CreateBranch(branchName));
        }

        public void Stage()
        {
            m_Repository.Diff.Compare<TreeChanges>().ToList().ForEach(change => s_Logger.Info($"Change: {change.Status}. File name: {change.Path}"));

            s_Logger.Info("Staging the changes.");
            Commands.Stage(m_Repository, "*");
        }

        public void Commit(Signature p_Signature)
        {
            string commitMessage = $"{m_CommitMessagePrefix} - Some commit message";
            s_Logger.Info($"Commit message. {commitMessage}");

            s_Logger.Info("Commiting the changes.");
            m_Repository.Commit(commitMessage, p_Signature, p_Signature, null);
        }
    }
}
