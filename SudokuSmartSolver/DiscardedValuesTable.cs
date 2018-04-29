using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SudokuPuzzleSolver
{
    public class DiscardedValuesTable : IDictionary<SudokuCell, List<int>>
    {
        private Dictionary<SudokuCell, List<int>> discardedCellValues;

        public ICollection<SudokuCell> Keys { get { return discardedCellValues.Keys; } }

        public ICollection<List<int>> Values { get { return discardedCellValues.Values; } }

        public int Count { get { return discardedCellValues.Count; } }

        public bool IsReadOnly { get { return false; } }

        public List<int> this[SudokuCell key] { get { return GetPairByKeyType(key).Value; } set { if (value == null) throw new InvalidOperationException(); else AddDiscardedValues(key, value); } }

        public DiscardedValuesTable() : base()
        {
            discardedCellValues = new Dictionary<SudokuCell, List<int>>();
        }

        public void AddKey(SudokuCell keyCell, List<int> value = null)
        {
            if (discardedCellValues.Keys.Contains(keyCell))
                Console.WriteLine("Key already exists in discarded values");
            else
            {
                if (value == null)
                    discardedCellValues[keyCell] = new List<int>();
                else
                {
                    if (value.Contains(0))
                        value.RemoveAll(ak => ak == 0);
                    if (keyCell.isFilled && value.Contains(keyCell.FillValue))
                        value.RemoveAll(af => af == keyCell.FillValue);
                    if (keyCell.isFilled && keyCell.NeighborFills != null && value.Any(v => keyCell.NeighborFills.Contains(v)))
                    {
                        value = value.Except(keyCell.NeighborFills).ToList();
                        if (value == null || value.Count == 0)
                        {
                            discardedCellValues[keyCell] = new List<int>();
                            return;
                        }
                    }
                    discardedCellValues[keyCell] = value;
                }
            }
        }

        public void AddDiscardedValue(SudokuCell keyCell, int discardedValue)
        {
            if (!discardedCellValues.ContainsKey(keyCell))
                AddKey(keyCell, new List<int>() { discardedValue });
            if (discardedCellValues[keyCell] != null && !discardedCellValues[keyCell].Contains(discardedValue))
            {
                if (!keyCell.isFilled || (keyCell.isFilled && discardedValue != keyCell.FillValue))
                {


                    discardedCellValues[keyCell].Add(discardedValue);
                    Console.WriteLine("{0} added to {1}'s list of discarded values", discardedValue, keyCell);
                    Debug.WriteLine("{0} added to {1}'s list of discarded values", discardedValue, keyCell);
                }
                if (!keyCell.isFilled && keyCell.NeighborFills != null && keyCell.NeighborFills.Contains(discardedValue))
                {
                    Console.WriteLine("Cannot add a neighboring value to discarded values table.");
                    Debug.WriteLine("Cannot add a neighboring value to discarded values table.");
                }
                if (keyCell.isFilled && keyCell.FillValue == discardedValue)
                {
                    Console.WriteLine("Cannot discard the fill value of a cell.");
                    Debug.WriteLine("Cannot discard the fill value of a cell.");
                }
            }
            else
            {
                Console.WriteLine("The cell already has {0} inside its list of discarded values.", discardedValue);
                Debug.WriteLine("The cell already has {0} inside its list of discarded values.", discardedValue);
            }
        }

        public void AddDiscardedValues(SudokuCell keyCell, List<int> discardedValues)
        {
            if (discardedCellValues != null && discardedCellValues.ContainsKey(keyCell))
            {
                if (discardedValues.Contains(0))
                    discardedValues.RemoveAll(dv => dv == 0);
                if (keyCell.isFilled && discardedValues.Contains(keyCell.FillValue))
                    discardedValues.RemoveAll(fv => fv == keyCell.FillValue);
                if (discardedValues != null && discardedValues.Count > 0)
                    discardedCellValues[keyCell] = discardedCellValues[keyCell].Union(discardedValues).ToList();
                if (keyCell.NeighborFills != null && discardedValues.Any(dv => keyCell.NeighborFills.Contains(dv)))
                {
                    discardedValues = discardedValues.Except(keyCell.NeighborFills).ToList();
                    if (discardedValues == null || discardedValues.Count == 0)
                        return;
                    else
                        discardedCellValues[keyCell] = discardedCellValues[keyCell].Union(discardedValues).Distinct().ToList();
                }
            }
            else if (discardedCellValues != null && !discardedCellValues.ContainsKey(keyCell))
            {
                AddKey(keyCell, discardedValues);
            }
        }

        public List<int> GetCellDiscardedValues(SudokuCell keyCell)
        {
            return discardedCellValues[keyCell];
        }

        public Dictionary<SudokuCell, List<int>> GetDiscardedValues()
        {
            return discardedCellValues;
        }

        public int RemoveValue(SudokuCell key, int value)
        {
            if (discardedCellValues != null && discardedCellValues.ContainsKey(key))
            {
                if (discardedCellValues[key].Contains(value))
                {
                    discardedCellValues[key].Remove(value);
                    return value;
                }
            }
            return 0;
        }

        public List<int> RemoveValues(SudokuCell keyCell, List<int> values)
        {
            if (discardedCellValues != null && discardedCellValues.ContainsKey(keyCell))
            {
                foreach (int value in values)
                {
                    if (discardedCellValues[keyCell].Contains(value))
                        discardedCellValues[keyCell].Remove(value);
                }
                return values;
            }
            return null;
        }

        public void ClearKey(SudokuCell key)
        {
            discardedCellValues[key].Clear();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void ClearTable()
        {
            discardedCellValues.Clear();
        }

        public bool ContainsKey(SudokuCell key)
        {
            KeyValuePair<SudokuCell, List<int>> pair = discardedCellValues.Where(u => u.Key.GetType() == key.GetType()).FirstOrDefault();
            if (pair.Key == null)
                return false;
            else
                return true;
        }

        public void Add(SudokuCell key, List<int> value)
        {
            (this as ICollection<KeyValuePair<SudokuCell, List<int>>>).Add(new KeyValuePair<SudokuCell, List<int>>(key, value));
        }

        void ICollection<KeyValuePair<SudokuCell, List<int>>>.Add(KeyValuePair<SudokuCell, List<int>> item)
        {
            if (this.ContainsKey(item.Key)){
                if (item.Value.Except(this[item.Key]) == null)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    this[item.Key].AddRange(item.Value.Except(this[item.Key]));
                }
            }
            else
            {
                if (item.Value == null || item.Value.Count < 1)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    discardedCellValues.Add(item.Key, item.Value);
                }
            }
        }

        public bool Remove(SudokuCell key)
        {
            if (ContainsKey(key))
            {
                return discardedCellValues.Remove(GetPairByKeyType(key).Key);
            }
            else
                return false;
        }

        bool ICollection<KeyValuePair<SudokuCell, List<int>>>.Remove(KeyValuePair<SudokuCell, List<int>> item)
        {
            this[item.Key] = null;
            return true;
        }

        public bool Remove(SudokuCell key, int value)
        {
            if (ContainsKey(key))
            {
                if (this[key].Contains(value))
                {
                    return this[key].Remove(value);
                }
            }
            return false;
        }

        public bool TryGetValue(SudokuCell key, out List<int> value)
        {
            if (ContainsKey(key))
            {
                value = GetPairByKeyType(key).Value;
                return true;
            }
            else
            {
                value = default(List<int>);
                return false;
            }
        }

        public void Add(KeyValuePair<SudokuCell, List<int>> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            discardedCellValues.Clear();
        }

        public bool Contains(KeyValuePair<SudokuCell, List<int>> item)
        {
            return Contains(new KeyValuePair<SudokuCell, List<int>>(item.Key, item.Value));
        }

        public void CopyTo(KeyValuePair<SudokuCell, List<int>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<SudokuCell, List<int>> item)
        {
            return Remove(item);
        }

        public IEnumerator<KeyValuePair<SudokuCell, List<int>>> GetEnumerator()
        {
            return discardedCellValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private KeyValuePair<SudokuCell, List<int>> GetPairByKeyType(SudokuCell key)
        {
            KeyValuePair<SudokuCell, List<int>> pair = discardedCellValues.Where(u => u.GetType() == key.GetType()).FirstOrDefault();
            if (pair.Key == null)
            {
                throw new KeyNotFoundException();
            }
            else
                return pair;
        }
    }
}
