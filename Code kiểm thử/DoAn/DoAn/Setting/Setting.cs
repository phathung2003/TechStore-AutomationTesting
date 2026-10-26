using System;
using System.IO;

namespace DoAn.Setting
{
    public sealed class Setting
    {
        public static Setting Instance = new Setting();
        public string Website() { return "http://localhost/"; }
        private readonly string _projectDirectory;
        private Setting() { _projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")); }


        public int DelayTime() { return 5000; }
        public string ExcelOutputLocation()
        {
            return Path.Combine(_projectDirectory, "21DH113722 - Nguyễn Phát Hưng - Set 9.xlsx");
        }

        public string ExcelTestData() 
        {
            return Path.Combine(_projectDirectory, "Test Data/Test Data.xlsx");
        }

        public string ChromeDriverLocation()
        {
            return Path.Combine(_projectDirectory, "Chrome Driver/Current use/chromedriver.exe");
        }
    }
}
