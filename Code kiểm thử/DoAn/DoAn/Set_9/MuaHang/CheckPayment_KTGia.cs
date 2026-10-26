using DoAn.Setting;
using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using DoAn.Setting.Variable;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;
namespace DoAn.Set_9.MuaHang
{
    internal class CheckPayment_KTGia
    {
        private readonly int delayTime = Setting.Setting.Instance.DelayTime();
        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private readonly GetData getData = new GetData();
        private WebComponent webComponent;
        private Feature feature;
        IWebDriver driver;

        //Biến
        private string testData, expected, note, rank;
        private double expectedTotal, USDConverter;
        private (string, bool) checkResult;
        private (string, double)[] productList;
        private (string, string, double) voucherInfo;
        public (string, string, string) userInfo;
        [SetUp]
        public void Setup()
        {
            //Khởi tạo driver
            setting.Setup(out driver);
            feature = new Feature(driver);
            webComponent = new WebComponent(driver);

            //Lấy giá trị quy đổi
            setting.ConverterPage();
            USDConverter = webComponent.Google_ConverterValue();
            
            //Mở trang web
            setting.ProductPage();

            //Đăng nhập
            setting.Login(LoginType.Customer_NoInfo);
        }

        [Test]
        [TestCase("MH032", VoucherType.None)]
        [TestCase("MH033", VoucherType.Accepted)]
        [TestCase("MH034", VoucherType.OutDate)]
        [TestCase("MH035", VoucherType.Overload)]
        [TestCase("MH036", VoucherType.OutOfStock)]
        [TestCase("MH037", VoucherType.NotExist)]
        public void CheckPrice(string ID, VoucherType voucherType)
        {
            userInfo = randomData.GenerateInfo("BANK");
            productList = randomData.RandomProduct(1);
            Format_TestData_Expected(voucherType);

            //B1: Check giỏ hàng
            checkResult = feature.Check_ShoppingCart(productList);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //Lấy thông tin giỏ hàng
            rank = webComponent.Cart_RankName();
            Format_TestData_Expected(voucherType);

            //Nhập thông tin giao hàng
            checkResult = feature.DestinationChange(userInfo.Item1, userInfo.Item2, userInfo.Item3);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //Thêm khuyến mãi
            if (voucherType != VoucherType.None)
            {
                webComponent.Cart_VoucherInput(voucherInfo.Item2);
                webComponent.Cart_Voucher_AddButton();
                Thread.Sleep(delayTime);
            }

            //Nhấn nút "Check Payment" và kiểm tra thông tin
            checkResult = CheckPrice();
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }


        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }


        private void Format_TestData_Expected(VoucherType voucherType)
        {
            //Tạm tính
            double preTotal = 0;
            string productFormat = "";
            for (int i = 0; i < productList.Length; i++)
            {
                double productPrice = productList[i].Item2;
                preTotal += productPrice;
                productFormat += (i == 0 ? "" : "\r\n") + $"- {productList[i].Item1} - Số lượng: 1 - Giá: {productPrice}";
            }
            double discountRankPercent = getData.GetDiscountRank(string.IsNullOrEmpty(rank) ? "Đồng" : rank);
            double discountRank = preTotal * discountRankPercent;
            voucherInfo = getData.GetVoucher(voucherType, preTotal - discountRank);
            double voucherValue = voucherType != VoucherType.None ? voucherInfo.Item3 : 0;
            expectedTotal = (preTotal - discountRank + (voucherType == VoucherType.Accepted ? voucherValue : 0)) * USDConverter;

            //Test Data
            testData = $"* Phần giao tới có thông tin:\r\n- Name: {userInfo.Item1}\r\n- Phone: {userInfo.Item2}\r\n- Address: {userInfo.Item3}";
            testData += $"\r\n\r\n* Sản phẩm mua: \r\n" + productFormat;
            if (voucherType != VoucherType.None)  { testData += $"\r\n\r\n* Mã khuyến mãi sử dụng: {voucherInfo.Item2}"; }

            //Expected
            expected = $"Payment amount: {expectedTotal:###,###} VND";

            //Note:
            DateTime today = DateTime.Now;
            string formatDate = today.ToString("dd/MM/yyyy");
            note = $"*Giá trị quy đổi ngày {formatDate}: 1 USD = {USDConverter:###,###.##} VNĐ\r\n- {expectedTotal/USDConverter:###,###.##} USD = {expectedTotal:###,###.##} VNĐ.";
        }

        private (string, bool) CheckPrice()
        {
            try
            {
                webComponent.Cart_CheckPaymentButton();
                
                Thread.Sleep(delayTime);
                string actuaValue = webComponent.VNPAY_TotalMoney();

                if (!double.TryParse(actuaValue, out double actualPrice)) { return ("Payment amount chứa chữ", false); }

                string actual = $"Payment amount: {actualPrice:###,###} VND";
                bool result = true;
                if ( $"{expectedTotal.ToString():###,###}" != actualPrice.ToString())
                {
                    actual += " [Sai]";
                    result = false;
                }
                return (actual, result);
            }
            catch { return ("Lỗi giao dịch", false); }
        }
    }
}
