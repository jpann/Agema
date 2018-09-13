using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CommandLine;
using Common.Logging;

namespace BigFileHoleCmd
{
    internal class Program
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static JsonFileSettings Settings { get; set; }

        private static void Main(string[] args)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

            var version = versionInfo.ProductVersion;

            var exeinfomsg =
                $"{Path.GetFileName(Assembly.GetExecutingAssembly().Location)} v.{version} (last modified on {fileInfo.LastWriteTime})";

            Console.WriteLine(exeinfomsg);
            Log.Info(exeinfomsg);

            if (args.Length == 0)
            {
                var appSettingDirectory =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @".BigFileHoleCmd");
                var appSettingPath = Path.Combine(appSettingDirectory, @"settings.json");

                if (!Directory.Exists(appSettingDirectory))
                    Directory.CreateDirectory(appSettingDirectory);

                if (!File.Exists(appSettingPath))
                {
                    Console.WriteLine(
                        $"Error: No command line arguments.\nError: No configuration file found: {appSettingPath}.");

                    return;
                }

                Settings = new JsonFileSettings(appSettingPath, true);
            }
            else
            {
                Parser.Default.ParseArguments<CommandLineOptions>(args)
                    .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
                    .WithNotParsed(errs => HandleParseError(errs));
            }


            var path = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrEmpty(Settings.WebsiteDirectory))
                path = Settings.WebsiteDirectory;

            var server = new SimpleHttpServer(Settings.Host, path, Settings.Port,
                Settings.BufferSizeBytes);

            server.UploadDirectory = Settings.UploadDirectory;

            Console.WriteLine($"Website directory: {Settings.WebsiteDirectory}");
            Console.WriteLine($"The big file hole (upload directory): {Settings.UploadDirectory}");
            Console.WriteLine($"Buffer Size (bytes): {Settings.BufferSizeBytes}");
            Console.WriteLine($"Simple HTTP Server Running and Listening on: {server.Prefix}");

            Console.ReadKey();

            server.Stop();
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Log.Debug("Enter");
        }

        private static void RunOptionsAndReturnExitCode(CommandLineOptions opts)
        {
            Log.Debug("Enter");
        }
    }
}