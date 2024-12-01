namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class RandomBranchInfo
    {
        public float Chances { get;}
        public bool ContinueInCurrentBranch { get; }
        public BranchPosition BranchPosition { get; }
        public IBranch Branch { get; }

        public RandomBranchInfo(float chances, BranchPosition branchPosition, IBranch branch)
        {
            Chances = chances;
            ContinueInCurrentBranch = false;
            BranchPosition = branchPosition;
            Branch = branch;
        }
        
        public RandomBranchInfo(float chances, BranchPosition branchPosition)
        {
            Chances = chances;
            ContinueInCurrentBranch = true;
            BranchPosition = branchPosition;
            Branch = null;
        }
    }
}