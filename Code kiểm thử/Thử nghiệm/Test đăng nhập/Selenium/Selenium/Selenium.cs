using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using System.Xml.Linq;

namespace Selenium
{
    public class Selenium
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            //Làm tự động
            //driver = new ChromeDriver();
            ////Làm thủ công
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService("~/DoAn/chromedriver-win64/chromedriver-win64");
            driver = new ChromeDriver(service, options);
        }

        [Test]
        [TestCase("admin123@gmail.com", "admin123")]
        //Test đăng nhập
        public void Input_Invalid(string user, string pass)
        {
            driver.Navigate().GoToUrl("http://localhost/");
            driver.FindElement(By.XPath("//a[@id='init-login-form-1']//span[@class='material-symbols-outlined'][normalize-space()='person']")).Click();
            driver.FindElement(By.XPath("//form[@class='form-sign-in']//input[@placeholder='Email']")).SendKeys(user);
            driver.FindElement(By.XPath("//form[@class='form-sign-in']//input[@placeholder='Password']")).SendKeys(pass);
            driver.FindElement(By.XPath("//form[@class='form-sign-in']//button[@type='submit'][normalize-space()='Sign In']")).Click();
            Thread.Sleep(2000);
            string actual = driver.FindElement(By.XPath("//span[normalize-space()='Hello admin !']")).Text;
            Assert.That(actual, Is.EqualTo("Hello admin !"));
        }

        ////Mở 1 trình duyện xong đóng trình duyệt đó đi
        //[TearDown]
        //public void TearDown()
        //{
        //    driver.Quit();
        //}
    }
}
