using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading;

namespace DoAn.Setting.Component
{
    internal class Feature
    {
        private readonly WebComponent webComponent;
        private readonly CatchError catchError;
        private readonly IWebDriver driver;

        private readonly int delayTime = Setting.Instance.DelayTime();

        //Biến
        private (string, bool) checkResult;

        public Feature(IWebDriver driver)
        {
            this.driver = driver;
            webComponent = new WebComponent(driver);
            catchError = new CatchError(driver);
        }


        //Lấy thông tin giỏ hàng
        public (string, bool) Check_ShoppingCart(out (string, int, double, bool)[] product_InShoppingCart, (string, double)[] productList)
        {
            //Danh sách kiểm tra
            FillCheckList(out (string, bool)[] checkList, productList);

            //B1: Vào giỏ hàng
            webComponent.Cart_Icon();

            //B2: Kiểm tra có trang báo lỗi không
            checkResult = catchError.ProgramErrorPage();
            if (checkResult.Item2)
            {
                product_InShoppingCart = new (string, int, double, bool)[0];
                return (checkResult.Item1, false);
            }

            //B3: Dò giỏ hàng
            Scan_ShoppingCart(out product_InShoppingCart);

            //B4: Đối chứng với danh sách sản phẩm muốn test
            if (product_InShoppingCart.Length > 0) { checkList = CheckCurrentProduct(product_InShoppingCart, checkList); }

            //B5: Thêm các sản phẩm còn thiếu
            checkResult = AddMissingProduct(checkList, false);
            if (!checkResult.Item2) return (checkResult.Item1, false);

            //B6: Vào trang giỏ hàng
            webComponent.Cart_Icon();
            return (null, true);
        }
        public (string, bool) Check_ShoppingCart(out (string, int, double, bool)[] product_InShoppingCart, (string, double, int, int)[] productList, bool addTwice)
        {
            //Danh sách kiểm tra
            product_InShoppingCart = new (string, int, double, bool)[0];
            FillCheckList(out (string, bool)[] checkList, productList);

            //B1: Vào giỏ hàng
            webComponent.Cart_Icon();

            //B2: Kiểm tra có trang báo lỗi không
            checkResult = catchError.ProgramErrorPage();
            if (checkResult.Item2) { return (checkResult.Item1, false); }

            //B3: Dò giỏ hàng
            Scan_ShoppingCart(out product_InShoppingCart);

            //B4: Đối chứng với danh sách sản phẩm muốn test
            if (product_InShoppingCart.Length > 0) { checkList = CheckCurrentProduct(product_InShoppingCart, checkList); }

            //B5: Thêm các sản phẩm còn thiếu
            checkResult = AddMissingProduct(checkList, addTwice);
            if (!checkResult.Item2) return (checkResult.Item1, false);

            //B6: Vào trang giỏ hàng
            webComponent.Cart_Icon();

            //B7: Dò giỏ hàng
            Scan_ShoppingCart(out product_InShoppingCart);

            return (null, true);
        }
        public (string, bool) Check_ShoppingCart((string, double)[] productList)
        {
            //Danh sách kiểm tra
            FillCheckList(out (string, bool)[] checkList, productList);

            //B1: Vào giỏ hàng
            webComponent.Cart_Icon();


            //B2: Kiểm tra có trang báo lỗi không
            (string, bool) checkResult = catchError.ProgramErrorPage();
            if (checkResult.Item2) { return (checkResult.Item1, false); }

            //B3: Dò giỏ hàng
            Scan_ShoppingCart(out (string, int, double, bool)[] product_InShoppingCart);

            //B4: Đối chứng với danh sách sản phẩm muốn test
            if (product_InShoppingCart.Length > 0) { checkList = CheckCurrentProduct(product_InShoppingCart, checkList); }

            //B5: Thêm các sản phẩm còn thiếu
            checkResult = AddMissingProduct(checkList, false);
            if (!checkResult.Item2) return (checkResult.Item1, false);

            //B6: Vào trang giỏ hàng
            webComponent.Cart_Icon();
            return (null, true);
        }


        
        //Tạo danh sách để kiểm tra
        public void FillCheckList(out (string, bool)[] checkList, (string, double)[] productList)
        {
            checkList = new (string, bool)[productList.Length];

            for (int i = 0; i < productList.Length; i++)
            {
                checkList[i] = (productList[i].Item1, false);
            }
        }
        public void FillCheckList(out (string, bool)[] checkList, (string, double, int, int)[] productList)
        {
            checkList = new (string, bool)[productList.Length];

            for (int i = 0; i < productList.Length; i++)
            {
                checkList[i] = (productList[i].Item1, false);
            }
        }

        //Thêm sản phẩm còn thiếu
        public (string, bool) AddMissingProduct((string, bool)[] checkList, bool addTwice)
        {
            for (int i = 0; i < checkList.Length; i++)
            {
                if (!checkList[i].Item2)
                {
                    (string, bool) checkResult = FindProduct_ShopPage(checkList[i].Item1, true, true, false, addTwice);
                    if (!checkResult.Item2) { return (checkResult.Item1, checkResult.Item2); }
                }
            }
            return (null, true);
        }

        //Đóng ô xác nhận
        public (string, bool) CloseNotification()
        {
            int restartNo = 0;
            while (true)
            {
                // Kiểm tra xem phần tử đã xuất hiện hay chưa
                try
                {
                    webComponent.Popup_Confirm();
                    return (null, true);
                }
                catch
                {
                    if (restartNo > 5) { break; }
                    Thread.Sleep(1000);
                    restartNo++;
                }
            }
            string actual = "Không hiện hoặc hiện quá lâu khung xác nhận.";
            return (actual, false);
        }

        //Tìm kiếm
        public void SearchProduct(string data)
        {
            Thread.Sleep(delayTime);
            webComponent.SearchProduct(data);
            Thread.Sleep(delayTime);
        }

        //Viết danh sách sản phẩm trong giỏ hàng
        public void Scan_ShoppingCart(out (string, int, double, bool)[] productList)
        {
            int productNo = 0;

            //Lấy toàn bộ giỏ hàng
            IList<IWebElement> cartProducts = webComponent.Cart_ProductList();
            productList = new (string, int, double, bool)[cartProducts.Count];

            for (int i = 0; i < cartProducts.Count; i++)
            {
                //Lấy thông tin
                string productName = webComponent.Cart_ProductName(cartProducts[i]);
                if (!string.IsNullOrEmpty(productName))
                {
                    int quantity = int.Parse(webComponent.Cart_ProductQuantity(cartProducts[i]));
                    double price = double.Parse(webComponent.Cart_ProductPrice(cartProducts[i]));
                    productList[productNo] = (productName, quantity, price, false);
                    productNo++;
                }
            }
        }

        public (string, bool)[] CheckCurrentProduct((string, int, double, bool)[] product_InShoppingCart, (string, bool)[] checkList)
        {
            for (int i = 0; i < product_InShoppingCart.Length; i++)
            {
                for (int j = 0; j < checkList.Length; j++)
                {
                    if (product_InShoppingCart[i].Item1 == checkList[j].Item1) { checkList[i].Item2 = true; }
                }
            }
            return checkList;
        }

        //Thêm sản phẩm từ trang Home
        public (string, bool) AddProduct_HomePage(string productTarget)
        {
            IWebElement product;
            try
            {
                //Vào trang Home
                webComponent.HomePage();
                Thread.Sleep(delayTime);

                //Tìm sản phẩm
                product = webComponent.HomePage_Product(productTarget);
            }
            catch
            {
                string actual = $"Không tìm thấy sản phẩm \"{productTarget}\" dù đã có trên hệ thống!";
                return (actual, false);
            }

            // Ấn vào nút "Buy now"
            webComponent.HomePage_BuyNowButton(product);

            // Ấn vào nút "Add To Cart"
            webComponent.ProductDetail_AddToCartButton();

            //Đóng thông báo
            return CloseNotification();
        }

        //Thêm sản phẩm từ trang Shop (Bao gồm tìm kiếm)
        public (string, bool) FindProduct_ShopPage(string productTarget, bool search, bool addCart, bool AddToCart, bool addTwice)
        {
            (string, bool) checkResult;
            // Khởi tạo đối tượng Actions (Trỏ chuột
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(driver);

            //Vào trang Shop
            if (!search)
            {
                webComponent.ShopPage();
                Thread.Sleep(delayTime);
            }
            else { SearchProduct(productTarget); }

            // Tìm tất cả các card được tìm thấy
            IList<IWebElement> productList = webComponent.Shop_ProductList();

            // Dò từng phần tử tìm thấy được
            bool productFound = false;
            int i;
            for (i = 0; i < productList.Count; i++)
            {
                string productName = webComponent.Shop_ProductName(productList[i]);

                //Kiếm thấy sản phẩm
                if (productName == productTarget)
                {
                    if (addTwice)
                    {
                        // Di chuột đến phần tử "card" - Lần 1
                        actions.MoveToElement(productList[i]).Perform();
                        Thread.Sleep(delayTime);

                        if (addCart)
                        {
                            webComponent.Shop_AddCartButton(productList[i]);
                            checkResult = CloseNotification();
                            if (!checkResult.Item2) { return checkResult; }
                        }
                        if (!addCart)
                        {
                            webComponent.Shop_BuyNowButton(productList[i]);

                            //Chỉ vào trang mà không thêm hàng vào giỏ
                            if (!AddToCart) return (null, true);
                            webComponent.ProductDetail_AddToCartButton();
                            checkResult = CloseNotification();
                            if (!checkResult.Item2) { return checkResult; }
                            driver.Navigate().Back();
                        }
                    }
                    // Di chuột đến phần tử "card"
                    actions.MoveToElement(productList[i]).Perform();
                    Thread.Sleep(delayTime);
                    productFound = true;
                }
            }
            //Không tìm thấy sản phẩm
            if (!productFound)
            {
                string actual = $"Không tìm thấy sản phẩm \"{productTarget}\" dù đã có trên hệ thống!";
                return (actual, false);
            }
            //Bấm nút "Buy now"
            if (!addCart)
            {
                webComponent.Shop_BuyNowButton(productList[i -1]);

                //Chỉ vào trang mà không thêm hàng vào giỏ
                if (!AddToCart) return (null, true);
                webComponent.ProductDetail_AddToCartButton();
            }
            //Bấm nút "Add Cart"
            else { webComponent.Shop_AddCartButton(productList[i - 1]); }
            return CloseNotification();
        }

        //Nhập thông tin giao hàng
        public (string, bool) DestinationChange(string name, string phone, string address)
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

        //Nhập thêm khuyến mãi
        public void InputVoucher(string voucher)
        {
            webComponent.Cart_VoucherInput(voucher);
            webComponent.Cart_Voucher_AddButton();
            Thread.Sleep(delayTime);
        }
    }
}
