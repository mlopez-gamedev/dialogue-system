using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightInvokeMethodCommandParser : CommandParser
    {
        private readonly IHighlightCommandFactory _highlightCommandFactory;

        public override string StartsWith => "do ";
        public const string MethodPattern = @"^(?<methodName>\w+)\((?<params>.*)\)$";
        public const string ParamsPattern = @"(\"".*?\""|[^,\(\)\s]+)";

        private readonly string _startWithColor;
        private readonly string _invokeMethodColor;
        private readonly string _stringParamColor;
        private readonly string _numericParamColor;
        private readonly string _booleanParamColor;
        private readonly string _defaultParamColor;
        private readonly string _wrongTextColor;
        private readonly string _errorColor;

        public HighlightInvokeMethodCommandParser(IHighlightCommandFactory highlightCommandFactory, HighlightStyle style)
        {
            _highlightCommandFactory = highlightCommandFactory;
            _startWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.InvokeMethodStartColor);
            _invokeMethodColor = "#" + ColorUtility.ToHtmlStringRGB(style.InvokeMethodColor);
            _stringParamColor = "#" + ColorUtility.ToHtmlStringRGB(style.StringParamColor);
            _numericParamColor = "#" + ColorUtility.ToHtmlStringRGB(style.NumericParamColor);
            _booleanParamColor = "#" + ColorUtility.ToHtmlStringRGB(style.BooleanParamColor);
            _defaultParamColor = "#" + ColorUtility.ToHtmlStringRGB(style.DefaultParamColor);
            _wrongTextColor = "#" + ColorUtility.ToHtmlStringRGB(style.WrongTextColor);
            _errorColor = "#" + ColorUtility.ToHtmlStringRGB(style.ErrorColor);
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }

            var highlightedText = GetBranchStarts(commandPath.Level);
            highlightedText += HighlightText(lineCommand);
            command = _highlightCommandFactory.CreateHighlightCommand(highlightedText);
            return true;
        }

        private string HighlightText(string lineCommand)
        {
            string highlightedCommand = $"<b><color={_startWithColor}>{StartsWith}</color></b>";
            lineCommand = lineCommand.Substring(StartsWith.Length);

            var match = Regex.Match(lineCommand, MethodPattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                highlightedCommand += $"\n<color={_wrongTextColor}><i>{Regex.Unescape(lineCommand)}</i></color> <color={_errorColor}>(this will be ignored)</color>";
                return highlightedCommand;
            }

            string methodName = match.Groups["methodName"].Value;
            string parameterValuesString = match.Groups["params"].Value;

            var parameterValues = ParseParameters(parameterValuesString);

            string formatedParameters = "";
            foreach (var parameterValue in parameterValues)
            {
                var parameter = parameterValue.Trim();
                if (formatedParameters.Length > 0)
                {
                    formatedParameters += ", ";
                }

                if (parameter == "true" || parameter == "false")
                {
                    formatedParameters += $"<color={_booleanParamColor}>{parameter}</color>";
                }
                else if (parameter.Length >= 2 && parameter[0] == '"' && parameter[^1] == '"')
                {
                    formatedParameters += $"<color={_stringParamColor}>{parameter}</color>";
                }
                else if (double.TryParse(parameter, out var __))
                {
                    formatedParameters += $"<color={_numericParamColor}>{parameter}</color>";
                }
                else
                {
                    formatedParameters += $"<color={_defaultParamColor}>{parameterValue}</color>";
                }
            }

            highlightedCommand += $"<color={_invokeMethodColor}>{methodName}({formatedParameters})</color>";

            return highlightedCommand;


            string[] ParseParameters(string parameterValuesString)
            {
                List<string> parameters = new List<string>();

                // Expresión regular para separar parámetros
                //var matches = Regex.Matches(parameterValuesString, ParamsPattern);

                var matches = Regex.Matches(parameterValuesString, ParamsPattern, RegexOptions.Singleline);

                foreach (Match match in matches)
                {
                    if (match.Groups[1].Success) // Si es una cadena entre comillas
                        parameters.Add(match.Groups[1].Value);
                    else if (match.Groups[2].Success) // Otros tipos de parámetros
                        parameters.Add(match.Groups[2].Value.Trim());
                }

                return parameters.ToArray();
            }
        }
    }

}
