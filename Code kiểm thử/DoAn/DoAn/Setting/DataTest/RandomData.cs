using System.Collections.Generic;
using System;
using DoAn.Setting.Variable;

namespace DoAn.Setting.DataTest
{
    internal class RandomData
    {
        //Format chữ ngẫu nhiên
        public string RandomFormatInput(string input)
        {
            char[] charArray = input.ToLower().ToCharArray();
            bool capitalizeNext = true;

            for (int i = 0; i < charArray.Length; i++)
            {
                if (char.IsLetter(charArray[i]))
                {
                    if (capitalizeNext)
                    {
                        charArray[i] = char.ToUpper(charArray[i]);
                        capitalizeNext = false;
                    }
                    else
                    {
                        charArray[i] = char.ToLower(charArray[i]);
                        capitalizeNext = true;
                    }
                }
            }
            return new string(charArray);
        }

        //Lấy sản phẩm ngẫu nhiên (Cho số)
        public (string,double)[] RandomProduct(int numberOfProduct)
        {
            //Lấy danh sách sản phẩm
            ReadDataTest readDataTest = new ReadDataTest();
            (string, string, string, double, int, string)[] productList = readDataTest.Product();
            
            //Danh sách sản phẩm ngẫu nhiên
            (string,double)[] result = new (string, double)[numberOfProduct];
            int[] prodcutNo = GenerateNumbers(productList.Length, numberOfProduct);

            for (int i = 0; i < numberOfProduct; i++)
            {
                result[i] = (productList[prodcutNo[i]].Item1, productList[prodcutNo[i]].Item4);
            }

            return result;
        }

        //Lấy ngẫu nhiên 1 phần của tên sản phẩm
        public string RandomPartOfTheName()
        {
            while (true)
            {
                (string,double)[] productName = RandomProduct(1);
                string[] partOfName = productName[0].Item1.Split(' ');

                Random random = new Random();
                int position = random.Next(0, partOfName.Length);
                try
                {
                    if (!int.TryParse(partOfName[position], out _))
                    {
                        return partOfName[position];
                    }
                }
                catch
                {
                    Console.WriteLine(partOfName[position]);
                    return partOfName[position];
                }
            }
        }

        //Lấy mã OTP ngẫu nhiên
        public string RandomOTP()
        {
            int OTP = 0;
            bool exitvongLap = true;
            while (exitvongLap)
            {
                OTP = GenerateNumber(1, 999999);
                if (OTP.ToString() != "123456") {exitvongLap = false;}
            }
            return $"{OTP:000000}";
        }

        //Lấy mã khuyến mãi ngẫu nhiên
        public VoucherType RandomVoucherType()
        {
            Random random = new Random();
            int number = random.Next(1, 7);
            switch (number)
            {
                case 1: return VoucherType.NotExist;
                case 2: return VoucherType.OutDate;
                case 3: return VoucherType.OutOfStock;
                case 4: return VoucherType.Overload;
                case 5: return VoucherType.Accepted;
                default: return VoucherType.None;
            }
        }

        //Tạo thông tin giao hàng
        public (string, string, string) GenerateInfo(string type)
        {
            DateTime now = DateTime.Now;

            // Chuyển đổi đối tượng DateTime thành chuỗi số với định dạng mong muốn
            string dateString = now.ToString("ddMMyyHHmmss");
            string name = type.ToUpper() + " - " + dateString;
            string phone = "0903112202";
            string address = "TP.Hồ Chí Minh";

            return (name, phone, address);
        }


        //Random số lượng hàng hoá được chọn
        public (string, double, int, int)[] RandomProductQuantity(QuantityType quantityType, int numberOfProduct)
        {
            ReadDataTest readDataTest = new ReadDataTest();
            (string, string, string, double, int, string)[] productList = readDataTest.Product();
            
            (string, double)[] productChoose = RandomProduct(productList, numberOfProduct);
            (string, double, int, int)[] randomProductQuantity = new (string, double, int, int)[productChoose.Length];

            int maxStorage, quantity, expectedQuantity;
            for(int i = 0; i < randomProductQuantity.Length; i++)
            {
                switch (quantityType)
                {
                    case QuantityType.Negative:
                        quantity = GenerateNumber(-100, -1);
                        expectedQuantity = 1;
                        break;
                    case QuantityType.Overstock:
                        maxStorage = GetStorage(productList, productChoose[i].Item1);
                        quantity = GenerateNumber(maxStorage, maxStorage + 100);
                        expectedQuantity = 1;
                        break;
                    default:
                        maxStorage = GetStorage(productList, productChoose[i].Item1);
                        quantity = GenerateNumber(1, maxStorage);
                        expectedQuantity = quantity;
                        break;
                }
                randomProductQuantity[i] = (productChoose[i].Item1, productChoose[i].Item2, quantity, expectedQuantity);
            }
            return randomProductQuantity;
        }



        //-----------------------------------------------------//
        //Random vị trí
        private int[] GenerateNumbers(int maxValue, int count)
        {
            if (count > (maxValue + 1))
                throw new ArgumentException("Vượt quá số sản phẩm cho phép !");

            List<int> numbers = new List<int>();
            Random random = new Random();

            while (numbers.Count < count)
            {
                int randomNumber = random.Next(0, maxValue);
                if (!numbers.Contains(randomNumber))
                {
                    numbers.Add(randomNumber);
                }
            }

            int[] result = new int[count];
            for (int i = 0; i < numbers.Count; i++)
            {
                result[i] = numbers[i];
            }

            return result;
        }

        //Random 1 số
        public int GenerateNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max + 1);
        }

        //------------------------------------------------------//
        private int GetStorage((string, string, string, double, int, string)[] productList, string productName)
        {
            for (int i = 0; i < productList.Length; i++)
            {
                if (productList[i].Item1 == productName) { return productList[i].Item5; }
            }
            return 0;
        }
        
        //Lấy sản phẩm ngẫu nhiên (Cho danh sách sản phẩm)
        public (string, double)[] RandomProduct((string, string, string, double, int, string)[] productList, int numberOfProduct)
        {
            //Danh sách sản phẩm ngẫu nhiên
            (string, double)[] result = new (string, double)[numberOfProduct];
            int[] prodcutNo = GenerateNumbers(productList.Length, numberOfProduct);

            for (int i = 0; i < numberOfProduct; i++)
            {
                result[i] = (productList[prodcutNo[i]].Item1, productList[prodcutNo[i]].Item4);
            }

            return result;
        }
    }
}
