using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class CellGroupCertifier
    {
        SudokuPuzzle _puzzle = SudokuPuzzle.GetPuzzle();

        public bool AllMembersFilled(CellGroup ip)
        {
            return ip.GetOpenCellCount() == 0;
        }

        public bool MemberValsFitDomain(CellGroup ip)
        {
            HashSet<int> memSet = new HashSet<int>(ip.GetGroupFills());
            HashSet<int> sudoHash = new HashSet<int>(SudokuPuzzle.sudokuDomain);
            return memSet.SetEquals(sudoHash);
        }

        public bool IsGroupWeightCorrect(CellGroup ip)
        {
            return ip.GetGroupFills().Sum() == 45;
        }
    }
}
