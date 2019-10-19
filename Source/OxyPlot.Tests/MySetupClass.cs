using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OxyPlot.Tests
{
    [SetUpFixture]
    public class MySetupClass
    {
        /// <summary>
        /// Workaround for breaking changes while moving to latest NUnit, <see cref="https://github.com/nunit/docs/wiki/Breaking-Changes"/>
        /// </summary>
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            // or identically under the hoods
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
        }
    }
}
