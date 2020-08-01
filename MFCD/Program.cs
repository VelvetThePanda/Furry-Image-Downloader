using MFCD.Content;
using Newtonsoft.Json;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace MFCD
{
    public class Program
    {

        public static Logger Log { get; } = LogManager.GetCurrentClassLogger();
        public static AppConfiguration Configuration { get; private set; } 
        public static void Main(string[] args)
        {
            //if (args.Length < 1)
            //    Environment.Exit(-4);

            InitLogger();
            SearchQueryManager.Init();
            if(!args.Any()) 
            {
                RetrieveAppConfiguration();
            }
            else
            {
                ParseArgs(args);
            }
            
            Log.Warn("Early termination may corrupt in-progress images. This will be fixed soon.");

            //ParseArgs(args);
            
            
          
            Console.Read();
            
        }

        private static void RetrieveAppConfiguration()
        {
            //General App Configuration
            #region AppConfig
            if (!File.Exists("Configuration.JSON"))
            {
                Log.Fatal(new FileNotFoundException(), "Application configuration not found! Created new configuration.");
                Configuration = new AppConfiguration
                {
                    SaveFolder = Directory.GetCurrentDirectory(),
                    CategorizedContentHashes = new Dictionary<string, HashSet<string>>()
                };
                var ConfigJSON = JsonConvert.SerializeObject(Configuration, Formatting.Indented, new JsonSerializerSettings 
                {
                    Formatting = Formatting.Indented,
                    
                });
                File.WriteAllText(Path.Combine(Configuration.SaveFolder, "Configuration.JSON"), ConfigJSON);
                Thread.Sleep(4000);
                Environment.Exit(1);
            }
            else
            {
                Configuration = JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText("Configuration.JSON"));
                Log.Info("Loaded app configuration");
            }
            #endregion AppConfig
            //Configuration for tags
            #region TagConfig
            if (!File.Exists("Searches.JSON"))
            {
                Log.Error(new FileNotFoundException(), "Search configuration file was not present. File generated in directory.");
                var config = new BulkSearchQuery()
                {
                    E621Searches = new List<E621Search> 
                        { 
                            new E621Search 
                                {
                                    BlackList = "This is your blacklist; no need to add (-) to anything, as it's automagically done for you.", 
                                    PageLimit = 1, 
                                    Tags = "This is the tags, seperated by spaces "
                                },
                             new E621Search
                                {
                                    BlackList = "This is your blacklist; no need to add (-) to anything, as it's automagically done for you.",
                                    PageLimit = 1,
                                    Tags = "This is the tags, seperated by spaces "
                                }
                        },
                    E926Searches = new List<E926Search>
                        {
                            new E926Search
                                {
                                    BlackList = "This is your blacklist; no need to add (-) to anything, as it's automagically done for you.",
                                    PageLimit = 1,
                                    Tags = "This is the tags, seperated by spaces"
                                },
                             new E926Search
                                {
                                    BlackList = "This is your blacklist; no need to add (-) to anything, as it's automagically done for you.",
                                    PageLimit = 1,
                                    Tags = "This is the tags, seperated by spaces"
                                }

                        }

                };

                var searchJSON = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText("Searches.JSON", searchJSON);
                Thread.Sleep(5000);
                Environment.Exit(-1);

            }
            else
            {
                InitSearches();
            }

            #endregion TagConfig
        }

        private static void InitSearches()
        {
            var searchInfoText = File.ReadAllText("Searches.JSON");
            var searchInfoJSON = JsonConvert.DeserializeObject<BulkSearchQuery>(searchInfoText);
        }

        private static void ParseArgs(string[] arguments) 
        {


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
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Info"), ConsoleOutputColor.Cyan, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Debug"), ConsoleOutputColor.Green, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Warn"), ConsoleOutputColor.Blue, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Error"), ConsoleOutputColor.Red, ConsoleOutputColor.Black));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule(ConditionParser.ParseExpression("level == LogLevel.Fatal"), ConsoleOutputColor.DarkRed, ConsoleOutputColor.Black));

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget, "*");
            LogManager.Configuration = config;
            Log.Info("Logging configured and ready.");
        }

    }
}
