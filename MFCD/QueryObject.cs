using MFCD.Booru_Site_Types;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using X10D;
using System.Text.RegularExpressions;
using MFCD.Content;

namespace MFCD
{
    public class QueryObject
    {
        public enum Site
        {
            e621,
            e926,
            //FurryBooru,
            //Rule34,
            //Gelbooru,
            //Safebooru,
            //XBooru,
            //Coming soon//
        }

        public int QueriedPages { get; private set; }
        public Dictionary<string, object> QueryDictionary { get; private set; }

        private HttpClient client;

        private Logger log = Program.Log;

        private readonly Site queryFrom;
        private Type JSONType;


        private string safeFolderName;

        private int remainingPosts;
        private int currentPage;


        private string baseURL;
        private string queryURL;
        

        public QueryObject(HttpClient client, Site query, int pages, params string[] tags)
        {
            queryFrom = query;
            QueriedPages = pages;
            this.client = client;
            var tagsString = string.Join(' ', tags);
            safeFolderName = Regex.Replace(tagsString, "[\\/?:<>\"|]", "", RegexOptions.IgnoreCase);
            SetURLFromSite();

            QueryDictionary = new Dictionary<string, object>
            {
                {"page", 1 },
                {"tags", tags }
            };
        }
        public QueryObject(HttpClient client, Site query, int pages)
        {
            queryFrom = query;
            QueriedPages = pages;
            SetURLFromSite();
        }
        /// <summary>
        /// Will query posts up to the specified amount of pages set.
        /// </summary>
        public async Task QueryPosts()
        {

            //Remember to set QueryDictionary["page"] to something else each iteration.//
            while(currentPage < QueriedPages)
            {
                QueryDictionary["page"] = currentPage;
                BuildURL();
                //Do something useful here//
                var response = await client.GetAsync(queryURL);
                
                var responseContent = response.Content;

                var responseJSON = JsonConvert.DeserializeObject<BooruJSONResponse>(await responseContent.ReadAsStringAsync());

                await DownloadPostsFromPageAsync(responseJSON);

                
                currentPage++;
            }
            
        }
        private async Task DownloadPostsFromPageAsync(BooruJSONResponse postResponse)
        {
            var downloadedPosts = 0;
            foreach(var post in postResponse.Posts)
            {
                if (ContainsHash(post.File!.Md5))
                {
                    log.Error($"Already saved post with hash of {post.File!.Md5}! Skipping.");
                    continue;
                }
                
                remainingPosts = postResponse.Posts.Count - downloadedPosts++;
                
                var imageStream = await client.GetStreamAsync(post.File!.Url);
                var imageURL = post.File.Url.ToString();
                var imageName = imageURL.Substring(imageURL.LastIndexOf('/'));
                using var sr = new FileStream(Path.Combine(Program.Configuration.SaveFolder, safeFolderName, imageName), FileMode.Create);
                
                if(remainingPosts == 0)
                {
                    log.Info("No posts remaining");
                }
            }
        }

        private bool ContainsHash(string hash)
        {
            var tagKey = (QueryDictionary["tags"] as string[])[0];
            return Program.Configuration.CategorizedContentHashes[tagKey].Contains(hash);
        }

        private string BuildURL()
        {   
            return new UriBuilder() { Host = baseURL, Query = QueryDictionary.ToGetParameters() }.ToString();
        }

        private void SetURLFromSite()
        {
            switch (queryFrom)
            {
                case Site.e621:
                    baseURL = "https://e621.net/posts.json?";
                    JSONType = typeof(BooruJSONResponse);
                    break;
                case Site.e926:
                    baseURL = "https://e926.net/posts.json?";
                    //This actually hasn't been implemented because I haven't bothered to go on e926 atm.
                    JSONType = typeof(BooruJSONResponse);
                    break;
            }
        }
    }
}
