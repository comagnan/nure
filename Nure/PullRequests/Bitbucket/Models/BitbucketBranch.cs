using Newtonsoft.Json;

namespace Nure.PullRequests.Bitbucket.Models
{
    public class BitbucketBranch
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
