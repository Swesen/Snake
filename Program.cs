﻿using System;

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
    }
}
