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
    internal class CheckPayment
    {
        private readonly int delayTime = Setting.Setting.Instance.DelayTime();

        //Thành phần
        private readonly SetUp setting = new SetUp(1);
        private CatchError catchError;
        private Feature feature;
        private WebComponent webComponent;

        //Dữ liệu
        private readonly RandomData randomData = new RandomData();
        private readonly GetData getData = new GetData();

        //Biến
        private string testData, expected, note, rank;
        private double expectedPreTotal, expectedTotal, expectedDiscountRank;
        private (string, bool) checkResult;
        private (string, string, double) discountInfo;
        private (string, string, string, string, string, string) bankAccountInfo;
        private (string, string, string) userInfo;
        private (string, double)[] productList;
        private VoucherType voucherType;
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
            catchError = new CatchError(driver);
            feature = new Feature(driver);
        }

        [Test]
        [TestCase("MH038", CardType.Success, false,false)]
        [TestCase("MH039", CardType.Success, true, false)]
        [TestCase("MH040", CardType.Success, true, true)]
        [TestCase("MH041", CardType.WrongCard, false, false)]
        [TestCase("MH042", CardType.InsufficientBalance, false, false)]
        [TestCase("MH043", CardType.NotActive, false, false)]
        [TestCase("MH044", CardType.Expried, false, false)]
        [TestCase("MH045", CardType.Locked, false, false)]
        [TestCase("MH046", CardType.None, false, false)]
        [TestCase("MH047", CardType.WrongCard, false, false)]
        public void Transaction_RightCard(string ID, CardType type, bool randomOTP,bool ThreeTime)
        {
            this.voucherType = randomData.RandomVoucherType();
            this.productList = randomData.RandomProduct(1);
            this.userInfo = randomData.GenerateInfo("BANK");
            this.bankAccountInfo = getData.GetBankAccount(type);
            Format_TestData_Expected(type);

            //B1: Kiểm tra giỏ hàng
            checkResult = feature.Check_ShoppingCart(productList);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B2: Lấy thông tin giỏ hàng
            rank = webComponent.Cart_Rank();
            Format_TestData_Expected(type);

            //B3: Nhập thông tin giao hàng
            checkResult = feature.DestinationChange(userInfo.Item1, userInfo.Item2, userInfo.Item3);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B4: Thêm khuyến mãi (Nếu có)
            if (voucherType != VoucherType.None) { feature.InputVoucher(discountInfo.Item2); }

            //B5: Nhấn nút "Check Payment" và kiểm tra thông tin
            checkResult = Transaction(type, randomOTP, ThreeTime);
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }


        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }

        private void Format_TestData_Expected(CardType cardType)
        {
            //Lấy danh sách sản phẩm
            string productFormat = GetProductList();
            double discountRankPercent = getData.GetDiscountRank(string.IsNullOrEmpty(rank) ? "Đồng" : rank);
            double discountRank = expectedPreTotal * discountRankPercent;
            discountInfo = voucherType != VoucherType.None ? getData.GetVoucher(voucherType, (this.expectedPreTotal - discountRank)) : (null, null, 0);
            expectedTotal = (expectedPreTotal - discountRank + (voucherType == VoucherType.Accepted ? discountInfo.Item3 : 0));

            //Test Data
            testData = $"* Phần giao tới có thông tin:\r\n" +
                       $"- Name: {userInfo.Item1}\r\n" +
                       $"- Phone: {userInfo.Item2}\r\n" +
                       $"- Address: {userInfo.Item3}";
            testData += $"\r\n\r\n* Sản phẩm mua: \r\n" + productFormat;
            if (voucherType != VoucherType.None)
            {
                testData += $"\r\n\r\n* Mã khuyến mãi sử dụng: {discountInfo.Item2}";
            }
            if(cardType != CardType.None)
            {
                testData += $"\r\n\r\n* Thông tin thẻ thanh toán\r\n" +
                            $"- Số thẻ: {bankAccountInfo.Item1}\r\n" +
                            $"- Tên chủ thẻ: {bankAccountInfo.Item2}\r\n" +
                            $"- Ngày phát hành thẻ: {bankAccountInfo.Item3}";
                if (string.IsNullOrEmpty(bankAccountInfo.Item4))
                {
                    testData += $"\r\n- Mã OTP: {bankAccountInfo.Item4}";
                }
            }

            //Expected
            this.expected = (cardType == CardType.Success ? "Có" : "Không có") + " đơn hàng với thông tin như sau:";
            this.expected += $"\r\n- Name: {userInfo.Item1}" +
                             $"\r\n- Phone: {userInfo.Item2}" +
                             $"\r\n- Address: {userInfo.Item3}";
            this.expected += "\r\n\r\nChi tiết đơn hàng có:\r\n" + productFormat;
            this.expected += $"\r\n\r\nTotal: {this.expectedPreTotal}";
            this.expected += "\r\nVoucher: " + (voucherType == VoucherType.Accepted ? -discountInfo.Item3 : 0);
            this.expected += $"\r\nDiscount Rank: {this.expectedDiscountRank}";
            this.expected += $"\r\nTotal paid: {this.expectedTotal}";

            //Note:
            note = cardType != CardType.Success ? $"Thông báo lỗi trên trang VNPAY là: {bankAccountInfo.Item6}" : "";

        }
        private string GetProductList()
        {
            //Danh sách sản phẩm
            this.expectedPreTotal = 0;
            string productFormat = "";

            for (int i = 0; i < productList.Length; i++)
            {
                double productPrice = productList[i].Item2;
                this.expectedPreTotal += productPrice;
                productFormat += (i == 0 ? "" : "\r\n") + $"- {productList[i].Item1} - Số lượng: 1 - Giá: {productPrice}";
            }
            return productFormat;
        }
        private (string, bool) Transaction(CardType type, bool RandomOTP, bool ThreeTimes)
        {
            //Vào trang thanh toán
            try { webComponent.Cart_CheckPaymentButton(); }
            catch { return ("Không vào được trang thanh toán", false); }

            try 
            {
                //Nhập thông tin thẻ
                if(type != CardType.None)
                {
                    webComponent.VNPAY_CardNumber(bankAccountInfo.Item1);
                    webComponent.VNPAY_CardHolder(bankAccountInfo.Item2);
                    webComponent.VNPAY_IssuingDate(bankAccountInfo.Item3);
                }

                webComponent.VNPAY_CardInfo_ContinueButton();
                Thread.Sleep(delayTime);
                if(type != CardType.None)
                {
                    webComponent.VNPAY_AgreeButton();
                    Thread.Sleep(delayTime);
                }


                bool access = true;
                try
                {
                    //Xác nhận OTP
                    string OTP = RandomOTP ? randomData.RandomOTP() : bankAccountInfo.Item4;
                    webComponent.VNPAY_OTP(OTP);
                    webComponent.VNPAY_OTP_ConfirmButton();
                    Thread.Sleep(delayTime);

                    if (RandomOTP)
                    {
                        checkResult = WrongOTP(ThreeTimes);
                        if (!checkResult.Item2) { return checkResult; }
                    }
                }
                catch { access = false; }

                //Nếu như thẻ không hợp lệ
                if(access && type != CardType.Success) { return ("Không hiện báo lỗi", false); }
                if(!access)
                {
                    if(type == CardType.Success) { return TransactionError(); }
                    checkResult = WrongCardInfo(ThreeTimes);
                    if (!checkResult.Item2) { return checkResult; }
                }
            }
            catch { return TransactionError(); }

            //Xác nhận giỏ hàng
            try { return GetPendingList(discountInfo.Item3); }
            catch
            {
                checkResult = catchError.ProgramErrorPage();
                if (checkResult.Item2) { return (checkResult.Item1, false); }

                checkResult = catchError.ErrorCode();
                if (checkResult.Item2) { return (checkResult.Item1, false); }

                return ("Gặp lỗi không xác định.", false);
            }
        }
        private (string, bool) WrongOTP(bool ThreeTimes)
        {
            if (catchError.VNPAYError().Item2)
            {
                if (ThreeTimes)
                {
                    for(int i = 0; i < 2; i++)
                    {
                        webComponent.VNPAY_OTP_ConfirmButton();
                    }
                }
                Thread.Sleep(delayTime);
                webComponent.VNPAY_OTP_CancelButton();
                Thread.Sleep(delayTime);
                webComponent.VNPAY_Cancel_ConfirmButton();
                Thread.Sleep(delayTime);
                return (null, true);
            }
            else { return ("Không hiện báo lỗi", false); }
        }
        private (string, bool) WrongCardInfo(bool ThreeTimes)
        {
            if (catchError.VNPAYError().Item2)
            {
                if (ThreeTimes)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        try { webComponent.VNPAY_OTP_ConfirmButton(); }
                        catch { webComponent.VNPAY_CardInfo_ContinueButton(); }
                    }
                }
                Thread.Sleep(delayTime);
                webComponent.VNPAY_CardInfo_CancelButton();
                Thread.Sleep(delayTime);
                webComponent.VNPAY_Cancel_ConfirmButton();
                Thread.Sleep(delayTime);
                return (null, true);
            }
            else { return ("Không hiện báo lỗi", false); }
        }
        private (string,bool) TransactionError()
        {
            checkResult = catchError.VNPAYError();
            if (!string.IsNullOrEmpty(checkResult.Item1)) { return (checkResult.Item1, false); }

            return ("Lỗi giao dịch không xác định.", false);
        }
        private (string, bool) GetPendingList(double expectedVoucher)
        {
            int rightProduct = 0;
            bool result = true;
            (string, bool) checkResult;
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
                    IList<IWebElement> orderDetail = webComponent.OrderList_OrderDetail(orderList[i]);
                    for (int j = 0; j < orderDetail.Count; j++)
                    {
                        string productName = webComponent.OrderList_OrderDetail_ProductName(orderDetail[j]);
                        string quantityValue = webComponent.OrderList_OrderDetail_Quantity(orderDetail[j]);
                        string priceValue = webComponent.OrderList_OrderDetail_Price(orderDetail[j]);


                        if (!int.TryParse(quantityValue, out int quantity)) { return ("Số lượng hàng hoá có chữ", false); }
                        if (!double.TryParse(priceValue, out double price)) { return ("Tổng giá trị sản phẩm có chữ", false); }

                        checkResult = ProductCheck(productName, quantity, price);
                        actual += checkResult.Item1;
                        if (checkResult.Item2)
                        {
                            rightProduct++;
                        }
                    }
                    if (rightProduct != this.productList.Length)
                    {
                        actual += "\r\n[Không đủ số lượng sản phẩm]";
                    }

                    string totalValue = webComponent.OrderList_Total(orderList[i]);
                    string voucherValue = webComponent.OrderList_Voucher(orderList[i]);
                    string discountRankValue = webComponent.OrderList_DiscountRank(orderList[i]);
                    string totalPaidValue = webComponent.OrderList_TotalPaid(orderList[i]);

                    if (!double.TryParse(totalValue, out double total)) { return ("Total có chữ", false); }
                    if (!double.TryParse(voucherValue, out double voucher)) { return ("Voucher có chữ", false); }
                    if (!double.TryParse(discountRankValue, out double discountRank)) { return ("Discount rank có chữ", false); }
                    if (!double.TryParse(totalPaidValue, out double totalPaid)) { return ("Total paid có chữ", false); }

                    checkResult = TotalCheck(expectedPreTotal, expectedVoucher, expectedDiscountRank, expectedTotal, total, voucher, discountRank, totalPaid);
                    actual += checkResult.Item1;
                    result = checkResult.Item2;

                    if (rightProduct != this.productList.Length) {result = false;}
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
        private (string, bool) ProductCheck(string productName, int quantity, double price)
        {
            bool productNameResult = false;
            bool quantityResult = false;
            bool priceResult = false;

            for (int i = 0; i < productList.Length; i++)
            {
                if (productList[i].Item1 == productName)
                {
                    productNameResult = true;
                    if (quantity == 1) { quantityResult = true; }
                    if (productList[i].Item2 == price) { priceResult = true; }
                    break;
                }
            }

            string actual = "\r\n" + $"- {productName}" + (!productNameResult ? " [Sai] " : " ")
                                   + $"- Số lượng: {quantity}" + (!quantityResult ? " [Sai] " : " ")
                                   + $"- Giá: {price}" + (!priceResult ? " [Sai] " : "");
            if (productNameResult && quantityResult && priceResult) { return (actual, true); }
            return (actual, false);
        }
        private (string, bool) TotalCheck(double expectedPreTotal, double expectedVoucher, double expectedDiscountRank, double expectedTotal, double actualPreTotal, double actualVoucher, double actualDiscountRank, double actualTotal)
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
