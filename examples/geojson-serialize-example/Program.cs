using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agema.Common;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace geojson_serialize_example
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = new List<Point>
            {
                new Point(new Position(52.370725881211314, 4.889259338378906)),
                new Point(new Position(52.3711451105601, 4.895267486572266)),
                new Point(new Position(52.36931095278263, 4.892091751098633)),
                new Point(new Position(52.370725881211314, 4.889259338378906))
            };

            var multiPoint = new MultiPoint(points);


            var actualJson = JsonConvert.SerializeObject(multiPoint);
            // ReSharper disable once UseFormatSpecifierInInterpolation
            var outputFilePath = Path.Combine(TempFileHelper.GetTemporaryDirectory(),
                $"{Guid.NewGuid().ToString("N")}.json");

            
            File.WriteAllText(outputFilePath, actualJson);

            Console.WriteLine($"GeoJson Geometry Written To: {outputFilePath}");
        }
    }
}
