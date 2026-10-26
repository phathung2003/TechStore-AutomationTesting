using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string ID = "FSP005";
            //string testData = "JBL Tune Flex";
            //string excepted = "Chỉ hiện những sản phẩm có chứa tên JBL Tune Flex";
            //string actual = "Chỉ hiện những sản phẩm có chứa tên JBL Tune Flex";
            //bool result = false;

            //string filePath = "C:\\Users\\Hung\\Desktop\\DoAn\\21DH113722 - Nguyễn Phát Hưng - Set 9.xlsx";
            //Application excel = new Application();
            //Workbook wb;
            //Worksheet ws;

            //wb = excel.Workbooks.Open(filePath);
            //ws = wb.Worksheets[1];

            //int lastUsedRow = ws.Cells.Find("*", System.Reflection.Missing.Value,
            //                   System.Reflection.Missing.Value, System.Reflection.Missing.Value,
            //                   Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
            //                   false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
            //int temp;
            //// Duyệt qua từng hàng trong cột A
            //for (int i = 1; i <= ws.Rows.Count; i++)
            //{
            //    Range cell = ws.Cells[i, 1]; // Cột A, hàng thứ i
            //    string cellValue = cell.Value2?.ToString(); // Lấy giá trị của ô

            //    //Có ID trong hệ thống
            //    if (cellValue == ID)
            //    {
            //        ws.Cells[i, 5].Value = testData;
            //        ws.Cells[i, 7].Value = excepted;
            //        ws.Cells[i, 8].Value = actual;
            //        ws.Cells[i, 9].Value = result == true ? "Passed" : "Failed";
            //        break;
            //    }
            //}

            //wb.Save();
            //wb.Close();
            //excel.Quit();

            //Console.WriteLine(lastUsedRow);
            //Console.ReadLine();

            int length = 1000000;

            (int, int)[] ranges = { (48, 57), (65, 90), (97, 122) };
            Random random = new Random();

            char[] character = new char[length];
            for (int i = 0; i < length; i++)
            {
                (int, int) randomRange = ranges[random.Next(ranges.Length)];
                int stt = random.Next(randomRange.Item1, randomRange.Item2 + 1);
                character[i] = (char)stt;
            }

            Console.WriteLine(character);
            Console.ReadLine();
        }
    }
}
