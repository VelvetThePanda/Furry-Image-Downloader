using NLog;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
            var ranSleep = new Random();
            Thread1Client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD / 1.0 (By VelvetTheRedPanda)");
            log.Debug("Initialized Thread 1 HTTP Client.");
            Thread.Sleep(ranSleep.Next(500, 700));
            Thread2Client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD / 1.0 (By VelvetTheRedPanda)");
            log.Debug("Initialized Thread 2 HTTP Client.");
            Thread.Sleep(ranSleep.Next(200, 500));
            Thread3Client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD / 1.0 (By VelvetTheRedPanda)");
            log.Debug("Initialized Thread 3 HTTP Client.");
            Thread.Sleep(ranSleep.Next(300, 500));
            Thread4Client.DefaultRequestHeaders.UserAgent.ParseAdd("MCFD / 1.0 (By VelvetTheRedPanda)");
            log.Debug("Initialized Thread 4 HTTP Client.");
        }


        public static async Task StartSearchQueries()
        {

            

            await Task.Delay(0);
        }

    }
    
}
