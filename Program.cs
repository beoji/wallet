using System;
using System.Globalization;

namespace Wallet
{
    class Program
    {
        static void Main(string[] args)
        {
            IMenu menu = new Menu(new Manager(@"purchace.csv"));
            do 
                menu.Show(); 
            while(true);
        }
    }
}
