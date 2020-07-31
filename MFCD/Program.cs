using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;
using System.IO;
using System.Reflection;

namespace MFCD
{
    public class Program
    {

        public static Logger Log { get; } = LogManager.GetCurrentClassLogger();
        public static AppConfiguration Configuration { get; private set; } 
        public static void Main(string[] args)
        {
            if (args.Length < 1)
                Environment.Exit(-4);
            InitLogger();
            RetrieveAppConfiguration();
            //ParseArgs(args);
            
          
            Console.Read();
            
        }

        private static void RetrieveAppConfiguration()
        {
            
            if (!File.Exists("Configuration.JSON"))
            {
               
                Configuration = new AppConfiguration
                {
                    FirstUse = true,
                    SaveFolder = Directory.GetCurrentDirectory()
                };
                var ConfigJSON = JsonConvert.SerializeObject(Configuration);
                File.WriteAllText(Path.Combine(Configuration.SaveFolder, "Config.JSON"), ConfigJSON);
                Environment.Exit(1);
            }
            else
            {
                Configuration = JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText("./Configuration.JSON"));
            }
        }

        private static void ParseArgs(string[] arguments) 
        {
            throw new NotImplementedException();
        }

        private static void InitLogger()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "${longdate} [${level:uppercase=true}] \u001b[0m${message}",
                EnableAnsiOutput = true
            };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget, "*");
            LogManager.Configuration = config;
        }

    }
}
