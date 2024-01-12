using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.edgewords.wakefield.POMPages
{
    internal class LoginPagePOM
    {
        private IWebDriver _driver;

        public LoginPagePOM(IWebDriver driver)
        {
            this._driver = driver;
            string pageTitle = driver.Title;
            Assert.That(pageTitle, Is.EqualTo("Automated Tools Test Site"));
        }

        //Locators
        IWebElement usernameField => _driver.FindElement(By.Id("username"));
        IWebElement passwordField => _driver.FindElement(By.Id("password"));
        IWebElement submitButton => _driver.FindElement(By.LinkText("Submit"));

        //Service Methods

        public LoginPagePOM SetUsername(string username)
        {
            usernameField.Clear();
            usernameField.SendKeys(username);
            return this;
        }

        public LoginPagePOM SetPassword(string password)
        {
            passwordField.Clear();
            passwordField.SendKeys(password);
            return this;
        }

        public void SubmitForm()
        {
            submitButton.Click();

        }

        //Higher level helpers
        public void Login(string username, string password)
        {
            SetUsername(username);
            SetPassword(password);
            SubmitForm();
        }

        public bool LoginExpectSuccess(string username, string password)
        {
            Login(username, password);
            
            try
            {
                _driver.SwitchTo().Alert();
                return false;
            }
            catch (NoAlertPresentException ex)
            {
                return true;
            }
        }

        public bool LoginExpectFail(string username, string password)
        {
            Login(username, password);

            try
            {
                _driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException ex)
            {
                return false;
            }
        }

    }
}
