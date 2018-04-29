using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public struct CellAttempts //data structure meant to keep track of the fill values that were tried with sudoku cells.
                                      //make dictionary of SudokuCells and a List of Cell Attempts, where the key is the root cell.
    {
        public SudokuCell keyCell;
        public List<int> attemptedValues;

        public CellAttempts(SudokuCell s_cell, List<int> attemptedVals) //constructor with multiple attempted values
        {
            keyCell = s_cell;
            attemptedValues = attemptedVals;
        }

        public CellAttempts(SudokuCell s_cell, int singularVal) //constructor with only a single attempted value
        {
            keyCell = s_cell;
            attemptedValues = new List<int>();
            attemptedValues.Add(singularVal);
        }
    }
}
