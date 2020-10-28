using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Nure.Configuration;

namespace Nure.Update
{
    public class NuGetWrapper
    {
        private const string NUGET_PACKAGE_HEADER = "Top-level Package";
        private const string DOTNET_PROJECT_SEPARATOR = "----------";

        private static readonly ILogger s_Logger = LogManager.GetCurrentClassLogger();
        private static readonly Regex s_ProjectNameRegex = new Regex("(?<=Project `)(.*?)(?=` has the following updates to its packages)", RegexOptions.Compiled);

        private readonly NureOptions m_NureOptions;
        private readonly string m_RepositoryDirectory;
        private readonly List<string> m_MonitoredProjects;

        public NuGetWrapper(NureOptions p_NureOptions,
                            string p_RepositoryDirectory)
        {
            m_NureOptions = p_NureOptions;
            m_RepositoryDirectory = p_RepositoryDirectory;
            m_MonitoredProjects = GetProjects();
        }

        public void Run()
        {
            Process process = GetPackageListProcess();
            process.Start();
            Dictionary<string, List<NuGetPackage>> packages = GetUpdatesFromStream(process.StandardOutput);
            s_Logger.Info($"Found {packages.Count} projects with packages to update.");
            UpdatePackages(packages);
            s_Logger.Info("Finished updating dependencies.");
        }

        private Dictionary<string, List<NuGetPackage>> GetUpdatesFromStream(StreamReader p_InputStream)
        {
            Dictionary<string, List<NuGetPackage>> packagesByProject = new Dictionary<string, List<NuGetPackage>>();

            bool isReadingPackages = false;
            string currentProject = "";
            string currentLine;

            while ((currentLine = p_InputStream.ReadLine()) != null) {
                if (string.IsNullOrWhiteSpace(currentLine)) {
                    continue;
                }

                if (currentLine.Contains(NUGET_PACKAGE_HEADER)) {
                    isReadingPackages = true;
                    continue;
                }

                Match regexMatch = s_ProjectNameRegex.Match(currentLine);

                if (regexMatch.Success) {
                    isReadingPackages = false;

                    currentProject = m_MonitoredProjects.First(project => project.Contains(regexMatch.Value, StringComparison.InvariantCultureIgnoreCase));
                    packagesByProject.Add(currentProject, new List<NuGetPackage>());
                } else if (isReadingPackages) {
                    List<string> packageComponents = currentLine.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (packageComponents.Count == 5) {
                        string packageName = packageComponents[1];
                        if (!m_NureOptions.PackagesToIgnore.Contains(packageName)) {
                            packagesByProject[currentProject].Add(new NuGetPackage(packageName, packageComponents[2], packageComponents[3], packageComponents[4]));
                        }
                    } else {
                        s_Logger.Warn($"Something went wrong with this package: {packageComponents}");
                    }
                }
            }

            return packagesByProject;
        }

        private Process GetPackageListProcess()
        {
            string processArguments = $"list {m_RepositoryDirectory} package --outdated";

            if (m_NureOptions.AllowPrereleaseDependencies) {
                processArguments += " --include-prerelease";
            }

            return new Process {
                StartInfo = GetDotnetStartInfo(processArguments)
            };
        }

        private void UpdatePackages(Dictionary<string, List<NuGetPackage>> p_PackagesByProject)
        {
            foreach (KeyValuePair<string, List<NuGetPackage>> projectPackages in p_PackagesByProject) {
                List<NuGetPackage> migratedPackages;
                List<NuGetPackage> unmigratedPackages = projectPackages.Value;
                do {
                    migratedPackages = new List<NuGetPackage>();
                    s_Logger.Info($"Updating {unmigratedPackages.Count} packages for project {projectPackages.Key}.");
                    foreach (NuGetPackage nuGetPackage in unmigratedPackages) {
                        try {
                            Process process = new Process { StartInfo = GetDotnetStartInfo($"add {Path.Combine(m_RepositoryDirectory, projectPackages.Key)} package {nuGetPackage.Name} --version {nuGetPackage.LatestVersion}") };

                            process.Start();
                            s_Logger.Info(process.StandardOutput.ReadToEnd);
                            if (process.ExitCode == 0) {
                                migratedPackages.Add(nuGetPackage);
                            }
                        } catch (Exception e) {
                            s_Logger.Warn(e, $"Exception thrown while trying to update package {nuGetPackage.Name}.");
                        }
                    }

                    unmigratedPackages = unmigratedPackages.Except(migratedPackages).ToList();
                } while (migratedPackages.Count > 0 && unmigratedPackages.Count > 0);
            }
        }

        private List<string> GetProjects()
        {
            Process process = new Process { StartInfo = GetDotnetStartInfo($"sln {m_RepositoryDirectory} list") };
            process.Start();
            return GetProjectsFromStream(process.StandardOutput);
        }

        private List<string> GetProjectsFromStream(StreamReader p_InputStream)
        {
            bool isReadingProjects = false;
            List<string> projects = new List<string>();
            string currentLine;

            while ((currentLine = p_InputStream.ReadLine()) != null) {
                if (currentLine.Contains(DOTNET_PROJECT_SEPARATOR)) {
                    isReadingProjects = true;
                } else if (isReadingProjects) {
                    projects.Add(currentLine);
                }
            }

            return projects;
        }

        private ProcessStartInfo GetDotnetStartInfo(string p_Arguments) =>
            new ProcessStartInfo {
                FileName = "dotnet",
                Arguments = p_Arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
    }
}
