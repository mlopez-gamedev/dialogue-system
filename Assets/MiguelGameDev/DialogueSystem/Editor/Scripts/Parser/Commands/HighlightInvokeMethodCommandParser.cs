using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightInvokeMethodCommandParser : CommandParser
    {
        private readonly IHighlightCommandFactory _highlightCommandFactory;

        public override string StartsWith => "do ";
        //public const string MethodPattern = @"^(?<methodName>\w+)\((?<params>.*)\)$";
        //public const string ParamsPattern = @"\"".*?\""|[^,\(\)\s]+";
        public const string MethodPattern = @"(?<methodName>\w+)\(|(?<string>""(?:[^""\\]|\\.)*"")|(?<float>\d+\.\d+f?)|(?<int>\d+)|(?<bool>\btrue\b|\bfalse\b)|(?<separator>[,])|(?<paren>[()])|(?<whitespace>\s+)|(?<other>.)";
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
            StringBuilder highlightedCommandBuilder = new StringBuilder($"<b><color={_startWithColor}>{StartsWith}</color></b>");
            lineCommand = lineCommand.Substring(StartsWith.Length);

            highlightedCommandBuilder.Append($"<color={_invokeMethodColor}>");

            var matches = Regex.Matches(lineCommand, MethodPattern, RegexOptions.Singleline);

            bool isValid = false;
            foreach (Match match in matches)
            {
                // Si coincide con un grupo específico, le aplicamos color
                if (match.Groups["methodName"].Success)
                {
                    highlightedCommandBuilder.Append(Regex.Escape(match.Value));
                    isValid = true;
                }
                else if (match.Groups["string"].Success)
                {
                    highlightedCommandBuilder.Append($"<color={_stringParamColor}>{Regex.Escape(match.Value)}</color>");
                }
                else if (match.Groups["float"].Success)
                {
                    highlightedCommandBuilder.Append($"<color={_numericParamColor}>{Regex.Escape(match.Value)}</color>");
                }
                else if (match.Groups["int"].Success)
                {
                    highlightedCommandBuilder.Append($"<color={_numericParamColor}>{Regex.Escape(match.Value)}</color>");
                }
                else if (match.Groups["bool"].Success)
                {
                    highlightedCommandBuilder.Append($"<color={_booleanParamColor}>{Regex.Escape(match.Value)}</color>");
                }
                else if (match.Groups["other"].Success)
                {
                    highlightedCommandBuilder.Append($"<color={_wrongTextColor}>{Regex.Escape(match.Value)}</color>");
                }
                else if (match.Groups["paren"].Success || match.Groups["separator"].Success || match.Groups["whitespace"].Success)
                {
                    highlightedCommandBuilder.Append(Regex.Escape(match.Value));
                }
            }

            highlightedCommandBuilder.Append("</color>");

            if (!isValid)
            {
                highlightedCommandBuilder.Append($" <color={_errorColor}>(method is not defined)</color>");
            }

            return highlightedCommandBuilder.ToString();
        }
    }

}
