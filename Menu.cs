using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Menu
    {
        public static int DisplayChoiceMenu(List<String> choices, bool clear = true)
        {
            int choice = 0;

            while (true)
            {
                if (clear)
                {
                    Console.Clear();
                }

                foreach (string item in choices)
                {
                    Console.WriteLine(item);
                }

                Console.Write("Choose an option: ");

                if (Int32.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= choices.Count)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice, please try again.");
                }
            }

            return choice;
        }

        public static void DrawLine(char c = '-')
        {
            string line = new string('-', Console.WindowWidth);
            Console.WriteLine(line);
        }
    }
}
