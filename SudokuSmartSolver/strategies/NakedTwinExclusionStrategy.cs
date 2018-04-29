using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.strategies
{
    //Debug this now
    //current issue is taking into account a pair whose neighbors might ALSO be twins that share value(s)
    public class NakedTwinExclusionStrategy: Strategy<Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>>
    {
        private SudokuPuzzle sudoku = SudokuPuzzle.GetPuzzle();

        public NakedTwinExclusionStrategy() { }

        public override void AlgorithmInterface(Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> param)
        {
            //get the possibility lists of all open cells in the cell group.
            //HashSet<SudokuCell> twinCellPool = new HashSet<SudokuCell>();
            if (param == null || param.Count == 0)
                Console.WriteLine("There are no valid lists of cell groups passed in.");
            else
                foreach (CellGroup cg in param.Keys)
                {
                    foreach(Pair<SudokuCell, SudokuCell> nTwin in param[cg])
                    {
                        var sharedVals = nTwin.GetX().Possibilities;
                        var collectiveNeighbors = nTwin.GetX().neighbors.Union(nTwin.GetY().neighbors).Distinct();
                        foreach(SudokuCell c in collectiveNeighbors)
                        {
                            if (c.Possibilities != null && sharedVals != null) //error his here! seems to be discarding ALL possibilities of a cell
                            {
                                var sharedPoss = c.Possibilities.Intersect(sharedVals);
                                sudoku.discardedValuesTable.AddDiscardedValues(c, sharedPoss.ToList());
                            }
                        }
                    }
                    }
                }
        

        //public List<SudokuCell> GetNeighborTwins(Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> twinDictionary, CellGroup neighborKey, Pair<SudokuCell,SudokuCell> twinBaseComparison)
        //{
        //    List<Pair<SudokuCell, SudokuCell>> pairList = twinDictionary[neighborKey];
        //    List<SudokuCell> tAssociates = twinBaseComparison.TransformPairIntoList();
        //    foreach(Pair<SudokuCell, SudokuCell> pt in pairList)
        //    {
        //        if (tAssociates.Any(t => t.neighbors.Contains(pt.GetX())))
        //        {
        //            if (tAssociates.Any(t => t.Possibilities.Any(c => pt.GetX().Possibilitie)
        //        }
        //    }
        //}

    }
}
