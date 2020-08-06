using Newtonsoft.Json;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MFCD
{
    public class Program
    {

        public static event EventHandler OnClose;

        public static Logger Log { get; } = LogManager.GetCurrentClassLogger();
        public static async Task Main()
        {
            Console.CancelKeyPress += OnControlC;
            InitLogger();
            //Get Configuration from file.
            await Task.Delay(600);
            var config = await RetrieveConfigAsync();
            await Task.Delay(1500);
            GenerateDirectories(config);
            await new TopLevelQueryManager().QueryPostsFromJSON(config);



            await Task.Delay(1500);
            await PostDownloadHelper.DrainQueue();


            Console.ReadKey(true);



        }

        private static void GenerateDirectories(SearchList config)
        {
           foreach(var search in config.Queries)
           {
                var unsanitizedFoldername = string.Join(' ', search.Tags, search.Blacklist);
                var sanitizedFoldername = Regex.Replace(unsanitizedFoldername, "[\\/?:<>\"|]+", "", RegexOptions.IgnoreCase);
                if (sanitizedFoldername == " ")
                    continue;
                Directory.CreateDirectory(sanitizedFoldername.TrimEnd());
           }
        }

        private static async Task<SearchList> RetrieveConfigAsync()
        {
            if (File.Exists("Config.JSON"))
            {
                var config = await File.ReadAllTextAsync("Config.JSON");
                try 
                {
                    var hashsets = await File.ReadAllTextAsync("HashStorage.hs");
                    PostDownloadHelper.MD5Hashes = JsonConvert.DeserializeObject<ConcurrentDictionary<string, HashSet<string>>>(hashsets);
                }
                catch(FileNotFoundException fnf)
                {
                    Log.Error($"Could not find MD5 HashSet file at expected location ({fnf.FileName})");
                }

                var returnConfig = JsonConvert.DeserializeObject<SearchList>(config);
                Log.Debug("Loaded configuration successfully");
                return returnConfig;
            }
            else
            {
                Log.Error($"No config exists in directory. Please configure the one left ({Directory.GetCurrentDirectory()})");
                var save = JsonConvert.SerializeObject(new SearchList
                {
                    Queries = new List<Search> 
                    {
                        new Search { },
                        new Search { },
                        new Search { },
                        new Search { },
                    }
                },
                Formatting.Indented
                );
                await File.WriteAllTextAsync("Config.JSON", save);
                await Task.Delay(5000);
                Environment.Exit(404);
            }

            return null;
        }

        private static void OnControlC(object sender, ConsoleCancelEventArgs e)
        {
            Log.Trace("Ctrl+C / Ctrl + Break Detected!");
            //Do some thread handling here.
            OnClose(null, new EventArgs());
            PostDownloadHelper.ThreadsFinished = true;
            Environment.Exit(200);
        }

        private static void InitLogger()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "${time} [${level:uppercase=true}] \u001b[0m${message}",
                EnableAnsiOutput = true,
                UseDefaultRowHighlightingRules = false,

            };
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Trace"), ConsoleOutputColor.Green, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Info"), ConsoleOutputColor.Cyan, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Debug"), ConsoleOutputColor.Yellow, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Warn"), ConsoleOutputColor.Blue, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Error"), ConsoleOutputColor.Red, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Fatal"), ConsoleOutputColor.DarkRed, ConsoleOutputColor.Black));

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget, "*");
            LogManager.Configuration = config;
            Log.Info("Logging configured and ready.");
        }

    }
}
