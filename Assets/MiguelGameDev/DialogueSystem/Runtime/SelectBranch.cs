namespace MiguelGameDev.DialogueSystem
{
    public readonly struct SelectBranch
    {
        public string Text { get; }
        public int BranchIndex { get; }

        public SelectBranch(string text, int branchIndex)
        {
            Text = text;
            BranchIndex = branchIndex;
        }
    }

}
