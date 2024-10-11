using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Text.RegularExpressions;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class DefaultHighlightParser : CommandParser
    {
        public override string StartsWith => null;
        private readonly IHighlightCommandFactory _highlightCommandFactory;

        public DefaultHighlightParser(IHighlightCommandFactory highlightCommandFactory)
        {
            _highlightCommandFactory = highlightCommandFactory;
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            var highlightedText = string.Empty.PadRight(commandPath.Level, '\t');
            highlightedText += Regex.Unescape(lineCommand);

            command = _highlightCommandFactory.CreateHighlightCommand(highlightedText);
            return true;
        }
    }

}
