using Newtonsoft.Json;

namespace MFCD
{
    public abstract class Search
    {
        [JsonProperty]
        public string Site { get; protected set; }
        [JsonProperty]
        public string Tags { get; set; }
        [JsonProperty]
        public string Blacklist { get; set; }
        [JsonProperty]
        public int Pages { get; set; }

    }
}
