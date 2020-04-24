using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nure.PullRequest.Bitbucket.Models
{
    public class BitbucketPullRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("source")]
        public BitbucketBranchInfo SourceBranch { get; set; }

        [JsonProperty("destination")]
        public BitbucketBranchInfo DestinationBranch { get; set; }

        [JsonProperty("reviewers")]
        public List<BitbucketUser> Reviewers { get; set; }

        [JsonProperty("close_source_branch")]
        public bool CloseBranchAfterMerge { get; set; }
    }
}
