using System;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;

namespace SudokuGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.ForegroundColor= ConsoleColor.Green;

            /*    i = inner    o = outer     */

            // width & height of inner block
            int iw = 3;
            int ih = 3;

            // width & height of outer block
            int ow = 3; 
            int oh = 3;

            int[,,,] board = new int[ow, oh, iw, ih];

            DrawSqaure(board);

            Console.WriteLine();
            //SmallSquare();
            //Random();
        }

        static void DrawSqaure(int[,,,] board)
        {
            // width & height of outer block
            int ow = board.GetLength(0);
            int oh = board.GetLength(1);
            int iw = board.GetLength(2);
            int ih = board.GetLength(3);
            string line = " " + new String('-', (oh * ih) * 4 - 1);


            Console.WriteLine(line);
            for (int oy = 0; oy < oh; oy++) // y coords of outer block
            {
                for (int iy = 0; iy < ih; iy++) // y coords of inner block
                {
                    Console.Write(" | "); // after each inner y
                    for (int ox = 0; ox < ow; ox++) // x coords of outer block
                    {

                        for (int ix = 0; ix < iw; ix++) // x coords of inner block
                        {
                            board[ox, oy, ix, iy] = ix;
                            //Console.Write($" {board[ox,oy,ix,iy]} ");
                        }

                        Console.Write(" |");
                    }

                    Console.WriteLine(); // end of inner line
                }
                Console.WriteLine(line);
                //Console.WriteLine(" ---+---+---+");
         
            }
        }

        static int[,] SmallSquare()
        {
            int[,] array = new int[3, 3];

            for (int i = 0; i < array.GetLength(0); i++)
            {
                array[i, 0] = i;
                //Console.WriteLine(array[i, 0]);

                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[0, j] = j;
                    Console.Write(" | " + array[0, j] + " | ");


                }

                Console.WriteLine("\n");
            }

            return array;
        }

        static void Random(int[,,,] board)
        {
            //Console.ForegroundColor = ConsoleColor.DarkGreen;

            int[] num = new int[9];

            int randNum;
            Random rnd = new Random();

            Console.WriteLine(num.Length);

            for (int i = 0; i < num.Length; i++)
            {
                do
                {
                    Console.Write(".");
                    randNum = rnd.Next(1, 10);

                }
                while (num.Contains(randNum));
                num[i] = randNum;
                Console.WriteLine($"\r\nNUM {i} : {num[i]}");

            }
        }
    }
}

