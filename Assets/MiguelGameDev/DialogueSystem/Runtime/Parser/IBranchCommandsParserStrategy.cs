using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Parser
{
    public interface IBranchCommandsParserStrategy
    {
        public IDialogueCommand[] ParseCommands(string text, int tabs = 0, params BranchPosition[] path);
    }
}
