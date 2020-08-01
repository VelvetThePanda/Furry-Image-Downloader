using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MFCD.Content
{
    public static class SearchQueryManager
    {
        private static readonly List<HttpClient> httpClients = new List<HttpClient>
        {
            new HttpClient(), new HttpClient(),
            new HttpClient(), new HttpClient(),
        };

        private static readonly Logger log = Program.Log;

        public static void Init()
        {
           foreach(var client in httpClients)
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD Project / 1.0 (By VelvetThePanda)");
            
        }


        public static async Task StartSearchQueries(BulkSearchQuery searchQuery)
        {
            var searchE621 = true;
            var searchE926 = false;
            log.Info("Querying inital search");
            if (!searchQuery.E621Searches.Any())
            {
                log.Info("e621.NET Search Queries empty; skipping");
                searchE621 = false;
            }

            if (!searchQuery.E621Searches.Any())
            {
                log.Info("e926.NET Search Queries empty; skipping");
                searchE926 = false;
            }
            
            for(int i = 0; i < 4; i++)
            {

            }

            if (searchE621)
            {
            }

            await Task.Delay(0);
        }

    }
    
}
