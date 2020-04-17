// Copyright (c) 2005-2020, Coveo Solutions Inc.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nure.PullRequests.Bitbucket.Models
{
    public class BitbucketPagedResult<T>
    {
        [JsonProperty("pagelen")]
        public int PageLength { get; set; }

        [JsonProperty("values")]
        public List<T> Values { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }
    }
}
