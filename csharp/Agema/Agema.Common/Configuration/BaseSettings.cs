using System;
using System.Configuration;
using System.Reflection;
using Common.Logging;

namespace Agema.Common.Configuration
{
    /// <summary>
    ///     A base settings class
    /// </summary>
    public class BaseSettings
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Gets the program data path for application.  Many classes that inherit BaseSettings will have a
        ///     SetProgramDataPath() method.
        /// </summary>
        /// <value>
        ///     The program data path.
        /// </value>
        public static string ProgramDataPath { get; set; }

        protected static T Get<T>(string key)
        {
            return Get(key, false, default(T));
        }

        protected static T Get<T>(string key, bool allowNull, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(value))
            {
                if (!allowNull)
                    throw new ConfigurationErrorsException($"Missing configuration setting \"{key}\"");

                return defaultValue;
            }

            return (T) Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        ///     Sets the connection string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected static void SetConnectionString(string key, string value)
        {
            var settings = ConfigurationManager.ConnectionStrings[key];

            if (Log.IsDebugEnabled)
                foreach (var connectionString in ConfigurationManager.ConnectionStrings)
                {
                    var connectionStringSetting = connectionString as ConnectionStringSettings;

                    if (connectionStringSetting != null)
                        Log.Debug(
                            $"{connectionStringSetting.Name}: {connectionStringSetting.ConnectionString} ({connectionStringSetting?.CurrentConfiguration?.FilePath})");
                }

            if (settings == null)
            {
                ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings(key, value));
            }
            else
            {
                var fi = typeof(ConfigurationElement).GetField("_bReadOnly",
                    BindingFlags.Instance | BindingFlags.NonPublic);

                if (fi != null)
                {
                    fi.SetValue(settings, false);

                    settings.ConnectionString = value;
                }
            }
        }
    }
}