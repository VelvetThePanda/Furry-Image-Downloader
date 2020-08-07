using Newtonsoft.Json;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using System;
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
        [JsonProperty(PropertyName = "Tag hashset hashes")]
        public static ConcurrentDictionary<string, HashSet<string>> MD5Hashes { get; set; } = new ConcurrentDictionary<string, HashSet<string>>();

        public static ConcurrentQueue<Download> DownloadQueue { get; } = new ConcurrentQueue<Download>();

       

        public static bool ThreadsFinished { get => threadsFinished; set => threadsFinished = value; }

        private static volatile bool threadsFinished = false;
        private static readonly HttpClient client = new HttpClient();

        private static readonly Logger log = Program.Log;

        public static async Task DrainQueue()
        {
            Program.OnClose += OnClose;
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MFCD / 1.0 (By VelvetThePanda)");
            log.Info("Queue wait loop active.");
            await Task.Delay(1000);
            log.Trace("Waiting for images to fill loop.");
            while (true)
            {
                if (threadsFinished)
                {
                    log.Trace("Threads finished querying images; preceding to download images.");
                    var numOfImages = DownloadQueue.Count;
                    foreach (var img in DownloadQueue)
                    {
                        DownloadQueue.TryDequeue(out var download);
                        if (download is null) continue;
                        var stream = await client.GetStreamAsync(download.URL);
                        var folderName = Regex.Replace(download.FolderName, "[\\/?:<>\"|]+", "", RegexOptions.IgnoreCase);
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName.TrimEnd());
                        var fileName = download.URL.Substring(download.URL.LastIndexOf('/') + 1);
                        using var writer = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create);
                        await stream.CopyToAsync(writer);
                        log.Info($"Saved {fileName}!");
                    }

                    log.Info("No images remain in queue.");
                    log.Info($"Saved {numOfImages} images to disk.");
                    

                    var hashes = JsonConvert.SerializeObject(MD5Hashes, Formatting.Indented);
                    File.WriteAllText("HashStorage.hs", hashes);
                    log.Info("Saved hashes to file; exiting");
                    break;
                }
            }
        }


        public static void OnClose(object sender, EventArgs e)
        {
            log.Trace("Close event triggered! Saving hashes and exiting.");
            var hashes = JsonConvert.SerializeObject(MD5Hashes, Formatting.Indented);
            File.WriteAllText("HashStorage.hs", hashes);
        }


        public static bool ContainsHash(string tag, string hash)
        {
            if (MD5Hashes.Keys.Contains(tag))
            {
                return MD5Hashes[tag].Contains(hash);
            }
            else
            {
                log.Error("No matching dictionary entry; generating new HashMap for tags.");
                MD5Hashes.TryAdd(tag, new HashSet<string>());
                MD5Hashes[tag].Add(hash);
                return false;
            }
        }
    }
}
