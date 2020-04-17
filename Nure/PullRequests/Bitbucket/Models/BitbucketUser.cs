using Newtonsoft.Json;

namespace Nure.PullRequests.Bitbucket.Models
{
    public class BitbucketUser
    {
        [JsonProperty("uuid")]
        public string UserId { get; set; }
    }
}
