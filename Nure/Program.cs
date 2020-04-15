using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LibGit2Sharp;
using NDesk.Options;
using Nure.Configuration;

namespace Nure
{
    class Program
    {
        private const string CONFIGURATION_FILE_NAME = "nure-config.json";

        static void Main(string[] p_Args)
        {
            bool show_help = false;
            List<string> parameters = new List<string>();

            var optionSet = new OptionSet() {
                { "d|directory-path=", "Absolute path to the repository to update.", value => parameters.Add(value) },
                { "g|git-api-key=", "API key to push git commits.", value => parameters.Add(value) },
                { "k|hosting-api-key=", "API key to create pull requests.", value => parameters.Add(value) },
                { "h|help", "Show this message", value => show_help = value != null }
            };

            try {
                optionSet.Parse(p_Args);
            } catch (OptionException e) {
                Console.Write(e.Message);
                Console.Write("Try `--help` for more information.");
                return;
            }

            if (show_help) {
                ShowHelp(optionSet);
                return;
            }

            if (parameters.Count != 3) {
                Console.Write("Invalid number of arguments.");
                Console.Write("Try `--help` for more information.");
            }

            Run(parameters[0], parameters[1], parameters[2]);
        }

        private static void ShowHelp(OptionSet p_OptionSet)
        {
            Console.WriteLine("Nure:");
            Console.WriteLine("Updates the dependencies of a repository using NuGet, then sends a pull request.");

            p_OptionSet.WriteOptionDescriptions(Console.Out);
        }

        /// <summary>
        /// Updates nuget dependencies and creates a pull request.
        /// </summary>
        /// <param name="p_DirectoryPath">Absolute path to the repository to update.</param>
        /// <param name="p_GitApiKey">API key to use to push commits.</param>
        /// <param name="p_HostingApiKey">API key to create pull re4quests.</param>
        static void Run(string p_DirectoryPath,
            string p_GitApiKey,
            string p_HostingApiKey)
        {
            string jsonString = File.ReadAllText(Path.Combine(p_DirectoryPath, CONFIGURATION_FILE_NAME));
            NureOptions nureOptions = JsonSerializer.Deserialize<NureOptions>(jsonString);
            Console.WriteLine(nureOptions.ToString());

            if(!Directory.Exists(p_DirectoryPath))
            {
                Console.WriteLine($"Invalid directory provided. Absolute Path: {p_DirectoryPath}");
                return;
            }

            if(Repository.IsValid(p_DirectoryPath))
            {
                Console.WriteLine($"Invalid Repository provided. Repository: {p_DirectoryPath}");
                return;
            }

            Repository targetRepository = new Repository(p_DirectoryPath);

            //Fetch latest Changes
            //todo parametrize remote name
            string logMessage = "Fetching the latest changes.";
            var remote = targetRepository.Network.Remotes["origin"];
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
            Commands.Fetch(targetRepository, remote.Name, refSpecs, null, logMessage);

            //get default branch
            Branch defaultBranch = targetRepository.Branches[nureOptions.DefaultBranch];

            if (defaultBranch == null)
            {
               Console.WriteLine("Could not locate the default branch.");
               return;
            }

            //Checkout default branch
            Commands.Checkout(targetRepository , defaultBranch);

            //Create branch name
            string ticketName = "_";
            string guid = "A number";
            string branchName = $"Nure_{ticketName}_{guid}";
            //todo Create branch from default
            Branch newBranch = targetRepository.CreateBranch(branchName);

            //Todo Call the package updater

            //todo Add all the changes
            Commands.Stage(targetRepository, "*");

            Identity identity = new Identity("Jenkins", "SomeEmail@email.com");
            Signature signature = new Signature(identity, DateTimeOffset.Now);

            // create commit message
            string commitMessage = $"{nureOptions.CommitMessage} - Some commit message";

            // commit the changes
            targetRepository.Commit(commitMessage,signature, signature, null);
        }
    }
}
