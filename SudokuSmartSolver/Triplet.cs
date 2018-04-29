using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class Triplet<X, Y, Z>
    {
        private X x;
        private Y y;
        private Z z;

        public Triplet(X a, Y b, Z c)
        {
            x = a;
            y = b;
            z = c;
        }

        public X GetX()
        {
            return x;
        }

        public Y GetY()
        {
            return y;
        }

        public Z GetZ()
        {
            return z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Triplet<X, Y, Z> compTrip)
                return x.Equals(compTrip.x) && y.Equals(compTrip.y) && z.Equals(compTrip.z);
            return false;
        }
    }
}
