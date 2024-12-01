
namespace MiguelGameDev.DialogueSystem.Editor
{
    public readonly struct HighlightSelectBranch
    {
        public string SelectHighlightedText { get; }
        public IBranch Branch { get; }

        public HighlightSelectBranch(string selectHighlightedText, IBranch branch)
        {
            SelectHighlightedText = selectHighlightedText;
            Branch = branch;
        }
    }
}
