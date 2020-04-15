using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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
                optionSet.WriteOptionDescriptions(Console.Out);
                return;
            }

            Run(parameters[0], parameters[1], parameters[2]);
        }

        /// <summary>
        /// Updates nuget dependencies and creates a pull request.
        /// </summary>
        /// <param name="directoryPath">Absolute path to the repository to update.</param>
        /// <param name="gitApiKey">API key to use to push commits.</param>
        /// <param name="hostingApiKey">API key to create pull requests.</param>
        static void Run(string directoryPath,
            string gitApiKey,
            string hostingApiKey)
        {
            string jsonString = File.ReadAllText(Path.Combine(directoryPath, CONFIGURATION_FILE_NAME));
            NureOptions options = JsonSerializer.Deserialize<NureOptions>(jsonString);
            Console.WriteLine(options.ToString());
        }
    }
}
