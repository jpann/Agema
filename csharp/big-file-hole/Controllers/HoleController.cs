using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Common.Logging;
using Newtonsoft.Json.Linq;

namespace big_file_hole.Controllers
{
    public class HoleController : ApiController
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // GET: api/Hole
        [HttpGet]
        public JObject Get()
        {
            JObject jo = new JObject();

            return jo;
        }

    

        // POST: api/Hole
        [HttpPost]
        [Route("api/hole/{fileName}")]
        public void UploadFile(string fileName){
        
            Log.Debug("Enter");

            if(string.IsNullOrEmpty(fileName))
                throw new ApplicationException($"FileName is required.");

            var filepath = GetUploadDirectory();

            var fullPath = Path.Combine(filepath, "tmp.file");
            
            ReadStream(HttpContext.Current, fullPath);
        }

        private void ReadStream(HttpContext context, string filePath)
        {
            using (var reader = new StreamReader(context.Request.GetBufferlessInputStream(true)))
            using (var filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, true))
            using (var writer = new StreamWriter(filestream))
            {
                var readBuffer = reader.ReadToEnd();
                writer.WriteAsync(readBuffer);
            }
        }

        // PUT: api/Hole/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Hole/5
        public void Delete(int id)
        {
        }


        private string GetUploadDirectory()
        {
            var value = ConfigurationManager.AppSettings["UploadDirectoryPath"];

            if (string.IsNullOrEmpty(value ))
                throw new ApplicationException($"No Upload Directory Configured.");



            return value;
        }
    }
}
