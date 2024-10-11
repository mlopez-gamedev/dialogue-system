using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public interface IHighlightLineWithSelectBranchCommandFactory
    {
        IDialogueCommand CreateHighlightCommand(string line, HighlightSelectBranch[] branchSelectors);
    }
}
