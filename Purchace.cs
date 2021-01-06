using System;
using System.Globalization;

namespace Wallet
{
    class Purchace
    {
        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Date.ToShortDateString(), -15} {Category, -20} {Amount, -10} {Name, -80}";
        }

        public string ToCsv()
        {
            return $"{Date},{Amount.ToString(new CultureInfo("en-US"))},{Name},{Category}";
        }
    }
}
