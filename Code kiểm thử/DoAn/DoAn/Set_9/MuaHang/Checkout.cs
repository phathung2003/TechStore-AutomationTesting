using DoAn.Setting;
using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using DoAn.Setting.Variable;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DoAn.Set_9.MuaHang
{
    internal class Checkout
    {
        private readonly int delayTime = Setting.Setting.Instance.DelayTime();
        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private readonly GetData getData = new GetData();
        private Feature feature;
        private WebComponent webComponent;
        IWebDriver driver;

        //Biến
        private string testData, expected, actual, rank;
        private double expectedPreTotal, expectedTotal, expectedVoucher, expectedDiscountRank;
        private (string, bool) checkResult;
        private (string, string, double) voucherInfo;
        private (string, string, string) userInfo;
        private (string,double)[] productList;

        [SetUp]
        public void Setup()
        {
            //Khởi tạo driver
            setting.Setup(out driver);

            //Mở trang web
            setting.ProductPage();

            webComponent = new WebComponent(driver);
            feature = new Feature(driver);
        }

        [Test]
        [TestCase("MH025", null)]
        [TestCase("MH026", VoucherType.Accepted)]
        [TestCase("MH027", VoucherType.OutDate)]
        [TestCase("MH028", VoucherType.Overload)]
        [TestCase("MH029", VoucherType.OutOfStock)]
        [TestCase("MH030", VoucherType.NotExist)]
        public void Checkout_HaveDiscount(string ID, VoucherType voucherType)
        {
            SetupVariable();
            Format_TestData_Expected(voucherType);

            //B1: Đăng nhập
            setting.Login(LoginType.Customer_NoInfo);

            //B2: Kiểm tra giỏ hàng
            checkResult = feature.Check_ShoppingCart(productList);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B3: Lấy thông tin giỏ hàng
            rank = webComponent.Cart_Rank();
            Format_TestData_Expected(voucherType);

            //B4: Nhập thông tin giao hàng
            checkResult = feature.DestinationChange(userInfo.Item1, userInfo.Item2, userInfo.Item3);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B5: Thêm khuyến mãi (Nếu có)
            if (voucherType != VoucherType.None) { feature.InputVoucher(voucherInfo.Item2); }

            //B6: Nhấn thông báo xác nhận
            checkResult = GetNotification(out actual);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B7: Kiểm tra thông tin
            checkResult = GetPendingList();
            actual += checkResult.Item1;
            setting.FinalResult(ID, testData, expected, null, (actual, checkResult.Item2));
        }


        [Test]
        [TestCase("MH031")]
        public void Checkout_HaveInfo(string ID)
        {
            SetupVariable();

            //B1: Đăng nhập tài khoản
            setting.Login(LoginType.Customer_HaveInfo);

            //B2: Lấy thông tin người dùng
            webComponent.Account_Icon();
            webComponent.UserInfoButton();
            string name = webComponent.UserInfo_NameInput().GetAttribute("value");
            string phone = webComponent.UserInfo_PhoneInput().GetAttribute("value");
            string address = webComponent.UserInfo_AddressInput().GetAttribute("value");
            userInfo = (name, phone, address);
            Format_TestData_Expected(VoucherType.None);


            //B3: Kiểm tra giỏ hàng
            checkResult = feature.Check_ShoppingCart(productList);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B4: Lấy thông tin giỏ hàng
            rank = webComponent.Cart_Rank();
            Format_TestData_Expected(VoucherType.None);

          
            //B5: Nhấn thông báo xác nhận
            checkResult = GetNotification(out actual);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B6: Kiểm tra thông tin
            checkResult = GetPendingList();
            actual += checkResult.Item1;
            setting.FinalResult(ID, testData, expected, null, (actual, checkResult.Item2));
        }


        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }


        private void SetupVariable()
        {
            this.userInfo = randomData.GenerateInfo("COD");
            this.productList = randomData.RandomProduct(1);
        }

        private void Format_TestData_Expected(VoucherType voucherType)
        {
            //Tạm tính
            this.expectedPreTotal = 0;
            string productFormat = "";

            for (int i = 0; i < this.productList.Length; i++)
            {
                double productPrice = productList[i].Item2;
                this.expectedPreTotal += productPrice;
                productFormat += (i == 0 ? "" : "\r\n") + $"- {this.productList[i].Item1} - Số lượng: 1 - Giá: {productPrice}";
            }
            double discountRankPercent = getData.GetDiscountRank(string.IsNullOrEmpty(rank) ? "Đồng" : rank);
            this.expectedDiscountRank = this.expectedPreTotal * discountRankPercent;
            voucherInfo = voucherType != VoucherType.None ? getData.GetVoucher(voucherType, this.expectedPreTotal - expectedDiscountRank) : (null, null, 0);
            expectedVoucher = voucherType == VoucherType.Accepted ? voucherInfo.Item3 : 0;
            this.expectedTotal = this.expectedPreTotal - this.expectedDiscountRank + expectedVoucher;

            //Test Data
            this.testData = $"* Phần giao tới có thông tin:\r\n- Name: {this.userInfo.Item1}\r\n- Phone: {this.userInfo.Item2}\r\n- Address: {userInfo.Item3}";
            this.testData += $"\r\n\r\n* Sản phẩm mua: \r\n" + productFormat;

            if (voucherType != VoucherType.None) { this.testData += $"\r\n\r\n* Mã khuyến mãi sử dụng: {voucherInfo.Item2}"; }

            //Expected
            this.expected = "Hiện thông báo:\r\nThe order was checked out successful";
            this.expected += $"\r\n\r\nCó đơn hàng có thông tin như sau:";
            this.expected += $"\r\n- Name: {userInfo.Item1}\r\n- Phone: {userInfo.Item2}\r\n- Address: {userInfo.Item3}";
            this.expected += "\r\n\r\nChi tiết đơn hàng có:\r\n" + productFormat;
            this.expected += $"\r\n\r\nTotal: {this.expectedPreTotal}";
            this.expected += "\r\nVoucher: " + (-expectedVoucher);
            this.expected += $"\r\nDiscount Rank: {this.expectedDiscountRank}";
            this.expected += $"\r\nTotal paid: {this.expectedTotal}";
        }

        private (string,bool) GetPendingList()
        {
            int rightProduct = 0;
            bool result = true;
            bool haveOrder = false;

            string actual = "\r\n\r\nCó đơn hàng có thông tin như sau:";
            
            //Vào Account
            Thread.Sleep(delayTime);
            webComponent.Account_Icon();
            Thread.Sleep(delayTime);

            //Vào Order
            webComponent.Order();
            Thread.Sleep(delayTime);

            //Lấy từng đơn hàng
            IList<IWebElement> orderList = webComponent.OrderList();
            // Duyệt qua từng phần tử trong danh sách
            for (int i = 0; i < orderList.Count; i++)
            {
                // Lấy thông tin của mỗi đơn hàng
                string name = webComponent.OrderList_UserName(orderList[i]);
                string address = webComponent.OrderList_Address(orderList[i]);
                string phone = webComponent.OrderList_Phone(orderList[i]);

                if (name == userInfo.Item1 && phone == userInfo.Item2 && address == userInfo.Item3)
                {
                    haveOrder = true;

                    actual += $"\r\n- Name: {userInfo.Item1}\r\n- Phone: {userInfo.Item2}\r\n- Address: {userInfo.Item3}";
                    actual += "\r\n\r\nChi tiết đơn hàng có:";

                    // Chi tiết đơn hàng
                    IList<IWebElement> orderDetailList = webComponent.OrderList_OrderDetail(orderList[i]);
                    for(int j  = 0; j < orderDetailList.Count; j++)
                    {
                        string productName = webComponent.OrderList_OrderDetail_ProductName(orderDetailList[j]);
                        string quantityValue = webComponent.OrderList_OrderDetail_Quantity(orderDetailList[j]);
                        string priceValue = webComponent.OrderList_OrderDetail_Price(orderDetailList[j]);

                        if (!int.TryParse(quantityValue, out int quantity)) { return ("Số lượng hàng hoá có chữ", false); }
                        if (!double.TryParse(priceValue, out double price)) { return ("Tổng giá trị sản phẩm có chữ", false); }

                        checkResult = ProductCheck(productName, quantity, price);
                        actual += checkResult.Item1;
                        if (checkResult.Item2) { rightProduct++; }
                    }
                    if(rightProduct != productList.Length) { actual += "\r\n[Không đủ số lượng sản phẩm]"; }

                    string totalValue = webComponent.OrderList_Total(orderList[i]);
                    string voucherValue = webComponent.OrderList_Voucher(orderList[i]);
                    string discountRankValue = webComponent.OrderList_DiscountRank(orderList[i]);
                    string totalPaidValue = webComponent.OrderList_TotalPaid(orderList[i]);

                    if (!double.TryParse(totalValue, out double total)) { return ("Total có chữ", false); }
                    if (!double.TryParse(voucherValue, out double voucher)) { return ("Voucher có chữ", false); }
                    if (!double.TryParse(discountRankValue, out double discountRank)) { return ("Discount rank có chữ", false); }
                    if (!double.TryParse(totalPaidValue, out double totalPaid)) { return ("Total paid có chữ", false); }

                    checkResult = TotalCheck(total, voucher, discountRank, totalPaid);
                    actual += checkResult.Item1;
                    result = checkResult.Item2;

                    if (rightProduct != productList.Length) { result = false; }
                    if (result) { break; }
                }        
            }
            if (!haveOrder)
            {
                actual = "\r\n\r\n[Không tìm thấy đơn hàng]";
                result = false;
            }
            return (actual, result);
        }

        private (string,bool) GetNotification(out string actual)
        {
            int fail = 0;

            //Nhấn nút "Checkout"
            Thread.Sleep(delayTime);
            webComponent.Cart_CheckoutButton();
            while (true)
            {
                try
                {
                    string notification = webComponent.Cart_Checkout_NotificationStatus();
                    if (notification != "The order was checked out successful")
                    {
                        actual = $"Hiện thông báo: \r\n{notification} [Sai]";
                        return (actual, false);
                    }
                    actual = $"Hiện thông báo: \r\n{notification}";
                    return feature.CloseNotification();
                }
                catch
                {
                    if (fail > 5) { break; }
                    Thread.Sleep(delayTime);
                    fail++;
                }
            }
            actual = $"Không hiện hoặc hiện ô thông báo đặt hàng thành công quá lâu!";
            return (actual, false);
        }

        private (string,bool) ProductCheck(string productName, int quantity, double price)
        {
            bool productNameResult = false;
            bool quantityResult = false;
            bool priceResult = false;

            for (int i = 0; i < productList.Length; i++)
            {
                if (productList[i].Item1 == productName)
                {
                    productNameResult = true;
                    if(quantity == 1){ quantityResult = true; }
                    if (productList[i].Item2 == price) { priceResult = true; }
                    break;
                }
            }
            string actual = "\r\n" + $"- {productName}" + (!productNameResult ? " [Sai] ":" ")
                                   + $"- Số lượng: {quantity}" + (!quantityResult ? " [Sai] " : " ")
                                   + $"- Giá: {price}" + (!priceResult ? " [Sai] " : "");
            if(productNameResult && quantityResult && priceResult) { return (actual, true); }
            return (actual, false);
        }

        private (string,bool) TotalCheck(double actualPreTotal, double actualVoucher, double actualDiscountRank, double actualTotal)
        {
            string actual;
            bool result = true;

            actual = $"\r\n\r\nTotal: {actualPreTotal}";
            if (expectedPreTotal != actualPreTotal)
            {
                actual += " [Sai]";
                result = false;
            }

            actual += $"\r\nVoucher: {actualVoucher}";
            if (expectedVoucher != actualVoucher)
            {
                actual += " [Sai]";
                result = false;
            }

            actual += $"\r\nDiscount Rank: {actualDiscountRank}";
            if (expectedDiscountRank != actualDiscountRank)
            {
                actual += " [Sai]";
                result = false;
            }

            actual += $"\r\nTotal:  {actualTotal}";
            if (expectedTotal != actualTotal)
            {
                actual += " [Sai]";
                result = false;
            }

            return (actual, result);
        }
    }
}
