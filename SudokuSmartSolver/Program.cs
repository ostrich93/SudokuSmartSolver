using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver.strategies;
using System.Diagnostics;

namespace SudokuPuzzleSolver
{
    class Program
    {
        static List<string> exitKeywords = new List<string>() { string.Empty, "exit", "Exit", "quit", "Quit" };
        static void Main(string[] args)
        {
            string puzzleName;
            
            SudokuPuzzle puzzle = SudokuPuzzle.GetPuzzle();
            Agent brainsolver = new Agent();
            do
            {
                Console.WriteLine("Enter a sudoku puzzle file. To quit, type exit, Exit, quit, Quit, or give no input ");
                puzzleName = Console.ReadLine();
                if (!exitKeywords.Contains(puzzleName))
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
                if (exitKeywords.Contains(puzzleName))
                    System.Environment.Exit(0);
            } while (!exitKeywords.Contains(puzzleName));
        }
    }
}
