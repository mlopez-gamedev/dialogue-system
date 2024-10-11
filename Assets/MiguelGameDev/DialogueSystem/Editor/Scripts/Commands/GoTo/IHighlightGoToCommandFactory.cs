using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Editor
{

    public interface IHighlightGoToCommandFactory
    {
        IDialogueCommand CreateHighlightCommand(string title, string text);
    }
}
