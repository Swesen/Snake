using System;
using System.Collections.Generic;

namespace Snake
{
    internal class Program
    {

        // global variables
        private static int gameSpeed = 300;

        private static int gameFieldSize = 45;
        private static int[,] virtualGameGrid = new int[gameFieldSize, gameFieldSize];

        private static Dictionary<string, string> visualBlocks = new Dictionary<string, string> {
            {"Snake", "██"},

            {"Food", " ♥"},

            {"Barrier", "▒▒"},

            {"Empty", "  "}
        };

        private static Dictionary<string, Vector2> Directions = new Dictionary<string, Vector2> {
            {"Left", new Vector2(-1,0) },
            {"Right", new Vector2(1,0) },
            {"Up", new Vector2(0,-1) },
            {"Down", new Vector2(0,1) }
        };

        // snake
        private static Vector2 startPosition = new Vector2(20, 20);

        private static Snake snake = new Snake(4, Directions["Right"], startPosition);

        private static void Main(string[] args)
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
                PlaySnake(virtualGameGrid, gameSpeed, snake);

                // print score
                LossScreen();

                // ask play again
            } while (playAgain);
        }

        /// <summary>
        /// Takes a value and returns the correct visualBlock
        /// </summary>
        /// <param name="gridValue"></param>
        /// <returns></returns>
        private static string GridContent(int gridValue)
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
        private static void WriteLineCentered(string s)
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
        private static void PlaySnake(int[,] gameField, int gameLoopMS, Snake snake)
        {
            SpawnFood(virtualGameGrid);
            bool gameOver = false;
            do
            {
                // start timer to keep track of execution time
                var timer = System.Diagnostics.Stopwatch.StartNew();

                // execute any game updating logick after this point
                gameOver = AdvanceSnakePosition(gameField, snake);

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
                                if (snake.Direction != Directions["Right"])
                                {
                                    snake.Direction = Directions["Left"]; 
                                }
                                break;

                            case ConsoleKey.RightArrow:
                                if (snake.Direction != Directions["Left"])
                                {
                                    snake.Direction = Directions["Right"]; 
                                }
                                break;

                            case ConsoleKey.UpArrow:
                                if (snake.Direction != Directions["Down"])
                                {
                                    snake.Direction = Directions["Up"]; 
                                }
                                break;

                            case ConsoleKey.DownArrow:
                                if (snake.Direction != Directions["Up"])
                                {
                                    snake.Direction = Directions["Down"]; 
                                }
                                break;

                            default:
                                break;
                        }
                    }
                } while (timer.ElapsedMilliseconds < gameLoopMS);

                timer.Stop();
            } while (!gameOver);
        }

        /// <summary>
        /// Updates the snake position in the gameField
        /// </summary>
        /// <param name="gameField">Reference the game grid</param>
        /// <param name="snake">Reference to the snake object</param>
        private static bool AdvanceSnakePosition(int[,] gameField, Snake snake)
        {
            HitDetection(gameField, snake.SnakePositions[0] + snake.Direction);
            bool gameOver = false;

            // deletes current positions
            for (int i = 0; i < snake.SnakePositions.Count; i++)
            {
                Vector2 newPos = snake.SnakePositions[i];
                if (newPos.X >= 0 && newPos.X < 45 && newPos.Y >= 0 && newPos.Y < 45)
                {
                    gameField[newPos.X, newPos.Y] = 0;
                    gameOver = false;
                }
                else
                {
                    return false;
                }
            }

            
            MoveSnakePositions(snake);
            // handle collisions


            // fill in new snake positions
            for (int i = 0; i < snake.SnakePositions.Count; i++)
            {
                Vector2 newPos = snake.SnakePositions[i];
                if (newPos.X >= 0 && newPos.X < 45 && newPos.Y >= 0 && newPos.Y < 45)
                {
                    gameField[newPos.X, newPos.Y] = i + 1;
                    gameOver = false;
                }
                else
                {
                    return true;
                }
            }
            return gameOver;
        }

        /// <summary>
        /// Moves the internal position of the referenced snake object
        /// </summary>
        /// <param name="snake">Reference to the snake object</param>
        private static void MoveSnakePositions(Snake snake)
        {
            for (int i = snake.SnakePositions.Count - 1; i > 0; i--)
            {
                snake.SnakePositions[i] = snake.SnakePositions[i - 1];
            }

            snake.SnakePositions[0] = snake.SnakePositions[0] + snake.Direction;
        }

        private static void LossScreen()
        {
            Console.Clear();
            WriteLineCentered("********************");
            WriteLineCentered("YOU LOSE");
            string a = "your score was: " + snake.Length;
            WriteLineCentered(a);
            WriteLineCentered("You SUCK");
            WriteLineCentered("My MoM CoUlD Do BeTtEr ThEn YoU");
            WriteLineCentered("Why aRe YoU UgLy?");
            WriteLineCentered("[Insert every horrible inslult here]");
            WriteLineCentered("********************");
            Console.ReadKey();
        }

        private static void SpawnFood(int[,] gameField)
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
            }
        }

        /// <summary>
        /// try/catch to look for snake going out of bounds. for the switch case: if -1, add to snake length and spawn food
        /// if 0, nothing happens since its empty space, and every other number is snake body
        /// </summary>
        /// <param name="gameField"></param>
        /// <param name="coordinates"></param>
        /// <returns>Returns false if snake collides with wall or itself. Returns true otherwise </returns>
        private static bool HitDetection(int[,] gameField, Vector2 coordinates)
        {
            try
            {
                switch (gameField[coordinates.X, coordinates.Y])
                {
                    case -1:
                        SpawnFood(virtualGameGrid);
                        return true;

                    case 0:
                        //empty space
                        return true;

                    default:
                        //collide with snake
                        LossScreen();
                        return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                //go out of map
                LossScreen();
                return false;
            }

        }

        /// <summary>
        /// An object for keeping track of everything related to the snake
        /// </summary>
        private struct Snake
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
        private struct Vector2
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

            public static bool operator ==(Vector2 a, Vector2 b)
            {
                if (a.X == b.X && a.Y == b.Y)
                {
                    return true;
                }
                return false;
            }

            public static bool operator !=(Vector2 a, Vector2 b)
            {
                if (a.X != b.X && a.Y != b.Y)
                {
                    return true;
                }
                return false;
            }
        }
        

    }
}