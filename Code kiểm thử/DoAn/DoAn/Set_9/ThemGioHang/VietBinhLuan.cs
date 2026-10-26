using DoAn.Setting;
using DoAn.Setting.Component;
using DoAn.Setting.DataTest;
using DoAn.Setting.Variable;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DoAn.Set_9.ThemGioHang
{
    internal class VietBinhLuan
    {
        private readonly int delayTime = Setting.Setting.Instance.DelayTime();

        private readonly SetUp setting = new SetUp(1);
        private readonly RandomData randomData = new RandomData();
        private Feature feature;
        private WebComponent webComponent;
        IWebDriver driver;

        //Biến
        private int before_TotalComment, after_TotalComment;
        private string expected, note, testData, postTime;
        private (string, bool) checkResult;
        private (string, double)[] productList;
        private IList<IWebElement> commentList;

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
        [TestCase("TGH027", "Test Comment")]
        public void PostComment_NoLogin(string ID, string message)
        {
            SetUpData(message, false);

            //B1: Đăng bình luận
            checkResult = PostComment(productList[0].Item1, message);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B3: Kiểm tra số lượng bình luận
            checkResult = CheckTotalComment(false);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B4: Kiểm tra bình luận được thêm
            checkResult = CheckComment(message, true);
            setting.Logout();
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }

        [Test]
        [TestCase("TGH028", "Test Comment")]
        public void PostComment_Login(string ID, string message)
        {
            SetUpData(message, true);

            //B1: Đăng nhập
            setting.Login(LoginType.Customer_NoInfo);
            Thread.Sleep(delayTime);

            //B2: Đăng bình luận
            checkResult = PostComment(productList[0].Item1, message);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B3: Kiểm tra số lượng bình luận
            checkResult = CheckTotalComment(true);
            setting.CheckFailed(ID, testData, expected, note, checkResult);

            //B4: Kiểm tra bình luận được thêm
            checkResult = CheckComment(message, true);
            setting.Logout();
            setting.FinalResult(ID, testData, expected, note, checkResult);
        }

        [TearDown]
        public void TearDown() {driver.Close();}

        private void SetUpData(string message, bool addComment)
        {
            productList = randomData.RandomProduct(1);
            note = "Sản phẩm được chọn để thêm bình luận";
            testData = $"- Sản phẩm được thêm bình luận: {productList[0].Item1}\r\n" +
                       $"- Nội dung: {message}";
            expected = $"- Ở lại trang sản phẩm {productList[0].Item1}\r\n" + (addComment ? $"- Thêm được bình luận có nội dung: {message}" : "- Không có bình luận được thêm");
        }

        private (string, bool) PostComment(string product_name, string message)
        {
            string actual;

            //Tìm vị trí sản phẩm
            (string, bool) checkResult = feature.FindProduct_ShopPage(product_name, true, false, false, false);
            if (!checkResult.Item2)
            {
                return (checkResult.Item1, checkResult.Item2);
            }

            //Kiểm tra trước khi đăng bình luận
            commentList = webComponent.ProductDetail_AllComments();
            before_TotalComment = commentList.Count;

            //Đăng bình luận
            webComponent.ProductDetail_AddCommentInput(message);
            DateTime currentTime = DateTime.Now;
            postTime = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
            webComponent.ProductDetail_ShareButton();

            //Kiểm tra có bị ra trang chủ không
            try
            {
                if(IsHomePage())
                {
                    actual = "- Đã nhảy ra khỏi trang sản phẩm được thêm bình luận!";
                    return (actual, false);
                }
                commentList = driver.FindElements(By.ClassName("media-block"));
                after_TotalComment = commentList.Count;
            }
            catch
            {
                ////Vào lại trang chủ
                ////Đá ra trang chủ nên vào lại sản phẩm
                //checkResult = replica.FindProduct_ShopPage(driver, product_name[0], true, false, false);
                //if (!checkResult.Item2)
                //{
                //    return (checkResult.Item1, checkResult.Item2);
                //}
                actual = "- Đã nhảy ra khỏi trang sản phẩm được thêm bình luận!";
                return (actual, false);
            }
            return (null, true);
        }

        private (string, bool) CheckTotalComment(bool addComment)
        {
            string actual;
            if (addComment && after_TotalComment - before_TotalComment < 1)
            {
                actual = "- Không có bình luận được thêm.";
                return (actual, false);
            }
            if (!addComment && after_TotalComment - before_TotalComment == 0)
            {
                actual = $"- Ở lại trang sản phẩm {productList[0].Item1}\r\n" +
                         $"- Không có bình luận nào được thêm";
                return (actual, true);
            }
            return (null, true);
        }

        private (string, bool) CheckComment(string message, bool addComment)
        {
            string actual;
            for(int i =  0; i < commentList.Count; i++)
            {
                string postTimeValue = webComponent.ProductDetail_Comment_PostDate(commentList[i]);
                DateTime expectedPostTime = DateTime.ParseExact(postTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                DateTime actualPostTime   = DateTime.ParseExact(postTimeValue, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                TimeSpan difference = expectedPostTime - actualPostTime;

                //Kiểm tra ngày viết bình luận (Cho chênh lệch 5 giây)
                if (Math.Abs((int)difference.TotalSeconds) < 5)
                {
                    Console.WriteLine("Come here");
                    string commentContent = webComponent.ProductDetail_Comment_Content(commentList[i]);
                    Console.WriteLine(commentContent);
                    if (commentContent == message)
                    {
                        //Thêm được bình luận
                        if (addComment)
                        {
                            actual = $"- Ở lại trang sản phẩm {productList[0].Item1}\r\n" +
                                     $"- Thêm được bình luận có nội dung: {commentContent}";
                            return (actual, true);
                        }
                        //Không thêm được bình luận
                        else
                        {
                            actual = $"- Ở lại trang sản phẩm {productList[0].Item1}\r\n" +
                                     $"- Thêm được bình luận có nội dung: {commentContent} [Sai - Không thêm được]";
                            return (actual, false);
                        }
                    }
                    else
                    {
                        //Thêm được bình luận
                        if (addComment)
                        {
                            actual = $"- Ở lại trang sản phẩm {productList[0].Item1}\r\n" +
                                     $"- Thêm được bình luận có nội dung: {commentContent} [Sai - Phải là: {message}]";
                        }
                        //Không thêm được bình luận
                        else
                        {
                            actual = $"- Ở lại trang sản phẩm {productList[0].Item1}\r\n" +
                                     $"- Thêm được bình luận có nội dung: {commentContent} [Sai - Không thêm được]";
                        }
                        return(actual, false);
                    }

                }
            }
            //Thêm được bình luận
            if (addComment)
            {
                actual = $"- Ở lại trang sản phẩm {productList[0].Item1}\r\n" +
                         $"- Không tìm thấy bình luận được thêm.";
                return (actual, false);
            }
            //Không thêm được bình luận
            actual = $"Ở lại trang sản phẩm {productList[0].Item1}\r\n" +
                     $"- Không có bình luận được thêm";
            return (actual, true);
        }

        private bool IsHomePage()
        {
            try
            {
                webComponent.HomePage_Banner();
                return true;
            }
            catch { return false; }
        }

    }
}
