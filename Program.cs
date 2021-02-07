using System;
using static System.Console;

namespace Snake
{
    class Program
    {
        ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
        char key = 'W';
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Snek!");
            Console.WriteLine("*******************************************");
            Console.WriteLine("The best game!");
        }

        public void Input()
        {
            if (Console.KeyAvailable)
            {
                keyInfo = ReadKey(true);
                key = keyInfo.KeyChar;
            }
        }
    }
}
