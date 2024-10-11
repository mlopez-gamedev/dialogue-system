using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightTitleParser : CommandParser
    {
        public override string StartsWith => "~ ";
        private readonly IHighlightTitleCommandFactory _highlightCommandFactory;

        private readonly string _startWithColor;
        private readonly string _titleColor;

        public HighlightTitleParser(IHighlightTitleCommandFactory highlightCommandFactory, HighlightStyle style)
        {
            _highlightCommandFactory = highlightCommandFactory;
            _startWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.TitleStartColor);
            _titleColor = "#" + ColorUtility.ToHtmlStringRGB(style.TitleColor);
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }

            var highlightedText = string.Empty.PadRight(commandPath.Level, '\t');
            highlightedText += HighlightText(lineCommand, out var title);

            command = _highlightCommandFactory.CreateHighlightCommand(title, commandPath, highlightedText);
            return true;
        }

        private string HighlightText(string lineCommand, out string title)
        {
            string highlightedCommand = $"<b><color={_startWithColor}>{StartsWith}</color></b>";

            lineCommand = lineCommand.Substring(StartsWith.Length);

            var lines = lineCommand.Split("\n");
            title = Regex.Unescape(lines[0]);
            highlightedCommand += $"<color={_titleColor}>{title}</color>";

            return highlightedCommand;
        }
    }

}
