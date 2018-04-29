using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SudokuPuzzleSolver
{
    public interface ICell
    {
        bool isFilled { get; }
        bool isClue { get; }
    }

    public enum CellType
    {
        Normal,
        Clue
    }

    public enum UnitType
    {
        row,
        column,
        subgrid
    }

    [System.Serializable]
    public class CellData //A class that the clues are deserialized into. A sudoku cell can be created by using this as an input
    {
        public int fillValue { get; set; }
        public int rowNum { get; set; }
        public int columnNum { get; set; }
        public int subgridNum { get; set; }
    }

    public class SudokuCell : ICell
    {
        private int fillValue { get; set; }
        public int rowNumber;
        public int colNumber;
        public int sgNumber;

        public bool isFilled => fillValue > 0;
        private CellType clueMark { get; set; }
        public CellType ClueMark { get { return clueMark; } }
        public bool isClue => clueMark == CellType.Clue;
        public List<SudokuCell> rowNeighbors { get { return GetRowNeighbors(); } }
        public List<SudokuCell> columnNeighbors { get { return GetColumnNeighbors(); } }
        public List<SudokuCell> subgridNeighbors { get { return GetSubgridNeighbors(); } }
        public List<SudokuCell> neighbors { get { return rowNeighbors.Union(columnNeighbors).Union(subgridNeighbors).Distinct().DefaultIfEmpty().ToList(); } } //the list of all neighboring sudoku cells. CURRENT SOURCE OF BUGS

        public CellGroup rowPointer = new CellGroup(UnitType.row);
        public CellGroup columnPointer = new CellGroup(UnitType.column);
        public CellGroup subgridPointer = new CellGroup(UnitType.subgrid);

        private SudokuPuzzle puzzlePointer = SudokuPuzzle.GetPuzzle();

        public int FillValue //the fill value cannot be changed if the cell is a clue
        {
            get { return fillValue; }
            set
            {
                if (clueMark == CellType.Normal)
                    fillValue = value;
                else
                    Console.WriteLine("Cannot change the fill value of a clue");
            }
        } 

        public SudokuCell(CellType ct = CellType.Normal) //the constructor for non-clues, which always start out empty on construction.
        {
            fillValue = 0;
            rowNumber = 0;
            colNumber = 0;
            sgNumber = 0;
            clueMark = ct;
            //neighbors = new List<SudokuCell>();
        }

        public SudokuCell(CellData cData) //the constructor for clue cells, taking in the deserialized CellData object and using its parameters to create the cell.
        {
            clueMark = CellType.Clue;
            fillValue = cData.fillValue;
            rowNumber = cData.rowNum;
            colNumber = cData.columnNum;
            sgNumber = cData.subgridNum;
            //neighbors = new List<SudokuCell>();
        }

        public override string ToString()
        {
            return String.Format("R{0}C{1}", rowNumber, colNumber);
        }

        public List<SudokuCell> getUnitNeighbors(UnitType utype) //returns a list of neighbors within the cell's row, column, and subgrid (based on input). It does so by calling the the approrpiate function and returning the result. It's the public function exposed to the agent and the like.
        {
            List<SudokuCell> unitNeighbors = new List<SudokuCell>();
            if (utype == UnitType.row) {
                unitNeighbors = GetRowNeighbors();
            }
            if (utype == UnitType.column)
            {
                unitNeighbors = GetColumnNeighbors();
            }
            if (utype == UnitType.subgrid)
            {
                unitNeighbors = GetSubgridNeighbors();
            }
            return unitNeighbors;
        }
        
        //the three functions below are functions that grab the neighbors that are in the same row, column, and subgrid respectively. It does so by calling a LINQ query and returning the results as a list. The functions are private; other classes access them through the getUnitNeighbors function.
        private List<SudokuCell> GetRowNeighbors()
        {
            if (rowPointer == null || rowPointer.Members == null || rowPointer.Members.Count == 0)
            {
                return null;
            }
            return rowPointer.Members.Where(rc => rc != this).ToList();
        }

        private List<SudokuCell> GetColumnNeighbors()
        {
            if (columnPointer == null || columnPointer.Members == null || columnPointer.Members.Count == 0)
            {
                return null;
            }
            return columnPointer.Members.Where(cc => cc != this).ToList();
        }

        private List<SudokuCell> GetSubgridNeighbors()
        {
            if (subgridPointer == null || subgridPointer.Members == null || subgridPointer.Members.Count == 0)
            {
                return null;
            }
            return subgridPointer.Members.Where(sc => sc != this).ToList();
        }

        public List<int> NeighborFills { get { return neighbors.Select(C => C.fillValue).Distinct().ToList(); } } //returns a list of fill values derived from the list of neighboring cells. Mainly used for deriving the list of possible numbers the cell can have

        public List<int> DiscardedFills { get { if (puzzlePointer == null || puzzlePointer.discardedValuesTable == null || !puzzlePointer.discardedValuesTable.ContainsKey(this)) { return new List<int>(); } return puzzlePointer.discardedValuesTable.GetCellDiscardedValues(this); } }

        public List<int> Possibilities //the function that returns the list of possibile numbers the cell can have, which are derived from the fill values of neighboring cells. From now on, use this instead of sudokuPuzzle.TruePossibilities
        {
            get
            {
                if (puzzlePointer == null)
                    return null;
                if (isClue || isFilled) //first, it checks if the cell is filled or a clue. If it's either of these two cases, it returns a null value;
                    return null;
                if (NeighborFills == null && DiscardedFills == null)
                    return new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                if (NeighborFills != null && (DiscardedFills == null || DiscardedFills.Count == 0))
                    return SudokuPuzzle.sudokuDomain.Except(NeighborFills).ToList();
                if ((NeighborFills == null || NeighborFills.Count == 0) && DiscardedFills != null)
                    return SudokuPuzzle.sudokuDomain.Except(DiscardedFills).ToList();
                if (NeighborFills == null && DiscardedFills == null) //this is used for getting a list of possibilities if the list of neighbors is not yet assigned or is empty.
                    return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                List<int> domain = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //the domain of integers that the possible integers that can be assigned as the fill value (and be considered filled).
                var possQuery = SudokuPuzzle.sudokuDomain.Except(DiscardedFills ?? new List<int>().Union(NeighborFills ?? new List<int>()).Distinct()).Distinct().ToList(); //the integers in the possibilities list are the integers in the domain that are not assigned as the fillValues to any of the cell's neighbor's fillValues. Uses a LINQ query and converts it to a list.
                if (possQuery != null && possQuery.Contains(0)) //if the list somehow includes 0, get rid of it from the possibilities list (as a cell with fillValue = 0 means that the cell is empty).
                    possQuery.RemoveAll(n => n == 0);
                return possQuery;
            }
        }

        public List<SudokuCell> getAdjacentNeighbors
        {
            get {
                var adjacentQuery = from SudokuCell adj in neighbors
                                    where SudokuCellExtensions.getCellDistance(this, adj) == 1
                                    select adj;
                return adjacentQuery.ToList();
            }
        }

        public int getUnfilledNeighborCount()
        {
            var ufNeighborsQuery = from SudokuCell cell in neighbors
                                   where !cell.isFilled
                                   select cell;
            List<SudokuCell> ufneighbors = ufNeighborsQuery.ToList();
            return ufneighbors.Count;
        }

        public int getUnfilledSGNeighborCount()
        {
            var ufsgsQuery = from SudokuCell cell in neighbors
                             where !cell.isFilled && cell.sgNumber == sgNumber
                             select cell;
            List<SudokuCell> ufsgs = ufsgsQuery.ToList();
            return ufsgs.Count;
        }

        public int getMostFrequentPossibility()
        {
            var fp = NeighborFills.GroupBy(x => x);
            int mode = fp.OrderByDescending(g => g.Count()).First().Key;
            return mode;
        }

        public SudokuCell getFewestPossibilitiesNeighbor()
        {
            SudokuCell fewestCell = neighbors.OrderBy(y => y.Possibilities.Count()).FirstOrDefault();
            return fewestCell;
        }

        public List<SudokuCell> GetNeighborsWithPossibility(int g, UnitType gType)
        {
            switch (gType)
            {
                case UnitType.row:
                    var rnps = from rc in rowNeighbors
                               where !rc.isFilled && rc.Possibilities.Contains(g)
                               select rc;
                    return rnps.ToList();
                case UnitType.column:
                    var cnps = from cc in columnNeighbors
                               where !cc.isFilled && cc.Possibilities.Contains(g)
                               select cc;
                    return cnps.ToList();
                case UnitType.subgrid:
                    var sgnps = from sgc in subgridNeighbors
                                where !sgc.isFilled && sgc.Possibilities.Contains(g)
                                select sgc;
                    return sgnps.ToList();
                default:
                    return null;
            }
        }

        public bool CompareHashSet(HashSet<int> hset)
        {
            List<int> hList = new List<int>(hset);
            return hList == Possibilities;
        }
    }
}
