using DoAn.Setting;
using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using DoAn.Setting.Variable;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace DoAn.Set_9.ThemGioHang
{
    internal class ThemGioHang
    {
        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private WebComponent webComponent;
        private Feature feature;
        private CatchError catchError;
        IWebDriver driver;

        //Biến
        private int before;
        string expected, testData, note;
        (string, bool) checkResult;
        (string, double)[] productList;
        (string, int, double, bool)[] product_Before_InShoppingCart, product_After_InShoppingCart;

        [SetUp]
        public void Setup()
        {
            //Khởi tạo driver
            setting.Setup(out driver);

            //Mở trang web
            setting.ProductPage();

            feature = new Feature(driver);
            webComponent = new WebComponent(driver);
            catchError = new CatchError(driver);
        }

        [Test]
        [TestCase("TGH001", LoginType.Customer_NoInfo, 1)]
        [TestCase("TGH002", LoginType.Customer_NoInfo, 3)]
        [TestCase("TGH014", LoginType.Admin, 1)]
        [TestCase("TGH015", LoginType.Admin, 3)]
        public void AddCart_Home(string ID, LoginType loginType, int numberOfProduct)
        {
            SetUpData(numberOfProduct, false);

            //B1: Đăng nhập
            setting.Login(loginType);

            //B2: Kiểm tra giỏ hàng trước khi thêm
            checkResult = CheckCart(out product_Before_InShoppingCart,out before);
            setting.CheckFailed(ID, testData, expected, note, checkResult);
            
            //B3: Thêm sản phẩm
            for (int i = 0; i < productList.Length; i++)
            {
                checkResult = feature.AddProduct_HomePage(productList[i].Item1);
                setting.CheckFailed(ID, testData, expected, note, checkResult);
            }

            //B4: Kiểm tra giỏ hàng sau khi thêm
            checkResult = CheckCartAfterAdd();
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }

        [Test]
        [TestCase("TGH003", LoginType.Customer_NoInfo, 1, false)]
        [TestCase("TGH004", LoginType.Customer_NoInfo, 3, false)]
        [TestCase("TGH005", LoginType.Customer_NoInfo, 1, true)]
        [TestCase("TGH006", LoginType.Customer_NoInfo, 3, true)]
        [TestCase("TGH016", LoginType.Admin, 1, false)]
        [TestCase("TGH017", LoginType.Admin, 3, false)]
        [TestCase("TGH018", LoginType.Admin, 1, true)]
        [TestCase("TGH019", LoginType.Admin, 1, true)]
        public void AddCart_Shop(string ID, LoginType loginType, int numberOfProduct, bool addCart)
        {
            SetUpData(numberOfProduct, false);
            
            //B1: Đăng nhập
            setting.Login(loginType);

            //B2: Kiểm tra giỏ hàng trước khi thêm
            checkResult = CheckCart(out product_Before_InShoppingCart, out before);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B3: Thêm sản phẩm
            for (int i = 0; i < productList.Length; i++)
            {
                //addCart = true: Ấn nút Add Card | addCart = false: Ấn nút Buy Now
                checkResult = feature.FindProduct_ShopPage(productList[i].Item1, false, addCart, true, false);
                setting.CheckFailed(ID, testData, expected, note, checkResult);
            }

            //B4: Kiểm tra giỏ hàng sau khi thêm
            checkResult = CheckCartAfterAdd();
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }


        [Test]
        [TestCase("TGH007", LoginType.Customer_NoInfo, 1)]
        [TestCase("TGH008", LoginType.Customer_NoInfo, 3)]
        [TestCase("TGH020", LoginType.Admin, 1)]
        [TestCase("TGH021", LoginType.Admin, 3)]
        public void AddCart_Wishlist(string ID, LoginType loginType, int numberOfProduct)
        {
            SetUpData(numberOfProduct, false);

            //B1: Đăng nhập
            setting.Login(loginType);

            //B2: Check Wishlist
            checkResult = WishlistCheck();
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B3: Thêm sản phẩm
            for (int i = 0; i < productList.Length; i++)
            {
                checkResult = Wishlist_AddCart(productList[i].Item1);
                setting.CheckFailed(ID, testData, expected, note, checkResult);
            }

            //B4: Kiểm tra giỏ hàng sau khi thêm
            checkResult = CheckCartAfterAdd();
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }


        [Test]
        [TestCase("TGH009", LoginType.Customer_NoInfo, 1, false)]
        [TestCase("TGH010", LoginType.Customer_NoInfo, 3, false)]
        [TestCase("TGH011", LoginType.Customer_NoInfo, 1, true)]
        [TestCase("TGH012", LoginType.Customer_NoInfo, 3, true)]
        [TestCase("TGH022", LoginType.Admin, 1, false)]
        [TestCase("TGH023", LoginType.Admin, 3, false)]
        [TestCase("TGH024", LoginType.Admin, 1, true)]
        [TestCase("TGH025", LoginType.Admin, 3, true)]
        public void AddCart_Search(string ID,LoginType loginType, int numberOfProduct, bool addCart)
        {
            SetUpData(numberOfProduct, false);

            //B1: Đăng nhập
            setting.Login(loginType);


            //B2: Kiểm tra giỏ hàng trước khi thêm
            checkResult = CheckCart(out product_Before_InShoppingCart, out before);
            setting.CheckFailed(ID, testData, expected, note, checkResult);


            //B3: Thêm sản phẩm
            for (int i = 0; i < productList.Length; i++)
            {
                //addCart = true: Ấn nút Add Card | addCart = false: Ấn nút Buy Now
                checkResult = feature.FindProduct_ShopPage(productList[i].Item1, true, addCart, true, false);
                setting.CheckFailed(ID, testData, expected, note, checkResult);
            }

            //B4: Kiểm tra giỏ hàng sau khi thêm
            checkResult = CheckCartAfterAdd();
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }

        [Test]
        [TestCase("TGH013", LoginType.Customer_NoInfo, 3)]
        [TestCase("TGH026", LoginType.Admin, 3)]
        public void CartSave(string ID, LoginType loginType, int numberOfProduct)
        {
            SetUpData(numberOfProduct, true);
            
            //B1: Đăng nhập
            setting.Login(loginType);

            //B2: Kiểm tra giỏ hàng trước khi thêm
            checkResult = CheckCart(out product_Before_InShoppingCart, out before);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B3: Thêm sản phẩm
            for (int i = 0; i < productList.Length; i++)
            {
                //addCart = true: Ấn nút Add Card | addCart = false: Ấn nút Buy Now
                checkResult = feature.FindProduct_ShopPage(productList[i].Item1, true, true, false, false);
                setting.CheckFailed(ID, testData, expected, note, checkResult);
            }

            //B4: Kiểm tra giỏ hàng sau khi thêm
            checkResult = CheckCartAfterAdd();
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B5: Đăng xuất
            setting.Logout();

            //B6: Đăng nhập
            setting.Login(loginType);

            //B7: Kiểm tra giỏ hàng trước khi thêm
            checkResult = CheckCart(out product_Before_InShoppingCart, out before);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B8: Kiểm tra số lượng sản phẩm
            int totalProductAfterLogout = 0;
            for (int i = 0; i < product_After_InShoppingCart.Length; i++)
            {
                for (int j = 0; j < product_Before_InShoppingCart.Length; j++)
                {
                    if (product_After_InShoppingCart[i] == product_Before_InShoppingCart[j])
                    {
                        totalProductAfterLogout++;
                    }
                }
            }

            string actual;
            bool result;
            if (totalProductAfterLogout == product_After_InShoppingCart.Length)
            {
                actual = "Giỏ hàng lưu đủ sản phẩm";
                result = true;
            }
            else if (totalProductAfterLogout == 0)
            {
                actual = "Giỏ hàng không lưu sản phẩm nào";
                result = false;
            }
            else
            {
                actual = "Giỏ hàng lưu thiếu sản phẩm";
                result = false;
            }
            setting.FinalResult(ID, testData, expected, note, (actual, result));
        }


        [TearDown]
        public void TearDown()
        {
            setting.Logout();
            driver.Close();
        }

        private (string, bool) CheckCart(out (string, int, double, bool)[] product_InShoppingCart, out int totalProduct)
        {
            string actual;
            product_InShoppingCart = new (string, int, double, bool)[0];
            totalProduct = 0;


            //Đọc số lượng sản phẩm trên giỏ hàng
            if (!ReadShoppingCartItemNumber(out int cartShow))
            {
                actual = "Tổng số lượng hàng hoá có chữ";
                return (actual, false);
            }

            //Vào giỏ hàng
            webComponent.Cart_Icon();

            //Nếu có trang báo lỗi
            checkResult = catchError.ProgramErrorPage();
            if (checkResult.Item2) { return (checkResult.Item1, false); }

            //Dò giỏ hàng
            feature.Scan_ShoppingCart(out product_InShoppingCart);

            //Kiểm tra số lượng có khớp với số lượng giỏ hàng không
            for (int i = 0; i < product_InShoppingCart.Length; i++)
            {
                if (!string.IsNullOrEmpty(product_InShoppingCart[i].Item1))
                {
                    totalProduct += product_InShoppingCart[i].Item2;
                }
            }
            if (totalProduct != cartShow)
            {
                actual = $"Tổng số lượng hàng hoá trong giỏ hàng không khớp với số hiển thị trên icon Giỏ hàng\r\n" +
                         $"Hiển thị: {cartShow} thay vì {totalProduct}";
                return (actual, false);
            }

            return (null, true);
        }
        private (string, bool) CheckCartAfterAdd()
        {
            string actual;

            //Kiểm tra số lượng hiển thị trên giỏ hàng có khớp với số lượng trong đơn
            checkResult = CheckCart(out product_After_InShoppingCart, out _);
            if (!checkResult.Item2) { return (checkResult.Item1, false); }

            int totalProduct = 0;
            actual = "Trong giỏ hàng có chứa sản phẩm:";


            for (int i = 0; i < product_After_InShoppingCart.Length; i++)
            {
                bool notInList = true;
                if (before > 0)
                {
                    for (int j = 0; j < product_Before_InShoppingCart.Length; j++)
                    {
                        //Hàng trước đó đã thêm
                        if (product_Before_InShoppingCart[j].Item1 == product_After_InShoppingCart[j].Item1)
                        {
                            if (product_After_InShoppingCart[i].Item2 == product_Before_InShoppingCart[j].Item2 + 1)
                            {
                                totalProduct++;
                                notInList = false;
                                actual += $"\r\n- {product_After_InShoppingCart[j].Item1} - Số lượng: {product_After_InShoppingCart[j].Item2}";
                            }
                            else
                            {
                                actual += $"\r\n- {product_After_InShoppingCart[j].Item1} - Số lượng: {product_After_InShoppingCart[j].Item2} [Sai - Phải là: {product_Before_InShoppingCart[j].Item2 + 1}]";
                            }
                            break;
                        }
                    }
                }
                if (notInList)
                {
                    for (int j = 0; j < productList.Length; j++)
                    {
                        if (productList[j].Item1 == product_After_InShoppingCart[i].Item1)
                        {
                            if (product_After_InShoppingCart[i].Item2 == 1)
                            {
                                totalProduct++;
                                actual += $"\r\n- {productList[j].Item1} - Số lượng: {product_After_InShoppingCart[i].Item2}";
                            }
                            else
                            {
                                actual += $"\r\n- {productList[j].Item1} - Số lượng: {product_After_InShoppingCart[i].Item2} [Sai - Phải là: 1]";
                            }
                            break;
                        }
                    }
                }
            }

            if(totalProduct == 0)
            {
                return ("Không có sản phẩm đúng theo yêu cầu nào được thêm!", false);
            }
            return (actual, totalProduct == productList.Length);
        }
        private bool ReadShoppingCartItemNumber(out int totalProduct)
        {
            if (int.TryParse(webComponent.Cart_TotalProduct(), out totalProduct))
                return true;
            return false;
        }
        private (string, bool) WishlistCheck()
        {

            string actual;
            

            //Tạo Checklist
            feature.FillCheckList(out (string, bool)[] checkList, productList);

            //Đọc số lượng sản phẩm trên yên thích
            if (!ReadWishListItemNumber(out int wishListCount))
            {
                actual = "Tổng số lượng yêu thích có chữ";
                return (actual, false);
            }

            //Vào trang Yêu thích
            webComponent.Favorite_Icon();

            // Tìm phần tử chứa thông tin sản phẩm
            IList<IWebElement> productFavorite = webComponent.Favorite_ProductList();
            checkResult = CheckWishListQuantity(wishListCount, productFavorite.Count);
            if (!checkResult.Item2) { return (checkResult.Item1, checkResult.Item2); }

            //Đánh dấu những sản phẩm đã có trên Wishlist
            for (int i = 0; i < productFavorite.Count; i++)
            {
                string name = webComponent.Favorite_ProductName(productFavorite[i]);

                for (int j = 0; j < checkList.Length; j++)
                {
                    if (checkList[j].Item1 == name)
                    {
                        checkList[j].Item2 = true;
                    }
                }
            }

            //Thêm sản phẩm còn thiếu vào Yêu thích
            for (int i = 0; i < checkList.Length; i++)
            {
                if (!checkList[i].Item2)
                {
                    checkResult = feature.FindProduct_ShopPage(checkList[i].Item1, true, false, false, false);
                    if (!checkResult.Item2)
                    {
                        return (checkResult.Item1, checkResult.Item2);
                    }
                    webComponent.ProductDetail_AddFavoriteButton();
                    checkResult = feature.CloseNotification();
                    if (!checkResult.Item2)
                    {
                        return (checkResult.Item1, checkResult.Item2);
                    }
                }

            }

            //Đọc số lượng sản phẩm trên yên thích
            if (!ReadWishListItemNumber(out wishListCount))
            {
                actual = "Tổng số lượng yêu thích có chữ.";
                return (actual, false);
            }

            //Vào trang Yêu thích
            webComponent.Favorite_Icon();

            // Tìm phần tử chứa thông tin sản phẩm
            productFavorite = webComponent.Favorite_ProductList();
            return CheckWishListQuantity(wishListCount, productFavorite.Count);
        }
        private (string,bool) Wishlist_AddCart(string targetProduct)
        {
            string actual;
            //Vào trang yêu thích
            webComponent.Favorite_Icon();

            //Đọc hết danh sách
            IList<IWebElement> productFavorite = webComponent.Favorite_ProductList();

            //Dò sản phẩm và nhấn vào
            bool findProduct = false;
            for (int i = 0; i < productFavorite.Count; i++)
            {
                string productName = webComponent.Favorite_ProductName(productFavorite[i]);
                if (productName == targetProduct)
                {
                    webComponent.Favorite_ClickProduct(productFavorite[i]);
                    findProduct = true;
                    break;
                }
            }
            if (!findProduct)
            {
                actual = $"Không tìm thấy sản phẩm \"{targetProduct}\" trong Wishlist";
                return (actual, false);
            }

            //Thêm vào giỏ hàng
            webComponent.ProductDetail_AddToCartButton();

            //Nhấn nút xác nhận
            return feature.CloseNotification();
        }
        private bool ReadWishListItemNumber(out int number)
        {
            if (int.TryParse(driver.FindElement(By.XPath("//span[@id='icon-amount-favorite']")).Text, out number))
                return true;
            return false;
        }
        private (string, bool) CheckWishListQuantity(int wishListCount, int actualCount)
        {
            if (wishListCount != actualCount)
            {
                string actual = "Tổng số lượng hàng hoá trong trang yêu thích không khớp với số hiển thị trên icon Yêu thích!";
                return (actual, false);
            }
            return (null, true);
        }
        private void SetUpData(int numberOfProduct, bool reset)
        {
            productList = randomData.RandomProduct(numberOfProduct);

            note = reset ? "Thêm sản phẩm đó vào giỏ hàng sau đó đăng xuất rồi đăng nhập lại để xem có lưu lại giỏ hàng không." : "Thêm sản phẩm đó vào giỏ hàng.";
            expected = "Trong giỏ hàng có chứa sản phẩm:";
            testData = "";

            for (int i = 0; i < productList.Length; i++)
            {
                expected += $"\r\n- {productList[i].Item1}";
                testData += (i == 0 ? "" : "\r\n") + $" - {productList[i].Item1}";
            }
            expected = reset ? "Giỏ hàng lưu đủ sản phẩm" : expected;
        }
    }
}
