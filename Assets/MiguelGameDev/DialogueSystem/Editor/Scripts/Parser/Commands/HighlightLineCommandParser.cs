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
        public const string LinePattern = @"^(?:(?<author>[^:]+):\s)?(?<message>.*?)(?:\s\[(?<metadata>.+)\])?$";
        private readonly IHighlightCommandFactory _highlightCommandFactory;
        
        private readonly string _startWithColor;
        private readonly string _authorColor;
        private readonly string _authorSeparatorColor;
        private readonly string _metadataColor;
        private readonly string _metadataSeparatorColor;

        public HighlightLineCommandParser(IHighlightCommandFactory highlightCommandFactory, HighlightStyle style)
        {
            _highlightCommandFactory = highlightCommandFactory;
            _startWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.LineStartColor);
            _authorColor = "#" + ColorUtility.ToHtmlStringRGB(style.AuthorColor);
            _authorSeparatorColor = "#" + ColorUtility.ToHtmlStringRGB(style.AuthorSeparatorColor);
            _metadataColor = "#" + ColorUtility.ToHtmlStringRGB(style.MetadataColor);
            _metadataSeparatorColor = "#" + ColorUtility.ToHtmlStringRGB(style.MetadataSeparatorColor);
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
            string newLine = string.Empty;
            if (lineCommand.EndsWith("\n"))
            {
                newLine = "\n";
            }

            string highlightedCommand = $"<b><color={_startWithColor}>{StartsWith}</color></b>";
            lineCommand = lineCommand.Substring(StartsWith.Length);

            var match = Regex.Match(lineCommand, LinePattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                highlightedCommand += lineCommand;
                return highlightedCommand;
            }

            string author = Regex.Unescape(match.Groups["author"].Value);
            string message = Regex.Unescape(match.Groups["message"].Value);
            string metadata = Regex.Unescape(match.Groups["metadata"].Value);

            if (!string.IsNullOrEmpty(author))
            {
                highlightedCommand += $"<color={_authorColor}>{author}</color><color={_authorSeparatorColor}>:</color> ";
            }

            highlightedCommand += message;

            if (!string.IsNullOrEmpty(metadata))
            {
                highlightedCommand += $" <b><color={_metadataSeparatorColor}>[</color></b><color={_metadataColor}>{metadata}</color><b><color={_metadataSeparatorColor}>]</color></b>";
            }

            return highlightedCommand + newLine;
        }
    }

}
