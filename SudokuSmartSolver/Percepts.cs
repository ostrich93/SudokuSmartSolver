using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver
{
    public enum Percepts //include an enum for each of the behaviors?
    {
        None,
        AreSinglePossibilities,
        AreOnlyChoice,
        SubgroupExclusion,
        FindingNakedTwin,
        FindingHiddenTwin
    }
}
