using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public struct SGUnitIntersectKey
    {
        public List<SudokuCell> intersectCells;
        public UnitType intersectType;
        public int cellGroupIndex;
        public int subgridIndex;
        public List<int> intersectedUnitValues;

        public SGUnitIntersectKey(CellGroup cg, List<SudokuCell> intercells, int sgIdx, List<int> intersectingInts)
        {
            intersectCells = intercells;
            intersectType = cg.GroupType;
            intersectedUnitValues = intersectingInts;
            subgridIndex = sgIdx;
            cellGroupIndex = cg.Index;
        }

        public SGUnitIntersectKey(CellGroup cg, List<SudokuCell> intercells, int sgIdx, int intersectingInt)
        {
            intersectCells = intercells;
            intersectType = cg.GroupType;
            intersectedUnitValues = new List<int>() { intersectingInt };
            cellGroupIndex = cg.Index;
            subgridIndex = sgIdx;
        }

        public override string ToString()
        {
            return intersectCells.ToString() + " " + intersectedUnitValues.ToString();
        }
    }
}
