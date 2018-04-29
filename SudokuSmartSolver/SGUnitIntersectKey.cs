using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public struct SGUnitIntersectKey
    {
        public List<SudokuCell> intersectCells; //cells to not alter
        public List<SudokuCell> elimCells; //cells to elminate interesectKey from their possiblity lists
        public CellGroup subgrid; //replace with CellGroup
        public int intersectValue; //replace with single int value

        public SGUnitIntersectKey(CellGroup cg, List<SudokuCell> intercells, List<SudokuCell> prohibitCells, int intersectingInt)
        {
            intersectCells = intercells;
            elimCells = prohibitCells;
            intersectValue = intersectingInt;
            subgrid = cg;
        }

        public override string ToString()
        {
            return intersectCells.ToString() + " " + intersectValue.ToString();
        }
    }
}
