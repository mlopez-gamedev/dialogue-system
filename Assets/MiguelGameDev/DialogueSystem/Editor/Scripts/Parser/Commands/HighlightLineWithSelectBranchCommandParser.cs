using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightLineWithSelectBranchCommandParser : CommandParser
    {
        public override string StartsWith => "- ";
        public const string LinePattern = @"^(?:(?<author>[^:]+):\s)?(?<message>.*?)(?:\s\[(?<metadata>.+)\])?$";
        public const string OptionPattern = @"^(?<message>.*?)(?:\s\[(?<metadata>.+)\])?$";
        public const string SelectionSplitter = "*";

        private HighlightBranchParser _branchParser;
        private readonly IHighlightLineWithSelectBranchCommandFactory _highlightSelectLineCommandFactory;

        private readonly string _startWithColor;
        private readonly string _authorColor;
        private readonly string _authorSeparatorColor;
        private readonly string _metadataColor;
        private readonly string _metadataSeparatorColor;
        private readonly string _selectionLineStartWithColor;
        private readonly string _selectionLineTextColor;

        public HighlightLineWithSelectBranchCommandParser(IHighlightLineWithSelectBranchCommandFactory highlightSelectLineCommandFactory, HighlightBranchParser branchParser, HighlightStyle style)
        {
            _highlightSelectLineCommandFactory = highlightSelectLineCommandFactory;
            _branchParser = branchParser;

            _startWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.LineStartColor);
            _authorColor = "#" + ColorUtility.ToHtmlStringRGB(style.AuthorColor);
            _authorSeparatorColor = "#" + ColorUtility.ToHtmlStringRGB(style.AuthorSeparatorColor);
            _metadataColor = "#" + ColorUtility.ToHtmlStringRGB(style.MetadataColor);
            _metadataSeparatorColor = "#" + ColorUtility.ToHtmlStringRGB(style.MetadataSeparatorColor);
            _selectionLineStartWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.SelectLineStartColor);
            _selectionLineTextColor = "#" + ColorUtility.ToHtmlStringRGB(style.SelectLineColor);
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }

            var lineAndBranches = lineCommand.Split(GetSelectionSplitter(commandPath.Level));

            if (lineAndBranches.Length < 2)
            {
                command = null;
                return false;
            }

            var highlightedLine = GetBranchStarts(commandPath.Level);
            highlightedLine += HighlightLine(lineAndBranches[0]);

            var branchSelectors = new HighlightSelectBranch[lineAndBranches.Length - 1];
            for (int i = 1; i < lineAndBranches.Length; ++i)
            {
                var splits = lineAndBranches[i].Split(GetBranchSplitter(commandPath.Level + 1));
                //var highlightedSelector = GetBranchStarts(commandPath.Level);
                var highlightedSelector = HighlightSelector(splits[0], commandPath.Level);
                
                IBranch branch = null;
                if (splits.Length > 1)
                {
                    var branchPosition = new BranchPosition(commandPath.CommandIndex, i);
                    var branchText = lineAndBranches[i].Substring(splits[0].Length);
                    branch = _branchParser.Parse(branchText, commandPath.CommandIndex, commandPath.Level + 1, commandPath.BranchPositions.Append(branchPosition).ToArray());
                }

                branchSelectors[i - 1] = new HighlightSelectBranch(highlightedSelector, branch);
            }

            command = _highlightSelectLineCommandFactory.CreateHighlightCommand(highlightedLine, branchSelectors);
            return true;
        }

        private string HighlightLine(string lineCommand)
        {
            string newLine = string.Empty;
            if (lineCommand.EndsWith("\n"))
            {
                Debug.Log("Ends with \\n!");
                newLine = "\n";
            }

            string highlightedCommand = $"<color={_startWithColor}>{StartsWith}</color>";
            lineCommand = lineCommand.Substring(StartsWith.Length);

            var match = Regex.Match(lineCommand, LinePattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                highlightedCommand += lineCommand;
                return highlightedCommand + newLine;
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
                highlightedCommand += $" <color={_metadataSeparatorColor}>[</color><color={_metadataColor}>{metadata}</color><color={_metadataSeparatorColor}>]</color>";
            }

            return highlightedCommand + newLine;
        }

        private string HighlightSelector(string selectionCommand, int level)
        {
            string newLine = string.Empty;
            if (selectionCommand.EndsWith("\n"))
            {
                newLine = "\n";
            }

            string highlightedCommand = $"<color={_selectionLineStartWithColor}>{GetSelectionSplitter(level)}</color>";

            var match = Regex.Match(selectionCommand, OptionPattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                highlightedCommand += selectionCommand;
                return highlightedCommand + newLine;
            }

            string message = Regex.Unescape(match.Groups["message"].Value);
            string metadata = Regex.Unescape(match.Groups["metadata"].Value);

            if (!string.IsNullOrEmpty(metadata))
            {
                Debug.Log("match.Groups.Count => " + match.Groups.Count);
                Debug.Log(selectionCommand);
                for (var i = 1; i <= match.Groups.Count; ++i)
                {
                    Debug.Log(match.Groups[i]);
                }

            }

            highlightedCommand += $"<color={_selectionLineTextColor}>{message}</color>";

            if (!string.IsNullOrEmpty(metadata))
            {
                highlightedCommand += $" <color={_metadataSeparatorColor}>[</color><color={_metadataColor}>{metadata}</color><color={_metadataSeparatorColor}>]</color>";
            }

            return highlightedCommand + newLine;
        }

        private string GetSelectionSplitter(int level)
        {
            var selectionSplitter = "\n";
            for (int i = 0; i < level; i++)
            {
                selectionSplitter += BranchPrefix;
            }
            return selectionSplitter + SelectionSplitter;
        }
    }

}
