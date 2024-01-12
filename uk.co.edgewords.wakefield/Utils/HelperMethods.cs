using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using System.Reflection;

namespace uk.co.edgewords.wakefield.Utils
{
    public static class HelperMethods
    {
        public static IWebElement WaitForLocator(IWebDriver driver, int Timeout, By Locator)
        {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(Timeout));
            myWait.Until(drv => drv.FindElement(Locator).Displayed);
            return driver.FindElement(Locator);
        }

        public static void SayHi()
        {
            Console.WriteLine("Hi");
        }
        public static IEnumerable<string[]> GetTestData()
        {
            var csvFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\data.csv"; //Find test dll folder, add data.csv on to it. That's our data source.
            csvFile = new Uri(csvFile).LocalPath; //Get the Uri for the data source

            using (var csv = new CsvReader(new StreamReader(csvFile), true)) //Read the data source. The first line is data column headers(true) not actual test data.
            {
                while (csv.ReadNextRecord()) //while there are new lines of data available (i.e. not end of file)
                {
                    string data1 = csv[0]; //Get row x - col 1 data
                    string data2 = csv[1]; //Get row x - col 2 data
                    yield return new[] { data1, data2 }; //Hand back an array with those two parts
                } //When End Of File reached we will have handed back a list of string arrays. Just what nUnit data driven tests want.
            }
        }
    }
}
