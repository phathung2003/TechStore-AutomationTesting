using Microsoft.Office.Interop.Excel;
using System;
using System.Globalization;
using DoAn.Setting.Variable;

namespace DoAn.Setting
{
    internal class ReadDataTest
    {
        private readonly Application excel = new Application();
        Workbook workBook;
        Worksheet workSheet;
        public ReadDataTest() { }

        //Lấy dữ liệu Rank
        public (string, double)[] Rank()
        {
            (string, double)[] rank;
            workBook = excel.Workbooks.Open(Setting.Instance.ExcelTestData());
            workSheet = workBook.Worksheets[5];

            int totalRows = TotalRows();
            rank = new (string, double)[totalRows - 1];

            for (int i = 2; i <= totalRows; i++)
            {
                string discountValue = GetCellValue(i, 2);

                if (double.TryParse(discountValue, out double discount))
                {
                    string nameValue = GetCellValue(i, 1);
                    rank[i - 2] = (nameValue, discount / 100);
                }
            }

            workBook.Close();
            excel.Quit();

            return rank;
        }

        //Lấy dữ liệu sản phẩm
        public (string, string, string, double, int, string)[] Product()
        {
            (string, string, string, double, int, string)[] productList;
            workBook = excel.Workbooks.Open(Setting.Instance.ExcelTestData());
            workSheet = workBook.Worksheets[3];

            int totalRows = TotalRows();
            int totalProduct = int.Parse(GetCellValue(1, 20));
            int productNo = 0;

            productList = new (string, string, string, double, int, string)[totalProduct];

            for (int i = 2; i <= totalRows; i++)
            {
                string checkValue = GetCellValue(i, 18);

                try
                {
                    if (checkValue.ToLower() == "x")
                    {
                        string priceValue = GetCellValue(i, 10);
                        string storageValue = GetCellValue(i, 12);

                        if (double.TryParse(priceValue, out double price) && int.TryParse(storageValue, out int storage))
                        {
                            string name = GetCellValue(i, 2);
                            string category = GetCellValue(i, 4);
                            string brand = GetCellValue(i, 6);
                            string color = GetCellValue(i, 17);
                            productList[productNo] = (name, category, brand, price, storage, color);
                            productNo++;
                        }
                    }
                }
                catch {}     
            }
            workBook.Close();
            excel.Quit();

            return productList;
        }

        //Lấy dữ liệu mã giảm giá
        public (string, string , double, string, int, DateTime)[] Voucher()
        {
            (string, string, double, string, int, DateTime)[] discountList;
            workBook = excel.Workbooks.Open(Setting.Instance.ExcelTestData());
            workSheet = workBook.Worksheets[4];

            int totalRows = TotalRows();
            discountList = new (string, string, double, string, int, DateTime)[totalRows - 1];
            int discountNo = 0;

            for (int i = 2; i <= totalRows; i++)
            {
                try
                {
                    string name = GetCellValue(i, 1);
                    string code = GetCellValue(i, 2);
                    string priceValue = GetCellValue(i, 3);
                    string rank = GetCellValue(i, 4);
                    string amountValue = GetCellValue(i, 5);
                    string dateEndValue = GetCellValue(i, 7);

                    DateTime dateEnd = DateTime.ParseExact(dateEndValue, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if (double.TryParse(priceValue, out double price) && int.TryParse(amountValue, out int amount))
                    {
                        discountList[discountNo] = (name, code, price, rank, amount, dateEnd);
                        discountNo++;
                    }
                }
                catch { }
            }
            workBook.Close();
            excel.Quit();
            return discountList;
        }

        //Lấy tài khoản ngân hàng
        public (string, string, string, string, string, string)[] BankAccount()
        {
            (string, string, string, string, string, string)[] bankAccountList;
            workBook = excel.Workbooks.Open(Setting.Instance.ExcelTestData());
            workSheet = workBook.Worksheets[6];

            int totalRows = TotalRows();
            bankAccountList = new (string, string, string, string, string, string)[totalRows - 1];

            for (int i = 2; i <= totalRows; i++)
            {
                try
                {
                    string cardNumber = GetCellValue(i, 2);
                    string cardHolder = GetCellValue(i, 3);
                    string ngayPhatHanh = GetCellValue(i, 4);
                    string OTP = GetCellValue(i, 5);
                    string note = GetCellValue(i, 6);
                    string systemNotification = GetCellValue(i, 7);
                    bankAccountList[i - 2] = (cardNumber, cardHolder, ngayPhatHanh, OTP, note, systemNotification);
                }
                catch { }
            }
            workBook.Close();
            excel.Quit();
            return bankAccountList;
        }

        //Lấy tài khoản đăng nhập
        public (string, string) Account(LoginType loginType)
        {
            string email, password;

            switch (loginType)
            {
                //Tài khoản Admin
                case LoginType.Admin:
                    email = "admin123@gmail.com";
                    password = "admin123";
                    break;

                //Tài khoản khách hàng - Có thông tin
                case LoginType.Customer_HaveInfo:
                    email = "have.info@gmail.com";
                    password = "12345678";
                    break;

                //Tài khoản khách hàng - Không có thông tin
                default:
                    email = "no.info@gmail.com";
                    password = "12345678";
                    break;
            }
            return (email, password);
        }

        //-----------------------------------------------------//
        //Lấy dữ liệu ô trong Excel
        private string GetCellValue(int rows, int col)
        {
            Microsoft.Office.Interop.Excel.Range cell = workSheet.Cells[rows, col];
            return cell.Value2?.ToString();
        }

        //Tổng số dòng có dữ liệu
        private int TotalRows()
        {
            int lastUsedRow = workSheet.Cells.Find("*", System.Reflection.Missing.Value,
                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                               Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

            return lastUsedRow;
        }
    }
}
