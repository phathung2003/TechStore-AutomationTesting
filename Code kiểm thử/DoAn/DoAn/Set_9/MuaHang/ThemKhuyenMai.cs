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
    internal class ThemKhuyenMai
    {
        private readonly int delayTime = Setting.Setting.Instance.DelayTime();

        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private readonly GetData getData = new GetData();
        private WebComponent webComponent;
        private Feature feature;
        IWebDriver driver;

        //Biến
        private string testData, expected,note, rank;
        private double expectedPreTotal, expectedTotal;
        private (string, bool) checkResult;
        private (string, string, double) voucherInfo;
        (string, double)[] productList;

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
        [TestCase("MH016", VoucherType.Accepted)]
        [TestCase("MH017", VoucherType.OutDate)]
        [TestCase("MH018", VoucherType.Overload)]
        [TestCase("MH019", VoucherType.OutOfStock)]
        [TestCase("MH020", VoucherType.NotExist)]
        public void AddDiscount(string ID, VoucherType voucherType)
        {
            productList = randomData.RandomProduct(1);
            Format_TestData_Expected(voucherType, false);

            //B1: Check giỏ hàng
            checkResult = feature.Check_ShoppingCart(productList);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B2: Lấy thông tin rank
            rank = webComponent.Cart_RankName();
            Format_TestData_Expected(voucherType, false);

            //B3: Thêm khuyến mãi
            webComponent.Cart_VoucherInput(voucherInfo.Item2);
            webComponent.Cart_Voucher_AddButton();
            Thread.Sleep(delayTime);
            
            //B4: Kiểm tra thông tin
            checkResult = CheckInfo(voucherType);
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }

        [Test]
        [TestCase("MH021", VoucherType.Accepted)]
        [TestCase("MH022", VoucherType.OutDate)]
        [TestCase("MH023", VoucherType.Overload)]
        [TestCase("MH024", VoucherType.OutOfStock)]
        public void ChooseDiscount(string ID, VoucherType voucherType)
        {
            string actual = "";
            productList = randomData.RandomProduct(1);
            Format_TestData_Expected(voucherType,true);

            //B1: Check giỏ hàng
            checkResult = feature.Check_ShoppingCart(productList);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B2: Lấy thông tin rank
            rank = webComponent.Cart_RankName();
            Format_TestData_Expected(voucherType, true);

            //Chọn khuyến mãi
            bool result = true;
            checkResult = SelectVoucher(voucherInfo.Item1);
            if (voucherType != VoucherType.Accepted && voucherType != VoucherType.Overload)
            {
                if (checkResult.Item2) { result = false; }
                else { actual = "Không có voucher để chọn\r\n\r\n"; }
            }
            else { setting.CheckFailed(ID, testData, expected, note, checkResult); }

            //B4: Kiểm tra thông tin
            checkResult = CheckInfo(voucherType);
            actual += checkResult.Item1;
            setting.FinalResult(ID, testData, expected, note, (actual, result ? checkResult.Item2 : result));
        }


        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }


        private void Format_TestData_Expected(VoucherType voucherType, bool selectVoucher)
        {
            expectedPreTotal = 0;
            string productFormat = "";
            for(int i = 0; i < productList.Length; i++)
            {
                expectedPreTotal += productList[i].Item2;
                productFormat += (i == 0 ? "" : "\r\n") + $"- {productList[i]}";
            }
            double discountRankPercent = getData.GetDiscountRank(string.IsNullOrEmpty(rank) ? "Đồng" : rank);
            double discountRank = expectedPreTotal * discountRankPercent;
            string rankString = string.IsNullOrEmpty(rank) ? "Hạng Đồng: " : $"Hạng {rank}: " + $"{-discountRank}";
            voucherInfo = getData.GetVoucher(voucherType, expectedPreTotal - discountRank);

            //Test Data
            testData = $"* Mã khuyến mãi sử dụng: {voucherInfo.Item2}\r\n\r\n" +
                       $"* Sản phẩm mua:\r\n" + productFormat;

            //Expected
            expected = "";
            if (selectVoucher && (voucherType != VoucherType.Accepted && voucherType != VoucherType.Overload))
            {
                expected += "Không có voucher để chọn\r\n\r\n";
            }
            expected += "Phần thanh toán hiển thị như sau:";
            expected += $"\r\nTạm tính: {expectedPreTotal}";
            expected += $"\r\n{rankString}";

            if(voucherType == VoucherType.Accepted)
            {
                expectedTotal = expectedPreTotal - discountRank + voucherInfo.Item3;
                expected += $"\r\nGiảm giá: {voucherInfo.Item3}";
                expected += $"\r\nVoucher: {voucherInfo.Item1}";
                expected += $"\r\nTổng tiền: {expectedTotal}";
            }
            else
            {
                expectedTotal = expectedPreTotal - discountRank;
                expected += $"\r\nGiảm giá: 0";
                expected += $"\r\nTổng tiền: {expectedTotal}";
            }

            //Note
            note = "";
            switch (voucherType)
            {
                case VoucherType.OutDate:
                    note = "Mã hết hạn không được sử dụng";
                    break;
                case VoucherType.Overload:
                    note = "Mã giảm quá tiền sản phẩm\r\n" +
                           "--> Chủ shop phải trả tiền cho khách hàng (Không hợp lý)\r\n" +
                           "--> Mã không được sử dụng";
                    break;
                case VoucherType.OutOfStock:
                    note = "Hết mã để sử dụng";
                    break;
                default:
                    note = "";
                    break;
            }
        }

        private (string,bool) CheckInfo(VoucherType voucherType)
        {
            bool result = true;
            string actual;
            double discountPercentage = getData.GetDiscountRank(rank);

            string preTotalValue = webComponent.Cart_PreTotal();
            if(!double.TryParse(preTotalValue, out double actualPreTotal))
            {
                actual = "Tạm tính có chữ";
                return (actual, false);
            }

            string rankDiscountValue = webComponent.Cart_RankDiscount();
            if(!double.TryParse(rankDiscountValue, out double acutalRankDiscount))
            {
                actual = "Hạng giảm giá có chữ";
                return (actual, false);
            }

            string voucherValue = webComponent.Cart_VoucherDiscount();
            if (!double.TryParse(voucherValue, out double actualVoucherDiscount))
            {
                actual = "Giảm giá có chữ";
                return (actual, false);
            }

            string totalValue = webComponent.Cart_Total();
            if(!double.TryParse (totalValue, out double actualTotal))
            {
                actual = "Tổng tiền có chữ";
                return (actual, false);
            }

            bool haveVoucherName = false;
            string discountName = "";
            try
            {
                discountName = webComponent.Cart_VoucherName();
                haveVoucherName = true;
            }
            catch { }
            

            actual = "Phần thanh toán hiển thị như sau:";
            //Kiểm tra tạm tính
            actual += $"\r\nTạm tính: {actualPreTotal}";
            if(expectedPreTotal != actualPreTotal)
            {
                actual += $" [Sai - Phải là {expectedPreTotal}]";
                result = false;
            }

            //Kiểm tra hạng thành viên
            actual += $"\r\nHạng {rank}: {acutalRankDiscount}";
            if(acutalRankDiscount.ToString() != (expectedPreTotal * -discountPercentage).ToString())
            {
                actual += $" [Sai - Phải là -{expectedPreTotal * discountPercentage}]";
                result = false;
            }

            //Kiểm tra giảm giá
            actual += $"\r\nGiảm giá: {actualVoucherDiscount}";
            if (voucherType == VoucherType.Accepted)
            {
                if(actualVoucherDiscount != voucherInfo.Item3)
                {
                    actual += $" [Sai - Phải là {voucherInfo.Item3}]";
                    result = false;
                }

                //Kiểm tra tên voucher
                actual += $"\r\nVoucher: {discountName}";
                if(discountName != voucherInfo.Item1)
                {
                    actual += $" [Sai - Phải là {voucherInfo.Item1}]";
                    result = false;
                }
            }
            else
            {
                if(actualVoucherDiscount != 0)
                {
                    actual += $" [Sai - Phải là 0]";
                    result = false;
                }
                if (haveVoucherName)
                {
                    actual += $"\r\nVoucher: {discountName} [Sai - Phải là không có voucher]";
                    result = false;
                }
            }

            actual += $"\r\nTổng tiền: {actualTotal}";
            if(actualTotal.ToString() != expectedTotal.ToString())
            {
                actual += $" [Sai - Phải là {expectedTotal}]";
                result = false;
            }
            return (actual, result);
        }

        private (string,bool) SelectVoucher(string targetVoucherName)
        {
            webComponent.Cart_SelectYourVoucher();
            IList<IWebElement> voucherList = webComponent.Cart_SelectYourVoucher_AllVoucher();

            for (int i = 0; i < voucherList.Count; i++)
            {
                string voucherName = webComponent.Cart_SelectYourVoucher_VoucherName(voucherList[i]);
                if (voucherName == targetVoucherName) 
                {
                    webComponent.Cart_SelectYourVoucher_AddButton(voucherList[i]);
                    return (null, true);
                }
            }

            webComponent.Cart_SelectYourVoucher_CloseButton();
            string acutal = "Không tìm thấy voucher.";
            return(acutal, false);
        }

    }
}
