using Newtonsoft.Json;

namespace Nure.PullRequest.Bitbucket.Models
{
    public class BitbucketBranchInfo
    {
        [JsonProperty("branch")]
        public BitbucketBranch Branch { get; set; }
    }
}
