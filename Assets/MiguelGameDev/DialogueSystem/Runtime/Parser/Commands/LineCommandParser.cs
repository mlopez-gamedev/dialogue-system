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
            var match = Regex.Match(lineCommand, AuthorSeparatorPattern);

            if (!match.Success)
            {
                return new Line(lineCommand);
            }

            var author = Regex.Unescape(lineCommand.Substring(0, match.Index));
            var message = Regex.Unescape(lineCommand.Substring(match.Index + match.Length).Trim(MessageTrim));

            return new Line(author, message);
        }
    }
}
