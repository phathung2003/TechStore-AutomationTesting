using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using DoAn.Setting.Variable;
using DoAn.Setting;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using System.Collections.Generic;
namespace DoAn.Set_9.HuyDon
{
    internal class HuyDon
    {
        private readonly int delayTime = Setting.Setting.Instance.DelayTime();

        //Thành phần
        private readonly SetUp setting = new SetUp(1);
        private Feature feature;
        private WebComponent webComponent;

        //Dữ liệu
        private readonly RandomData randomData = new RandomData();
        private readonly GetData getData = new GetData();

        //Biến
        string testData, expected, rank;
        private double expectedPreTotal, expectedTotal, expectedDiscountRank, expectedVoucher;
        private (string, string, string) userInfo;
        private (string, double)[] productList;
        private (string, int, double)[] orderProductList;
        private (string, string, double) discountInfo;
        private (string, bool) checkResult;
        private int position;
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            //Khởi tạo driver
            setting.Setup(out driver);

            //Mở trang web
            setting.ProductPage();

            //Đăng nhập
            setting.Login(LoginType.Customer_NoInfo);

            webComponent = new WebComponent(driver);
            feature = new Feature(driver);
        }

        [Test]
        [TestCase("HD001", false)]
        [TestCase("HD002", true)]
        public void CancelOrder(string ID, bool delete)
        {
            //B1: Kiểm tra có đơn hàng nào không
            checkResult = CheckOrder(delete);
            setting.CheckFailed(ID, testData, expected, null, checkResult);
            
            //B2: Thực hiện xoá đơn
            DeleteOrder(delete);

            //B3: Kiểm tra đơn
            checkResult = CheckOrderDelete(delete);
            setting.FinalResult(ID, testData, expected, null, checkResult);
        }

        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }

        private (string, bool) CheckOrder(bool delete)
        {
            //Vào Account
            webComponent.Account_Icon();

            //Vào Order
            webComponent.Order();

            //Lấy từng đơn hàng
            IList<IWebElement> orderList = webComponent.OrderList();

            if (orderList.Count == 0)
            {
                checkResult = AddOrder(delete);
                if (!checkResult.Item2) { return (checkResult.Item1, checkResult.Item2); }
                Thread.Sleep(delayTime);
                webComponent.Account_Icon();
                webComponent.Order();
                Thread.Sleep(delayTime * 2);
            }
            orderList = webComponent.OrderList();
            position = randomData.GenerateNumber(0, orderList.Count - 1);

            // Lấy thông tin của mỗi đơn hàng
            string name = webComponent.OrderList_UserName(orderList[position]);
            string address = webComponent.OrderList_Address(orderList[position]);
            string phone = webComponent.OrderList_Phone(orderList[position]);
            userInfo = (name, phone, address);

            IList<IWebElement> orderDetail = webComponent.OrderList_OrderDetail(orderList[position]);
            orderProductList = new (string, int, double)[orderDetail.Count];
            for (int j = 0; j < orderDetail.Count; j++)
            {
                string productName = webComponent.OrderList_OrderDetail_ProductName(orderDetail[j]);
                string quantityValue = webComponent.OrderList_OrderDetail_Quantity(orderDetail[j]);
                string priceValue = webComponent.OrderList_OrderDetail_Price(orderDetail[j]);

                if (!int.TryParse(quantityValue, out int quantity)) { return ("Số lượng hàng hoá có chữ", false); }
                if (!double.TryParse(priceValue, out double price)) { return ("Tổng giá trị sản phẩm có chữ", false); }
                orderProductList[j] = (productName, quantity, price);
            }

            string totalValue = webComponent.OrderList_Total(orderList[position]);
            string voucherValue = webComponent.OrderList_Voucher(orderList[position]);
            string discountRankValue = webComponent.OrderList_DiscountRank(orderList[position]);
            string totalPaidValue = webComponent.OrderList_TotalPaid(orderList[position]);

            if (!double.TryParse(totalValue, out expectedPreTotal)) { return ("Total có chữ", false); }
            if (!double.TryParse(voucherValue, out expectedVoucher)) { return ("Voucher có chữ", false); }
            if (!double.TryParse(discountRankValue, out expectedDiscountRank)) { return ("Discount rank có chữ", false); }
            if (!double.TryParse(totalPaidValue, out expectedTotal)) { return ("Total paid có chữ", false); }
            Format_TestData_Expected(true, delete);
            return (null, true);
        }

        private (string, bool) AddOrder(bool delete)
        {
            productList = randomData.RandomProduct(1);
            userInfo = randomData.GenerateInfo("COD");
            Format_TestData_Expected(false, delete);

            //B1: Kiểm tra giỏ hàng
            checkResult = feature.Check_ShoppingCart(productList);
            if(!checkResult.Item2) { return (checkResult.Item1, checkResult.Item2); }

            //B2: Lấy thông tin giỏ hàng
            rank = webComponent.Cart_Rank();
            Format_TestData_Expected(false, delete);

            //B3: Nhập thông tin giao hàng
            checkResult = feature.DestinationChange(userInfo.Item1, userInfo.Item2, userInfo.Item3);
            if (!checkResult.Item2) { return (checkResult.Item1, checkResult.Item2); }

            //B4: Thêm khuyến mãi
            feature.InputVoucher(discountInfo.Item2);

            //B5: Ấn nút Checkout
            webComponent.Cart_CheckoutButton();
            return feature.CloseNotification();
        }

        private void Format_TestData_Expected(bool haveOrder, bool delete)
        {
            string productFormat = GetProductList(haveOrder);

            if (!haveOrder)
            {
                double discountRankPercent = getData.GetDiscountRank(string.IsNullOrEmpty(rank) ? "Đồng" : rank);
                expectedDiscountRank = expectedPreTotal * discountRankPercent;
                discountInfo = getData.GetVoucher(VoucherType.Accepted, (expectedPreTotal - expectedDiscountRank));
                expectedTotal = (expectedPreTotal - expectedDiscountRank + discountInfo.Item3);
            }

            testData = "Đơn hàng có thông tin như sau:\r\n" +
                       $"- Name: {userInfo.Item1}\r\n" +
                       $"- Phone: {userInfo.Item2}\r\n" +
                       $"- Address: {userInfo.Item3}";

            testData += "\r\n\r\nChi tiết đơn hàng có:\r\n" + productFormat;
            testData += $"\r\n\r\nTotal: {expectedDiscountRank}";
            testData += $"\r\nVoucher: " + (!haveOrder ? -discountInfo.Item3 : expectedVoucher);
            testData += $"\r\nDiscount Rank: {expectedTotal}";
            testData += $"\r\nTotal Paid: {expectedTotal}";

            if (delete) { expected = "Không tìm thấy dơn hàng trong Pending\r\nTìm thấy đơn hàng trong Cancelled"; }
            else { expected = "Còn đơn hàng trong Pending\r\nKhông tìm thấy dơn hàng trong Cancelled"; }
        }

        private string GetProductList(bool haveOrder)
        {
            string productFormat = "";
            if (!haveOrder) 
            { 
                expectedPreTotal = 0;

                for (int i = 0; i < productList.Length; i++)
                {
                    double productPrice = productList[i].Item2;
                    expectedPreTotal += productPrice;
                    productFormat += (i == 0 ? "" : "\r\n") + $"- {productList[i].Item1} - Số lượng: 1 - Giá: {productList[i].Item2}";
                }
            }
            else
            {
                for (int i = 0; i < orderProductList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(orderProductList[i].Item1))
                    {
                        productFormat += (i == 0 ? "" : "\r\n") + $"- {orderProductList[i].Item1} - Số lượng: {orderProductList[i].Item2} - Giá: {orderProductList[i].Item3}";
                    }
                }
            }

            return productFormat;
        }
      

        private void DeleteOrder(bool delete)
        {
            IList<IWebElement> orderList = webComponent.OrderList();
            webComponent.OrderList_CancelledButton(orderList[position]);
            if (delete) { driver.SwitchTo().Alert().Accept(); }
            else { driver.SwitchTo().Alert().Dismiss(); }
            Thread.Sleep(delayTime);
        }

        private (string,bool) CheckOrderDelete(bool delete)
        {
            string actual = "";
            bool result;

            checkResult = FindOrder();
            if (!string.IsNullOrEmpty(checkResult.Item1) && !checkResult.Item2) { return checkResult; }
            
            //Kiểm tra trang Pending
            //Không xoá
            if (!delete)
            {
                if (checkResult.Item2)
                {
                    actual += "Còn đơn hàng trong Pending";
                    result = true;
                }
                else
                {
                    actual += "Không tìm thấy dơn hàng trong Pending [Sai]";
                    result = false;
                }
            }
            //Xoá
            else
            {
                if (checkResult.Item2)
                {
                    actual += "Còn đơn hàng trong Pending [Sai]";
                    result = false;
                }
                else
                {
                    actual += "Không tìm thấy dơn hàng trong Pending";
                    result = true;
                }
            }

            //Kiểm tra trang Cancelled
            webComponent.Order_CancelledButton();
            Thread.Sleep(delayTime);
            checkResult = FindOrder();
            if (!string.IsNullOrEmpty(checkResult.Item1) && !checkResult.Item2) { return checkResult; }

            //Không xoá
            if (!delete)
            {
                if (checkResult.Item2)
                {
                    actual += "\r\nTìm thấy đơn hàng trong Cancelled [Sai]";
                    result = false;
                }
                else
                {
                    actual += "\r\nKhông tìm thấy dơn hàng trong Cancelled";
                    result = true;
                }
            }
            //Xoá
            else
            {
                if (checkResult.Item2)
                {
                    actual += "\r\nTìm thấy đơn hàng trong Cancelled";
                    result = true;
                }
                else
                {
                    actual += "\r\nKhông tìm thấy dơn hàng trong Cancelled [Sai]";
                    result = false;
                }
            }
            return (actual, result);
        }

        private (string,bool) FindOrder()
        {
            bool haveOrder = false;
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
                    int totalProduct = 0;
                    // Chi tiết đơn hàng
                    IList<IWebElement> orderDetail = webComponent.OrderList_OrderDetail(orderList[i]);
                    for (int j = 0; j < orderDetail.Count; j++)
                    {
                        string productName = webComponent.OrderList_OrderDetail_ProductName(orderDetail[j]);
                        string quantityValue = webComponent.OrderList_OrderDetail_Quantity(orderDetail[j]);
                        string priceValue = webComponent.OrderList_OrderDetail_Price(orderDetail[j]);

                        if (!int.TryParse(quantityValue, out int quantity)) { return ("Số lượng hàng hoá có chữ", false); }
                        if (!double.TryParse(priceValue, out double price)) { return ("Tổng giá trị sản phẩm có chữ", false); }

                        for (int k = 0; k < orderProductList.Length; k++)
                        {
                            if (productName == orderProductList[k].Item1 && quantity == orderProductList[k].Item2 && price == orderProductList[k].Item3)
                            {
                                totalProduct++;
                            }
                        }
                    }
                    if (totalProduct == orderProductList.Length)
                    {
                        string totalValue = webComponent.OrderList_Total(orderList[i]);
                        string voucherValue = webComponent.OrderList_Voucher(orderList[i]);
                        string discountRankValue = webComponent.OrderList_DiscountRank(orderList[i]);
                        string totalPaidValue = webComponent.OrderList_TotalPaid(orderList[i]);

                        if (!double.TryParse(totalValue, out double total)) { return ("Total có chữ", false); }
                        if (!double.TryParse(voucherValue, out double voucher)) { return ("Voucher có chữ", false); }
                        if (!double.TryParse(discountRankValue, out double discountRank)) { return ("Discount rank có chữ", false); }
                        if (!double.TryParse(totalPaidValue, out double totalPaid)) { return ("Total paid có chữ", false); }

                        if (total == expectedPreTotal && voucher == expectedVoucher && discountRank == expectedDiscountRank && totalPaid == expectedTotal)
                        {
                            haveOrder = true;
                        }
                    }
                }
            }
            return(null, haveOrder);
        }
    }
}
