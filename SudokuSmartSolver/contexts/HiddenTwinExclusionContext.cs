using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.contexts
{
    //Needs some cleanup I think
    public class HiddenTwinExclusionContext: Context<Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>>
    {        
        protected override Percepts percept => Percepts.FindingHiddenTwin;
        public override bool GetContext(out Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> htwins)
        {
            List<CellGroup> potSGs = FindSubgroupsWithPotentialTwinsByType(UnitType.subgrid);
            List<CellGroup> potRs = FindSubgroupsWithPotentialTwinsByType(UnitType.row);
            List<CellGroup> potCs = FindSubgroupsWithPotentialTwinsByType(UnitType.column);
            Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> hiddenTwinsDictionary = new Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>();
            if ((potSGs == null || potSGs.Count == 0) && (potRs == null || potRs.Count == 0) && (potCs == null || potCs.Count == 0))
            {
                //output variable set to null
                htwins = null;
                return false;
            }
            if (potSGs != null && potSGs.Count > 0)
            {
                foreach(CellGroup cellGroup in potSGs)
                {
                    KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>> keyValuePair = HandleHiddenTwinHashing(cellGroup);
                    if (!keyValuePair.Equals(null) && keyValuePair.Key != null && keyValuePair.Value != null && keyValuePair.Value.Count > 0)
                        hiddenTwinsDictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    
                }
            }

            else if (potRs != null && potRs.Count > 0)
            {
                foreach(CellGroup cellRow in potRs)
                {
                    KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>> keyValuePair = HandleHiddenTwinHashing(cellRow);
                    if (!keyValuePair.Equals(null) && keyValuePair.Key != null && keyValuePair.Value != null && keyValuePair.Value.Count > 0)
                        hiddenTwinsDictionary.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            else if (potCs != null && potCs.Count > 0)
            {
                foreach(CellGroup cellCol in potCs)
                {
                    KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>> keyValuePair = HandleHiddenTwinHashing(cellCol);
                    if (!keyValuePair.Equals(null) && keyValuePair.Key != null && keyValuePair.Value != null && keyValuePair.Value.Count > 0)
                        hiddenTwinsDictionary.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            htwins = hiddenTwinsDictionary;
            return (hiddenTwinsDictionary != null && hiddenTwinsDictionary.Count > 0);
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

        private KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>> HandleHiddenTwinHashing(CellGroup cg)
        {
            var possU = from openMem in cg.GetOpenMembers()
                        where openMem != null && openMem.Possibilities != null && openMem.Possibilities.Count > 0
                        group openMem by openMem.Possibilities.Count;

            List<Pair<SudokuCell, SudokuCell>> hTwinsList = new List<Pair<SudokuCell, SudokuCell>>();

            foreach(var group in possU)
            {
                Dictionary<SudokuCell, HashSet<int>> setDict = new Dictionary<SudokuCell, HashSet<int>>();
                foreach(SudokuCell u in possU)
                {
                    HashSet<int> possSet = new HashSet<int>(u.Possibilities);
                    setDict.Add(u, possSet);
                }
                if (setDict == null || setDict.Keys.Count == 0)
                {
                    foreach(SudokuCell e in setDict.Keys)
                    {
                        foreach(SudokuCell f in setDict.Keys)
                        {
                            var intersectionTemp = setDict[e].Intersect(setDict[f]);
                            if (e != f && (setDict[e].SetEquals(intersectionTemp) || setDict[f].SetEquals(intersectionTemp)))
                            {
                                Pair<SudokuCell, SudokuCell> hTwin = new Pair<SudokuCell, SudokuCell>(e, f);
                                if (!hTwinsList.Contains(hTwin))
                                    hTwinsList.Add(hTwin);
                            }
                        }
                    }
                }
            }
            return new KeyValuePair<CellGroup, List<Pair<SudokuCell, SudokuCell>>>(cg, hTwinsList);
        }
    }
}
