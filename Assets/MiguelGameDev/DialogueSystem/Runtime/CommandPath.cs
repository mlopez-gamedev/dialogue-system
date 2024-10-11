namespace MiguelGameDev.DialogueSystem
{
    public readonly struct CommandPath
    {
        public int CommandIndex { get; }
        public int Level { get; }
        public BranchPosition[] BranchPositions { get; }

        public CommandPath(int commandIndex, int level = 0, params BranchPosition[] branchPositions)
        {
            CommandIndex = commandIndex;
            Level = level;
            BranchPositions = branchPositions;
        }
    }

}
