using System.Linq;
using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class RandomBranchCommandParser : CommandParser
    {
        private readonly IRandomBranchCommandFactory _randomBranchCommandFactory;
        private readonly BranchParser _branchParser;
        
        public override string StartsWith => "rand";
        private const string RandomSplitter = "%";
        
        public RandomBranchCommandParser(IRandomBranchCommandFactory lineCommandFactory, BranchParser branchParser)
        {
            _randomBranchCommandFactory = lineCommandFactory;
            _branchParser = branchParser;
        }
        
        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }

            lineCommand = lineCommand.Substring(StartsWith.Length);
            var branches = lineCommand.Split(GetRandomSplitter(commandPath.Level), System.StringSplitOptions.RemoveEmptyEntries);

            if (branches.Length == 0)
            {
                command = null;
                return false;
            }

            var selectBranchInfos = new RandomBranchInfo[branches.Length];
            for (int i = 0; i < branches.Length; ++i)
            {
                var branchIndex = i;
                selectBranchInfos[branchIndex] = CreateRandomBranchInfo(commandPath, branchIndex, branches[i]);
            }

            command = _randomBranchCommandFactory.CreateRandomBranchCommand(selectBranchInfos);
            return true;
        }
        
        private RandomBranchInfo CreateRandomBranchInfo(CommandPath commandPath, int branchIndex, string lineCommand)
        {
            var splits = lineCommand.Split(GetBranchSplitter(commandPath.Level));
            var branchPosition = new BranchPosition(commandPath.CommandIndex, branchIndex);

            float chances = 0f;
            if (!float.TryParse(splits[0], out chances))
            {
                return new RandomBranchInfo(chances, branchPosition);
            }
            
            if (splits.Length > 1)
            {
                var branchText = lineCommand.Substring(splits[0].Length);
                var branch = _branchParser.Parse(branchText, commandPath.CommandIndex, commandPath.Level + 1, commandPath.BranchPositions.Append(branchPosition).ToArray());
                
                return new RandomBranchInfo(chances, branchPosition, branch);
            }

            return new RandomBranchInfo(chances, branchPosition);
        }
        
        private string GetRandomSplitter(int level)
        {
            var selectionSplitter = "\n";
            for (int i = 0; i < level; i++)
            {
                selectionSplitter += BranchPrefix;
            }
            return selectionSplitter + RandomSplitter;
        }
    }
}