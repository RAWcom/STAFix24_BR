using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            String konto = "06101010780060572223000000";
            String s = BLL.Tools.Format_Konto1(konto);
            Debug.WriteLine(s);
        }
    
    
    
    }
}
