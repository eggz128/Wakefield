using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using NUnit.Framework.Interfaces;

namespace uk.co.edgewords.wakefield.Utils
{
    internal class BaseTest
    {
        //#pragma warning disable NUnit1032 //Make NUnit less fussy about us cleaning up WebDriver - we do clean up in [TearDown]
        //This error/warning was introduced in v3.7 of NUnit.Analyzers
        protected IWebDriver driver; //Although this displays as an error it wont actually stop the code compiling and running
        //driver *cannot* be private (i.e. only useable in *this* class). Other classes inherit from here and will need access, so use protected
        //see https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/accessibility-levels


        [SetUp] //Do stuff before the [Test] starts
        public void Setup()
        {
            ////Invoke the web browser
            //EdgeOptions options = new EdgeOptions(); //Edge specific options
            //options.AddArguments("start-maximized");
            ////driver variable is a "field" - a variable that belongs to the class and is accessible by all methods in the class
            //driver = new EdgeDriver(options); //Browser specific options an be passed at browser start time

            /*
             * Now test classes derive from this class, the browser for all tests can be changed in one place - here!
             */
            EdgeOptions options = new EdgeOptions();
            options.AddArgument("--headless");
            driver = new EdgeDriver(options);
            //Implicit wait
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            driver.Manage().Window.Maximize(); //But for maximizing this works for all browsers without browser specific options
            /*
             * This is a block
             * comment
             */
            
        }

        [TearDown] //This annotated method will run even if the test crashes half way through
        public void TearDown()
        {
            //Add a comment
            //Test Azure CI
            //Add another comment
            //Add another comment
            if(TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                //Take some screenshots
            }
            //We finished - close the web browser
            //driver.Close(); //Close current tab - if only one tab close browser
            driver.Quit();  //Quit browser (and the Driver Server that runs in the background)

            //The .Quit() method will dispose of the driver. But NUnit.Analyzers doesnt know that.
            //If not using a pragma as above, another way to solve this "error" is to add a .editorconfig file with the following:
            //# Set up additional disposal methods for a specific diagnostic rule
            //dotnet_diagnostic.NUnit1032.additional_dispose_methods = Quit
            //You can also configure this "error" to be a "warning" instead 
            //# NUnit1032: An IDisposable field/property should be Disposed in a TearDown method
            //dotnet_diagnostic.NUnit1032.severity = chosenSeverity
            //where chosenSeverity is one of none, silent, suggestion, warning, or error

            //See following links for more info:
            //https://github.com/nunit/nunit.analyzers/blob/master/documentation/NUnit1032.md#nunit1032
            //https://github.com/nunit/nunit.analyzers/issues/666

        }
    }
}
