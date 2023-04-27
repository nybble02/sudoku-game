using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGame
{
    public class UserEntry
    {
        public int OuterXCoord { get; set; }
        public int OuterYCoord {get; set; }
        public int InnerXCoord { get; set; }
        public int InnerYCoord { get; set; }
        public int UserNum { get; set; }
        public bool isUndone { get; set; } // 0 active 1 unactive

        public UserEntry(int ox, int oy, int ix, int iy, int userNum) 
        {
            OuterXCoord = ox;
            OuterYCoord = oy;
            InnerXCoord = ix;
            InnerYCoord = iy;
            UserNum = userNum;

            isUndone = false;
        }





    }
}
