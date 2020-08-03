using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MFCD
{
    /// <summary>
    /// Currently just holds a list of what's downloaded, though this will be expanded upon.
    /// </summary>
    public static class PostDownloadHelper
    {
        public static ConcurrentDictionary<string, HashSet<string>> MD5Hashes { get; private set; } = new ConcurrentDictionary<string, HashSet<string>>();

        /// <summary>
        /// The list of links to download, after being checked.
        /// </summary>
        public static ConcurrentQueue<string> DownloadQueue { get; } = new ConcurrentQueue<string>();



        public static bool ContainsHash(string tag, string hash)
        {
            return MD5Hashes[tag].Contains(hash);
        }

        /*
         * Thread safe dictionary, because, threads.
         * 
         * Put in a string, get a hashmap back, check if the hashmap has the MD5, and if it doesn't, we'll download the image
         * otherwise continue on.
         */
    }
}
