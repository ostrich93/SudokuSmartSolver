using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SudokuPuzzleSolver
{
    //first map maps to a key of fillValue and the sudokuCell it is a possiblity of, and first reduce stores by fillValue the list of cells it is a possibility of
    //second map would emit for each fillValue one key for each combination of sets it's in, with fillValue as value, and second reduce stores final answer.
    public class TwinMapper
    {
        public static ConcurrentBag<CellPossAssociation> possBag = new ConcurrentBag<CellPossAssociation>();
        public BlockingCollection<CellPossAssociation> possTwins = new BlockingCollection<CellPossAssociation>(possBag);

        //public IEnumerable<CellPossAssociation> GetCellPossibilityAssociations(CellGroup cell_gr) {

        //}

        public void MapCellAssociations(CellGroup c_gr)
        {

        }

//        public ConcurrentDictionary<>
    }
}
