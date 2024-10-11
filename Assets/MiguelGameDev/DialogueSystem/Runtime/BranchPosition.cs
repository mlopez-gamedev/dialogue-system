using System;

namespace MiguelGameDev.DialogueSystem
{
    public readonly struct BranchPosition : IEquatable<BranchPosition>
    {
        public int CommandIndex { get; }
        public int BranchIndex { get; }
        public BranchPosition(int commandIndex, int branchIndex)
        {
            CommandIndex = commandIndex;
            BranchIndex = branchIndex;
        }

        public bool Equals(BranchPosition other)
        {
            return CommandIndex.Equals(other.CommandIndex) && BranchIndex.Equals(other.BranchIndex);
        }
    }

}
