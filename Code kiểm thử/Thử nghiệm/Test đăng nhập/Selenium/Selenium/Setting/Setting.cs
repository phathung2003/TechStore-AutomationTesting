using static System.Net.WebRequestMethods;

namespace Selenium.Setting
{
    public sealed class Setting
    { 
        public static Setting Instance = new Setting();
        public string Website()
        {
            return "http://localhost/";
        }
    }
}
