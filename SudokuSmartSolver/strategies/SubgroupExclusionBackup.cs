using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.strategies
{
    public class SubgroupExclusionStrategy: Strategy<int> //make it Strategy<Pair<CellGroup, int>> instead
    {
        private SudokuPuzzle sudoku = SudokuPuzzle.GetPuzzle();
        private FrequencyCounter<int> frequencyCounter; //the frequency in which certain numbers are possibilities in cells

        public SubgroupExclusionStrategy()
        {
            frequencyCounter = new FrequencyCounter<int>();
            SetupFreqCounter();
        }

        public void SetupFreqCounter()
        {
            foreach(int a in SudokuPuzzle.sudokuDomain)
            {
                CountOccurances(a);
            }
        }

        public int CountOccurances(int n)
        {
            foreach(SudokuCell key in sudoku.cellColl)
            {
                if (sudoku.TruePossibilities(key).Contains(n))
                {
                    frequencyCounter.IncrementFor(n);
                }
            }
            return frequencyCounter.GetCount(n);
        }

        public override void AlgorithmInterface(int param) //Subgroup exclusion rule. Do it for three most frequently occuring numbers
        {
            SubgroupExculsion(param);
        }

        public IEnumerable<KeyValuePair<int,int>> GetTopThreeOccurances()
        {
            List<KeyValuePair<int, int>> orderedList = new List<KeyValuePair<int, int>>();
            foreach (int a in SudokuPuzzle.sudokuDomain)
            {
                int numOcc = frequencyCounter.GetCount(a);
                orderedList.Add(new KeyValuePair<int, int>(a, numOcc));
            }
            IEnumerable<KeyValuePair<int, int>> sortedList = from KeyValuePair<int, int> dom in orderedList
                                                             orderby dom.Value descending
                                                             select dom;
            return sortedList.Take(3);
        }

        public List<SudokuCell> XGrab(int m) //return the list of cells that contain target value as possibility
        {
            var xQuery = from SudokuCell t in sudoku.cellColl
                         where sudoku.TruePossibilities(t).Contains(m) && !t.isFilled
                         select t;
            return xQuery.ToList();
        }

        public void XSearch(int m)
        {
            List<SudokuCell> returnedList = XGrab(m); //the list of cells with m as possibility
            if (returnedList != null && returnedList.Count > 0)
            {
                var setQueries = from SudokuCell a in returnedList
                                 group a by a.rowNumber into groupA
                                 from groupB in
                                    (from cell in groupA
                                     group cell by cell.colNumber)
                                 group groupB by groupA.Key;
            }
        }

        public (List<SudokuCell>, List<int>, List<int>, List<int>) GrabSubgroup(int targetNumber)
        {
            List<SudokuCell> possibleCells = XGrab(targetNumber); //the list of cells with targetNumber as possibility
            List<int> retrievedRowIndices = SudokuCellExtensions.DeriveRowNums(possibleCells); //the rows of the cells with targetNumber as possibility
            List<int> retrievedColIndices = SudokuCellExtensions.DeriveColNums(possibleCells); //the columns of the cells with targetNumber as possibility
            List<int> retrievedSGIndices = SudokuCellExtensions.DeriveSGNums(possibleCells); //the subgrids of the cells with targetNumber as possibility
            return (possibleCells, retrievedRowIndices, retrievedColIndices, retrievedSGIndices);
        }

        public List<SudokuCell> GetSGIntersect(List<SudokuCell> sgCellList, List<SudokuCell> secUnitList)
        {
            List<SudokuCell> sgIntersection = sgCellList.Intersect(secUnitList).ToList(); //get the list of cells that belong to both sgCellList and secUnitList
            return sgIntersection;
        }

        public void SubgroupExculsion(int targetNum)
        {
            (List<SudokuCell> candidateCells, List<int> rowInds, List<int> colInds, List<int> sgInds) = GrabSubgroup(targetNum);
            Dictionary<int, List<SudokuCell>> rowPossCs = new Dictionary<int, List<SudokuCell>>();
            Dictionary<int, List<SudokuCell>> colPossCs = new Dictionary<int, List<SudokuCell>>();
            Dictionary<int, List<SudokuCell>> sgPossCs = new Dictionary<int, List<SudokuCell>>();
            Dictionary<SGUnitIntersectKey, List<SudokuCell>> sgIntersects = new Dictionary<SGUnitIntersectKey, List<SudokuCell>>(); //an empty dictionary to store the SGUnitIntersectKey, List<SudokuCell> pairs

            foreach (int rowInd in rowInds) //for each row in the returned list of row indices
            {
                var rQuery = candidateCells.Select(x => x).Where(y => y.rowNumber == rowInd); //select all cells in candidateCells where the rowNumber matches the current row index
                rowPossCs[rowInd] = rQuery.ToList(); //store the list of cells in the appropriate row number in dictionary.
            }
            foreach (int colInd in colInds)
            {
                var cQuery = candidateCells.Select(x => x).Where(y => y.colNumber == colInd);
                colPossCs[colInd] = cQuery.ToList();
            }
            foreach (int sgInd in sgInds)
            {
                var sgQuery = candidateCells.Select(x => x).Where(y => y.sgNumber == sgInd);
                sgPossCs[sgInd] = sgQuery.ToList();
                List<int> sgRs = SudokuCellExtensions.DeriveRowIndicesFromSG(sgInd);
                List<int> sgCs = SudokuCellExtensions.DeriveColIndicesFromSG(sgInd);
                foreach (int key in rowPossCs.Keys)
                {
                    if (sgRs.Contains(key))
                    {
                        SGUnitIntersectKey rca = new SGUnitIntersectKey(sgInd, UnitType.row, key);
                        List<SudokuCell> intersectVals = GetSGIntersect(sgPossCs[sgInd], rowPossCs[key]);
                        if (intersectVals != null)
                        {
                            if (intersectVals.Count > 1)
                                sgIntersects[rca] = intersectVals;
                        }
                    }
                }
                foreach (int key in colPossCs.Keys)
                {
                    if (sgCs.Contains(key))
                    {
                        SGUnitIntersectKey cca = new SGUnitIntersectKey(sgInd, UnitType.column, key);
                        List<SudokuCell> intersectVals = GetSGIntersect(sgPossCs[sgInd], colPossCs[key]);
                        if (intersectVals != null)
                        {
                            if (intersectVals.Count > 1)
                                sgIntersects[cca] = intersectVals;
                        }
                    }
                }
            }
            List<SGUnitIntersectKey> targetedSubgroupKeys = sgIntersects.Keys.ToList();
            foreach (SGUnitIntersectKey sgIK in targetedSubgroupKeys)
            {
                List<SudokuCell> openSGs = candidateCells.Select(x => x).Where(y => y.sgNumber == sgIK.sgValue).ToList();
                foreach (SudokuCell openSGCell in openSGs)
                {
                    if (!sgIntersects[sgIK].Contains(openSGCell))
                    {
                        sudoku.discardedValuesTable.AddDiscardedValue(openSGCell, targetNum);
                    }
                }
            }
        }
    }
}
