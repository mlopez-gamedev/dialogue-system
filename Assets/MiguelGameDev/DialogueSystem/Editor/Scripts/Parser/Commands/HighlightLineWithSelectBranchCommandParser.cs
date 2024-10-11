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
        public const string AuthorSeparatorPattern = @"(?<!\\): ";
        public const string SelectionSplitter = "\n* ";
        public const string BranchSplitter = "\n\t";
        private readonly char[] MessageTrim = new char[] { ' ', '\n' };

        private HighlightBranchParser _branchParser;
        private readonly IHighlightLineWithSelectBranchCommandFactory _highlightSelectLineCommandFactory;

        private readonly string _startWithColor;
        private readonly string _authorColor;
        private readonly string _authorSeparatorColor;
        private readonly string _selectionLineStartWithColor;
        private readonly string _selectionLineTextColor;

        public HighlightLineWithSelectBranchCommandParser(IHighlightLineWithSelectBranchCommandFactory highlightSelectLineCommandFactory, HighlightBranchParser branchParser, HighlightStyle style)
        {
            _highlightSelectLineCommandFactory = highlightSelectLineCommandFactory;
            _branchParser = branchParser;

            _startWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.LineStartColor);
            _authorColor = "#" + ColorUtility.ToHtmlStringRGB(style.AuthorColor);
            _authorSeparatorColor = "#" + ColorUtility.ToHtmlStringRGB(style.AuthorSeparatorColor);
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

            var lineAndBranches = lineCommand.Split(SelectionSplitter, System.StringSplitOptions.RemoveEmptyEntries);

            if (lineAndBranches.Length < 2)
            {
                command = null;
                return false;
            }

            var highlightedLine = string.Empty.PadRight(commandPath.Level, '\t');
            highlightedLine = HighlightLine(lineAndBranches[0]);

            var branchSelectors = new HighlightSelectBranch[lineAndBranches.Length - 1];
            for (int i = 1; i < lineAndBranches.Length; ++i)
            {
                var splits = lineAndBranches[i].Split(BranchSplitter);

                var highlightedSelector = string.Empty.PadRight(commandPath.Level, '\t');
                highlightedSelector += HighlightSelector(splits[0]);
                
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

        private string HighlightSelector(string selectionCommand)
        {
            string highlightedCommand = $"<b><color={_selectionLineStartWithColor}>{SelectionSplitter}</color></b>";

            highlightedCommand += $"<color={_selectionLineTextColor}>{selectionCommand}</color>";

            return highlightedCommand;
        }
    }

}
