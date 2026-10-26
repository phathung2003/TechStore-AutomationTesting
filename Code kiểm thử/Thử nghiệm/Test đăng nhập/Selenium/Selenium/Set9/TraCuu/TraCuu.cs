using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using System.Xml.Linq;

namespace Selenium.Set9.TraCuu
{
    internal class TraCuu
    {
        private string webLink = Setting.Setting.Instance.Website();

        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService("S:\\Download\\Compressed\\chromedriver-win64_2\\chromedriver-win64");
            driver = new ChromeDriver(service, options);
        }

        [Test]
        [TestCase("admin123@gmail.com", "admin123")]
        //Test đăng nhập
        public void Input_Invalid()
        {
            driver.Navigate().GoToUrl(webLink);    
            Thread.Sleep(10000);
        }

        ////Mở 1 trình duyện xong đóng trình duyệt đó đi
        //[TearDown]
        //public void TearDown()
        //{
        //    driver.Quit();
        //}
    }
}
