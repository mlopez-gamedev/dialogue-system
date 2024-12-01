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
        private readonly string _wrongTextColor;
        private readonly string _errorColor;

        public HighlightTitleParser(IHighlightTitleCommandFactory highlightCommandFactory, HighlightStyle style)
        {
            _highlightCommandFactory = highlightCommandFactory;
            _startWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.TitleStartColor);
            _titleColor = "#" + ColorUtility.ToHtmlStringRGB(style.TitleColor);
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
            for (int i = 1; i < lines.Length; ++i)
            {
                highlightedCommand += $"\n<color={_wrongTextColor}><i>{Regex.Unescape(lines[i])}</i></color> <color={_errorColor}>(this will be ignored)</color>";
            }
            return highlightedCommand;
        }
    }

}
