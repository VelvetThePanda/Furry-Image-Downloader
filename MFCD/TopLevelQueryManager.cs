using NLog.Fluent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MFCD
{
    //This actually queries things.
    public class TopLevelQueryManager
    {

        public async Task QueryPostsFromJSON(SearchList Searches)
        {
            var TaskList = new List<Task<bool>>();
            foreach(var search in Searches.Queries)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MFCD / 1.0 (By VelvetThePanda)");
                TaskList.Add(new QueryObject(client, search.Pages, string.Join(' ', search.Tags, search.Blacklist)).QueryPosts());
                Program.Log.Trace("Added Task to Task List.");
            }

            Task queryTask = Task.WhenAll(TaskList);
            Program.Log.Info("Waiting for Tasks to complete.");
            await queryTask;
            PostDownloadHelper.ThreadsFinished = queryTask.IsCompleted;
            Program.Log.Info("Downloader thread waiting for query threads to finish.");
        }
    }
}
