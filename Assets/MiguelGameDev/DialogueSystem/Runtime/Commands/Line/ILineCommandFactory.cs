namespace MiguelGameDev.DialogueSystem.Commands
{
    public interface ILineCommandFactory
    {
        IDialogueCommand CreateLineCommand(Line line);
    }

}
