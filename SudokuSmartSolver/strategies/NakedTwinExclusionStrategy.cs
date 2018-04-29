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
    public class NakedTwinExclusionStrategy: Strategy<TwinNodesCollection>
    {
        private SudokuPuzzle sudoku = SudokuPuzzle.GetPuzzle();

        public NakedTwinExclusionStrategy() { }

        public override void AlgorithmInterface(TwinNodesCollection param)
        {
            //get the possibility lists of all open cells in the cell group.
            //HashSet<SudokuCell> twinCellPool = new HashSet<SudokuCell>();
            if (param == null || param.TwinNodes.Count == 0)
                Console.WriteLine("There are no naked twins passed in");
            else
                foreach (TwinNode nTwin in param.TwinNodes)
                {
                    List<SudokuCell> universalNeighbors = nTwin.keyA.neighbors.Union(nTwin.keyB.neighbors).Except(new List<SudokuCell>() {nTwin.keyA, nTwin.keyB }).ToList();
                    List<SudokuCell> debugNeighbors = universalNeighbors.Select(h => h).Where(uf => uf.Possibilities != null && uf.Possibilities.Intersect(nTwin.possSet)!= null && uf.Possibilities.Intersect(nTwin.possSet).Count() > 0).ToList();
                    foreach (SudokuCell neigh in universalNeighbors ?? new List<SudokuCell>())
                    {
                        if (param.ConfirmTwinKey(neigh))
                        {
                            List<TwinNode> nakedTwins = param.GetTwinNodesWithKey(neigh);
                            var untouchs = nakedTwins.SelectMany(neightw => neightw.possSet).Distinct();
                            HashSet<int> neighHash = new HashSet<int>(untouchs);
                            List<int> touchables = nTwin.possSet.Except(neighHash).ToList();
                            if (touchables != null && touchables.Count > 0)
                            {
                                sudoku.discardedValuesTable.AddDiscardedValues(neigh, touchables);
                            }
                        }
                        else
                        {
                            if (neigh.Possibilities != null && neigh.Possibilities.Intersect(nTwin.possSet).Distinct() != null)
                                sudoku.discardedValuesTable.AddDiscardedValues(neigh, neigh.Possibilities.Intersect(nTwin.possSet).Distinct().ToList());
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
