using System;
using System.Collections.Generic;

namespace Snake
{
    class Program
    {
        // global variables

        static void Main(string[] args)
        {
            bool playAgain = false;
            do
            {
                // welcome screen
                WelcomeScreen();

                // ask for player name

                // run game loop
                Game(500);

                // print score

                // ask play again

            } while (playAgain);
        }

        private static void WelcomeScreen()
        {
            Console.WriteLine("Welcome to Snek!");
            Console.WriteLine("****************");
            Console.WriteLine(" The best game! ");
            Console.ReadKey();
        }

        // gameLoopMS controls the time between updates
        static void Game(int gameLoopMS)
        {
            do
            {
                // start timer to keep track of execution time
                var timer = System.Diagnostics.Stopwatch.StartNew();

                // execute any game updating logick after this point


                // after every update read user input controls untill the next game update
                ConsoleKey input = new ConsoleKey();
                do
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);

                        // only accept keys that are used in game
                        switch (key.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                input = key.Key;
                                break;
                            case ConsoleKey.RightArrow:
                                input = key.Key;
                                break;
                            case ConsoleKey.UpArrow:
                                input = key.Key;
                                break;
                            case ConsoleKey.DownArrow:
                                input = key.Key;
                                break;
                            default:
                                break;
                        }
                    }
                } while (timer.ElapsedMilliseconds < gameLoopMS);

                // debug
                if (input != ConsoleKey.Escape)
                {
                    Console.Write(input.ToString());
                }

                timer.Stop();

            } while (true);
        }
    }
}
