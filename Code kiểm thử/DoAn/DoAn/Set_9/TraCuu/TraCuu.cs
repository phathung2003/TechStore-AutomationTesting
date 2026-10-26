using DoAn.Setting;
using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DoAn.Set_9.TraCuu
{
    public class TraCuu
    {
        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private readonly GetData getData = new GetData();
        private Feature feature;
        IWebDriver driver;

        //Biến
        private (string, double)[] productList;
        private string expected, testData;
        private int totalProduct;
        private (string, bool) checkResult;
        
        
        [SetUp]
        public void SetUp()
        {
            //Khởi tạo driver
            setting.Setup(out driver);

            //Mở trang web
            setting.ProductPage();

            feature = new Feature(driver);
        }

        [Test] //Kiểm tra full tên
        [TestCase("FSP001",false)]
        [TestCase("FSP002",true)]
        public void Search_FullName(string ID, bool RandomFormat)
        {
            //B1: Lấy dữ liệu test
            SetUpData(RandomFormat, false);
          
            //B2: Tìm kiếm sản phẩm
            feature.SearchProduct(testData);

            //B3: Kiểm tra thông tin
            checkResult = Check_ProductName(testData, false);
            setting.FinalResult(ID, testData, expected, null, checkResult);
        }

        [Test] //Kiểm tra 1 phần của tên
        [TestCase("FSP003", false)]
        [TestCase("FSP004", true)]
        public void Search_PartOfTheName(string ID, bool RandomFormat)
        {
            //B1: Lấy dữ liệu test
            SetUpData(RandomFormat, true);

            //B2: Tìm kiếm sản phẩm
            feature.SearchProduct(testData);

            //B3: Kiểm tra thông tin
            checkResult = Check_ProductName(testData,false);
            setting.FinalResult(ID, testData, expected, null, checkResult);
        }

        [Test] //Kiểm tra sản phẩm không tồn tại
        [TestCase("FSP005", "Xiaomi")]
        public void Search_NotFound(string ID,string testData)
        {
            expected = "Hiện trang trắng.";

            //B1: Tìm kiếm sản phẩm
            feature.SearchProduct(testData);

            //B2: Kiểm tra thông tin
            checkResult = Check_ProductName(testData, true);
            setting.FinalResult(ID, testData, expected, null, checkResult);
        }
        
        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }




        //Kiểm tra sản phẩm có chứa tên
        private (string, bool) Check_ProductName(string target, bool noMatch)
        {
            string actual;
            string pattern = $@".*{target.ToLower()}.*";
            Regex regex = new Regex(pattern);

            //Lấy danh sách hàng trong Shop
            IList<IWebElement> shopList = driver.FindElements(By.CssSelector(".card__title"));
            if(shopList.Count > 0)
            {
                for (int i = 0; i < shopList.Count; i++)
                {
                    if (!regex.IsMatch(shopList[i].Text.ToLower()))
                    {
                        actual = $"Có 1 sản phẩm không chứa tên: \"{shopList[i].Text}\".";
                        return (actual, false);
                    }
                }
            }
            return Check_TotalProduct(totalProduct, shopList.Count, noMatch ? testData : productList[0].Item1, noMatch);
        }


        //Kiểm tra số lượng sản phẩm
        private (string, bool) Check_TotalProduct(int expectedQuantity, int actualQuantity, string dataTest, bool noMatch)
        {
            string actual;

            //Không tìm thấy sản phẩm
            if (noMatch)
            {
                switch (actualQuantity)
                {
                    case 0:
                        actual = "Hiện trang trắng.";
                        return (actual, true);
                    default:
                        actual = $"Hiện {actualQuantity} sản phẩm.";
                        return (actual, false);
                }
            }
            //Tìm thấy sản phẩm
            if (expectedQuantity != actualQuantity)
            {
                actual = $"Hiển thị {actualQuantity} sản phẩm có chứa tên \"{dataTest}\" [Sai - Phải là: {expectedQuantity} sản phẩm]";
                return (actual, false);
            }
            actual = $"Hiển thị {actualQuantity} sản phẩm có chứa tên \"{dataTest}\".";
            return (actual, true);
        }


        //Setup dữ liệu
        private void SetUpData(bool randomFormat, bool partOfName)
        {
            if (partOfName)
            {
                testData = randomData.RandomPartOfTheName();
                productList = new (string,double)[1];
                productList[0] = (testData, 0);
                testData = randomFormat ? randomData.RandomFormatInput(testData) : testData;
            }
            else
            {
                productList = randomData.RandomProduct(1);
                testData = randomFormat ? randomData.RandomFormatInput(productList[0].Item1) : productList[0].Item1;
            }

            totalProduct = getData.TotalProductContainName(productList[0].Item1);
            expected = $"Hiển thị {totalProduct} sản phẩm có chứa tên \"{productList[0].Item1}\".";
        }
    }
}
