using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using DoAn.Setting.Component;
using DoAn.Setting.Variable;
using OpenQA.Selenium.Chrome;

namespace DoAn.Setting
{
    internal class SetUp
    {
        private readonly bool SHOW_TEST_WEBSITE = false;
        private readonly int delayTime = Setting.Instance.DelayTime();

        private readonly string productWebLink = Setting.Instance.Website();
        private readonly string fileName = Setting.Instance.ExcelOutputLocation();
        private readonly string chromeDriverDirectory = Setting.Instance.ChromeDriverLocation();
        private WebComponent webComponent;

        private readonly ReadDataTest dataTest = new ReadDataTest();
        private readonly Excel.Export export;

        private IWebDriver driver;
        public SetUp(int worksheetNo)
        {
            //Đọc file excel
            this.export = new Excel.Export(fileName, worksheetNo);
        }

        //Cài đặt
        public void Setup(out IWebDriver driver)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications"); //Tắt hiện thông báo

            // Hiển thị trang web bật/mở mỗi lần chạy hay không
            if(SHOW_TEST_WEBSITE == true)
            {
                options.AddArgument("--start-maximized");
            }
            else
            {
                options.AddArgument("--headless");
                options.AddArgument("--window-size=1920,1080");
            }

                ChromeDriverService service = ChromeDriverService.CreateDefaultService(chromeDriverDirectory);
            driver = new ChromeDriver(service, options);
            this.driver = driver;
            webComponent = new WebComponent(driver);
        }

        //Đến trang sản phẩm
        public void ProductPage() => GoToPage(productWebLink);
        public void ConverterPage() => GoToPage("https://wise.com/vn/currency-converter/usd-to-vnd-rate");
        //Đăng nhập 
        public void Login(LoginType loginType)
        {
            (string, string) loginInfo = dataTest.Account(loginType);

            webComponent.Login_Icon();
            Thread.Sleep(delayTime);

            webComponent.Login_EmailInput(loginInfo.Item1);
            webComponent.Login_PasswordInput(loginInfo.Item2);
            webComponent.Login_SignInButton();
        }

        //Đăng xuất
        public void Logout()
        {
            try
            {
                try { webComponent.User_Avatar(); }     //Kiểm tra đang ở trang Profile
                catch { webComponent.Account_Icon(); } //Vào trang profile
                webComponent.SignOut();
            }
            catch {}
        }

        //Kiểm tra có failed hay không
        public void CheckFailed(string ID, string testData, string expected, string note, (string, bool) actual)
        {
            if (!actual.Item2)
            {
                Report(ID, testData, expected, actual.Item1, actual.Item2, note);
                Assert.Fail();
            }
        }

        public void FinalResult(string ID, string testData, string expected, string note, (string, bool) actual)
        {
            Report(ID, testData, expected, actual.Item1, actual.Item2, note);
            if (actual.Item2)
                Assert.Pass();
            else
                Assert.Fail();
        }

        private void GoToPage(string link)
        {
            try { driver.Navigate().GoToUrl(link); }
            catch{ throw new Exception("Không tiếp cận được trang web"); }
        }
        private void Report(string ID, string testData, string expected, string actual, bool result, string note)
        {
            export.ExportData(ID, testData, expected, actual, result, note);
            Console.WriteLine($"Expected:\r\n{expected}\r\n\r\n  Acutal:\r\n{actual}");
        }

    }
}
