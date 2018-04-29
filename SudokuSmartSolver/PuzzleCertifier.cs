using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class PuzzleCertifier
    {
        CellGroupCertifier cellGroupCertifier = new CellGroupCertifier();
        SudokuPuzzle _puzzle = SudokuPuzzle.GetPuzzle();

        public IEnumerable<SudokuCell> GetAllCellsFromPuzzle()
        {
            return _puzzle.cellColl;
        }

        public bool AllFilled()
        {
            var filledVar = from SudokuCell fc in _puzzle.cellColl
                            where fc.isFilled
                            select fc;
            return filledVar.Count() == 81;
        }

        public bool DoesWeightMatch()
        {
            var filler = from SudokuCell wc in _puzzle.cellColl
                         select wc.FillValue;
            return filler.Sum() == 45 * 9;
        }

        public bool AllGroupsValid()
        {
            var validGroups = _puzzle.rowCollection.Union(_puzzle.colCollection.Union(_puzzle.sgCollection)).Where(groupo => cellGroupCertifier.AllMembersFilled(groupo) && cellGroupCertifier.IsGroupWeightCorrect(groupo) && cellGroupCertifier.MemberValsFitDomain(groupo));
            return validGroups.Count() == 27;
        }
    }
}
