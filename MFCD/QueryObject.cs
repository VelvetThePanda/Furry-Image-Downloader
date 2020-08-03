using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using X10D;

namespace MFCD
{
    public sealed class QueryObject
    {

        public string Site { get; set; }
        public string Tags { get; set; }


        private readonly int pages;

        private readonly HttpClient client;

        private readonly Logger log = Program.Log;

        private readonly Dictionary<string, object> queryDictionary = new Dictionary<string, object>();

        /*
         * Maybe Six helper threads that retreive from a queue in no particular order,
         * So this wouldn't have the concurrent queue, but some other object.
         * 
         * 
         */

        public QueryObject(HttpClient clientRef, int pages, string tags = null)
        {
            this.pages = pages;
            Tags = tags;
            client = clientRef;
            queryDictionary.Add("tags", Tags);
            queryDictionary.Add("page", 1);
        }

        public async Task QueryPosts()
        {
            if (Site is null || Tags is null)
                return;


            for(int i = 0; i < pages; i++)
            {
                var page = queryDictionary["page"];
                queryDictionary["page"] = int.Parse(page.ToString()) + 1;

                var queryPage = new UriBuilder
                {
                    Host = Site,
                    Query = queryDictionary.ToGetParameters(),
                };


                var response = await client.GetAsync(queryPage.ToString());
                var rawResponseJSON = await response.Content.ReadAsStringAsync();
                var responseJSON = JsonConvert.DeserializeObject<BooruResponse>(rawResponseJSON);

                foreach (var post in responseJSON.Posts)
                {
                    var hash = post.File.Md5;
                    if (PostDownloadHelper.ContainsHash(Tags, hash))
                    {
                        log.Info($"File with hash {hash} already exists; skipping.");
                    }
                    else
                    {
                        PostDownloadHelper.MD5Hashes[Tags].Add(hash);
                        PostDownloadHelper.DownloadQueue.Enqueue(post.File.Url.ToString());
                    }
                }
            }    
        }
    }
}
