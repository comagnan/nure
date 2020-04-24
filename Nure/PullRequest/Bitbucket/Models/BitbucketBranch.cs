using Newtonsoft.Json;

namespace Nure.PullRequest.Bitbucket.Models
{
    public class BitbucketBranch
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
