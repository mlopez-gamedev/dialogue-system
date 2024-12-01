using MiguelGameDev.DialogueSystem.Parser.Command;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public interface IRandomBranchCommandFactory
    {
        IDialogueCommand CreateRandomBranchCommand(RandomBranchInfo[] selectBranchInfos);
    }
}