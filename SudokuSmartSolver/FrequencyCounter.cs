using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class FrequencyCounter<T>
    {
        private Dictionary<T, int> counter;
        private int total;

        public FrequencyCounter()
        {
            counter = new Dictionary<T, int>();
            total = 0;
        }

        public int GetCount(T key)
        {
            if (!counter.ContainsKey(key))
            {
                counter.Add(key, 0);
                return 0;
            }
            return counter[key];
        }

        public void IncrementFor(T key)
        {
            if (!counter.ContainsKey(key))
                counter.Add(key, 1);
            else
            {
                int countValue = counter[key];
                counter[key]++;
            }
            total++;
        }

        public void DecrementFor(T key)
        {
            if (counter.ContainsKey(key))
            {
                int countValue = counter[key];
                counter[key]--;
            }
            total--;
        }

        public double GetPercentage(T key)
        {
            if (!counter.ContainsKey(key))
            {
                return 0.0;
            }
            int countValue = counter[key];
            if (countValue == 0)
                return 0.0;
            double total = 0.0;
            foreach (T k in counter.Keys)
                total += GetCount(k);
            return countValue / total;
        }

        public void Clear()
        {
            counter.Clear();
            total = 0;
        }

        public HashSet<T> GetStates()
        {
            return new HashSet<T>(counter.Keys);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
