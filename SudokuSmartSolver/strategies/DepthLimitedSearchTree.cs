using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;
using SudokuPuzzleSolver.commands;

namespace SudokuPuzzleSolver.strategies
{
    public class DepthLimitedSearchTree
    {
        private SudokuCell root;
        public SudokuCell current;

        public readonly int max_depth = 5;
        public int currentDepth = 0;

        protected List<SudokuCell> exploredCells;
        protected Stack<SudokuCell> discoveredCells;
        protected Dictionary<SudokuCell, SudokuCell> parents;
        protected Dictionary<SudokuCell, List<int>> attemptedValues;

        public DepthLimitedSearchTree(SudokuCell rootcell)
        {
            root = rootcell;
            current = root;
            exploredCells = new List<SudokuCell>();
            discoveredCells = new Stack<SudokuCell>();
            parents = new Dictionary<SudokuCell, SudokuCell>();
            attemptedValues = new Dictionary<SudokuCell, List<int>>();

            discoveredCells.Push(root);
        }

        public void AddToDiscovered(SudokuCell discCell)
        {

        }

        public void AddNeighbors(List<SudokuCell> inputNeighbors)
        {
            foreach (SudokuCell s in inputNeighbors)
                discoveredCells.Push(s);
        }

        public void Traverse()
        {
            //basically, the agent needs to start out by making a DFS tree after finding the cell with the least number of possible fill values and/or the least number of open neighbors, setting that cell as the root.
            //add the (adjacent?) neighbors with AddNeighbors function
            //fill the cell with a possible value and record it in the fillValues dictionary.
            //go to the next neighbor in the stack (set it to the current)
            //if the current node has no possible fill values, undo everything except the recording of the parent node's fill value in the fill values dictionary.
            //try a different fill value and repeat until a valid fill value is selected for the neighbor.
            //once the now-current cell is filled, get a neighbor that is in the same subgrid>column>row (in order of priority) as both the current and the root cell.
            
            //add a cell to the explored list when either it's filled or all the possible fill values are in its entry in attempted values.

            if (currentDepth < max_depth)
            {
                while (exploredCells.Contains(discoveredCells.Peek()))
                {
                    exploredCells.Add(discoveredCells.Pop());
                }
                current = discoveredCells.Pop();
                currentDepth++;
            }
        }
    }
}
