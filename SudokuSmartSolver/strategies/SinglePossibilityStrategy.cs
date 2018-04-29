using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;
using System.Diagnostics;

namespace SudokuPuzzleSolver.strategies
{
    public class SinglePossibilityStrategy: Strategy<List<SudokuCell>> {
        private SudokuPuzzle sudoku = SudokuPuzzle.GetPuzzle();

        public SinglePossibilityStrategy()
        {

        }

        public override void AlgorithmInterface(List<SudokuCell> param)
        {
            //param.AsParallel().ForAll(a => a.FillValue = a.Possibilities.First());
            for (int i = 0; i < param.Count; i++)
            {
                if (param[i].Possibilities != null && param[i].Possibilities.Count > 0)
                {
                    Debug.WriteLine("{0} : {1}", param[i], param[i].Possibilities.First());
                    param[i].FillCell(param[i].Possibilities.First());
                    Debug.WriteLine("{0} is filled.", param[i]);
                }
            }
        }
    }
}
