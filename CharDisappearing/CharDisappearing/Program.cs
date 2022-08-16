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
                Console.WriteLine("This is a text test. Press Esc to test and Enter to exit");
                Console.WriteLine("This");
                ConsoleKey c = Console.ReadKey().Key;
                if (c == ConsoleKey.Enter) break;
            }
            while (true);
        }
    }
}
