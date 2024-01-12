using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uk.co.edgewords.wakefield.Utils; //Make BaseTest.cs available to this class
using static uk.co.edgewords.wakefield.Utils.HelperMethods;

namespace uk.co.edgewords.wakefield.WebDriverTests
{
    internal class HelloWebDriver : BaseTest //HelloWebDriver derives from (inherits from/copies in) Utils/BaseTest.cs
    {
        
        [Test, Category("Smoke")]
        public void LogoutTest()
        {
            /*
             * Arrange - Get the app in to a state to test "the thing"
             */
            //Assuming web browser is open (it should be via [SetUp] in BaseClass)
            SayHi();

            //Set (open) the first page
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2";
            //Find and click login link
            driver.FindElement(By.LinkText("Login To Restricted Area")).Click();
            //Wait for new page
            //WebDriverWait initialWait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            //initialWait.Until(drv => drv.FindElement(By.LinkText("Submit")));

            //Using static helper
            //WaitForLocator(driver, 1, By.LinkText("Submit"));
            
            //Using instance helper
            InstanceHelperMethods myHelpers = new InstanceHelperMethods(driver);
            myHelpers.WaitForLocator(1, By.LinkText("Submit"));

            //Should be on next page - lets capture the heading and report to check
            string headingText = driver.FindElement(By.CssSelector("#right-column > h1:nth-child(1)")).Text;
            Console.WriteLine("The page heading is:" + headingText);

            try
            {
                Assert.That(headingText, Is.EqualTo("Access and authentication"), "Not on login page?");
            } //This assertion will fail - but the fail gets caught
            catch (AssertionException ex)
            {
                Assert.Warn("Make a warning"); //And logged
                Console.WriteLine("Perhaps we are not on the login page - try and carry on anyway");
            }
            //And we can continue rather than crash out.




            //Find username input and type in to it
            driver.FindElement(By.Id("username")).SendKeys("stevepowell");
            //driver.FindElement(By.Id("username")).Clear(); //Get rid of first bit of typing
            driver.FindElement(By.Id("username")).SendKeys(Keys.Control + "a"); //Ctrl+a - select all
            driver.FindElement(By.Id("username")).SendKeys(Keys.Backspace); //Delete
            driver.FindElement(By.Id("username")).SendKeys("edgewords");

            //string usernameText = driver.FindElement(By.Id("username")).Text; //No - this is an 'empty' input element
            string usernameText = driver.FindElement(By.Id("username")).GetAttribute("value"); //Get the "value" attribute for text boxes instead
            Console.WriteLine("The username text is :" + usernameText);

            string pageTitle = driver.Title; //Capture page title - dont use find element. Title is not part of Body and not displayed.

            //Fill in password
            //Find and store ref to element so we can reuse it - less typing! 
            IWebElement passwordField = driver.FindElement(By.CssSelector("#password"));
            passwordField.Clear();
            passwordField.SendKeys("edgewords123");

            //Click the submit "Button"
            driver.FindElement(By.LinkText("Submit")).Click(); //"Button" is not a HTML button - it's actually an "a" anchor/link

            //Wait for page load
            //Thread.Sleep(2000);
            IWebElement logoutButton = WaitForLocator(driver, 2, By.LinkText("Log Out"));
            
            /*
             * Act - Do "the thing" - the point of the test. Can we log out? Lets try...
             */
            //Now logout
            logoutButton.Click();
            //Spawns a js confirm
            driver.SwitchTo().Alert().Accept();




            //Quitting driver moved to TearDown()
            Thread.Sleep(10000); //Wait for interstital page to clear ToDo: fix properly with a webdriverwait later.


            /*
             * Assert - check "the thing" did what it was supposed to
             * Capture a value, assert that the value is as expected
             */
            string finalHeading = driver.FindElement(By.CssSelector("#right-column > h1:nth-child(1)")).Text;
            //Check that the heading at the end was "Access and Authentication" - if yes - passs, if no fail
            //Assert.IsTrue(finalHeading == "Access and authentication","Wrong title"); //Classic model - not great errors
            Assert.That(finalHeading, Is.EqualTo("Access and Authentication"), "Wrong Title");
            Console.WriteLine("Finished"); //If "the thing" worked we reach here. Report we are done and all is well with the world.
        }
        


        [Test]
        public void DragDropDemo()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/docs/cssXPath.html";
            WaitForLocator(driver, 5, By.CssSelector(".ui-slider-handle"));
            IWebElement gripper = driver.FindElement(By.CssSelector(".ui-slider-handle"));
            IWebElement footer = driver.FindElement(By.Id("footer"));
            Actions myAction = new Actions(driver);
            
            //Cross browser scrolling by injecting JS
            IJavaScriptExecutor? jsdriver = driver as IJavaScriptExecutor;
            jsdriver?.ExecuteScript("arguments[0].scrollIntoView()", footer);
            
            IAction dragDrop = myAction
                .ScrollToElement(footer) //Chromium (and derivatives) only scrolling for now.
                .ClickAndHold(gripper) //Left click and hold on gripper
                .MoveByOffset(10, 0) //Begin moving - do lots of little jumps to work around Chromium issue
                .MoveByOffset(10, 0)
                .Pause(TimeSpan.FromSeconds(2)) //Brief pause half way through. Only avaialable in C# on newest WebDriver versions.
                .MoveByOffset(10, 0)
                .MoveByOffset(10, 0)
                .MoveByOffset(10, 0)
                .MoveByOffset(10, 0)
                .MoveByOffset(10, 0)
                .Release() //Release the left mouse button
                .Build(); //This was all one line
            
            dragDrop.Perform(); //Actually do the action

            Thread.Sleep(3000); //Pause to verify by sight that it worked
        }

        [Test]
        public void Chiaining()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/docs/forms.html";
            driver.FindElement(By.CssSelector("#select")) //Find select element
                .FindElement(By.CssSelector("[value='Selection Two'")) //find option /inside/ select
                .Click(); //click it

            //Checking if an element exists (and not crashing if it does not)
            try
            {
                driver.FindElement(By.Id("DOESNOTEXIST"));
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("No element but continue");
                Console.WriteLine("Exception was: " + ex);
            }

            IList<IWebElement> myElms = driver.FindElements(By.Id("STILLDOESNOTEXIST")); //Gather a collection pof elements that match the locator. If nothing matches just have an empty colection.
            if(myElms.Count == 0) //Collection objects have a Count property - if it is 0 nothing was found.
            {
                Console.WriteLine("No elms found");
            }
            
            Thread.Sleep(3000);
        }

    }

}
