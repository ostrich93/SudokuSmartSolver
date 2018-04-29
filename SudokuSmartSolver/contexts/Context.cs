using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.contexts
{
    public abstract class Context<T>
    {
        protected readonly SudokuPuzzle puzzle = SudokuPuzzle.GetPuzzle();
        protected virtual Percepts percept { get; }
        public abstract bool GetContext(out T outer);
        public virtual Percepts ContextAlgorithm()
        {
            if (GetContext(out T liad))
                return percept;
            return Percepts.None;
        }
    }
}
