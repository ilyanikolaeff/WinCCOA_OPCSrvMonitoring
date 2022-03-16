using Newtonsoft.Json;
using NLog;
using System.IO;

namespace WinCCOA_OPCServerMonitoring
{
    class Settings
    {
        private static Settings _instance;
        public static Settings GetSettings()
        {
            if (_instance == null)
                _instance = Deserialize();
            return _instance;
        }

        public string CmdLine;

        public string OpcServerAddress;
        public string OpcServerName;
        public string OpcSignalName;

        public string DataPointName;
        public object DataPointValueWhenFailed;
        public object DataPointValueWhenSuccess;
        public int Delay;

        public static Settings Deserialize()
        {
            var jsonSerializer = new JsonSerializer();

            using (var reader = new StreamReader("Settings.json"))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var settings = jsonSerializer.Deserialize<Settings>(jsonReader);
                    settings.PrintSettings();
                    return settings;
                }
            }
        }

        private void PrintSettings()
        {
            var logger = LogManager.GetCurrentClassLogger();
            var settingsString = "\n";
            settingsString += $"CMD_LINE = {CmdLine}\n";
            settingsString += $"OpcServerAddress = {OpcServerAddress}\n";
            settingsString += $"OpcServerName = {OpcServerName}\n";
            settingsString += $"OpcSignalName = {OpcSignalName}\n";
            settingsString += $"DataPointName = {DataPointName}\n";
            settingsString += $"DataPointValueWhenFailed = {DataPointValueWhenFailed}\n";
            settingsString += $"DataPointValueWhenSuccess = {DataPointValueWhenSuccess}\n";
            settingsString += $"Delay = {Delay}";
            logger.Info(settingsString);
        }

        public static void Serialize(Settings settings)
        {
            var jsonSerializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            using (var writer = new StreamWriter("Settings.json"))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonSerializer.Serialize(jsonWriter, settings);
                }
            }
        }
    }
}
