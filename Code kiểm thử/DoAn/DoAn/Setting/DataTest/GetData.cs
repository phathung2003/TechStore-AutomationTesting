using System;
using System.Text.RegularExpressions;

namespace DoAn.Setting.DataTest
{
    internal class GetData
    {
        public (string, string, string, string, string, string) GetBankAccount(CardType type)
        {
            ReadDataTest readDataTest = new ReadDataTest();
            (string, string, string, string, string, string)[] bankAccountList = readDataTest.BankAccount();
            
            if (type == CardType.WrongCard) { return ("9704198526191432190", "NGUYEN VAN A", "07/15", "987654", "Thẻ sai", "Incorrect card information"); }
            for (int i = 0; i < bankAccountList.Length; i++)
            {
                if (type == CardType.Success && bankAccountList[i].Item5 == "Thành công") { return bankAccountList[i]; }
                if (type == CardType.InsufficientBalance && bankAccountList[i].Item5 == "Thẻ không đủ số dư") { return bankAccountList[i]; }
                if (type == CardType.NotActive && bankAccountList[i].Item5 == "Thẻ chưa kích hoạt") { return bankAccountList[i]; }
                if (type == CardType.Locked && bankAccountList[i].Item5 == "Thẻ bị khóa") { return bankAccountList[i]; }
                if (type == CardType.Expried && bankAccountList[i].Item5 == "Thẻ bị hết hạn") { return bankAccountList[i]; }
            }
            return (null, null, null, null, null, null);
        }

        public (string, string, double) GetVoucher(VoucherType type, double totalMoney)
        {
            ReadDataTest readDataTest = new ReadDataTest();
            (string, string, double, string, int, DateTime)[] voucherList = readDataTest.Voucher();

            if (type == VoucherType.NotExist) { return (null, "NotExist", 0); }
            for (int i = 0; i < voucherList.Length; i++)
            {
                string name = voucherList[i].Item1;
                string code = voucherList[i].Item2;
                double discount = voucherList[i].Item3;

                if (type == VoucherType.OutDate && voucherList[i].Item6 < DateTime.Now) { return (name, code, -discount); }

                //Hết mã khuyến mãi
                if (type == VoucherType.OutOfStock && voucherList[i].Item5 == 0) { return (name, code, -discount); }

                //Giảm quá số tiền sản phẩm
                if (type == VoucherType.Overload && ((double.IsNaN(totalMoney) && discount > 1000000) || discount > totalMoney))
                {
                    return (name, code, -discount);
                }

                //Chấp nhận
                if (type == VoucherType.Accepted && ((double.IsNaN(totalMoney) && discount < 1000000) || discount <= totalMoney))
                {
                    { return (name, code, -discount); }
                }
            }
            return (null, null, 0);
        }

        public double GetDiscountRank(string rankName)
        {
            ReadDataTest readDataTest = new ReadDataTest();
            (string, double)[] rankList = readDataTest.Rank();

            for (int i = 0; i < rankList.Length; i++)
            {
                if (rankList[i].Item1 == rankName)
                {
                    return rankList[i].Item2;
                }
            }
            return 0;
        }



        //Tính toán dữ liệu
        //Tổng số sản phẩm trùng tên
        public int TotalProductContainName(string name)
        {
            //Lấy danh sách sản phẩm
            ReadDataTest readDataTest = new ReadDataTest();
            (string, string, string, double, int, string)[] productList = readDataTest.Product();
            int totalProduct = 0;

            string pattern = $@".*{name.ToLower()}.*";
            Regex regex = new Regex(pattern);


            for (int i = 0; i < productList.Length; i++)
            {
                string productName = productList[i].Item1;
                if (!string.IsNullOrWhiteSpace(productName))
                {
                    if (regex.IsMatch(productName.ToLower()))
                        totalProduct++;
                }
            }
            return totalProduct;
        }
    }
}
