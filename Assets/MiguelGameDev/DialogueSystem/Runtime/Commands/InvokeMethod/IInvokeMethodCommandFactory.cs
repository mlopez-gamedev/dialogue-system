namespace MiguelGameDev.DialogueSystem.Commands
{
    public interface IInvokeMethodCommandFactory
    {
        IDialogueCommand CreateInvokeMethodCommand(string methodName, params string[] parameters);
    }

}
