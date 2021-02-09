﻿using System;
using System.Collections.Generic;

namespace Snake
{
    class Program
    {

        // global variables
        static int gameSpeed = 500;
        static int gameFieldSize = 45;
        static int[,] virtualGameGrid = new int[gameFieldSize, gameFieldSize];

        //   , ░░, ▒▒, ▓▓, ██
        static Dictionary<string, string> visualBlocks = new Dictionary<string, string> {
            {"Snake", "██"},

            {"Food", " ♥"},

            {"Barrier", "▒▒"}
        };
        static int snakeLength = 0;

        static void Main(string[] args)
        {
            // set window size
            Console.SetWindowSize(100, 50);
            Console.SetBufferSize(100, 50);

            bool playAgain = false;
            do
            {

                // welcome screen
                WelcomeScreen();

                // ask for player name


                // run game loop

                DrawVirtualGrid();
                // test snake
                virtualGameGrid[19, 21] = 1;
                virtualGameGrid[20, 21] = 2;
                virtualGameGrid[21, 21] = 3;
                virtualGameGrid[22, 21] = 4;
                virtualGameGrid[23, 21] = 5;
                var position = MoveWithoutUserinput();
                while (position > 1)
                {
                    position = MoveWithoutUserinput();
                }
                
                PlaySnake(gameSpeed);

                // print score



                // ask play again

            } while (playAgain);
        }

        static int MoveWithoutUserinput()
        {
            int positionHead = 22;
            for (int x = 0; x < virtualGameGrid.GetLength(0); x++)
            {
                for (int y = 0; y < virtualGameGrid.GetLength(1); y++)
                {
                    if(virtualGameGrid[x,y] == 1)
                    {
                        virtualGameGrid[x, y] = 0;
                        virtualGameGrid[x+1, y] = 0;
                        virtualGameGrid[x+2, y] = 0;
                        virtualGameGrid[x+3, y] = 0;
                        virtualGameGrid[x+4, y] = 0;

                        virtualGameGrid[x-1, y] = 1;
                        virtualGameGrid[x, y] = 2;
                        virtualGameGrid[x+1, y] = 3;
                        virtualGameGrid[x+2, y] = 4;
                        virtualGameGrid[x+3, y] = 5;
                        virtualGameGrid[x+4, y] = 0;
                        Console.Clear();
                        DrawVirtualGrid();
                        positionHead = x;
                    }

                }
            }
            return positionHead;
        }

        static string GridContent(int gridValue)
        {
            if (gridValue == -1)
            {
                return visualBlocks["Food"];
            }
            else if (gridValue > 0)
            {
                return visualBlocks["Snake"];
            }

            return "  ";
        }

        private static void DrawVirtualGrid()
        {
            // some test values
            
            // test food
            virtualGameGrid[10, 15] = -1;

            // debug draw virtual game field
            for (int i = 0; i < gameFieldSize + 2; i++)
            {
                Console.Write($"{visualBlocks["Barrier"]}");
            }
            Console.WriteLine();
            for (int x = 0; x < gameFieldSize; x++)
            {
                Console.Write($"{visualBlocks["Barrier"]}");
                for (int y = 0; y < gameFieldSize; y++)
                {
                    Console.Write($"{GridContent(virtualGameGrid[x, y])}");
                }
                Console.WriteLine($"{visualBlocks["Barrier"]}");
            }
            for (int i = 0; i < gameFieldSize + 2; i++)
            {
                Console.Write($"{visualBlocks["Barrier"]}");
            }
        }

        static void WriteLineCentered(string s)
        {
            Console.CursorLeft = (Console.WindowWidth - s.Length) / 2;
            Console.WriteLine(s);
        }

        private static void WelcomeScreen()
        {
            WriteLineCentered("Welcome to Snek!");
            WriteLineCentered("****************");
            WriteLineCentered("The best game!");
            Console.WriteLine();
            WriteLineCentered("Press any key to start");
            Console.ReadKey();
            Console.Clear();
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

                    //if (Console.KeyAvailable)
                    //{
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
                    //}


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
