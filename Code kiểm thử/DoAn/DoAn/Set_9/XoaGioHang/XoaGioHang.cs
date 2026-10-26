using DoAn.Setting;
using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using DoAn.Setting.Variable;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace DoAn.Set_9.XoaGioHang
{
    internal class XoaGioHang
    {
        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private WebComponent webComponent;
        private Feature feature;
        IWebDriver driver;

        //Biến
        private (string, bool) checkResult;
        private string expected, actual, testData;
        private (string, double)[] productList;
        private (string, int, double, bool)[] product_InShoppingCart_Before, product_InShoppingCart_After;


        [SetUp] 
        public void SetUp() 
        {
            //Khởi tạo driver
            setting.Setup(out driver);

            //Mở trang web
            setting.ProductPage();

            webComponent = new WebComponent(driver);
            feature = new Feature(driver);
        }

        [Test]
        [TestCase("XGH001", LoginType.Customer_NoInfo, 1)]
        [TestCase("XGH002", LoginType.Customer_NoInfo, 3)]
        [TestCase("XGH003", LoginType.Admin, 1)]
        [TestCase("XGH004", LoginType.Admin, 3)]
        public void XoaHang(string ID, LoginType loginType, int numberOfProduct)
        {
            //B1: Lấy danh sách sản phẩm
            productList = randomData.RandomProduct(numberOfProduct);
            Format_TestData_Expected();

            //B2: Đăng nhập
            setting.Login(loginType);

            //B3: Kiểm tra giỏ hàng
            checkResult = feature.Check_ShoppingCart(out product_InShoppingCart_Before, productList);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B4: Đọc giỏ hàng
            checkResult = DeleteProduct();
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B5: Cập nhật danh sách giỏ hàng
            feature.Scan_ShoppingCart(out product_InShoppingCart_After);

            //B6: Kiểm tra kết quả
            checkResult = CheckDeleteProduct();
            setting.FinalResult(ID, testData, expected, null, checkResult);
        }

        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }


        //Tạo Test Data & Expected Result
        private void Format_TestData_Expected()
        {
            expected = "Trong giỏ hàng đã xoá các sản phẩm: ";
 
            for (int i = 0; i < productList.Length; i++)
            {
                expected += $"\r\n- {productList[i].Item1}";

                if (i == 0){ testData = "- " + productList[i].Item1; }
                else { testData += $"\r\n- {productList[i].Item1}"; }
            }
        }

        //Xoá đồ trong giỏ hàng
        private (string, bool) DeleteProduct()
        {
            IList<IWebElement> cartProducts = webComponent.Cart_ProductList();

            foreach (IWebElement product in cartProducts)
            {
                // Bắt đối tượng chứa tên hàng
                string itemName = webComponent.Cart_ProductName(product);

                // Kiểm tra tên hàng và thực hiện hành động nếu tên hàng khớp
                for (int i = 0; i < productList.Length; i++)
                {
                    if (itemName == productList[i].Item1)
                    {
                        //Bắt đối tượng biểu tượng thùng rác và thực hiện nhấn vào đó
                        webComponent.Cart_DeleteProduct(product);

                        //Đóng ô xác nhận
                        return feature.CloseNotification();
                    }
                }
            }
            return (null, true);
        }

        //Kiểm tra giỏ hàng
        private (string, bool) CheckDeleteProduct()
        {
            for (int i = 0; i < product_InShoppingCart_After.Length; i++)
            {
                for (int j = 0; j < product_InShoppingCart_Before.Length; j++)
                {
                    if (product_InShoppingCart_Before[j].Item1 == product_InShoppingCart_After[i].Item1 && product_InShoppingCart_After[i].Item1 != "")
                    {
                        if (product_InShoppingCart_Before[j].Item1 == product_InShoppingCart_After[i].Item1)
                        {
                            actual = $"Không xoá được sản phẩm: {product_InShoppingCart_Before[j].Item1}";
                            return (actual, false);
                        }
                    }
                }
            }

            actual = "Trong giỏ hàng đã xoá các sản phẩm: ";

            for (int i = 0; i < productList.Length; i++)
            {
                actual += $"\r\n- {productList[i].Item1}";
            }
            return (actual, true);
        }
    }
}
