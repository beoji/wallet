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
            string a = "[1] Miesięczne";
            string b = "[2] Całkowite";
            string c = "[3] Powrót";

            Console.WriteLine($"{a, -20} {b, -20} {c, -20}");

            byte number;
            do
            {
                Console.Write(">>> ");
            } while (!Byte.TryParse(Console.ReadLine(), out number) && number < 4 && number > 0);

            switch (number)
            {
                case 1:
                    do
                    {
                        Console.Write("[1-12] >>> ");
                    } while (!Byte.TryParse(Console.ReadLine(), out number) && number < 13 && number > 0);
                    this.ShowMonthlySummary(number);
                    break;
                case 2:
                    this.ShowTotalSummary();
                    break;
                case 3:
                    this.Show();
                    break;
            }
        }

        public void ShowMonthlySummary(int month)
        {
            var purchaces = _manager.GetPurchaces();
            var grouped = purchaces.GroupBy(c => c.Category);

            Console.WriteLine(new String('-', 54));
            Console.WriteLine(String.Format("{0,-40} {1,13}", "Kategoria", "Suma"));
            Console.WriteLine(new String('-', 54));

            foreach(var purchace in grouped)
            {
                var s = purchace
                    .Where(d => d.Date.Month == month)
                    .Select(p => p.Amount)
                    .Sum();
                var name = purchace.Key;

                Console.WriteLine($"{name,-40} {s,10} zł");
            }

            var sum = purchaces
                .Where(d => d.Date.Month == month)
                .Select(p => p.Amount)
                .Sum();

            Console.WriteLine(String.Format("{0,40} {1,10} zł", "Razem:", sum));
            Console.WriteLine(new String('-', 54));

        }

        public void ShowTotalSummary()
        {
            var purchaces = _manager.GetPurchaces();
            var grouped = purchaces.GroupBy(c => c.Category);

            Console.WriteLine(new String('-', 54));
            Console.WriteLine(String.Format("{0,-40} {1,13}", "Kategoria", "Suma"));
            Console.WriteLine(new String('-', 54));

            foreach(var purchace in grouped)
            {
                var s = purchace.Select(p => p.Amount).Sum();
                var name = purchace.Key;

                Console.WriteLine($"{name,-40} {s,10} zł");
            }

            var sum = purchaces.Select(p => p.Amount).Sum();

            Console.WriteLine(String.Format("{0,40} {1,10} zł", "Razem:", sum));
            Console.WriteLine(new String('-', 54));
        }
    }
}
