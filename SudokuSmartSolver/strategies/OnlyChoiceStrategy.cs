using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.strategies
{
    public class OnlyChoiceStrategy : Strategy<List<CellGroup>>
    {
        private SudokuPuzzle sudoku = SudokuPuzzle.GetPuzzle();
        public OnlyChoiceStrategy() { }

        public override void AlgorithmInterface(List<CellGroup> param)
        {
            foreach (CellGroup gr in param)
            {
                if (gr.GetOpenCellCount() == 1)
                {
                    SudokuCell onlyCell = gr.GetOpenMembers().FirstOrDefault();
                    onlyCell.FillCell(onlyCell.Possibilities.FirstOrDefault());
                    Console.WriteLine("Filled {0} with {1}", onlyCell.ToString(), onlyCell.FillValue);
                    Debug.WriteLine("Filled {0} with {1}", onlyCell.ToString(), onlyCell.FillValue);

                }
                else
                    Debug.WriteLine("Cell Group does not have only one choice");
            }
        }
    }
}
