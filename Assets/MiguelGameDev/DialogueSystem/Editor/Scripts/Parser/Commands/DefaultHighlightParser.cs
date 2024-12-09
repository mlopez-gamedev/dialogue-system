using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{

    public class DefaultHighlightParser : CommandParser
    {
        public override string StartsWith => string.Empty;
        private readonly IHighlightCommandFactory _highlightCommandFactory;
        
        private readonly string _wrongTextColor;
        private readonly string _errorColor;

        public DefaultHighlightParser(IHighlightCommandFactory highlightCommandFactory, HighlightStyle style)
        {
            _highlightCommandFactory = highlightCommandFactory;
            _wrongTextColor = "#" + ColorUtility.ToHtmlStringRGB(style.WrongTextColor);
            _errorColor = "#" + ColorUtility.ToHtmlStringRGB(style.ErrorColor);
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (lineCommand.Length > 0 && lineCommand[0] == '\n')
            {
                command = null;
                return false;
            }
            
            var highlightedText = GetBranchStarts(commandPath.Level);
            lineCommand = $"<color={_wrongTextColor}>{lineCommand}</color> <i><color={_errorColor}>(this will be ignored)</color></i>";
            highlightedText += Regex.Unescape(lineCommand);

            command = _highlightCommandFactory.CreateHighlightCommand(highlightedText);
            return true;
        }
    }

}
