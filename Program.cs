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

        static Dictionary<string, string> visualBlocks = new Dictionary<string, string> {
            {"Snake", "██"},

            {"Food", " ♥"},

            {"Barrier", "▒▒"},

            {"Empty", "  "}
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
                LossScreen();
                WelcomeScreen();

                // ask for player name


                // run game loop

                DrawVirtualGrid();


                PlaySnake(virtualGameGrid, gameSpeed, snake);

                // print score

                // ask play again

            } while (playAgain);
        }

        /// <summary>
        /// Takes a value and returns the correct visualBlock
        /// </summary>
        /// <param name="gridValue"></param>
        /// <returns></returns>
        static string GridContent(int gridValue)
        {
            if (gridValue == 0)
            {
                return visualBlocks["Empty"];
            }
            else if (gridValue == -1)
            {
                return visualBlocks["Food"];
            }
            else if (gridValue > 0)
            {
                return visualBlocks["Snake"];
            }

            return "  ";
        }

        /// <summary>
        /// Call this to draw the virtualGameGrid with surounding walls
        /// </summary>
        private static void DrawVirtualGrid()
        {
            Console.SetCursorPosition(0, 0);
            string lineToDraw;
            lineToDraw = DrawBarrierLine();
            for (int y = 0; y < gameFieldSize; y++)
            {
                lineToDraw += $"{visualBlocks["Barrier"]}";
                for (int x = 0; x < gameFieldSize; x++)
                {
                    lineToDraw += $"{GridContent(virtualGameGrid[x, y])}";
                }
                lineToDraw += $"{visualBlocks["Barrier"]}\n";
            }
            lineToDraw += DrawBarrierLine();
            Console.WriteLine(lineToDraw);
        }

        private static string DrawBarrierLine()
        {
            string lineToDraw = "";
            for (int i = 0; i < gameFieldSize + 2; i++)
            {
                lineToDraw += $"{visualBlocks["Barrier"]}";
            }
            lineToDraw += "\n";
            return lineToDraw;
        }

        /// <summary>
        /// Prints a single line string centered in the console.
        /// </summary>
        /// <param name="s">String to print</param>
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
        static void PlaySnake(int[,] gameField, int gameLoopMS, Snake snake)
        {
            do
            {
                // start timer to keep track of execution time
                var timer = System.Diagnostics.Stopwatch.StartNew();

                // execute any game updating logick after this point
                AdvanceSnakePosition(gameField, snake);

                DrawVirtualGrid();
                // keep this last
                // loop read input controls until the next game update
                do
                {
                    // runs if a key on the keyboard is pressed
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

                timer.Stop();
            } while (true);
        }

        /// <summary>
        /// Updates the snake position in the gameField
        /// </summary>
        /// <param name="gameField">Reference the game grid</param>
        /// <param name="snake">Reference to the snake object</param>
        private static void AdvanceSnakePosition(int[,] gameField, Snake snake)
        {
            for (int i = 0; i < snake.SnakePositions.Count; i++)
            {
                Vector2 newPos = snake.SnakePositions[i];
                gameField[newPos.X, newPos.Y] = 0;
            }

            MoveSnake(snake);

            // fill in new snake positions
            for (int i = 0; i < snake.SnakePositions.Count; i++)
            {
                Vector2 newPos = snake.SnakePositions[i];
                gameField[newPos.X, newPos.Y] = i + 1;
            }
        }

        /// <summary>
        /// Moves the internal position of the referenced snake object
        /// </summary>
        /// <param name="snake">Reference to the snake object</param>
        private static void MoveSnake(Snake snake)
        {
            for (int i = snake.SnakePositions.Count - 1; i > 0; i--)
            {
                snake.SnakePositions[i] = snake.SnakePositions[i - 1];
            }

            snake.SnakePositions[0] = snake.SnakePositions[0] + snake.Direction;
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

        /// <summary>
        /// An object for keeping track of everything related to the snake
        /// </summary>
        struct Snake
        {
            public int Length;
            public Vector2 Direction;
            public List<Vector2> SnakePositions;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="length">Sets the starting length of the snake</param>
            /// <param name="direction">Sets the direction to move at the start</param>
            /// <param name="startPosition">Sets the position of the head</param>
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
        }


        /// <summary>
        /// An object for storing a position or a directional vector as integers
        /// </summary>
        struct Vector2
        {
            public int X;
            public int Y;

            public Vector2(int x, int y)
            {
                X = x;
                Y = y;
            }

            /// <summary>
            /// Addition operator, enables adding two vector2 together
            /// </summary>
            public static Vector2 operator +(Vector2 a, Vector2 b)
            {
                return new Vector2(a.X + b.X, a.Y + b.Y);
            }

            /// <summary>
            /// Subtraction operator, enables subtracting one vector2 from another
            /// </summary>
            public static Vector2 operator -(Vector2 a, Vector2 b)
            {
                return new Vector2(a.X - b.X, a.Y - b.Y);
            }
        }
        static void LossScreen()
        {
            Console.Clear();
            WriteLineCentered("********************");
            WriteLineCentered("YOU LOSE");
            string a = "your score was: " + snakeLength;
            WriteLineCentered(a);
            WriteLineCentered("You SUCK");
            WriteLineCentered("My MoM CoUlD Do BeTtEr ThEn YoU");
            WriteLineCentered("Why aRe YoU UgLy?");
            WriteLineCentered("[Insert every horrible inslult here]");
            WriteLineCentered("********************");
        }
    }
}
