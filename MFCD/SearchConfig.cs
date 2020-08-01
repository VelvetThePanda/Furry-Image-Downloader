using Newtonsoft.Json;
using System.Collections.Generic;

namespace MFCD
{
    public class SearchConfig
    {
        [JsonProperty]
        public List<string> E621Searches { get; set; }

        [JsonProperty]
        public List<string> E926Searches { get; set; }
    }
}
