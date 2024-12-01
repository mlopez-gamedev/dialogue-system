using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public interface IHighlightRandomBranchCommandFactory
    {
        IDialogueCommand CreateHighlightCommand(string chances, HighlightSelectBranch[] branchSelectors);
    }
}