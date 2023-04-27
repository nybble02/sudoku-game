using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Enumeration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SudokuGame
{
    class SudokuGame
    {
        const int BOARD_SIZE = 3;
        Square[,,,] Board;
        int Difficulty;
        int customClues;
        bool ShowAnswers = false;
        List<UserEntry> UserEntries = new List<UserEntry>();

        int UndoIndex = -1;


        public SudokuGame()
        {
            Board = new Square[BOARD_SIZE, BOARD_SIZE, BOARD_SIZE, BOARD_SIZE];
            ChooseDifficultyOptions();
            InitBoard();
            GeneratePuzzle();
        }


        int ChooseDifficultyOptions()
        {
            Console.WriteLine("Welcome to Sudoku!");
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine(" Please select a difficulty");
                Console.WriteLine(" (1) Easy  ");
                Console.WriteLine(" (2) Medium");
                Console.WriteLine(" (3) Hard  ");
                Console.WriteLine(" (4) Custom  ");
                Console.WriteLine(" (5) Load Game");



                string userInput = Console.ReadLine();

                if (userInput == "1")
                {
                    Difficulty = 1;
                    break;
                }
                else if (userInput == "2")
                {
                    Difficulty = 2;
                    break;
                }
                else if (userInput == "3")
                {
                    Difficulty = 3;
                    break;
                }
                else if (userInput == "4")
                {
                    Difficulty = 4;
                    CustomDifficulty();
                    break;
                }
                else if (userInput == "5")
                {
                    LoadGame();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" Input is not recognised ~ please try again\n");
                }
            }

            return Difficulty;
        }

        void Gameplay()
        {

            string userChoice;
            while (true)
            {
                DrawBoard();
                Console.WriteLine(" ~ Please choose an option ~");
                Console.WriteLine("(1) Add a number");
                Console.WriteLine("(2) Undo move");
                Console.WriteLine("(3) Redo move");
                Console.WriteLine("(4) Toggle answers");
                Console.WriteLine("(5) Save Game");
                Console.WriteLine("(6) Change difficulty / New game");
                Console.WriteLine("(7) Terminate Game");


                userChoice = Console.ReadLine();

                if (userChoice == "1")
                {
                    PlaceNumber();
                }
                else if (userChoice == "2")
                {
                    Undo();
                }
                else if (userChoice == "3")
                {
                    Redo();
                }
                else if (userChoice == "4")
                {
                    Console.Clear();
                    ShowAnswers = !ShowAnswers;
                    //DrawBoard();
                }
                else if (userChoice == "5")
                {
                    SaveGame();
                }
                else if (userChoice == "6")
                {
                    Console.Clear();
                    SudokuGame game = new SudokuGame();
                }
                else if (userChoice == "7")
                {
                    Console.Clear();
                    Console.WriteLine("Thank you for playing!");
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

        void CustomDifficulty()
        {
            int minValue = 1;
            int maxValue = 80;
            string userInput;
            int userNumber;
            bool isNumber;

            while (true)
            {
                Console.WriteLine("How many clues do you want? ");
                Console.Write($" Please enter a number between {minValue} and {maxValue} ");
                userInput = Console.ReadLine();

                isNumber = int.TryParse(userInput, out userNumber);

                if (isNumber && userNumber >= minValue && userNumber <= maxValue)
                {
                    customClues = userNumber;
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" Input is not recognised ~ please try again\n");
                    Console.WriteLine("");

                }


            }


        }

        void PlaceNumber()
        {
            int ox = 0, oy = 0, ix = 0, iy = 0;
            string userInputRow;
            string userInputCol;
            while (true)
            {
                userInputCol = InputOptions(false);
                userInputRow = InputOptions(true);
                char charCol = Convert.ToChar(userInputCol);
                char charRow = Convert.ToChar(userInputRow);

                int asciiCol = Convert.ToInt32(charCol);
                int asciiRow = Convert.ToInt32(charRow);

                int col = asciiCol & 15;
                int row = asciiRow & 15;

                // ox 
                ox = col / Board.GetLength(0);
                if (col % Board.GetLength(0) == 0)
                {
                    ox--;
                }

                // oy
                oy = row / Board.GetLength(1);
                if (row % Board.GetLength(1) == 0)
                {
                    oy--;

                }

                //ix
                if (ox == 1)
                {
                    ix = (col - Board.GetLength(2)) - 1;
                }
                else if (ox == 2)
                {
                    ix = (col - (Board.GetLength(2) * 2)) - 1;
                }
                else
                {
                    ix = col - 1;
                }

                //iy
                if (oy == 1)
                {
                    iy = (row - Board.GetLength(3)) - 1;
                }
                else if (oy == 2)
                {
                    iy = (row - (Board.GetLength(3) * 2)) - 1;
                }
                else
                {
                    iy = row - 1;
                }


                if (Board[ox, oy, ix, iy].IsUserField)
                {
                    break;

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($" Please choose a position that is not occupied");
                    Console.WriteLine($" Previous Choice ~ Column: {userInputCol.ToUpper()} Row: {userInputRow.ToUpper()}");
                    Console.WriteLine();
                    DrawBoard();

                }

            }

            int minValue = 1;
            int maxValue = 9;
            string userInput;
            int userNumber;
            bool isNumber;

            while (true)
            {
                Console.Write($" Please enter a number between {minValue} and {maxValue}: ");
                userInput = Console.ReadLine();

                isNumber = int.TryParse(userInput, out userNumber);

                if (isNumber && userNumber >= minValue && userNumber <= maxValue)
                {
                    if (isComplete())
                    {
                        GameComplete();
                    }
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" Input is not recognised ~ please try again\n");
                    Console.WriteLine($" Column: {userInputCol.ToUpper()} Row: {userInputRow.ToUpper()}");
                    Console.WriteLine("");
                    DrawBoard();

                }


            }

            if (Board[ox, oy, ix, iy].IsUserField)
            {
                Board[ox, oy, ix, iy].UserNum = userNumber;
                if (UndoIndex >= 0 && UserEntries.Count != 0)
                {
                    UserEntries.RemoveRange(UndoIndex + 1, UserEntries.Count - (UndoIndex + 1)); // removes anything after UndoIndex rmeoves all items after the last posistion
                }
                UserEntries.Add(new UserEntry(ox, oy, ix, iy, userNumber));
                UndoIndex = UserEntries.Count - 1;
            }

            Console.Clear();
            //Gameplay();

        }

        void Undo()
        {
            Console.WriteLine($"UndoIndex: {UndoIndex}");
            if (UndoIndex < 0)
            {
                Console.WriteLine("There are no numbers to be undone");
                return;
            }

            var userEntry = UserEntries[UndoIndex]; // gets most recent entry
            Board[userEntry.OuterXCoord, userEntry.OuterYCoord, userEntry.InnerXCoord, userEntry.InnerYCoord].UserNum = 0;
            userEntry.isUndone = true;

            UndoIndex--;
            // Gameplay();
        }

        void Redo()
        {
            if (UserEntries.Count < 0)
            {
                Console.WriteLine("There are no entries");
                return;
            }

            if (UndoIndex >= UserEntries.Count - 1)
            {
                Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAH");
                return;
            }
            UndoIndex++;
            var userEntry = UserEntries[UndoIndex]; // gets most recent entry
            Board[userEntry.OuterXCoord, userEntry.OuterYCoord, userEntry.InnerXCoord, userEntry.InnerYCoord].UserNum = userEntry.UserNum;
            userEntry.isUndone = false;

            //Gameplay();
        }

        string InputOptions(bool isRow)
        {

            string[] inputs = new string[] { "A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "G", "g", "H", "h", "I", "i" };
            string question = " Please enter the column (A -> I) for your number: ";
            if (isRow)
            {
                question = " Please enter the row (A -> I) for your number: ";
            }
            string userPlace = "";

            while (true)
            {
                Console.Write(question);
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
                    DrawBoard();

                }
            }

            return userPlace;
        }

        // Displaying the Grid
        void DrawBoard()
        {

            string line = " " + new String('-', (BOARD_SIZE * BOARD_SIZE) * 4 - 1);
            string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I" };

            int cnt = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("    A  B  C    D  E  F    G  H  I");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(line);
            for (int oy = 0; oy < BOARD_SIZE; oy++) // y coords of outer block
            {


                for (int iy = 0; iy < BOARD_SIZE; iy++) // y coords of inner block
                {


                    Console.Write(" | "); // after each inner y
                    for (int ox = 0; ox < BOARD_SIZE; ox++) // x coords of outer block
                    {

                        for (int ix = 0; ix < BOARD_SIZE; ix++) // x coords of inner block
                        {
                            Console.Write(" ");
                            Board[ox, oy, ix, iy].DrawNum(ShowAnswers);
                            Console.Write(" ");

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

        void RemoveNumbers()
        {

            Random rnd = new Random();
            int randNum;
            int numOfClues = 0;

            if (Difficulty == 1) // easy
            {
                numOfClues = 40;
            }
            else if (Difficulty == 2) // medium
            {
                numOfClues = 28;
            }
            else if (Difficulty == 3) // hard
            {
                numOfClues = 20;
            }
            else if (Difficulty == 4) // Custom difficulty
            {
                Console.WriteLine("Custom " + customClues);

                numOfClues = customClues;
            }
            // number of spaces to remove (difficulty)
            for (int i = 0; i < Board.Length - numOfClues; i++)
            {

                int ox = rnd.Next(0, 3);
                int oy = rnd.Next(0, 3);
                int ix = rnd.Next(0, 3);
                int iy = rnd.Next(0, 3);

                if (Board[ox, oy, ix, iy].IsUserField)
                {
                    i--;
                }
                else
                {
                    Board[ox, oy, ix, iy].IsUserField = true;

                }
            }

            Console.WriteLine();
            Console.WriteLine($"Clues: {numOfClues}");
            Console.WriteLine();



        }

        void InitBoard()
        {
            for (int ox = 0; ox < BOARD_SIZE; ox++) // y coords of outer block
            {
                for (int oy = 0; oy < BOARD_SIZE; oy++) // y coords of inner block
                {
                    for (int ix = 0; ix < BOARD_SIZE; ix++) // x coords of outer block
                    {
                        for (int iy = 0; iy < BOARD_SIZE; iy++) // x coords of inner block
                        {
                            Board[ox, oy, ix, iy] = new Square(ox,oy,ix,iy);
                        }
                    }

                }
            }
        }

        void GeneratePuzzle()
        {

            // top row
            HashSet<int> usedNums = new HashSet<int>();
            Random random = new Random();

            int randNum;

            for (int ox = 0; ox < BOARD_SIZE; ox++)
            {
                for (int ix = 0; ix < BOARD_SIZE; ix++)
                {
                    do
                    {
                        randNum = random.Next(1, 10);

                    }
                    while (usedNums.Contains(randNum));

                    usedNums.Add(randNum);
                    Board[ox, 0, ix, 0].SetNum(randNum);
                }
            }


            Solve();

            RemoveNumbers();

            Gameplay();

        }

        bool Solve()
        {

            Random rnd = new Random();
            int randNum;

            for (int ox = 0; ox < BOARD_SIZE; ox++) // y coords of outer block
            {
                for (int oy = 0; oy < BOARD_SIZE; oy++) // y coords of inner block
                {
                    for (int ix = 0; ix < BOARD_SIZE; ix++) // x coords of outer block
                    {
                        for (int iy = 0; iy < BOARD_SIZE; iy++) // x coords of inner block
                        {
                            if (Board[ox, oy, ix, iy].GenNum == 0) // empty space
                            {
                                for (int num = 1; num <= 9; num++)
                                {
                                    if (isValid(ox, oy, ix, iy, num))
                                    {
                                        Board[ox, oy, ix, iy].GenNum = num;

                                        if (Solve())
                                        {
                                            return true;
                                        }

                                        Board[ox, oy, ix, iy].GenNum = 0;
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
        bool isValid(int outerCol, int outerRow, int innerCol, int innerRow, int num)
        {

            //col
            for (int ox = 0; ox < BOARD_SIZE; ox++)
            {
                for (int ix = 0; ix < BOARD_SIZE; ix++)
                {
                    if (Board[ox, outerRow, ix, innerRow].GenNum == num)
                    {
                        return false;

                    }
                }
            }

            //row
            for (int oy = 0; oy < BOARD_SIZE; oy++)
            {
                for (int iy = 0; iy < BOARD_SIZE; iy++)
                {
                    if (Board[outerCol, oy, innerCol, iy].GenNum == num)
                    {
                        return false;

                    }
                }
            }

            // inner square
            for (int iy = 0; iy < BOARD_SIZE; iy++)
            {
                for (int ix = 0; ix < BOARD_SIZE; ix++)
                {
                    if (Board[outerCol, outerRow, ix, iy].GenNum == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        bool isComplete()
        {
            for (int ox = 0; ox < BOARD_SIZE; ox++) // y coords of outer block
            {
                for (int oy = 0; oy < BOARD_SIZE; oy++) // y coords of inner block
                {
                    for (int ix = 0; ix < BOARD_SIZE; ix++) // x coords of outer block
                    {
                        for (int iy = 0; iy < BOARD_SIZE; iy++) // x coords of inner block
                        {
                            var square = Board[ox, oy, ix, iy];
                            if (square.IsUserField && square.UserNum == 0) // empty space
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;

        }

        void GameComplete()
        {

        }

        void SaveGame()
        {
            Console.WriteLine("Enter file name");
            string fileName = Console.ReadLine();

            List<Square> squareList = new List<Square>();

            for (int ox = 0; ox < BOARD_SIZE; ox++) // y coords of outer block
            {
                for (int oy = 0; oy < BOARD_SIZE; oy++) // y coords of inner block
                {
                    for (int ix = 0; ix < BOARD_SIZE; ix++) // x coords of outer block
                    {
                        for (int iy = 0; iy < BOARD_SIZE; iy++) // x coords of inner block
                        {
                            var square = Board[ox, oy, ix, iy];
                            squareList.Add(square);
                        }
                    }
                }
            } 


           // string fileName = "sudoku_saved_game.json";
            string jsonString = JsonSerializer.Serialize(squareList, new JsonSerializerOptions { WriteIndented = true});

            File.WriteAllText($"{fileName}.sudoku", jsonString);


        }

        void LoadGame()
        {
            var fileNameList = Directory.GetFiles(".", "*.sudoku");

            Console.WriteLine($"Choose a file name, enter number between 1 and {fileNameList.Length}");
            if (fileNameList.Length == 0)
            {
                Console.WriteLine("No saved games found...");
                return;
            }

            for (int i =0; i < fileNameList.Length; i++)
            {
              
                Console.WriteLine($"{i + 1} {fileNameList[i]}");

            }

            string userFile = Console.ReadLine();

            if(!int.TryParse(userFile, out int userFileNum))
            {
                Console.WriteLine("Input is not recognised ~ please try again");
                return;
            }

            if(userFileNum < 1 || userFileNum > fileNameList.Length)
            {
                Console.WriteLine("Invalid file selected");
                return;
            }



            List<Square> squareList = new List<Square>();
            string jsonString = File.ReadAllText(fileNameList[userFileNum - 1]);
            squareList = JsonSerializer.Deserialize<List<Square>>(jsonString);

            foreach(var square in squareList)
            {
                Board[square.OuterXCoord, square.OuterYCoord, square.InnerXCoord, square.InnerYCoord] = square;
            }

            Gameplay();
        }

    }

}
