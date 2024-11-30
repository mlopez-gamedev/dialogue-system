namespace MiguelGameDev.DialogueSystem
{
    public readonly struct SelectBranch
    {
        public string Message { get; }
        public string Metadata { get; }
        public bool HasMetadata => !string.IsNullOrEmpty(Metadata);
        public int BranchIndex { get; }

        public SelectBranch(string message, string metadata, int branchIndex)
        {
            Message = message;
            Metadata = metadata;
            BranchIndex = branchIndex;
        }
    }

}
