using Newtonsoft.Json;

namespace Nure.PullRequest.Bitbucket.Models
{
    public class BitbucketUser
    {
        [JsonProperty("uuid")]
        public string UserId { get; set; }
    }
}
