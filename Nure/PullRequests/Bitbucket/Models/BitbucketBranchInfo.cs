using Newtonsoft.Json;

namespace Nure.PullRequests.Bitbucket.Models
{
    public class BitbucketBranchInfo
    {
        [JsonProperty("branch")]
        public BitbucketBranch Branch { get; set; }
    }
}
