// Copyright (c) 2005-2020, Coveo Solutions Inc.

using System.Diagnostics;
using System.Runtime.InteropServices;
using NLog;
using Nure.Configuration;

namespace Nure.Update
{
    public class NuKeeperWrapper
    {
        private static ILogger s_Logger = LogManager.GetCurrentClassLogger();
        private const string NUKEEPER_INSTALLATION_COMMAND = "dotnet tool install nukeeper --global";

        private readonly NureOptions m_NureOptions;
        private readonly string m_RepositoryDirectory;

        public NuKeeperWrapper(NureOptions p_NureOptions,
            string p_RepositoryDirectory)
        {
            m_NureOptions = p_NureOptions;
            m_RepositoryDirectory = p_RepositoryDirectory;
        }

        public void Run()
        {
            ExecuteInShell(NUKEEPER_INSTALLATION_COMMAND);
            ExecuteInShell(GetNuKeeperUpdateCommand());
        }

        private string GetNuKeeperUpdateCommand()
        {
            string usePrerelease = m_NureOptions.AllowPrereleaseDependencies ? "Always" : "Never";

            return $"nukeeper update {m_RepositoryDirectory} --useprerelease {usePrerelease} -a 0";
        }

        private void ExecuteInShell(string p_Command)
        {
            s_Logger.Info($"About to execute \"{EscapeCommand(p_Command)}\"");

            var process = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = GetShell(),
                    Arguments = $"{GetCommandParameter()} \"{EscapeCommand(p_Command)}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            s_Logger.Info($"Started process {process.Id}");
            string output = process.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(output)) {
                s_Logger.Info(output);
            }

            process.WaitForExit();
        }

        private string GetShell()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return "cmd.exe";
            }

            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "/bin/zsh" : "/bin/bash";
        }

        private string EscapeCommand(string p_String)
        {
            return p_String.Replace("\"", "\\\"").Replace("/", "//");
        }

        private string GetCommandParameter()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "/C";

            return "-c";
        }
    }
}
