using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class CommentParser : CommandParser
    {
        public override string StartsWith => "//";

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            command = null;
            return lineCommand.StartsWith(StartsWith);
        }
    }
}