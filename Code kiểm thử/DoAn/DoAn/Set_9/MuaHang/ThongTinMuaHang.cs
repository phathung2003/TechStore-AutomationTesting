using DoAn.Setting;
using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using DoAn.Setting.Variable;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;

namespace DoAn.Set_9.MuaHang
{
    internal class ThongTinMuaHang
    {
        private readonly int delayTime = Setting.Setting.Instance.DelayTime();

        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private WebComponent webComponent;
        private Feature feature;
        IWebDriver driver;

        //Biến
        private string testData, expected, actual, status, notification;
        private (string, bool) checkResult;
        private (string, double)[] productList;

        [SetUp]
        public void Setup()
        {
            //Khởi tạo driver
            setting.Setup(out driver);

            //Mở trang web
            setting.ProductPage();

            feature = new Feature(driver);
            webComponent = new WebComponent(driver);
        }

        [Test]
        [TestCase("MH001", true)]
        [TestCase("MH002", false)]
        public void Transaction_WithoutInfo(string ID, bool checkOut)
        {
            expected = "Hiện thông báo lỗi với nội dung là: \r\nOops... \r\nYou dont have fill out your information to " + (checkOut ? "checkout" : "check payment");
            SetUpData(ID, null, null, null, true, LoginType.Customer_NoInfo);

            //B1: Nhấn nút "Checkout"
            webComponent.Cart_CheckoutButton();
            Thread.Sleep(delayTime);

            //B2: Đọc nội dung thông báo
            checkResult = GetNotification(out status, out notification);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B3: Kiểm tra thông báo lỗi
            actual = $"Hiện thông báo lỗi với nội dung là: \r\n{status} \r\n{notification}";
            bool result = true;
            if (status != "Oops..." || notification != "You dont have fill out your information to " + (checkOut ? "checkout" : "check payment"))
            {
                actual += " [Sai]";
                result = false;
            }
            setting.FinalResult(ID, testData, expected, null, (actual, result));
        }

        [Test]
        [TestCase("MH003","Hưng","09031122002","TP.Hồ Chí Minh")]
        [TestCase("MH004", null, "09031122002", "TP.Hồ Chí Minh")]
        [TestCase("MH005", "Hưng", null, "TP.Hồ Chí Minh")]
        [TestCase("MH006", "Hưng", "09031122002", null)]
        public void FillFullInfo_WithoutInfo(string ID, string name, string phone, string address)
        {
            actual = "";
            expected = Expected_FillInfo(name, phone, address);
            SetUpData(ID, name, phone, address, true, LoginType.Customer_NoInfo);

            //B1: Điền dữ liệu giao hàng
            checkResult = DestinationChange(name, phone, address);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B2: Kiểm tra dữ liệu
            bool result = true;
            bool missing = false;
            if (string.IsNullOrEmpty(name))
            {
                try
                {
                    string nameError = webComponent.Cart_ChangeInfo_NameError();
                    actual = $"Dưới ô nhập của Name báo lỗi:\r\n{nameError}";
                }
                catch
                {
                    actual = "Không báo lỗi thiếu Name hoặc thông báo lỗi sai!";
                    result = false;
                }
                missing = true;
            }
            if (string.IsNullOrEmpty(phone))
            {
                try
                {
                    string phoneError = webComponent.Cart_ChangeInfo_PhoneError();
                    actual += (missing ? "\r\n" : "") + $"Dưới ô nhập của Phone báo lỗi:\r\n{phoneError}";
                }
                catch
                {
                    actual += (missing ? "\r\n" : "") + "Không báo lỗi thiếu Phone hoặc thông báo lỗi sai!";
                    result = false;
                }
                missing = true;
            }
            if (string.IsNullOrEmpty(address))
            {
                try
                {
                    string addressError = webComponent.Cart_ChangeInfo_AddressError();
                    actual += (missing ? "\r\n" : "") + $"Dưới ô nhập của Addresss báo lỗi:\r\n{addressError}";
                }
                catch
                {
                    actual += (missing ? "\r\n" : "") + "Không báo lỗi thiếu Address hoặc thông báo lỗi sai!";
                    result = false;
                }
                missing = true;
            }
            if(!missing && result)
            {
                string orderName = webComponent.Cart_Info_Name();
                string orderPhone = webComponent.Cart_Info_Phone();
                string orderAddress = webComponent.Cart_Info_Address();

                //Kiểm tra Name
                actual = $"Phần giao tới hiện nội dung sau:\r\n- Name: {orderName}";
                if (orderName != name)
                {
                    actual += " [Sai]";
                    result = false;
                }

                //Kiểm tra Phone
                actual += $"\r\n- Phone: {orderPhone}";
                if (orderPhone != phone)
                {
                    actual += " [Sai]";
                    result = false;
                }

                //Kiểm tra Address
                actual += $"\r\n- Address: {orderAddress}";
                if (orderAddress != address)
                {
                    actual += " [Sai]";
                    result = false;
                }
            }
            setting.FinalResult(ID, testData, expected, null, (actual, result));
        }

        [Test]
        [TestCase("MH007", "Hưng", "09031122002", "TP.Hồ Chí Minh")]
        public void SaveChange_WithoutInfo(string ID, string name, string phone, string address)
        {
            expected = $"Phần chỉnh sửa thông tin có những nội dung sau: \r\n- Name: {name} \r\n- Phone: {phone}\r\n- Address: {address}";
            SetUpData(ID, name, phone, address, true, LoginType.Customer_NoInfo);

            //B1: Thực hiện điền dữ liệu
            checkResult = DestinationChange(name, phone, address);
            setting.CheckFailed(ID, testData, expected, null, checkResult);

            //B2: Lấy thông tin giao hàng
            string orderName = webComponent.Cart_Info_Name();
            string orderPhone = webComponent.Cart_Info_Phone();
            string orderAddress = webComponent.Cart_Info_Address();

            //B3: Vào lại Chỉnh sửa lấy thông tin
            webComponent.Cart_ChangeInfo();
            string inputName = webComponent.Cart_ChangeInfo_NameInput().GetAttribute("value");
            string inputPhone = webComponent.Cart_ChangeInfo_PhoneInput().GetAttribute("value");
            string inputAddress = webComponent.Cart_ChangeInfo_AddressInput().GetAttribute("value");

            //B4: Kiểm tra thông tin
            checkResult = CheckSaveChange(orderName, orderPhone, orderAddress, inputName, inputPhone, inputAddress);
            setting.FinalResult(ID, testData, expected, null, checkResult);
        }

        [Test]
        [TestCase("MH008")]
        public void CheckInfo_HaveInfo(string ID)
        {
            string expected = "Thông tin người mua hàng với thông tin cá nhân trùng khớp";
            SetUpData(ID, null, null, null, true, LoginType.Customer_HaveInfo);

            //B1: Lấy thông tin trong giỏ hàng
            string orderName = webComponent.Cart_Info_Name();
            string orderPhone = webComponent.Cart_Info_Phone();
            string orderAddress = webComponent.Cart_Info_Address();

            //B2: Lấy thông tin trong profile
            webComponent.Account_Icon();
            webComponent.UserInfoButton();
            string name = webComponent.UserInfo_NameInput().GetAttribute("value");
            string phone = webComponent.UserInfo_PhoneInput().GetAttribute("value");
            string address = webComponent.UserInfo_AddressInput().GetAttribute("value");

            string actual = "";
            bool result = true;
            if(name != orderName)
            {
                actual = $"- Name ghi sai - {name} nhưng lại ghi {orderName} [Sai]";
                result = false ;
            }
            if(phone != orderPhone)
            {
                actual += (!result ? $"\r\n" : "") + $"- Phone ghi sai - {phone} nhưng lại ghi {orderPhone} [Sai]";
                result = false;
            }
            if(address != orderAddress)
            {
                actual += (!result ? $"\r\n" : "") + $"- Address ghi sai - {address} nhưng lại ghi {orderAddress} [Sai]";
            }
            if(result)
            {
                actual = "Thông tin người mua hàng với thông tin cá nhân trùng khớp";
            }
            setting.FinalResult(ID, testData, expected, null, (actual, result));
        }

        [Test]
        [TestCase("MH009")]
        public void SaveChange_HaveInfo(string ID)
        {
            SetUpData(ID, null, null, null, false, LoginType.Customer_HaveInfo);

            //B1: Lấy thông tin Profile
            webComponent.Account_Icon();
            webComponent.UserInfoButton();
            string name = webComponent.UserInfo_NameInput().GetAttribute("value");
            string phone = webComponent.UserInfo_PhoneInput().GetAttribute("value");
            string address = webComponent.UserInfo_AddressInput().GetAttribute("value");

            expected = $"Phần chỉnh sửa thông tin có những nội dung sau: \r\n- Name: {name} \r\n- Phone: {phone}\r\n- Address: {address}";

            //B2: Check giỏ hàng
            checkResult = feature.Check_ShoppingCart(productList);
            setting.CheckFailed(ID, testData, expected, null, checkResult);


            //B3: Vào phần điều chỉnh thông tin
            webComponent.Cart_ChangeInfo();
            string inputName = webComponent.Cart_ChangeInfo_NameInput().GetAttribute("value");
            string inputPhone = webComponent.Cart_ChangeInfo_PhoneInput().GetAttribute("value");
            string inputAddress = webComponent.Cart_ChangeInfo_AddressInput().GetAttribute("value");

            //B4: Kiểm tra thông tin
            checkResult = CheckSaveChange(name, phone, address, inputName, inputPhone, inputAddress);
            setting.FinalResult(ID, testData, expected, null, checkResult);
        }

        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }

        //Format DataTest
        public string DataTestProduct(string name, string phone, string address)
        {
            string testData = "Sản phẩm mua gồm: ";

            for (int i = 0; i < productList.Length; i++)
            {
                testData += $"\r\n- {productList[i].Item1}";
            }

            int missing = 0;
            if (string.IsNullOrEmpty(name))     missing++;
            if (string.IsNullOrEmpty(phone))    missing++;
            if (string.IsNullOrEmpty(address))  missing++;

            if(missing < 3)
            {
                testData += $"\r\n\r\nThông tin người mua hàng:";
                if (!string.IsNullOrEmpty(name))    testData += $"\r\n- Name: {name}";
                if (!string.IsNullOrEmpty(phone))   testData += $"\r\n- Phone: {phone}";
                if (!string.IsNullOrEmpty(address)) testData += $"\r\n- Address: {address}";
            }
            return testData;
        }

        //Expected - Điền thông tin
        private string Expected_FillInfo(string name, string phone, string address)
        {
            string expected = "";
            bool missing = false;

            if (string.IsNullOrEmpty(name))
            {
                expected += "Dưới ô nhập của Name báo lỗi:\r\nThe nick name field is required.";
                missing = true;
            }
            if (string.IsNullOrEmpty(phone))
            {
                expected += (missing ? "\r\n" : "") + "Dưới ô nhập của Phone báo lỗi:\r\nThe phone field is required.";
                missing = true;
            }
            if (string.IsNullOrEmpty(address))
            {
                expected += (missing ? "\r\n" : "") + "Dưới ô nhập của Address báo lỗi:\r\nThe address field is required.";
                missing = true;
            } 
            if(!missing)
            {
                expected = $"Phần giao tới hiện nội dung sau: \r\n- Name: {name} \r\n- Phone: {phone}\r\n- Address: {address}";
            }
            return expected;
        }

        //Nhập dữ liệu
        private (string,bool) DestinationChange(string name, string phone, string address)
        {
            try
            {
                webComponent.Cart_ChangeInfo();

                //Nhập Name
                webComponent.Cart_ChangeInfo_NameInput().Clear();
                if (!string.IsNullOrEmpty(name)) { webComponent.Cart_ChangeInfo_NameInput().SendKeys(name); }

                //Nhập Phone
                webComponent.Cart_ChangeInfo_PhoneInput().Clear();
                if (!string.IsNullOrEmpty(phone)) { webComponent.Cart_ChangeInfo_PhoneInput().SendKeys(phone); }

                //Nhập Addresss
                webComponent.Cart_ChangeInfo_AddressInput().Clear();
                if (!string.IsNullOrEmpty(address)) { webComponent.Cart_ChangeInfo_AddressInput().SendKeys(address); }

                //Nhấn nút xác nhận
                webComponent.Cart_ChangeInfo_SubmitButton();
                return (null, true);
            }
            catch
            {
                string actual = "Gặp lỗi trong quá trình thực hiện điền dữ liệu.";
                return (actual, false);
            }
        }

        //Kiểm tra dữ liệu đã lưu
        private (string, bool) CheckSaveChange(string name, string phone, string address, string inputName, string inputPhone, string inputAddress)
        {
            bool result = true;
            string actual = "Phần chỉnh sửa thông tin có những nội dung sau:";
            if (string.IsNullOrEmpty(inputName) || inputName != name)
            {
                actual += string.IsNullOrEmpty(inputName) ? "\r\n- Name không được lưu lại" : $"\r\n- Name: {name} [Sai - Phải là: {inputName}]";
                result = false;
            }
            else { actual += $"\r\n- Name: {inputName}"; }


            if (string.IsNullOrEmpty(inputPhone) || inputPhone != phone)
            {
                actual += string.IsNullOrEmpty(inputPhone) ? "\r\n- Phone không được lưu lại" : $"\r\n- Phone: {phone} [Sai - Phải là: {inputPhone}]]";
                result = false;
            }
            else { actual += $"\r\n- Phone: {inputPhone}"; }


            if (string.IsNullOrEmpty(inputAddress) || inputAddress != address)
            {
                actual += string.IsNullOrEmpty(inputAddress) ? "\r\n- Address không được lưu lại" : $"\r\n- Address: {address} [Sai - {inputAddress}]";
                result = false;
            }
            else { actual += $"\r\n- Address: {inputAddress}"; }
            return (actual,result);
        }
        
        //Kiểm tra nội dung thông báo
        public (string, bool) GetNotification(out string status, out string notification)
        {
            status = null;
            notification = null;
            int restartNo = 0;
            while (true)
            {
                // Kiểm tra xem phần tử đã xuất hiện hay chưa
                try
                {
                    status = webComponent.Cart_Checkout_NotificationStatus();
                    notification = webComponent.Cart_Checkout_NotificationMessage();
                    return (null, true);
                }
                catch
                {
                    if (restartNo > 5) { break; }
                    System.Threading.Thread.Sleep(1000);
                    restartNo++;
                }
            }
            string actual = "Không hiện hoặc hiện quá lâu ô thông báo xác nhận.";
            return (actual, false);
        }

        private void SetUpData(string ID, string name, string phone, string address, bool checkCart, LoginType loginType)
        {
            productList = randomData.RandomProduct(1);
            testData = DataTestProduct(name, phone, address);

            //B1: Đăng nhập
            setting.Login(loginType);

            //B2: Check giỏ hàng
            if (checkCart)
            {
                checkResult = feature.Check_ShoppingCart(productList);
                setting.CheckFailed(ID, testData, expected, null, checkResult);
            }
        }
    }
}
