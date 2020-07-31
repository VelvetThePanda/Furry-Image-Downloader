using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Site queryFrom;
        private int remainingPosts;
        private string baseURL;
        private string queryURL;
        private int currentPage;

        public QueryObject(Site query, int pages, params string[] tags)
        {
            queryFrom = query;
            QueriedPages = pages;
            SetURLFromSite();

            QueryDictionary = new Dictionary<string, object>
            {
                {"page", 1 },
                {"tags", tags }
            };
        }
        public QueryObject(Site query, int pages)
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
            queryURL = BuildURL(); //I'm probably gonna be calling this alot?//
            //Remember to set QueryDictionary["page"] to something else each iteration.//
            while(currentPage < QueriedPages)
            {
                QueryDictionary["page"] = currentPage;
                //Do something useful here//



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
