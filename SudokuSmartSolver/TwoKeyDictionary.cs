using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class TwoKeyDictionary<X, Y, V>: Dictionary<Pair<X,Y>, V>
    {
        public void Put(X key1, Y key2, V value)
        {
            base.Add(new Pair<X, Y>(key1, key2), value);
        }

        public V Get(X key1, Y key2)
        {
            if (base.TryGetValue(new Pair<X, Y>(key1, key2), out V value))
                return value;
            return value;
        }

        public bool ContainsKey(X key1, Y key2)
        {
            return base.ContainsKey(new Pair<X, Y>(key1, key2));
        }

        public V RemoveKey(X key1, Y key2, V value)
        {
            base.Remove(new Pair<X, Y>(key1, key2));
            return value;
        }
    }
}
