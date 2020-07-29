using Newtonsoft.Json;
using System.Collections.Generic;

namespace MFCD
{
    public class AppConfiguration
    {
        [JsonProperty]
        public bool FirstUse { get; set; }
        [JsonProperty]
        public string SaveFolder { get; set; }
        [JsonProperty]
        public Dictionary<string, HashSet<int>> CategorizedContentHashes { get; set; }

        public const int MAX_THREADS = 4;
        public const int MAX_QUERIES_PER_THREAD = 4;
        

        public bool TryAddHash(string tag, int hash)
        {
            if (CategorizedContentHashes.ContainsKey(tag))
            {
                var hashList = CategorizedContentHashes[tag];
                if (hashList.Contains(hash))
                {
                    return false;
                }
                else
                {
                    hashList.Add(hash);
                    return true;
                }
            }
            else
            {
                CategorizedContentHashes.Add(tag, new HashSet<int>() { hash });
                return true;
            }
        }
        public AppConfiguration() 
        {
            
        }



    }
}
