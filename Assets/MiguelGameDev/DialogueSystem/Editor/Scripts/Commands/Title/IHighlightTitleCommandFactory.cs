using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public interface IHighlightTitleCommandFactory
    {
        IDialogueCommand CreateHighlightCommand(string title, CommandPath commandPath, string text);
    }
}
