using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Editor
{

    public interface IHighlightCommandFactory
    {
        IDialogueCommand CreateHighlightCommand(string text);
    }
}
