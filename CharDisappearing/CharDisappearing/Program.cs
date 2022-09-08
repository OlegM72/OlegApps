using System;

namespace CharDisappearing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("This is the first line. If you press Esc when running, the first char disappears. Press Enter to exit");
                Console.WriteLine("This is a second line. The first letter does not disappear in it.");
                
                ConsoleKey c = Console.ReadKey().Key; // using ReadKey(true) solves the problem but does not show the reason of the problem
                if (c == ConsoleKey.Enter) break;   // leave the endless cycle
            }
            while (true);
        }
    }
}
