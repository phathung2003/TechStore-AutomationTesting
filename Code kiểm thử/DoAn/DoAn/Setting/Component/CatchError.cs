using OpenQA.Selenium;
using System;

namespace DoAn.Setting.Component
{
    internal class CatchError
    {
        private readonly WebComponent webComponent;
        string errorMessage;
        public CatchError(IWebDriver driver) 
        {
            webComponent = new WebComponent(driver);
        }

        public (string, bool) ProgramErrorPage()
        {
            try 
            {
                errorMessage = "Hiện trang báo lỗi với nội dung:\r\n" + webComponent.ErrorPage_ErrorName();
                return (errorMessage, true); 
            }
            catch { return (null, false); }
        }

        public (string, bool) VNPAYError()
        {
            try
            {
                errorMessage = "Xảy ra lỗi: " + webComponent.VNPAY_ErrorMessage();
                return (errorMessage, true);
            }
            catch{ return (null, false); }
        }

        public (string, bool) ErrorCode()
        {
            try
            {
                errorMessage = "Hiện trang báo lỗi với nội dung:\r\n" + webComponent.ErrorPage_ErrorCode();
                return (errorMessage, true);
            }
            catch { return (null, false); }
        }
    }
}
