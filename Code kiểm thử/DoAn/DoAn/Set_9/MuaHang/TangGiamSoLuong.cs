using DoAn.Setting;
using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using DoAn.Setting.Variable;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading;

namespace DoAn.Set_9.MuaHang
{
    internal class TangGiamSoLuong
    {
        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private WebComponent webComponent;
        private Feature feature;
        IWebDriver driver;

        //Biến
        private bool result;
        private string testData, expected, note, actual;
        private double totalPrice;
        private (string, bool) checkResult;
        private (string, double, int, int)[] productList;
        private (string, int, double, bool)[] cartProductInfo_Before, cartProductInfo_After;

        [SetUp]
        public void Setup()
        {
            //Khởi tạo driver
            setting.Setup(out driver);

            //Mở trang web
            setting.ProductPage();
            setting.Login(LoginType.Customer_NoInfo);
            feature = new Feature(driver);
            webComponent = new WebComponent(driver);
        }

        [Test]
        [TestCase("MH010")]
        public void Increase_Quantity(string ID)
        {
            SetUpData(ID, false, true, false, QuantityType.Normal);

            //B1: Tăng số lượng
            Increase_DecreaseQuantity(true,false);

            //B2: Lấy thông tin giỏ hàng sau khi tăng
            feature.Scan_ShoppingCart(out cartProductInfo_After);

            //B3: Kiểm tra thông tin
            CheckQuantity(out result, out actual, true, false);
            setting.FinalResult(ID, testData, expected, note, (actual, result));
        }

        [Test]
        [TestCase("MH011")]
        public void Decrease_Quantity_OneItem(string ID)
        {
            SetUpData(ID, false, false, false, QuantityType.Normal);

            //B1: Giảm số lượng
            Increase_DecreaseQuantity(false, false);

            //B2: Lấy thông tin giỏ hàng sau khi giảm
            feature.Scan_ShoppingCart(out cartProductInfo_After);

            //B3: Kiểm tra thông tin
            CheckQuantity(out result, out actual, false, false);
            setting.FinalResult(ID, testData, expected, note, (actual, result));
        }

        [Test]
        [TestCase("MH012")]
        public void Decrease_Quantity_HigherThanOneItem (string ID)
        {
            SetUpData(ID, false, false, true, QuantityType.Normal);

            //B1: Giảm số lượng
            Increase_DecreaseQuantity(false, false);

            //B2: Lấy thông tin giỏ hàng sau khi giảm
            feature.Scan_ShoppingCart(out cartProductInfo_After);
            
            //b3: Kiểm tra thông tin
            CheckQuantity(out result, out actual, false, false);
            setting.FinalResult(ID, testData, expected, note, (actual, result));
        }

        [Test]
        [TestCase("MH013", QuantityType.Negative)]
        [TestCase("MH014", QuantityType.Overstock)]
        [TestCase("MH015", QuantityType.Normal)]
        public void Input_Quantity(string ID, QuantityType quantityType)
        {
            SetUpData(ID, true, true, false, quantityType);

            //B1: Thay đổi số lượng
            Increase_DecreaseQuantity(false, true);

            //B2: Lấy thông tin giỏ hàng sau khi tăng
            feature.Scan_ShoppingCart(out cartProductInfo_After);

            //Kiểm tra thông tin
            CheckQuantity(out result, out actual, false, true);
            setting.FinalResult(ID, testData, expected, note, (actual, result));
        }

        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }

        private void SetUpData(string ID, bool inputQuantity, bool increase, bool addTwice, QuantityType quantityType)
        {
            //Khởi tạo dữ liệu
            productList = randomData.RandomProductQuantity(quantityType, 1);
            Format_TestData_Expected(increase, inputQuantity, addTwice);

            //Check giỏ hàng
            checkResult = feature.Check_ShoppingCart(out cartProductInfo_Before, productList, addTwice);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            Format_TestData_Expected(increase, inputQuantity, addTwice);
        }

        private void Format_TestData_Expected(bool increase, bool input, bool addTwice)
        {
            testData = "Sản phẩm mua gồm:";
            expected = "Trong giỏ hàng có chứa sản phẩm:";
            bool firstRow = true;
            int expectedItem;
            double productPrice;
            string productFormat = "";
            totalPrice = 0;

            if (cartProductInfo_Before != null && cartProductInfo_Before.Length > 0)
            {
                for (int i = 0; i < cartProductInfo_Before.Length; i++)
                {
                    for (int j = 0; j < productList.Length; j++)
                    {
                        if (cartProductInfo_Before[i].Item1 == productList[j].Item1)
                        {
                            productPrice = productList[j].Item2;

                            //Nhập số lượng
                            if (input)
                            {
                                totalPrice += productPrice * productList[j].Item4;
                                productFormat += $"- {cartProductInfo_Before[i].Item1} - Số lượng: {productList[j].Item4} - Giá tiền: {productList[j].Item4 * productPrice}";
                                expected += "\r\n" + productFormat;
                                if (firstRow)
                                {
                                    expected += productFormat;
                                    firstRow = false;
                                }
                                else
                                {
                                    expected += "\r\n" + productFormat;
                                }
                            }

                            //Điều chỉnh số lượng
                            else
                            {
                                if (increase) { expectedItem = cartProductInfo_Before[i].Item2 + 1; }
                                else { expectedItem = (cartProductInfo_Before[i].Item2 == 1) ? 1 : cartProductInfo_Before[i].Item2 - 1; }
                                totalPrice += productPrice * expectedItem;
                                expected += $"\r\n- {cartProductInfo_Before[i].Item1} - Số lượng: {expectedItem} - Giá tiền: {expectedItem * productPrice}";

                                if (firstRow)
                                {
                                    testData += $"- {cartProductInfo_Before[i].Item1} - Số lượng: {cartProductInfo_Before[i].Item2} - Giá tiền: {productPrice}";
                                    firstRow = false;
                                }
                                else { testData += $"\r\n- {cartProductInfo_Before[i].Item1} - Số lượng: {cartProductInfo_Before[i].Item2} - Giá tiền: {productPrice}"; }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < productList.Length; i++)
                {
                    //Nhập số lượng
                    if (input)
                    {
                        productPrice = productList[i].Item2;
                        totalPrice += productPrice * productList[i].Item4;

                        expected += $"\r\n- {productList[i].Item1} - Số lượng: {productList[i].Item4} - Giá tiền: {productList[i].Item4 * productPrice}";
                    }
                    //Điều chỉnh số lượng
                    else
                    {
                        expectedItem = (increase) ? 2 : 1;
                        productPrice = productList[i].Item2;
                        totalPrice += productPrice * expectedItem;
                        expected += $"\r\n- {productList[i].Item1} - Số lượng: {expectedItem} - Giá tiền: {expectedItem * productPrice}";
                    }
                    testData += (i == 0 ? "" : "\r\n") + $"- {productList[i].Item1} - Số lượng: 1 - Giá tiền: {productPrice}";
                }
            }
            expected += $"\r\n\r\n Tạm tính: {totalPrice}";
            note = addTwice ? "Sản phẩm đã ở mức thấp nhất nên không thể hạ hơn được nữa." : "";
        }

        //Tăng - Giảm số lượng sản phẩm
        private void Increase_DecreaseQuantity(bool increase, bool input)
        {
            Thread.Sleep(1000);
            //Tăng số lượng
            IList<IWebElement> cartProducts = driver.FindElements(By.CssSelector(".test.r-item"));
            for (int i = 0; i < cartProducts.Count; i++)
            {
                string productName = webComponent.Cart_ProductName(cartProducts[i]);
                for (int j = 0; j < productList.Length; j++)
                {
                    if (productName == productList[j].Item1)
                    {
                        //Nhập số lượng sản phẩm
                        if (input)
                        {
                            webComponent.Cart_QuantityInput(cartProducts[i]).Clear();
                            webComponent.Cart_QuantityInput(cartProducts[i]).SendKeys(productList[j].Item3.ToString());
                            webComponent.Cart_QuantityInput(cartProducts[i]).SendKeys(Keys.Enter);
                        }
                        //Ấn nút tăng giảm số lượng
                        else
                        {
                            if (increase) { webComponent.Cart_AddQuantityButton(cartProducts[i]); }
                            else { webComponent.Cart_MinusQuantityButton(cartProducts[i]); }
                        }

                    }
                }
            }
        }

        //Kiểm tra số lượng
        private void CheckQuantity(out bool result, out string actual, bool increase, bool input)
        {
            result = true;
            actual = "Trong giỏ hàng có chứa sản phẩm:";
            double tempPrice = 0;
            double productPrice;

            for (int i = 0; i < cartProductInfo_After.Length; i++)
            {
                for (int j = 0; j < productList.Length; j++)
                {
                    if (cartProductInfo_After[i].Item1 == productList[j].Item1)
                    {
                        //Kiểm tra số lượng
                        actual += $"\r\n- {cartProductInfo_After[i].Item1} - Số lượng: {cartProductInfo_After[i].Item2}";

                        //Nhập số lượng
                        if (input)
                        {
                            if (cartProductInfo_After[i].Item2 != productList[j].Item4)
                            {
                                actual += $" [Sai - Phải là {productList[j].Item4}]";
                                result = false;
                            }
                        }
                        else
                        {
                            if (increase && cartProductInfo_After[i].Item2 != cartProductInfo_Before[i].Item2 + 1)
                            {
                                actual += $" [Sai - Phải là {cartProductInfo_Before[i].Item2 + 1}]";
                                result = false;
                            }
                            if (!increase)
                            {
                                if (cartProductInfo_Before[i].Item2 == 1 && cartProductInfo_After[i].Item2 != 1)
                                {
                                    actual += " [Sai - Phải là 1]";
                                    result = false;
                                }
                                if (cartProductInfo_After[i].Item2 != cartProductInfo_Before[i].Item2 - 1 && cartProductInfo_Before[i].Item2 != 1)
                                {
                                    actual += $" [Sai - Phải là {cartProductInfo_Before[i].Item2 - 1}]";
                                    result = false;
                                }
                            }
                        }

                        //Kiểm tra số tiền
                        productPrice = productList[j].Item2;
                        actual += $" - Giá tiền: {cartProductInfo_After[i].Item3}";
                        double expectedPrice = 0;
                        bool priceResult = true;

                        //Nhập số lượng
                        if (input)
                        {
                            if (cartProductInfo_After[i].Item3 != productPrice * productList[j].Item4)
                            {
                                expectedPrice = productPrice * productList[j].Item4;
                                actual += $" [Sai - Phải là: {expectedPrice}]";
                                result = false;
                            }
                        }
                        else
                        {
                            //Tăng số lượng
                            if (increase && cartProductInfo_After[i].Item3 != productPrice * (cartProductInfo_Before[i].Item2 + 1))
                            {
                                expectedPrice = productPrice * (cartProductInfo_Before[i].Item2 + 1);
                                result = false;
                                priceResult = false;
                            }
                            //Giảm số lượng
                            if (!increase)
                            {
                                if (cartProductInfo_Before[i].Item2 == 1 && cartProductInfo_After[i].Item3 != productPrice)
                                {
                                    expectedPrice = productPrice;
                                    result = false;
                                    priceResult = false;
                                }
                                if (cartProductInfo_Before[i].Item2 != 1 && cartProductInfo_After[i].Item3 != productPrice * (cartProductInfo_Before[i].Item2 - 1))
                                {
                                    expectedPrice = productPrice * (cartProductInfo_Before[i].Item2 - 1);
                                    result = false;
                                    priceResult = false;
                                }
                            }
                            if (!priceResult)
                            {
                                actual += $" [Sai - Phải là: {expectedPrice}]";
                            }
                        }
                        tempPrice += cartProductInfo_After[i].Item3;
                    }
                }
            }

            actual += $"\r\n\r\n Tạm tính: {tempPrice}";
            if (tempPrice != totalPrice) { actual += $" [Sai - Phải là: {totalPrice}]"; }
        }
    }
}
