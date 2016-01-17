using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {

            string s = BLL.Tools.Format_Konto1("06333333334444444444555556");
            Debug.WriteLine(s);
                
        }
    }
}
