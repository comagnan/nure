﻿// Copyright (c) 2005-2020, Coveo Solutions Inc.

using System;
using System.Collections.Generic;
using System.IO;
using NDesk.Options;
using Newtonsoft.Json;
using NLog;
using Nure.Configuration;
using Nure.Update;

namespace Nure
{
    class Program
    {
        private static ILogger s_Logger = LogManager.GetCurrentClassLogger();
        private const string CONFIGURATION_FILE_NAME = "nure-config.json";

        static void Main(string[] p_Args)
        {
            bool show_help = false;
            List<string> parameters = new List<string>();

            var optionSet = new OptionSet {
                { "d|directory-path=", "Absolute path to the repository to update.", value => parameters.Add(value) },
                { "g|git-api-key=", "API key to push git commits.", value => parameters.Add(value) },
                { "u|hosting-username=", "Username used with the hosting platform.", value => parameters.Add(value) },
                { "p|hosting-password=", "Password used with the hosting platform.", value => parameters.Add(value) },
                { "h|help", "Show this message", value => show_help = value != null }
            };

            try {
                optionSet.Parse(p_Args);
            } catch (OptionException e) {
                s_Logger.Error(e.Message);
                s_Logger.Info("Try `--help` for more information.");
                return;
            }

            if (show_help) {
                ShowHelp(optionSet);
                return;
            }

            if (parameters.Count != 4) {
                s_Logger.Error("Invalid number of arguments.");
                s_Logger.Info("Try `--help` for more information.");
                return;
            }

            Run(parameters[0], parameters[1], parameters[2], parameters[3]);
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
        /// <param name="p_DirectoryPath">Absolute path to the repository to update.</param>
        /// <param name="p_GitApiKey">API key to use to push commits.</param>
        /// <param name="p_HostingUsername">Username used with the hosting platform.</param>
        /// <param name="p_HostingPassword">Password used with the hosting platform.</param>
        private static void Run(string p_DirectoryPath,
            string p_GitApiKey,
            string p_HostingUsername,
            string p_HostingPassword)
        {
            TextReader json = File.OpenText(Path.Combine(p_DirectoryPath, CONFIGURATION_FILE_NAME));
            NureOptions options = JsonSerializer.CreateDefault().Deserialize<NureOptions>(new JsonTextReader(json));
            s_Logger.Info(options.ToString);
            NuKeeperWrapper nukeeper = new NuKeeperWrapper(options, p_DirectoryPath);
            nukeeper.Run();
        }
    }
}
