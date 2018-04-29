using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;
using System.Diagnostics;

namespace SudokuPuzzleSolver.strategies
{
    public class SubgroupExclusionStrategy: Strategy<List<SGUnitIntersectKey>>
    {
        private SudokuPuzzle sudoku = SudokuPuzzle.GetPuzzle();
        

        public SubgroupExclusionStrategy()
        {

        }

        public override void AlgorithmInterface(List<SGUnitIntersectKey> paramets)
        {
            foreach (SGUnitIntersectKey sgKey in paramets ?? new List<SGUnitIntersectKey>())
            {
                CellGroup targetSG = sudoku.sgCollection[sgKey.subgridIndex - 1];
                var exclusionTargets = targetSG.GetOpenMembers().Select(es => es).Where(ex => ex.Possibilities.Any(t => sgKey.intersectedUnitValues.Contains(t)) && !sgKey.intersectCells.Contains(ex));
                if (exclusionTargets != null && exclusionTargets.Count() > 0)
                {
                    foreach(SudokuCell exTarget in exclusionTargets)
                    {
                        List<int> targetDisValues = exTarget.Possibilities.Intersect(sgKey.intersectedUnitValues).ToList();
                        sudoku.discardedValuesTable.AddDiscardedValues(exTarget, targetDisValues);
                    }
                }
            }
        }
        //ultimately, what the strategy is supposed to do is to go through all numbers 1-9. For each number, it takes all the subgrids that have more than one open cell that have that number in its remaining fills. 
            //In each of these subgrids, it finds all the sudoku cells with that number as possiblity and checks to see if the only cells in its row and column contain that possibility are the ones in the subgrid.
                //if so, store the List<SudokuCell> in a table/collection to be marked as lists of sudoku cells with subgroup intersections at a shared fill value. Might want to use a custom data structure for this (retool SGUintIntersect
                //to have a list of cells as the parameter, and an List of integers to mark the int value they're an intersect at. This will be useful to know what values to eliminate from possibilities of neighbors that don't have the intersect
            //after all the subgrids are checked, go through each sgIntersect and add the number to all the cells in the subgrid that are not listed in the intersect and that do not already contain that value in their discarded values
        //do this until all the numbers are done.

    }
}
