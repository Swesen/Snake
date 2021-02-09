using System;
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

        static Dictionary<string, Vector2> Directions = new Dictionary<string, Vector2> {
            {"Left", new Vector2(-1,0) },
            {"Right", new Vector2(1,0) },
            {"Up", new Vector2(0,-1) },
            {"Down", new Vector2(0,1) }
        };

        // snake
        static Vector2 startPosition = new Vector2(20, 20);
        static Snake snake = new Snake(4, Directions["Right"], startPosition);




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

                SpawnFood(virtualGameGrid);
                DrawVirtualGrid();


                PlaySnake(gameSpeed);

                // print score

                // ask play again

            } while (playAgain);
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
            Console.SetCursorPosition(0, 0);

            // debug draw virtual game field
            for (int i = 0; i < gameFieldSize + 2; i++)
            {
                Console.Write($"{visualBlocks["Barrier"]}");
            }
            Console.WriteLine();
            for (int y = 0; y < gameFieldSize; y++)
            {
                Console.Write($"{visualBlocks["Barrier"]}");
                for (int x = 0; x < gameFieldSize; x++)
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
            do
            {
                // start timer to keep track of execution time
                var timer = System.Diagnostics.Stopwatch.StartNew();

                // execute any game updating logick after this point
                AdvanceSnakePosition(virtualGameGrid);

                DrawVirtualGrid();
                // keep this last
                // loop read input controls until the next game update
                do
                {

                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);

                        // only accept keys that are used in game
                        switch (key.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                snake.Direction = Directions["Left"];
                                break;
                            case ConsoleKey.RightArrow:
                                snake.Direction = Directions["Right"];
                                break;
                            case ConsoleKey.UpArrow:
                                snake.Direction = Directions["Up"];
                                break;
                            case ConsoleKey.DownArrow:
                                snake.Direction = Directions["Down"];
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

        private static void AdvanceSnakePosition(int[,] gameField)
        {
            // set tail end to 0
            gameField[snake.SnakePositions[snake.Length - 1].X, snake.SnakePositions[snake.Length - 1].X] = 0;

            snake.MoveSnake();

            // fill in new snake positions
            for (int i = 0; i < snake.SnakePositions.Count; i++)
            {
                Vector2 newPos = snake.SnakePositions[i];
                gameField[newPos.X, newPos.Y] = i + 1;
            }
        }

        static void SpawnFood(int[,] gameField)
        {
            Random rnd = new Random();
            while (true)
            {
                //random coords
                int xCord = rnd.Next(0, 45);
                int yCord = rnd.Next(0, 45);
                if (gameField[xCord, yCord] == 0)
                {
                    gameField[xCord, yCord] = -1;
                    //dont know if necessary 
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        class Snake
        {
            public int Length;
            public Vector2 Direction;
            public List<Vector2> SnakePositions;

            public Snake(int length, Vector2 direction, Vector2 startPosition)
            {
                Length = length;
                Direction = direction;
                SnakePositions = new List<Vector2>();
                SnakePositions.Add(startPosition);
                for (int i = 1; i < Length; i++)
                {
                    SnakePositions.Add(SnakePositions[i - 1] - direction);
                }
            }

            public void MoveSnake()
            {
                if (Length > SnakePositions.Count)
                {
                    SnakePositions.Add(SnakePositions[SnakePositions.Count - 1]);
                }

                for (int i = SnakePositions.Count - 1; i < 0; i--)
                {
                    SnakePositions[i] = SnakePositions[i - 1];
                }

                SnakePositions[0] = SnakePositions[0] + Direction;
            }
        }

      
        struct Vector2
        {
            public int X;
            public int Y;

            public Vector2(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static Vector2 operator +(Vector2 a, Vector2 b)
            {
                return new Vector2(a.X + b.X, a.Y + b.Y);
            }

            public static Vector2 operator -(Vector2 a, Vector2 b)
            {
                return new Vector2(a.X - b.X, a.Y - b.Y);
            }
        }
    }
}
