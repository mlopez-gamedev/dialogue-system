using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class NotFoundHighlightParser : CommandParser
    {
        public override string StartsWith => string.Empty;
        private readonly IHighlightCommandFactory _highlightCommandFactory;

        private readonly string _wrongTextColor;
        private readonly string _errorColor;
        
        public NotFoundHighlightParser(IHighlightCommandFactory highlightCommandFactory, HighlightStyle style)
        {
            _highlightCommandFactory = highlightCommandFactory;
            _wrongTextColor = "#" + ColorUtility.ToHtmlStringRGB(style.WrongTextColor);
            _errorColor = "#" + ColorUtility.ToHtmlStringRGB(style.ErrorColor);
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            lineCommand = $"<color={_wrongTextColor}><i>{lineCommand.TrimStart('\n')}</i></color> <color={_errorColor}>(this will be ignored)</color>";
            
            var highlightedText = Regex.Unescape(lineCommand);

            command = _highlightCommandFactory.CreateHighlightCommand(highlightedText);
            return true;
        }
    }

}
