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
            var purchaces = _manager.GetPurchaces().ToList();
            Decorate();
            purchaces.ForEach((item) => Console.WriteLine(item));
            Decorate();
        }

        public void ShowCategories()
        {
            var categories = _manager.GetCategories().ToList();
            Decorate();
            categories.ForEach((item) => Console.WriteLine(item));
            Decorate();
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

        public void ShowMonthlySummary(int month=0)
        {
            Decorate();
            Console.WriteLine(String.Format("{0,-40} {1,-10} {2,13}", "Kategoria", "% całości", "Suma"));
            Decorate();

            var purchaces = _manager.GetPurchaces();

            var sum = purchaces
                .Where(d => d.Date.Month == month)
                .Select(p => p.Amount)
                .Sum();
            
            var summary = new Dictionary<string, (decimal, decimal)>();
            var grouped = purchaces.GroupBy(c => c.Category);

            foreach(var purchace in grouped)
            {
                var s = purchace
                    .Where(d => d.Date.Month == month)
                    .Select(p => p.Amount)
                    .Sum();
                var name = purchace.Key.ToString();
                var percent = (s / sum) * 100;
                if(s != 0)
                    summary[name] = (Math.Round(percent), s);
            }

            foreach(var s in summary.OrderByDescending(i => i.Value.Item2))
            {
                Console.WriteLine($"{s.Key,-40} {s.Value.Item1,-10} {s.Value.Item2,10} zł");
            }

            Decorate();
            Console.WriteLine(String.Format("{0,50} {1,11} zł", "Razem:", sum));
            Decorate();

        }

        public void ShowTotalSummary()
        {
            Decorate();
            Console.WriteLine(String.Format("{0,-40} {1,-10} {2,13}", "Kategoria", "% całości", "Suma"));
            Decorate();

            var purchaces = _manager.GetPurchaces();
            var sum = purchaces.Select(p => p.Amount).Sum();
            
            var summary = new Dictionary<string, (decimal, decimal)>();
            var grouped = purchaces.GroupBy(c => c.Category);

            foreach(var purchace in grouped)
            {
                var s = purchace.Select(p => p.Amount).Sum();
                var name = purchace.Key.ToString();
                var percent = (s / sum) * 100;
                if(s != 0)
                    summary[name] = (Math.Round(percent), s);
            }

            foreach(var s in summary.OrderByDescending(i => i.Value.Item2))
            {
                Console.WriteLine($"{s.Key,-40} {s.Value.Item1,-10} {s.Value.Item2,10} zł");
            }

            Decorate();
            Console.WriteLine(String.Format("{0,50} {1,11} zł", "Razem:", sum));
            Decorate();
        }

        private void Decorate() => Console.WriteLine(new String('-', 65));
    }
}
