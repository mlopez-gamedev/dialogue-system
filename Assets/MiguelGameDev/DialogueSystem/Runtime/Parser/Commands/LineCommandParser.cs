using MiguelGameDev.DialogueSystem.Commands;
using System.Text.RegularExpressions;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{

    public class LineCommandParser : CommandParser
    {
        public override string StartsWith => "- ";
        public const string AuthorSeparatorPattern = @"(?<!\\): ";
        private readonly char[] MessageTrim = new char[] { ' ', '\n' };

        private readonly ILineCommandFactory _lineCommandFactory;

        public LineCommandParser(ILineCommandFactory lineCommandFactory)
        {
            _lineCommandFactory = lineCommandFactory;
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }

            var line = CreateLine(lineCommand.Substring(StartsWith.Length));
            command = _lineCommandFactory.CreateLineCommand(line);
            return true;
        }

        private Line CreateLine(string lineCommand)
        {
            string message, metadata;
            var match = Regex.Match(lineCommand, AuthorSeparatorPattern);
            
            
            if (!match.Success)
            {
                (message, metadata, _) = SplitMessageAndMetadata(lineCommand);
                return new Line(message, metadata);
            }

            var author = Regex.Unescape(lineCommand.Substring(0, match.Index));
            var line = Regex.Unescape(lineCommand.Substring(match.Index + match.Length).Trim(MessageTrim));
            (message, metadata, _) = SplitMessageAndMetadata(line);

            return new Line(author, message, metadata);
        }
    }
}
