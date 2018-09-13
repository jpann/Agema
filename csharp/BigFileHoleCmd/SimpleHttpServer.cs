﻿// MIT License - Copyright (c) 2016 Can Güney Aksakalli
// https://aksakalli.github.io/2014/02/24/simple-http-server-with-csparp.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using Agema.Common.Diagnostics;
using Common.Logging;
using ILog = Common.Logging.ILog;

namespace BigFileHoleCmd
{

    public class SimpleHttpServer
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Prefix { get;  }

        private readonly string[] _indexFiles =
        {
            "index.html",
            "index.htm",
            "default.html",
            "default.htm"
        };

        private static IDictionary<string, string> _mimeTypeMappings =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                #region extension to MIME type list

                {".asf", "video/x-ms-asf"},
                {".asx", "video/x-ms-asf"},
                {".avi", "video/x-msvideo"},
                {".bin", "application/octet-stream"},
                {".cco", "application/x-cocoa"},
                {".crt", "application/x-x509-ca-cert"},
                {".css", "text/css"},
                {".deb", "application/octet-stream"},
                {".der", "application/x-x509-ca-cert"},
                {".dll", "application/octet-stream"},
                {".dmg", "application/octet-stream"},
                {".ear", "application/java-archive"},
                {".eot", "application/octet-stream"},
                {".exe", "application/octet-stream"},
                {".flv", "video/x-flv"},
                {".gif", "image/gif"},
                {".hqx", "application/mac-binhex40"},
                {".htc", "text/x-component"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".ico", "image/x-icon"},
                {".img", "application/octet-stream"},
                {".iso", "application/octet-stream"},
                {".jar", "application/java-archive"},
                {".jardiff", "application/x-java-archive-diff"},
                {".jng", "image/x-jng"},
                {".jnlp", "application/x-java-jnlp-file"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".js", "application/x-javascript"},
                {".mml", "text/mathml"},
                {".mng", "video/x-mng"},
                {".mov", "video/quicktime"},
                {".mp3", "audio/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpg", "video/mpeg"},
                {".msi", "application/octet-stream"},
                {".msm", "application/octet-stream"},
                {".msp", "application/octet-stream"},
                {".pdb", "application/x-pilot"},
                {".pdf", "application/pdf"},
                {".pem", "application/x-x509-ca-cert"},
                {".pl", "application/x-perl"},
                {".pm", "application/x-perl"},
                {".png", "image/png"},
                {".prc", "application/x-pilot"},
                {".ra", "audio/x-realaudio"},
                {".rar", "application/x-rar-compressed"},
                {".rpm", "application/x-redhat-package-manager"},
                {".rss", "text/xml"},
                {".run", "application/x-makeself"},
                {".sea", "application/x-sea"},
                {".shtml", "text/html"},
                {".sit", "application/x-stuffit"},
                {".swf", "application/x-shockwave-flash"},
                {".tcl", "application/x-tcl"},
                {".tk", "application/x-tcl"},
                {".txt", "text/plain"},
                {".war", "application/java-archive"},
                {".wbmp", "image/vnd.wap.wbmp"},
                {".wmv", "video/x-ms-wmv"},
                {".xml", "text/xml"},
                {".xpi", "application/x-xpinstall"},
                {".zip", "application/zip"},

                #endregion
            };

        private Thread _serverThread;
        private string _rootDirectory;
        private HttpListener _listener;
        private int _port;

        public int Port
        {
            get { return _port; }
            private set { }
        }
        /// <summary>
        /// Size of the buffer in bytes.
        /// </summary>
        private int BufferSizeBytes { get; }

        /// <summary>
        /// Construct server with given port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        /// <param name="port">Port of the server.</param>
        /// /// <param name="bufferSizeBytes">Size of the buffer to use when reading streams.</param>
        public SimpleHttpServer(string path, int port, int bufferSizeBytes)
        {
            Prefix= $"http://{Properties.Settings.Default.Host}:{port}/";

            BufferSizeBytes = bufferSizeBytes;

            this.Initialize(path, port);
        }

        /// <summary>
        /// Construct server with suitable port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        public SimpleHttpServer(string path)
        {

            Prefix= $"http://{Properties.Settings.Default.Host}:{_port}/";

            //get an empty port
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint) l.LocalEndpoint).Port;
            l.Stop();
            this.Initialize(path, port);
        }

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            _serverThread.Abort();
            _listener.Stop();
        }

        private void Listen()
        {
            _listener = new HttpListener();
            
            try
            {
                _listener.Prefixes.Add(Prefix);
                _listener.Start();

                while (true)
                {
                    try
                    {
                        HttpListenerContext context = _listener.GetContext();
                        Process(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (HttpListenerException httpListenerException)
            {
                Log.Error(httpListenerException);
                Console.Write(httpListenerException.Message);

                if (httpListenerException.Message.Equals("Access is Denied",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    var errmsg = $"Could not add prefix: {Prefix} to listener. Perhaps: netsh http add urlacl url={Prefix} user=<username>";
                    Console.WriteLine(errmsg);
                    Log.Error(errmsg);
                }
                // netsh http add urlacl url=http://*:8087/ user=<username>

                // http://*:8087/   
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex);

                throw;
            }
        }

        private void Process(HttpListenerContext context)
        {
            string filename = context.Request.Url.AbsolutePath;
            Console.WriteLine($"HTTP POST {context.Request.HttpMethod} FILE {filename}");

            if (context.Request.HttpMethod.Equals("POST",StringComparison.InvariantCultureIgnoreCase))
            {
                ProcessPost(context);
                return;
            }
            
            filename = filename.Substring(1);

            if (string.IsNullOrEmpty(filename))
            {
                foreach (string indexFile in _indexFiles)
                {
                    if (File.Exists(Path.Combine(_rootDirectory, indexFile)))
                    {
                        filename = indexFile;
                        break;
                    }
                }
            }

            filename = Path.Combine(_rootDirectory, filename);

            if (File.Exists(filename))
            {
                try
                {
                    Stream input = new FileStream(filename, FileMode.Open);

                    //Adding permanent http response headers
                    string mime;
                    context.Response.ContentType = _mimeTypeMappings.TryGetValue(Path.GetExtension(filename), out mime)
                        ? mime
                        : "application/octet-stream";
                    context.Response.ContentLength64 = input.Length;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    context.Response.AddHeader("Last-Modified",
                        System.IO.File.GetLastWriteTime(filename).ToString("r"));

                    byte[] buffer = new byte[1024 * 16];
                    int nbytes;
                    while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                        context.Response.OutputStream.Write(buffer, 0, nbytes);
                    input.Close();

                    context.Response.StatusCode = (int) HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }

            }
            else
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }

        private void ProcessPost(HttpListenerContext context)
        {
            Log.Debug("Enter");

            try
            {
                var stopwatch = Stopwatch.StartNew();

                string filename = context.Request.Url.LocalPath.Replace("/",string.Empty);

                var fullpath = Path.Combine(Properties.Settings.Default.UploadDirectory, filename);
            
                ReadStream(context, fullpath );
                stopwatch.Stop();
            
                Console.WriteLine($"File written to: {fullpath} in {stopwatch.ElapsedTime()}.");

                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
                context.Response.OutputStream.Close();
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);

                Console.WriteLine(ex.Message);

                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.OutputStream.Flush();
                context.Response.OutputStream.Close();
            }

        }

        public void ReadStream(HttpListenerContext context, string filePath)
        {
            const int chunkSize = 1024; // read the file by chunks of 1KB
            int bytesRead;
            var buffer = new byte[chunkSize];

            string currentRatioString = string.Empty;

            double lastRatio = 0;

            var totalSize = context.Request.ContentLength64;
            long currentSize = 0;

            using (var reader = new BinaryReader(context.Request.InputStream))
            using (var filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, true))
            using (var writer = new BinaryWriter(filestream))
            {
                while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    currentSize += bytesRead;

                    var ratio = (currentSize / (double) totalSize) * 100;

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (lastRatio != Math.Round(ratio,1, MidpointRounding.AwayFromZero))
                    {
                        lastRatio = Math.Round(ratio,1, MidpointRounding.AwayFromZero);
                        Console.WriteLine($"{lastRatio}% complete. {currentSize} bytes read of {totalSize} bytes.");
                    }

                    // TODO: Process bytesRead number of bytes from the buffer
                    // not the entire buffer as the size of the buffer is 1KB
                    // whereas the actual number of bytes that are read are 
                    // stored in the bytesRead integer.

                    writer.Write(buffer);
                }
                writer.Close();
            }
        }

        private void Initialize(string path, int port)
        {
            this._rootDirectory = path;
            this._port = port;
            _serverThread = new Thread(this.Listen);
            _serverThread.Start();
        }
    }
}