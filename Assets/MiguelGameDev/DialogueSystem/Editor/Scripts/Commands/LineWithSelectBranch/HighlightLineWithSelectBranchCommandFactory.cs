using MiguelGameDev.DialogueSystem.Commands;
using System.Text;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightLineWithSelectBranchCommandFactory : IHighlightLineWithSelectBranchCommandFactory
    {
        private readonly StringBuilder _stringBuilder;

        public HighlightLineWithSelectBranchCommandFactory(StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;
        }

        public IDialogueCommand CreateHighlightCommand(string line, HighlightSelectBranch[] branchSelectors)
        {
            return new HighlightLineWithSelectBranchCommand(_stringBuilder, line, branchSelectors);
        }
    }
}
