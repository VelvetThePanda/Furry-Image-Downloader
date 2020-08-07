using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        public QueryObject(HttpClient clientRef, int pages, string tags = "")
        {
            this.pages = pages;
            Tags = tags;
            client = clientRef;
            queryDictionary.Add("tags", Tags);
            queryDictionary.Add("page", 0);
        }

        public async Task<bool> QueryPosts()
        {
            if (Site is null || Tags is null)
                return false;
            Site += "/posts.json";

            for (int i = 0; i < pages; i++)
            {
                var page = queryDictionary["page"];
                queryDictionary["page"] = int.Parse(page.ToString()) + 1;

                var queryPage = new UriBuilder
                {
                    Host = Site,
                    Query = queryDictionary.ToGetParameters(),
                };


                var url = queryPage.ToString();
                var response = await client.GetAsync(url.Remove(url.LastIndexOf('/'), 1));
                var rawResponseJSON = await response.Content.ReadAsStringAsync();

                var responseJSON = JsonConvert.DeserializeObject<BooruResponse>(rawResponseJSON);

                foreach (var post in responseJSON.Posts)
                {
                    var hash = post.File.Md5;
                    var lookupTag = Tags.Split(' ').Where(tag => tag != "").ToArray();
                    Array.Sort(lookupTag);
                    if (PostDownloadHelper.ContainsHash(lookupTag[0], hash))
                    {
                        log.Info($"File with hash {hash} already exists; skipping.");
                    }
                    else
                    {
                        if (post.File.Url is null)
                            continue;
                        PostDownloadHelper.MD5Hashes[lookupTag[0]].Add(hash);
                        PostDownloadHelper.DownloadQueue.Enqueue(new Download { FolderName = Tags, URL = post.File.Url.ToString() } );
                    }
                }
            }
            return true;
        }
    }
}
