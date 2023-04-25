using System;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;

namespace SudokuGame {
    public class Program {
        static void Main(string[] args) {
            //Console.ForegroundColor= ConsoleColor.Green;

            /*    i = inner    o = outer     */

            // width & height of inner block
            int iw = 3;
            int ih = 3;

            // width & height of outer block
            int ow = 3;
            int oh = 3;

            int[,,,] board = new int[ow, oh, iw, ih];
            

            GeneratePuzzle(board);
            DrawSqaure(board);


            Console.WriteLine(board.Length);
        }



       


        // Displaying the Grid
        static void DrawSqaure(int[,,,] board) {
            // width & height of outer block
            int ow = board.GetLength(0);
            int oh = board.GetLength(1);
            int iw = board.GetLength(2);
            int ih = board.GetLength(3);
            string line = " " + new String('-', (oh * ih) * 4 - 1);

            //Console.Clear();
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
                            if (board[ox, oy, ix, iy] == 0)
                            {
                                Console.ForegroundColor= ConsoleColor.Green;
                                Console.Write($" {board[ox, oy, ix, iy]} ");
                                Console.ForegroundColor = ConsoleColor.White;


                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($" {board[ox, oy, ix, iy]} ");

                            }

                            // board[ox, oy, ix, iy] = ix;
                            //Console.Write($" {board[ox, oy, ix, iy]} ");
                        }

                        Console.Write(" |");
                    }

                    Console.WriteLine(); // end of inner line
                }
                Console.WriteLine(line);
                //Console.WriteLine(" ---+---+---+");

            }
        }

        public static List<int> getRandomSet() {
            var _rng = new Random();
            var array = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (var n = array.Count; n > 1;) {
                var k = _rng.Next(n);
                n--;
                (array[k], array[n]) = (array[n], array[k]);
            }
            Console.WriteLine(String.Join(", ", array.ToArray()));
            return array;
        }

        static void DeleteSpace(int[,,,] board)
        {
            int ow = board.GetLength(0);
            int oh = board.GetLength(1);
            int iw = board.GetLength(2);
            int ih = board.GetLength(3);

            Random rnd = new Random();
            int randNum;

            // number of spaces to remove (difficulty)
            int numToRemove = 10;


            for (int i = 0; i <= 70; i++)
            {
                
                int ox = rnd.Next(0,3);
                int oy = rnd.Next(0,3);
                int ix = rnd.Next(0,3);
                int iy = rnd.Next(0,3);

                board[ox, oy, ix, iy] = 0;
            }



        }


        static void GeneratePuzzle(int[,,,] board)
        {
            int ow = board.GetLength(0);
            int oh = board.GetLength(1);
            int iw = board.GetLength(2);
            int ih = board.GetLength(3);

            // top row
            HashSet<int> usedNums = new HashSet<int>();
            Random random = new Random();

            int randNum;

            for (int ox = 0; ox < ow; ox++)
            {
                for (int ix = 0; ix < iw; ix++)
                {
                    do
                    {
                        randNum = random.Next(1, 10);

                    }
                    while (usedNums.Contains(randNum));

                    usedNums.Add(randNum);
                    board[ox, 0, ix, 0] = randNum;
                }
            }


            Solve(board);

            DeleteSpace(board);



        }


        static bool Solve(int[,,,] board)
        {
            int ow = board.GetLength(0);
            int oh = board.GetLength(1);
            int iw = board.GetLength(2);
            int ih = board.GetLength(3);

            Random rnd = new Random();
            int randNum;

            for (int ox = 0; ox < ow; ox++) // y coords of outer block
            {
                for (int oy = 0; oy < oh; oy++) // y coords of inner block
                {
                    for (int ix = 0; ix < iw; ix++) // x coords of outer block
                    {
                        for (int iy = 0; iy < ih; iy++) // x coords of inner block
                        {
                            if (board[ox, oy, ix, iy] == 0) // empty space
                            {
                                for (int num = 1; num <= 9; num++)
                                {
                                    if (isValid(board, ox, oy, ix, iy, num))
                                    {
                                        board[ox, oy, ix, iy] = num;

                                        if (Solve(board))
                                        {
                                            return true;
                                        }

                                        board[ox, oy, ix, iy] = 0;
                                    }
                                }

                                return false;
                            }
                        }

                    }
                }
            }


            return true;
        }





            // Generates a sudoku puzzle using backtracking algorithm
            static void GeneratePuzzle_OLD(int[,,,] board)
            {
                int ow = board.GetLength(0);
                int oh = board.GetLength(1);
                int iw = board.GetLength(2);
                int ih = board.GetLength(3);


                Random rnd = new Random();

                HashSet<int> usedNums = new HashSet<int>();


                // top row of the puzzle

                for (int oy = 0; oy < oh; oy++) // y coords of outer block
                {
                    for (int ox = 0; ox < ow; ox++) // x coords of outer block
                    {
                        Console.WriteLine($"ox {ox} oy {oy}");
                        var isOk = false;
                        var retryCount = 0;
                        while (!isOk)
                        {
                            //retryCount++;
                            //if (retryCount == 1) {
                            //    if (ox > 0) ox--;
                            //    Console.WriteLine($"Decrease ox {ox}");

                            //} else if (retryCount == 2) {
                            //    if (oy > 0) oy--;
                            //    Console.WriteLine($"Decrease oy {oy}");
                            //    retryCount = 0;
                            //}

                            // DrawSqaure(board);
                            //  Console.Clear();
                            isOk = true;
                            for (int iy = 0; iy < ih; iy++) // y coords of inner block
                            {
                                var randSet = getRandomSet();

                                for (int ix = 0; ix < iw; ix++) // x coords of inner block
                                {
                                    for (var cnt = 0; cnt < 9; cnt++)
                                    {
                                        var randNum = randSet[cnt];

                                        if (isValid(board, ox, oy, ix, iy, randNum))
                                        {
                                            board[ox, oy, ix, iy] = randNum;
                                            Console.WriteLine($"{ox},{oy},{ix},{iy}  ??? {randNum}");
                                            break;

                                        }
                                        //if (cnt == 8) {
                                        //    isOk = false;
                                        //    Console.WriteLine("RETRY...");
                                        //}
                                    }

                                }

                            }
                        }

                    }

                }

            }


            // Checks if number is present in row, col or inner 3x3 square
            static bool isValid(int[,,,] board, int outerCol, int outerRow, int innerCol, int innerRow, int num)
            {
                int ow = board.GetLength(0);
                int oh = board.GetLength(1);
                int iw = board.GetLength(2);
                int ih = board.GetLength(3);

                //col
                for (int ox = 0; ox < ow; ox++)
                {
                    for (int ix = 0; ix < iw; ix++)
                    {
                        if (board[ox, outerRow, ix, innerRow] == num)
                        {
                            return false;

                        }
                    }
                }

                //row
                for (int oy = 0; oy < oh; oy++)
                {
                    for (int iy = 0; iy < ih; iy++)
                    {
                        if (board[outerCol, oy, innerCol, iy] == num)
                        {
                            return false;

                        }
                    }
                }

                // inner square
                for (int iy = 0; iy < ih; iy++)
                {
                    for (int ix = 0; ix < iw; ix++)
                    {
                        //Console.WriteLine($"{outerCol},{outerRow},{ix},{iy}:{board[outerCol, outerRow, ix, iy]}  ??? {num}");
                        //Console.WriteLine($"{outerCol},{outerRow},{ix},{iy}: {board[outerCol, outerRow, ix, iy]}  ??? {num}");
                        if (board[outerCol, outerRow, ix, iy] == num)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }



            //static bool isValid_OLD(int[,,,] board, int outerRow, int outerCol, int innerRow, int innerCol, int num) {
            //    int ow = board.GetLength(0);
            //    int oh = board.GetLength(1);
            //    int iw = board.GetLength(2);
            //    int ih = board.GetLength(3);



            //    // row
            //    for (int ox = 0; ox < ow; ox++) {
            //        for (int ix = 0; ix < iw; ix++) {
            //            if (board[ox, outerRow, ix, innerRow] == num) {
            //                return false;

            //            }
            //        }
            //    }

            //    // col
            //    for (int oy = 0; oy < oh; oy++) {
            //        for (int iy = 0; iy < ih; iy++) {
            //            if (board[outerCol, oy, innerCol, iy] == num) {
            //                return false;

            //            }
            //        }
            //    }

            //    // inner square
            //    for (int ix = 0; ix < iw; ix++) {
            //        for (int iy = 0; iy < ih; iy++) {
            //            if (board[outerCol, outerRow, ix, iy] == num) {
            //                return false;
            //            }
            //        }
            //    }
            //    return true;
            //}



    }
}