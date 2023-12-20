# NuRe

This repository is not maintained! Use the code within at your own risk.

## What is this repository for? ##

This repository containers the code for NuRe, a dependency updater for .NET projects using NuGet. Its goal is to keep code bases secure by updating them more quickly and more efficiently.

## Setup NuRe as a user ##

In the git repository you wish to update, create a file named `nure-config.json`. An example is provided in this repository.

The configuration parameters are as follows:
- HostingService: Select `bitbucket` or `github` here. (Default: bitbucket)
- PathToSolution: If the .net solution isn't at the root of the repository, specify its relative path here. (Default: "")
- DefaultBranch: Branch to apply updates to. (Default: default)
- NureBranchPrefix: Name to give to branches created by NuRe. (Default: nure/)
- CommitMessage: Custom message to include with commits created by NuRe. (Default: [Version bump])
- PullRequestDescription: Custom description to include with pull requests created by NuRE. (Default: none)
- AllowPrereleaseDependencies: Whether to update dependencies to prerelease versions. (Default: false)
- PackagesToIgnore: Select packages to ignore when looking for updates. (Default: []])

Then, when launching NuRe, pass the following arguments: `-d "/Path/To/Repository/To/Update" -u "hostingServiceUsername" -p "hostingServicePassword"`.

If you use the Docker image instead, share the repository with the container like so: `docker run -v /Path/To/Repository/To/Update:/Internal/Container/Path comagnan/nure:latest -d /Internal/Container/Path -u "hostingServiceUsername" -p "hostingServicePassword"`.

## Setup NuRe as a developer ##
### Requirements ###
* [.Net 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
* An IDE that supports this SDK (For example Visual Studio or JetBrains Rider)
