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
            Console.WriteLine("1. Wprowadź zakup");
            Console.WriteLine("2. Pokaż listę zakupów");
            Console.WriteLine("3. Pokaż listę kategorii");
            Console.WriteLine("4. Zakończ");

            byte number;
            do
            {
                Console.Write("Twój wybór >>> ");
            } while (!Byte.TryParse(Console.ReadLine(), out number) && number < 5 && number > 0);

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
    }
}
