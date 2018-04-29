using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SudokuPuzzleSolver
{
    public sealed class SudokuPuzzle //turn into singleton
    {
        private static readonly Lazy<SudokuPuzzle> _puzzle = new Lazy<SudokuPuzzle>(() => new SudokuPuzzle());
        public static SudokuPuzzle GetPuzzle() => _puzzle.Value;

        public Board cellBoard = new Board();
        public List<SudokuCell> cellColl { get
            {
                if (cellBoard != null && cellBoard.cellList != null)
                    return cellBoard.cellList;
                else
                    return null;
            } }
        private bool isSolved = false;

        public CellGroup[] rowCollection
        {
            get
            {
                if (cellColl == null || cellBoard.rowList.Count() == 0)
                    return null;
                else
                    return cellBoard.rowList;
            }
            set { if (cellColl != null)
                    cellBoard.rowList = value;
            }
        }
        public CellGroup[] colCollection
        {
            get
            {
                if (cellColl == null || cellBoard.columnList.Count() == 0)
                    return null;
                else
                    return cellBoard.columnList;
            }
            set { if (cellColl != null)
                    cellBoard.columnList = value;
            }
        }
        public CellGroup[] sgCollection
        {
            get
            {
                if (cellColl == null || cellBoard.subgridList.Count() == 0)
                    return null;
                else
                    return cellBoard.subgridList;
            }
            set
            {
                if (cellColl != null)
                    cellBoard.subgridList = value;
            }
        }

        public static List<int> sudokuDomain = new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public DiscardedValuesTable discardedValuesTable = new DiscardedValuesTable();

        public FrequencyCounter<int> domainFreqCounter
        {
            get
            {
                if (cellBoard == null || cellBoard.fillFrequencyCounter == null)
                    return null;
                else
                    return cellBoard.fillFrequencyCounter;
            }
            set
            {
                cellBoard.fillFrequencyCounter = value;
            }
        }//use to get the number of times any of the cells are filled with a number within sudoku domain (e.g if four cells have 5 as fill value, getCount(5) shoul return 4)

        private SudokuPuzzle() {
            
        }

        public void BuildPuzzle(string cluesfile)
        {
            cellBoard.SetupBoard(cluesfile);
            PopulateDiscardedValueKeys();
        }

        public void InitializeCellGroups()
        {
            for (int i = 0; i < 9; i++)
            {
                CellGroup freshRow = new CellGroup(UnitType.row);
                rowCollection[i] = freshRow;
                rowCollection[i].SetIndex(i+1);
                CellGroup freshColumn = new CellGroup(UnitType.column);
                colCollection[i] = freshColumn;
                colCollection[i].SetIndex(i+1);
                CellGroup freshSubgrid = new CellGroup(UnitType.subgrid);
                sgCollection[i] = freshSubgrid;
                sgCollection[i].SetIndex(i+1);
            }
        }

        public void PopulateDiscardedValueKeys()
        {
            foreach(SudokuCell sckey in cellColl)
            {
                discardedValuesTable.AddKey(sckey);
            }
        }

        public CellGroup GetCellGroupByIndex(UnitType ut, int ind)
        {
            if (ind >= 1 && ind <= 9)
            {
                switch (ut)
                {
                    case UnitType.row:
                        return rowCollection[ind - 1];
                    case UnitType.column:
                        return colCollection[ind - 1];
                    case UnitType.subgrid:
                        return sgCollection[ind - 1];
                }
            }
            return null;
        }

        public List<int> GetSubgridIndices(CellGroup cg, int n)
        {
            if (cg.GroupType == UnitType.subgrid)
                return new List<int>() { cg.Index };
            List<SudokuCell> cellsWithN = cg.GetCellsWithPossibility(n); //first, get the list of cells within the cell group that has n as a possibility
            if (cellsWithN != null)
            {
                var sgInds = cellsWithN.Select(x => x.sgNumber);
                return sgInds.ToList();
            }
            return null;
        }

        public bool AreIndicesDistinct(List<int> indList)
        {
            if (indList == null)
                return false;
            return indList.Distinct().Count() == indList.Count;
        }

        public List<CellGroup> GetOpenRows()
        {
            return rowCollection.Select(x => x).Where(y => !y.IsFull).ToList();
        }

        public List<CellGroup> GetOpenColumns()
        {
            return colCollection.Select(x => x).Where(y => !y.IsFull).ToList();
        }

        public List<CellGroup> GetOpenSubgrids()
        {
            return sgCollection.Select(x => x).Where(y => !y.IsFull).ToList();
        }

        public List<CellGroup> GetUnifiedOpenCellGroups()
        {
            List<CellGroup> unifiedList = rowCollection.Union(colCollection.Union(sgCollection)).ToList();
            return unifiedList.Select(t => t).Where(u => !u.IsFull).ToList();
        }

        //will probably need to redo this.
        public void AssignNeighbors()
        {
            foreach (SudokuCell cell in cellColl)
            {
                var rNeighborsQuery = from SudokuCell incel in cellColl
                                      where incel.rowNumber == cell.rowNumber
                                      select incel;
                var cNeighborsQuery = from SudokuCell incell in cellColl
                                      where incell.colNumber == cell.colNumber
                                      select incell;
                var sNeighborsQuery = from SudokuCell innocel in cellColl
                                      where innocel.sgNumber == cell.sgNumber
                                      select innocel;
                var neighborResults1 = rNeighborsQuery.Union(cNeighborsQuery);
                var allNeighbors = neighborResults1.Union(sNeighborsQuery);
                //cell.neighbors = allNeighbors.ToList();
                discardedValuesTable.AddDiscardedValues(cell, cell.NeighborFills); //probably unnecessary since sudoku cell possibilities already exlclude neighboring values
            }
        }

        public void ClearPuzzle()
        {
            discardedValuesTable.Clear();
            cellBoard = new Board();
            rowCollection = new CellGroup[9];
            colCollection = new CellGroup[9];
            sgCollection = new CellGroup[9];
            isSolved = false;
            //domainFreqCounter.Clear();
        }

        public void ResetPuzzle()
        {
            discardedValuesTable.Clear();
            string puzzleName = cellBoard.puzzleId;
            cellBoard = new Board();
            cellBoard.SetupBoard(puzzleName);
            var cellsToClear = from SudokuCell ctc in cellColl
                               where !ctc.isClue
                               select ctc;
        }

        public bool AllFilled()
        {
            return cellColl.All(c => !c.isFilled);
        }

        public int GetRemainingInstances(int inspectNum) //check the number of times a number from one to nine can be used.
        {
            if (!sudokuDomain.Contains(inspectNum))
                return 0;
            return 9 - domainFreqCounter.GetCount(inspectNum);
        }
    }
}
