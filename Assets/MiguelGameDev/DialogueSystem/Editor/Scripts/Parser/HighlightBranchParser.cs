using MiguelGameDev.DialogueSystem.Parser;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightBranchParser
    {
        private IBranchCommandsParserStrategy _commandsParserStrategy;

        public void Setup(IBranchCommandsParserStrategy commandsParserStrategy)
        {
            _commandsParserStrategy = commandsParserStrategy;
        }

        public IBranch Parse(string text, int index = 0, int tabs = 0, params BranchPosition[] path)
        {
            var commands = _commandsParserStrategy.ParseCommands(text, tabs, path);
            return new HighlightBranch(commands, index, path);
        }
    }

}
