using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Togyz
{
    public class TogyzBoard
    {
        const int NUM_KUMALAKS = 9;

        const int DRAW_GAME = NUM_KUMALAKS * NUM_KUMALAKS;

        const int TOTAL_KUMALAKS = DRAW_GAME * 2;

        const int TUZD = -1;

        private bool finished = false;

        private int gameResult = -2;

        private int[] fields = new int[23];

        private List<string> moves = new List<string>();

        public TogyzBoard()
        {
            for (int i = 0; i < 23; i++)
            {
                if (i < 18)
                {
                    fields[i] = NUM_KUMALAKS;
                }
                else
                {
                    fields[i] = 0;
                }
            }
        }

        public void printNotation()
        {
            string notation = "";
            for (int i = 0; i < moves.Count; i++)
            {
                if (i % 2 == 0)
                {
                    notation += (i / 2 + 1).ToString() + ". " + moves[i];
                }
                else
                {
                    notation += " " + moves[i] + "\n";
                }
            }
            Console.WriteLine (notation);
        }

        public void printPosition()
        {
            string position = "\n" + fields[21].ToString().PadLeft(6);
            for (int i = 17; i > 8; i--)
            {
                if (fields[i] == TUZD)
                {
                    position += "X".PadLeft(4);
                }
                else
                {
                    position += fields[i].ToString().PadLeft(4);
                }
            }

            position += "\n    ";

            for (int j = 0; j < 9; j++)
            {
                if (fields[j] == TUZD)
                {
                    position += "X".PadLeft(4);
                }
                else
                {
                    position += fields[j].ToString().PadLeft(4);
                }
            }

            position += fields[20].ToString().PadLeft(6);
            Console.WriteLine (position);
        }

        public void checkPosition()
        {
            int
                color,
                numWhite,
                numBlack;

            color = fields[22];
            numWhite = 0;
            for (int i = 0; i < 9; i++)
            {
                if (fields[i] > 0)
                {
                    numWhite += fields[i];
                }
            }

            numBlack = TOTAL_KUMALAKS - numWhite - fields[20] - fields[21];

            if ((color == 0) && (numWhite == 0))
            {
                fields[21] += numBlack;
            }
            else if ((color == 1) && (numBlack == 0))
            {
                fields[20] += numWhite;
            }

            if (fields[20] > DRAW_GAME)
            {
                finished = true;
                gameResult = 1;
            }
            else if (fields[21] > DRAW_GAME)
            {
                finished = true;
                gameResult = -1;
            }
            else if ((fields[20] == DRAW_GAME) && (fields[21] == DRAW_GAME))
            {
                finished = true;
                gameResult = 0;
            }
        }

        public String makeMove(int move)
        {
            int
                sow,
                color,
                num;
            bool tuzdCaptured = false;
            string madeMove;

            color = fields[22];
            madeMove = move.ToString();

            move = move + (color * 9) - 1;
            num = fields[move];

            if ((num == 0) || (num == TUZD))
            {
                Console.WriteLine("Incorrect move!");
                return "";
            }

            if (num == 1)
            {
                fields[move] = 0;
                sow = 1;
            }
            else
            {
                fields[move] = 1;
                sow = num - 1;
            }

            num = move;
            for (int i = 1; i <= sow; i++)
            {
                num += 1;
                if (num > 17)
                {
                    num = 0;
                }

                if (fields[num] == TUZD)
                {
                    if (num < 9)
                    {
                        fields[21] += 1;
                    }
                    else
                    {
                        fields[20] += 1;
                    }
                }
                else
                {
                    fields[num] += 1;
                }
            }

            if (fields[num] % 2 == 0)
            {
                if ((color == 0) && (num > 8))
                {
                    fields[20] += fields[num];
                    fields[num] = 0;
                }
                else if ((color == 1) && (num < 9))
                {
                    fields[21] += fields[num];
                    fields[num] = 0;
                }
            }
            else if (fields[num] == 3)
            {
                if (
                    (color == 0) &&
                    (fields[18] == 0) &&
                    (num > 8) &&
                    (num < 17) &&
                    (fields[19] != num - 8)
                )
                {
                    fields[18] = num - 8;
                    fields[num] = TUZD;
                    fields[20] += 3;
                    tuzdCaptured = true;
                }
                else if (
                    (color == 1) &&
                    (fields[19] == 0) &&
                    (num < 8) &&
                    (fields[18] != num + 1)
                )
                {
                    fields[19] = num + 1;
                    fields[num] = TUZD;
                    fields[21] += 3;
                    tuzdCaptured = true;
                }
            }

            if (color == 0)
            {
                fields[22] = 1;
            }
            else
            {
                fields[22] = 0;
            }

            if (num < 9)
            {
                num = num + 1;
            }
            else
            {
                num = num - 8;
            }

            madeMove += num.ToString();
            if (tuzdCaptured)
            {
                madeMove += "x";
            }

            moves.Add(madeMove);
            checkPosition();
            return madeMove;
        }

        public String makeRandomMove()
        {
            int
                move,
                color,
                randomIndex,
                randMove;
            string madeMove;
            List<int> possible = new List<int>();

            color = fields[22];
            for (int i = 1; i <= 9; i++)
            {
                move = i + (color * 9) - 1;
                if (fields[move] > 0)
                {
                    possible.Add(i);
                }
            }

            if (possible.Count == 0)
            {
                Console.WriteLine("No possible moves!");
                return "";
            }

            if (possible.Count == 1)
            {
                randomIndex = 0;
            }
            else
            {
                Random rnd = new Random();
                randomIndex = rnd.Next(possible.Count);
            }

            randMove = possible[randomIndex];
            madeMove = makeMove(randMove);
            return madeMove;
        }

        public bool isGameFinished()
        {
            return finished;
        }

        public String getScore()
        {
            return fields[20].ToString() + " - " + fields[21].ToString();
        }

        public int getResult()
        {
            return gameResult;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the TogyzKumalak world!\n");
            Console
                .WriteLine("Enter the mode (h - human play, m - random machine, r - against random AI): ");

            var input = Console.ReadLine();
            if (input == "h")
            {
                humanPlay();
            }
            else if (input == "m")
            {
                machinePlay();
            }
            else if (input == "r")
            {
                randomPlay();
            }
        }

        private static void humanPlay()
        {
            TogyzBoard tBoard = new TogyzBoard();

            while (true)
            {
                Console.WriteLine("\nEnter your move (1-9, 0 - exit): ");
                int input = int.Parse(Console.ReadLine());
                if (input == 0)
                {
                    break;
                }

                if ((input >= 1) && (input <= 9))
                {
                    tBoard.makeMove (input);
                    tBoard.printNotation();
                    tBoard.printPosition();
                    if (tBoard.isGameFinished())
                    {
                        break;
                    }
                }
            }

            tBoard.printNotation();
            tBoard.printPosition();
            Console.WriteLine($"\nGame over: {tBoard.getScore()}. Result: {tBoard.getResult()}");
        }

        private static void machinePlay()
        {
            int num;
            while (true)
            {
                Console.WriteLine("\nEnter number of iterations (1-100000): ");
                num = int.Parse(Console.ReadLine());
                if ((num >= 1) && (num <= 100000))
                {
                    break;
                }
            }

            int
                win = 0,
                draw = 0,
                loss = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < num; i++)
            {
                TogyzBoard tBoard = new TogyzBoard();
                while (!tBoard.isGameFinished())
                {
                    tBoard.makeRandomMove();
                }

                if (num <= 5)
                {
                    tBoard.printPosition();
                    Console.WriteLine($"\nGame over: {tBoard.getScore()}. Result: {tBoard.getResult()}\n");
                }

                if (tBoard.getResult() == 1)
                {
                    win += 1;
                }
                else if (tBoard.getResult() == -1)
                {
                    loss += 1;
                }
                else if (tBoard.getResult() == 0)
                {
                    draw += 1;
                }
                else
                {
                    Console.WriteLine("What??");
                }
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime =
                String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            Console.WriteLine("Elapsed: " + elapsedTime);
            Console.WriteLine($"W: {win}, D: {draw}, L: {loss}");
        }

        private static void randomPlay()
        {
            TogyzBoard tBoard = new TogyzBoard();
            int currentColor = 0;
            int
                color,
                move;
            string ai;

            while (true)
            {
                Console
                    .WriteLine("\nEnter your color (0 - white, 1 - black): ");
                color = int.Parse(Console.ReadLine());
                if ((color == 0) || (color == 1))
                {
                    break;
                }
            }

            while (!tBoard.isGameFinished())
            {
                if (currentColor == color)
                {
                    while (true)
                    {
                        Console
                            .WriteLine("\nEnter your move (1-9, 0 - exit): ");
                        move = int.Parse(Console.ReadLine());
                        if ((move >= 0) && (move <= 9))
                        {
                            break;
                        }
                    }

                    if (move == 0)
                    {
                        break;
                    }

                    tBoard.makeMove (move);
                    tBoard.printNotation();
                    tBoard.printPosition();
                    if (currentColor == 0)
                    {
                        currentColor = 1;
                    }
                    else
                    {
                        currentColor = 0;
                    }
                }
                else
                {
                    ai = tBoard.makeRandomMove();
                    Console.WriteLine("AI move:", ai);
                    tBoard.printPosition();
                    if (currentColor == 0)
                    {
                        currentColor = 1;
                    }
                    else
                    {
                        currentColor = 0;
                    }
                }
            }

            tBoard.printNotation();
            tBoard.printPosition();
            Console.WriteLine($"\nGame over: {tBoard.getScore()}. Result: {tBoard.getResult()}");
        }
    }
}
