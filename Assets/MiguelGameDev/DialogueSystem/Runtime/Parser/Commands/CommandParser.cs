using System.Text.RegularExpressions;
using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public abstract class CommandParser
    {
        private const string MetadataPattern = @"^(.*?)\s*\[(.*?)\]$";
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

        protected (string, string) SplitMessageAndMetadata(string line)
        {
            var match = Regex.Match(line, MetadataPattern);
            if (!match.Success)
            {
                return (line, string.Empty);
            }
            
            return (match.Groups[1].Value, match.Groups[2].Value);
        }
    }
}
