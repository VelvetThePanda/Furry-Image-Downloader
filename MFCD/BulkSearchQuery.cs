using Newtonsoft.Json;
using System.Collections.Generic;

namespace MFCD
{
    public class BulkSearchQuery
    {
        [JsonProperty]
        public List<E621Search> E621Searches { get; set; }
        [JsonProperty]
        public List<E926Search> E926Searches { get; set; }


        //push to these arraays when threads signal they need more tags.
        public E621Search[] QuerriedE621Tags { get; private set; } = new E621Search[4];
        public E926Search[] QuerriedE926Tags { get; private set; } = new E926Search[4];


        /* Only the first four searches will be querried to begin with,
         * and when a thread signals it's done, we'll push the next four into the query.
         */
        
        public void QueryNextTags()
        {
            //Get the next four e6 posts, if any.
            for (int i = 0; i < 4 || i < E621Searches.Count; i++)
            {
                if (E621Searches is null) break;
                QuerriedE621Tags[i] = E621Searches[i];
                E621Searches.RemoveAt(i);
            }

            //Get the next four e9 posts, if any.
            for (int i = 0; i < 4 || i < E926Searches.Count; i++)
            {
                if (E926Searches is null) break;
                QuerriedE926Tags[i] = E926Searches[i];
                E926Searches.RemoveAt(i);
            }
        }

    }
}
