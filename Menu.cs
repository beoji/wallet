using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;


namespace Wallet
{
    class Menu : IMenu
    {
        IManager _manager;

        public Menu(IManager manager)
        {
            _manager = manager;
            _manager.Load();
        }

        public void Show()
        {          
            string a = "[1] Wprowadź zakup";
            string b = "[2] Lista zakupów";
            string c = "[3] Lista kategorii";
            string d = "[4] Podsumowanie";
            string e = "[5] Zakończ";

            Console.WriteLine($"{a, -20} {b, -20} {c, -20} {d, -20} {e, -20}");

            byte number;
            do
            {
                Console.Write(">>> ");
            } while (!Byte.TryParse(Console.ReadLine(), out number) && number < 6 && number > 0);

            switch (number)
            {
                case 1:
                    this.AddPurchace();
                    break;
                case 2:
                    this.ShowPurchaces();
                    break;
                case 3:
                    this.ShowCategories();
                    break;
                case 4:
                    this.ShowSummary();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }

        public void AddPurchace()
        {
            decimal amount;

            do
            {
                Console.Write("Wprowadź poprawną kwotę: ");
            } while(!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, new CultureInfo("en-US"), out amount));
            
            Console.Write("Wprowadź nazwę: ");
            string name = Console.ReadLine();
            
            Console.Write("Wprowadź kategorię: ");
            string categoryName = Console.ReadLine();
            Category category = _manager.GetOrCreateCategory(categoryName);
            
            var purchace = new Purchace() {
                Date = DateTime.Now,
                Amount = amount,
                Name = name,
                Category = category
            };

            _manager.AddPurchace(purchace);
            _manager.Save();
        }

        public void ShowPurchaces()
        {
            var purchaces = _manager.GetPurchaces();
            foreach(var purchace in purchaces)
            {
                Console.WriteLine(purchace);
            }
        }

        public void ShowCategories()
        {
            var categories = _manager.GetCategories();
            foreach(var category in categories)
            {
                Console.WriteLine(category);
            }
        }

        public void ShowSummary()
        {
            decimal sum = 0;
            var purchaces = _manager.GetPurchaces();
            foreach(var purchace in purchaces)
            {
                sum += purchace.Amount;
            }
            Console.WriteLine($"Suma: {sum,10} zł");
        }
    }
}
