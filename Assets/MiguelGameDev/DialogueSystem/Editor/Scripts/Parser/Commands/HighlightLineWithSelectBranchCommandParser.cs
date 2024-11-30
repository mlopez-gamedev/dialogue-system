﻿using MiguelGameDev.DialogueSystem.Commands;
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
        public const string SelectionSplitter = "*";
        public const string BranchSplitter = "\t";
        private readonly char[] MessageTrim = new char[] { ' ', '\n' };

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

            var lineAndBranches = lineCommand.Split(GetSelectionSplitter(commandPath.Level), System.StringSplitOptions.RemoveEmptyEntries);

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
                var splits = lineAndBranches[i].Split(GetBranchSplitter(commandPath.Level));

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
                    highlightedLine += $" <color={_metadataSeparatorColor}>[</color><color={_metadataColor}>{metadata}</color><color={_metadataSeparatorColor}>]</color>"; 
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
                highlightedLine += $" <color={_metadataSeparatorColor}>[</color><color={_metadataColor}>{metadata}</color><color={_metadataSeparatorColor}>]</color>"; 
            }

            highlightedCommand += $"<color={_authorColor}>{author}</color><color={_authorSeparatorColor}>:</color> {highlightedLine}";

            return highlightedCommand;
        }

        private string HighlightSelector(string selectionCommand, int level)
        {
            string highlightedCommand = $"<b><color={_selectionLineStartWithColor}>{GetSelectionSplitter(level)}</color></b>";

            var (message, metadata) = SplitMessageAndMetadata(selectionCommand);
            var highlightedLine =  $"<color={_selectionLineTextColor}>{message}</color>";;
            if (!string.IsNullOrEmpty(metadata))
            {
                highlightedLine += $" <color={_metadataSeparatorColor}>[</color><color={_metadataColor}>{metadata}</color><color={_metadataSeparatorColor}>]</color>"; 
            }
            
            highlightedCommand += highlightedLine;

            return highlightedCommand;
        }

        private string GetSelectionSplitter(int level)
        {
            var selectionbSplitter = "\n";
            for (int i = 0; i < level; i++)
            {
                selectionbSplitter += BranchSplitter;
            }
            return selectionbSplitter + SelectionSplitter;
        }

        private string GetBranchSplitter(int level)
        {
            var branchSplitter = "\n" + BranchSplitter;
            for (int i = 0; i < level; i++)
            {
                branchSplitter += BranchSplitter;
            }
            return branchSplitter;
        }

        private string GetBranchStarts(int level)
        {
            var branchSplitter = "";
            for (int i = 0; i < level; i++)
            {
                branchSplitter += BranchSplitter;
            }
            return branchSplitter;
        }
    }

}
