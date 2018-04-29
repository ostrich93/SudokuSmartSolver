using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class TwoValueDictionary<T, X, Y>: Dictionary<T, Pair<X,Y>>
    {
        public void Put(T key, X v1, Y v2)
        {
            base.Add(key, new Pair<X, Y>(v1, v2));
        }

        public Pair<X,Y> Get(T key)
        {
            if (base.TryGetValue(key, out Pair<X, Y> value))
                return value;
            return value;
        }

        public bool ContainsKey(T key)
        {
            return base.ContainsKey(key);
        }

        public Pair<X, Y> RemoveKey(T key, Pair<X, Y> value)
        {
            base.Remove(key);
            return value;
        }
    }
}
