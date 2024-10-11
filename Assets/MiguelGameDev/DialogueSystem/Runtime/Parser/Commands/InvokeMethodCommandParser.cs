using MiguelGameDev.DialogueSystem.Commands;
using System.Text.RegularExpressions;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class InvokeMethodCommandParser : CommandParser
    {
        private readonly IInvokeMethodCommandFactory _invokeMethodCommandFactory;

        public override string StartsWith => "do ";

        public InvokeMethodCommandParser(IInvokeMethodCommandFactory invokeMethodCommandFactory)
        {
            _invokeMethodCommandFactory = invokeMethodCommandFactory;
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }


            command = CreateInvokeMethod(lineCommand);
            return true;
        }

        private IDialogueCommand CreateInvokeMethod(string lineCommand)
        {
            lineCommand = lineCommand.Substring(StartsWith.Length);
            var lines = lineCommand.Split("\n");
            lineCommand = Regex.Unescape(lines[0]).Trim();

            int paramsStartIndex = lineCommand.IndexOf("(");

            string methodName = lineCommand.Substring(0, paramsStartIndex);
            string parameterValuesString = lineCommand.Substring(paramsStartIndex + 1, lineCommand.Length - lineCommand.IndexOf("(") - 2);

            var parameterValues = parameterValuesString.Split(',');

            return _invokeMethodCommandFactory.CreateInvokeMethodCommand(methodName, parameterValues);
        }
    }
}
