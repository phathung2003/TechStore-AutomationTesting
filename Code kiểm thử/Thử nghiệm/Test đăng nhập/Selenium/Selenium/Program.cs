using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{
    internal class Program
    {
        static void Main(string[] args)
        {
         Selenium demo = new Selenium();
            demo.Setup();
            demo.Input_Invalid("1", "1");
            //demo.TearDown();
            Console.ReadLine();
        }
    }
}
