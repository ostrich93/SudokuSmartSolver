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
        //private SubgroupExclusionStrategy subgroupExclusionStrategy = new SubgroupExclusionStrategy();
        private HiddenTwinExclusionStrategy hiddenTwinExclusionStrategy = new HiddenTwinExclusionStrategy();
        private NakedTwinExclusionStrategy nakedTwinExclusionStrategy = new NakedTwinExclusionStrategy();

        private List<int> availableFills = SudokuPuzzle.sudokuDomain;

        private OnlyChoiceContext onlyChoiceContext = new OnlyChoiceContext();
        private SinglePossibilityContext singlePossibilityContext = new SinglePossibilityContext();
        //private SubgroupExclusionContext subgroupExclusionContext = new SubgroupExclusionContext();
        private NakedTwinExclusionContext nakedTwinExclusionContext = new NakedTwinExclusionContext();
        private HiddenTwinExclusionContext hiddenTwinExclusionContext = new HiddenTwinExclusionContext();

        private List<SudokuCell> stTemp = new List<SudokuCell>();
        private List<CellGroup> ocTemp = new List<CellGroup>();
        //private List<SGUnitIntersectKey> sgeTemp = new List<SGUnitIntersectKey>();
        private Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> ntTemp = new Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>();
        private Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>> htTemp = new Dictionary<CellGroup, List<Pair<SudokuCell, SudokuCell>>>();

        private Percepts thisPercept = Percepts.None;

        public PuzzleCertifier certifier = new PuzzleCertifier();

        private List<Percepts> perceptsList = new List<Percepts>();
        //private Percepts lastPercept { get { if (perceptsList.Count == 0) { return Percepts.None; } return perceptsList.Last(); } }
        //private Percepts percept = Percepts.None;
        //private Dictionary<Percepts, int> consecutiveActionsDictionary = new Dictionary<Percepts, int>() { {Percepts.AreSinglePossibilities, 0}, {Percepts.AreOnlyChoice, 0}, {Percepts.SubgroupExclusion, 0}, {Percepts.FindingNakedTwin, 0}, { Percepts.FindingHiddenTwin, 0 } };
        private LiveStatus isAlive;
        //private readonly int consecutiveLimit = 200;

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
                    case Percepts.AreSinglePossibilities:
                        singlePossibilityStrategy.AlgorithmInterface(stTemp);
                        break;
                    case Percepts.AreOnlyChoice:
                        onlyChoiceStrategy.AlgorithmInterface(ocTemp);
                        Console.WriteLine("Only Choice Strategy executed this round");
                        Debug.WriteLine("Only Choice Strategy executed this round.");
                        break;
                        //case Percepts.SubgroupExclusion:
                        //    subgroupExclusionStrategy.AlgorithmInterface(sgeTemp);
                        //Console.WriteLine("Subgroup Exclusion executed this round");
                        //Debug.WriteLine("Subgroup Exclusion executed this round.");
                    //    break;
                    case Percepts.FindingNakedTwin:
                        nakedTwinExclusionStrategy.AlgorithmInterface(ntTemp);
                        Console.WriteLine("Naked Twin Strategy executed this round");
                        Debug.WriteLine("Naked Twin Strategy executed this round.");
                        break;
                    case Percepts.FindingHiddenTwin:
                        hiddenTwinExclusionStrategy.AlgorithmInterface(htTemp);
                        Console.WriteLine("Hidden Twin Strategy executed this round");
                        Debug.WriteLine("Hidden Twin Strategy executed this round.");
                        break;
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
            if (singlePossibilityContext.ContextAlgorithm() == Percepts.AreSinglePossibilities)
            {
                bool sc = singlePossibilityContext.GetContext(out stTemp);
                return Percepts.AreSinglePossibilities;
            }
            if (onlyChoiceContext.ContextAlgorithm() == Percepts.AreOnlyChoice)
            {
                bool ot = onlyChoiceContext.GetContext(out ocTemp);
                return Percepts.AreOnlyChoice;
            }
            //if (subgroupExclusionContext.ContextAlgorithm() == Percepts.SubgroupExclusion) //bugs are mainly here. Think it might be discarding too many values? That or too broad definition. Might try without it. 
            //{
            //    bool sgt = subgroupExclusionContext.GetContext(out sgeTemp);
            //    return Percepts.SubgroupExclusion;
            //}
            if (nakedTwinExclusionContext.ContextAlgorithm() == Percepts.FindingNakedTwin) {
                bool ntt = nakedTwinExclusionContext.GetContext(out ntTemp);
                return Percepts.FindingNakedTwin;
            }
            if (hiddenTwinExclusionContext.ContextAlgorithm() == Percepts.FindingHiddenTwin)
            {
                bool hnht = hiddenTwinExclusionContext.GetContext(out htTemp);
                return Percepts.FindingHiddenTwin;
            }
            return Percepts.None;
        }

        public void CleanUp()
        {
            thisPercept = Percepts.None;
            //sgeTemp.Clear();
            stTemp.Clear();
            htTemp.Clear();
            ntTemp.Clear();
            ocTemp.Clear();
        }
    }
}
