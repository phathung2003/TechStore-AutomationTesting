using Microsoft.Office.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
namespace DoAn.Setting.Component
{
    internal class WebComponent
    {
        private readonly IWebDriver driver;
        public WebComponent(IWebDriver driver) { this.driver = driver; }

        //[TopBar]
        public void Login_Icon() => driver.FindElement(By.CssSelector("a[id='init-login-form-1'] span[class='material-symbols-outlined']")).Click();
        public void Account_Icon() => driver.FindElement(By.XPath("//span[normalize-space()='person']")).Click();
        public void Favorite_Icon() => driver.FindElement(By.XPath("//span[contains(text(), 'favorite')]")).Click();
        public void Cart_Icon() => driver.FindElement(By.XPath("//span[normalize-space()='shopping_bag']")).Click();
        public void SearchProduct(string value)
        {
            driver.FindElement(By.XPath("//input[@placeholder='Search something...']")).SendKeys(value);
            driver.FindElement(By.XPath("//button[@class='btn btn_common']")).Click();
        }

        //[Login]
        public void Login_EmailInput(string email) => driver.FindElement(By.XPath("//form[@class='form-sign-in']//input[@placeholder='Email']")).SendKeys(email);
        public void Login_PasswordInput(string password) => driver.FindElement(By.XPath("//form[@class='form-sign-in']//input[@placeholder='Password']")).SendKeys(password);
        public void Login_SignInButton() => driver.FindElement(By.XPath("//form[@class='form-sign-in']//button[@type='submit'][normalize-space()='Sign In']")).Click();

        //[HomePage]
        public void HomePage() => driver.FindElement(By.XPath("//span[@class='menu-item-title'][normalize-space()='Home']")).Click();
        public void HomePage_Banner() => driver.FindElement(By.CssSelector("li.splide__slide.is-active.is-visible"));
        public IWebElement HomePage_Product(string productName) => driver.FindElement(By.XPath($"//h5[text()='{productName}']/ancestor::div[@class='card']"));
        public void HomePage_BuyNowButton(IWebElement product) => product.FindElement(By.CssSelector(".badge.bg-primary.ms-2")).Click();

        //[Shop Page]
        public void ShopPage() => driver.FindElement(By.XPath("//span[normalize-space()='Shop']")).Click();
        public IList<IWebElement> Shop_ProductList() => driver.FindElements(By.CssSelector(".card"));
        public string Shop_ProductName(IWebElement product) => product.FindElement(By.CssSelector(".card__title")).Text;
        public void Shop_BuyNowButton(IWebElement product) => product.FindElement(By.XPath(".//button[contains(text(), 'Buy now')]")).Click();
        public void Shop_AddCartButton(IWebElement product) => product.FindElement(By.XPath(".//a[contains(@class, 'add-cart')]")).Click();

        //[Product Detail]
        public void ProductDetail_AddToCartButton() => driver.FindElement(By.XPath("//a[normalize-space()='add to cart']")).Click();
        public void ProductDetail_AddFavoriteButton() => driver.FindElement(By.XPath("//a[@class='like btn btn-default']")).Click();
        public IList<IWebElement> ProductDetail_AllComments() => driver.FindElements(By.ClassName("media-block"));
        public void ProductDetail_AddCommentInput(string comment) => driver.FindElement(By.CssSelector("input[placeholder='What are you thinking?']")).SendKeys(comment);
        public void ProductDetail_ShareButton() => driver.FindElement(By.XPath("//button[normalize-space()='Share']")).Click();
        public string ProductDetail_Comment_PostDate(IWebElement comment) => comment.FindElement(By.XPath(".//p[contains(@class, 'text-muted')]")).Text.Trim();
        public string ProductDetail_Comment_Content(IWebElement comment) => comment.FindElement(By.XPath(".//p[@class='ml-2']")).Text;

        //[Profile] - User Avatar
        public void User_Avatar() => driver.FindElement(By.XPath("//div[@class='user']//i[@class='fas fa-user-circle']"));

        //[Profile] - Thông tin cá nhân
        public void UserInfoButton() => driver.FindElement(By.XPath("//div[@class='option']//a[normalize-space()='Account Detail']")).Click();
        public IWebElement UserInfo_NameInput() => driver.FindElement(By.CssSelector("input[name='name']"));
        public IWebElement UserInfo_PhoneInput() => driver.FindElement(By.CssSelector("input[name='phone']"));
        public IWebElement UserInfo_AddressInput() => driver.FindElement(By.CssSelector("input[name='address']"));
        public void SignOut() => driver.FindElement(By.XPath("//a[@id='submit-form']")).Click();

        //[Profile] - Order
        public void Order() => driver.FindElement(By.XPath("//a[contains(@href, 'getOrderConfirm')]")).Click();
        public IList<IWebElement> OrderList() => driver.FindElements(By.XPath("//div[@class='card']"));
        public string OrderList_UserName(IWebElement order) => order.FindElement(By.XPath(".//div[@class='d-flex justify-content-between align-items-center mb-1'][1]/p[2]")).Text;
        public string OrderList_Address(IWebElement order) => order.FindElement(By.XPath(".//div[@class='d-flex justify-content-between align-items-center mb-1'][2]/p[2]")).Text;
        public string OrderList_Phone(IWebElement order) => order.FindElement(By.XPath(".//div[@class='d-flex justify-content-between align-items-center mb-1'][3]/p[2]")).Text;
        public IList<IWebElement> OrderList_OrderDetail(IWebElement order) => order.FindElements(By.CssSelector(".card-body table tbody tr"));
        public string OrderList_OrderDetail_ProductName(IWebElement orderDetail) => orderDetail.FindElement(By.CssSelector("td:nth-of-type(2)")).Text;
        public string OrderList_OrderDetail_Quantity(IWebElement orderDetail) => orderDetail.FindElement(By.CssSelector("td:nth-of-type(5)")).Text;
        public string OrderList_OrderDetail_Price(IWebElement orderDetail) => orderDetail.FindElement(By.CssSelector("td:nth-of-type(6)")).Text;
        public string OrderList_Total(IWebElement order) => order.FindElement(By.XPath(".//div[@class='d-flex justify-content-between pt-1'][1]/p[2]")).Text;
        public string OrderList_Voucher(IWebElement order) => order.FindElement(By.XPath(".//div[@class='d-flex justify-content-between pt-1'][2]/p[2]")).Text;
        public string OrderList_DiscountRank(IWebElement order) => order.FindElement(By.XPath(".//div[@class='d-flex justify-content-between pt-1'][3]/p[2]")).Text;
        public string OrderList_TotalPaid(IWebElement order) => order.FindElement(By.XPath($".//h5[contains(text(), 'Total paid:')]")).Text.Substring(12);
        public void OrderList_CancelledButton(IWebElement order) => order.FindElement(By.XPath("//a[contains(@href, 'cancelCart')]")).Click();
        public void Order_CancelledButton() => driver.FindElement(By.XPath("//a[contains(@type,'button')][normalize-space()='Cancelled']")).Click();

        //[Yêu thích]
        public IList<IWebElement> Favorite_ProductList() => driver.FindElements(By.XPath("//div[@class='test r-item']"));
        public string Favorite_ProductName(IWebElement product) => product.FindElement(By.XPath(".//div[@class='product-name']/h5")).Text;
        public void Favorite_ClickProduct(IWebElement product) => product.FindElement(By.CssSelector(".link-img .product-img")).Click();

        //[Giỏ hàng] - Thông tin hàng
        public string Cart_TotalProduct() => driver.FindElement(By.XPath("//span[@id='icon-amount-orders']")).Text;
        public IList<IWebElement> Cart_ProductList() => driver.FindElements(By.CssSelector(".test.r-item"));
        public string Cart_ProductName(IWebElement product) => product.FindElement(By.ClassName("product-name")).Text;
        public string Cart_ProductQuantity(IWebElement product) => product.FindElement(By.CssSelector(".input-qty")).GetAttribute("value");
        public IWebElement Cart_QuantityInput(IWebElement product) => product.FindElement(By.CssSelector(".qty-container input.input-qty"));
        public void Cart_MinusQuantityButton(IWebElement product) => product.FindElement(By.CssSelector(".qty-btn-minus")).Click();
        public void Cart_AddQuantityButton(IWebElement product) => product.FindElement(By.CssSelector(".qty-btn-plus")).Click();

        public string Cart_ProductPrice(IWebElement product) => product.FindElement(By.CssSelector(".ta-c span")).Text;
        public void Cart_DeleteProduct(IWebElement product) => product.FindElement(By.CssSelector("[onclick*='deleteItemListCart']")).Click();

        //[Giỏ hàng] - Thông tin mua hàng
        public string Cart_Info_Name() => driver.FindElement(By.CssSelector("p.customer-info-name")).Text;
        public string Cart_Info_Phone() => driver.FindElement(By.CssSelector("p.customer-info-phone")).Text;
        public string Cart_Info_Address() => driver.FindElement(By.CssSelector(".address")).Text.Substring(6);
        public void Cart_ChangeInfo() => driver.FindElement(By.CssSelector("div[class='content-bic'] a[class='change-address']")).Click();
        public IWebElement Cart_ChangeInfo_NameInput() => driver.FindElement(By.CssSelector("input[placeholder='name...']"));
        public string Cart_ChangeInfo_NameError() => driver.FindElement(By.XPath("//span[normalize-space()='The nick name field is required.']")).Text;
        public IWebElement Cart_ChangeInfo_PhoneInput() => driver.FindElement(By.CssSelector("input[placeholder='Your Phone...']"));
        public string Cart_ChangeInfo_PhoneError() => driver.FindElement(By.XPath("//span[normalize-space()='The phone field is required.']")).Text;
        public IWebElement Cart_ChangeInfo_AddressInput() => driver.FindElement(By.CssSelector("input[placeholder='Your Addresss...']"));
        public string Cart_ChangeInfo_AddressError() => driver.FindElement(By.XPath("//span[normalize-space()='The address field is required.']")).Text;
        public void Cart_ChangeInfo_SubmitButton() => driver.FindElement(By.CssSelector("div[class='card-body'] button[type='submit']")).Click();

        //[Giỏ hàng] - Voucher
        public void Cart_VoucherInput(string voucher) => driver.FindElement(By.XPath("//div[@id='content-right']//input[@placeholder='Nhập mã khuyến mãi...']")).SendKeys(voucher);
        public void Cart_Voucher_AddButton() => driver.FindElement(By.XPath("//div[@id='content-right']//button[@type='submit'][normalize-space()='Add']")).Click();
        public void Cart_SelectYourVoucher() => driver.FindElement(By.XPath("//span[@id='voucher-link']")).Click();
        public IList<IWebElement> Cart_SelectYourVoucher_AllVoucher() => driver.FindElements(By.XPath("//div[@class='row']"));
        public string Cart_SelectYourVoucher_VoucherName(IWebElement voucher) => voucher.FindElement(By.ClassName("voucher-item-name")).Text;
        public void Cart_SelectYourVoucher_AddButton(IWebElement voucher) => voucher.FindElement(By.CssSelector("button[type='submit']")).Click();
        public void Cart_SelectYourVoucher_CloseButton() => driver.FindElement(By.XPath("//span[@id='close-vouchers-wrap']")).Click();


        //[Giỏ hàng] - Hoá đơn
        public string Cart_PreTotal() => driver.FindElement(By.XPath("//span[@id='pre-total-price']")).Text;
        public string Cart_RankName() => driver.FindElement(By.XPath("//span[contains(text(), 'Hạng')]")).Text.Split(' ')[1];
        public string Cart_RankDiscount() => driver.FindElement(By.XPath("//span[@id='price-discount-rank']")).Text;
        public string Cart_VoucherDiscount() => driver.FindElement(By.XPath("//span[@class='price-text' and text()='Giảm giá']")).FindElement(By.XPath("./following-sibling::div[@class='price-value']")).Text;
        public string Cart_VoucherName() => driver.FindElement(By.XPath("//li[@class='price-item']//span[text()='Voucher']/following-sibling::div")).Text;
        public string Cart_Total() => driver.FindElement(By.XPath("//span[@id='price-total-value']")).Text;
        
        //[Giỏ hàng] - Checkout
        public void Cart_CheckoutButton() => driver.FindElement(By.CssSelector(".btn-checkout")).Click();
        public string Cart_Checkout_NotificationStatus() => driver.FindElement(By.CssSelector("#swal2-title")).Text;
        public string Cart_Checkout_NotificationMessage() => driver.FindElement(By.CssSelector("#swal2-html-container")).Text;
        //[Giỏ hàng] - Check Payment
        public void Cart_CheckPaymentButton() => driver.FindElement(By.XPath("//body/div/div[@class='page-content']/div[@class='container']/div[@id='main-cart']/div[@id='content-right']/form[1]")).Click();

        //[Giỏ hàng] - Rank
        public string Cart_Rank() => driver.FindElement(By.XPath("//span[contains(text(), 'Hạng')]")).Text.Split(' ')[1];


        //[VNPAY] - Thông tin thẻ
        public string VNPAY_TotalMoney() => (driver.FindElement(By.XPath("//span[@id='totalAmountDt']")).Text).Replace(",", "");
        public void VNPAY_CardNumber(string cardNumber) => driver.FindElement(By.XPath("//input[@id='card_number_mask']")).SendKeys(cardNumber);
        public void VNPAY_CardHolder(string cardHolder) => driver.FindElement(By.XPath("//input[@id='cardHolder']")).SendKeys(cardHolder);
        public void VNPAY_IssuingDate(string date) => driver.FindElement(By.XPath("//input[@id='cardDate']")).SendKeys(date);
        public void VNPAY_CardInfo_ContinueButton() => driver.FindElement(By.XPath("//a[@id='btnContinue']")).Click();
        public void VNPAY_AgreeButton() => driver.FindElement(By.XPath("//a[@id='btnAgree']")).Click();
        public void VNPAY_CardInfo_CancelButton() => driver.FindElement(By.XPath("//a[@data-bs-target='#modalCancelPayment']")).Click();

        //[VNPAY] - OTP
        public void VNPAY_OTP(string OTP) => driver.FindElement(By.Id("otpvalue")).SendKeys(OTP);
        public void VNPAY_OTP_ConfirmButton() => driver.FindElement(By.XPath("//button[@id='btnConfirm']")).Click();
        public void VNPAY_OTP_CancelButton() => driver.FindElement(By.XPath("//a[@data-bs-toggle='modal']")).Click();
        public void VNPAY_Cancel_ConfirmButton() => driver.FindElement(By.XPath("//a[@class='ubg-danger ubox-size-button-default ubg-hover ubg-active ubtn']")).Click();
        public string VNPAY_ErrorMessage() => driver.FindElement(By.XPath("//label[@id='lb_message_error']")).Text;


        //[ErrorPage]
        public string ErrorPage_ErrorName() => driver.FindElement(By.XPath("//div[@class='line-clamp-2']")).Text;
        public string ErrorPage_ErrorCode()
        {
            string errorCode = driver.FindElement(By.XPath("//div[@class='flex items-center pt-8 sm:justify-start sm:pt-0']/div[1]")).Text.Trim();
            string errorMessage = driver.FindElement(By.XPath("//div[@class='flex items-center pt-8 sm:justify-start sm:pt-0']/div[2]")).Text.Trim();

            return $"{errorCode} - {errorMessage}";
        }

        //Popup
        public void Popup_Confirm() => driver.FindElement(By.XPath("//button[normalize-space()='OK']")).Click();

        //[Google]
        public double Google_ConverterValue()
        {
            IWebElement converterValue = driver.FindElement(By.Id("target-input"));
            string result = converterValue.GetAttribute("value");
            string[] parts = result.Split('.');
            string formattedValue = String.Join(",", parts[0], parts[1]);
            formattedValue += "." + parts[2];
            if (!double.TryParse(formattedValue, out double USDConverter)) { return 0; }
            else { return USDConverter; }
        }
    }
}
