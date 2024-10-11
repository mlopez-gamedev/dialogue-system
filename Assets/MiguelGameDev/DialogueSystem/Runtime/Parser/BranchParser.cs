namespace MiguelGameDev.DialogueSystem.Parser
{
    public class BranchParser
    {
        private IBranchCommandsParserStrategy _commandsParserStrategy;

        public void Setup(IBranchCommandsParserStrategy commandsParserStrategy)
        {
            _commandsParserStrategy = commandsParserStrategy;
        }

        public IBranch Parse(string text, int index = 0, int tabs = 0, params BranchPosition[] branchPath)
        {
            var commands = _commandsParserStrategy.ParseCommands(text, tabs, branchPath);
            return new Branch(commands, index, branchPath);
        }
    }
}
