using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.edgewords.wakefield.Utils
{
    internal class InstanceHelperMethods
    {
        private IWebDriver driver;

        public InstanceHelperMethods(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void WaitForLocator(int Timeout, By Locator)
        {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(Timeout));
            myWait.Until(drv => drv.FindElement(Locator).Displayed);
        }
    }
}
