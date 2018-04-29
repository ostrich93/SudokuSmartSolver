using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.strategies
{
    public abstract class Strategy<T> //need to make this capable of taking in a generic parameter.
    {
        public abstract void AlgorithmInterface(T param); //algorithm interface is what executes the strategies in each class, e.g it's like a series of instructions.
    }
}
