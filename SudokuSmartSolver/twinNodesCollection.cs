using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuPuzzleSolver
{
    public class TwinNodesCollection
    {
        private List<TwinNode> _twinNodes = new List<TwinNode>();
        private List<SudokuCell> _keyNodes = new List<SudokuCell>();

        public List<TwinNode> TwinNodes { get { return _twinNodes; } }
        public List<SudokuCell> KeyNodes { get { return _keyNodes; } }

        public void AddNewTwinNode(TwinNode tn)
        {
            if (_twinNodes.Count > 0)
            {
                if (!_twinNodes.Contains(tn) && _twinNodes.Select(t => t).Where(tp => tp.GetPair() == new Pair<SudokuCell, SudokuCell>(tn.keyB, tn.keyA)).Count() == 0)
                {
                    _twinNodes.Add(tn);
                    if (!_keyNodes.Contains(tn.keyA))
                        _keyNodes.Add(tn.keyA);
                    if (!_keyNodes.Contains(tn.keyB))
                        _keyNodes.Add(tn.keyB);
                }
            }
            else
            {
                _twinNodes.Add(tn);
                _keyNodes.Add(tn.keyA);
                _keyNodes.Add(tn.keyB);
            }
        }

        public bool ConfirmTwinKey(SudokuCell inquiry)
        {
            return _keyNodes.Contains(inquiry);
        }

        public List<TwinNode> GetTwinNodesWithKey(SudokuCell targetCell)
        {
            if (!ConfirmTwinKey(targetCell))
                return null;
            return _twinNodes.Select(tnc => tnc).Where(tidx => tidx.keyA == targetCell || tidx.keyB == targetCell).ToList();
        }

        public void Clear()
        {
            _twinNodes = new List<TwinNode>();
            _keyNodes = new List<SudokuCell>();
        }
    }
}
