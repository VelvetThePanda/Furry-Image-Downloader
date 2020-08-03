using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;
using System.Threading.Tasks;

namespace MFCD
{
    public class Program
    {

        public static Logger Log { get; } = LogManager.GetCurrentClassLogger();
        public static async Task Main() 
        {
            InitLogger();
            

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
