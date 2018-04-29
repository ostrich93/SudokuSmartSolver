using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class TwinNode
    {
        private Pair<SudokuCell, SudokuCell> twinCells;
        public SudokuCell keyA { get { return twinCells.GetX(); } }
        public SudokuCell keyB { get { return twinCells.GetY(); } }
        public HashSet<int> possSet = new HashSet<int>();
        public TwinEnum twinEnum;

        public TwinNode(SudokuCell a, SudokuCell b, HashSet<int> sharedValues, TwinEnum tEnum)
        {
            twinCells = new Pair<SudokuCell, SudokuCell>(a, b);
            possSet = sharedValues;
            twinEnum = tEnum;
        }

        public Pair<SudokuCell, SudokuCell> GetPair()
        {
            return twinCells;
        }

        public override bool Equals(object obj)
        {
            if (obj is TwinNode tnode)
            {
                if (possSet.SetEquals(tnode.possSet))
                {
                    return ((keyA == tnode.keyA && keyB == tnode.keyB) || (keyA == tnode.keyB && keyB == tnode.keyA));
                }
            }
            return false;
        }
    }
}
