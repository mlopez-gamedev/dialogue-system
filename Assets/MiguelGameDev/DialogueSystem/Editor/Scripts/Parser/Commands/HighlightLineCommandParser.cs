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

            var highlightedText = string.Empty.PadRight(commandPath.Level, '\t');
            highlightedText += HighlightText(lineCommand);
            command = _highlightCommandFactory.CreateHighlightCommand(highlightedText);
            return true;
        }

        private string HighlightText(string lineCommand)
        {
            string highlightedCommand = $"<b><color={_startWithColor}>{StartsWith}</color></b>";
            lineCommand = lineCommand.Substring(StartsWith.Length);
            
            string highlightedLine;
            string message, metadata;
            var match = Regex.Match(lineCommand, AuthorSeparatorPattern);

            if (!match.Success)
            {
                (message, metadata) = SplitMessageAndMetadata(lineCommand);
                highlightedLine = message;
                if (!string.IsNullOrEmpty(metadata))
                {
                    highlightedLine += $" <b><color={_metadataSeparatorColor}>[</color></b><color={_metadataColor}>{metadata}</color><b><color={_metadataSeparatorColor}>]</color></b>"; 
                }
                
                highlightedCommand += Regex.Unescape(highlightedLine);
                return highlightedCommand;
            }


            var author = Regex.Unescape(lineCommand.Substring(0, match.Index));
            var line = Regex.Unescape(lineCommand.Substring(match.Index + match.Length));
            (message, metadata) = SplitMessageAndMetadata(line);
            highlightedLine = message;
            if (!string.IsNullOrEmpty(metadata))
            {
                highlightedLine += $" <b><color={_metadataSeparatorColor}>[</color></b><color={_metadataColor}>{metadata}</color><b><color={_metadataSeparatorColor}>]</color></b>"; 
            }


            highlightedCommand += $"<color={_authorColor}>{author}></color><color={_authorSeparatorColor}>:</color> {highlightedLine}";

            return highlightedCommand;
        }
    }

}
