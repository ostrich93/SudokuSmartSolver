using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class SudokuCellExtensions
    {

        public static void AssignSubgridNumber(SudokuCell inputCell)
        {
            bool canBeFilled = !inputCell.isClue || inputCell.rowNumber == 0 || inputCell.colNumber == 0;
            if (canBeFilled)
            {
                if (inputCell.rowNumber <= 3)
                {
                    if (inputCell.colNumber <= 3)
                        inputCell.sgNumber = 1;
                    else if (inputCell.colNumber <= 6)
                        inputCell.sgNumber = 2;
                    else
                        inputCell.sgNumber = 3;
                }
                else if (inputCell.rowNumber <= 6)
                {
                    if (inputCell.colNumber <= 3)
                        inputCell.sgNumber = 4;
                    else if (inputCell.colNumber <= 6)
                        inputCell.sgNumber = 5;
                    else
                        inputCell.sgNumber = 6;
                }
                else
                {
                    if (inputCell.colNumber <= 3)
                        inputCell.sgNumber = 7;
                    else if (inputCell.colNumber >= 4 && inputCell.colNumber <= 6)
                        inputCell.sgNumber = 8;
                    else
                        inputCell.sgNumber = 9;
                }
            }
            else
            {
                if (inputCell.isClue)
                    Console.WriteLine("Cannot change the subgrid number of a clue");
                else
                    Console.WriteLine("Assign the row and column numbers of the cell a value ranging from 1-9");
            }
        }
        
        public static int getCellDistance(SudokuCell c1, SudokuCell c2)
        {
            int dx = (c2.rowNumber - c1.rowNumber);
            int dy = (c2.colNumber - c2.colNumber);

            double totalDistance = Math.Sqrt((dx ^ 2) + (dy ^ 2));
            return (int)totalDistance;
        }

        public static List<int> DeriveColIndicesFromSG(int sgNum)
        {
            if (sgNum == 1 || sgNum == 4 || sgNum == 7)
                return new List<int>{ 1, 2, 3};
            if (sgNum == 2 || sgNum == 5 || sgNum == 8)
                return new List<int> { 4, 5, 6 };
            if (sgNum == 3 || sgNum == 6 || sgNum == 9)
                return new List<int> { 7, 8, 9 };
            return null;
        }

        public static List<int> DeriveRowIndicesFromSG(int sgNum)
        {
            if (sgNum >= 1 && sgNum <= 3)
                return new List<int> { 1, 2, 3 };
            if (sgNum >= 4 && sgNum <= 6)
                return new List<int> { 4, 5, 6 };
            if (sgNum >= 7 && sgNum <= 9)
                return new List<int> { 7, 8, 9 };
            return null;
        }

        public static List<int> DeriveSGIndicesFromRow(int rowNum)
        {
            if (rowNum <= 3)
                return new List<int> { 1, 2, 3 };
            else if (rowNum <= 6)
                return new List<int> { 4, 5, 6 };
            else if (rowNum >= 7 && rowNum <= 9)
                return new List<int> { 7, 8, 9 };
            return null;
        }

        public static List<int> DeriveSGIndicesFromCol(int colNum)
        {
            if (colNum <= 3)
                return new List<int> { 1, 4, 7 };
            else if (colNum <= 6)
                return new List<int> { 2, 5, 8 };
            else if (colNum >= 7 && colNum <= 9)
                return new List<int> { 3, 6, 9 };
            return null;
        }

        public static bool IsGroupClosed(CellGroup cellGroup)
        {
            if (cellGroup.GetOpenCellCount() == 0)
                return true;
            else
                return false;
        }

        public static List<CellGroup> GetOpenCellGroups(List<int> indices, SudokuPuzzle puzzle, UnitType uType)
        {
            CellGroup[] cellGroups = new CellGroup[3];
            switch (uType)
            {
                case UnitType.row:
                    cellGroups[0] = puzzle.rowCollection[indices[0]-1];
                    cellGroups[1] = puzzle.rowCollection[indices[1] - 1];
                    cellGroups[2] = puzzle.rowCollection[indices[2] - 1];
                    break;
                case UnitType.column:
                    cellGroups[0] = puzzle.colCollection[indices[0] - 1];
                    cellGroups[1] = puzzle.colCollection[indices[1] - 1];
                    cellGroups[2] = puzzle.colCollection[indices[2] - 1];
                    break;
                case UnitType.subgrid:
                    cellGroups[0] = puzzle.sgCollection[indices[0] - 1];
                    cellGroups[1] = puzzle.sgCollection[indices[1] - 1];
                    cellGroups[2] = puzzle.sgCollection[indices[2] - 1];
                    break;
            }

            var openGroups = from CellGroup cg in cellGroups
                             where !IsGroupClosed(cg)
                             select cg;
            if (openGroups != null)
                return openGroups.ToList();
            return null;

        }

        public static List<int> DeriveRowNums(List<SudokuCell> cellConglamorate)
        {
            List<int> rowValues = cellConglamorate.Select(x => x.rowNumber).Distinct().ToList();
            return rowValues;
        }

        public static List<int> DeriveColNums(List<SudokuCell> cellCong)
        {
            List<int> colValues = cellCong.Select(y => y.colNumber).Distinct().ToList();
            return colValues;
        }

        public static List<int> DeriveSGNums(List<SudokuCell> cellCong)
        {
            List<int> sgValues = cellCong.Select(z => z.sgNumber).Distinct().ToList();
            return sgValues;
        }

        public static List<CellGroup> GetMatchingSubgroups(UnitType uType, List<int> indices, SudokuPuzzle puzzle)
        {
            List<CellGroup> retrievedGroups = new List<CellGroup>();
            foreach (int index in indices)
            {
                switch (uType)
                {
                    case UnitType.row:
                        retrievedGroups.Add(puzzle.rowCollection[index - 1]);
                        break;
                    case UnitType.column:
                        retrievedGroups.Add(puzzle.colCollection[index - 1]);
                        break;
                    default:
                        break;
                }
            }
            return retrievedGroups;
        }

    }

    public static class FindCellIntersectionCombinationsExtension
    {
        public static IEnumerable<IEnumerable<SudokuCell>> FindIntersectionCombinations(this IEnumerable<SudokuCell> inspectedcells, int k) //k is number of elements, always starts with number = size of cells being inspected. Recursion
        {
            return k == 0 ? new[] { new SudokuCell[0] } :
                inspectedcells.SelectMany((e, i) => inspectedcells.Skip(i + 1).FindIntersectionCombinations(k - 1).Select(j => (new[] { e }).Concat(j)));
        }
    }
}
