using System;

namespace Nure.PullRequests.Bitbucket
{
    public class BitbucketRepository
    {
        public BitbucketRepository(string p_FullPath)
        {
            Uri bitbucketUri = new Uri(p_FullPath);
            BaseUrl = $"https://api.{bitbucketUri.Authority}";
            WorkspaceId = bitbucketUri.Segments[1].TrimEnd('/');
            RepositoryId = bitbucketUri.Segments[2].TrimEnd('/');
        }

        public string BaseUrl { get; set; }
        public string WorkspaceId { get; set; }
        public string RepositoryId { get; set; }
    }
}