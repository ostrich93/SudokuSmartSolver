using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.contexts
{
    public class NakedTwinExclusionContext : Context<TwinNodesCollection>
    {
        //change these to use its own potential Twins methods
        protected override Percepts percept => Percepts.FindingNakedTwin;
        public override bool GetContext(out TwinNodesCollection ntwins)
        {
            TwinNodesCollection twinNodesCollection = new TwinNodesCollection();
            var potTwins = from c in puzzle.cellColl
                           where !c.isFilled && c.neighbors != null && c.neighbors.Where(n => !n.isFilled).Count() > 2
                           select c;
            potTwins = potTwins.ToList();
            foreach (SudokuCell potTwin in potTwins ?? new List<SudokuCell>())
            {
                List<SudokuCell> a = potTwin.neighbors.Where(acell => !acell.isFilled && acell.Possibilities.Count == 2 ).ToList();
                foreach (SudokuCell ac in a ?? new List<SudokuCell>())
                {
                    HashSet<int> hashA = new HashSet<int>(potTwin.Possibilities);
                    HashSet<int> hashB = new HashSet<int>(ac.Possibilities);
                    if (hashA.SetEquals(hashB))
                    {
                        TwinNode tNode = new TwinNode(potTwin, ac, hashA, TwinEnum.Naked);
                        twinNodesCollection.AddNewTwinNode(tNode);
                    }
                }
            }
            ntwins = twinNodesCollection;
            return twinNodesCollection.TwinNodes.Count > 0 && CheckContextTwo(twinNodesCollection);
        }
        
        public bool CheckContextTwo(TwinNodesCollection twins)
        {
            foreach(TwinNode nTwin in twins.TwinNodes)
            {
                List<SudokuCell> universalNeighbors = nTwin.keyA.neighbors.Union(nTwin.keyB.neighbors).Except(new List<SudokuCell>() { nTwin.keyA, nTwin.keyB }).ToList();
                List<SudokuCell> debugNeighbors = universalNeighbors.Select(h => h).Where(uf => uf.Possibilities != null && uf.Possibilities.Intersect(nTwin.possSet) != null && uf.Possibilities.Intersect(nTwin.possSet).Count() > 0).ToList();
                if (debugNeighbors.Count > 0)
                    return true;
            }
            return false;
        }
    }
}