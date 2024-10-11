using System;
using System.Linq;
using System.Reflection;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class InvokeMethodCommandFactory : IInvokeMethodCommandFactory
    {
        private readonly DialogueContext _dialogueContext;
        private readonly Type _dialogueContextType;

        private MethodInfo _methodInfo;

        public InvokeMethodCommandFactory(DialogueContext dialogueContext)
        {
            _dialogueContext = dialogueContext;
            _dialogueContextType = _dialogueContext.GetType();
        }

        public IDialogueCommand CreateInvokeMethodCommand(string methodName, params string[] parameterNames)
        {
            _methodInfo = _dialogueContextType.GetMethod(methodName);

            var parameters = new object[parameterNames.Length];
            var methodParameters = _methodInfo.GetParameters();

            for (int i = 0; i < parameterNames.Length; ++i)
            {
                parameters[i] = Convert.ChangeType(parameterNames[i].Trim(), methodParameters[i].ParameterType);
            }

            return new InvokeMethodCommand(_dialogueContext, _methodInfo, parameters);
        }
    }

}
