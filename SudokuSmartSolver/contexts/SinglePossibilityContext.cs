using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.contexts
{
    public class SinglePossibilityContext : Context<List<SudokuCell>>
    {
        protected override Percepts percept => Percepts.AreSinglePossibilities;
        public override bool GetContext(out List<SudokuCell> outCells)
        {
            List<SudokuCell> singlePossCells = puzzle.cellColl.Select(y => y).Where(z => z !=null).Where(omega => omega.Possibilities?.Count == 1).ToList();
            outCells = singlePossCells;
            return (singlePossCells != null && singlePossCells.Count >= 1);
        }
    }
}
