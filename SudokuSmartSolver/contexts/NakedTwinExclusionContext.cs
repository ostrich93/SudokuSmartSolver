using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.contexts
{
    public class NakedTwinExclusionContext : Context<Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>>
    {
        //change these to use its own potential Twins methods
        protected override Percepts percept => Percepts.FindingNakedTwin;
        public override bool GetContext(out Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> ntwins)
        {
            List<CellGroup> potSGs = FindSubgroupsWithPotentialTwinsByType(UnitType.subgrid);
            List<CellGroup> potRs = FindSubgroupsWithPotentialTwinsByType(UnitType.row);
            List<CellGroup> potCs = FindSubgroupsWithPotentialTwinsByType(UnitType.column);
            Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> nakedTwinsDictionary = new Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>();
            if ((potSGs == null || potSGs.Count == 0) && (potRs == null || potRs.Count == 0) && (potCs == null || potCs.Count == 0))
            {
                //output variable set to null
                ntwins = null;
                return false;
            }
            if (potSGs != null && potSGs.Count > 0)
            {
                foreach (CellGroup cellGroup in potSGs)
                {
                    KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>> keyValuePair = HandleTwinNakedHashing(cellGroup);
                    if (keyValuePair.Key != null && keyValuePair.Value != null && keyValuePair.Value.Count != 0)
                        nakedTwinsDictionary.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            else if (potRs != null && potRs.Count > 0)
            {
                foreach (CellGroup cellRow in potRs) {
                    KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>> keyValuePair = HandleTwinNakedHashing(cellRow);
                    if (keyValuePair.Key != null && keyValuePair.Value != null && keyValuePair.Value.Count != 0)
                        nakedTwinsDictionary.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            else if (potCs != null && potCs.Count > 0)
            {
                foreach (CellGroup cellCol in potCs)
                {
                    KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>> keyValuePair = HandleTwinNakedHashing(cellCol);
                    if (keyValuePair.Key != null && keyValuePair.Value != null && keyValuePair.Value.Count != 0)
                        nakedTwinsDictionary.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            ntwins = nakedTwinsDictionary;
            return nakedTwinsDictionary != null;
        }
        
        private List<CellGroup> FindSubgroupsWithPotentialTwinsByType(UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.row:
                    return puzzle.GetOpenRows().Where(r => r.GetOpenCellCount() >= 2).ToList();
                case UnitType.column:
                    return puzzle.GetOpenColumns().Where(c => c.GetOpenCellCount() >= 2).ToList();
                case UnitType.subgrid:
                    return puzzle.GetOpenSubgrids().Where(s => s.GetOpenCellCount() >= 2).ToList();
                default:
                    return null;
            }
        }

        private KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>> HandleTwinNakedHashing(CellGroup cg) //do this for all cell groups of appropriate type and then feed in a Dictionary<CellGroup, List<Pair<SudokuCell,SudokuCell>>>. this function here should return KeyValuePair<CellGroup,List<Pair<SudokuCell,SudokuCell>>
        {
            var possC = from opMem in cg.GetOpenMembers()
                        where opMem != null && opMem.Possibilities != null && opMem.Possibilities.Count > 0
                        group opMem by opMem.Possibilities.Count;

            List<Pair<SudokuCell, SudokuCell>> nTwinsList = new List<Pair<SudokuCell, SudokuCell>>();
            foreach (var group in possC)
            {
                Dictionary<SudokuCell, HashSet<int>> setDict = new Dictionary<SudokuCell, HashSet<int>>();
                foreach (SudokuCell g in group) //error here, casting grouping as cell is the error?
                {
                    HashSet<int> possSet = new HashSet<int>(g.Possibilities);
                    setDict.Add(g, possSet);
                }
                if (setDict != null && setDict.Keys.Count > 0)
                {
                    foreach (SudokuCell c in setDict.Keys)
                    {
                        foreach (SudokuCell d in setDict.Keys)
                        {
                            if (c != d && setDict[c].SetEquals(setDict[d]))
                            {
                                Pair<SudokuCell, SudokuCell> nTwin = new Pair<SudokuCell, SudokuCell>(c, d);
                                if (!nTwinsList.Contains(nTwin))
                                    nTwinsList.Add(nTwin);
                            }
                        }
                    }
                }
            }
            return new KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>>(cg, nTwinsList);
        }

        //I wonder if the cells might be adding to discard tables of cells already filled. If so, needs to stop, might be an issue.

        //feed in UnitType and return all the CellGroups in puzzle of that type that have at least two open cells
            //if there are ONLY two open cells, then there's a twin there for sure, as they could only have two possibilities.
            //if there are MORE THAN two open cells, then there's a potential twin. Don't just blindly retrieve all groups w/two or more open cells, compare to see if the number of SHARED possibilities is equal. If so, those are potential twins or triplets.
        //to compare lists of possibilities with counts of two or more for shared elements, feed in List<List<int>>.

        //MAP REDUCTION!
    }
}
