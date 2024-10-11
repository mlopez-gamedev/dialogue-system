using MiguelGameDev.DialogueSystem.Commands;
using System.Text.RegularExpressions;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class TitleCommandParser : CommandParser
    {
        public override string StartsWith => "~ ";

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }

            command = new TitleCommand(CreateTitle(lineCommand), commandPath);
            return true;
        }

        private string CreateTitle(string lineCommand)
        {
            lineCommand = lineCommand.Substring(StartsWith.Length);
            var lines = lineCommand.Split("\n");
            return Regex.Unescape(lines[0]).Trim();

        }
    }

}
