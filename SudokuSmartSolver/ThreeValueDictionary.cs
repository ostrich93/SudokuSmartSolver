using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class ThreeValueDictionary<T, X, Y, Z>: Dictionary<T, Triplet<X, Y, Z>>
    {
        public void Put(T key, X value1, Y value2, Z value3)
        {
            base.Add(key, new Triplet<X, Y, Z>(value1, value2, value3));
        }

        public Triplet<X, Y, Z> Get(T key)
        {
            if (base.TryGetValue(key, out Triplet<X,Y,Z> value))
                return value;
            return value;
        }

        public bool ContainsKey(T key)
        {
            return base.ContainsKey(key);
        }

        public Triplet<X,Y,Z> RemoveKey(T key, Triplet<X,Y,Z> value)
        {
            base.Remove(key);
            return value;
        }
    }
}
