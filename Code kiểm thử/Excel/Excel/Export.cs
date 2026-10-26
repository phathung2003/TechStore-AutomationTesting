using Microsoft.Office.Interop.Excel;
using System;

namespace Excel
{
    public class Export
    {
        private string filePath;
        private int worksheetNo;

        public Export(string filePath, int worksheetNo)
        {
            this.filePath = filePath;
            this.worksheetNo = worksheetNo;
            checkData();
        }

        public void ExportData(string ID, string testData, string expected, string actual, bool result)
        {
            Application excel = new Application();
            Workbook wb;
            Worksheet ws;

            try
            {
                wb = excel.Workbooks.Open(filePath);
            }
            catch
            {
                throw new Exception("Không tìm thấy file Excel này");
            }

            try
            {
                ws = wb.Worksheets[worksheetNo];
            }
            catch
            {
                throw new Exception("Không có trang này trong file");
            }


            int temp;
            // Duyệt qua từng hàng trong cột A
            for (int i = 1; i <= ws.Rows.Count; i++)
            {
                Microsoft.Office.Interop.Excel.Range cell = ws.Cells[i, 1]; // Cột A, hàng thứ i
                string cellValue = cell.Value2?.ToString(); // Lấy giá trị của ô

                //Có ID trong hệ thống
                if (cellValue == ID)
                {
                    ws.Cells[i, 5].Value = testData;
                    ws.Cells[i, 7].Value = expected;
                    ws.Cells[i, 8].Value = actual;
                    ws.Cells[i, 9].Value = result == true ? "Passed" : "Failed";
                    break;
                }
            }
            wb.Save();
            wb.Close();
            excel.Quit();
        }

        private void checkData()
        {
            if (filePath == "" || filePath == null)
            {
                throw new Exception("Thiếu đường dẫn file");
            }
            if (worksheetNo == null)
            {
                throw new Exception("Thiếu số trang");
            }
            if (worksheetNo < 0)
            {
                throw new Exception("Số trang không hợp lệ");
            }
        }
    }
}
