using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{

    public class NotFoundParser : CommandParser
    {
        public override string StartsWith => null;

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            throw new System.ArgumentException("lineCommand", $"Not found parser for line '{lineCommand}'");
        }
    }

}
