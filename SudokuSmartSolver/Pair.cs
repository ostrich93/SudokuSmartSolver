using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class Pair<X, Y>
    {
        private X x;
        private Y y;

        public Pair(X a, Y b)
        {
            x = a;
            y = b;
        }

        public X GetX()
        {
            return x;
        }

        public Y GetY()
        {
            return y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Pair<X, Y> compPair)
            {
                return (x.Equals(compPair.x) && y.Equals(compPair.y)) || (x.Equals(compPair.y) && y.Equals(compPair.x));
            }
            return false;
        }
    }
}
