using NLog.Fluent;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MFCD
{
    /// <summary>
    /// Currently just holds a list of what's downloaded, though this will be expanded upon.
    /// </summary>
    public static class PostDownloadHelper
    {
        public static ConcurrentDictionary<string, HashSet<string>> MD5Hashes { get; } = new ConcurrentDictionary<string, HashSet<string>>();

        public static ConcurrentQueue<Download> DownloadQueue { get; } = new ConcurrentQueue<Download>();
        
        public static bool ThreadsFinished { get => threadsFinished; set => threadsFinished = value; }

        private static volatile bool threadsFinished;
        private static readonly HttpClient client = new HttpClient();

        public static async Task DrainQueue()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MFCD / 1.0 (By VelvetThePanda)");
            Program.Log.Trace("Entering queue wait loop");
            await Task.Delay(1000);
            Program.Log.Debug("Waiting for images to fill loop.");
            while (true)
            {
                if (!threadsFinished) 
                {
                    if(DownloadQueue.Count < 10)
                        await Task.Delay(100);
                    else
                    {
                        for(var i = 0; i < 10; i++)
                        {
                            DownloadQueue.TryDequeue(out var download);
                            if (download is null) continue;
                            var stream = await client.GetStreamAsync(download.URL);
                            var folderName = Regex.Replace(download.FolderName, "[\\/?:<>\"|]", "", RegexOptions.IgnoreCase);
                            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                            var fileName = download.URL.Substring(download.URL.LastIndexOf('/') + 1);
                            using var writer = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create);
                            await stream.CopyToAsync(writer);
                        }
                    }
                }
                else
                {
                    Log.Info("Finishing remaining images in queue.");
                    foreach (var post in DownloadQueue)
                    {
                        var stream = await client.GetStreamAsync(post.URL);
                        var folderName = Regex.Replace(post.FolderName, "[\\/?:<>\"|]", "", RegexOptions.IgnoreCase);
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        var fileName = post.URL.Substring(post.URL.LastIndexOf('/') + 1);

                        using var writer = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create);
                        await stream.CopyToAsync(writer);
                    }

                    Log.Trace("Threads joined.");
                    Log.Info("No images remain in queue.");
                    break;
                } 
            }       
        }


        public static bool ContainsHash(string tag, string hash)
        {
            if (MD5Hashes.Keys.Contains(tag))
            {
                return MD5Hashes[tag].Contains(hash);
            }
            else
            {
                Log.Error("No matching dictionary entry; generating new HashMap for tags.");
                MD5Hashes.TryAdd(tag, new HashSet<string>());
                MD5Hashes[tag].Add(hash);
                return true;
            }
        }
    }
}
