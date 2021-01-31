using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace Wallet
{
    class Manager : IManager
    {
        string purchacePath;
        List<Purchace> Purchaces { get; set; } = new List<Purchace>();
        List<Category> Categories { get; set; } = new List<Category>();

        public Manager()
        { }

        public void Load()
        {
            // string hd = System.Environment.GetEnvironmentVariable("HOME");
            string homeDir = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string dir = $"{homeDir}/.Wallet";

            if(!Directory.Exists(dir)) 
            { 
                DirectoryInfo di = Directory.CreateDirectory(dir); 
                di.Attributes |= FileAttributes.Hidden; 
            }

            this.purchacePath = $"{dir}/purchace.csv";

            if(!File.Exists(purchacePath))
            {
                var fs = File.Create(purchacePath);
                fs.Close();
            }
            else
            {
                Read();
            }          
        }

        private void Read()
        {
            foreach(var line in File.ReadLines(purchacePath))
            {
                string[] props = line.Split(',');

                DateTime date = DateTime.Parse(props[0]);
                decimal amount = Convert.ToDecimal(props[1], new CultureInfo("en-US"));
                string name = props[2];
                string categoryName = props[3];
                Category category = GetOrCreateCategory(categoryName);

                Purchace purchace = new Purchace() {
                    Date = date,
                    Amount = amount,
                    Name = name,
                    Category = category
                };
                Purchaces.Add(purchace);
            }
        }

        public void Save()
        {
            StreamWriter sw = File.CreateText(purchacePath);
            // var w = File.Open(purchacePath, FileMode.OpenOrCreate);
            foreach(var purchace in Purchaces)
            {
                sw.WriteLine(purchace.ToCsv());
            }
            sw.Close();
        }

        public void AddPurchace(Purchace purchace)
        {
            Purchaces.Add(purchace);
        }

        public Category GetOrCreateCategory(string categoryName)
        {
            int idx = this.Categories.FindIndex(c => c.Name == categoryName);
            Category category = null;

            if(idx >= 0)
            {
                category = this.Categories[idx];
            }
            else
            {
                category = new Category { Name = categoryName };
                this.Categories.Add(category);
            }
            return category;
        }

        public IEnumerable<Purchace> GetPurchaces()
        {
            return this.Purchaces;
        }

        public IEnumerable<Category> GetCategories()
        {
            return this.Categories;
        }
    }
}
