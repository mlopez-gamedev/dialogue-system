using MiguelGameDev.DialogueSystem.Parser.Command;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class RandomBranchCommandFactory : IRandomBranchCommandFactory
    {
        public IDialogueCommand CreateRandomBranchCommand(RandomBranchInfo[] selectBranchInfos)
        {
            return new RandomBranchCommand(selectBranchInfos);
        }
    }
}