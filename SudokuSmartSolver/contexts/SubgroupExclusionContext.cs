using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;
using System.Diagnostics;

namespace SudokuPuzzleSolver.contexts
{
    public class SubgroupExclusionContext : Context<List<SGUnitIntersectKey>>
    {        
        protected override Percepts percept => Percepts.SubgroupExclusion;
        public override bool GetContext(out List<SGUnitIntersectKey> sgintersections)
        {
            List<CellGroup> potentialSGs = puzzle.GetOpenSubgrids().Where(o => o.GetOpenCellCount() > 1).ToList();
            if (potentialSGs == null || potentialSGs.Count == 0)
            {
                sgintersections = null;
                return false;
            }
            List<CellGroup> potentialRows = GetAssociatedPotentialGroups(UnitType.row, potentialSGs);
            List<CellGroup> potentialColumns = GetAssociatedPotentialGroups(UnitType.column, potentialSGs);

            List<SGUnitIntersectKey> subgroupIntersections = new List<SGUnitIntersectKey>();
            var puzzleWideFills = SudokuPuzzle.sudokuDomain.Where(f => puzzle.GetRemainingInstances(f) > 1);
            if (puzzleWideFills != null && puzzleWideFills.Count() > 0) {
                foreach(int sd in puzzleWideFills)
                {
                    List<CellGroup> candSGs = potentialSGs.Where(i => i.GetRemainingFills().Contains(sd)).ToList();
                    foreach (CellGroup cancan in candSGs ?? new List<CellGroup>())
                    {
                        List<SGUnitIntersectKey> rowIntersects = GetProperSGUIntersects(potentialRows, sd, cancan);
                        if (rowIntersects != null && rowIntersects.Count > 0)
                            subgroupIntersections.AddRange(rowIntersects);
                        List<SGUnitIntersectKey> columnIntersects = GetProperSGUIntersects(potentialColumns, sd, cancan);
                        if (columnIntersects != null && columnIntersects.Count > 0)
                            subgroupIntersections.AddRange(columnIntersects);
                    }
                }
            }
            sgintersections = subgroupIntersections;
            return (sgintersections != null && sgintersections.Count > 0);
        }

        private List<SGUnitIntersectKey> GetProperSGUIntersects(List<CellGroup> associatedGs, int target, CellGroup subGrid)
        {
            if (associatedGs == null || associatedGs.Count == 0)
                return null;
            List<SGUnitIntersectKey> sgsterSects = new List<SGUnitIntersectKey>();
            foreach (CellGroup assoc in associatedGs)
            {
                List<SudokuCell> excCells = HelpGetProperCells(assoc, target, subGrid);
                if (excCells != null)
                {
                    SGUnitIntersectKey sGUnitIntersect = new SGUnitIntersectKey(assoc, excCells, subGrid.Index, target);
                    sgsterSects.Add(sGUnitIntersect);
                }
            }
            return sgsterSects;
        }

        private List<SudokuCell> HelpGetProperCells(CellGroup associatedG, int target, CellGroup subgrid)
        {
            List<SudokuCell> helpVar = associatedG.GetCellsWithPossibility(target);
            if (helpVar == null || helpVar.Count == 0)
            {
                return null;
            }
            List<SudokuCell> exclusionCells = new List<SudokuCell>();
            int intersectCount = helpVar.Select(tcg => tcg).Where(ab => ab.sgNumber == subgrid.Index).Count();
            if (intersectCount == helpVar.Count)
                exclusionCells = helpVar;
            return exclusionCells;
        }

        private List<CellGroup> GetAssociatedPotentialGroups(UnitType unitType, List<CellGroup> posg)
        {
            List<int> sgInds = posg.Select(q => q.Index).ToList();
            if (sgInds == null || sgInds.Count < 1)
                return null;
            switch (unitType)
            {
                case UnitType.row:
                    List<int> rinds = sgInds.Select(n => n).Where(m => SudokuCellExtensions.DeriveRowIndicesFromSG(m) != null && SudokuCellExtensions.DeriveRowIndicesFromSG(m).Count > 0).ToList();
                    if (rinds != null && rinds.Count > 0)
                        return puzzle.GetOpenRows().Select(r => r).Where(s => s != null && s.GetOpenCellCount() > 1 && rinds.Contains(s.Index)).ToList();
                    break;
                case UnitType.column:
                    List<int> colinds = sgInds.Select(s => s).Where(t => SudokuCellExtensions.DeriveColIndicesFromSG(t) != null && SudokuCellExtensions.DeriveColIndicesFromSG(t).Count > 0).ToList();
                    if (colinds != null && colinds.Count > 0)
                        return puzzle.GetOpenColumns().Select(c => c).Where(d => d != null && d.GetOpenCellCount() > 1 && colinds.Contains(d.Index)).ToList();
                    break;
                case UnitType.subgrid:
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}