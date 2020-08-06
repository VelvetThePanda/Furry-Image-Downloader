using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MFCD
{
    public class TopLevelQueryManager
    {
        public async Task QueryPostsFromJSON(SearchList Searches)
        {
            var TaskList = new List<Task<bool>>();
            var ran = new Random();
            foreach(var search in Searches.Queries)
            {
                if (search.Tags == string.Empty) 
                    continue;
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MFCD / 1.0 (By VelvetThePanda)");
                TaskList.Add(new QueryObject(client, search.Pages, string.Join(' ', search.Tags, search.Blacklist)) {Site = search.Site }.QueryPosts());
                Program.Log.Trace("Added Task to Task List.");
                await Task.Delay(ran.Next(15, 50));
            }
            Task queryTask = Task.WhenAll(TaskList);
            await Task.Delay(500);
            Program.Log.Info("Waiting for Tasks to complete.");
            await queryTask;
            await Task.Delay(1200);
            PostDownloadHelper.ThreadsFinished = queryTask.IsCompleted;
        }
    }
}
