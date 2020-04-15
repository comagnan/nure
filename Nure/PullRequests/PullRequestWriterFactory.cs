using Nure.Configuration;

namespace Nure.PullRequests
{
    public class PullRequestWriterFactory : IPullRequestWriterFactory
    {
        private const string BITBUCKET_HOSTING_SERVICE = "bitbucket";
        private const string GITHUB_HOSTING_SERVICE = "github";

        private readonly NureOptions m_NureOptions;
        private readonly string m_HostingUsername;
        private readonly string m_HostingPassword;

        public PullRequestWriterFactory(NureOptions p_NureOptions,
            string p_HostingUsername,
            string p_HostingPassword)
        {
            m_NureOptions = p_NureOptions;
            m_HostingUsername = p_HostingUsername;
            m_HostingPassword = p_HostingPassword;
        }

        public IPullRequestWriter Create()
        {
            if (m_NureOptions.HostingService.ToLowerInvariant() == BITBUCKET_HOSTING_SERVICE) {
                return new BitbucketPullRequestWriter(m_NureOptions, m_HostingUsername, m_HostingPassword);
            }

            if (m_NureOptions.HostingService.ToLowerInvariant() == GITHUB_HOSTING_SERVICE) {
                return new GitHubPullRequestWriter(m_NureOptions, m_HostingPassword);
            }

            throw new System.NotImplementedException($"Hosting service {m_NureOptions.HostingService} is not supported by NuRe.");
        }
    }
}
