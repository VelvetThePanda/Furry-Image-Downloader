using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace MFCD
{
    public class Program
    {
        public static AppConfiguration Configuration { get; private set; } 
        public static void Main(string[] args)
        {
            RetrieveAppConfiguration();
            ParseArgs(args);

            
        }

        private static void RetrieveAppConfiguration()
        {
            
            if (!File.Exists("./Configuration.JSON"))
            {
                Configuration = new AppConfiguration
                {
                    FirstUse = true,
                    SaveFolder = Assembly.GetExecutingAssembly().Location
                };
                var ConfigJSON = JsonConvert.SerializeObject(Configuration);
                File.WriteAllText(Configuration.SaveFolder, ConfigJSON);
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
    }
}
