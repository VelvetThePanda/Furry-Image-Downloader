using NLog;
using System.Net.Http;

namespace MFCD.Content
{
    public static class SearchQueryManager
    {
        public static HttpClient Thread1Client { get; } = new HttpClient();
        public static HttpClient Thread2Client { get; } = new HttpClient();
        public static HttpClient Thread3Client { get; } = new HttpClient();
        public static HttpClient Thread4Client { get; } = new HttpClient();

        private static readonly Logger log = Program.Log;

        public static void Init()
        {
            Thread1Client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD / 1.0 (By VelvetTheRedPanda)");
            log.Debug("Initialized Thread 1 HTTP Client.");
            Thread2Client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD / 1.0 (By VelvetTheRedPanda)");
            log.Debug("Initialized Thread 2 HTTP Client.");
            Thread3Client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD / 1.0 (By VelvetTheRedPanda)");
            log.Debug("Initialized Thread 3 HTTP Client.");
            Thread4Client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD / 1.0 (By VelvetTheRedPanda)");
            log.Debug("Initialized Thread 4 HTTP Client.");
        }


    }
    
}
