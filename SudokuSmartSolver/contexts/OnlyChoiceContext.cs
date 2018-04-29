using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.contexts
{
    public class OnlyChoiceContext : Context<List<CellGroup>>
    {
        protected override Percepts percept { get => Percepts.AreOnlyChoice; }
        public override bool GetContext(out List<CellGroup> otcgroups)
        {
            List<CellGroup> onlyChoiceCellGroups = puzzle.GetUnifiedOpenCellGroups().Select(y => y).Where(z => z.GetRemainingFills().Count == 1).ToList();
            otcgroups = onlyChoiceCellGroups;
            return (onlyChoiceCellGroups != null && onlyChoiceCellGroups.Count >= 1);
        }

        protected bool GetContext(List<CellGroup> onlyChoiceGroups) //allow for quicker runtimes
        {
            return onlyChoiceGroups != null ? onlyChoiceGroups.Count >= 1 : false;
        }

        public List<CellGroup> GetOnlyChoiceCellGroups()
        {
            return puzzle.GetUnifiedOpenCellGroups().Select(y => y).Where(z => z.GetRemainingFills().Count == 1).ToList();
        }
    }
}
