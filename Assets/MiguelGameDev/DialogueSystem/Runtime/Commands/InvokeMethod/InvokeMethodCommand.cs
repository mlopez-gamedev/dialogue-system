using System.Reflection;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class InvokeMethodCommand : IDialogueCommand
    {
        private readonly DialogueContext _instance;
        private readonly MethodInfo _method;
        private readonly object[] _parameters;

        public InvokeMethodCommand(DialogueContext instance, MethodInfo method, params object[] parameters)
        {
            _instance = instance;
            _method = method;
            _parameters = parameters;
        }

        public void CreateBranches(IBranch ___)
        {
            // Nothing to do here
        }

        public void Setup(IDialogue __, IBranch ___)
        {
            // Nothing to do here
        }

        public void Execute()
        {
            _method.Invoke(_instance, _parameters);
        }
    }
}