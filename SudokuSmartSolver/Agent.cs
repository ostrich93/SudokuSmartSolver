using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuPuzzleSolver;
using SudokuPuzzleSolver.strategies;
using System.Diagnostics;
using SudokuPuzzleSolver.contexts;

namespace SudokuPuzzleSolver
{
    public enum LiveStatus
    {
        IsDead,
        IsAlive
    }

    public enum PotentialTwinCombo //link to the bools in the conditionals for naked/hidden twin, use as parameters for a function to handle the process of running through twins
    {
        NoNulls,
        RowsAndColumns,
        RowsAndSubgrids,
        ColumnsAndSubgrids,
        OnlyRows,
        OnlyColumns,
        OnlySubgrids,
        AllNulls
    }

    public class Agent
    {
        private SudokuPuzzle puzzle = SudokuPuzzle.GetPuzzle(); //can probably get rid of this field once Puzzle turned into singleton?

        private OnlyChoiceStrategy onlyChoiceStrategy = new OnlyChoiceStrategy();
        private SinglePossibilityStrategy singlePossibilityStrategy = new SinglePossibilityStrategy();
        private SubgroupExclusionStrategy subgroupExclusionStrategy = new SubgroupExclusionStrategy();
        //private HiddenTwinExclusionStrategy hiddenTwinExclusionStrategy = new HiddenTwinExclusionStrategy();
        private NakedTwinExclusionStrategy nakedTwinExclusionStrategy = new NakedTwinExclusionStrategy();

        private List<int> availableFills = SudokuPuzzle.sudokuDomain;

        private OnlyChoiceContext onlyChoiceContext = new OnlyChoiceContext();
        private SinglePossibilityContext singlePossibilityContext = new SinglePossibilityContext();
        private SubgroupExclusionContext subgroupExclusionContext = new SubgroupExclusionContext();
        private NakedTwinExclusionContext nakedTwinExclusionContext = new NakedTwinExclusionContext();
        //private HiddenTwinExclusionContext hiddenTwinExclusionContext = new HiddenTwinExclusionContext();

        private List<SudokuCell> stTemp = new List<SudokuCell>();
        private List<CellGroup> ocTemp = new List<CellGroup>();
        private List<SGUnitIntersectKey> sgeTemp = new List<SGUnitIntersectKey>();
        private TwinNodesCollection ntTemp = new TwinNodesCollection();
        //private Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> htTemp = new Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>();

        private Percepts thisPercept = Percepts.None;

        public PuzzleCertifier certifier = new PuzzleCertifier();

        private LiveStatus isAlive;

        public Agent()
        {
            isAlive = LiveStatus.IsDead;
            Console.WriteLine("Agent Created.");
        }

        public void Init()
        {
            isAlive = LiveStatus.IsAlive;
        }

        public void Execute()
        {
            while (isAlive == LiveStatus.IsAlive && puzzle != null && IsPuzzleSolved() == false)
            {
                thisPercept = GetPerceptForRound();
                switch (thisPercept)
                {
                    case Percepts.None:
                        Console.WriteLine("No strategy can be executed this round.");
                        Debug.WriteLine("No strategy can be exceuted this round.");
                        break;
                    case Percepts.AreOnlyChoice:
                        onlyChoiceStrategy.AlgorithmInterface(ocTemp);
                        Console.WriteLine("Only Choice Strategy executed this round");
                        Debug.WriteLine("Only Choice Strategy executed this round.");
                        break;
                    case Percepts.AreSinglePossibilities:
                        singlePossibilityStrategy.AlgorithmInterface(stTemp);
                        break;
                    case Percepts.SubgroupExclusion:
                        subgroupExclusionStrategy.AlgorithmInterface(sgeTemp);
                        Console.WriteLine("Subgroup Exclusion executed this round");
                        Debug.WriteLine("Subgroup Exclusion executed this round.");
                        break;
                    case Percepts.FindingNakedTwin:
                        nakedTwinExclusionStrategy.AlgorithmInterface(ntTemp); //stuck on infinite loop here. THe issue appears to be that a list of twins are returned BUT none of the twin's neighbors have twin values as possibilities
                        Console.WriteLine("Naked Twin Strategy executed this round");
                        Debug.WriteLine("Naked Twin Strategy executed this round.");
                        break;
                    //case Percepts.FindingHiddenTwin:
                    //    hiddenTwinExclusionStrategy.AlgorithmInterface(htTemp);
                    //    Console.WriteLine("Hidden Twin Strategy executed this round");
                    //    Debug.WriteLine("Hidden Twin Strategy executed this round.");
                    //    break;
                    default:
                        break;
                }
                if (IsPuzzleSolved()) { //execute this if statement at the end of the while loop
                    isAlive = LiveStatus.IsDead;
                    //Draw Board?
                    Console.WriteLine("The puzzle is solved.");
                    CleanUp();
                }
            }
        }

        public bool IsPuzzleSolved()
        {
            return certifier.AllFilled() && certifier.AllGroupsValid() && certifier.DoesWeightMatch();
        }

        public void RestartPuzzle()
        {
            foreach (SudokuCell fCell in puzzle.cellColl)
            {
                if (!fCell.isClue && !fCell.isFilled)
                {
                    fCell.FillValue = 0;
                }
            }
        }

        private Percepts GetPerceptForRound()
        {
            if (onlyChoiceContext.ContextAlgorithm() == Percepts.AreOnlyChoice)
            {
                bool ot = onlyChoiceContext.GetContext(out ocTemp);
                return Percepts.AreOnlyChoice;
            }
            if (singlePossibilityContext.ContextAlgorithm() == Percepts.AreSinglePossibilities)
            {
                bool sc = singlePossibilityContext.GetContext(out stTemp);
                return Percepts.AreSinglePossibilities;
            }
            if (subgroupExclusionContext.ContextAlgorithm() == Percepts.SubgroupExclusion) //bugs are mainly here. Think it might be discarding too many values? That or too broad definition. Might try without it. 
            {
                bool sgt = subgroupExclusionContext.GetContext(out sgeTemp);
                return Percepts.SubgroupExclusion;
            }
            if (nakedTwinExclusionContext.ContextAlgorithm() == Percepts.FindingNakedTwin) {
                bool ntt = nakedTwinExclusionContext.GetContext(out ntTemp); //bool should be changed so that it doesn't return the percept if none of the neighbors of the twins share any possibility values in the set.
                return Percepts.FindingNakedTwin;
            }
            //if (hiddenTwinExclusionContext.ContextAlgorithm() == Percepts.FindingHiddenTwin)
            //{
            //    bool hnht = hiddenTwinExclusionContext.GetContext(out htTemp);
            //    return Percepts.FindingHiddenTwin;
            //}
            return Percepts.None;
        }

        public void CleanUp()
        {
            thisPercept = Percepts.None;
            //sgeTemp.Clear();
            stTemp.Clear();
            //htTemp.Clear();
            ntTemp.Clear();
            ocTemp.Clear();
        }
    }
}
