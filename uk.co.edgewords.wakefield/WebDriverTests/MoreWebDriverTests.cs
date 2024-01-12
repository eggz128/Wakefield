using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uk.co.edgewords.wakefield.Utils;
using OpenQA.Selenium.Support.Extensions;
using static uk.co.edgewords.wakefield.Utils.HelperMethods;


namespace uk.co.edgewords.wakefield.WebDriverTests
{
    [TestFixture]
    internal class MoreWebDriverTests : BaseTest
    {
        //SetUp and TearDown handled by BaseTest
        
        [Test,Order(1)] //Force an execution order
        public void ATestInAnotherClass2()
        {
            driver.Url = "https://www.google.com/";
            
            Thread.Sleep(3000);
            Assert.Pass("This also works ");
        }

        [Test, Order(2), Category("Smoke")] //Force an order, and categorise/add a trait/tag the test so we can pick and choose which tests to execute together
        public void ATestInAnotherClass()
        {
            Thread.Sleep(3000);
            Assert.Pass("This works ");
        }


    }
}
