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
            List<SGUnitIntersectKey> sgIntersections = new List<SGUnitIntersectKey>();
            for (int i = 1; i < 10; i++)
            {
                List<CellGroup> subgridCands = puzzle.GetOpenSubgrids().Where(osg => osg.GetRemainingFills().Contains(i)).ToList();
                if (subgridCands != null && subgridCands.Count > 0)
                {
                    foreach (CellGroup sgrid in subgridCands)
                    {
                        List<SudokuCell> sgMems = sgrid.GetOpenMembers().Where(sgmem => sgmem.Possibilities.Contains(i)).ToList();
                        if (sgMems != null && sgMems.Count > 0)
                        {
                            List<SudokuCell> sgExlclusivePossCells = sgMems.Where(sgc => SoloSGNs(sgc, i)).ToList();
                            List<SudokuCell> nonExclusiveCells = sgMems.Where(sgc => !SoloSGNs(sgc, i)).ToList();
                            if (sgExlclusivePossCells.Count >= 1)
                            {
                                //create new SGUIntersectKey and add to list
                                sgIntersections.Add(new SGUnitIntersectKey(sgrid, sgExlclusivePossCells, nonExclusiveCells, i));
                            }
                        }
                    }
                }
            }
            sgintersections = sgIntersections.Where(a => a.elimCells != null && a.elimCells.Count > 0).ToList();
            return sgintersections != null && sgintersections.Count > 0;
        }

        private bool SoloSGNs(SudokuCell scell, int fN)
        {
            return (scell.GetNeighborsWithPossibility(fN, UnitType.row).Select(s => s.sgNumber).Distinct().Count() == 1) || (scell.GetNeighborsWithPossibility(fN, UnitType.column).Select(s => s.sgNumber).Distinct().Count() == 1);
        }

        
    }
}