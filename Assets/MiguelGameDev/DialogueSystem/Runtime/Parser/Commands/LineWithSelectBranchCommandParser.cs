using MiguelGameDev.DialogueSystem.Commands;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class LineWithSelectBranchCommandParser : CommandParser
    {
        public override string StartsWith => "- ";
        public const string LinePattern = @"^(?:(?<author>[^:]+):\s)?(?<message>.*?)(?:\s\[(?<metadata>.+)\])?$";
        public const string OptionPattern = @"^(?<message>.*?)(?:\s\[(?<metadata>.+)\])?$";
        public const string SelectionSplitter = "*";

        private readonly ILineWithSelectBranchCommandFactory _lineWithSelectBranchCommandFactory;
        private readonly BranchParser _branchParser;

        public LineWithSelectBranchCommandParser(ILineWithSelectBranchCommandFactory lineCommandFactory, BranchParser branchParser)
        {
            _lineWithSelectBranchCommandFactory = lineCommandFactory;
            _branchParser = branchParser;
        }

        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }

            var lineAndBranches = lineCommand.Split(GetSelectionSplitter(commandPath.Level), System.StringSplitOptions.RemoveEmptyEntries);

            if (lineAndBranches.Length <= 1)
            {
                command = null;
                return false;
            }

            var line = CreateLine(lineAndBranches[0].Substring(StartsWith.Length));

            var selectBranchInfos = new SelectBranchInfo[lineAndBranches.Length - 1];
            for (int i = 1; i < lineAndBranches.Length; ++i)
            {
                var branchIndex = i - 1;
                selectBranchInfos[branchIndex] = CreateSelectBranchInfo(commandPath, branchIndex, lineAndBranches[i]);
            }

            command = _lineWithSelectBranchCommandFactory.CreateLineWithSelectBranchCommand(line, selectBranchInfos);
            return true;
        }

        private Line CreateLine(string lineCommand)
        {
            var match = Regex.Match(lineCommand, LinePattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                return new Line(lineCommand, string.Empty);
            }

            string author = Regex.Unescape(match.Groups["author"].Value);
            string message = Regex.Unescape(match.Groups["message"].Value);
            string metadata = Regex.Unescape(match.Groups["metadata"].Value);

            return new Line(author, message, metadata);
        }

        private SelectBranchInfo CreateSelectBranchInfo(CommandPath commandPath, int branchIndex, string lineCommand)
        {
            var splits = lineCommand.Split(GetBranchSplitter(commandPath.Level));
            var branchPosition = new BranchPosition(commandPath.CommandIndex, branchIndex);

            var match = Regex.Match(splits[0], OptionPattern, RegexOptions.Singleline);

            string message, metadata;
            if (!match.Success)
            {
                message = splits[0];
                metadata = string.Empty;
            }
            else
            {
                message = Regex.Unescape(match.Groups["message"].Value);
                metadata = Regex.Unescape(match.Groups["metadata"].Value);
            }

            if (splits.Length > 1)
            {
                var branchText = lineCommand.Substring(splits[0].Length);
                var branch = _branchParser.Parse(branchText, commandPath.CommandIndex, commandPath.Level + 1, commandPath.BranchPositions.Append(branchPosition).ToArray());

                return new SelectBranchInfo(message, metadata, branchPosition, branch);
            }

            return new SelectBranchInfo(message, metadata, branchPosition);
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