using System;
using System.Reflection;
using BigFileHoleCmd.Properties;
using Common.Logging;

namespace BigFileHoleCmd
{
    internal class Program
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrEmpty(Settings.Default.WebsiteDirectory))
                path = Settings.Default.WebsiteDirectory;

            var server = new SimpleHttpServer(path, Settings.Default.Port,
                Settings.Default.BufferSizeBytes);

            Console.WriteLine($"Website directory: {Settings.Default.WebsiteDirectory}");
            Console.WriteLine($"The big file hole (upload directory): {Settings.Default.UploadDirectory}");
            Console.WriteLine($"Buffer Size (bytes): {Settings.Default.BufferSizeBytes}");
            Console.WriteLine($"Simple HTTP Server Running and Listening on: {server.Prefix}");

            Console.ReadKey();

            server.Stop();
        }
    }
}