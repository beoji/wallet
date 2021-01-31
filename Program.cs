namespace Wallet
{
    class Program
    {
        static void Main(string[] args)
        {         
            IMenu menu = new Menu(new Manager());
            do 
                menu.Show(); 
            while(true);
        }
    }
}
