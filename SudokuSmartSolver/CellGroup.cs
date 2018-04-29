using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class CellGroup //assign this as a parameter for SudokuCells.
    {
        private UnitType _groupType;
        public UnitType GroupType { get { return _groupType; } }
        private List<SudokuCell> _members = new List<SudokuCell>();
        public List<SudokuCell> Members { get { return _members; } }
        private int index;
        public int Index { get { return index; } }
        public bool IsFull { get { return GetOpenCellCount() == 0; } }

        public CellGroup(UnitType uT)
        {
            _groupType = uT;
            index = 0;
        }

        public void SetIndex(int id)
        {
            index = id;
        }

        public void AddMember(SudokuCell newMember)
        {
            _members.Add(newMember);
        }

        public void RemoveMember(SudokuCell target)
        {
            if (!_members.Contains(target))
                Console.WriteLine("The desired cell is not in this group.");
            else
                _members.Remove(target);
        }

        public bool ListWithinCellGroup(List<SudokuCell> scells)
        {
            return Members.Intersect(scells) == scells; //if all of the cells are in the subgroup, then the intersection should be the same as scells
        }

        public int GetOpenCellCount()
        {
            if (_members == null || _members.Count == 0)
                return 0;
            List<SudokuCell> openCells = _members.Where(x => !x.isFilled).ToList();
            if (openCells == null)
                return 0;
            return openCells.Count;
        }

        public List<SudokuCell> GetCellsWithPossibility(int n)
        {
            if (GetRemainingFills() == null || !GetRemainingFills().Contains(n))
                return null;
            var posCs = Members.Select(x => x).Where(y => !y.isFilled && y.Possibilities.Contains(n)).ToList();
            return posCs;
        }

        public List<SudokuCell> GetOpenMembers()
        {
            if (_members == null || _members.Count == 0)
                return null;
            List<SudokuCell> openCells = _members.Where(x => !x.isFilled && !x.isClue).ToList();
            return openCells;
        }

        public List<int> GetGroupFills() // get the cells that are not empty. Use this to derive possibilities
        {
            if (_members == null || _members.Count == 0)
            {
                return null;
            }
            var filledQuery = from SudokuCell fc in _members
                                           where fc.isFilled == true
                                           select fc;
            //if filledQuery != null
            List<SudokuCell> filledCells = filledQuery.ToList();
            List<int> filledValues = filledCells.Select(A => A.FillValue).ToList();
            return filledValues;
            
        }

        public List<int> GetRemainingFills()
        {
            List<int> usedFills = GetGroupFills();
            if (usedFills == null || usedFills.Count == 0)
            {
                return null;
            }
            List<int> remainingFills = SudokuPuzzle.sudokuDomain.Except(usedFills).ToList();
            return remainingFills;
        }

        //function to input a cell and return list not containing said cell (if cell is inside)
        public List<SudokuCell> GetNeighboringCells(SudokuCell inputCell)
        {
            if (_members.Contains(inputCell))
            {
                return _members.Where(z => z != inputCell).ToList();
            }
            return null;
        }

        public IEnumerable<List<int>> getCellPossibilityLists()
        {
            return _members.Select(m => m?.Possibilities).Where(n => n != null && n.Count > 0);
        }

        //function to search for an integer from 1-9 and whether or not it is used as a fill value
        public bool TargetValueUsed(int target)
        {
            if (!SudokuPuzzle.sudokuDomain.Contains(target))
                return false;
            foreach (SudokuCell cell in _members)
            {
                if (cell.FillValue == target)
                    return true;
            }
            return false;
        }

        //function to return cell(s) that all belong to one different subgroup
        public List<SudokuCell> ReturnSharedNeighbors(SudokuCell sc)
        {
            if (!_members.Contains(sc))
                return null;
            List<SudokuCell> sharedNeighbors = new List<SudokuCell>();
            foreach(SudokuCell icell in _members)
            {
                switch (_groupType)
                {
                    case UnitType.row:
                        if (icell.colNumber == sc.colNumber || icell.sgNumber == sc.sgNumber)
                            sharedNeighbors.Add(icell);
                        break;

                    case UnitType.column:
                        if (icell.rowNumber == sc.rowNumber || icell.sgNumber == sc.sgNumber)
                            sharedNeighbors.Add(icell);
                        break;

                    case UnitType.subgrid:
                        if (icell.rowNumber == sc.rowNumber || icell.colNumber == sc.colNumber)
                            sharedNeighbors.Add(icell);
                        break;

                    default:
                        break;
                }
            }
            return sharedNeighbors;
        }

        public bool CheckValidity()
        {
            bool va =  _members.Select(x => x.isFilled).Count() == _members.Count();
            List<int> groupFills = GetGroupFills();
            bool vb = va && groupFills.Distinct().Count() == groupFills.Count;
            bool vc = vb && groupFills.Intersect(SudokuPuzzle.sudokuDomain) == SudokuPuzzle.sudokuDomain;
            return vc;
        }

        public Pair<UnitType, int> ReturnGroupIndexPair()
        {
            return new Pair<UnitType, int>(GroupType, Index);
        }

        public override string ToString()
        {
            return GroupType.ToString() + index.ToString();
        }
    }
}
