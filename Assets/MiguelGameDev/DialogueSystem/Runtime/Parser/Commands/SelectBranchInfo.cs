namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public readonly struct SelectBranchInfo
    {
        public string Text { get; }
        public bool ContinueInCurrentBranch { get; }
        public BranchPosition BranchPosition { get; }
        public IBranch Branch { get; }

        public SelectBranchInfo(string text, BranchPosition branchPosition, IBranch branch)
        {
            Text = text;
            ContinueInCurrentBranch = false;
            BranchPosition = branchPosition;
            Branch = branch;
        }

        public SelectBranchInfo(string text)
        {
            Text = text;
            ContinueInCurrentBranch = true;
            BranchPosition = default;
            Branch = null;
        }
    }
}
