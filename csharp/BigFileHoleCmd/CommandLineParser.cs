using System.Reflection;
using CommandLine;
using Common.Logging;

namespace BigFileHoleCmd
{
    public class CommandLineOptions
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Option('h', "host", Required = false, HelpText = "The host name that the HTTP Server will listen on.")]
        public string Host { get; set; }

        [Option('p', "port", Required = false, Default = 80,
            HelpText = "The port that the HTTP Server will listen on.")]
        public string Port { get; set; }

        [Option('b', "buffer", Required = false, Default = 4096,
            HelpText = "The buffer size to use when reading streams.")]
        public int BufferSizeBytes { get; set; }

        [Option('U', Required = false, HelpText = "The upload directory.")]
        public string UploadDirectory { get; set; }

        [Option('W', Required = false, HelpText = "The website directory.")]
        public string WebsiteDirectory { get; set; }
    }
}