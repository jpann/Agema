using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Agema.DotSpatial.Test.Helpers;
using Common.Logging;
using DotSpatial.Data;
using NUnit.Framework;
using ILog = Common.Logging.ILog;

namespace Agema.DotSpatial.Test
{
    [TestFixture]
    public class GeometryTests 
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string expectedWkt = @"POLYGON ((-96.9877624511719000 33.9011268615723000, -96.9927673339844000 33.9674911499023000, -96.9075012207031000 33.9755477905273000, -96.8511047363281000 33.9344329833984000, -96.8538818359375000 33.8833312988281000, -96.7222290039063000 33.8572158813477000, -96.7088928222656000 33.9338760375977000, -96.5958404541016000 33.9374923706055000, -96.6600036621094000 34.0519332885742000, -96.6180572509766000 34.0355529785156000, -96.5333251953125000 34.0924911499023000, -96.5205535888672000 34.0641555786133000, -96.5738983154297000 33.9994354248047000, -96.5319519042969000 33.9341583251953000, -96.5633239746094000 33.9222183227539000, -96.5794525146484000 33.8572158813477000, -96.5398559570313000 33.8212127685547000, -96.6524963378906000 33.8552703857422000, -96.7772216796875000 33.8286056518555000, -96.7833251953125000 33.7647171020508000, -96.8202819824219000 33.8108215332031000, -96.8063812255859000 33.8505477905273000, -96.8797149658203000 33.8622131347656000, -96.9097290039063000 33.9263839721680000, -96.9674987792969000 33.9299926757813000, -96.9877624511719000 33.9011268615723000))";

        /// <summary>
        /// Geometry ToWkt Test using Feature
        /// </summary>
        [Test]
        public void GeometryToWktTest()
        {
            Log.Debug("Enter");

            var testShapeFileFullPath =  TestHelper.GetTestFile(@"Shapefiles\lakes.shp");
            Assert.True(File.Exists(testShapeFileFullPath),$"Shapefile: {testShapeFileFullPath} does not exist.");

            string name = "Lake Texoma";

            Shapefile shapefile = Shapefile.OpenFile(testShapeFileFullPath);
            var feature = shapefile.SelectByAttribute($"NAME = '{name}'").First();
            Assert.IsNotNull(feature);

            var actual = feature.ToString();

            // Console.WriteLine(actual);

            Assert.AreEqual(expectedWkt, actual);
        }

        /// <summary>
        /// Geometry ToWkt Test using BasicGeometry 
        /// </summary>
        [Test]
        public void GeometryToWktTest_2()
        {
            Log.Debug("Enter");

            var testShapeFileFullPath = TestHelper.GetTestFile(@"Shapefiles\lakes.shp");
            Assert.True(File.Exists(testShapeFileFullPath), $"Shapefile: {testShapeFileFullPath} does not exist.");

            string name = "Lake Texoma";

            Shapefile shapefile = Shapefile.OpenFile(testShapeFileFullPath);
            var feature = shapefile.SelectByAttribute($"NAME = '{name}'").First();
            Assert.IsNotNull(feature);

            var actual = feature.BasicGeometry.ToString();

            // Console.WriteLine(actual);

            Assert.AreEqual(expectedWkt, actual);
        }


    }
}
