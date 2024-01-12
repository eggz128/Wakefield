using LumenWorks.Framework.IO.Csv;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using uk.co.edgewords.wakefield.POMPages;
using uk.co.edgewords.wakefield.Utils;

namespace uk.co.edgewords.wakefield.WebDriverTests
{
    internal class pomtests : BaseTest
    {

        //[TestCase("edgewords","edgewords123")] //Annotation can supply values to test method args
        //[TestCase("webdriver","edgewords123xxxxx")] //Annotation can be repeated for new test cases
        //[Test] //For plain test, or test with values supplied inline with method args
        [Test, TestCaseSource(typeof(Utils.HelperMethods),"GetTestData")] //Call helper method to get test data, then feed in to test method args
        public void LoginWithValidCredsPOMTest(
            //[Values("edgewords","edgewords123")] //Use with plain [Test] for pairwise combination
            string username,
            //[Values("edgewords","edgewords123")] //Use with plain [Test] for pairwise combination
            string password)
        {
            
            driver.Url = TestContext.Parameters["baseurl"]; //Get the starting URL from a test param defined in a runsettings file
            
            Thread.Sleep(1000);
            HomePagePOM home = new HomePagePOM(driver);
            //home.loginLink.FindElement(By.XPath("..//button")).Click();
            home.GoLogin();
            //Now on login page
            LoginPagePOM loginPage = new LoginPagePOM(driver);
            //Use "low level" service methods
            //loginPage.SetUsername("edgewords");
            //loginPage.SetPassword("edgewords123");
            //loginPage.SubmitForm();
            
            //Using higher level service method
            //loginPage.Login("edgewords", "edgewords123");

            //If a method returns an instance of it's class ("this") you can chain method calls.
            //loginPage.SetUsername("edgewords")
            //    .SetPassword("edgewords123")
            //    .SubmitForm();

            //Check login success - this is the responsibility of the Test *not* the POM
            //So the POM call just returns a value. Then we check if it's what we wanted.
            bool didWeLogIn = loginPage.LoginExpectSuccess(username, password);
            //IMPORTANT REMINDER: Asserts go here - in the Test! Not in the POM!
            Assert.That(didWeLogIn, Is.True, "We did not log in - alert present"); //We logged in if pass

            Thread.Sleep(1000);
        }

        


        [Test]
        public void AttemptLoginWithInvalidCreds()
        {
            Console.WriteLine("Starting test");
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/";
            Console.WriteLine("On home page");
            Console.WriteLine();
            Thread.Sleep(1000);
            HomePagePOM home = new HomePagePOM(driver);
            home.GoLogin();
            
            LoginPagePOM loginPage = new LoginPagePOM(driver);
            Console.WriteLine("On login page");

            //ToDo: Screenshotting will be done/needed by many tests. Create some helper methods
            //ToDo: Those methods could be "smarter" - e.g. timestamp when a screenshot is taken rather than simply hardcode path and file name
            Screenshot fullPageScreenshot = driver.TakeScreenshot(); //Don't need to cast WebDrivers any more
            fullPageScreenshot.SaveAsFile(@"D:\Screenshots\Wholepage.png");

            IWebElement theForm = driver.FindElement(By.CssSelector("#Login"));
            ITakesScreenshot sselm = theForm as ITakesScreenshot; //IWebElements can now be screenshotted - but you have to cast first to get the ability
            Screenshot theelmScreenshot = sselm.GetScreenshot();
            theelmScreenshot.SaveAsFile(@"D:\screenshots\justtheform.png");

            TestContext.WriteLine("An alternative way to write out report info");
            TestContext.AddTestAttachment(@"D:\screenshots\justtheform.png", "A screenshot"); //Attach the screenshot to the TRX report.


            bool didWeLogIn = loginPage.LoginExpectFail("edgewords", "edgewords123xxxxx");



            IAlert thealert = driver.SwitchTo().Alert(); //Cant screenshot alerts - they aren't "web" content. They are JS.
            Console.WriteLine("Alert with text found:" + thealert.Text); //We can get the text for logging purposes though.

            Assert.That(didWeLogIn, Is.True, "No alert - we must have logged in"); //We logged in if pass

            Thread.Sleep(1000);
            Console.WriteLine("Finished Test");
        }
        [Test]
        public void CITest()
        {
            Assert.Pass("Just for demo purposes");
        }

    }
}
