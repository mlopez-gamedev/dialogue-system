using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public abstract class CommandParser
    {
        public abstract string StartsWith { get; }
        private CommandParser _nextParser;
        internal CommandParser NextParser => _nextParser;

        public void SetNextParser(CommandParser nextParser)
        {
            _nextParser = nextParser;
        }

        public IDialogueCommand Parse(string lineCommand, CommandPath commandPath)
        {
            if (TryParse(lineCommand, commandPath, out var command))
            {
                return command;
            }

            return _nextParser?.Parse(lineCommand, commandPath);
        }

        protected abstract bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command);
    }

}
