// Copyright (c) 2005-2020, Coveo Solutions Inc.

using System;
using System.IO;
using LibGit2Sharp;
using NDesk.Options;
using Newtonsoft.Json;
using NLog;
using Nure.Configuration;
using Nure.PullRequests;
using Nure.RepositoryManager;
using Nure.Update;

namespace Nure
{
    class Program
    {
        private static ILogger s_Logger = LogManager.GetCurrentClassLogger();
        private const string CONFIGURATION_FILE_NAME = "nure-config.json";

        static void Main(string[] p_Args)
        {
            RunTimeParameters runTimeParameters = new RunTimeParameters();

            var optionSet = new OptionSet {
                { "d|directory-path=", "Absolute path to the repository to update.", value => runTimeParameters.DirectoryPath = value },
                { "g|git-api-key=", "API key to push git commits.", value => runTimeParameters.GitApiKey = value },
                { "u|hosting-username=", "Username used with the hosting platform.", value => runTimeParameters.Username = value },
                { "p|hosting-password=", "Password used with the hosting platform.", value => runTimeParameters.Password = value },
                { "h|help", "Show this message", value => runTimeParameters.Help = value != null }
            };

            try {
                optionSet.Parse(p_Args);
            } catch (OptionException e) {
                s_Logger.Error(e.Message);
                s_Logger.Info("Try `--help` for more information.");
                return;
            }

            if (runTimeParameters.Help) {
                ShowHelp(optionSet);
                return;
            }

            if (!runTimeParameters.Validate(false)) {
                s_Logger.Error("Invalid arguments.");
                s_Logger.Info("Try `--help` for more information.");
                return;
            }

            Run(runTimeParameters);
        }

        private static void ShowHelp(OptionSet p_OptionSet)
        {
            s_Logger.Info("Nure:");
            s_Logger.Info("Updates the dependencies of a repository using NuGet, then sends a pull request.");

            p_OptionSet.WriteOptionDescriptions(Console.Out);
        }

        /// <summary>
        /// Updates nuget dependencies and creates a pull request.
        /// </summary>
        /// <param name="p_RuntimeParameters">Parameters Poco.</param>
        private static void Run(RunTimeParameters p_RuntimeParameters)
        {
            NureOptions nureOptions;

            try {
                TextReader json = File.OpenText(Path.Combine(p_RuntimeParameters.DirectoryPath, CONFIGURATION_FILE_NAME));
                nureOptions = JsonSerializer.CreateDefault().Deserialize<NureOptions>(new JsonTextReader(json));
                s_Logger.Info("Launching NuRe with the following configuration:\n" + nureOptions);
            } catch (FileNotFoundException exception) {
                s_Logger.Error($"Could not locate the file. {exception.Message}");
                return;
            }

            var gitWrapper = new GitAgent(nureOptions.CommitMessage);
            string remoteName = "origin";
            string branchName;

            try {
                gitWrapper.CreateRepository(p_RuntimeParameters.DirectoryPath);
                gitWrapper.Fetch(p_RuntimeParameters, remoteName, nureOptions.HostingUrl);
                branchName = gitWrapper.SetupBranch(nureOptions.NureBranchPrefix, remoteName);
            } catch (LibGit2SharpException exception) {
                s_Logger.Error($"Could not setup the branch. {exception.Message}");
                return;
            } catch (InvalidProgramException exception) {
                s_Logger.Error($"Could not create the repository. {exception.Message}");
                return;
            } catch (UriFormatException exception) {
                s_Logger.Error($"Could not fetch the repository. Hosting Url: {nureOptions.HostingUrl}. {exception.Message}");
                return;
            }

            NuKeeperWrapper nukeeper = new NuKeeperWrapper(nureOptions, p_RuntimeParameters.DirectoryPath);
            nukeeper.Run();
            s_Logger.Info("Run Complete");

            gitWrapper.Stage();
            //todo get them from the options file
            Identity identity = new Identity("Jenkins", "SomeEmail@email.com");
            Signature signature = new Signature(identity, DateTimeOffset.Now);

            try {
                gitWrapper.Commit(signature);
                gitWrapper.Push(p_RuntimeParameters);
            } catch (LibGit2SharpException exception) {
                s_Logger.Error($"Could not setup the branch. {exception.Message}");
                return;
            }

            var pullRequestWriterFactory = new PullRequestWriterFactory(nureOptions, p_RuntimeParameters.Username, p_RuntimeParameters.Password);
            pullRequestWriterFactory.Create().WritePullRequest(branchName);
        }
    }
}
