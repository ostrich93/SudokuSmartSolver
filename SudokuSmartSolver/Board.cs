using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SudokuPuzzleSolver
{
    public class Board
    {
        //private SudokuPuzzle puzzle = SudokuPuzzle.GetPuzzle();
        public List<SudokuCell> cellList = new List<SudokuCell>();
        public CellGroup[] rowList = new CellGroup[9];
        public CellGroup[] columnList = new CellGroup[9];
        public CellGroup[] subgridList = new CellGroup[9];
        public string puzzleId = String.Empty;

        public FrequencyCounter<int> fillFrequencyCounter = new FrequencyCounter<int>();

        public void SetupBoard(string clueFile)
        {
            InitializeCellGroups();
            List<CellData> clueList = ClueSerializationUtility.DeserailizeClues(clueFile, out bool fileExists);
            if (clueList != null && clueList.Count > 0) {
                puzzleId = clueFile;
                for (int i = 0; i < 81; i++)
                {
                    int rn = i / 9;
                    int cn = i % 9;
                    CellData cellular = clueList.FirstOrDefault(a => a.rowNum == rn + 1 && a.columnNum == cn + 1);
                    if (cellular != null)
                    {
                        SudokuCell cellulos = new SudokuCell(cellular);
                        cellList.Add(cellulos);
                        rowList[rn].AddMember(cellulos);
                        columnList[cn].AddMember(cellulos);
                        subgridList[cellulos.sgNumber - 1].AddMember(cellulos);
                        cellulos.rowPointer = rowList[rn];
                        cellulos.columnPointer = columnList[cn];
                        cellulos.subgridPointer = subgridList[cellulos.sgNumber - 1];
                        fillFrequencyCounter.IncrementFor(cellulos.FillValue);
                        Debug.WriteLine(cellulos);
                    }
                    else
                    {
                        SudokuCell cellulos = new SudokuCell();
                        cellList.Add(cellulos);
                        cellulos.rowNumber = rn + 1;
                        cellulos.colNumber = cn + 1;
                        SudokuCellExtensions.AssignSubgridNumber(cellulos);
                        rowList[rn].AddMember(cellulos);
                        columnList[cn].AddMember(cellulos);
                        subgridList[cellulos.sgNumber - 1].AddMember(cellulos);
                        cellulos.rowPointer = rowList[rn];
                        cellulos.columnPointer = columnList[cn];
                        cellulos.subgridPointer = subgridList[cellulos.sgNumber - 1];
                    }
                }
            }
        }

        public void InitializeCellGroups()
        {
            for (int h = 0; h < 9; h++)
            {
                rowList[h] = new CellGroup(UnitType.row);
                columnList[h] = new CellGroup(UnitType.column);
                subgridList[h] = new CellGroup(UnitType.subgrid);
            }
            for (int i=0; i < 9; i++)
            {
                rowList[i] = new CellGroup(UnitType.row);
                rowList[i].SetIndex(i+1);
                columnList[i] = new CellGroup(UnitType.column);
                columnList[i].SetIndex(i+1);
                subgridList[i] = new CellGroup(UnitType.subgrid);
                subgridList[i].SetIndex(i+1);
            }
        }
    }
}
