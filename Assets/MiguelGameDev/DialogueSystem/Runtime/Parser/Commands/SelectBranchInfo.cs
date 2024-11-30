namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public readonly struct SelectBranchInfo
    {
        public string Text { get; }
        public string Metadata { get; }
        public bool HasMetadata => !string.IsNullOrEmpty(Metadata);
        public bool ContinueInCurrentBranch { get; }
        public BranchPosition BranchPosition { get; }
        public IBranch Branch { get; } 

        public SelectBranchInfo(string text, string metadata, BranchPosition branchPosition, IBranch branch)
        {
            Text = text;
            Metadata = metadata;
            ContinueInCurrentBranch = false;
            BranchPosition = branchPosition;
            Branch = branch;
        }

        public SelectBranchInfo(string text, string metadata, BranchPosition branchPosition)
        {
            Text = text;
            Metadata = metadata;
            ContinueInCurrentBranch = true;
            BranchPosition = branchPosition;
            Branch = null;
        }
    }
}
