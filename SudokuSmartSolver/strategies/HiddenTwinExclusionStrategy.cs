using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;
using System.Diagnostics;

namespace SudokuPuzzleSolver.strategies
{
    //Needs some cleaning up, not ready to be reimplemented.
    public class HiddenTwinExclusionStrategy: Strategy<Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>> {
        private SudokuPuzzle sudoku = SudokuPuzzle.GetPuzzle();

        public HiddenTwinExclusionStrategy()
        {

        }

        //hidden twin: if (possFillMemberMap[a].Count == possFillMemberMap[b].Count) { int termCt = possFillMemberMap[a].Count; if (possFillMemberMap[a].Intersect(possFillMemberMap[b]).Count == termCt) { List<int> hiddenTwinNums = new List<int>{a, b}; var extraneousNumsA = TruePossibilities(possFillMap[a]).Except(hiddenTwinNums); var extraneousNumsB = TruePossibilities(possFilMap[b].Except(hiddenTwinNums); if (extraneousNumsA != null && extraneousNumsA.Count > 0) sudoku.AddDiscardedValues(cellA, extraneousNumsA); if (extraneousNumsB != null && extraneousNumsB.Count > 0) sudoku.AddDiscardedValues(cellB, extraneousNumsB); }}}

        public override void AlgorithmInterface(Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> param)
        {
            //get the possibility lists of all open cells in the cell group.
            if (param == null || param.Count == 0)
                Console.WriteLine("There are no valid lists of cell groups passed in.");
            else
                foreach (CellGroup cg in param.Keys)
                {
                    foreach (Pair<SudokuCell, SudokuCell> nTwin in param[cg])
                    {
                        var sharedVals = nTwin.GetX().Possibilities.Intersect(nTwin.GetY().Possibilities);
                        var collectiveNeighbors = nTwin.GetX().neighbors.Union(nTwin.GetY().neighbors).Distinct();
                        foreach (SudokuCell c in collectiveNeighbors)
                        {
                            if (sharedVals.Any(nce => c.Possibilities.Contains(nce)))
                            {
                                var sharedPoss = c.Possibilities.Intersect(sharedVals);
                                sudoku.discardedValuesTable.AddDiscardedValues(c, sharedPoss.ToList());
                            }
                        }
                    }

                }
        }

        //function to get a frequency counter of the number of times an int value appears in TruePossibilities in the cells for each CellGroup

        //function to retrieve cells within a cell group that contains some int n in its TruePossibilities?
        //Need to compare the lists of cell possibilities within a group. E.g, when searching for HiddenTwin in Column 1, take the possibleGroupFills and for each one in list, get the list of cells with it as possibility. A hidden twin is found when there are two numbers that can only be placed in the same two cells (or when there are three numbers only placeable in the same three cells)
        //e.g if the list of cells w/possibility of 4 = {R8C1, R9C1} and the list of cells w/possibility of 5 = {R8C1, R9C1}, then that's a hidden twin.
        //once you identify the hidden twin, get the 2 cells and eliminate all possibilities that are not in the twin from each one.

        public Dictionary<IEnumerable<IEnumerable<SudokuCell>>, int> MapReducePossibilities(CellGroup cellGroup)
        {
            Dictionary<IEnumerable<IEnumerable<SudokuCell>>, int> intersectionMap = new Dictionary<IEnumerable<IEnumerable<SudokuCell>>, int>();
            List<SudokuCell> suspectedCells = cellGroup.GetOpenMembers();
            var fillElems = suspectedCells.SelectMany(x => x.Possibilities).Where(y => cellGroup.GetCellsWithPossibility(y).Count >= 2).ToList();
            if (fillElems != null || fillElems.Count > 0)
            {
                foreach (int e in fillElems)
                {
                    //find the set of elements whose intersections contain this value
                    var cellsWPoss = suspectedCells.Select(a => a).Where(b => b.Possibilities.Contains(e)); //a value is intersected when it is a member of all the sets in the intersection, so this variable retrieves all the ones that have the target value and then gets all combinations
                    var combinations = cellsWPoss.FindIntersectionCombinations(cellsWPoss.Count());
                    intersectionMap.Add(combinations, e); //maybe better to store in a dictionary, with e as the key?
                }
            }
            return intersectionMap;
        }
    }
}
