using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MFCD
{
    //This actually queries things.
    public class TopLevelQueryManager
    {

        public async Task QueryPostsFromJSON(Search[]  searches)
        {

            var TaskList = new List<Task>();
            foreach(var search in searches)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MFCD / 1.0 (By VelvetThePanda)");
                TaskList.Add(new QueryObject(client, search.Pages, string.Join(' ', search.Tags, search.Blacklist)).QueryPosts());
            }

            Task queryTask = Task.WhenAll(TaskList);
            await queryTask;
            PostDownloadHelper.ThreadsFinished = queryTask.IsCompleted;
        }

    }
}
