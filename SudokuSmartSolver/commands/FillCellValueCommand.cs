using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.commands
{
    public class FillCellValueCommand : ICommand
    {
        SudokuCell cell;
        int fillNum;

        public FillCellValueCommand(SudokuCell newcell, int filly)
        {
            cell = newcell;
            fillNum = filly;
        }

        public void Execute()
        {
            cell.FillValue = fillNum;
        }

        public void Undo()
        {
            cell.FillValue = 0;
        }
    }
}
