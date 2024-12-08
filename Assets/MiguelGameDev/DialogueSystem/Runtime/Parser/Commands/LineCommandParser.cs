using MiguelGameDev.DialogueSystem.Commands;
using System.Text.RegularExpressions;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{

    public class LineCommandParser : CommandParser
    {
        public override string StartsWith => "- ";
        public const string LinePattern = @"^(?:(?<author>[^:]+):\s)?(?<message>.*?)(?:\s\[(?<metadata>.+)\])?$";
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
            var match = Regex.Match(lineCommand, LinePattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                return new Line(lineCommand, string.Empty);
            }

            string author = Regex.Unescape(match.Groups["author"].Value);
            string message = Regex.Unescape(match.Groups["message"].Value);
            string metadata = Regex.Unescape(match.Groups["metadata"].Value);

            return new Line(author, message, metadata);
        }
    }
}
