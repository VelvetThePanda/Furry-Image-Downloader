using Newtonsoft.Json;
using System;

namespace MFCD
{

    public sealed class Search
    {

        [JsonProperty]
        public string Site { get; set; } = "";
        [JsonProperty]
        public string Tags { get; set; } = "";
        [JsonProperty]
        public string Blacklist { get; set; } = "";
        [JsonProperty(PropertyName = "Page limit (Hard Limit of 750)")]
        public int Pages { get; set; } = 1;

    }
}
