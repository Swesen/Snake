using System;
using System.Collections.Generic;

namespace Snake
{
    class Program
    {
        // global variables
        static int gameSpeed = 500;
        static int snakeLength = 0;

        static void Main(string[] args)
        {
            bool playAgain = false;
            do
            {
                int[,] gameField = new int[50, 2];
                for (int i = 0; i < 50; i++)
                {
                    gameField[i, 0] = i;
                    Console.Write(gameField[i, 0] + " ");
                }

                // welcome screen
                //WelcomeScreen();

                // ask for player name

                // run game loop
                PlaySnake(gameSpeed);

                // print score

                // ask play again

            } while (playAgain);
        }

        private static void WelcomeScreen()
        {
            Console.WriteLine("Welcome to Snek!");
            Console.WriteLine("****************");
            Console.WriteLine(" The best game! ");
            Console.WriteLine("\nPress any key to start");
            Console.ReadKey();
        }


        /// <summary>
        /// Main game, loops through the functions required to play the game.
        /// </summary>
        /// <param name="gameLoopMS">The time in milliseconds in between each game loop</param>
        static void PlaySnake(int gameLoopMS)
        {
            ConsoleKey userInput = new ConsoleKey();
            do
            {
                // start timer to keep track of execution time
                var timer = System.Diagnostics.Stopwatch.StartNew();

                // execute any game updating logick after this point

                // check userInput if the snake is going to turn this update


                // keep this last
                // loop read input controls until the next game update
                // clear userInput from last game update
                userInput = new ConsoleKey();
                do
                {

                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);

                        // only accept keys that are used in game
                        switch (key.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                userInput = key.Key;
                                break;
                            case ConsoleKey.RightArrow:
                                userInput = key.Key;
                                break;
                            case ConsoleKey.UpArrow:
                                userInput = key.Key;
                                break;
                            case ConsoleKey.DownArrow:
                                userInput = key.Key;
                                break;
                            default:
                                break;
                        }
                    }
                } while (timer.ElapsedMilliseconds < gameLoopMS);

                // debug input
                //if (userInput != ConsoleKey.Escape)
                //{
                //    Console.Write(userInput.ToString());
                //}

                timer.Stop();
            } while (true);
        }
        

    }
}
