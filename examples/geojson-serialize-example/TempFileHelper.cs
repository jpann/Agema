using System;
using System.IO;
using System.Reflection;
using Common.Logging;

namespace Agema.Common
{
    /// <summary>
    ///     Helper methods for creating Temporary files and directories
    /// </summary>
    public static class TempFileHelper
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        ///     Gets the assembly application data directory directory (e.g. C:\Users\{{user}}\AppData\Roaming\{{assemblyname}})
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyAppDataDirectory()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            var assemblyAppData = Path.Combine(appDataPath, assemblyName);

            if (!Directory.Exists(assemblyAppData))
            {
                Directory.CreateDirectory(assemblyAppData);

                Log.Info("Create directory: {assemblyAppData}");
            }

            return assemblyAppData;
        }

        public static string GetTemporaryDirectory()
        {
            var tempDir = Path.Combine(GetAssemblyAppDataDirectory(), @"tmp");

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);

                Log.Info("Create directory: {tempDir}");
            }

            return tempDir;
        }
    }
}