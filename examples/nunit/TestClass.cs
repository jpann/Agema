using System.Reflection;
using Common.Logging;
using NUnit.Framework;

namespace CompassKey.NUnit
{
    [TestFixture]
    public class TestClass
    {
        [SetUp]
        public void DerivedSetUp()
        {
            Log.Debug("Enter");
        }

        [TearDown]
        public void DerivedTearDown()
        {
            Log.Debug("Enter");
        }

        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Log.Debug("Enter");
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            Log.Debug("Enter");
        }

        [Test]
        public void Foo1()
        {
            Log.Debug("Enter");
        }
    }
}