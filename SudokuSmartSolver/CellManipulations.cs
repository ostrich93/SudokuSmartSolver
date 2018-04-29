using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SudokuPuzzleSolver
{
    public static class CellManipulations
    {
        public static void FillCell(this SudokuCell sudokuCell, int newFill)
        {
            if (sudokuCell == null)
                new NullReferenceException();
            else {
                if (sudokuCell.Possibilities.Contains(newFill))
                {
                    sudokuCell.FillValue = newFill;
                    Console.WriteLine("{0} now has fill value of {1}", sudokuCell.ToString(), newFill.ToString());
                    Debug.WriteLine("{0} now has fill value of {1}", sudokuCell.ToString(), newFill.ToString());
                    SudokuPuzzle.GetPuzzle().cellBoard.fillFrequencyCounter.IncrementFor(newFill);
                }
                else
                    Console.WriteLine("{0} not part of {1}'s possibilities", newFill.ToString(), sudokuCell.ToString());
            }
        }

        public static void UnfillCell(this SudokuCell sudokuCell)
        {
            if (sudokuCell == null)
                new NullReferenceException();
            else
            {
                if (sudokuCell.isFilled)
                {
                    int oldValue = sudokuCell.FillValue;
                    sudokuCell.FillValue = 0;
                    SudokuPuzzle.GetPuzzle().cellBoard.fillFrequencyCounter.DecrementFor(oldValue);
                }
                else
                    Console.WriteLine("{0} is already unfilled.", sudokuCell.ToString());
            }
        }

        public static List<SudokuCell> TransformPairIntoList(this Pair<SudokuCell, SudokuCell> cellPair)
        {
            if (cellPair == null || (cellPair.GetX() == null && cellPair.GetY() == null))
                return null;
            List<SudokuCell> list = new List<SudokuCell>();
            if (cellPair.GetX() != null)
                list.Add(cellPair.GetX());
            if (cellPair.GetY() != null)
                list.Add(cellPair.GetY());
            return list;
        }
    }
}