using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightNotFoundParser : CommandParser
    {
        public override string StartsWith => string.Empty;
        private readonly IHighlightCommandFactory _highlightCommandFactory;

        private readonly string _wrongTextColor;
        private readonly string _errorColor;
        
        public HighlightNotFoundParser(IHighlightCommandFactory highlightCommandFactory, HighlightStyle style)
        {
            _highlightCommandFactory = highlightCommandFactory;
            _wrongTextColor = "#" + ColorUtility.ToHtmlStringRGB(style.WrongTextColor);
            _errorColor = "#" + ColorUtility.ToHtmlStringRGB(style.ErrorColor);
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (lineCommand.Length == 0) {
                command = _highlightCommandFactory.CreateHighlightCommand(string.Empty);
                return true;
            }
            
            if (NeedsCutFirstCharacter())
            {
                lineCommand = lineCommand.Substring(1);
            }

            if (string.IsNullOrWhiteSpace(lineCommand))
            {
                command = _highlightCommandFactory.CreateHighlightCommand(lineCommand);
                return true;
            }

            lineCommand = $"<color={_wrongTextColor}>{lineCommand}</color>";
            
            var highlightedText = lineCommand;

            command = _highlightCommandFactory.CreateHighlightCommand(highlightedText);
            return true;

            bool NeedsCutFirstCharacter()
            {
                if (commandPath.Level == 0 && commandPath.CommandIndex == 0)
                {
                    return true;
                }

                // if (lineCommand[0] == '\n')
                // {
                //     return true;
                // }

                return false;
            }
        }
    }
}
