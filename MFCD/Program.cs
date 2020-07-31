using MFCD.Content;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
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
            RetrieveAppConfiguration();
            Log.Warn("Early termination may corrupt in-progress images. This will be fixed soon.");

            //ParseArgs(args);

          
            Console.Read();
            
        }

        private static void RetrieveAppConfiguration()
        {
            
            if (!File.Exists("Configuration.JSON"))
            {
                Log.Error(new FileNotFoundException(), "Config JSON not found! Config made in this folder");
                Configuration = new AppConfiguration
                {
                    FirstUse = true,
                    SaveFolder = Directory.GetCurrentDirectory()
                };
                var ConfigJSON = JsonConvert.SerializeObject(Configuration);
                File.WriteAllText(Path.Combine(Configuration.SaveFolder, "Configuration.JSON"), ConfigJSON);
                Thread.Sleep(4000);
                Environment.Exit(1);
            }
            else
            {
                Configuration = JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText("./Configuration.JSON"));
                Log.Info("Loaded app configuration");
            }
        }

        private static void ParseArgs(string[] arguments) 
        {
            //if (args.Length < 1)
            //{
            //    Environment.Exit(-4);
            //}

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
