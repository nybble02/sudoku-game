using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SudokuGame
{
    public class Square
    {
        public int OuterXCoord { get; set; }
        public int OuterYCoord { get; set; }
        public int InnerXCoord { get; set; }
        public int InnerYCoord { get; set; }
        public int GenNum {get; set; } // generated number
        public bool IsUserField { get; set; } // Hides the number for the user to input a number to there
        public int UserNum {get; set; } // number the user inputs

        public Square() { }

        public Square(int ox, int oy, int ix, int iy) 
        { 
            GenNum= 0;
            IsUserField= false;
            UserNum= 0;
            OuterXCoord = ox;
            OuterYCoord = oy;
            InnerXCoord = ix;
            InnerYCoord = iy;
        }



        // inizialezde the square
        public void SetNum(int genNum)
        {
            IsUserField = false;
            UserNum = 0;
            GenNum = genNum;
          
        }


        public void DrawNum(bool isShowAnswers)
        {
            

            if (IsUserField) // user field
            {
                if (isShowAnswers)
                {
                    if (UserNum == GenNum)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(UserNum);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(GenNum);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                else if (UserNum == 0 )
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("-");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(UserNum);
                    Console.ForegroundColor = ConsoleColor.White;

                }
            }
            else
            {
                Console.Write(GenNum);
            }

        }

        

    }
}
