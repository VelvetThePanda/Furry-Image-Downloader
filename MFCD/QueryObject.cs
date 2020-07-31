using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using X10D;

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

        private int remainingPosts;
        private int currentPage;


        private string baseURL;
        private string queryURL;
        

        public QueryObject(HttpClient client, Site query, int pages, params string[] tags)
        {
            queryFrom = query;
            QueriedPages = pages;
            this.client = client;
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
        public void QueryPosts()
        {

            //Remember to set QueryDictionary["page"] to something else each iteration.//
            while(currentPage < QueriedPages)
            {
                QueryDictionary["page"] = currentPage;
                BuildURL();
                //Do something useful here//
                
                switch (remainingPosts)
                {
                    case 10:
                        log.Info("10 remaining posts for " + string.Join(',', QueryDictionary["tags"]));
                        break;
                    case 5:
                        log.Info("5 remaining posts for " + string.Join(',', QueryDictionary["tags"]));
                        break;
                    case 0:
                        log.Info("No remaining posts for " + string.Join(',', QueryDictionary["tags"]));
                        break;
                    default:
                        break;
                }
                currentPage++;
            }
            
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
                    break;
                case Site.e926:
                    baseURL = "https://e926.net/posts.json?";
                    break;
            }
        }
    }
}
