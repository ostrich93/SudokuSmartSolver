using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver.strategies;
using System.Diagnostics;

namespace SudokuPuzzleSolver
{
    //DEBUG STATUS: AS OF NOW, MAIN CAUSE OF ERRORS IS RELATED TO DISCARDED VALUES TABLE: SOMETIMES, CELLS HAVE ALL 9 VALUES IN THEIR DISCARDED VALUES OR THEY SOMEHOW HAVE ALL 9 POSSIBLE VALUES IN NEIGHBORFILLS
    class Program
    {
        static void Main(string[] args)
        {
            string puzzleName;
            
            SudokuPuzzle puzzle = SudokuPuzzle.GetPuzzle();
            Agent brainsolver = new Agent();
            do
            {
                Console.WriteLine("Enter a sudoku puzzle file (or type exit/Exit to quit)");
                puzzleName = Console.ReadLine();
                if (!puzzleName.Equals("exit") && !puzzleName.Equals("Exit"))
                {
                    puzzle.BuildPuzzle(puzzleName);
                    brainsolver.Init();
                    brainsolver.Execute();
                    if (brainsolver.IsPuzzleSolved())
                    {
                        Console.WriteLine("The puzzle {0} was solved", puzzleName);
                        puzzle.ClearPuzzle();
                    }
                    //}
                }

            } while (!puzzleName.Equals("exit") || !puzzleName.Equals("Exit") || !puzzleName.Equals(""));
            //Console.WriteLine("List of Sudoku Cells: ", puzzle.cellColl);

            //SudokuCell tCellA = agent.GetLeastOptCell();
            //SudokuCell tCellA = puzzle.cellColl[1, 7];
            //Debug.WriteLine(tCellA.FillValue);
            //List<int> possums = tCellA.Possibilities;
            //Debug.WriteLine(possums);

            //SudokuCell tcellB = agent.GetLeastOpenNeighbors();
            //Debug.WriteLine(tcellB.FillValue);

            //SudokuCell tcellC = agent.GetLeastOpenSGNeighbors();
            //Console.WriteLine(tcellC.FillValue);

            /*List<SudokuCell> tcells = agent.GetSinglePossibilityCells();
            while (tcells.Count > 0)
            {
                //tcells = agent.GetSinglePossibilityCells();
                foreach (SudokuCell acell in tcells)
                    Debug.WriteLine(acell + "; Possible Fills: " + acell.Possibilities[0]);
                Debug.WriteLine(tcells.Count);
                foreach (SudokuCell bcell in tcells)
                {
                    bcell.FillValue = bcell.Possibilities[0];
                    //tcells.Remove(bcell);
                }
                tcells = agent.GetSinglePossibilityCells();
                //if (tcells.Count > 0)
                //{
                //    tcells[0].FillValue = tcells[0].Possibilities[0];
                //    tcells.RemoveAt(0);
                //}
            }*/


            Console.ReadLine();
        }
    }
}
