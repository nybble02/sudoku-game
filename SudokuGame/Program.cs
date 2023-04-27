using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Xml;

namespace SudokuGame
{
    public class Program
    {

        static int difficulty;
        static int[,,,] boardSaved;
        static void Main(string[] args)
        {
            ////Console.ForegroundColor= ConsoleColor.Green;

            ///*    i = inner    o = outer     */

            //// width & height of inner block
            //int iw = 3;
            //int ih = 3;

            //// width & height of outer block
            //int ow = 3;
            //int oh = 3;

            //int[,,,] board = new int[ow, oh, iw, ih];
            //boardSaved = new int[ow, oh, iw, ih];


            ////Console.WriteLine(9 % 3);




            //ChooseDifficultyOptions();
            //// Console.WriteLine(difficulty);
            
            //GeneratePuzzle(board);

            ////Gameplay(board);
            ///


            SudokuGame sudokuGame= new SudokuGame();



        }


        static int ChooseDifficultyOptions()
        {
            Console.WriteLine(" Welcome to sudoku!");
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine(" Please select a difficulty");
                Console.WriteLine(" 1 - Easy  ");
                Console.WriteLine(" 2 - Medium");
                Console.WriteLine(" 3 - Hard  ");
                Console.WriteLine(" 3 - Custom  ");


                string userInput = Console.ReadLine();

                if (userInput == "1")
                {
                    difficulty = 1;
                    break;
                }
                else if (userInput == "2")
                {
                    difficulty = 2;
                    break;
                }
                else if (userInput == "3")
                {
                    difficulty = 3;
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" Input is not recognised ~ please try again\n");
                }
            }

            return difficulty;
        }

        static void Gameplay(int[,,,] board)
        {

            string userChoice;
            while (true)
            {
                if (isDone(board))
                {
                    break;
                }

                DrawBoard(board);
                Console.WriteLine("Please choose an option");
                Console.WriteLine("(1) Add a number");
                Console.WriteLine("(2) Undo move");
                Console.WriteLine("(3) Generate new board");
                Console.WriteLine("(4) Check board");
                Console.WriteLine("(5) Terminate Game");

                userChoice = Console.ReadLine();

                if (userChoice == "1")
                {
                    PlaceNumber(board);
                }
                else if (userChoice == "2")
                {
                    DeleteNumber(board);
                    break;
                }
                else if (userChoice == "3")
                {
                    GeneratePuzzle(board);
                    break;
                }
                else if (userChoice == "4")
                {
                    // Check board
                    break;
                }
                else if (userChoice == "4")
                {
                    // Terminate function
                    Console.WriteLine(" Thank you for playing");
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" Input is not recognised ~ please try again");
                    Console.WriteLine("");
                }

            }



        }

        static void PlaceNumber(int[,,,] board)
        {
            int ox = 0, oy = 0, ix = 0, iy = 0;
            string userInputRow;
            string userInputCol;
            while (true)
            {
                userInputCol = InputOptions(false, board);
                userInputRow = InputOptions(true, board);
                char charCol = Convert.ToChar(userInputCol);
                char charRow = Convert.ToChar(userInputRow);

                int asciiCol = Convert.ToInt32(charCol);
                int asciiRow = Convert.ToInt32(charRow);

                int col = asciiCol & 15;
                int row = asciiRow & 15;

                Console.WriteLine();

                // ox 
                ox = col / board.GetLength(0);
                if (col % board.GetLength(0) == 0)
                {
                    ox--;
                }

                // oy
                oy = row / board.GetLength(1);
                if (row % board.GetLength(1) == 0)
                {
                    oy--;

                }

                //ix
                if (ox == 1)
                {
                    ix = (col - board.GetLength(2)) - 1;
                }
                else if (ox == 2)
                {
                    ix = (col - (board.GetLength(2) * 2)) - 1;
                }
                else
                {
                    ix = col - 1;
                }

                //iy
                if (oy == 1)
                {
                    iy = (row - board.GetLength(3)) - 1;
                }
                else if (oy == 2)
                {
                    iy = (row - (board.GetLength(3) * 2)) - 1;
                }
                else
                {
                    iy = row - 1;
                }


                if (board[ox, oy, ix, iy] == 0)
                {
                    break;

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($" Please choose a position that is not occupied");
                    Console.WriteLine($" Previous Choice ~ Column: {userInputCol.ToUpper()} Row: {userInputRow.ToUpper()}");
                    Console.WriteLine();
                    DrawBoard(board);

                }

            }

            int minValue = 1;
            int maxValue = 9;
            string userInput;
            int userNumber;
            bool isNumber;

            while (true)
            {
                Console.WriteLine($" Please enter a number between {minValue} and {maxValue}:");
                userInput = Console.ReadLine();

                isNumber = int.TryParse(userInput, out userNumber);

                if (isNumber && userNumber >= minValue && userNumber <= maxValue)
                {
                    if (isValid(board, ox, oy, ix, iy, userNumber))
                    {
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(" Number placed in invalid position");
                        Console.WriteLine($" Column: {userInputCol.ToUpper()} Row: {userInputRow.ToUpper()}");
                        Console.WriteLine();
                        DrawBoard(board);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" Input is not recognised ~ please try again\n");
                    Console.WriteLine($" Column: {userInputCol.ToUpper()} Row: {userInputRow.ToUpper()}");
                    Console.WriteLine("");
                    DrawBoard(board);



                }


            }

            board[ox, oy, ix, iy] = userNumber;
            Gameplay(board);

        }

        static void DeleteNumber(int[,,,] board)
        {
            int ox = 0, oy = 0, ix = 0, iy = 0;
            string userInputRow;
            string userInputCol;
            while (true)
            {
                userInputCol = InputOptions(false, board);
                userInputRow = InputOptions(true, board);
                char charCol = Convert.ToChar(userInputCol);
                char charRow = Convert.ToChar(userInputRow);

                int asciiCol = Convert.ToInt32(charCol);
                int asciiRow = Convert.ToInt32(charRow);

                int col = asciiCol & 15;
                int row = asciiRow & 15;

                Console.WriteLine();

                // ox 
                ox = col / board.GetLength(0);
                if (col % board.GetLength(0) == 0)
                {
                    ox--;
                }

                // oy
                oy = row / board.GetLength(1);
                if (row % board.GetLength(1) == 0)
                {
                    oy--;

                }

                //ix
                if (ox == 1)
                {
                    ix = (col - board.GetLength(2)) - 1;
                }
                else if (ox == 2)
                {
                    ix = (col - (board.GetLength(2) * 2)) - 1;
                }
                else
                {
                    ix = col - 1;
                }

                //iy
                if (oy == 1)
                {
                    iy = (row - board.GetLength(3)) - 1;
                }
                else if (oy == 2)
                {
                    iy = (row - (board.GetLength(3) * 2)) - 1;
                }
                else
                {
                    iy = row - 1;
                }


                if (board[ox, oy, ix, iy] == 0)
                {
                    break;

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($" Please choose a position that is not occupied");
                    Console.WriteLine($" Previous Choice ~ Column: {userInputCol.ToUpper()} Row: {userInputRow.ToUpper()}");
                    Console.WriteLine();
                    DrawBoard(board);

                }

            }
        }

        static string InputOptions(bool isRow, int[,,,] board)
        {
            
            string[] inputs = new string[] { "A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "G", "g", "H", "h", "I", "i" };
            string question = " Please enter the coloumn (A -> I) for your number";
            if (isRow)
            {
                question = " Please enter the row (A -> I) for your number";
            }
            string userPlace = "";

            while (true)
            {
                Console.WriteLine(question);
                string userInput = Console.ReadLine();
                Console.WriteLine();

                if (inputs.Contains(userInput))
                {
                    userPlace = userInput;
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Input not recognised, please try again");
                    DrawBoard(board);
 
                }
            }

            return userPlace;
        }


        // Displaying the Grid
        static void DrawBoard(int[,,,] board)
        {
            // width & height of outer block
            int ow = board.GetLength(0);
            int oh = board.GetLength(1);
            int iw = board.GetLength(2);
            int ih = board.GetLength(3);
            string line = " " + new String('-', (oh * ih) * 4 - 1);
            string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I" };

            int cnt = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("    A  B  C    D  E  F    G  H  I");
            Console.ForegroundColor = ConsoleColor.White;

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
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($" {board[ox, oy, ix, iy]} ");
                                Console.ForegroundColor = ConsoleColor.White;

                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($" {board[ox, oy, ix, iy]} ");
                            }
                        }

                        Console.Write(" |");
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($" {letters[cnt]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    cnt++;


                    Console.WriteLine(); // end of inner line
                }
                Console.WriteLine(line);
                 
            }
            Console.WriteLine();
        }


        static void RemoveNumbers(int[,,,] board)
        {
            int ow = board.GetLength(0);
            int oh = board.GetLength(1);
            int iw = board.GetLength(2);
            int ih = board.GetLength(3);


            Random rnd = new Random();
            int randNum;
            int numOfClues = 10;


            if (difficulty == 1) // easy
            {
                numOfClues = 40;
            }
            else if (difficulty == 2) // medium
            {
                numOfClues = 28;
            }
            else if (difficulty == 3) // hard
            {
                numOfClues = 20;
            }





            // number of spaces to remove (difficulty)
            for (int i = 0; i < board.Length - numOfClues; i++)
            {

                int ox = rnd.Next(0, 3);
                int oy = rnd.Next(0, 3);
                int ix = rnd.Next(0, 3);
                int iy = rnd.Next(0, 3);

                if (board[ox, oy, ix, iy] == 0)
                {
                    i--;
                }
                else
                {
                    board[ox, oy, ix, iy] = 0;

                }


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
                    boardSaved[ox, 0, ix, 0] = randNum;

                }
            }


            Solve(board);

            

            RemoveNumbers(board);

            Gameplay(board);


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
                                        boardSaved[ox, oy, ix, iy] = num;


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


        static bool isDone(int[,,,] board)
        {
            int ow = board.GetLength(0);
            int oh = board.GetLength(1);
            int iw = board.GetLength(2);
            int ih = board.GetLength(3);

            for (int oy = 0; oy < oh; oy++) // y coords of outer block
            {
                for (int iy = 0; iy < ih; iy++) // y coords of inner block
                {
                    for (int ox = 0; ox < ow; ox++) // x coords of outer block
                    {
                        for (int ix = 0; ix < iw; ix++) // x coords of inner block
                        {
                            if (board[ox, oy, ix, iy] == 0)
                            {
                                return false;
                            }
                           
                        }

                    }
                    


                }

            }
            return true;
        }
    }
}