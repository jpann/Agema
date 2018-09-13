using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BigFileHoleCmd
{
    public class JsonFileSettings
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public JsonFileSettings(string filePath, bool watch = false)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Could not find file: {filePath}");

            JsonFilePath = filePath;

            if (watch)
            {
                var watcher = new FileSystemWatcher();
                watcher.Path = Path.GetDirectoryName(filePath);
                watcher.Filter = Path.GetFileName(filePath);
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += OnFileWatcherChanged;
                watcher.EnableRaisingEvents = true;
            }

            ReadSettingsFile();
        }

        public string WebsiteDirectory { get; private set; }
        public string UploadDirectory { get; private set; }
        public int BufferSizeBytes { get; private set; }
        public int Port { get; private set; }
        public string Host { get; private set; }

        private string JsonFilePath { get; }

        private JObject SettingsJsonObject { get; set; }

        public int JsonTextHashCode { get; set; }

        private void OnFileWatcherChanged(object sender, FileSystemEventArgs e)
        {
            ReadSettingsFile();
        }

        private void ReadSettingsFile()
        {
            Log.Debug("Enter");

            Thread.Sleep(300);

            var text = string.Empty;
            using (var fs = new FileStream(JsonFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                text = sr.ReadToEnd();
                sr.Close();
            }

            if (!string.IsNullOrEmpty(text)
                && text.GetHashCode() != JsonTextHashCode)
            {
                SettingsJsonObject = JObject.Parse(text);

                WebsiteDirectory = SettingsJsonObject.Value<string>("websiteDirectory");
                UploadDirectory = SettingsJsonObject.Value<string>("uploadDirectory");
                Host = SettingsJsonObject.Value<string>("host");
                Port = SettingsJsonObject.Value<int>("port");
                BufferSizeBytes = SettingsJsonObject.Value<int>("buffer");

                JsonTextHashCode = text.GetHashCode();

                Console.WriteLine(
                    $"Settings set to:\n{JsonConvert.SerializeObject(SettingsJsonObject, Formatting.Indented)}");
            }
        }
    }
}