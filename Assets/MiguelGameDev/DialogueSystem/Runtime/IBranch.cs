namespace MiguelGameDev.DialogueSystem
{
    public interface IBranch
    {
        bool IsMain { get; }
        IBranch Parent { get; }
        BranchPosition[] Path { get; }
        void CreateBranches();
        void Setup(IDialogue dialogue, IBranch parent = null);
        void Start();
        void Next();
        void GoTo(int index);
        bool TrySelectBranch(int branchIndex, out IBranch branch);
        IBranch GoToBranchAt(BranchPosition position);
        void RegisterBranch(BranchPosition position, IBranch branch);
    }
}
