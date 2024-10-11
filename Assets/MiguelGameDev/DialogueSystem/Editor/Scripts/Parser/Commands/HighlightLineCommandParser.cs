using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{

    public class HighlightLineCommandParser : CommandParser
    {
        public override string StartsWith => "- ";
        public const string AuthorSeparatorPattern = @"(?<!\\): ";
        private readonly IHighlightCommandFactory _highlightCommandFactory;
        
        private readonly string _startWithColor;
        private readonly string _authorColor;
        private readonly string _authorSeparatorColor;

        public HighlightLineCommandParser(IHighlightCommandFactory highlightCommandFactory, HighlightStyle style)
        {
            _highlightCommandFactory = highlightCommandFactory;
            _startWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.LineStartColor);
            _authorColor = "#" + ColorUtility.ToHtmlStringRGB(style.AuthorColor);
            _authorSeparatorColor = "#" + ColorUtility.ToHtmlStringRGB(style.AuthorSeparatorColor);
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }

            var highlightedText = string.Empty.PadRight(commandPath.Level, '\t');
            highlightedText += HighlightText(lineCommand);
            command = _highlightCommandFactory.CreateHighlightCommand(highlightedText);
            return true;
        }

        private string HighlightText(string lineCommand)
        {
            string highlightedCommand = $"<b><color={_startWithColor}>{StartsWith}</color></b>";
            lineCommand = lineCommand.Substring(StartsWith.Length);
            
            var match = Regex.Match(lineCommand, AuthorSeparatorPattern);

            if (!match.Success)
            {

                highlightedCommand += Regex.Unescape(lineCommand);
                return highlightedCommand;
            }


            var author = Regex.Unescape(lineCommand.Substring(0, match.Index));
            var message = Regex.Unescape(lineCommand.Substring(match.Index + match.Length));


            highlightedCommand += $"<color={_authorColor}>{author}</color><color={_authorSeparatorColor}>:</color> {message}";

            return highlightedCommand;
        }
    }

}
