using MiguelGameDev.DialogueSystem.Parser.Command;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public interface ILineWithSelectBranchCommandFactory
    {
        IDialogueCommand CreateLineWithSelectBranchCommand(Line line, SelectBranchInfo[] selectBranchInfos);
    }
}
