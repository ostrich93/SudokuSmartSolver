using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;

namespace SudokuPuzzleSolver.contexts
{
    public class TwinContext : Context //naked twin and hidden twin contexts inherit from this class
    {
        protected virtual List<CellGroup> potentialRows { get { return GetCellGroupsWithPotentialTwins(UnitType.row); } }
        protected virtual List<CellGroup> potentialColumns { get { return GetCellGroupsWithPotentialTwins(UnitType.column); } }
        protected virtual List<CellGroup> potentialSubgrids { get { return GetCellGroupsWithPotentialTwins(UnitType.subgrid); } }

        protected override bool GetContext()
        {
            return (potentialRows != null && potentialColumns != null && potentialSubgrids != null);
        }

        public override Percepts ContextAlgorithm()
        {
            if (GetContext())
                return percept;
            return Percepts.None;
        }

        protected Dictionary<SudokuCell, List<int>> GetGroupPotentialFills(CellGroup cellGroup)
        {
            Dictionary<SudokuCell, List<int>> potentialFillsDictionary = new Dictionary<SudokuCell, List<int>>();
            List<SudokuCell> openCells = cellGroup.GetOpenMembers();
            foreach (SudokuCell openCell in openCells ?? new List<SudokuCell>())
                potentialFillsDictionary.Add(openCell, openCell.Possibilities);
            return potentialFillsDictionary;
        }

        //make a virtual method probably
        protected virtual List<CellGroup> GetCellGroupsWithPotentialTwins(UnitType unitType)
        {
            List<CellGroup> potentialGroup = new List<CellGroup>();
            switch (unitType)
            {
                case UnitType.row:
                    potentialGroup = puzzle.GetOpenRows().Select(x => x).Where(y => y.GetOpenCellCount() >= 2).ToList();
                    break;
                case UnitType.column:
                    potentialGroup = puzzle.GetOpenColumns().Select(u => u).Where(w => w.GetOpenCellCount() >= 2).ToList();
                    break;
                case UnitType.subgrid:
                    potentialGroup = puzzle.GetOpenSubgrids().Select(q => q).Where(r => r.GetOpenCellCount() >= 2).ToList();
                    break;
            }
            return potentialGroup;
        }

        protected PotentialTwinCombo GetTwinPotential(List<CellGroup> rows, List<CellGroup> columns, List<CellGroup> subgrids)
        {
            if (rows != null && columns != null && subgrids != null)
                return PotentialTwinCombo.NoNulls;
            if (rows != null && columns != null && subgrids == null)
                return PotentialTwinCombo.RowsAndColumns;
            if (rows != null && columns == null && subgrids != null)
                return PotentialTwinCombo.RowsAndSubgrids;
            if (rows == null && columns != null && subgrids != null)
                return PotentialTwinCombo.ColumnsAndSubgrids;
            if (rows != null && columns == null && subgrids == null)
                return PotentialTwinCombo.OnlyRows;
            if (rows == null && columns != null && subgrids == null)
                return PotentialTwinCombo.OnlyColumns;
            if (rows == null && columns == null && subgrids != null)
                return PotentialTwinCombo.OnlySubgrids;
            return PotentialTwinCombo.AllNulls;
        }

        public QuantityType GetQuantityType(PotentialTwinCombo combo)
        {
            QuantityType qType = QuantityType.None;
            switch (combo)
            {
                case PotentialTwinCombo.NoNulls:
                    qType = QuantityType.Multiple;
                    break;
                case PotentialTwinCombo.RowsAndColumns:
                    qType = QuantityType.Multiple;
                    break;
                case PotentialTwinCombo.RowsAndSubgrids:
                    qType = QuantityType.Multiple;
                    break;
                case PotentialTwinCombo.ColumnsAndSubgrids:
                    qType = QuantityType.Multiple;
                    break;
                case PotentialTwinCombo.OnlyRows:
                    if (potentialRows.Count == 1)
                        qType = QuantityType.Singular;
                    else
                        qType = QuantityType.Multiple;
                    break;
                case PotentialTwinCombo.OnlyColumns:
                    if (potentialColumns.Count == 1)
                        qType = QuantityType.Singular;
                    else
                        qType = QuantityType.Multiple;
                    break;
                case PotentialTwinCombo.OnlySubgrids:
                    if (potentialSubgrids.Count == 1)
                        qType = QuantityType.Singular;
                    else
                        qType = QuantityType.Multiple;
                    break;
                case PotentialTwinCombo.AllNulls:
                    break;
            }
            return qType;
        }
    }
}
