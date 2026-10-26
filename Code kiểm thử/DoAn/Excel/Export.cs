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

        public void ExportData(string ID, string testData, string expected, string actual, bool result, string note)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb;
            Worksheet ws;

            try { wb = excel.Workbooks.Open(filePath);}
            catch { throw new Exception("Không tìm thấy file Excel này");}

            try {ws = wb.Worksheets[worksheetNo];}
            catch{throw new Exception("Không có trang này trong file");}
            try
            {
                int totalRows = TotalRows(ws);
                // Duyệt qua từng hàng trong cột A
                for (int i = 5; i <= ws.Rows.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range cell = ws.Cells[i, 1]; // Cột A, hàng thứ i
                    string cellValue = cell.Value2?.ToString(); // Lấy giá trị của ô

                    //Có ID trong hệ thống
                    if (cellValue == ID)
                    {
                        ws.Cells[i, 5].Value = testData;
                        ws.Cells[i, 6].Value = note;
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
            catch (Exception ex)
            {
                wb.Save();
                wb.Close();
                excel.Quit();
                throw new Exception($"Ghi dữ liệu vào file Excel thất bại. \r\nGặp lỗi: {ex}");
            }
        }

        private void checkData()
        {
            if (string.IsNullOrEmpty(filePath)) { throw new Exception("Thiếu đường dẫn file"); }
            if (worksheetNo == default(int)) { throw new Exception("Thiếu số trang"); }
            if (worksheetNo < 0) { throw new Exception("Số trang không hợp lệ"); }
        }

        private int TotalRows(Worksheet ws)
        {
            int lastRow = ws.Cells.Find("*", System.Reflection.Missing.Value,
                            System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                            Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                            false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
            return lastRow;
        }
    }
}

