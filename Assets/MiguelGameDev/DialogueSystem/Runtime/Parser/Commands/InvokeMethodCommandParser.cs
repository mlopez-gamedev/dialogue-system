using MiguelGameDev.DialogueSystem.Commands;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class InvokeMethodCommandParser : CommandParser
    {
        private readonly IInvokeMethodCommandFactory _invokeMethodCommandFactory;

        public override string StartsWith => "do ";
        public const string MethodPattern = @"^(?<methodName>\w+)\((?<params>.*)\)$";
        public const string ParamsPattern = @"\"".*?\""|[^,\(\)\s]+";

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
            var match = Regex.Match(lineCommand, MethodPattern, RegexOptions.Singleline);

            Debug.Assert(match.Success, $"Line command is not valid: {lineCommand}");

            string methodName = Regex.Escape(match.Groups["methodName"].Value);
            string parameterValuesString = match.Groups["params"].Value;

            // Dividimos los parámetros respetando las cadenas entre comillas
            var parameterValues = ParseParameters(parameterValuesString);

            return _invokeMethodCommandFactory.CreateInvokeMethodCommand(methodName, parameterValues);


            string[] ParseParameters(string parameterValuesString)
            {
                var matches = Regex.Matches(parameterValuesString, ParamsPattern, RegexOptions.Singleline);
                string[] parameters = new string[matches.Count];

                for (int i = 0; i < matches.Count; ++i)
                {
                    parameters[i] = Regex.Escape(matches[i].Value).Trim();
                }

                return parameters;
            }
        }
    }
}
