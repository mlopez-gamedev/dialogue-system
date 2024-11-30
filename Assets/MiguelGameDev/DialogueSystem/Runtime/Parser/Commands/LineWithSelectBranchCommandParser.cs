using MiguelGameDev.DialogueSystem.Commands;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class LineWithSelectBranchCommandParser : CommandParser
    {
        public override string StartsWith => "- ";
        public const string AuthorSeparatorPattern = @"(?<!\\): ";
        public const string SelectionSplitter = "*";
        public const string BranchSplitter = "\t";
        private readonly char[] MessageTrim = new char[] { ' ', '\n' };

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
            string message, metadata;
            var match = Regex.Match(lineCommand, AuthorSeparatorPattern);

            if (!match.Success)
            {
                (message, metadata) = SplitMessageAndMetadata(lineCommand);
                return new Line(message, metadata);
            }


            var author = Regex.Unescape(lineCommand.Substring(0, match.Index));
            var line = Regex.Unescape(lineCommand.Substring(match.Index + match.Length).Trim(MessageTrim));
            (message, metadata) = SplitMessageAndMetadata(line);

            return new Line(author, message, metadata);
        }

        private SelectBranchInfo CreateSelectBranchInfo(CommandPath commandPath, int branchIndex, string lineCommand)
        {
            var splits = lineCommand.Split(GetBranchSplitter(commandPath.Level));
            var branchPosition = new BranchPosition(commandPath.CommandIndex, branchIndex);

            var (message, metadata) = SplitMessageAndMetadata(splits[0]);
            
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
    }
}