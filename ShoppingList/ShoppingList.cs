using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTasksDLL;

namespace ShoppingList 
{
    public class Item
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public Item(string name, double x, double y)
        {
            Name = name;
            Quantity = x;
            Price = y;
            Cost = x * y;
        }
        

    }
    
    class ShoppingList : CT 
    {
        static void Main(string[] args)
        {
            CT.Header("Shopping List", "");
            
            string fileName = Directory.GetCurrentDirectory();
            fileName = fileName.Replace(@"\ShoppingList\bin\Debug", @"\ShoppingList.txt");
            
            Preset(fileName);
            
            IfAddedMoreItems:           
            StreamReader file = new StreamReader(fileName);

            int lineNum = 0;
            string line;
            string fruit;
            double quantity, price, totalCost = 0;
            while ((line = file.ReadLine()) != null)
            {
                string[] set = line.Split(',');
                fruit = set[0].Trim();
                quantity = Convert.ToDouble(set[1].Trim());
                price = Convert.ToDouble(set[2].Trim());
                lineNum++;
                Item item = new Item(fruit, quantity, price);
                totalCost += item.Cost;
                Console.WriteLine("\nItem number {0}: {1}\nYou need {2}, each at {3:C}.\t\t"
                    +"Current total -- {4:C}", lineNum, item.Name, item.Quantity,
                    item.Price, totalCost);
                
            }
            CT.Color("magenta");
            Console.WriteLine("\n\nThe total cost is {0:C}", totalCost);
            CT.Color("white");
            Console.Write("Do you want to add any more items (Y/N): ");
            string moreItems = Console.ReadLine().ToLower();
            List<string> NewItem = new List<string>();
            int goToTop = 0;
            while (moreItems == "y")
            {
                goToTop++;
                moreItems = "";
                Console.WriteLine("What would you like to add to your shopping list");
                NewItem.Add(CT.AskUserForString("the fruit name") + ", " + CT.AskUserForDouble("the quantity of that fruit") + ", "
                + CT.AskUserForDouble("the price of each fruit") + ",");
                CT.Color("white");
                Console.Write("Do you want to add any more items enter (Y/N): ");
                moreItems = Console.ReadLine().ToLower();
            }
            file.Close();

            if (goToTop > 0)
            {
                FileStream fappend = File.Open(fileName, FileMode.Append);
                StreamWriter writer = new StreamWriter(fappend);
                foreach (string newItem in NewItem)
                {
                    writer.WriteLine(newItem);
                }
                writer.Close();
                goto IfAddedMoreItems;
            }

            Console.WriteLine("Do you want to print a copy of your new shopping list (Y/N)");
            if (Console.ReadLine().ToLower() == "y")
                PrintCopy(fileName);

            CT.Footer();
        }
        public static void PrintCopy(string x)
        {

            Console.Write("What would you like to name your shopping list...");
            string nameOfCopy = Console.ReadLine();
            File.Copy(x, x.Replace("ShoppingList", nameOfCopy), true);
            Console.WriteLine("Done!");
        }
        public static void Preset(string x)
        {
            //Puts back original
            string[] original = new string[] { "apple, 12, 0.64,", "peach, 7, 0.36,", "pear, 9, 0.47," };
            File.WriteAllLines(x, original);

        }
    }
}
