using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.edgewords.wakefield.POMPages
{
    public class HomePagePOM
    {
        private IWebDriver _driver; //Field to hold driver for this class to work with

        //Constructor - method with the same name as the class it is defined in
        //runs automatically at instantiation (new). Accepts the driver, puts it in the private field above
        public HomePagePOM(IWebDriver driver)
        {
            this._driver = driver;
        }

        //Locators - find stuff on page
        public IWebElement loginLink => _driver.FindElement(By.PartialLinkText("Login"));

        //Service Methods - do stuff with locators on page
        public void GoLogin()
        {
            loginLink.Click();
        }

    }
}
