using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using NUnit.Framework;
 
namespace CompassKey.Nunit
{
    /// <summary>
    /// A base test class
    /// </summary>
    public class BaseTest
    {
        /// <summary>
        ///     The Log (Common.Logging)
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This code runs once no matter how many tests are in this class.
        /// </summary>
        [OneTimeSetUp]
        public void FixtureSetUp()
        {            
        }
		
		
		
    }
}
