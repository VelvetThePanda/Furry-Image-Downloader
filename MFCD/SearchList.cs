using Newtonsoft.Json;
using System.Collections.Generic;

namespace MFCD
{
    public class SearchList
    {
        [JsonProperty(PropertyName = "Image Searches", Order = 2)]
        public List<Search> Queries { get; set; } = new List<Search>();


        [JsonProperty(PropertyName = "Valid Websites", Order = 1)]
        public string[] SupportedSites { get; } = new string[]
        {
            "e621.net",
            "e926.net"
        };
            
    }
}
