using System;
using System.Runtime.Serialization.Formatters;

namespace SudokuGame
{
    public class Program
    {
        static void Main(string[] args)
        {



            int[,] numbers = new int[3, 3];

            for (int i = 0; i < numbers.GetLength(0); ++i)
            {
                for (int j = 0; j < numbers.GetLength(1); ++j)
                {
                    numbers[0, j] = j;

                    Console.Write($" | {numbers[0, j]} |");
                }
                numbers[i, 0] = i;

                Console.WriteLine("\n");
            }

            Console.ReadLine();
        }

        static void Random()
        {
            //Console.ForegroundColor = ConsoleColor.DarkGreen;

            //int[] num = new int[9];
            //int[] prevNums = new int[20];

            //int prevNum = 10;
            //int randNum;
            //Random rnd = new Random();

            //Console.WriteLine(num.Length);

            //for (int i = 0; i < num.Length; i++)
            //{
            //    do
            //    {
            //        Console.Write(".");
            //       randNum = rnd.Next(1, 10);

            //    }
            //    while (num.Contains(randNum));
            //    num[i] = randNum;
            //    Console.WriteLine($"\r\nNUM {i} : {num[i]}");

            //}
        }


    }

}
